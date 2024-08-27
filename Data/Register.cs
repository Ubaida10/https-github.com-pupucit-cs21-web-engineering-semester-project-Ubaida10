using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CineTix.Data;

public class Register:IdentityUser
{
    public int Id { get; set; }

    [Display(Name = "Full name")]
    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; }

    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Email address is required")]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    
    [Display(Name = "I agree to the terms and conditions")]
    [Required(ErrorMessage = "You must agree to the terms and conditions")]
    public bool TermsAndConditions { get; set; }
    
    [Display(Name="First Name")]
    [Required(ErrorMessage = "First name is required")]
    [StringLength(20, ErrorMessage = "First name cannot exceed 20 characters")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    [StringLength(20, ErrorMessage = "Last name cannot exceed 20 characters")]
    public string LastName { get; set; }
    public bool RememberMe { get; set; }
}