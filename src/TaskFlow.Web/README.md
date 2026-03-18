# TaskFlow.Web

The frontend layer of the TaskFlow application.

## Technologies Used

- **Angular 21**
- **zardui** - Component library (planned)
- **Tailwind CSS** - Styling (with SCSS where needed)
- **Orval** - API model generation (planned)
- **NgRx Signal Store** - State management (planned)
- **ngx-translate** - translation service

## Project Structure (Target)

The project follows a feature-based modular structure to ensure scalability:

```text
src/app/
├── core/                   # Singletons: API services, interceptors, authorization
│   ├── services/           # ApiService, BoardService, WorkItemService
│   ├── interceptors/       # AuthInterceptor, ErrorInterceptor
│   └── models/             # DTO interfaces (BoardDto, ColumnDto, etc.)
├── shared/                 # Shared UI components (button, modal, loader)
│   ├── components/         # ButtonComponent, ModalComponent, CardComponent
│   └── pipes/              # Custom formatting pipes
├── features/               # Functional modules (views)
│   ├── auth/               # Login, Registration
│   ├── landing/            # Public Landing Page
│   ├── dashboard/          # User Dashboard (board list)
│   ├── boards/             # Board View (Kanban)
│   │   ├── components/     # Board-specific UI (e.g. ColumnCard)
│   │   ├── services/       # Board-specific logic/API wrappers
│   │   ├── store/          # NgRx Signal Store for board state
│   │   ├── models/         # Board-specific interfaces
│   │   └── pages/          # Main page components for routing
│   └── work-items/         # Task details, editing (often as modals)
└── layout/                 # Application skeleton
    ├── header/
    ├── sidebar/
    └── footer/
```

## Current Status

The project is currently in the initial scaffolding phase. The directory structure mentioned above will be implemented as features are developed. Core dependencies (zardui, ngrx, orval) will be added upon first usage.
