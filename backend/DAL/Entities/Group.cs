namespace backend.DAL.Entities;

public class Group
{
    public Group()
    {
        TeacherGroups = new HashSet<TeacherGroup>();
        Students = new HashSet<User>();
        Subjects = new HashSet<Subject>();
        Works = new HashSet<Work>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<TeacherGroup> TeacherGroups { get; set; }

    public ICollection<User> Students { get; set; }

    public ICollection<Subject> Subjects { get; set; }

    public ICollection<Work> Works { get; set; }

    public ICollection<EducationMaterialGroup> EducationMaterials { get; set; }
}