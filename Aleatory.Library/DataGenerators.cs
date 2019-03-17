using System;

namespace Aleatory
{
    public class DataGenerators
    {
        private readonly Random _random;

        public DataGenerators(Random random)
        {
            _random = random;
        }

        public IntegerGenerator IntegerGenerator() => new IntegerGenerator(_random);
    }
}
