using System.ComponentModel.DataAnnotations;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required.")]
    [StringLength(100, ErrorMessage = "Designation cannot exceed 100 characters.")]
    public string Designation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Join is required.")]
    [DataType(DataType.Date)]
    public DateTime DateOfJoin { get; set; }

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Salary is required.")]
    [Range(0, 1000000000, ErrorMessage = "Salary must be a positive value.")] // Example range
    public decimal Salary { get; set; }

    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
    public string State { get; set; } = string.Empty;
}