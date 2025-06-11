using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using System.IO;

namespace SiroccoDemo.Infrastructure.Validations
{
    public class AccountValidator : IAccountValidator
    {
        public void Validate(AccountInput account)
        {
            if (string.IsNullOrEmpty(account.Name))
                throw new InvalidDataException("Account name is required.");
        }
    }
}
