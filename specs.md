## Instruções
CP5-API RESTful com EF Core Net 10

Disciplina: C# Software Development
Tema: Aplicativo para rastrear a national dex de pokemon

## Objetivo
Desenvolver uma API RESTful utilizando C# .NET 10 e Entity Framework Core, aplicando os conceitos abordados em aula e boas práticas de desenvolvimento de software.

Formação dos Grupos
Grupos de até 5 integrantes:
- [Andrey Rodrigues Nagata] 	555339
- [Henrique Soubhia] 	554493
- [Oliver Kanai Trindade] 	554954
- [Pedro Gutierre Cardoso de Oliveira] 	555445
- [William Weile Feng] 	555132

Todos os integrantes devem constar no README do repositório

Banco de Dados
Escolha livre — SQLite, SQL Server, PostgreSQL, Oracle, MySQL ou outro de sua preferência
O banco utilizado deve ser informado no README

Entrega — Repositório GitHub
O repositório deve conter obrigatoriamente:
1. README.md completo com:

Nome do projeto
Integrantes do grupo
Contexto do projeto — o que é, qual problema resolve, para quem é destinado
Banco de dados utilizado
Instruções para rodar o projeto localmente
Endpoints disponíveis (rota, método HTTP e descrição)

2. Código-fonte do projeto completo e funcional

3. Prints de evidências de teste — capturas de tela do Postman, Insomnia, Swagger ou ferramenta similar, demonstrando cada endpoint funcionando

## Critérios de Avaliação
1     Conceitos REST    - uso correto dos métodos HTTP (GET, POST, PUT, DELETE), status codes adequados (200, 201, 204, 400, 404) e estrutura de rotas coerente   3,0 pts
2     EF Core     - uso do DbContext, entidades mapeadas, operações via EF Core e ao menos uma Migration documentada    2,5 pts
3     Versionamento de Endpoint - API com versionamento implementado (ex: /api/v1/recurso)      1,5 pts
4     Boas Práticas - Organização do projeto, nomes significativos, código limpo e tratamento de erros      2,0 pts
5     Documentação e Evidências - README completo, contexto claro do projeto e prints de testes funcionando 1,0 pt
Total       10,0 pts

## Observações Importantes
O projeto deve compilar e executar — código que não roda não será avaliado
Repositório privado sem acesso ao professor equivale a não entregue
Cada grupo deve entregar um único repositório


## stack

use swagger e sqlite

---

## Decisões de Arquitetura e Escopo Definidas

### 1. Nome do Projeto e Estrutura
- **Projeto**: `PokemonNationalDex.API`
- **Plataforma**: ASP.NET Core Web API em .NET 10
- **ORM**: Entity Framework Core com SQLite
- **Documentação interativa**: Swagger / OpenAPI integrado

### 2. Modelagem de Dados (Entity Framework Core)
- **Abordagem**: Tracker direto (pessoal), focado em eficiência e simplicidade.
- **Entidade `Pokemon`**:
  - `Id` (int, chave primária)
  - `DexNumber` (int, número na National Dex, ex: 1, 25, 150)
  - `Name` (string, obrigatório)
  - `PrimaryType` (string, ex: "Grass", "Fire", "Water")
  - `SecondaryType` (string, opcional, ex: "Poison", "Flying")
  - `Generation` (int, geração 1 a 9)
  - `IsLegendary` (bool)
  - `SpriteUrl` (string, opcional)
- **Entidade `DexEntry`**:
  - `Id` (int, chave primária)
  - `PokemonId` (int, FK para `Pokemon`)
  - `Pokemon` (relação de navegação)
  - `Status` (enum/string: `Seen` [Avistado], `Caught` [Capturado])
  - `IsShiny` (bool, se é variante brilhante)
  - `CaughtDate` (DateTime, data do registro)
  - `CaughtLocation` (string, ex: "Viridian Forest", "Route 1")
  - `GameVersion` (string, ex: "Red", "FireRed", "Scarlet")
  - `Notes` (string, observações adicionais)

### 3. Versionamento e Endpoints da API (`/api/v1/...`)
- **/api/v1/pokemons**:
  - `GET /api/v1/pokemons` - Lista Pokémons com filtros opcionais (name, generation, type) e paginação.
  - `GET /api/v1/pokemons/{id}` - Busca Pokémon por Id.
  - `POST /api/v1/pokemons` - Cria novo Pokémon (retorna 201 Created).
  - `PUT /api/v1/pokemons/{id}` - Atualiza Pokémon existente (retorna 204 No Content).
  - `DELETE /api/v1/pokemons/{id}` - Remove Pokémon (retorna 204 No Content).
- **/api/v1/dex-entries**:
  - `GET /api/v1/dex-entries` - Lista entradas do rastreador com filtros (status, isShiny, gameVersion).
  - `GET /api/v1/dex-entries/{id}` - Busca entrada por Id.
  - `GET /api/v1/dex-entries/stats` - Retorna estatísticas de progresso (total na dex, capturados, vistos, percentual concluído e total de shinies).
  - `POST /api/v1/dex-entries` - Registra captura/avistamento (retorna 201 Created).
  - `PUT /api/v1/dex-entries/{id}` - Atualiza registro de captura (retorna 204 No Content).
  - `DELETE /api/v1/dex-entries/{id}` - Remove registro de captura (retorna 204 No Content).

### 4. Boas Práticas e Tratamento de Erros
- DTOs separados para Requests (`CreatePokemonDto`, `UpdatePokemonDto`, `CreateDexEntryDto`, `UpdateDexEntryDto`) e Responses (`PokemonDto`, `DexEntryDto`, `DexStatsDto`).
- Data Annotations para validações (ex: `Required`, `StringLength`, `Range`).
- Retorno de códigos HTTP padronizados: `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`.
- Migrations do EF Core gerenciadas e documentadas, com Seeding inicial de Pokémons clássicos.

### 5. Documentação e Evidências
- Captura automatizada de screenshots do Swagger UI e endpoints com requisições reais salvas em `docs/screenshots/`.
- README.md detalhado contendo contexto, integrantes, requisitos, instrução de execução local, tabela de endpoints e galeria de evidências com as capturas.