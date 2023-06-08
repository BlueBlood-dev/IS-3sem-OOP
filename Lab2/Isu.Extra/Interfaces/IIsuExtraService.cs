using Isu.Extra.Entities;
using Isu.Extra.Enums;
using Isu.Models;

namespace Isu.Extra.Interfaces;

public interface IIsuExtraService
{
    IsuExtraGroup AddGroup(GroupName name, List<Lesson> lessons, Faculty faculty);
    IsuExtraStudent AddStudent(IsuExtraGroup group, string name);
    IsuExtraStudent GetStudent(Guid id);
    IsuExtraStudent? FindStudent(Guid id);
    IReadOnlyCollection<IsuExtraStudent> FindStudents(GroupName groupName);

    IReadOnlyCollection<IsuExtraStudent> FindStudents(CourseNumber courseNumber);
    IsuExtraGroup? FindGroup(GroupName groupName);
    IReadOnlyCollection<IsuExtraGroup> FindGroups(CourseNumber courseNumber);
    void ChangeStudentGroup(IsuExtraStudent student, IsuExtraGroup newGroup);
    Course CreateCourse(Faculty faculty);
    CourseStream CreateStream(Course course, List<Lesson> lessons);
    void EnrollStudentToCourse(IsuExtraStudent student, Course course, int streamNumber);
    void RemoveStudentFromCourse(IsuExtraStudent student, Course course, CourseStream stream);
    IReadOnlyCollection<CourseStream> GetStreams(Course course);
    IReadOnlyCollection<IsuExtraStudent> GetStudentsByCourse(Course course);

    IReadOnlyCollection<IsuExtraStudent> GetStudentsWithNoCourse();
}