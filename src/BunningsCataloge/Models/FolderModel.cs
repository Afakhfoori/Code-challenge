using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BunningsCataloge.Models
{
    public class FolderModel
    {
        [Required]
        public string Input { get; set; }
        [Required]
        public string Output { get; set; }
    }
}
