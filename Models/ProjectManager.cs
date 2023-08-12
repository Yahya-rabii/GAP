using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class ProjectManager : User
    {

        public int ProjectManagerID { get; set; }

        public List<Project> Projects { get; set; } // Foreign key property


        public ProjectManager() : base()
        {

            Projects = new List<Project>();
        }

        public ProjectManager(string? email, string? password, string? fn, string? ln) : base(email, password, fn, ln)
        {
            Projects = new List<Project>();


        }
    }
}
