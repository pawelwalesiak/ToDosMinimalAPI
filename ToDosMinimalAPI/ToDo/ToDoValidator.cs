using FluentValidation;

namespace ToDosMinimalAPI.ToDo
{
    public class ToDoValidator : AbstractValidator<ToDo>
    {
        public ToDoValidator()
        {
            RuleFor(t => t.Value)
                .NotEmpty()
                .MinimumLength(5)
                .WithMessage("Value of a todo must be more than 5 char");
        }
    }
}
