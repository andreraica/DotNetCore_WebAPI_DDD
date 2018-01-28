using System;
using System.Linq.Expressions;

namespace Infrastructure.CrossCutting.Interfaces
{
    public interface ICacheManager
    {
        T Do<T>(Expression<Func<T>> expression, TimeSpan? expire = null, bool forceUpdate = false);
        void Set<T>(string key, T value, TimeSpan? timeExpire = null);
        T Get<T>(string key);
        void Remove(string key);
        bool Exists(string key);
    }
}