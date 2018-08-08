using Nest;

namespace Common
{
    [ElasticsearchType(Name = "default", IdProperty = nameof(Id))]
    public class User
    {
        [Number(Name = nameof(Id), IgnoreMalformed = true)]
        public int Id { get; set; }


        [GeoPoint(Name = nameof(AmapLocation), IgnoreMalformed = true)]
        public GeoLocation AmapLocation { get; set; }
    }
}
