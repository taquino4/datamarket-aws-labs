ED5 - Conectando a EC2 ao Amazon RDS PostgreSQL

1. Atualizar os pacotes
sudo dnf update -y

2. Instalar o cliente PostgreSQL
sudo dnf install postgresql15 -y

Verifique a instalação:
psql --version

3. Testar a resolução DNS do RDS
Substitua `ENDPOINT_DO_RDS` pelo endpoint da sua instância Amazon RDS.
getent hosts ENDPOINT_DO_RDS

4. Testar a porta PostgreSQL
timeout 5 bash -c '</dev/tcp/ENDPOINT_DO_RDS/5432' && echo "Porta 5432 acessível" || echo "Falha na conexão"

5. Conectar ao PostgreSQL
psql -h ENDPOINT_DO_RDS -p 5432 -U datamarketadmin -d postgres

Digite a senha definida durante a criação do RDS.

6. Validar a conexão
No prompt do PostgreSQL:

SELECT current_database();
SELECT current_user;

7. Criar o banco DataMarket

CREATE DATABASE datamarket;
Conecte-se ao novo banco:
\c datamarket

8. Criar a tabela produtos
CREATE TABLE produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco NUMERIC(10,2) NOT NULL,
    estoque INTEGER NOT NULL
);

Confira a estrutura:
\d produtos

9. Inserir os produtos iniciais

INSERT INTO produtos (nome, preco, estoque)
VALUES
    ('Notebook', 4500.00, 5),
    ('Mouse', 120.00, 20);

Consulte os registros:

SELECT * FROM produtos;

10. Sair do PostgreSQL

\q


11. Conectar diretamente ao banco DataMarket

psql -h ENDPOINT_DO_RDS \
    -p 5432 \
    -U datamarketadmin \
    -d datamarket
Consulte novamente:

SELECT * FROM produtos;

Os registros deverão continuar armazenados no Amazon RDS mesmo que a API .NET não esteja em execução.
