using Isu.Extra.Enums;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Course
{
    private const int MinAllowedAmountOfLessons = 1;
    private const int StreamsCapacity = 5;
    private readonly List<CourseStream> _streams = new List<CourseStream>();

    public Course(Faculty faculty)
    {
        ArgumentNullException.ThrowIfNull(faculty);
        Faculty = faculty;
    }

    public Faculty Faculty { get; }

    public IReadOnlyCollection<CourseStream> Streams => _streams.AsReadOnly();

    public CourseStream AddStream(List<Lesson> lessons)
    {
        if (_streams.Count == StreamsCapacity)
            throw StreamException.StreamCapacityException();
        if (lessons.Count < MinAllowedAmountOfLessons)
            throw new ArgumentNullException($"there are no lessons in presented timetable");
        var stream = new CourseStream(lessons, _streams.Count);
        _streams.Add(stream);
        return stream;
    }

    public void EnrollStudentOnCourse(IsuExtraStudent student, int streamNumber)
    {
        ArgumentNullException.ThrowIfNull(student);
        _streams[streamNumber].EnrollStudent(student, Faculty);
    }

    public void RemoveStudentFromStream(IsuExtraStudent student, CourseStream stream)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(stream);
        stream.RemoveStudentFromStream(student);
    }
}