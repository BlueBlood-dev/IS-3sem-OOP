namespace Isu.Extra.Exceptions;

public class LessonException : Exception
{
    private LessonException(string message)
        : base(message)
    {
    }

    public static LessonException LessonCrossException()
    {
        return new LessonException("lessons are crossing");
    }

    public static LessonException WrongLessonOrderException()
    {
        return new LessonException("lesson with such order mustn't exist");
    }

    public static LessonException LessonNotFoundException()
    {
        return new LessonException("Can't find lesson to remove it from student's timetable");
    }
}