using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class HLSService : IHLSService, IDisposable
    {
        private readonly ConcurrentDictionary<string, StreamInfo> _streams;
        private readonly string _streamOutputPath;
        private readonly ILogger<HLSService> _logger;
        private bool _disposed;

        public HLSService(ILogger<HLSService> logger)
        {
            _logger = logger;
            _streams = new ConcurrentDictionary<string, StreamInfo>();
            var projectDir = Directory.GetCurrentDirectory();
            _streamOutputPath = Path.Combine(projectDir, "wwwroot", "streams");
            EnsureDirectoryExists(_streamOutputPath);
            _logger.LogInformation($"[Instance {GetHashCode()}] HLS Service initialized. Output path: {_streamOutputPath}");
        }

        public async Task<string> StartStreamAsync(string streamKey)
        {
            var instanceId = GetHashCode();
            _logger.LogInformation($"[Instance {instanceId}] Starting stream with key: {streamKey}");

            if (string.IsNullOrWhiteSpace(streamKey) || streamKey.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("Invalid stream key", nameof(streamKey));
            }

            try
            {
                var streamDirectory = Path.Combine(_streamOutputPath, streamKey);
                EnsureDirectoryExists(streamDirectory);
                _logger.LogInformation($"[Instance {instanceId}] Created stream directory: {streamDirectory}");

                var segmentPathTemplate = Path.Combine(streamDirectory, "segment_%03d.ts");
                var playlistPath = Path.Combine(streamDirectory, "playlist.m3u8");

                _logger.LogInformation($"[Instance {instanceId}] Setting up FFmpeg with paths: segments={segmentPathTemplate}, playlist={playlistPath}");

                var ffmpegArgs = $"-f webm -i pipe:0 " +
                                $"-c:v libvpx -b:v 1M -cpu-used 5 -deadline realtime " +
                                $"-c:a libopus -b:a 128k -ar 48000 " +
                                $"-f hls -hls_time 4 -hls_list_size 3 -hls_flags delete_segments " +
                                $"-hls_segment_filename {segmentPathTemplate} {playlistPath} " +
                                $"-loglevel debug";

                _logger.LogInformation($"[Instance {instanceId}] FFmpeg arguments: {ffmpegArgs}");

                var ffmpegProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = ffmpegArgs,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    },
                    EnableRaisingEvents = true
                };

                _logger.LogInformation($"[Instance {instanceId}] Starting FFmpeg process...");
                
                try 
                {
                    ffmpegProcess.Start();
                    _logger.LogInformation($"[Instance {instanceId}] FFmpeg process started successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[Instance {instanceId}] Failed to start FFmpeg process");
                    throw new InvalidOperationException("Failed to start FFmpeg process", ex);
                }

                ffmpegProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        _logger.LogInformation($"[Instance {instanceId}] FFmpeg Error: {e.Data}");
                    }
                };

                ffmpegProcess.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        _logger.LogInformation($"[Instance {instanceId}] FFmpeg Output: {e.Data}");
                    }
                };

                ffmpegProcess.BeginErrorReadLine();
                ffmpegProcess.BeginOutputReadLine();

                // Створюємо і додаємо StreamInfo
                var streamInfo = new StreamInfo
                {
                    StreamKey = streamKey,
                    Process = ffmpegProcess,
                    StartTime = DateTime.UtcNow,
                    Status = "active"
                };

                if (!_streams.TryAdd(streamKey, streamInfo))
                {
                    _logger.LogError($"[Instance {instanceId}] Failed to add stream {streamKey} to dictionary");
                    ffmpegProcess.Kill();
                    throw new InvalidOperationException($"Failed to add stream {streamKey} to active streams");
                }

                _logger.LogInformation($"[Instance {instanceId}] Stream {streamKey} added to active streams with status: {streamInfo.Status}");

                // Чекаємо трохи, щоб FFmpeg встиг ініціалізуватися
                await Task.Delay(2000);

                // Перевіряємо, чи процес все ще працює
                if (ffmpegProcess.HasExited)
                {
                    var exitCode = ffmpegProcess.ExitCode;
                    var error = await ffmpegProcess.StandardError.ReadToEndAsync();
                    _logger.LogError($"[Instance {instanceId}] FFmpeg process exited with code {exitCode}. Error: {error}");
                    _streams.TryRemove(streamKey, out _);
                    throw new InvalidOperationException($"FFmpeg process failed to start. Exit code: {exitCode}. Error: {error}");
                }

                return streamKey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Instance {instanceId}] Error starting stream {streamKey}");
                throw;
            }
        }

        public async Task StopStreamAsync(string streamKey)
        {
            _logger.LogInformation($"Stopping stream: {streamKey}");

            if (_streams.TryRemove(streamKey, out var streamInfo))
            {
                try
                {
                    if (!streamInfo.Process.HasExited)
                    {
                        streamInfo.Process.Kill();
                        _logger.LogInformation("FFmpeg process killed");
                    }

                    var streamDirectory = Path.Combine(_streamOutputPath, streamKey);
                    if (Directory.Exists(streamDirectory))
                    {
                        Directory.Delete(streamDirectory, true);
                        _logger.LogInformation($"Deleted stream directory: {streamDirectory}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error stopping stream");
                    throw;
                }
            }
        }

        public async Task<byte[]> GetSegmentAsync(string streamKey, string segmentName)
        {
            var segmentPath = Path.Combine(_streamOutputPath, streamKey, segmentName);
            if (File.Exists(segmentPath))
            {
                return await File.ReadAllBytesAsync(segmentPath);
            }
            return null;
        }

        public async Task<string> GetPlaylistAsync(string streamKey)
        {
            var playlistPath = Path.Combine(_streamOutputPath, streamKey, "playlist.m3u8");
            if (File.Exists(playlistPath))
            {
                return await File.ReadAllTextAsync(playlistPath);
            }
            return null;
        }

        public async Task<string> GetStreamStatusAsync(string streamKey)
        {
            var instanceId = GetHashCode();
            _logger.LogInformation($"[Instance {instanceId}] Checking status for stream {streamKey}");
            _logger.LogInformation($"[Instance {instanceId}] Active streams count: {_streams.Count}");
            _logger.LogInformation($"[Instance {instanceId}] Active stream keys: {string.Join(", ", _streams.Keys)}");

            if (_streams.TryGetValue(streamKey, out var streamInfo))
            {
                _logger.LogInformation($"[Instance {instanceId}] Found stream {streamKey} with status: {streamInfo.Status}");
                return streamInfo.Status;
            }

            _logger.LogWarning($"[Instance {instanceId}] Stream {streamKey} not found in active streams");
            return "inactive";
        }

        public async Task WriteStreamDataAsync(string streamKey, byte[] buffer, int offset, int count)
        {
            if (!_streams.TryGetValue(streamKey, out var streamInfo))
            {
                _logger.LogWarning($"Stream {streamKey} not found");
                return;
            }

            try
            {
                _logger.LogInformation($"Writing {count} bytes to stream {streamKey}");

                if (streamInfo.Process == null || streamInfo.Process.HasExited)
                {
                    _logger.LogError($"FFmpeg process for stream {streamKey} is not running");
                    return;
                }

                var stream = streamInfo.Process.StandardInput.BaseStream;

                if (stream == null)
                {
                    _logger.LogError($"StandardInput stream is null for {streamKey}");
                    return;
                }

                await stream.WriteAsync(buffer, offset, count);
                await stream.FlushAsync();

                _logger.LogInformation($"Successfully wrote {count} bytes to FFmpeg process {streamInfo.Process.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error writing stream data for {streamKey}");
                throw;
            }
        }

        public async Task ProcessSegmentAsync(string streamKey, byte[] segmentData, double duration, int sequence)
        {
            _logger.LogInformation($"Processing segment {sequence} for stream {streamKey}");

            var streamDirectory = Path.Combine(_streamOutputPath, streamKey);
            var segmentPath = Path.Combine(streamDirectory, $"segment_{sequence}.ts");
            var playlistPath = Path.Combine(streamDirectory, "playlist.m3u8");

            // Зберігаємо сегмент
            await File.WriteAllBytesAsync(segmentPath, segmentData);

            // Оновлюємо плейлист
            var playlistContent = new StringBuilder();
            playlistContent.AppendLine("#EXTM3U");
            playlistContent.AppendLine("#EXT-X-VERSION:3");
            playlistContent.AppendLine("#EXT-X-TARGETDURATION:" + Math.Ceiling(duration));
            playlistContent.AppendLine("#EXT-X-MEDIA-SEQUENCE:" + Math.Max(0, sequence - 3));

            // Додаємо останні 3 сегменти
            for (int i = Math.Max(0, sequence - 2); i <= sequence; i++)
            {
                var segmentFile = $"segment_{i}.ts";
                if (File.Exists(Path.Combine(streamDirectory, segmentFile)))
                {
                    playlistContent.AppendLine($"#EXTINF:{duration},");
                    playlistContent.AppendLine(segmentFile);
                }
            }

            // Зберігаємо плейлист
            await File.WriteAllTextAsync(playlistPath, playlistContent.ToString());

            // Видаляємо старі сегменти
            var oldSegments = Directory.GetFiles(streamDirectory, "segment_*.ts")
                .Where(f => int.Parse(Path.GetFileNameWithoutExtension(f).Split('_')[1]) < sequence - 3);

            foreach (var oldSegment in oldSegments)
            {
                try
                {
                    File.Delete(oldSegment);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting old segment {oldSegment}");
                }
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                _logger.LogInformation($"Creating directory: {path}");
                Directory.CreateDirectory(path);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var stream in _streams.Values)
                    {
                        if (!stream.Process.HasExited)
                        {
                            stream.Process.Kill();
                        }
                        stream.Process.Dispose();
                    }
                    _streams.Clear();
                }
                _disposed = true;
            }
        }

        public Task ProcessStreamDataAsync(string streamKey, Stream stream)
        {
            throw new NotImplementedException();
        }

        private class StreamInfo
        {
            public string StreamKey { get; set; }
            public Process Process { get; set; }
            public DateTime StartTime { get; set; }
            public string Status { get; set; }
        }
    }
}