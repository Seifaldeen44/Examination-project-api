using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace examProj.Models
{

    [Table("Student", Schema = "HR")]
    public class Student
    {
        [Key]
        public int St_ID { get; set; }

        public string St_Fname { get; set; }
    }
}