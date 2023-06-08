using Isu.Exceptions;
using Isu.Extra.Enums;
using Isu.Models;

namespace Isu.Extra.Entities;

public class IsuExtraGroup : Isu.Entities.Group
{
    private const int MaxStudentsAmount = 25;
    private const int MinAllowedAmountOfLessons = 1;
    private readonly List<IsuExtraStudent> _studentsList = new List<IsuExtraStudent>();
    private readonly List<Lesson> _groupLessons;

    public IsuExtraGroup(GroupName groupName, CourseNumber courseNumber, List<Lesson> groupLessons, Faculty faculty)
        : base(groupName, courseNumber)
    {
        if (groupLessons.Count < MinAllowedAmountOfLessons)
            throw new ArgumentNullException($"can't create group with no lessons");
        _groupLessons = groupLessons;
    }

    public Faculty Faculty { get; }
    public IReadOnlyCollection<IsuExtraStudent> Students => _studentsList.AsReadOnly();
    public IReadOnlyCollection<Lesson> GroupLessons => _groupLessons.AsReadOnly();

    public void AddStudent(IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (_studentsList.Count == MaxStudentsAmount)
        {
            throw new StudentAmountLimitException(
                $"amount of students in group must be less than {MaxStudentsAmount}");
        }

        student.SetGroup(this);
        _studentsList.Add(student);
    }

    public void MoveStudentFromOtherGroup(IsuExtraGroup otherGroup, IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(otherGroup);
        ArgumentNullException.ThrowIfNull(student);
        AddStudent(student);
        otherGroup.RemoveStudent(student);
        student.SetGroup(this);
    }

    private void RemoveStudent(IsuExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);
        _studentsList.Remove(student);
    }
}