using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{
    [SwaggerSchema("ProjectManager", Title = "Project Manager")]
    public class ProjectManager : User
    {
        public int ProjectManagerID { get; set; }

        [SwaggerSchema("Project", Title = "Project")]
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
