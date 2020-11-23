using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snapper.Core
{
    internal static class ReflectionHelpers
    {
        internal static IEnumerable<T> GetCustomAttributes<T>(
            this MemberInfo member)
            where T : Attribute
        {
            var providers = new ICustomAttributeProvider[] {
                member, // check method
                member?.DeclaringType, // check class
                member?.DeclaringType?.Assembly // check assembly
            };

            var attrs = providers.SelectMany(p => p.GetCustomAttributes(typeof(T), true));

            return providers
                .SelectMany(p => p.GetCustomAttributes(typeof(T), true))
                .Cast<T>();
        }
    }
}
