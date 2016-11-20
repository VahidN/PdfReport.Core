using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Fast property access, using Reflection.Emit.
    /// </summary>
    public class FastReflection
    {
        /// <summary>
        /// It's a lazy loaded thread-safe singleton.
        /// </summary>
        private static readonly Lazy<FastReflection> _instance =
            new Lazy<FastReflection>(() => new FastReflection(), LazyThreadSafetyMode.ExecutionAndPublication);

        // 'GetOrAdd' call on the dictionary is not thread safe and we might end up creating the GetterInfo more than
        // once. To prevent this Lazy<> is used. In the worst case multiple Lazy<> objects are created for multiple
        // threads but only one of the objects succeeds in creating a GetterInfo.
        private readonly ConcurrentDictionary<Type, Lazy<List<GetterInfo>>> _gettersCache =
            new ConcurrentDictionary<Type, Lazy<List<GetterInfo>>>();

        private FastReflection()
        {
        }

        /// <summary>
        /// Singleton instance of FastReflection.
        /// </summary>
        public static FastReflection Instance { get; } = _instance.Value;

        /// <summary>
        /// Fast property access, using Reflection.Emit.
        /// </summary>
        public IList<GetterInfo> GetGetterDelegates(Type type)
        {
            var getterDelegates = _gettersCache.GetOrAdd(type, x => new Lazy<List<GetterInfo>>(
                () =>
                {
                    var gettersList = new List<GetterInfo>();
                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    int index = 0;
                    foreach (var property in properties)
                    {
                        var getterDelegate = createGetterPropertyDelegate(type, property, index);
                        index++;

                        if (getterDelegate == null)
                            continue;

                        var info = new GetterInfo
                        {
                            Name = property.Name,
                            GetterFunc = getterDelegate,
                            PropertyType = property.PropertyType,
                            MemberInfo = property
                        };
                        gettersList.Add(info);
                    }

                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var field in fields)
                    {
                        var getterDelegate = createGetterFieldDelegate(type, field);
                        if (getterDelegate == null)
                            continue;

                        var info = new GetterInfo
                        {
                            Name = field.Name,
                            GetterFunc = getterDelegate,
                            PropertyType = field.FieldType,
                            MemberInfo = field
                        };
                        gettersList.Add(info);
                    }

                    return gettersList;
                }));
            return getterDelegates.Value;
        }

        static Func<object, object> createGetterFieldDelegate(Type type, FieldInfo fieldInfo)
        {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var field = Expression.Field(Expression.TypeAs(instanceParam, type), fieldInfo);
            var convertField = Expression.TypeAs(field, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertField, instanceParam).Compile();
        }

        static Func<object, object> createGetterPropertyDelegate(Type type, PropertyInfo propertyInfo, int index)
        {
            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null)
                throw new InvalidOperationException(string.Format("Couldn't get the GetMethod of {0}", type));

            var hasParameter = getMethod.GetParameters().Any();

            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var getterExpression = Expression.Convert(
                hasParameter ? Expression.Call(Expression.Convert(instanceParam, type), getMethod, Expression.Constant(index))
                : Expression.Call(Expression.Convert(instanceParam, type), getMethod)
                , typeof(object));
            return Expression.Lambda<Func<object, object>>(getterExpression, instanceParam).Compile();
        }
    }

    /// <summary>
    /// Getter method's info.
    /// </summary>
    public class GetterInfo
    {
        /// <summary>
        /// Property/Field's Getter method.
        /// </summary>
        public Func<object, object> GetterFunc { set; get; }

        /// <summary>
        /// Obtains information about the attributes of a member and provides access.
        /// </summary>
        public MemberInfo MemberInfo { set; get; }

        /// <summary>
        /// Property/Field's name.
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Property/Field's Type.
        /// </summary>
        public Type PropertyType { set; get; }
    }
}