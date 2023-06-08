using Isu.Exceptions;

namespace Isu.Extra.Exceptions;

public class GroupAlredyExistsException : IsuLogicException
{
    public GroupAlredyExistsException(string message)
        : base(message)
    {
    }
}