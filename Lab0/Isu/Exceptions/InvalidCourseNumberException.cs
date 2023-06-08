namespace Isu.Exceptions;

    public class InvalidCourseNumberException : IsuLogicException
    {
        public InvalidCourseNumberException(string message)
            : base(message)
        {
        }
    }
