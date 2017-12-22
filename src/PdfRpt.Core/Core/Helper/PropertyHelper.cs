using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Get the string name and type of a property or field. Eg <c>string name = Property.Name&lt;string&gt;(x =&gt; x.Length);</c>
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Gets the type for the specified entity property or field. Eg <c>string name = Property.Name&lt;string&gt;(x =&gt; x.Length);</c>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>The name of the property as a string</returns>
        public static string Name<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return GetNestedPropertyName(expression);
        }

        /// <summary>
        /// Gets the type for the specified entity property or field. Eg Type&lt;string&gt;(x =&gt; x.Length) == typeof(int)
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>A type.</returns>
        public static Type Type<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var memberExpression = GetMemberExpression(expression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.PropertyType;

            //not a property, maybe a public field
            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.FieldType;

            //unknown
            return typeof(object);
        }

        /// <summary>
        /// Converts an Expression to a MemberExpression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression<TEntity, T>(Expression<Func<TEntity, T>> expression)
        {
            //originally from Fluent NHibernate
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            //runtime exception if not a member
            if (memberExpression == null)
                throw new ArgumentException("Not a property or field", "expression");

            return memberExpression;
        }

        /// <summary>
        /// Converts a nested property (eg. Studies.ID or Studies.Class.Name) to string
        /// </summary>
        /// <param name="expression">LambdaExpression</param>
        /// <returns></returns>
        public static string GetNestedPropertyName(LambdaExpression expression)
        {
            var bodyExpression = expression.Body;
            var body = bodyExpression as UnaryExpression;
            if (body != null)
            {
                if (body.NodeType == ExpressionType.Convert)
                {
                    var mex = (MemberExpression)body.Operand;
                    if (mex.Expression.NodeType != ExpressionType.MemberAccess)
                        return mex.Member.Name;

                    bodyExpression = mex;
                }
            }

            var memberExpression = bodyExpression as MemberExpression;
            //runtime exception if not a member
            if (memberExpression == null)
                throw new ArgumentException("Not a property or field", "expression");

            var memberExpressionOrg = memberExpression;

            var path = "";
            while (memberExpression != null &&
                   memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var propInfo = memberExpression.Expression.GetType().GetProperty("Member");
                var propValue = propInfo.GetValue(memberExpression.Expression, null) as PropertyInfo;
                path = propValue.Name + "." + path;
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return path + memberExpressionOrg.Member.Name;
        }


        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
#if NET40
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
#else
                    if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
#endif
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Gets value of a property, including enum's string description and NullDisplayTextAttribute.
        /// </summary>
        /// <param name="propertyInfo">property info</param>
        /// <param name="instance">object's instance</param>
        /// <returns>value of the property</returns>
        public static object GetPropertyValue(this PropertyInfo propertyInfo, object instance)
        {
            var value = propertyInfo.GetValue(instance, null);
#if NET40
            if (value != null && propertyInfo.PropertyType.IsEnum)
#else
            if (value != null && propertyInfo.PropertyType.GetTypeInfo().IsEnum)
#endif
            {
                return ((Enum)value).GetEnumStringValue();
            }

            if (value == null)
            {
                var nullDisplayText = propertyInfo.GetNullDisplayTextAttribute();
                if (!string.IsNullOrEmpty(nullDisplayText)) return nullDisplayText;
            }

            return value;
        }
    }
}