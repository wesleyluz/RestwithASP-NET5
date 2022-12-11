using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermidia.Abstract;
using System.Collections.Generic;

namespace RestWithASPNET.Data.VO
{
    public class PersonVO : ISupporstHyperMedia
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public long Age { get; set; }

        public bool Enabled { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
