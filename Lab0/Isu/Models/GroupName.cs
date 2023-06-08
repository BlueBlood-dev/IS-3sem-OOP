using System;
using Isu.Exceptions;

namespace Isu.Models
{
    public class GroupName
    {
        private const int MaxNameLength = 5;
        public GroupName(string name)
        {
            IsGroupNameValid(name);
            Name = name;
        }

        public string Name { get; }

        private void IsGroupNameValid(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            if (name.Length != MaxNameLength)
                throw new InvalidGroupNameLengthException($"current GroupName length is {name.Length}, required is 5");

            if (!IsEnglishLetter(name[0]))
                throw new InvalidGroupNameFormat("The first symbol of GroupName must be an english letter");

            if (!int.TryParse(name.Substring(1), out _))
                throw new InvalidGroupNameFormat("After the english letter, there should be 4 numerals");
        }

        private bool IsEnglishLetter(char c)
        {
            return c is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
        }
    }
}