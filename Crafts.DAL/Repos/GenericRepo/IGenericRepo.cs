using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.GenericRepo;

public interface IGenericRepo<T> where T: class
{
    List<T> GetAll();
    T? GetById(int id);
    Task Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    int SaveChanges();
}
