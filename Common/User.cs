using Nest;

namespace Common
{
    public class User
    {
        [Number(Name = nameof(Id), IgnoreMalformed = true)]
        public int Id { get; set; }

        [GeoPoint(Name = nameof(AmapLocation), IgnoreMalformed = true)]
        public GeoLocation AmapLocation { get; set; }

        [Keyword(Name = nameof(Name))]
        public string Name { get; set; }
    }
}
