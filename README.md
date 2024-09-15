
# Desafio RabbitMQ

## Tecnologias Utilizadas na Demonstração
* RabbitMQ
* PostgreSQL
* OpenTelemetry
* Elasticsearch
* Kibana
* .NET 8
* xUnit

# Como Executar o Projeto

## 1) Iniciar o Banco de Dados e o RabbitMQ
Na pasta raiz do projeto, execute o seguinte comando:
```
docker-compose up --build -d
```

## 2) Executar a API no Docker
Na pasta `src\Desafio.ProtocoloAPI`, execute os seguintes comandos:
```
docker build -t matzet99/api .
docker run --rm -p 8080:8080 --network desafiorabbitmq_app-network matzet99/api
```

#### Acesse a URL do Swagger disponível em:

[http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

#### Para registrar-se na aplicação:

Faça uma requisição POST para: 
```
http://localhost:8080/api/Auth/register
```
Com o corpo da requisição:
```
{
  "email": "user@example.com",
  "password": "string",
  "confirmPassword": "string"
}
```

#### Para fazer login e gerar o token:

Faça uma requisição POST para: 
```
http://localhost:8080/api/Auth/login
```
Com o corpo da requisição:
```
{
  "email": "user@example.com",
  "password": "string"
}
```

#### Consumidor
O consumidor está em execução como um serviço em segundo plano da aplicação. Esse serviço fica escutando a __Fila:__ `protocolos_pending_queue`. Se os dados forem válidos, serão salvos no banco de dados e enviados para a __Fila:__ `protocolos_finish_queue`. Após o salvamento, eles serão encaminhados com a __Routing Key:__ `protocolos.approved`. Caso contrário, serão enviados com a __Routing Key:__ `protocolos.refused`.

## 3) Executar o Projeto do Publicador no Docker
Na pasta `src\Desafio.ProtocoloPublisher`, execute os seguintes comandos:
```
docker build -t matzet99/pub .
docker run --rm --network desafiorabbitmq_app-network matzet99/pub
```

#### Informações Necessárias
Ao executar o `ProtocoloPublisher`, ele enviará dados simulados para o RabbitMQ.

- __Exchange:__ `protocolos`
- __Fila:__ `protocolos_pending_queue`
- __Routing Key:__ `protocolos.pending`
