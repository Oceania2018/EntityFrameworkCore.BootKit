using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public static class Utility
    {
        public static List<Type> GetClassesWithInterface(Type type, string assemblyName)
        {
            List<Type> types = Assembly.Load(new AssemblyName(assemblyName))
                .GetTypes()
                .Where(x => !x.IsAbstract && !x.FullName.StartsWith("<>f__AnonymousType"))
                .ToList();

            types = types.Where(x => !x.GetTypeInfo().IsAbstract && x.GetInterfaces().Contains(type)).ToList();

            return types;
        }

        public static List<Type> GetClassesWithInterface(Type type, params string[] assemblyNames)
        {
            List<Type> types = new List<Type>();
            assemblyNames.ToList().ForEach(assemblyName => {
                types.AddRange(GetClassesWithInterface(type, assemblyName));
            });

            return types;
        }

        public static List<T> GetInstanceWithInterface<T>(params string[] assemblyNames)
        {
            List<T> instances = new List<T>();

            var types = GetClassesWithInterface(typeof(T), assemblyNames);
            var objects = types.Where(x => x.GetInterfaces().Contains(typeof(T))).Select(x => (T)Activator.CreateInstance(x)).ToList();
            instances.AddRange(objects);

            return instances;
        }

        public static bool SetValue(this object obj, string propName, object value)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperties().FirstOrDefault(x => x.Name.ToLower().Equals(propName.ToLower()));
            if (property == null) return false;

            if (property.PropertyType.Equals(typeof(String)))
            {
                property.SetValue(obj, value.ToString(), null);
            }
            else if (property.PropertyType.Equals(typeof(DateTime)) && value.GetType() == typeof(string))
            {
                property.SetValue(obj, DateTime.Parse(value.ToString()), null);
            }
            else if (property.PropertyType.Equals(typeof(Int32)) && value.GetType() == typeof(string))
            {
                property.SetValue(obj, Int32.Parse(value.ToString()), null);
            }
            /*else if (property.PropertyType.IsEnum)
            {
                property.SetValue(obj, int.Parse(value.ToString()), null);
            }
            else if (property.PropertyType.IsGenericType && Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).IsEnum)
            {
                var enumType = Nullable.GetUnderlyingType(property.PropertyType);
                var enumValue = Enum.ToObject(enumType, value);
                property.SetValue(obj, enumValue, null);
            }
            else if (property.PropertyType.IsGenericType && Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).Equals(typeof(int)))
            {
                property.SetValue(obj, int.Parse(value.ToString()), null);
            }*/
            else if (property.PropertyType.IsGenericType && Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).Equals(typeof(decimal)))
            {
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    property.SetValue(obj, null);
                }
                else
                {
                    property.SetValue(obj, decimal.Parse(value.ToString()));
                }
            }
            else if (property.PropertyType.Equals(typeof(Decimal)))
            {
                property.SetValue(obj, decimal.Parse(value.ToString()));
            }
            else
            {
                property.SetValue(obj, value, null);
            }

            return true;
        }
    }
}
