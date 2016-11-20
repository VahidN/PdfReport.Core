using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// Converts a list to a crosstab list
    /// </summary>
    public static class CrosstabExtension
    {
        /// <summary>
        /// Dynamic crosstab data source maker
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">List of rows</param>
        /// <param name="leftColumn">Row Heading</param>
        /// <param name="topField">Pivot Column</param>
        /// <param name="valueField">Aggregate</param>
        /// <param name="additionalFields">Additional Fields</param>
        /// <returns></returns>
        public static IEnumerable Pivot<TSource, TKey1, TKey2, TKey3, TValue>
                (
                    this IEnumerable<TSource> source,
                    Func<TSource, TKey1> leftColumn,
                    Func<TSource, TKey2> topField,
                    Func<IEnumerable<TSource>, TValue> valueField,
                    Func<IEnumerable<TSource>, TKey3> additionalFields
                )
        {
            return source.GroupBy(leftColumn)
                             .Select(
                                myGroup => new
                                {
                                    X = myGroup.Key,
                                    Y = myGroup.GroupBy(topField)
                                         .Select(
                                            z => new
                                            {
                                                Z = z.Key,
                                                V = valueField(z)
                                            }),
                                    h = additionalFields(myGroup)
                                });
        }


        /// <summary>
        /// Dynamic crosstab data source maker
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">List of rows</param>
        /// <param name="leftColumn">Row Heading</param>
        /// <param name="topField1">Pivot Column</param>
        /// <param name="valueField1">Aggregate</param>
        /// <param name="topField2">Pivot Column</param>
        /// <param name="valueField2">Aggregate</param>
        /// <param name="additionalFields">Additional Fields</param>
        /// <returns></returns>
        public static IEnumerable Pivot<TSource, TKey1, TKey2, TKey3, TValue>
                (
                    this IEnumerable<TSource> source,
                    Func<TSource, TKey1> leftColumn,
                    Func<TSource, TKey2> topField1,
                    Func<IEnumerable<TSource>, TValue> valueField1,
                    Func<TSource, TKey2> topField2,
                    Func<IEnumerable<TSource>, TValue> valueField2,
                    Func<IEnumerable<TSource>, TKey3> additionalFields
                )
        {
            return source.GroupBy(leftColumn)
                             .Select(
                                myGroup => new
                                {
                                    X = myGroup.Key,
                                    Y = myGroup.GroupBy(topField1)
                                         .Select(
                                            z => new
                                            {
                                                Z = z.Key,
                                                V = valueField1(z)
                                            }),
                                    J = myGroup.GroupBy(topField2)
                                         .Select(
                                            z => new
                                            {
                                                Z = z.Key,
                                                V = valueField2(z)
                                            }),
                                    h = additionalFields(myGroup)
                                });
        }
    }
}
