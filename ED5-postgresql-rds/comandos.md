# ED5 - Conectando a EC2 ao Amazon RDS PostgreSQL

## 1. Atualizar os pacotes

```bash
sudo dnf update -y
```

## 2. Instalar o cliente PostgreSQL

```bash
sudo dnf install postgresql15 -y
```

Verifique a instalação:

```bash
psql --version
```

## 3. Testar a resolução DNS do RDS

Substitua `ENDPOINT_DO_RDS` pelo endpoint da sua instância Amazon RDS.

```bash
getent hosts ENDPOINT_DO_RDS
```

## 4. Testar a porta PostgreSQL

```bash
timeout 5 bash -c '</dev/tcp/ENDPOINT_DO_RDS/5432' && echo "Porta 5432 acessível" || echo "Falha na conexão"
```

## 5. Conectar ao PostgreSQL

```bash
psql -h ENDPOINT_DO_RDS -p 5432 -U datamarketadmin -d postgres
```

Digite a senha definida durante a criação do RDS.

## 6. Validar a conexão

No prompt do PostgreSQL:

```sql
SELECT current_database();
SELECT current_user;
```

## 7. Criar o banco DataMarket

```sql
CREATE DATABASE datamarket;
```

Conecte-se ao novo banco:

```text
\c datamarket
```

## 8. Criar a tabela produtos

```sql
CREATE TABLE produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco NUMERIC(10,2) NOT NULL,
    estoque INTEGER NOT NULL
);
```

Confira a estrutura:

```text
\d produtos
```

## 9. Inserir os produtos iniciais

```sql
INSERT INTO produtos (nome, preco, estoque)
VALUES
    ('Notebook', 4500.00, 5),
    ('Mouse', 120.00, 20);
```

Consulte os registros:

```sql
SELECT * FROM produtos;
```

## 10. Sair do PostgreSQL

```text
\q
```

## 11. Conectar diretamente ao banco DataMarket

```bash
psql -h ENDPOINT_DO_RDS \
    -p 5432 \
    -U datamarketadmin \
    -d datamarket
```

Consulte novamente:

```sql
SELECT * FROM produtos;
```

Os registros deverão continuar armazenados no Amazon RDS mesmo que a API .NET não esteja em execução.
