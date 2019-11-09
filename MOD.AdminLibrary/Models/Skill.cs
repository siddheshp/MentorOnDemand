using MOD.AuthLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOD.AdminLibrary.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MODUser AdminUser { get; set; } 
    }
}
