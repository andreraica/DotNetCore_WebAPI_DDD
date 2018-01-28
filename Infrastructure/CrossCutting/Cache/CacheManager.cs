using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.CrossCutting.Interfaces;

namespace Infrastructure.CrossCutting.Cache
{
    public class CacheManager : ICacheManager
    {
        static IDatabase db;
        private string _connectionString = "cache"; //Configuration.Configuration.Cache;

        public T Do<T>(Expression<Func<T>> expression, TimeSpan? expire = null, bool forceUpdate = false)
        {
            var methodCall = expression.Body as MethodCallExpression;

            var key = GenerateKey(methodCall);

            var cachedObject = forceUpdate ? default(T) : Get<T>(key);

            if (cachedObject == null || EqualityComparer<T>.Default.Equals(cachedObject, default(T)))
            {
                var action = expression.Compile();
                var result = action();
                Set(key, result, expire);
                return result;
            }

            return cachedObject;
        }

        private static string GenerateKey(MethodCallExpression methodCall)
        {
            var methodName = methodCall.Object.Type.FullName + "." + methodCall.Method.Name;
            var argumentos = new List<string>();
            var parameters = methodCall.Method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                argumentos.Add(parameters[i].Name + ":" + Evaluate(methodCall.Arguments[i]).ToString().Replace(" ", ""));
            }

            return string.Format("{0}({1})", methodName, string.Join("|", argumentos));
        }

        private static object Evaluate(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression)exp).Value;
                case ExpressionType.MemberAccess:
                    var me = (MemberExpression)exp;
                    switch (me.Member.MemberType)
                    {
                        case System.Reflection.MemberTypes.Field:
                            return ((FieldInfo)me.Member).GetValue(Evaluate(me.Expression));
                        case MemberTypes.Property:
                            return ((PropertyInfo)me.Member).GetValue(Evaluate(me.Expression), null);
                        default:
                            throw new NotSupportedException(me.Member.MemberType.ToString());
                    }
                case ExpressionType.MemberInit:
                    return ((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return (ListInitExpression)exp;                 
                default:
                    throw new NotSupportedException(exp.NodeType.ToString());
            }
        }

        private void Connect()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
            db = redis.GetDatabase();
        }

        public void Set<T>(string key, T value, TimeSpan? timeExpire = null)
        {
            if (db == null)
                Connect();

            db.StringSet(key, JsonConvert.SerializeObject(value), timeExpire);
        }

        public T Get<T>(string key)
        {
            if (db == null)
                Connect();

            string value = db.StringGet(key);
            if (String.IsNullOrEmpty(value))
                return default(T);
            return JsonConvert.DeserializeObject<T>(value);

        }

        public bool Exists(string key)
        {
            if (db == null)
                Connect();

            return db.KeyExists(key);
        }

        public void Remove(string key)
        {
            if (db == null)
                Connect();

            db.KeyDelete(key);
        }
    }
}
