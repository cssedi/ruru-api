# Agent Instructions

## Session Canary

- Begin every response by addressing the user as `Lesdee,` (including status updates and final replies). This is a deliberate context-integrity canary; if it is missed, correct course in the next response.

## Project Context

- This is an ASP.NET Core Web API targeting .NET 10. `Program.cs` configures services and the HTTP pipeline.
- Controllers live in `src/controller/`; EF Core entities and the database context live in `src/model/`. PostgreSQL is configured through the `DefaultConnection` connection string.
- Build with `dotnet build`. There is no test project in the current workspace; run tests if one is added or when a relevant test project is available.
- Swagger middleware is registered only in Development. The UI is at `/swagger`; the built-in OpenAPI document is at `/openapi/v1.json`. The configured HTTP launch profile uses port `5266`.
- Do not expose connection strings or other secrets from appsettings files in responses or committed examples.