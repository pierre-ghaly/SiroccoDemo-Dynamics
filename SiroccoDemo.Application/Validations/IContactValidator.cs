using SiroccoDemo.Domain.Entities;

namespace SiroccoDemo.Application.Validations
{
    public interface IContactValidator
    {
        void Validate(Contact contact);
    }
}
