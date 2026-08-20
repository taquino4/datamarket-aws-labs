# ED11 - Hospedando o Front-end da DataMarket no Amazon S3

## 1. Criar o bucket S3

No Console AWS:

```text
S3 → Buckets → Criar bucket
```

Nome sugerido:

```text
datamarket-frontend-SEU-NOME
```

Mantenha:

```text
Tipo de bucket: Propósito geral
Namespace: Global
Região: mesma região utilizada no ambiente DataMarket
```

---

## 2. Habilitar hospedagem de site estático

No bucket:

```text
Propriedades → Hospedagem de site estático → Editar
```

Configure:

```text
Hospedagem de site estático: Habilitar
Tipo de hospedagem: Hospedar um site estático
Documento de índice: index.html
```

Depois de salvar, copie o endpoint do site.

Exemplo:

```text
http://datamarket-frontend-SEU-NOME.s3-website-us-east-1.amazonaws.com
```

---

## 3. Configurar leitura pública

No bucket:

```text
Permissões → Bloquear acesso público → Editar
```

Desative:

```text
Bloquear todo o acesso público
```

Confirme a alteração.

Depois:

```text
Permissões → Política do bucket → Editar
```

Use:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "PublicReadGetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Action": "s3:GetObject",
      "Resource": "arn:aws:s3:::datamarket-frontend-SEU-NOME/*"
    }
  ]
}
```

Substitua:

```text
SEU-NOME
```

pelo nome utilizado na criação do bucket.

---

## 4. Conferir os arquivos do front-end na EC2

Na EC2:

```bash
cd ~/datamarket-api/wwwroot
ls -l
```

Arquivos utilizados no ED11:

```text
index.html
cadastro-produto.js
```

---

## 5. Baixar os arquivos da EC2

No computador local:

```cmd
scp -i "C:\Users\SEU_USUARIO\.ssh\datamarket-key.pem" ec2-user@IP_PUBLICO_DA_EC2:/home/ec2-user/datamarket-api/wwwroot/index.html .
```

```cmd
scp -i "C:\Users\SEU_USUARIO\.ssh\datamarket-key.pem" ec2-user@IP_PUBLICO_DA_EC2:/home/ec2-user/datamarket-api/wwwroot/cadastro-produto.js .
```

---

## 6. Publicar os arquivos no S3

No bucket:

```text
Objetos → Carregar
```

Envie:

```text
index.html
cadastro-produto.js
```

Depois acesse o endpoint do site.

Neste momento, o front-end deverá ser carregado pelo S3, mas a comunicação com a API ainda poderá falhar.

---

## 7. Ajustar o fetch() para acessar a EC2

Antes do ED11, o front-end utilizava:

```javascript
fetch('/api/produtos')
```

Agora o front-end está no S3 e a API continua na EC2.

Edite o `index.html` e substitua por:

```javascript
fetch('http://IP_PUBLICO_DA_EC2:5000/api/produtos')
```

Faça o mesmo no:

```text
cadastro-produto.js
```

para o endpoint utilizado no cadastro de produtos.

---

## 8. Republicar os arquivos

No S3:

```text
Objetos → Carregar
```

Envie novamente:

```text
index.html
cadastro-produto.js
```

substituindo os arquivos existentes.

---

## 9. Validar a API diretamente

No navegador:

```text
http://IP_PUBLICO_DA_EC2:5000/api/produtos
```

Se o JSON for apresentado, temos:

```text
EC2 + API REST .NET + RDS PostgreSQL = OK
```

O problema fica isolado em:

```text
S3 → JavaScript → EC2
```

---

## 10. Configurar CORS na API ASP.NET Core

Na EC2:

```bash
cd ~/datamarket-api
```

Interrompa a aplicação:

```text
Ctrl+C
```

Faça backup:

```bash
cp Program.cs Program.cs.ed11.bak
```

Edite:

```bash
nano Program.cs
```

Depois de:

```csharp
var builder = WebApplication.CreateBuilder(args);
```

adicione:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowS3Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://datamarket-frontend-SEU-NOME.s3-website-us-east-1.amazonaws.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

Depois de:

```csharp
var app = builder.Build();
```

adicione:

```csharp
app.UseCors("AllowS3Frontend");
```

---

## 11. Compilar e executar

```bash
dotnet build
```

Resultado esperado:

```text
Build succeeded.
```

Depois:

```bash
dotnet run
```

---

## 12. Validar o resultado final

Atualize o endpoint do S3 no navegador:

```text
http://datamarket-frontend-SEU-NOME.s3-website-us-east-1.amazonaws.com
```

Os produtos deverão ser carregados normalmente.

Arquitetura validada:

```text
Amazon S3
   ↓
HTML + CSS + JavaScript
   ↓ fetch()
Amazon EC2
   ↓
API REST .NET
   ↓
Npgsql
   ↓
Amazon RDS PostgreSQL
```

---

## Resultado do ED11

Ao final deste estudo, o front-end da DataMarket passou a ser hospedado no Amazon S3.

A API REST .NET permaneceu na Amazon EC2 e continuou utilizando o Amazon RDS PostgreSQL para persistência.

Também configuramos CORS no ASP.NET Core para permitir a comunicação entre o front-end hospedado no S3 e a API hospedada na EC2.
