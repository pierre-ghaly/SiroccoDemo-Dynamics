using SiroccoDemo.Domain.Entities;

namespace SiroccoDemo.Application.Validations
{
    public interface INoteValidator
    {
        void Validate(Note note);
    }
}
