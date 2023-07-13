﻿using System.ComponentModel.DataAnnotations;
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

      

        public bool IsAdmin { get; set; }


    }
}
