namespace Backups.Exceptions;

public class PathException : Exception
{
    private PathException(string message)
        : base(message)
    {
    }

    public static PathException PathIsNullException() =>
        new PathException("Entered path is null");

    public static PathException WrongPathException() =>
        new PathException("Entered path is invalid");
}