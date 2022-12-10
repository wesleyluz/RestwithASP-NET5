using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Repository.UserRep
{
    public interface IPersonRepository : IRepository<Person>
    {
        public Person Disable(long id);
        public List<Person> FindByName(string firstName, string lastName);
    }
}
