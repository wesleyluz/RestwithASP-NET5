using RestWithASPNET.Hypermedia;
using System.Collections.Generic;

namespace RestWithASPNET.Hypermidia.Abstract
{
    public interface ISupporstHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }
    }
}
