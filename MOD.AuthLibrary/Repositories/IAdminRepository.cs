using System;
using System.Collections.Generic;
using System.Text;
using MOD.AuthLibrary.Models;

namespace MOD.AuthLibrary.Repositories
{
    public interface IAdminRepository
    {
        bool AddSkill(Skill model);
    }
}
