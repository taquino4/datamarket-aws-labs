# DataMarket - Laboratórios AWS

Repositório de apoio aos Estudos Dirigidos (EDs) da disciplina
**Sistemas em Nuvem com AWS**.

## Objetivo

Este repositório contém códigos, scripts e comandos utilizados durante
os laboratórios práticos do projeto **DataMarket**.

Os arquivos disponibilizados aqui complementam os Estudos Dirigidos e
devem ser utilizados em conjunto com as instruções apresentadas em aula.

## Arquitetura do projeto

Ao longo dos laboratórios construiremos gradualmente uma aplicação
utilizando alguns dos principais serviços da AWS:

- Amazon EC2 para hospedar a API REST .NET
- Security Groups para controle do tráfego de rede
- Amazon RDS PostgreSQL para persistência dos dados
- Amazon S3 para hospedagem do front-end
- Amazon CloudWatch para monitoramento

## Organização

O conteúdo será organizado de acordo com os Estudos Dirigidos:

- ED1 - Criação da infraestrutura EC2
- ED2 - Criação da API REST .NET
- ED3 - Testes da API
- ED4 - Criação do Amazon RDS PostgreSQL
- ED5 - Conexão da EC2 com o PostgreSQL
- ED6 - Integração da API .NET com o RDS

Novos arquivos serão adicionados conforme o desenvolvimento dos laboratórios.

## Importante

Os endereços IP, endpoints, identificadores e demais informações
específicas da AWS apresentados nos laboratórios podem ser diferentes
em cada conta.

Nunca publique neste repositório:

- Senhas
- Chaves privadas (.pem)
- AWS Access Keys
- Secrets
- Credenciais de banco de dados

Utilize sempre os valores correspondentes ao seu próprio ambiente AWS.
