using FluentValidation;

namespace Catalog.API.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price
) : ICommand<UpdateProductCommandResult>;

public sealed record UpdateProductCommandResult(bool IsSuccess);

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(2, 150)
            .WithMessage("Name must be between 2 and 150 characters");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}

public sealed class UpdateProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
{
    public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        
        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        session.Update(product);
        
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductCommandResult(true);
    }
}