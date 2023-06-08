namespace Isu.Extra.Exceptions;

public class CourseException : Exception
{
    public CourseException(string message)
        : base(message)
    {
    }

    public static CourseException CourseAlreadyExistException()
    {
        return new CourseException("such course already exists");
    }

    public static CourseException UndefinedCourseException()
    {
        return new CourseException("there is no such course");
    }

    public static CourseException StudentPretendsToHaveMoreCoursesThanAllowed()
    {
        return new CourseException("student mustn't have so many courses");
    }
}