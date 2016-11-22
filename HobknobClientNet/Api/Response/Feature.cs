using System.Collections.Generic;

namespace HobknobClientNet.Api.Response
{
    public class Feature
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<bool?> Values { get; set; }

        public int CategoryId { get; set; }
    }
}
