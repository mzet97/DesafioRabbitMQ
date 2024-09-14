# DesafioRabbitMQ

## Tecnologia usadas na demo
* RabbitMQ
* PostgreSQL
* OpenTelemetry
* Elasticsearch
* Kibana
* .Net 8
* xUnit

# Como rodar o pojetro

## 1) Subir o banco de dados e o RabbitMQ
Na pasta raiz do projeto, execute o seguinte comando:
```
docker-compose up --build -d
```

## 2) Executar o projeto do Publisher
Na pasta src\Desafio.ProtocoloPublisher, execute os seguintes comandos:
```
docker build -t matzet99/pub .
docker run --rm --network desafiorabbitmq_app-network matzet99/pub
```

## 3) Executar a API
Na pasta src\Desafio.ProtocoloAPI, execute os seguintes comandos:
```
docker build -t matzet99/api .
docker run --rm -p 8080:8080 --network desafiorabbitmq_app-network matzet99/api
```

Acesse a url do swagger disponivel em:

http://localhost:8080/swagger/index.html

Para registrar na aplicação:

POST http://localhost:8080/api/Auth/register

```
{
  "email": "user@example.com",
  "password": "string",
  "confirmPassword": "string",
}
```


Para fazer o login e gerar o token:

POST http://localhost:8080/api/Auth/login

```
{
  "email": "user@example.com",
  "password": "string"
}
```