using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GAP.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? Password { get; set; }

        [Required]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string? LastName { get; set; }


        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false;

        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureFileName { get; set; }
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
