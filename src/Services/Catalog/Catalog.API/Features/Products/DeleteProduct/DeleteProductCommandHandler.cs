using FluentValidation;

namespace Catalog.API.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(
    Guid Id
) : ICommand<DeleteProductCommandResult>;

public sealed record DeleteProductCommandResult(bool IsSuccess);

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}

public sealed class DeleteProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
{
    public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.Id);
        
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProductCommandResult(true);
    }
}