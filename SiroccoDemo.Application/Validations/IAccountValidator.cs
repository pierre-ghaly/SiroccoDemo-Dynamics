using SiroccoDemo.Domain.Entities;

namespace SiroccoDemo.Application.Validations
{
    public interface IAccountValidator
    {
        void Validate(Account account);
    }
}
