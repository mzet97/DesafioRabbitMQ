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
docker run --rm --network desafiorabbitmq_app-network pmatzet99/pub
```

## 3) Executar a API