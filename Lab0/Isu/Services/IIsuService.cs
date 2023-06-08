using System;
using System.Collections.Generic;
using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group AddGroup(GroupName name);
    Student AddStudent(Group group, string name);

    Student GetStudent(Guid id);
    Student? FindStudent(Guid id);
    IReadOnlyCollection<Student> FindStudents(GroupName groupName);
    IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber);

    Group? FindGroup(GroupName groupName);
    IReadOnlyCollection<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}