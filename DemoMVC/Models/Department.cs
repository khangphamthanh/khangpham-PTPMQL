using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client.NativeInterop;

namespace DemoMVC.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [Required]
        public string? DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public ICollection<Student>? Students { get; set; }
    }
}