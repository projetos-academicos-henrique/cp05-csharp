# Pokemon National Dex Tracker API 🔴⚪

API RESTful desenvolvida em **C# .NET 10** e **Entity Framework Core** para gerenciamento e rastreamento da **National Pokédex**, permitindo catalogar espécies de Pokémon e registrar o progresso de avistamentos e capturas dos jogadores.

---

## 👥 Integrantes do Grupo

* **Andrey Rodrigues Nagata** — RM: 555339 
* **Henrique Soubhia** — RM: 554493
* **Oliver Kanai Trindade** — RM: 554954
* **Pedro Gutierre Cardoso de Oliveira** — RM: 555445
* **William Weile Feng** — RM: 555132

---

## 📖 Contexto do Projeto

### O que é?
O **Pokemon National Dex Tracker** é uma API RESTful projetada para ser a espinha dorsal de um sistema de acompanhamento da Pokédex Nacional completa. A National Dex reúne todas as espécies conhecidas do universo Pokémon através de todas as gerações.

### Qual problema resolve?
Com centenas de espécies introduzidas ao longo de diferentes jogos e gerações, jogadores dedicados (chamados na comunidade de *Living Dex Hunters* ou *Completionists*) enfrentam dificuldades para centralizar o rastreamento das suas capturas, quais Pokémon já viram, quais já capturaram, variantes *shiny* (brilhantes), localização onde foram encontrados e versão do jogo onde o registro ocorreu. A API fornece uma estrutura persistente, padronizada e com cálculo automático de métricas e porcentagens de conclusão da Dex.

### Para quem é destinado?
* **Treinadores Pokémon** e entusiastas que buscam completar a Pokédex Nacional (100% completion).
* **Desenvolvedores** que queiram integrar aplicativos mobile, web ou bots com uma API robusta de Pokédex e rastreamento de progresso.

---

## 🛠️ Tecnologias e Banco de Dados

* **Linguagem / Plataforma**: C# (.NET 10.0)
* **Framework Web**: ASP.NET Core Web API (Controllers)
* **ORM**: Entity Framework Core 10.0 (Code-First)
* **Banco de Dados**: **SQLite** (`pokemondex.db`)
* **Documentação Interativa**: Swagger / OpenAPI (Swashbuckle)
* **Versionamento de Rotas**: Padrão RESTful versionado (`/api/v1/...`)

---

## 🏗️ Arquitetura e Modelagem de Dados

O projeto segue boas práticas de organização em camadas dentro da aplicação:

* **`Models/`**: Entidades de domínio mapeadas no EF Core:
  * `Pokemon`: Representa as espécies da National Dex (`Id`, `DexNumber` [índice único], `Name`, `PrimaryType`, `SecondaryType`, `Generation`, `IsLegendary`, `SpriteUrl`).
  * `DexEntry`: Representa o registro individual de progresso do jogador (`Id`, `PokemonId`, `Status` [Seen / Caught], `IsShiny`, `CaughtDate`, `CaughtLocation`, `GameVersion`, `Notes`).
  * `DexStatus`: Enum que categoriza o status entre `Seen` (Avistado) e `Caught` (Capturado).
* **`Data/`**: `AppDbContext` com Fluent API, chaves estrangeiras com deleção em cascata, índices e carga inicial de dados (*Data Seeding* de espécies clássicas e registros de exemplo).
* **`DTOs/`**: Objetos de transferência desacoplados das entidades do banco com validações declarativas via `DataAnnotations` (`CreatePokemonDto`, `UpdatePokemonDto`, `PokemonResponseDto`, `CreateDexEntryDto`, `UpdateDexEntryDto`, `DexEntryResponseDto`, `DexStatsResponseDto`).
* **`Controllers/`**: Controladores versionados (`PokemonsController` e `DexEntriesController`) com respostas HTTP padronizadas.

---

## 🚀 Como Rodar o Projeto Localmente

### Pré-requisitos
* [.NET SDK 10.0](https://dotnet.microsoft.com/download) (ou superior) instalado na máquina.
* Git instalado.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd cp5-csharp
   ```

2. **Restaure as dependências e compile o projeto:**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Execução das Migrations do EF Core (Opcional - já executada automaticamente no startup):**
   A aplicação aplica as migrações e o seed de dados automaticamente ao inicializar. Caso queira rodar manualmente via CLI:
   ```bash
   cd PokemonNationalDex.API
   dotnet tool run dotnet-ef database update
   cd ..
   ```

4. **Inicie a aplicação:**
   ```bash
   dotnet run --project PokemonNationalDex.API
   ```

5. **Acesse a Documentação Swagger:**
   * Navegue até: [http://localhost:5191/swagger](http://localhost:5191/swagger) ou [http://localhost:5191](http://localhost:5191) (redirecionamento automático).

---

## 🌐 Endpoints Disponíveis da API (`/api/v1/...`)

### 1. Pokémons (`/api/v1/pokemons`)

| Método | Rota | Descrição | Status Codes |
|---|---|---|---|
| `GET` | `/api/v1/pokemons` | Lista Pokémons cadastrados com filtros opcionais (`name`, `generation`, `type`) e paginação (`page`, `pageSize`) | `200 OK` |
| `GET` | `/api/v1/pokemons/{id}` | Busca os detalhes de um Pokémon por ID | `200 OK`, `404 Not Found` |
| `POST` | `/api/v1/pokemons` | Cadastra um novo Pokémon na Pokédex Nacional | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/v1/pokemons/{id}` | Atualiza os dados de um Pokémon existente | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/v1/pokemons/{id}` | Remove um Pokémon da base de dados | `204 No Content`, `404 Not Found` |

### 2. Rastreamento e Entradas da Dex (`/api/v1/dex-entries`)

| Método | Rota | Descrição | Status Codes |
|---|---|---|---|
| `GET` | `/api/v1/dex-entries` | Lista todos os registros de rastreamento com filtros (`status`, `isShiny`, `gameVersion`) | `200 OK` |
| `GET` | `/api/v1/dex-entries/{id}` | Busca um registro de rastreamento por ID | `200 OK`, `404 Not Found` |
| `GET` | `/api/v1/dex-entries/stats` | Retorna estatísticas de conclusão da National Dex (% capturado, % visto, total shinies) | `200 OK` |
| `POST` | `/api/v1/dex-entries` | Registra uma nova captura ou avistamento de Pokémon | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/v1/dex-entries/{id}` | Atualiza as informações de uma captura existente | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/v1/dex-entries/{id}` | Remove um registro de rastreamento | `204 No Content`, `404 Not Found` |

---

## 📸 Evidências de Teste

Abaixo estão as capturas de tela demonstrando o funcionamento de cada endpoint, seus códigos de retorno HTTP e a interface de documentação Swagger.

### 1. Visão Geral da Documentação Swagger UI
![Swagger Overview](docs/screenshots/01_swagger_overview.png)

---

### 2. Endpoints de Pokémons

#### GET `/api/v1/pokemons` (Status: 200 OK)
*Listagem paginada dos Pokémon cadastrados na Pokédex Nacional.*
![GET Pokemons 200](docs/screenshots/02_pokemons_get_all_200.png)

#### GET `/api/v1/pokemons/{id}` (Status: 200 OK)
*Busca de detalhes do Pokémon por ID (ex: Bulbasaur #1).*
![GET Pokemon By ID 200](docs/screenshots/03_pokemons_get_by_id_200.png)

#### POST `/api/v1/pokemons` (Status: 201 Created)
*Cadastro bem-sucedido de novo Pokémon (Dragonite #149).*
![POST Pokemon 201](docs/screenshots/04_pokemons_post_created_201.png)

#### PUT `/api/v1/pokemons/{id}` (Status: 204 No Content)
*Atualização de informações de Pokémon existente.*
![PUT Pokemon 204](docs/screenshots/05_pokemons_put_updated_204.png)

#### DELETE `/api/v1/pokemons/{id}` (Status: 204 No Content)
*Exclusão de Pokémon por ID.*
![DELETE Pokemon 204](docs/screenshots/06_pokemons_delete_204.png)

#### GET `/api/v1/pokemons/999` (Status: 404 Not Found)
*Tratamento de erro amigável ao buscar recurso inexistente.*
![GET Pokemon 404](docs/screenshots/07_pokemons_not_found_404.png)

#### POST `/api/v1/pokemons` (Status: 400 Bad Request)
*Validação declarativa de entrada recusando dados inválidos com mensagens de erro.*
![POST Pokemon 400](docs/screenshots/08_pokemons_bad_request_400.png)

---

### 3. Endpoints de Rastreamento (Dex Entries & Stats)

#### GET `/api/v1/dex-entries` (Status: 200 OK)
*Listagem de registros rastreados com relacionamentos e dados do Pokémon.*
![GET Dex Entries 200](docs/screenshots/09_dex_entries_get_all_200.png)

#### GET `/api/v1/dex-entries/stats` (Status: 200 OK)
*Cálculo de métricas de conclusão da Pokédex (total capturado, visto, % concluído e shinies).*
![GET Stats 200](docs/screenshots/10_dex_entries_stats_200.png)

#### POST `/api/v1/dex-entries` (Status: 201 Created)
*Registro de captura de Pokémon com local, versão do jogo e anotações.*
![POST Dex Entry 201](docs/screenshots/11_dex_entries_post_created_201.png)

#### PUT `/api/v1/dex-entries/{id}` (Status: 204 No Content)
*Atualização de dados de uma captura registrada.*
![PUT Dex Entry 204](docs/screenshots/12_dex_entries_put_updated_204.png)

#### POST `/api/v1/dex-entries` com FK Inexistente (Status: 400 Bad Request)
*Validação relacional impedindo registro vinculado a Pokémon que não existe.*
![POST Dex Entry 400](docs/screenshots/13_dex_entries_bad_request_400.png)

#### GET `/api/v1/dex-entries/999` (Status: 404 Not Found)
*Tratamento de registro de rastreamento não encontrado.*
![GET Dex Entry 404](docs/screenshots/14_dex_entries_not_found_404.png)

---

## 🎯 Atendimento aos Critérios de Avaliação

1. **Conceitos REST (3,0 pts)**: Métodos HTTP aplicados adequadamente (`GET`, `POST`, `PUT`, `DELETE`), status codes padronizados (`200`, `201`, `204`, `400`, `404`) e rotas coerentes.
2. **EF Core (2,5 pts)**: `AppDbContext` configurado com relacionamentos, Data Seeding, operações assíncronas e Migration `InitialCreate` versionada na pasta `Migrations/`.
3. **Versionamento de Endpoint (1,5 pts)**: Endpoints com prefixo versionado `/api/v1/...`.
4. **Boas Práticas (2,0 pts)**: Estrutura em camadas limpa (`Models`, `Data`, `DTOs`, `Controllers`), DTOs para proteção de entidade, validações de modelo e tratamento de exceções/recursos não encontrados.
5. **Documentação e Evidências (1,0 pt)**: `README.md` completo com contexto, integrantes, instruções, tabela de rotas e 14 capturas de tela comprovando a execução de todos os fluxos.
