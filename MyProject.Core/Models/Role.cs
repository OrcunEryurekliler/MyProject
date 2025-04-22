using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyProject.Core.Models
{
    public class Role : IdentityRole<int>
    {
        public required string Name { get; set; }

    }
}
