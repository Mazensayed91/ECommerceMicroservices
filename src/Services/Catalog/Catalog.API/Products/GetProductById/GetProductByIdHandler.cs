namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;
    public GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
        _logger.LogInformation("GetProductByIdQueryHandler initialized.");
    }
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);
        if (product is null)
        {
            _logger.LogWarning("Product with ID {Id} not found.", query.Id);
            throw new ProductNotFoundException(query.Id);
        }
        _logger.LogInformation("Product with ID {Id} retrieved successfully.", query.Id);
        return new GetProductByIdResult(product);
    }
}

