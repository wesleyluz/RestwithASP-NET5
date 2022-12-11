using System.Collections.Generic;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindbyId(long id);
        List<BookVO> FindAll();
        PagedSearchVO<BookVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        BookVO Update(BookVO book);
        void Delete(long id);

    }
}
