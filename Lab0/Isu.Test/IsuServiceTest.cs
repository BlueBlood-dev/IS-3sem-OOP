using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test
{
    public class IsuServiceTest
    {
        private readonly IIsuService _service = new CommonIsuService();

        [Fact]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var name = new GroupName("M3105");
            Group @group = _service.AddGroup(name);
            Student student = _service.AddStudent(group, "Thomas Shelby");
            Assert.Contains(student, group.GetStudents());
            Assert.Equal(student.Group, group);
        }

        [Fact]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            var name = new GroupName("M3105");
            Group @group = _service.AddGroup(name);
            for (int i = 0; i < 25; i++)
            {
                Student student = _service.AddStudent(group, "Thomas Shelby");
            }

            Assert.Throws<StudentAmountLimitException>(() => _service.AddStudent(group, "Thomas Shelby"));
        }

        [Fact]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Throws<InvalidGroupNameFormat>(() => new GroupName("11111"));
            Assert.Throws<InvalidGroupNameLengthException>(() => new GroupName("M310555"));
        }

        [Fact]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var name = new GroupName("M3105");
            Group @group = _service.AddGroup(name);
            var otherName = new GroupName("M3100");
            Group otherGroup = _service.AddGroup(otherName);
            Student student = _service.AddStudent(group, "Thomas Shelby");
            _service.ChangeStudentGroup(student, otherGroup);
            Assert.Equal(student.Group, otherGroup);
        }
    }
}