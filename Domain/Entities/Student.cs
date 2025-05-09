using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }

    [MinLength(3),MaxLength(35), Required]
    public string FirstName { get; set; }
    [MinLength(3),MaxLength(35)]
    public string LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }
}
