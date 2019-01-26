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
            _esClient.Map<User>(m => m.Index(_indexName).AutoMap());

            Import().GetAwaiter().GetResult();

            Console.WriteLine("初始化数据完成");
            Console.ReadKey();
        }

        private static async Task Import()
        {
            List<User> userList = new List<User>
            {
                new User{ Id=1,AmapLocation=new GeoLocation(latitude:40.066074,longitude:116.359387),Name="东南门"},
                new User{ Id=2,AmapLocation=new GeoLocation(latitude:40.066033,longitude:116.359044),Name="亮哲剪艺"},
                new User{ Id=3,AmapLocation=new GeoLocation(latitude:40.066234,longitude:116.358035),Name="张亮麻辣烫"},
                new User{ Id=4,AmapLocation=new GeoLocation(latitude:40.066673,longitude:116.358024),Name="兰州老马食府"},
                new User{ Id=5,AmapLocation=new GeoLocation(latitude:40.066862,longitude:116.357075),Name="北京国际温泉体育健身中心"},
                new User{ Id=6,AmapLocation=new GeoLocation(latitude:40.067441,longitude:116.359913),Name="文德幼儿园"}
            };

            foreach (var user in userList)
            {
                await _esClient.IndexAsync(user, u => u.Index(_indexName));
            }

        }
    }
}
