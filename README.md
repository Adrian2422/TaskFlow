# TaskFlow

TaskFlow is an application for creating Kanban boards that help organize work for individuals and teams.

## Technology Stack

- **Backend:** ASP.NET Core with .NET Aspire
- **Frontend:** TaskFlow.Web (Angular, Tailwind CSS, zardui)
- **Database:** Entity Framework Core with MsSql
- **Validation:** FluentValidation

## Architecture & Patterns

- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Result Pattern

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/) must be installed and running.
- [.NET SDK](https://dotnet.microsoft.com/download) (compatible with the version in `global.json`).

### Development

To start the application for development, run the **Aspire** profile.

### API Documentation

Once the application is running, Swagger UI is available at:
`https://localhost:7011/swagger`
