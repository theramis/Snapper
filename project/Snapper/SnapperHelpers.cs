using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Snapper
{
    public static class SnapperHelpers
    {
        /// <summary>
        /// Removes the specified properties from the target object. Nested properties and array elements can be ignored too.
        /// </summary>
        /// <param name="target">The original target.</param>
        /// <param name="selectors">Expressions selecting which properties (or array elements) to ignore.</param>
        public static object Ignore<T>(this T target, params Expression<Func<T, object>>[] selectors)
        {
            object obj = target;
            foreach (var selector in selectors)
            {
                obj = obj.Ignore(selector.Body);
            }

            return obj;
        }

        private static object Ignore(this object obj, Expression selector)
        {
            return selector switch
            {
                LambdaExpression e => obj.Ignore(e.Body),
                BinaryExpression e => obj.IgnoreArrayIndex(e),
                MemberExpression e => obj.IgnoreMember(e),
                _ => obj,
            };
        }

        private static object IgnoreArrayIndex(this object obj, BinaryExpression expression)
        {
            if (expression.Right is ConstantExpression c &&
                c.Value is int index && 
                expression.Left is MemberExpression m)
            {
                return obj.EditMember(m.GetMembers(), (properties, member) =>
                {
                    var items = new List<object>();
                    var array = (Array)properties[member.Name];
                    for (var i = 0; i < array.Length; i++)
                    {
                        if (i != index)
                        {
                            items.Add(array.GetValue(i));
                        }
                    }
                    properties[member.Name] = items.ToArray();
                });
            }
            return obj;
        }

        private static object IgnoreMember(this object obj, MemberExpression expression)
        {
            return obj.EditMember(expression.GetMembers(), (properties, member) => properties.Remove(member.Name));
        }

        private static object EditMember(this object obj, Stack<MemberInfo> stack, Action<IDictionary<string, object>, MemberInfo> edit)
        {
            if (stack.Count == 0) throw new ArgumentException();
            dynamic dynamicObj = obj.AsDynamicObject();
            var properties = (IDictionary<string, object>)dynamicObj;

            var member = stack.Pop();

            if (stack.Count == 0)
            {
                edit(properties, member);
            }
            else
            {
                properties[member.Name] = properties[member.Name].EditMember(stack, edit);
            }
            return dynamicObj;

        }

        private static Stack<MemberInfo> GetMembers(this MemberExpression e)
        {
            var memberInfo = new Stack<MemberInfo>();
            do
            {
                memberInfo.Push(e.Member);
                e = e.Expression switch
                {
                    MemberExpression member => member,
                    ParameterExpression => null,
                    _ => throw new InvalidOperationException("Stack could not be resolved."),
                };
            }
            while (e is not null);
            return memberInfo;
        }

        private static ExpandoObject AsDynamicObject(this object obj)
        {
            if (obj is null) return null;
            if (obj is ExpandoObject o) return o;

            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var property in obj.GetType().GetProperties())
            {
                dictionary.Add(property.Name, property.GetValue(obj));
            }
            return expando;
        }
    }
}

