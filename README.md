# Challenge Backend Alfa

Backend em **.NET 9** estruturado em **microsserviços**, aplicando **DDD (Domain-Driven Design)** e **CQRS** para separar responsabilidades de escrita/leitura e manter o domínio coeso e testável.

A solução utiliza **Entity Framework Core** para persistência, **FluentValidation** para validações consistentes e **mensageria com RabbitMQ via MassTransit** para comunicação assíncrona entre serviços, publicação/consumo de eventos e desacoplamento entre componentes.

---

## Stack & Conceitos Utilizados

- **.NET 9**
- **Microsserviços**
- **DDD (Domain-Driven Design)**
  - Domínio rico, regras encapsuladas, entidades/Value Objects, boundaries claros
- **CQRS**
  - Separação de **Commands** e **Queries** com handlers
- **MassTransit + RabbitMQ**
  - Eventos, consumidores (consumers), filas, comunicação assíncrona entre serviços
- **Entity Framework Core (EF Core)**
  - Migrations, mapeamentos, controle de schema
- **FluentValidation**
  - Validações declarativas para inputs e regras de aplicação
- **Docker + Docker Compose**
  - Ambiente reproduzível para subir a stack completa (serviços + dependências)

---

## Pré-requisitos

- **Docker** instalado (Docker Desktop no Windows/Mac ou Docker Engine no Linux)
- (Opcional) **Git**

---

##  Configuração

Você deve configurar as **connection strings** (e quaisquer variáveis de ambiente necessárias) **seguindo a mesma estrutura** da *fake connection string* definida no `docker-compose.yml`.

 Caminho do projeto (raiz):
   \challenge-backend-alfa\challenge-backend-alfa

   > Importante: mantenha os nomes das chaves/variáveis **iguais** aos usados no `docker-compose.yml` para evitar divergência entre ambientes.

---

## Como executar (Docker Compose)

1. Abra um terminal na **raiz** do repositório:

2. Suba toda a stack com build: docker compose up --build

3. Acessar no seu navegador a Order API: http://localhost:18082/swagger/index.html
  
4. SKUs de teste (Swagger):
Para facilitar testes via Swagger, você pode usar os seguintes SKUs já previstos no seed de estoque (inclui casos comuns e edge cases):

SKU-001 (estoque: 100)

SKU-002 (estoque: 25)

SKU-003 (estoque: 10)

SKU-LOW (estoque: 1) — caso de estoque baixo

SKU-ZERO (estoque: 0) — caso de estoque zerado
