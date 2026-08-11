# ED2 - Comandos utilizados no laboratório

## Instalar o .NET 8

```bash
sudo dnf install -y dotnet-sdk-8.0
dotnet --version
```

## Criar a Minimal API DataMarket

```bash
mkdir datamarket-api
cd datamarket-api
dotnet new web
```

## Editar o Program.cs

```bash
nano Program.cs
```

O código completo do `Program.cs` está disponível nesta mesma pasta.

## Compilar a aplicação

```bash
dotnet build
```

## Executar a API

```bash
dotnet run
```

## Testar o endpoint de saúde

Em uma segunda conexão SSH com a EC2:

```bash
curl http://localhost:5000/health
```

Resultado esperado:

```json
{"status":"OK"}
```

## Consultar os produtos

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

Resultado esperado:

```json
[
    {
        "id": 1,
        "nome": "Notebook",
        "preco": 4500.00,
        "estoque": 5
    },
    {
        "id": 2,
        "nome": "Mouse",
        "preco": 120.00,
        "estoque": 20
    }
]
```
