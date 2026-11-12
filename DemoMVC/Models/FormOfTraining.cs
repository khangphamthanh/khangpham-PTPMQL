using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Models
{
    [Table("FormOfTrainings")]
    //Hệ đào tạo: chính quy, tại chức, từ xa
    // [Index(nameof(FormCode), IsUnique = true)]
    public class FormOfTraining
    {
        [Key]
        [Required]
        public string? FormId { get; set; }
        public string FormName { get; set; } = string.Empty;
        
        public ICollection<Student>? Students { get; set; }
    }
}