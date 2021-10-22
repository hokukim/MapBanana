using System;

namespace MapBanana.Api.Models
{
    public class MapModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}
