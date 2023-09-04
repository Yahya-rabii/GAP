using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace GAP.Models
{


    // Define an enum for user roles
    public enum UserRole
    {
        Admin,
        Purchasing_department_manager,
        Purchasing_receptionist,
        Finance_department_manager,
        Quality_testing_department_manager,
        Project_Manager,
        Supplier
    }

    [SwaggerSchema("User", Title = "User")]
    public class User
    {


    

        [SwaggerSchema("UserID", Description = "The unique identifier for the user.")]
        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("Email", Description = "The email address of the user.")]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("Password", Description = "The password of the user.")]
        public string? Password { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("First Name", Description = "The first name of the user.")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(255)]
        [SwaggerSchema("Last Name", Description = "The last name of the user.")]
        public string? LastName { get; set; }

        [DefaultValue(false)]
        [SwaggerSchema("Is Admin", Description = "Indicates whether the user is an admin.")]
        public bool IsAdmin { get; set; } = false;

        [SwaggerSchema("Role", Description = "The role of the user.")]
        public UserRole Role { get; set; }

        public byte[]? ProfilePicture { get; set; }

        [SwaggerSchema("Profile Picture File Name", Description = "The file name of the user's profile picture.")]
        public string? ProfilePictureFileName { get; set; }

        [DefaultValue(false)]
        [SwaggerSchema("Has Custom Profile Picture", Description = "Indicates whether the user has a custom profile picture.")]
        public bool HasCustomProfilePicture { get; set; } = false;

        public User()
        {
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public User(string? email, string? password, string? fn, string? ln)
        {
            Email = email;
            Password = password;
            FirstName = fn;
            LastName = ln;
        }
    }
}
