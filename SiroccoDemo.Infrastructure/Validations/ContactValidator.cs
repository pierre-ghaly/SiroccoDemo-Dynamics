using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using System.IO;
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
            if (string.IsNullOrWhiteSpace(contact.LastName))
                throw new InvalidDataException("Contact last name is required.");

            if (string.IsNullOrWhiteSpace(contact.Email) || !EmailRegex.IsMatch(contact.Email))
                throw new InvalidDataException("Valid email is required.");
        }
    }
}
