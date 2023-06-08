using Isu.Extra.Enums;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class CourseStream
{
    private const int MaxStudentsInStreamAmount = 100;
    private const int MinAllowedAmountOfLessons = 1;
    private const int MinAllowedStreamNumber = 0;
    private readonly List<IsuExtraStudent> _students = new List<IsuExtraStudent>();
    private readonly List<Lesson> _lessons;

    public CourseStream(List<Lesson> lessons, int streamNumber)
    {
        if (lessons.Count < MinAllowedAmountOfLessons)
            throw new ArgumentNullException($"there are no lessons in presented timetable");
        _lessons = lessons;
        if (streamNumber < MinAllowedStreamNumber)
            throw StreamException.WrongStreamNumberException();
        StreamNumber = streamNumber;
    }

    public int StreamNumber { get; }
    public int GetStreamCapacity => MaxStudentsInStreamAmount;

    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    public IReadOnlyCollection<IsuExtraStudent> Students => _students.AsReadOnly();

    public void EnrollStudent(IsuExtraStudent student, Faculty faculty)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (_students.Count == MaxStudentsInStreamAmount)
            throw StreamException.StreamStudentAmountLimitException();
        ThrowIfLessonCrossExist(student);
        if (faculty == student.Group.Faculty)
            throw EnrollException.WrongFacultyDuringEnrollException();
        _students.Add(student);
        _lessons.ForEach(s => student.AddLessonToStudentTimetable(s, faculty));
    }

    public void RemoveStudentFromStream(IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);
        _students.Remove(student);
        RemoveCourseLessons(student);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        _lessons.Add(lesson);
    }

    private void ThrowIfLessonCrossExist(IsuExtraStudent student)
    {
        if (_lessons.Any(l => student.FindLessonWithSameTimeInGroupTimetable(l) is not null))
            throw LessonException.LessonCrossException();
        if (_lessons.Any(l => student.FindLessonWithSameTimeInStudentCoursesTimetable(l) is not null))
            throw LessonException.LessonCrossException();
    }

    private void RemoveCourseLessons(IsuExtraStudent student)
    {
        _lessons.ForEach(student.RemoveLessonFromStudentTimetable);
    }
}