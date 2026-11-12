using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client.NativeInterop;

namespace DemoMVC.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [Required]
        public string StudentId { get; set; } = string.Empty;
        [Required(ErrorMessage = " KHông để trống họ tên")]
        [RegularExpression(@"^[a-zA-Z0-9ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂÂĐỔỨỰỞỨỨỬ Nhật ký\s]+$",
        ErrorMessage = "Không được chứa ký tự đặc biệt hoặc mã script")]

        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string University { get; set; } = string.Empty;

        public int EntryYear{ get; set; }

        public string DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; } 

        public string DisciplineId { get; set; }
        [ForeignKey("DisciplineId")]
        public Discipline? Discipline { get; set; } 

        public string FormId { get; set; }
        [ForeignKey("FormId")]
        public FormOfTraining? FormOfTraining { get; set; } 
        



    }
}