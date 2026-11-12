using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Models
{
    [Table("Disciplines")]
    //ngành học
    // [Index(nameof(DisciplineCode), IsUnique = true)]
    public class Discipline
    {
        [Key]
        [Required]
        public string? DisciplineId{ get; set; }
        public string DisciplineName { get; set; } = string.Empty;
        
        public ICollection<Student>? Students { get; set; }
    }
}