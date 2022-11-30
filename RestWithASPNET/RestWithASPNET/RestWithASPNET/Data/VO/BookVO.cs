using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermidia.Abstract;
using System;
using System.Collections.Generic;

namespace RestWithASPNET.Data.VO
{
    public class BookVO : ISupporstHyperMedia 
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public DateTime Launch_date { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
