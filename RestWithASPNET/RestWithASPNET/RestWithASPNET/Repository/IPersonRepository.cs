using System.Collections.Generic;
using RestWithASPNET.Model;

namespace RestWithASPNET.Repository
{
    public interface IPersonRepository
    {
       public Person Create(Person person);
       public Person FindbyId(long id);
       public List<Person> FindAll();
       public Person Update(Person person);
       public void Delete(long id);
       public bool Exists(long id);



   }
}
