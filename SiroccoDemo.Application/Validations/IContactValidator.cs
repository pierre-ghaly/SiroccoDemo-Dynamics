using SiroccoDemo.Application.Models;

namespace SiroccoDemo.Application.Validations
{
    public interface IContactValidator
    {
        void Validate(ContactInput contact);
    }
}
