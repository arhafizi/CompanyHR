using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects;
public record CompanyCreationDto {

    [Required(ErrorMessage = "company name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Address is a required field.")]
    public string? Address { get; init; }
    
    [Required(ErrorMessage = "Country is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Country is 30 characters.")]
    public string? Country { get; init; }

    public IEnumerable<EmployeeCreationDto>? Employees {  get; init; }
}