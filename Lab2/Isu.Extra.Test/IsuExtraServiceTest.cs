using Isu.Extra.Entities;
using Isu.Extra.Enums;
using Isu.Extra.Exceptions;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private readonly IsuExtraService _service = new IsuExtraService();

    [Fact]
    public void EnrollStudent_StudentHasCourseLessons()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        Course course = _service.CreateCourse(Faculty.FTMI);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        course.EnrollStudentOnCourse(student, stream.StreamNumber);
        Assert.NotEqual(0, student.StudentCourses.Count);
    }

    [Fact]
    public void RemoveStudentFromCourse_StudentRemoved()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        Course course = _service.CreateCourse(Faculty.FTMI);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        course.EnrollStudentOnCourse(student, stream.StreamNumber);
        course.RemoveStudentFromStream(student, stream);
        Console.WriteLine(student.UniqueLessons.Count);
        Assert.Equal(0, student.UniqueLessons.Count);
    }

    [Fact]
    public void GetStreamsByCourse_ReturnsStreams()
    {
        Course course = _service.CreateCourse(Faculty.FTMI);
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        lessonsStream.Add(new Lesson(3, Day.Thursday));
        CourseStream stream1 = course.AddStream(lessonsStream);
        Assert.IsAssignableFrom<IReadOnlyCollection<CourseStream>>(_service.GetStreams(course));
    }

    [Fact]
    public void GetStudentsByCourse_ReturnsStudents()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        IsuExtraStudent student1 = _service.AddStudent(group, "Gnom Gnomi4");
        Course course = _service.CreateCourse(Faculty.FTMI);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        course.EnrollStudentOnCourse(student, stream.StreamNumber);
        course.EnrollStudentOnCourse(student1, stream.StreamNumber);
        Assert.IsAssignableFrom<IReadOnlyCollection<IsuExtraStudent>>(_service.GetStudentsByCourse(course));
    }

    [Fact]
    public void GetStudentsWithNoCourse_ReturnsStudentsWithNoCourse()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        IsuExtraStudent student1 = _service.AddStudent(group, "Gnom Gnomi4");
        IsuExtraStudent lazyStudent = _service.AddStudent(group, "Sidorkin");
        Course course = _service.CreateCourse(Faculty.FTMI);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        course.EnrollStudentOnCourse(student, stream.StreamNumber);
        course.EnrollStudentOnCourse(student1, stream.StreamNumber);
        Assert.Equal(lazyStudent.Id, _service.GetStudentsWithNoCourse().ElementAt(0).Id);
    }

    [Fact]
    public void OverFlowStream_Throws_StreamException()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        Course course = _service.CreateCourse(Faculty.FTMI);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        for (int i = 0; i < stream.GetStreamCapacity; i++)
        {
            course.EnrollStudentOnCourse(new IsuExtraStudent("Thomas Shelby Clone", group), stream.StreamNumber);
        }

        Assert.Throws<StreamException>(() =>
        {
            course.EnrollStudentOnCourse(student, stream.StreamNumber);
        });
    }

    [Fact]
    public void WrongFacultyDueEnrolling_Throws_EnrollException()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        Course course = _service.CreateCourse(Faculty.TINT);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        Assert.Throws<EnrollException>(() =>
        {
            course.EnrollStudentOnCourse(student, stream.StreamNumber);
        });
    }

    [Fact]
    public void LessonCrossHappened_Throws_LessonException()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Monday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        IsuExtraGroup group = _service.AddGroup(new GroupName("M3205"), lessons, Faculty.TINT);
        IsuExtraStudent student = _service.AddStudent(group, "Thomas Shelby");
        Course course = _service.CreateCourse(Faculty.TINT);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        Assert.Throws<LessonException>(() =>
        {
            course.EnrollStudentOnCourse(student, stream.StreamNumber);
        });
    }

    [Fact]
    public void UnknownStudentTriesToEnroll_Throws_EnrollException()
    {
        var lesson1 = new Lesson(2, Day.Monday);
        var lesson2 = new Lesson(1, Day.Monday);
        var lessons = new List<Lesson>();
        var lesson1Stream = new Lesson(2, Day.Tuesday);
        var lesson2Stream = new Lesson(1, Day.Tuesday);
        var lessonsStream = new List<Lesson>();
        lessons.Add(lesson1);
        lessons.Add(lesson2);
        Course course = _service.CreateCourse(Faculty.TINT);
        lessonsStream.Add(lesson1Stream);
        lessonsStream.Add(lesson2Stream);
        CourseStream stream = course.AddStream(lessonsStream);
        Assert.Throws<EnrollException>(() =>
        {
            course.EnrollStudentOnCourse(
                new IsuExtraStudent(
                    "noname",
                    new IsuExtraGroup(new GroupName("M3205"), new CourseNumber(2), lessons, Faculty.TINT)),
                stream.StreamNumber);
        });
    }
}