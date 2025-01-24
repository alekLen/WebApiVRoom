using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using WebApiVRoom.BLL.DTO;


namespace WebApiVRoom.BLL.Services
{
    public class AlgoliaService : IAlgoliaService
    {
        private readonly ISearchIndex _index;
        public AlgoliaService(IConfiguration configuration)
        {
            var t = configuration.GetConnectionString("AlgoliaAppId");
            var s = configuration.GetConnectionString("AlgoliaKey");
            var algoliaClient = new SearchClient(t, s);
            _index = algoliaClient.InitIndex("videos");// "videos" — имя  индекса
        }

        public async Task<string> AddOrUpdateVideoAsync(VideoForAlgolia video)
        {
            if (string.IsNullOrEmpty(video.ObjectID))
            {
                video.ObjectID = Guid.NewGuid().ToString();
            }

            await _index.SaveObjectAsync(video);
            return video.ObjectID;
        }

        public async Task DeleteVideoAsync(string id)
        {
            await _index.DeleteObjectAsync(id);
        }

        public async Task<SearchResponse<VideoForAlgolia>> SearchVideosAsync(string query)
        {
            try
            {
                var algoliaQuery = new Query(query);

            //Дополнительные параметры в Query:
                //var algoliaQuery = new Query(query)
                //{
                //    HitsPerPage = 20, // Количество результатов на страницу
                //    Page = 0, // Номер страницы
                //    Filters = "IsShort:true", // Пример фильтра
                //    SortFacetValuesBy = "count" // Пример сортировки по количеству
                //};

            // Выполняем поиск по запросу
                return await _index.SearchAsync<VideoForAlgolia>(algoliaQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении поиска: {ex.Message}");
                throw;
            }
        }
    }
}

//JSON
//    {
//  "hits": [
//    {
//      ...
//      "objectID": "433"
//      ...
//    }
//  ],
//  "page": 0,
//  "nbHits": 1,
//  "nbPages": 1,
//  "hitsPerPage": 20,
//  "processingTimeMS": 1,
//  "query": "jimmie paint",
//  "params": "query=jimmie+paint&attributesToRetrieve=firstname,lastname&hitsPerPage=50"
//}
