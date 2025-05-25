namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session;
    public GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    {
        _session = session;
        logger.LogInformation("GetProductsQueryHandler initialized.");
    }
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}
