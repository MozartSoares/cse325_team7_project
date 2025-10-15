# Backend Architecture Overview

This document complements `README.md` and focuses on the backend.

## High-Level Design

```
Domain (entities, enums, value objects)
        ↓
API ──► DTOs ↔ Mapping Extensions
        ↓
      Services (MongoDB repositories + business rules)
        ↓
 Controllers (thin HTTP endpoints)
        ↓
 Middleware / Model Binders (cross-cutting concerns)
```

The goal is to keep controllers thin, concentrate business logic inside services, and treat MongoDB infrastructure as an implementation detail hidden behind the service layer. Cross-cutting aspects such as error handling and ObjectId parsing live in dedicated components.

## Domain Layer (`/Domain`)

| Path                                         | Description                                                                                         |
| -------------------------------------------- | --------------------------------------------------------------------------------------------------- |
| `Domain/Models/BaseDocument.cs`              | Base class for documents with `Id`, `CreatedAt`, `UpdatedAt` set in UTC.                            |
| `Domain/Models/Movie.cs`                     | Movie aggregate with genre, cast, images, budget, etc. Uses `DateOnlySerializer` for release dates. |
| `Domain/Models/User.cs`                      | User aggregate with password hash, role, and references to movie lists.                             |
| `Domain/Models/MoviesList.cs`                | Represents a curated list of movies identified by ObjectIds.                                        |
| `Domain/Enums/*.cs`                          | Enumeration definitions (genres, roles, cast roles).                                                |
| `Domain/ValueObjects/CastMember.cs`          | Value object used in a movie cast.                                                                  |
| `Domain/Serialization/DateOnlySerializer.cs` | Custom BSON serializer to store `DateOnly` as midnight UTC instants.                                |

These types are plain C# objects without knowledge of HTTP or persistence specifics.

## API Layer (`/Api`)

### DTOs and Mappings

- `Api/DTOs/*.cs` define the API contract (Create/Update/Response DTOs).
- `Api/DTOs/MappingExtensions.cs` contains extension methods to convert between domain models and DTOs. Controllers only deal with DTOs, while services operate on domain models.

### Services

Interfaces live in `Api/Services/Interfaces` and describe the CRUD surface for each aggregate. Implementations in `Api/Services/*.cs` use `IMongoCollection<T>`:

- `MovieService` handles movies, updating timestamps and replacing documents.
- `UserService` enforces unique usernames/emails before write operations.
- `MoviesListService` validates referenced movie ids before persisting a list.

### Controllers

REST endpoints are exposed via:

- `Api/Controllers/MoviesController.cs` → `/api/movies`
- `Api/Controllers/UsersController.cs` → `/api/users`
- `Api/Controllers/ListsController.cs` → `/api/lists`

Actions accept DTOs, delegate to services, and return DTOs. They do not contain manual validations or try/catch blocks thanks to the shared middleware and binder.

## Authentication & Authorization

JWT Bearer authentication is configured and integrated into the pipeline. Authentication endpoints:

- `POST /api/auth/register` (anonymous): Creates a user (default role is `User`) and returns a JWT plus its expiration.
- `POST /api/auth/login` (anonymous): Authenticates a user and returns a JWT plus its expiration.
- `GET /api/auth/me` (authenticated): Returns the user associated with the provided token.
- `POST /api/auth/refresh` (anonymous): Exchanges an about-to-expire access token for a fresh one. The action validates the token signature but ignores expiration.

Additional admin-only workflows:

- `POST /api/users` (requires `AdminOnly` policy): Allows an administrator to create additional users by supplying `UserCreateAdminDto`. The request can specify a target role (`User` or `Admin`).
- All movie and movie-list management endpoints (`POST/PUT/DELETE`) require the caller to be an administrator; read operations stay anonymous.

### Error Handling & Binding

- `Api/Common/Errors.cs` introduces `HttpException` and specific derivatives (`NotFoundException`, `ConflictException`, `ValidationException`, `BadRequestException`). Services and infrastructure throw these to signal business conditions.
- `Api/Middleware/ErrorHandlingMiddleware.cs` catches `HttpException` and converts them into JSON responses with the correct HTTP status code. Unexpected exceptions become HTTP 500 with a generic message (and are logged).
- `Api/Binders/ObjectIdModelBinder.cs` plus `ObjectIdModelBinderProvider.cs` ensure that route parameters typed as `ObjectId` are automatically parsed. Invalid ids trigger a `BadRequestException` that is handled by the middleware.

## Dependency Injection & Configuration (`Program.cs`)

- Registers Razor components (existing Blazor UI) so the server-side front end remains functional beside the API.
- Adds controllers and inserts the custom ObjectId binder.
- Wires MongoDB dependencies:
  - `IMongoClient` uses connection string `Mongo:ConnectionString` (default `mongodb://localhost:27017`).
  - `IMongoDatabase` uses `Mongo:Database` (default `moviehub`).
  - Individual `IMongoCollection<T>` registrations for movies, users, and lists.
- Registers scoped services for each aggregate (`MovieService`, `UserService`, `MoviesListService`).
- Adds `ErrorHandlingMiddleware` before antiforgery/static assets so every request benefits from consistent error responses.
- Maps controllers and keeps the existing Blazor pipeline intact.
- Enables authentication/authorization (`UseAuthentication` / `UseAuthorization`) and configures JWT options via configuration or secrets.
- Creates case-insensitive unique indexes on `users.username` and `users.email` during startup (best effort — failures are swallowed).

## Error Responses

| Scenario                      | Exception             | HTTP | Payload                                         |
| ----------------------------- | --------------------- | ---- | ----------------------------------------------- |
| Entity not found              | `NotFoundException`   | 404  | `{ "message": "..." }`                          |
| Uniqueness violation          | `ConflictException`   | 409  | `{ "message": "..." }`                          |
| Validation failure (business) | `ValidationException` | 422  | `{ "message": "..." }`                          |
| Invalid route id              | `BadRequestException` | 400  | `{ "message": "..." }`                          |
| Unhandled exception           | n/a                   | 500  | `{ "message": "An unexpected error occurred" }` |

## Extending the Backend

1. **Add new aggregate**
   - Create model/value objects/enums under `Domain`.
   - Create DTOs + mappings mirroring the domain type.
   - Add service interface/implementation following existing patterns (use Mongo collection and throw the appropriate `HttpException`).
   - Add controller exposing the HTTP contract.
2. **Add Mongo indexes**
   - Já implementado: índices únicos (case-insensitive) para `users.username` e `users.email` no startup.
3. **Authentication/Authorization**
   - Já configurado. Próximos incrementos: políticas "SelfOrAdmin" dedicadas, refresh tokens e revogação.
4. **Testing**
   - Create integration tests using `WebApplicationFactory` and either an in-memory Mongo instance (e.g., Mongo2Go) or a test container.

## Running Locally

1. Provide MongoDB connection details in `appsettings.Development.json`:

   ```json
   {
     "Mongo": {
       "ConnectionString": "mongodb://localhost:27017",
       "Database": "moviehub"
     }
   }
   ```

2. Restore & build: `dotnet restore && dotnet build`.
3. Run: `dotnet run` (API available at `https://localhost:5001`, UI at the root).
4. Test endpoints with cURL/Postman using the DTO shapes defined in `/Api/DTOs`.

### Container image

A multi-stage `Dockerfile` lives at the repository root. Publishing the image produces a self-contained ASP.NET runtime image that honors the `$PORT` environment variable (required for Render deployment). Example:

```bash
docker build -t moviehub-backend .
docker run --rm -it -e PORT=8080 -p 8080:8080 moviehub-backend
```

### Indexes

During startup unique indexes are created for `users.username` and `users.email` with collation `strength = 2` to keep comparisons case-insensitive.

## Notable Implementation Details

- **Role-aware registration** – `AuthService.Register` accepts an optional `UserRole` parameter. Regular signup calls set it to `User`, while the admin-only `/api/users` pipeline can create new administrators.
- **Token refresh** – `AuthTokenHandler` in the front end leverages the `/api/auth/refresh` endpoint, which in turn rehydrates the principal and issues a new token without forcing a logout.
- **Central error contracts** – All services throw `HttpException` derivations so the API surface always responds with `{ "message": "..." }`, easing front-end error handling.

This document should give new contributors enough context to navigate the backend architecture quickly.
