﻿using System;

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
        
        public CharacterGenerator CharacterGenerator() => new CharacterGenerator(IntegerGenerator());

        public StringGenerator StringGenerator() => new StringGenerator(CharacterGenerator(), IntegerGenerator());
    }
}
