namespace Isu.Entities;

public class Student
{
    public Student(string name, Group group)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(group);
        Name = name;
        Group = group;
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }
    public Group Group { get; private set; }

    public void SetGroup(Group group)
    {
        Group = group;
    }
}