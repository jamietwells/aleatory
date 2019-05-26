using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleatory
{
    public class CharacterGenerator
    {
        private readonly IntegerGenerator _generator;

        private CharacterGenerator(IntegerGenerator generator)
        {
            this._generator = generator;
        }

        internal CharacterGenerator(IntegerGenerator generator, char upper = 'a', char lower = 'z')
            : this(generator.Between(upper, lower))
        { }

        public CharacterGenerator Between(char minimum, char maximum) =>
            new CharacterGenerator(_generator.Between(minimum, maximum));

        public CharacterGenerator InclusiveLower() =>
            new CharacterGenerator(_generator.InclusiveLower());

        public CharacterGenerator ExclusiveLower() =>
            new CharacterGenerator(_generator.ExclusiveLower());

        public CharacterGenerator InclusiveUpper() =>
           new CharacterGenerator(_generator.InclusiveUpper());

        public CharacterGenerator ExclusiveUpper() =>
            new CharacterGenerator(_generator.ExclusiveUpper());

        public char Single() =>
            Convert.ToChar(_generator.Single());

        public IEnumerable<char> Many() =>
            _generator.Many().Select(Convert.ToChar);
        
    }
}
