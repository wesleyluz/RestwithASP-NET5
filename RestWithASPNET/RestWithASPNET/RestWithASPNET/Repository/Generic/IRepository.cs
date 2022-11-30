using System.Collections.Generic;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Base;


namespace RestWithASPNET.Repository
{
    public interface IRepository <T> where T : BaseEntity
    {
       public T Create(T item);
       public T FindbyId(long id);
       public List<T> FindAll();
       public T Update(T item);
       public void Delete(long id);
       public bool Exists(long id);



   }
}
