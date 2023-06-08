using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    private const int MinimalCourse = 1;
    private const int MaximalCourse = 4;

    public CourseNumber(int courseNumber)
    {
        IsCourseNumberValid(courseNumber);
        Number = courseNumber;
    }

    private int Number { get; }

    private void IsCourseNumberValid(int courseNumber)
    {
        if (courseNumber is < MinimalCourse or > MaximalCourse)
        {
            throw new InvalidCourseNumberException(
                $"course number must be less than 5 and bigger than 1, entered course number is {courseNumber}");
        }
    }
}