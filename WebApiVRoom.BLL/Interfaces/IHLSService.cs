using System.Threading.Tasks;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IHLSService
    {
        Task<string> StartStreamAsync(string streamKey);
        Task StopStreamAsync(string streamKey);
        Task<byte[]> GetSegmentAsync(string streamKey, string segmentName);
        Task<string> GetPlaylistAsync(string streamKey);
        Task<string> GetStreamStatusAsync(string streamKey);
        void Dispose();
        Task WriteStreamDataAsync(string streamKey, byte[] buffer, int v, int length);
        Task ProcessSegmentAsync(string streamKey, byte[] segmentData, double v1, int v2);
        Task ProcessStreamDataAsync(string streamKey, Stream stream);
    }
}