# Instruções de uso:

## Executando a aplicação:

A aplicação consiste em uma solução .NET 8 integrada com um banco de dados PostgreSQL, ambos configurados para execução em contêineres Docker. As instruções para criação das imagens e inicialização dos contêineres estão especificadas no arquivo `docker-compose.yml`.

Para hospedar os contêineres localmente, abra o terminal na raiz da solução (/SalesService) e execute o comando `docker-compose up --build`, que construirá e iniciará os serviços conforme configurado.

Para encerrar os contêineres, utilize o comando `docker-compose down`, finalizando os serviços e liberando os recursos alocados.

## Primeira execução:

### Emissão de token para autenticação:

A aplicação está protegida por autenticação via JWT em todos os seus endpoints. Para gerar um usuário e token para uso em ambiente de desenvolvimento, abra o terminal no diretório `/SalesService/SalesService.Api` e execute o comando `dotnet user-jwts create`. O usuário e token podem ser testados no endpoint `GET /api/v1/user`.

### Dados autogerados:

Como dados de representantes de vendas e de produtos são pré-requisitos para a criação de um pedido, e um pedido existente é um requisito para os casos de uso de atualização de status e alteração de itens, a aplicação verifica, durante sua inicialização, se o contêiner do banco de dados já possui registros nas tabelas correspondentes. Caso não haja registros, a aplicação realiza a inserção dos dados iniciais definidos no arquivo `/SalesService/SalesService.Persistence/InitialData.cs`, facilitando assim a realização de testes manuais e garantindo que todos os dados mínimos estejam disponíveis para os cenários propostos.