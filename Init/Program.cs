using Common;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Init
{
    class Program
    {
        const string _indexName = "user";
        const string _typeName = "default";
        private static ElasticClient _esClient = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200")));
        static void Main(string[] args)
        {
            _esClient.DeleteIndex(_indexName);
            _esClient.CreateIndex(_indexName, c => c
                 .Settings(s => s
                     .NumberOfShards(1)
                      .NumberOfReplicas(1)
                 )
            );
            _esClient.Map<User>(m => m.Index(_indexName).Type(_typeName).AutoMap());

            Import().GetAwaiter().GetResult();

            Console.WriteLine("Hello World!");
        }

        private static async Task Import()
        {
            List<User> userList = new List<User>
            {
                new User{ Id=1,AmapLocation=new GeoLocation(latitude:40.066061,longitude:116.359382)},
                new User{ Id=2,AmapLocation=new GeoLocation(latitude:40.066028,longitude:116.359012)},
                new User{ Id=3,AmapLocation=new GeoLocation(latitude:40.06643,longitude:116.359382)},
                new User{ Id=4,AmapLocation=new GeoLocation(latitude:40.066233,longitude:116.35803)}
            };

            foreach (var user in userList)
            {
                await _esClient.IndexAsync(user, u => u.Index(_indexName).Type(_typeName));
            }

        }
    }
}
