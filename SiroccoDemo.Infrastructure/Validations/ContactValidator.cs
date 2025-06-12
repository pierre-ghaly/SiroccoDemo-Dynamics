using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SiroccoDemo.Infrastructure.Validations
{
    public class ContactValidator : IContactValidator
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void Validate(ContactInput contact)
        {
            if (contact == null)
                throw new InvalidInputException("Contact information is required.", nameof(contact), null);

            if (string.IsNullOrWhiteSpace(contact.LastName))
                throw new InvalidInputException("Contact last name is required.", nameof(contact.LastName), contact.LastName);

            if (string.IsNullOrWhiteSpace(contact.Email))
                throw new InvalidInputException("Contact email is required.", nameof(contact.Email), contact.Email);

            if (!EmailRegex.IsMatch(contact.Email))
                throw new InvalidInputException("Contact email format is invalid.", nameof(contact.Email), contact.Email);

            if (!string.IsNullOrEmpty(contact.FirstName) && contact.FirstName.Length > 50)
                throw new InvalidInputException("Contact first name cannot exceed 50 characters.", nameof(contact.FirstName), contact.FirstName);

            if (contact.LastName.Length > 50)
                throw new InvalidInputException("Contact last name cannot exceed 50 characters.", nameof(contact.LastName), contact.LastName);
        }
    }
}
