
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
  "password": "string"
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

## 4) Executar os Testes Unitários
Para rodar os testes unitários, siga os passos abaixo:
1. Certifique-se de que o .NET SDK 8 está instalado em sua máquina.
2. Navegue até a pasta dos testes unitários do projeto Desafio.ProtocoloAPI:
```
cd src\Desafio.ProtocoloAPI
```
3. Execute o comando para rodar os testes:
```
dotnet test
```
4. Navegue até a pasta dos testes unitários do projeto Desafio.ProtocoloPublisher:
```
cd src\Desafio.ProtocoloPublisher
```
5. Execute o comando para rodar os testes:
```
dotnet test
```

## 5) Como ver os logs no Kibana
### 1. Acessar a Interface do Kibana
Após iniciar os serviços com docker-compose, você pode acessar o Kibana em:

[http://localhost:5601](http://localhost:5601)

### 2. Configurar um Padrão de Índice no Kibana
Uma vez na interface do Kibana, siga estas etapas:

- Acesse a seção "Stack Management":
  - No menu do lado esquerdo, clique em "Stack Management".
- Criar um Padrão de Índice (Index Pattern):
  - Dentro de "Stack Management", selecione "Index Patterns" e clique em "Create index pattern".
  - Insira `otel-logs-*` como o nome do padrão de índice para buscar os logs coletados pelo OpenTelemetry (conforme definido no Logstash).
  - Clique em "Next step".
- Selecionar Campo de Data:
  - Na lista de campos, selecione o campo de data apropriado (geralmente `@timestamp`) e clique em "Create index pattern".

### 3. Explorar os Logs
- No menu à esquerda, clique em "Discover".
- Selecione o padrão de índice que você criou (`otel-logs-*`).
- Você deve ver os logs enviados para o Elasticsearch e indexados pelo Logstash.

### 4. Visualizar e Criar Dashboards
- **Visualizar Logs:** Na seção "Discover", você pode aplicar filtros, mudar o intervalo de tempo e explorar os dados brutos que foram coletados.
- **Criar Visualizações:** No menu "Visualize Library" e "Dashboard", você pode criar gráficos e painéis com base nos logs e métricas que estão no Elasticsearch.
