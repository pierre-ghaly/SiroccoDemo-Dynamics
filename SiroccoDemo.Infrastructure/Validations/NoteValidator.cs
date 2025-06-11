using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using System.IO;

namespace SiroccoDemo.Infrastructure.Validations
{
    public class NoteValidator : INoteValidator
    {
        public void Validate(NoteInput note)
        {
            if (string.IsNullOrEmpty(note.Title))
                throw new InvalidDataException("Note title is required.");

            if (string.IsNullOrEmpty(note.Description))
                throw new InvalidDataException("Note description is required.");
        }
    }
}
