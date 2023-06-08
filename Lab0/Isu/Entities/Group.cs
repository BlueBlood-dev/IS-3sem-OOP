using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxStudentsAmount = 25;
    private readonly List<Student> _students;

    public Group(GroupName groupName, CourseNumber courseNumber)
    {
        GroupName = groupName;
        _students = new List<Student>();
        CourseNumber = courseNumber;
    }

    public GroupName GroupName { get; }

    public CourseNumber CourseNumber { get; }

    public void AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (_students.Count == MaxStudentsAmount)
        {
            throw new StudentAmountLimitException(
                $"amount of students in group must be less than {MaxStudentsAmount}");
        }

        student.SetGroup(this);
        _students.Add(student);
    }

    public IReadOnlyCollection<Student> GetStudents()
    {
        return _students.AsReadOnly();
    }

    public void MoveStudentFromOtherGroup(Group otherGroup, Student student)
    {
        ArgumentNullException.ThrowIfNull(otherGroup);
        ArgumentNullException.ThrowIfNull(student);
        AddStudent(student);
        otherGroup.RemoveStudent(student);
        student.SetGroup(this);
    }

    private void RemoveStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        _students.Remove(student);
    }
}