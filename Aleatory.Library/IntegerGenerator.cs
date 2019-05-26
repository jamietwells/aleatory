using System;
using System.Collections.Generic;

namespace Aleatory
{
    public class IntegerGenerator
    {
        private readonly Random _random;
        private readonly int _lowerBound;
        private readonly int _upperBound;
        private readonly bool _inclusiveLower;
        private readonly bool _inclusiveHigher;

        internal IntegerGenerator(Random random) : this(random, int.MinValue, int.MaxValue, true, false)
        { }

        private IntegerGenerator(Random random, int lowerBound, int upperBound, bool inclusiveLower, bool inclusiveHigher)
        {
            _random = random;
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            _inclusiveLower = inclusiveLower;
            _inclusiveHigher = inclusiveHigher;
        }

        public IntegerGenerator Between(int minimum, int maximum) =>
            new IntegerGenerator(_random, minimum, maximum, _inclusiveLower, _inclusiveHigher);

        public IntegerGenerator InclusiveLower() =>
            new IntegerGenerator(_random, _lowerBound, _upperBound, true, _inclusiveHigher);

        public IntegerGenerator ExclusiveLower() =>
            new IntegerGenerator(_random, _lowerBound, _upperBound, false, _inclusiveHigher);

        public IntegerGenerator InclusiveUpper() =>
            new IntegerGenerator(_random, _lowerBound, _upperBound, _inclusiveLower, true);

        public IntegerGenerator ExclusiveUpper() =>
            new IntegerGenerator(_random, _lowerBound, _upperBound, _inclusiveLower, false);

        public int Single() =>
            _random.Next(_inclusiveLower ? _lowerBound : _lowerBound + 1, _inclusiveHigher ? _upperBound + 1: _upperBound);

        public IEnumerable<int> Many()
        {
            while (true)
                yield return Single();
        }
    }
}
