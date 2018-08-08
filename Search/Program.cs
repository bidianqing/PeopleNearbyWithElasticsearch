using Common;
using Nest;
using System;
using System.Threading.Tasks;

namespace Search
{
    class Program
    {
        const string _indexName = "user";
        const string _typeName = "default";
        private static ElasticClient _esClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")));
        static void Main(string[] args)
        {
            var searchResponse = SearchPeople().GetAwaiter().GetResult();

            foreach (var item in searchResponse.Documents)
            {
                Console.WriteLine($"{item.Name}");
            }

            Console.ReadKey();
        }

        private static async Task<ISearchResponse<User>> SearchPeople()
        {
            return await _esClient.SearchAsync<User, User>(s => s
                 .Index(_indexName)
                 .Type(_typeName)
                 .Query(q => q
                     .Bool(b => b
                         .Must(m => m
                             .MatchAll()
                         )
                         .Filter(f => f
                             .GeoDistance(g => g
                                 .Distance("250m")
                                 .Field(p => p.AmapLocation)
                                 .Location(new GeoLocation(latitude: 40.066163, longitude: 116.359392))
                             )
                         )
                     )
                 )
           );
        }
    }
}
