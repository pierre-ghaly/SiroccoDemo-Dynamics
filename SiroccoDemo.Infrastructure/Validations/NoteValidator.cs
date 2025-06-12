using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Domain.Exceptions;

namespace SiroccoDemo.Infrastructure.Validations
{
    public class NoteValidator : INoteValidator
    {
        public void Validate(NoteInput note)
        {
            if (note == null)
                throw new InvalidInputException("Note information is required.", nameof(note), null);

            if (string.IsNullOrEmpty(note.Title))
                throw new InvalidInputException("Note title is required.", nameof(note.Title), note.Title);

            if (string.IsNullOrEmpty(note.Description))
                throw new InvalidInputException("Note description is required.", nameof(note.Description), note.Description);

            if (note.Title.Length > 500)
                throw new InvalidInputException("Note title cannot exceed 500 characters.", nameof(note.Title), note.Title);

            if (note.Description.Length > 100000)
                throw new InvalidInputException("Note description cannot exceed 100,000 characters.", nameof(note.Description), note.Description);
        }
    }
}
