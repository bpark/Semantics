using Microsoft.AspNetCore.Mvc;

namespace SemanticsWEB.Models
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IActionResult Handle(TCommand command);
    }
}