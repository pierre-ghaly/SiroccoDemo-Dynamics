using SiroccoDemo.Application.Models;

namespace SiroccoDemo.Application.Validations
{
    public interface INoteValidator
    {
        void Validate(NoteInput note);
    }
}
