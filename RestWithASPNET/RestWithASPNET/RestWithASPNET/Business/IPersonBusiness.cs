using System.Collections.Generic;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;

namespace RestWithASPNET.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO FindbyId(long id);
        List<PersonVO> FindByName(string firstName, string lastName);
        List<PersonVO> FindAll();
        PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        PersonVO Disable(long id);
        PersonVO Update(PersonVO person);
        void Delete(long id);


    }
}
