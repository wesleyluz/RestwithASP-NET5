using RestWithASPNET.Data.Converter.Implematations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
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

        public PagedSearchVO<BookVO> FindWithPagedSearch(string author, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from books p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(author)) query = query + $"and p.author like '%{author}%' ";
            query += $"order by p.author {sort} limit {size} offset {offset} ";

            string countQuery = "select count(*) from books p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(author)) countQuery = countQuery + $"and p.author like '%{author}%' ";

            var library = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<BookVO>
            {
                CurrentPage = page,
                List = _converter.Parse(library),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public BookVO Update(BookVO book)
        {
            var bookEntity = _converter.Parse(book);
            bookEntity = _repository.Update(bookEntity);
            return _converter.Parse(bookEntity);
        }
    }
}
