using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class CommonIsuService : IIsuService
{
    private readonly List<Group> _groups = new List<Group>();
    public Group AddGroup(GroupName name)
    {
        var group = new Group(name, new CourseNumber(name.Name[2] - '0'));
        _groups.Add(group);
        return group;
    }

    public Student AddStudent(Group @group, string name)
    {
        ArgumentNullException.ThrowIfNull(group);
        var student = new Student(name, group);
        group.AddStudent(student);
        return student;
    }

    public Student GetStudent(Guid id)
    {
       return _groups.SelectMany(p => p.GetStudents())
           .SingleOrDefault(s => s.Id == id) ?? throw new StudentNotFoundException("there is no such student");
    }

    public Student? FindStudent(Guid id)
    {
        return _groups
            .SelectMany(group => group.GetStudents())
            .FirstOrDefault(student => student.Id == id);
    }

    public IReadOnlyCollection<Student> FindStudents(GroupName groupName)
    {
        return _groups
            .SingleOrDefault(group => group.GroupName == groupName)?.GetStudents() ?? Array.Empty<Student>();
    }

    public IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber)
    {
        return _groups
            .Where(group => group.CourseNumber == courseNumber)
            .SelectMany(p => p.GetStudents()).ToArray();
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groups.FirstOrDefault(group => group.GroupName == groupName);
    }

    public IReadOnlyCollection<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.CourseNumber == courseNumber).ToArray();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(newGroup);
        newGroup.MoveStudentFromOtherGroup(student.Group, student);
    }
}