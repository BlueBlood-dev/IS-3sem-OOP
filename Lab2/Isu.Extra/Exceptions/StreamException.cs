namespace Isu.Extra.Exceptions;

public class StreamException : Exception
{
    private StreamException(string message)
        : base(message)
    {
    }

    public static StreamException StreamDoesNotExistException()
    {
        return new StreamException("such stream doesn't exist");
    }

    public static StreamException StreamStudentAmountLimitException()
    {
        return new StreamException("stream overflow");
    }

    public static StreamException WrongStreamNumberException()
    {
        return new StreamException("there is no stream witch such number");
    }

    public static StreamException StreamCapacityException()
    {
        return new StreamException("Can't add stream, this course is full of streams");
    }
}