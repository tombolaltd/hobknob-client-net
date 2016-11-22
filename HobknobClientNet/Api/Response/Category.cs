using System.Collections.Generic;

namespace HobknobClientNet.Api.Response
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Columns { get; set; }

        public List<Feature> Features { get; set; }
    }
}
