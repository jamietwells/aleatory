using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleatory
{
    public class StringGenerator
    {
        private readonly CharacterGenerator _characterGenerator;
        private readonly IntegerGenerator _integerGenerator;

        internal StringGenerator(CharacterGenerator generator, IntegerGenerator integerGenerator)
        {
            this._characterGenerator = generator;
            this._integerGenerator = integerGenerator;
        }

        public StringGenerator LengthBetween(int minimum, int maximum) =>
            new StringGenerator(_characterGenerator, _integerGenerator.Between(minimum, maximum));

        public StringGenerator LengthInclusiveLower() =>
            new StringGenerator(_characterGenerator, _integerGenerator.InclusiveLower());

        public StringGenerator LengthExclusiveLower() =>
            new StringGenerator(_characterGenerator, _integerGenerator.ExclusiveLower());

        public StringGenerator LengthInclusiveUpper() =>
           new StringGenerator(_characterGenerator, _integerGenerator.InclusiveUpper());

        public StringGenerator LengthExclusiveUpper() =>
            new StringGenerator(_characterGenerator, _integerGenerator.ExclusiveUpper());

        public StringGenerator CharactersBetween(char minimum, char maximum) =>
            new StringGenerator(_characterGenerator.Between(minimum, maximum), _integerGenerator);

        public StringGenerator CharactersInclusiveLower() =>
            new StringGenerator(_characterGenerator.InclusiveLower(), _integerGenerator);

        public StringGenerator CharactersExclusiveLower() =>
            new StringGenerator(_characterGenerator.ExclusiveLower(), _integerGenerator);

        public StringGenerator CharactersInclusiveUpper() =>
           new StringGenerator(_characterGenerator.InclusiveUpper(), _integerGenerator);

        public StringGenerator CharactersExclusiveUpper() =>
            new StringGenerator(_characterGenerator.ExclusiveUpper(), _integerGenerator);

        public string Single() =>
            new string(Enumerable.Repeat(_characterGenerator.Single(), _integerGenerator.Single()).ToArray());

        public IEnumerable<string> Many() {
            while(true)
                yield return Single();
        }
    }
}
