# ED6 - Integrando a API .NET com o Amazon RDS PostgreSQL

## 1. Acessar a pasta da aplicação

```bash
cd ~/datamarket-api
```

## 2. Instalar o driver PostgreSQL para .NET

```bash
dotnet add package Npgsql
```

Confira:

```bash
dotnet list package
```

## 3. Fazer backup do appsettings.json

```bash
cp appsettings.json appsettings.json.bak
```

Edite:

```bash
nano appsettings.json
```

Configure:

```json
{
  "ConnectionStrings": {
    "DataMarketDb": "Host=ENDPOINT_DO_RDS;Port=5432;Database=datamarket;Username=datamarketadmin;Password=SUA_SENHA"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> Substitua `ENDPOINT_DO_RDS` pelo endpoint da sua instância RDS e `SUA_SENHA` pela senha definida no laboratório.

> Não publique credenciais reais no GitHub.

## 4. Testar se o projeto continua compilando

```bash
dotnet build
```

## 5. Fazer backup do Program.cs

```bash
cp Program.cs Program.cs.bak
```

Edite:

```bash
nano Program.cs
```

Adicione no início:

```csharp
using Npgsql;
```

Após a criação do `app`, adicione:

```csharp
var connectionString =
    builder.Configuration.GetConnectionString("DataMarketDb")
    ?? throw new InvalidOperationException(
        "Connection string DataMarketDb não encontrada.");
```

## 6. Criar o endpoint de teste /dbcheck

Antes de:

```csharp
app.Run("http://0.0.0.0:5000");
```

adicione:

```csharp
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
```

Compile:

```bash
dotnet build
```

Execute:

```bash
dotnet run
```

Em outro terminal SSH:

```bash
curl -s http://localhost:5000/dbcheck | python3 -m json.tool
```

## 7. Alterar o GET /api/produtos

Pare a aplicação:

```text
Ctrl+C
```

Abra novamente:

```bash
nano Program.cs
```

Substitua o endpoint GET antigo por:

```csharp
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
```

Não remova ainda a coleção em memória, pois o POST antigo ainda depende dela.

Compile:

```bash
dotnet build
```

Execute:

```bash
dotnet run
```

Em outro terminal:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

## 8. Alterar o POST /api/produtos

Pare novamente a aplicação:

```text
Ctrl+C
```

Abra:

```bash
nano Program.cs
```

Substitua o endpoint POST antigo por:

```csharp
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
```

Agora remova a coleção em memória:

```csharp
var produtos = new List<Produto>
{
    new Produto(1, "Notebook", 4500.00m, 5),
    new Produto(2, "Mouse", 120.00m, 20)
};
```

Compile:

```bash
dotnet build
```

Execute:

```bash
dotnet run
```

## 9. Consultar os produtos no RDS

Em outro terminal:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

## 10. Inserir um produto via API

```bash
curl -X POST http://localhost:5000/api/produtos \
  -H "Content-Type: application/json" \
  -d '{"id":0,"nome":"Teclado","preco":250.00,"estoque":10}'
```

Consulte novamente:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

## 11. Testar a persistência

No terminal onde a API está executando:

```text
Ctrl+C
```

Inicie novamente:

```bash
dotnet run
```

No outro terminal:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

O produto `Teclado` deverá continuar sendo retornado, comprovando que os dados estão persistidos no Amazon RDS PostgreSQL.
