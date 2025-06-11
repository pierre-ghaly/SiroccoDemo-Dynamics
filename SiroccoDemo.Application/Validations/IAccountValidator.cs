using SiroccoDemo.Application.Models;

namespace SiroccoDemo.Application.Validations
{
    public interface IAccountValidator
    {
        void Validate(AccountInput account);
    }
}
