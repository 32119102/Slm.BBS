using System;
using System.Reflection;
using Volo.Abp;

namespace ContIn.Abp.Terminal.FreeRedis
{
    public class RedisClientNameAttribute : Attribute
    {
        private string Name;

        public RedisClientNameAttribute(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public static string GetClientName<T>()
        {
            return GetClientName(typeof(T));
        }

        public static string GetClientName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<RedisClientNameAttribute>();

            if (nameAttribute == null)
            {
                return type.FullName;
            }

            return nameAttribute.GetName();
        }
    }
}
