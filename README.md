# TaskFlow

TaskFlow is an application for creating Kanban boards that help organize work for individuals and teams.

## Functionalities

Below is a list of features currently implemented on the backend/API side (the frontend is under development):

- **Boards:** create, list all, get details, edit.
- **Columns:** create, edit name, reorder (move/reorder).
- **Work Items:** create (in a column or in the backlog), edit (title/description), move (between columns and the backlog), and reorder.
- **Backlog:** items without an assigned column (e.g., after creation without a `ColumnId` or after deleting a column).
- **Archiving:** archive and restore boards and work items; archiving a board archives associated items.
- **Secure Deletion:** Permanent deletion is only allowed for archived tables/work items.
- **Start Column Protection:** New tables receive a default column (e.g., "To Do") marked as protected (undeletable).
- **API Tooling:** Swagger/OpenAPI and endpoint health check (Aspire-ready).

## Roadmap

Feature proposals to be implemented in subsequent iterations:

- [ ] Table view in the UI (columns + backlog + tabs) with drag and drop (move/reorder) and loading/error states.
- [ ] Full CRUD in the UI for tables/columns/work items (forms, validation, action confirmations, optimistic updates).
- [ ] Authentication and authorization (users, roles, table ACLs).
- [ ] Teams and board sharing (invitations, permissions, user assignments to boards).
- [ ] Real-time collaboration (SignalR/WebSockets): live updates, activity indicators, event feed.
- [ ] Work item extensions: labels/tags, priority, due date (due date), assignments, checklists, comments, attachments.
- [ ] Board features: templates, WIP limits per column, rules/flows, archive management panel.
- [ ] Search and filtering (including full-text by title/description).
- [ ] Import/export (CSV/JSON) and integrations (webhooks, Slack/Teams).
- [ ] Quality and operations: e2e/integration tests, CI/CD, monitoring/alerts (OpenTelemetry), hardening of logging and metrics.

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
