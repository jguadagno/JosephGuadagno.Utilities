using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace JosephGuadagnoNet.Utilities.Extensions
{
    /// <summary>
    ///     General purpose helper methods.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     For each element in the input, runs the specified action. Yields input elements.
        /// </summary>
        /// <typeparam name="TResult">Element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to perform on each element of the source.</param>
        /// <returns>Source elements.</returns>
        public static IEnumerable<TResult> ForEach<TResult>(this IEnumerable<TResult> source, Action<TResult> action)
        {
            Validation.CheckArgumentNotNull(source, "source");
            Validation.CheckArgumentNotNull(action, "action");

            return ForEachIterator(source, action);
        }

        private static IEnumerable<TResult> ForEachIterator<TResult>(IEnumerable<TResult> source, Action<TResult> action)
        {
            foreach (TResult element in source)
            {
                action(element);
                yield return element;
            }
        }

        /// <summary>
        ///     Determines whether a generic type definition is assignable from a type given some
        ///     generic type arguments. For instance,
        ///     <code>typeof(IEnumerable&lt;&gt;).IsGenericAssignableFrom(typeof(List&lt;int&gt;), out genericArguments)</code>
        ///     returns true with generic arguments { typeof(int) }.
        /// </summary>
        /// <param name="toType">Target generic type definition (to which the value would be assigned).</param>
        /// <param name="fromType">Source type (instance of which is being assigned)</param>
        /// <param name="genericArguments">
        ///     Returns generic type arguments required for the assignment to succeed
        ///     or null if no such assignment exists.
        /// </param>
        /// <returns>true if the type can be assigned; otherwise false</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"),
         SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0"),
         SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
        {
            Validation.CheckArgumentNotNull(toType, "toType");
            Validation.CheckArgumentNotNull(fromType, "fromType");

            if (!toType.IsGenericTypeDefinition ||
                fromType.IsGenericTypeDefinition)
            {
                // if 'toType' is not generic or 'fromType' is generic, the assignment pattern 
                // is not matched (e.g. toType<genericArguments>.IsAssignableFrom(fromType)
                // cannot be satisfied)
                genericArguments = null;
                return false;
            }

            if (toType.IsInterface)
            {
                // if the toType is an interface, simply look for the interface implementation in fromType
                foreach (Type interfaceCandidate in fromType.GetInterfaces())
                {
                    if (interfaceCandidate.IsGenericType && interfaceCandidate.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = interfaceCandidate.GetGenericArguments();
                        return true;
                    }
                }
            }
            else
            {
                // if toType is not an interface, check hierarchy for match
                while (fromType != null)
                {
                    if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = fromType.GetGenericArguments();
                        return true;
                    }
                    fromType = fromType.BaseType;
                }
            }
            genericArguments = null;
            return false;
        }

        /// <summary>
        ///     Allows you to call Distinct with a property name of the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="uniqueCheckerMethod">The unique checker method.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, object> uniqueCheckerMethod)
        {
            return source.Distinct(new GenericComparer<T>(uniqueCheckerMethod));
        }

        /// <summary>
        ///     Traverses an object hierarchy and return a flattened list of elements
        ///     based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of object in your collection.</typeparam>
        /// <param name="source">The collection of your topmost TSource objects.</param>
        /// <param name="selectorFunction">A predicate for choosing the objects you want.</param>
        /// <param name="getChildrenFunction">A function that fetches the child collection from an object.</param>
        /// <returns>A flattened list of objects which meet the criteria in selectorFunction.</returns>
        /// <remarks></remarks>
        public static IEnumerable<TSource> Map<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> selectorFunction,
            Func<TSource, IEnumerable<TSource>> getChildrenFunction)
        {
            // Add what we have to the stack

            var flattenedList = source.Where(selectorFunction);

            // Go through the input enumerable looking for children,
            // and add those if we have them
            foreach (TSource element in source)
            {
                flattenedList = flattenedList.Concat(
                    getChildrenFunction(element).Map(selectorFunction,
                        getChildrenFunction)
                    );
            }
            return flattenedList;
        }

        private class GenericComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, object> _uniqueCheckerMethod;

            public GenericComparer(Func<T, object> uniqueCheckerMethod)
            {
                _uniqueCheckerMethod = uniqueCheckerMethod;
            }

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                return _uniqueCheckerMethod(x).Equals(_uniqueCheckerMethod(y));
            }

            int IEqualityComparer<T>.GetHashCode(T obj)
            {
                return _uniqueCheckerMethod(obj).GetHashCode();
            }
        }
    }
}