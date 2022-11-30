using RestWithASPNET.Data.Converter.Implematations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implemenatations
{
    public class BookBusinessImp : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BookConverter _converter;
        
        public BookBusinessImp(IRepository<Book> repository) 
        {
            _repository = repository;
            _converter = new BookConverter();
        }


        public BookVO Create(BookVO book)
        {
            var bookEntity = _converter.Parse(book);
            bookEntity = _repository.Create(bookEntity);
            return _converter.Parse(bookEntity);
        }


        public void Delete(long id)
        {
            _repository.Delete(id);

        }

        public List<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

      
        public BookVO FindbyId(long id)
        {

            return _converter.Parse(_repository.FindbyId(id));
        }

        public BookVO Update(BookVO book)
        {
            var bookEntity = _converter.Parse(book);
            bookEntity = _repository.Update(bookEntity);
            return _converter.Parse(bookEntity);
        }
    }
}
