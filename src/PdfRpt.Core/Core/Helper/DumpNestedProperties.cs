using System;
using System.Collections.Generic;
using System.Reflection;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// A helper class for dumping nested property values
    /// </summary>
    public class DumpNestedProperties
    {
        private readonly IList<CellData> _result = new List<CellData>();
        private int _index;

        /// <summary>
        /// Dumps Nested Property Values
        /// </summary>
        /// <param name="data">an instance object</param>
        /// <param name="parent">parent object's name</param>
        /// <param name="dumpLevel">how many levels should be searched</param>
        /// <returns>Nested Property Values List</returns>
        public IList<CellData> DumpPropertyValues(object data, string parent = "", int dumpLevel = 2)
        {
            if (data == null) return null;

            var propertyGetters = FastReflection.Instance.GetGetterDelegates(data.GetType());
            foreach (var propertyGetter in propertyGetters)
            {
                var dataValue = propertyGetter.GetterFunc(data);
                var name = string.Format("{0}{1}", parent, propertyGetter.Name);
                var propertyType = propertyGetter.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                var isNullable = underlyingType != null;
                if (isNullable)
                {
                    propertyType = underlyingType;
                }

                if (dataValue == null)
                {
                    var nullDisplayText = propertyGetter.MemberInfo.GetNullDisplayTextAttribute();
                    if (isNullable && string.IsNullOrWhiteSpace(nullDisplayText))
                    {
                        nullDisplayText = null;
                    }
                    _result.Add(new CellData
                    {
                        PropertyName = name,
                        PropertyValue = nullDisplayText,
                        PropertyIndex = _index++,
                        PropertyType = propertyType
                    });
                }
#if NET40
                else if (propertyType.IsEnum)
#else
                else if (propertyType.GetTypeInfo().IsEnum)
#endif
                {
                    var enumValue = ((Enum)dataValue).GetEnumStringValue();
                    _result.Add(new CellData
                    {
                        PropertyName = name,
                        PropertyValue = enumValue,
                        PropertyIndex = _index++,
                        PropertyType = propertyType
                    });
                }
                else if (isNestedProperty(propertyType))
                {
                    _result.Add(new CellData
                    {
                        PropertyName = name,
                        PropertyValue = dataValue,
                        PropertyIndex = _index++,
                        PropertyType = propertyType
                    });

                    if (parent.Split('.').Length > dumpLevel)
                    {
                        continue;
                    }
                    DumpPropertyValues(dataValue, $"{name}.", dumpLevel);
                }
                else
                {
                    _result.Add(new CellData
                    {
                        PropertyName = name,
                        PropertyValue = dataValue,
                        PropertyIndex = _index++,
                        PropertyType = propertyType
                    });
                }
            }
            return _result;
        }

        private static bool isNestedProperty(Type type)
        {
#if NET40
            var typeInfo = type;
#else
            var typeInfo = type.GetTypeInfo();
#endif
            var assemblyFullName = typeInfo.Assembly.FullName;
            if (assemblyFullName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase) ||
                assemblyFullName.StartsWith("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return
                   (typeInfo.IsClass || typeInfo.IsInterface) &&
                   !typeInfo.IsValueType &&
                   !string.IsNullOrEmpty(type.Namespace) &&
                   !type.Namespace.StartsWith("System.", StringComparison.OrdinalIgnoreCase);
        }
    }
}