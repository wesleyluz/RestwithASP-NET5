using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implemenatations
{
    public class PersonBusinessImp : IPersonBusiness
    {
        private readonly IPersonRepository _repository;
        
        public PersonBusinessImp(IPersonRepository repository) 
        {
            _repository = repository;
        }

      

        public Person Create(Person person)
        {
            
            return _repository.Create(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);

        }

        public List<Person> FindAll()
        {
            return _repository.FindAll();
        }

      
        public Person FindbyId(long id)
        {
           
            return _repository.FindbyId(id);
        }

        public Person Update(Person person)
        {
            
            return _repository.Update(person);
        }
    }
}
