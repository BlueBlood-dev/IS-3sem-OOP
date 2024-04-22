namespace Isu.Extra.Exceptions;

//dsdsdsdsdsdsdsds
public class EnrollException : Exception
{
    public EnrollException(string message)
        : base(message)
    {
    }
// dsdsdsds
    public static EnrollException StudentToEnrollNotFoundException()
    {
        return new EnrollException("student wasn't found");
    }

    public static EnrollException WrongFacultyDuringEnrollException()
    {
        return new EnrollException("can't enroll student to this course as faculties are similar");
    }
}