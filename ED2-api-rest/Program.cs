var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var produtos = new List<Produto>
{
    new Produto(1, "Notebook", 4500.00m, 5),
    new Produto(2, "Mouse", 120.00m, 20)
};

app.MapGet("/health", () =>
    Results.Ok(new { status = "OK" }));

app.MapGet("/api/produtos", () =>
    Results.Ok(produtos));

app.MapPost("/api/produtos", (Produto produto) =>
{
    var novoProduto = produto with
    {
        Id = produtos.Count == 0
            ? 1
            : produtos.Max(p => p.Id) + 1
    };

    produtos.Add(novoProduto);

    return Results.Created(
        $"/api/produtos/{novoProduto.Id}",
        novoProduto);
});

app.Run("http://0.0.0.0:5000");

public record Produto(
    int Id,
    string Nome,
    decimal Preco,
    int Estoque);
