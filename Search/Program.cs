using Common;
using Nest;
using System;
using System.Threading.Tasks;
using System.Linq;

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

            foreach (var hit in searchResponse.Hits)
            {
                Console.WriteLine($"距离{hit.Source.Name} {Math.Ceiling((double)hit.Sorts.FirstOrDefault())}米");
            }
            Console.ReadKey();
        }
        private static async Task<ISearchResponse<User>> SearchPeople()
        {
            return await _esClient.SearchAsync<User, User>(s => s
                 .Index(_indexName)
                 .Sort(ss => ss
                    .GeoDistance(g => g
                        .Field(p => p.AmapLocation)
                        .Order(SortOrder.Ascending)
                        .Unit( DistanceUnit.Meters)
                        .Mode(SortMode.Min)
                        .Points(new GeoLocation(latitude: 40.066163, longitude: 116.359392))
                    )
                 )
                 .Query(q => q
                     .Bool(b => b
                         .Must(m => m
                             .MatchAll()
                         )
                         .Filter(f => f
                             .GeoDistance(g => g
                                 .Distance("500m")
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
