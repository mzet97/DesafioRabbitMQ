# DesafioRabbitMQ

# Como rodar o pojetro

## 1) Subir o banco de dados e o RabbitMQ
Na pasta raiz do projeto, execute o seguinte comando:
```
docker-compose up --build -d
```

## 2) Executar o projeto do Publisher
Na pasta src\Desafio.ProtocoloPublisher, execute os seguintes comandos:
```
docker build -t protocoloPublisher .
docker run --rm --network desafiorabbitmq_app-network protocoloPublisher
```

## 3) Executar a API