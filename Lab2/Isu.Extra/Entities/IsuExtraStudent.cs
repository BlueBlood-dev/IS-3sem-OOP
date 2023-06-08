using Isu.Extra.Enums;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class IsuExtraStudent
{
    private const int MaxAllowedAmountOfCourses = 2;
    private readonly List<Lesson> _uniqueStudentLessons;
    private readonly List<Faculty> _studentCourses = new List<Faculty>();

    public IsuExtraStudent(string name, IsuExtraGroup group)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(group);
        Name = name;
        Group = group;
        _uniqueStudentLessons = new List<Lesson>();
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }
    public IsuExtraGroup Group { get; private set; }

    public IReadOnlyCollection<Faculty> StudentCourses => _studentCourses.AsReadOnly();

    public IReadOnlyCollection<Lesson> UniqueLessons => _uniqueStudentLessons.AsReadOnly();
    public void SetGroup(IsuExtraGroup group) => Group = group;

    public void AddLessonToStudentTimetable(Lesson lesson, Faculty faculty)
    {
        if (FindLessonWithSameTimeInGroupTimetable(lesson) is not null &&
            FindLessonWithSameTimeInStudentCoursesTimetable(lesson) is not null)
            throw LessonException.LessonCrossException();
        _uniqueStudentLessons.Add(lesson);
        if (_studentCourses.Count == MaxAllowedAmountOfCourses)
            throw CourseException.StudentPretendsToHaveMoreCoursesThanAllowed();
        if (!_studentCourses.Contains(faculty))
            _studentCourses.Add(faculty);
    }

    public void RemoveLessonFromStudentTimetable(Lesson lesson)
    {
        if (FindLessonWithSameTimeInStudentCoursesTimetable(lesson) is null)
            throw LessonException.LessonNotFoundException();
        _uniqueStudentLessons.Remove(_uniqueStudentLessons.Find(l => l.LessonTime == lesson.LessonTime) ??
                                     throw new ArgumentNullException());
    }

    public Lesson? FindLessonWithSameTimeInGroupTimetable(Lesson lesson) =>
        Group.GroupLessons.FirstOrDefault(l => l.LessonTime == lesson.LessonTime);

    public Lesson? FindLessonWithSameTimeInStudentCoursesTimetable(Lesson lesson) =>
        _uniqueStudentLessons.FirstOrDefault(l => l.LessonTime == lesson.LessonTime);
}