using FluentValidation;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Catalog.API.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    Guid Id,
    string? Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price
) : ICommand<CreateProductCommandResult>;

public sealed record CreateProductCommandResult(Guid Id);

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public sealed class CreateProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
{
    public async Task<CreateProductCommandResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        
        session.Store(product);
        
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductCommandResult(product.Id);
    }
}