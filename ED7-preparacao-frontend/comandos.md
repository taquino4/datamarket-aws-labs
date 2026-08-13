# ED7 - Preparando a Aplicação DataMarket para o Front-end

## 1. Acessar a instância EC2

```bash
ssh -i "datamarket-key.pem" ec2-user@IP_PUBLICO_DA_EC2
```

Entrar na pasta da aplicação:

```bash
cd ~/datamarket-api
```

---

## 2. Verificar se a API está funcionando

```bash
curl http://localhost:5000/health
```

Resultado esperado:

```json
{"status":"OK"}
```

Caso a API não esteja executando:

```bash
dotnet run
```

A aplicação deverá apresentar uma mensagem semelhante a:

```text
Now listening on: http://0.0.0.0:5000
```

---

## 3. Testar a conexão com o Amazon RDS PostgreSQL

Em um segundo terminal SSH:

```bash
curl -s http://localhost:5000/dbcheck | python3 -m json.tool
```

Resultado esperado:

```json
{
    "status": "OK",
    "database": "datamarket",
    "user": "datamarketadmin"
}
```

---

## 4. Consultar os produtos cadastrados

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

---

## 5. Cadastrar um produto de teste

```bash
curl -X POST http://localhost:5000/api/produtos \
  -H "Content-Type: application/json" \
  -d '{"id":0,"nome":"Teste ED7","preco":10.00,"estoque":1}'
```

Consultar novamente:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

---

## 6. Confirmar a persistência dos dados

Interromper a API:

```text
Ctrl+C
```

Executar novamente:

```bash
dotnet run
```

Em outro terminal:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

O produto `Teste ED7` deverá continuar aparecendo na lista.

---

## 7. Preparar a aplicação para o front-end

Interromper a API:

```text
Ctrl+C
```

Confirmar a pasta do projeto:

```bash
cd ~/datamarket-api
ls -l
```

---

## 8. Criar a pasta wwwroot

```bash
mkdir -p wwwroot
```

Conferir:

```bash
ls -l
```

---

## 9. Criar backup do Program.cs

```bash
cp Program.cs Program.cs.ed7.bak
```

---

## 10. Editar o Program.cs

```bash
nano Program.cs
```

Após:

```csharp
var app = builder.Build();
```

Adicionar:

```csharp
app.UseDefaultFiles();
app.UseStaticFiles();
```

Exemplo:

```csharp
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
```

Salvar no `nano`:

```text
Ctrl+O
Enter
Ctrl+X
```

---

## 11. Compilar a aplicação

```bash
dotnet build
```

Resultado esperado:

```text
Build succeeded.
```

---

## 12. Executar novamente a API

```bash
dotnet run
```

Resultado esperado:

```text
Now listening on: http://0.0.0.0:5000
```

---

## Resultado do ED7

Ao final deste estudo, a aplicação DataMarket continua utilizando:

```text
EC2 → API REST .NET → Npgsql → Amazon RDS PostgreSQL
```

e passa a estar preparada para servir arquivos estáticos por meio da pasta:

```text
wwwroot
```

O futuro front-end poderá utilizar arquivos HTML, CSS e JavaScript servidos pela própria aplicação ASP.NET Core.
