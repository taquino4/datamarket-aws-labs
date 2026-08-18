# ED8 - Criando o Front-end da Aplicação DataMarket

## 1. Acessar a pasta da aplicação

```bash
cd ~/datamarket-api
```

Confirmar a existência da pasta `wwwroot`:

```bash
ls -l
```

---

## 2. Criar a primeira página HTML

```bash
cd wwwroot
nano index.html
```

Conteúdo inicial:

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>DataMarket</title>
</head>

<body>
    <h1>DataMarket</h1>
    <h2>Catálogo de Produtos</h2>
    <p>Aplicação DataMarket em execução na AWS.</p>
</body>
</html>
```

Salvar:

```text
Ctrl+O
Enter
Ctrl+X
```

Confirmar:

```bash
ls -l
```

---

## 3. Acessar a aplicação no navegador

Com a API em execução:

```bash
dotnet run
```

Acesse:

```text
http://IP_PUBLICO_DA_EC2:5000/
```

Resultado esperado:

```text
DataMarket
Catálogo de Produtos
Aplicação DataMarket em execução na AWS.
```

---

## 4. Criar backup do index.html

```bash
cd ~/datamarket-api/wwwroot
cp index.html index.html.ed8.bak
```

---

## 5. Consumir a API com JavaScript

Editar:

```bash
nano index.html
```

Adicionar uma área para apresentação dos produtos:

```html
<div id="produtos">
    Carregando produtos...
</div>
```

Utilizar JavaScript para consumir a API:

```javascript
async function carregarProdutos() {

    const resposta = await fetch('/api/produtos');
    const produtos = await resposta.json();

    const divProdutos = document.getElementById('produtos');

    divProdutos.innerHTML = '';

    produtos.forEach(produto => {

        const item = document.createElement('p');

        item.textContent =
            `${produto.nome} - R$ ${produto.preco} - Estoque: ${produto.estoque}`;

        divProdutos.appendChild(item);
    });
}

carregarProdutos();
```

---

## 6. Testar o consumo da API

Atualize no navegador:

```text
http://IP_PUBLICO_DA_EC2:5000/
```

Os produtos armazenados no Amazon RDS PostgreSQL deverão aparecer na página.

Fluxo validado:

```text
Navegador
   ↓
index.html
   ↓
JavaScript fetch()
   ↓
GET /api/produtos
   ↓
API REST .NET
   ↓
Npgsql
   ↓
Amazon RDS PostgreSQL
```

---

## 7. Melhorar a apresentação com CSS

Editar novamente:

```bash
nano index.html
```

Utilizar o seguinte conteúdo:

```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>DataMarket</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 40px;
        }

        .container {
            max-width: 900px;
            margin: auto;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }

        h1 {
            color: #232f3e;
            margin-bottom: 5px;
        }

        h2 {
            color: #ff9900;
            margin-top: 0;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 25px;
        }

        th {
            background-color: #232f3e;
            color: white;
            text-align: left;
            padding: 12px;
        }

        td {
            padding: 12px;
            border-bottom: 1px solid #ddd;
        }

        tr:hover {
            background-color: #f2f2f2;
        }

        .status {
            color: #555;
            margin-bottom: 20px;
        }
    </style>
</head>

<body>

<div class="container">

    <h1>DataMarket</h1>
    <h2>Catálogo de Produtos</h2>

    <p class="status">
        Produtos cadastrados no Amazon RDS PostgreSQL.
    </p>

    <table>
        <thead>
            <tr>
                <th>ID</th>
                <th>Produto</th>
                <th>Preço</th>
                <th>Estoque</th>
            </tr>
        </thead>

        <tbody id="tabela-produtos">
            <tr>
                <td colspan="4">Carregando produtos...</td>
            </tr>
        </tbody>
    </table>

</div>

<script>
    async function carregarProdutos() {

        const resposta = await fetch('/api/produtos');
        const produtos = await resposta.json();

        const tabela =
            document.getElementById('tabela-produtos');

        tabela.innerHTML = '';

        produtos.forEach(produto => {

            const linha = document.createElement('tr');

            linha.innerHTML = `
                <td>${produto.id}</td>
                <td>${produto.nome}</td>
                <td>R$ ${Number(produto.preco).toFixed(2)}</td>
                <td>${produto.estoque}</td>
            `;

            tabela.appendChild(linha);
        });
    }

    carregarProdutos();
</script>

</body>
</html>
```

Salvar:

```text
Ctrl+O
Enter
Ctrl+X
```

---

## 8. Validar o resultado final

Atualize novamente:

```text
http://IP_PUBLICO_DA_EC2:5000/
```

A página deverá apresentar os produtos em uma tabela com:

```text
ID | Produto | Preço | Estoque
```

Os dados exibidos são recuperados dinamicamente do Amazon RDS PostgreSQL por meio da API REST.

---

## Resultado do ED8

Ao final deste estudo, construímos o primeiro front-end da aplicação DataMarket utilizando:

```text
HTML + CSS + JavaScript
```

O JavaScript utiliza:

```javascript
fetch('/api/produtos')
```

para consumir a API REST .NET e apresentar no navegador os produtos armazenados no Amazon RDS PostgreSQL.

Arquitetura validada:

```text
Usuário
   ↓
Navegador
   ↓
HTML + CSS + JavaScript
   ↓
API REST .NET
   ↓
Npgsql
   ↓
Amazon RDS PostgreSQL
```
