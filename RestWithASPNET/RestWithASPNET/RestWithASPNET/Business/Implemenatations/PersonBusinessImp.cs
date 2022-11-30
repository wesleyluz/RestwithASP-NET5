using RestWithASPNET.Data.Converter.Implematations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implemenatations
{
    public class PersonBusinessImp : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;
        
        private readonly PersonConverter _converter;
        
        public PersonBusinessImp(IRepository<Person> repository) 
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

      

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity= _repository.Create(personEntity);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);

        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

      
        public PersonVO FindbyId(long id)
        {
           
            return _converter.Parse(_repository.FindbyId(id));
        }

        public PersonVO Update(PersonVO person)
        {

            var personEntity = _converter.Parse(person);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }
    }
}
