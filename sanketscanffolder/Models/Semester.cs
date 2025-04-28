using System.ComponentModel.DataAnnotations.Schema;

namespace sanketscanffolder.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public string Subjet { get; set; }
        public int Mark1 { get; set; }
        public string Subjet2 { get; set; }
        public int Mark2 { get; set; }
        public int Total { get; set; }

        [ForeignKey("Id")]
        public int studId { get; set; }


    }
}
