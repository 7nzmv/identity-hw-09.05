namespace Domain.DTOs.Student;

public class CreateStudentDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }
}
