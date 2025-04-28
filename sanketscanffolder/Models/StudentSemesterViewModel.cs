namespace sanketscanffolder.Models
{
    public class StudentSemesterViewModel
    {
        public List<Student> Students { get; set; }
        public List<Semester> Semesters { get; set; }
        public int? SelectedStudentId { get; set; }

    }
}
