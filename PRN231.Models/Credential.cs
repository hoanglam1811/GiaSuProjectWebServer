namespace PRN231.Models;

public partial class Credential : BaseModel
{
    public int TutorId { get; set; }

    public int? SubjectId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Image { get; set; }

    public User Tutor { get; set; }

    public Subject? Subject { get; set; }
}
