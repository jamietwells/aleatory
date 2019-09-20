using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleatory
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T value)
        {
            yield return value;
        }

        public static T RandomElement<T>(this IEnumerable<T> values) => 
            RandomElement(values, new DataGenerators(new Random()).IntegerGenerator());

        internal static T RandomElement<T>(this IEnumerable<T> values, IntegerGenerator integerGenerator) => 
            values.ElementAt(integerGenerator.Between(0, values.Count()).InclusiveLower().ExclusiveUpper().Single());
    }
}