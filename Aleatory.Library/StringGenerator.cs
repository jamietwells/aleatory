using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleatory
{
    public interface ICharacterGroupBuilder
    {
        ICharacterGroupBuilder CharactersBetween(char minimum, char maximum);
        ICharacterGroupBuilder CharactersInclusiveLower();
        ICharacterGroupBuilder CharactersExclusiveLower();
        ICharacterGroupBuilder CharactersInclusiveUpper();
        ICharacterGroupBuilder CharactersExclusiveUpper();
        StringGenerator CompleteGroup();
    }

    public class StringGenerator
    {
        private class CharacterGroupGenerator : ICharacterGroupBuilder
        {
            private readonly CharacterGenerator _characterGenerator;
            private readonly StringGenerator _parent;

            public CharacterGroupGenerator(CharacterGenerator characterGenerator, StringGenerator parent)
            {
                _characterGenerator = characterGenerator;
                _parent = parent;
            }

            public ICharacterGroupBuilder CharactersBetween(char minimum, char maximum) =>
                new CharacterGroupGenerator(_characterGenerator.Between(minimum, maximum), _parent);

            public ICharacterGroupBuilder CharactersInclusiveLower() =>
                new CharacterGroupGenerator(_characterGenerator.InclusiveLower(), _parent);

            public ICharacterGroupBuilder CharactersExclusiveLower() =>
                new CharacterGroupGenerator(_characterGenerator.ExclusiveLower(), _parent);

            public ICharacterGroupBuilder CharactersInclusiveUpper() =>
               new CharacterGroupGenerator(_characterGenerator.InclusiveUpper(), _parent);

            public ICharacterGroupBuilder CharactersExclusiveUpper() =>
                new CharacterGroupGenerator(_characterGenerator.ExclusiveUpper(), _parent);

            internal char Single() => _characterGenerator.Single();

            public StringGenerator CompleteGroup() =>
                new StringGenerator(_characterGenerator, _parent._integerGenerator, _parent._characterGroupGenerators.Concat(this.Yield()));
        }

        private readonly CharacterGenerator _characterGenerator;
        private readonly IntegerGenerator _integerGenerator;
        private readonly IEnumerable<CharacterGroupGenerator> _characterGroupGenerators;

        internal StringGenerator(CharacterGenerator generator, IntegerGenerator integerGenerator) :
            this(generator, integerGenerator, Enumerable.Empty<CharacterGroupGenerator>())
        { }

        private StringGenerator(CharacterGenerator generator, IntegerGenerator integerGenerator, IEnumerable<CharacterGroupGenerator> characterGroupGenerators)
        {
            _characterGenerator = generator;
            _integerGenerator = integerGenerator;
            _characterGroupGenerators = characterGroupGenerators;
        }

        public ICharacterGroupBuilder AddCharacterGroup() =>
            new CharacterGroupGenerator(_characterGenerator, this);

        public StringGenerator LengthBetween(int minimum, int maximum) =>
            new StringGenerator(_characterGenerator, _integerGenerator.Between(minimum, maximum), _characterGroupGenerators);

        public StringGenerator LengthInclusiveLower() =>
            new StringGenerator(_characterGenerator, _integerGenerator.InclusiveLower(), _characterGroupGenerators);

        public StringGenerator LengthExclusiveLower() =>
            new StringGenerator(_characterGenerator, _integerGenerator.ExclusiveLower(), _characterGroupGenerators);

        public StringGenerator LengthInclusiveUpper() =>
           new StringGenerator(_characterGenerator, _integerGenerator.InclusiveUpper(), _characterGroupGenerators);

        public StringGenerator LengthExclusiveUpper() =>
            new StringGenerator(_characterGenerator, _integerGenerator.ExclusiveUpper(), _characterGroupGenerators);

        public string Single() {

            var haveCharacterGenerators = _characterGroupGenerators.Any();

            char GetChar()
            {
                if(haveCharacterGenerators)
                    return _characterGroupGenerators.RandomElement(_integerGenerator).Single();
                return _characterGenerator.Single();
            }

            return new string(Enumerable.Repeat<Func<char>>(GetChar, _integerGenerator.Single()).Select(f => f()).ToArray());
        }
            

        public IEnumerable<string> Many()
        {
            while (true)
                yield return Single();
        }
    }
}
