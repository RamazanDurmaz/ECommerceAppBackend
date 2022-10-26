using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    //where kısmının açıklaması referans olan IEntityden implemente edilmiş new lenebilir herşeyi gönderebilirsin
    public interface IEntityRepository<T> where T : class, IEntity
    {
        T Get(Expression<Func<T, bool>> filter);
        //Filtre gönderilmezse tümünü listelesin diye null atadık.
        IList<T> GetAll(Expression<Func<T, bool>> filter = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
