using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.Services
{
    public class LiveStreamingService
    {
        private readonly YouTubeService _youTubeService;

        public LiveStreamingService(string apiKey)
        {
            _youTubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = apiKey,
                ApplicationName = "Vroom"  
            });
        }

        public async Task<string> CreateLiveBroadcast(string title, string description, DateTime startTime)
        {
            var broadcast = new LiveBroadcast
            {
                Snippet = new LiveBroadcastSnippet
                {
                    Title = title,
                    Description = description,
                    ScheduledStartTime = startTime
                },
                Status = new LiveBroadcastStatus
                {
                    PrivacyStatus = "public" 
                },
                Kind = "youtube#liveBroadcast"
            };

            var request = _youTubeService.LiveBroadcasts.Insert(broadcast, "snippet,status");
            var response = await request.ExecuteAsync();
            return response.Id; 
        }

        public async Task<string> CreateLiveStream(string title)
        {
            var liveStream = new LiveStream
            {
                Snippet = new LiveStreamSnippet { Title = title },
                Cdn = new CdnSettings
                {
                    IngestionType = "rtmp",
                    Resolution = "720p",
                    FrameRate = "30fps"
                },
                Kind = "youtube#liveStream"
            };

            var request = _youTubeService.LiveStreams.Insert(liveStream, "snippet,cdn");
            var response = await request.ExecuteAsync();
            return response.Id; 
        }

        public async Task<string> BindBroadcastToStream(string broadcastId, string streamId)
        {
            var bindRequest = _youTubeService.LiveBroadcasts.Bind(broadcastId, "id,contentDetails");
            bindRequest.StreamId = streamId;
            var bindResponse = await bindRequest.ExecuteAsync();
            return bindResponse.Id;
        }
    }
}
