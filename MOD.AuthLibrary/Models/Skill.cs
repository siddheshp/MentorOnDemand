using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MOD.AuthLibrary.Models
{
    public class Skill
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public MODUser AdminUser { get; set; }
    }
}
