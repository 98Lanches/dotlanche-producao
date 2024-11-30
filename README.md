# 🏭 Dotlanches Produção

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=98Lanches_dotlanche-producao&metric=coverage)](https://sonarcloud.io/summary/new_code?id=98Lanches_dotlanche-producao)

Microsserviço de Produção Dotlanche. Responsável pela inicialização da produção de um pedido, por disponibilizar a fila de produção dos pedidos e também pela atualização dos status dos pedidos.

# Funcionalidades
- Inicialização de produção do pedido
- Fila de Pedidos
- Atualizar Status do Pedido

# Stack
- .NET 8.0
- MongoDb
- NUnit
- Moq
- Reqnroll
- Docker
- Docker Compose
- Kubernetes
- GitHub Actions

# Arquitetura do Sistema
O Serviço é uma WebApi com alguns endpoints síncronos. Foi construído utilizando arquitetura hexagonal para organização interna. O banco de dados selecionado foi o MongoDB pela escalabilidade e performance de escrita.

# Como executar o projeto

## Pré-requisitos
- Docker

1. Na raiz do projeto, execute o comando
```
docker compose up
```
2. Acesse o navegador o endereço http://localhost:8080/swagger/index.html

# Testes
Tanto os testes de unidade quanto os testes de BDD encontram-se no diretório `test`.

Para executar os testes da aplicação, basta rodar o comando abaixo na raiz do projeto:
```
dotnet test
```