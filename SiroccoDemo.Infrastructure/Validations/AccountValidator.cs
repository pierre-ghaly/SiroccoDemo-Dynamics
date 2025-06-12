using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Domain.Exceptions;

namespace SiroccoDemo.Infrastructure.Validations
{
    public class AccountValidator : IAccountValidator
    {
        public void Validate(AccountInput account)
        {
            if (account == null)
                throw new InvalidInputException("Account information is required.", nameof(account), null);

            if (string.IsNullOrEmpty(account.Name))
                throw new InvalidInputException("Account name is required.", nameof(account.Name), account.Name);

            if (account.Name.Length > 160)
                throw new InvalidInputException("Account name cannot exceed 160 characters.", nameof(account.Name), account.Name);
        }
    }
}
