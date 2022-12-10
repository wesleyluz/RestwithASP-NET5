using RestWithASPNET.Model.Base;
using RestWithASPNET.Model.Context;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RestWithASPNET.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    { 
        protected MySqlContext _context;
        private DbSet<T> _dataSet;

        public GenericRepository(MySqlContext context)
        {
            _context = context;
            _dataSet = context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                _dataSet.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public T Update(T item)
        {
            var result = _dataSet.SingleOrDefault(p => p.Id.Equals(item.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public void Delete(long id)
        {
            var result = _dataSet.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    _dataSet.Remove(result);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool Exists(long id)
        {
            return _dataSet.Any(p => p.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return _dataSet.ToList();
        }

        public T FindbyId(long id)
        {
            return _dataSet.SingleOrDefault(p => p.Id.Equals(id)); 
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return _dataSet.FromSqlRaw<T>(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }
            return int.Parse(result);
        }
    }
}
