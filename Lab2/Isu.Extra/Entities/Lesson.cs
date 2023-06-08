using Isu.Extra.Enums;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Lesson
{
    private const int MaxAllowedLessonOrder = 7;
    private const int MinAllowedLessonOrder = 0;

    public Lesson(int order, Day day)
    {
        if (order > MaxAllowedLessonOrder || order < MinAllowedLessonOrder)
            throw LessonException.WrongLessonOrderException();
        LessonTime = order + " " + day;
    }

    public string LessonTime { get; }
}