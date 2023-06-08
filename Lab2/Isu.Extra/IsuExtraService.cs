using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Entities;
using Isu.Extra.Enums;
using Isu.Extra.Exceptions;
using Isu.Extra.Interfaces;
using Isu.Models;

namespace Isu.Extra;

public class IsuExtraService : IIsuExtraService
{
    private readonly List<IsuExtraGroup> _groups = new List<IsuExtraGroup>();
    private readonly List<Course> _courses = new List<Course>();

    public bool IsSuchGrouoAlreadyExists(Group group) => _groups.Any(g => g.GroupName.Name == group.GroupName.Name);
    public bool IsSuchCourseAlreadyExists(Course course) => _courses.Any(c => c.Faculty == course.Faculty);

    public bool CheckIfStreamWitchSuchNumberExists(int streamNumber, Course course) =>
        streamNumber >= 0 && course.Streams.Count > streamNumber;

    public IsuExtraGroup AddGroup(GroupName name, List<Lesson> lessons, Faculty faculty)
    {
        var group = new IsuExtraGroup(name, new CourseNumber(name.Name[2] - '0'), lessons, faculty);
        if (IsSuchGrouoAlreadyExists(group))
            throw new GroupAlredyExistsException("such group already exists");
        _groups.Add(group);
        return group;
    }

    public IsuExtraStudent AddStudent(IsuExtraGroup group, string name)
    {
        ArgumentNullException.ThrowIfNull(group);
        var student = new IsuExtraStudent(name, group);
        group.AddStudent(student);
        return student;
    }

    public IsuExtraStudent GetStudent(Guid id)
    {
        return _groups.SelectMany(p => p.Students)
            .SingleOrDefault(s => s.Id == id) ?? throw new StudentNotFoundException("there is no such student");
    }

    public IsuExtraStudent? FindStudent(Guid id)
    {
        return _groups
            .SelectMany(group => group.Students)
            .FirstOrDefault(student => student.Id == id);
    }

    public IReadOnlyCollection<IsuExtraStudent> FindStudents(GroupName groupName)
    {
        return _groups
            .SingleOrDefault(group => group.GroupName == groupName)?.Students ?? Array.Empty<IsuExtraStudent>();
    }

    public IReadOnlyCollection<IsuExtraStudent> FindStudents(CourseNumber courseNumber)
    {
        return _groups
            .Where(group => group.CourseNumber == courseNumber)
            .SelectMany(p => p.Students).ToArray();
    }

    public IsuExtraGroup? FindGroup(GroupName groupName)
    {
        return _groups.FirstOrDefault(group => group.GroupName == groupName);
    }

    public IReadOnlyCollection<IsuExtraGroup> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.CourseNumber == courseNumber).ToArray();
    }

    public void ChangeStudentGroup(IsuExtraStudent student, IsuExtraGroup newGroup)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(newGroup);
        newGroup.MoveStudentFromOtherGroup(student.Group, student);
    }

    public Course CreateCourse(Faculty faculty)
    {
        ArgumentNullException.ThrowIfNull(faculty);
        var course = new Course(faculty);
        if (IsSuchCourseAlreadyExists(course))
            throw CourseException.CourseAlreadyExistException();
        _courses.Add(course);
        return course;
    }

    public CourseStream CreateStream(Course course, List<Lesson> lessons)
    {
        if (!IsSuchCourseAlreadyExists(course))
            throw CourseException.UndefinedCourseException();
        CourseStream courseStream = course.AddStream(lessons);
        return courseStream;
    }

    public void EnrollStudentToCourse(IsuExtraStudent student, Course course, int streamNumber)
    {
        if (FindStudent(student.Id) is null)
            throw EnrollException.StudentToEnrollNotFoundException();
        if (!IsSuchCourseAlreadyExists(course))
            throw CourseException.UndefinedCourseException();
        if (!CheckIfStreamWitchSuchNumberExists(streamNumber, course))
            throw StreamException.StreamDoesNotExistException();
        course.EnrollStudentOnCourse(student, streamNumber);
    }

    public void RemoveStudentFromCourse(IsuExtraStudent student, Course course, CourseStream stream)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (!IsSuchCourseAlreadyExists(course))
            throw CourseException.UndefinedCourseException();
        if (!CheckIfStreamWitchSuchNumberExists(stream.StreamNumber, course))
            throw StreamException.StreamDoesNotExistException();
        course.RemoveStudentFromStream(student, stream);
    }

    public IReadOnlyCollection<CourseStream> GetStreams(Course course)
    {
        if (!IsSuchCourseAlreadyExists(course))
            throw CourseException.UndefinedCourseException();
        return course.Streams;
    }

    public IReadOnlyCollection<IsuExtraStudent> GetStudentsByCourse(Course course)
    {
        if (!IsSuchCourseAlreadyExists(course))
            throw CourseException.UndefinedCourseException();
        var listOfStudents = new List<IsuExtraStudent>();
        listOfStudents.AddRange(course.Streams.SelectMany(s => s.Students));
        return listOfStudents.AsReadOnly();
    }

    public IReadOnlyCollection<IsuExtraStudent> GetStudentsWithNoCourse()
        => _groups.SelectMany(g => g.Students.Where(s => s.UniqueLessons.Count == 0))
            .ToList().AsReadOnly();
}