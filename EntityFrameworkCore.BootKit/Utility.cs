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
                .GetTypes().ToList();

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
    }
}
