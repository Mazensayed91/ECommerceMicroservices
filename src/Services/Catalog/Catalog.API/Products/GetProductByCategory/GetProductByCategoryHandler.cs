namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByCategoryQueryHandler> _logger;
    public GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
        _logger.LogInformation("GetProductByCategoryQueryHandler initialized.");
    }
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>()
            .Where(p => p.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken);
        if (!products.Any())
        {
            _logger.LogWarning("No products found for category {Category}.", query.Category);
        }
        else
        {
            _logger.LogInformation("{Count} products found for category {Category}.", products.Count, query.Category);
        }
        return new GetProductByCategoryResult(products);
    }
}