# Desafio RabbitMQ

## Tecnologias Utilizadas na Demonstração

- RabbitMQ
- PostgreSQL
- OpenTelemetry
- Elasticsearch
- Kibana
- .NET 8
- xUnit

## Instruções para Execução do Projeto

### 1. Configuração do Docker Swarm

#### 1.1 Inicialização do Docker Swarm

Para iniciar o Docker Swarm, execute o comando:

```bash
docker swarm init
```

O Docker Swarm é uma ferramenta poderosa que permite a orquestração de contêineres em múltiplos nós, facilitando o gerenciamento e a alta disponibilidade dos serviços. Ao inicializar o Swarm, seu host será configurado como um nó gerenciador, possibilitando a criação de clusters robustos para lidar com diversas cargas de trabalho.

#### 1.2 Criação da Rede para Contêineres

Crie a rede necessária para a comunicação dos contêineres:

```bash
docker network create --driver overlay --attachable desafiorabbitmq_app-network
```

A criação de uma rede overlay permite que os contêineres se comuniquem de maneira eficiente, mesmo que estejam distribuídos em diferentes nós do cluster. A opção `--attachable` possibilita a conexão direta de contêineres fora do contexto de serviços gerenciados, garantindo flexibilidade na comunicação entre componentes distribuídos.

#### 1.3 Execução do Docker Compose

Nesta etapa, o Docker Compose será executado via Swarm para instanciar os contêineres necessários para a demonstração. Na pasta raiz do projeto, execute o seguinte comando:

```bash
docker stack deploy -c docker-compose.yml desafio_stack
```

O comando acima implanta todos os serviços definidos no arquivo `docker-compose.yml` como um stack do Swarm, garantindo alta disponibilidade e fácil gerenciamento. Cada contêiner é instanciado como uma tarefa dentro do stack, facilitando a escalabilidade e o monitoramento dos componentes.

### 2. Executar a API no Docker Swarm

Na pasta `src/Desafio.ProtocoloAPI`, execute os seguintes comandos:

```bash
docker build -t matzet99/api .
docker service create --replicas 3 --name desafioMQ -p 8080:8080 --network desafiorabbitmq_app-network matzet99/api
```

```bash
docker service create --name desafioMQ --replicas 32 --limit-cpu 1 --limit-memory 2G --reserve-cpu 0.5 --reserve-memory 1G --restart-condition any --restart-max-attempts 3 --placement-pref 'spread=node.id' --network desafiorabbitmq_app-network -p 8080:8080 matzet99/api

```

Este processo cria a imagem da API e, em seguida, cria um serviço com 3 réplicas. As réplicas garantem maior resiliência, permitindo que o sistema continue funcionando mesmo que uma instância falhe. O uso da rede overlay assegura que todas as réplicas possam se comunicar de forma eficaz com os demais serviços.

#### Acessar a Interface do Swagger

A interface Swagger está disponível em: [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html) O Swagger fornece uma interface gráfica para interação com os endpoints da API, permitindo o teste de funcionalidades e a validação dos recursos disponíveis. Esta interface é essencial para garantir que os desenvolvedores possam explorar e compreender o comportamento da API de maneira intuitiva.

#### Registro na Aplicação

Faça uma requisição POST para o seguinte endpoint:

```
http://localhost:8080/api/Auth/register
```

Com o corpo da requisição:

```json
{
  "email": "user@example.com",
  "password": "string"
}
```

Este endpoint permite o registro de novos usuários na aplicação. O processo de registro é uma etapa fundamental para garantir o acesso seguro aos recursos disponibilizados pela API, utilizando um sistema de autenticação robusto.

#### Login e Geração de Token

Faça uma requisição POST para o seguinte endpoint:

```
http://localhost:8080/api/Auth/login
```

Com o corpo da requisição:

```json
{
  "email": "user@example.com",
  "password": "string"
}
```

Após o registro, o usuário deve realizar o login para obter um token de acesso. Este token será utilizado para autenticar as requisições subsequentes, garantindo que apenas usuários autorizados possam acessar os recursos protegidos da API.

#### Consumidor de Mensagens

O consumidor de mensagens está em execução como um serviço em segundo plano da aplicação. Esse serviço monitora a **fila** `protocolos_pending_queue`. Caso os dados sejam válidos, eles serão salvos no banco de dados e enviados para a **fila** `protocolos_finish_queue`. Após o salvamento, os dados serão encaminhados com a **Routing Key** `protocolos.approved`. Caso sejam inválidos, serão enviados com a **Routing Key** `protocolos.refused`. O consumidor implementa um padrão de mensageria que permite o processamento assíncrono de dados, aumentando a eficiência e a escalabilidade do sistema. A validação e roteamento dos dados permitem que ações apropriadas sejam tomadas, como persistência no banco de dados ou reenvio com uma chave de roteamento específica.

### 3. Executar o Publicador no Docker

Na pasta `src/Desafio.ProtocoloPublisher`, execute os seguintes comandos:

```bash
docker build -t matzet99/pub .
docker run --rm --network desafiorabbitmq_app-network matzet99/pub
```

O publicador é responsável por enviar mensagens para a fila `protocolos_pending_queue`, simulando a geração de novos protocolos. A execução deste componente permite testar a interação entre o produtor e o consumidor de mensagens, validando o fluxo completo de processamento.

#### Informações sobre o Publicador

Ao executar o `ProtocoloPublisher`, dados simulados serão enviados ao RabbitMQ.

- **Exchange**: `protocolos`
- **Fila**: `protocolos_pending_queue`
- **Routing Key**: `protocolos.pending`

A exchange `protocolos` é utilizada para distribuir mensagens para as filas corretas, com base na chave de roteamento `protocolos.pending`. Essa arquitetura garante flexibilidade e escalabilidade no processamento dos dados, permitindo adicionar novas filas e regras de roteamento conforme necessário.

### 4. Execução dos Testes Unitários

Para rodar os testes unitários, siga as instruções abaixo:

1. Certifique-se de que o .NET SDK 8 está instalado em sua máquina.
2. Navegue até a pasta dos testes unitários do projeto `Desafio.ProtocoloAPI`:
   ```bash
   cd src/Desafio.ProtocoloAPI
   ```
3. Execute o comando para rodar os testes:
   ```bash
   dotnet test
   ```
4. Navegue até a pasta dos testes unitários do projeto `Desafio.ProtocoloPublisher`:
   ```bash
   cd src/Desafio.ProtocoloPublisher
   ```
5. Execute o comando para rodar os testes:
   ```bash
   dotnet test
   ```

Os testes unitários são essenciais para validar o comportamento correto dos componentes individuais do sistema. Eles garantem que cada módulo funcione conforme o esperado, isolando erros e permitindo uma manutenção mais confiável do código. A execução periódica dos testes promove uma abordagem de desenvolvimento orientada à qualidade.

### 5. Visualizar Logs no Kibana

#### 5.1 Acesso à Interface do Kibana

Após iniciar os serviços com Docker Compose, acesse o Kibana em: [http://localhost:5601](http://localhost:5601) O Kibana é uma ferramenta poderosa para visualização de logs e análise de dados. Ele permite monitorar em tempo real as atividades da aplicação, facilitando a identificação de problemas e a análise de desempenho.

#### 5.2 Configuração de um Padrão de Índice no Kibana

Na interface do Kibana, siga estas etapas:

- Acesse a seção "Stack Management":
  - No menu do lado esquerdo, clique em "Stack Management".
- Criar um Padrão de Índice (Index Pattern):
  - Dentro de "Stack Management", selecione "Index Patterns" e clique em "Create index pattern".
  - Insira `otel-logs-*` como o nome do padrão de índice para buscar os logs coletados pelo OpenTelemetry (conforme definido no Logstash).
  - Clique em "Next step".
- Selecionar Campo de Data:
  - Na lista de campos, selecione o campo de data apropriado (geralmente `@timestamp`) e clique em "Create index pattern".

O padrão de índice permite ao Kibana buscar e organizar os dados do Elasticsearch de forma estruturada, facilitando a navegação e a análise dos registros. Configurar um índice adequado é essencial para garantir a visibilidade dos eventos mais relevantes do sistema.

#### 5.3 Explorar os Logs

- No menu à esquerda, clique em "Discover".
- Selecione o padrão de índice que você criou (`otel-logs-*`).
- Você deve visualizar os logs enviados para o Elasticsearch e indexados pelo Logstash.

A seção "Discover" do Kibana permite explorar os logs de forma detalhada, aplicando filtros e analisando registros específicos. Isso possibilita uma visão precisa das operações do sistema e facilita a detecção de possíveis problemas.

#### 5.4 Visualizar e Criar Dashboards

- **Exploração de Logs**: Na seção "Discover", aplique filtros, altere o intervalo de tempo e explore os dados brutos coletados.
- **Criação de Visualizações**: No menu "Visualize Library" e "Dashboard", crie gráficos e painéis com base nos logs e métricas que estão no Elasticsearch.

A criação de dashboards permite visualizar métricas e dados de maneira mais intuitiva, utilizando gráficos e diagramas que facilitam a interpretação dos resultados. Dashboards bem projetados são fundamentais para uma gestão eficiente e uma resposta rápida a incidentes, oferecendo insights críticos sobre o funcionamento e a performance da aplicação.
