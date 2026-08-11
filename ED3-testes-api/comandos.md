# ED3 - Testes externos da API DataMarket

Neste ED, a API criada no ED2 será disponibilizada para acesso externo e seus endpoints serão testados.

## Testar o acesso externo

No navegador do computador, substitua `IP_PUBLICO_DA_EC2` pelo IPv4 público da sua instância.

### Health check

```text
http://IP_PUBLICO_DA_EC2:5000/health
```

### Consultar produtos

```text
http://IP_PUBLICO_DA_EC2:5000/api/produtos
```

## Inserir um novo produto

Em uma conexão SSH com a EC2:

```bash
curl -X POST http://localhost:5000/api/produtos \
  -H "Content-Type: application/json" \
  -d '{"id":0,"nome":"Teclado","preco":250.00,"estoque":10}'
```

## Consultar os produtos

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

Após o POST, deverão ser apresentados três produtos:

- Notebook
- Mouse
- Teclado

## Testar a persistência dos dados

No terminal em que a API está sendo executada, pressione:

```text
Ctrl+C
```

Execute novamente:

```bash
dotnet run
```

Em outra conexão SSH, consulte novamente:

```bash
curl -s http://localhost:5000/api/produtos | python3 -m json.tool
```

O produto **Teclado** não será mais apresentado, pois os dados adicionados pelo POST estavam armazenados somente na memória da aplicação.

> Este comportamento demonstra a necessidade de utilizar armazenamento persistente nas próximas etapas da arquitetura DataMarket.
