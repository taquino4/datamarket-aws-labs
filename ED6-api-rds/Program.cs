using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString =
    builder.Configuration.GetConnectionString("DataMarketDb")
    ?? throw new InvalidOperationException(
        "Connection string DataMarketDb não encontrada.");

// ---------------------------------------------------------
// Health Check da API
// ---------------------------------------------------------

app.MapGet("/health", () =>
    Results.Ok(new { status = "OK" }));

// ---------------------------------------------------------
// Consulta de produtos no Amazon RDS PostgreSQL
// ---------------------------------------------------------

app.MapGet("/api/produtos", async () =>
{
    var produtos = new List<Produto>();

    await using var connection =
        new NpgsqlConnection(connectionString);

    await connection.OpenAsync();

    await using var command =
        new NpgsqlCommand(
            "SELECT id, nome, preco, estoque FROM produtos ORDER BY id;",
            connection);

    await using var reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        produtos.Add(new Produto(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetDecimal(2),
            reader.GetInt32(3)
        ));
    }

    return Results.Ok(produtos);
});

// ---------------------------------------------------------
// Cadastro de produtos no Amazon RDS PostgreSQL
// ---------------------------------------------------------

app.MapPost("/api/produtos", async (Produto produto) =>
{
    await using var connection =
        new NpgsqlConnection(connectionString);

    await connection.OpenAsync();

    await using var command =
        new NpgsqlCommand(
            @"INSERT INTO produtos (nome, preco, estoque)
              VALUES (@nome, @preco, @estoque)
              RETURNING id;",
            connection);

    command.Parameters.AddWithValue("nome", produto.Nome);
    command.Parameters.AddWithValue("preco", produto.Preco);
    command.Parameters.AddWithValue("estoque", produto.Estoque);

    var id = (int)(await command.ExecuteScalarAsync())!;

    var novoProduto = produto with { Id = id };

    return Results.Created(
        $"/api/produtos/{id}",
        novoProduto);
});

// ---------------------------------------------------------
// Teste da conexão da API com o PostgreSQL
// ---------------------------------------------------------

app.MapGet("/dbcheck", async () =>
{
    await using var connection =
        new NpgsqlConnection(connectionString);

    await connection.OpenAsync();

    await using var command =
        new NpgsqlCommand(
            "SELECT current_database(), current_user;",
            connection);

    await using var reader =
        await command.ExecuteReaderAsync();

    await reader.ReadAsync();

    return Results.Ok(new
    {
        status = "OK",
        database = reader.GetString(0),
        user = reader.GetString(1)
    });
});

// ---------------------------------------------------------
// API disponível na porta TCP 5000
// ---------------------------------------------------------

app.Run("http://0.0.0.0:5000");

public record Produto(
    int Id,
    string Nome,
    decimal Preco,
    int Estoque);
