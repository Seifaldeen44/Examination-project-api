using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examProj.Models
{
    [Table("Instructor", Schema = "HR")]
    public class Instructor
    {
        [Key]
        public int Ins_ID { get; set; }

        public string Ins_Name { get; set; }
    }

}
