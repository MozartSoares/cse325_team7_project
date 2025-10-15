# Frontend Architecture Overview

The UI is a server-side Blazor application that lives alongside the Web API inside the same ASP.NET host. Razor components reside under `Components/` and are rendered via the existing layout pipeline configured in `Program.cs`.

## Layout & Routing

| Path | Purpose |
| --- | --- |
| `Components/Routes.razor` | Central route table – maps URLs to pages. |
| `Components/Layout/MainLayout.razor` | Shell hosting the navigation menu, footer, and the `AuthDialogHost`. |
| `Components/Layout/NavMenu.razor` | Top navigation bar with login/register entry points wired through `AuthDialogService`. |
| `Components/App.razor` | Root component hosting the layout and cascading authentication state. |

## Key Pages

| Component | Highlights |
| --- | --- |
| `Components/Pages/Home.razor` | Marketing hero plus embedded `Movies` list. When the signed-in user is an admin a “Add New Movie” button appears, reusing the shared modal component for create operations. |
| `Components/Pages/Movies.razor` | Fetches movie data from `/api/movies`, exposes search, advanced filtering, and pagination. Styling lives in the adjacent `.css`. |
| `Components/Pages/MovieDetails.razor` | Shows movie metadata, list membership management, and (for admins) an “Edit Details” modal. |
| `Components/Pages/User.razor` | Profile display with edit/delete flows. Admins see the “Create Admin User” shortcut reusing the registration modal. |
| `Components/Pages/MovieList.razor` | Displays user-created lists by projecting list IDs into `MovieListCollection` components. |

Other samples (Weather, Counter) remain from the initial template but are not used by the app.

## Shared Components & Services

- `Components/Shared/Modal.razor`: Generic modal layout reused by registration, admin movie creation, edit forms, and confirmation dialogs. Width/scrolling is responsive as of the latest iteration.
- `Components/Shared/Auth/AuthDialogHost.razor`: Container that listens for login/register requests and renders the corresponding modals.
- `Components/Shared/Auth/LoginModal.razor` & `RegisterModal.razor`: Forms that talk to the auth endpoints through `AuthApiClient`. `RegisterModal` accepts parameters to toggle between end-user signup and admin-only user creation flows.
- `Components/Shared/MovieListCollection.razor`: Small component that fetches list entries and renders movie cards inside a list context.
- `Components/Services/AuthStateService`: Persists authentication state in `localStorage`, refreshes tokens, and exposes `AuthStateChanged` so UI reacts to role changes.
- `Components/Services/AuthDialogService`: Lightweight pub/sub to show auth modals from anywhere (e.g., nav menu, home CTA).
- `Components/Services/AuthTokenHandler`: Delegating handler that injects the bearer token into outgoing API calls; also triggers proactive refresh using `/api/auth/refresh`.
- `Components/Services/AuthApiClient`: Wrapper around `HttpClient` for `login` and `register` endpoints – keeps error parsing logic in one place.

## Authentication UX

1. Anonymous visitors can register or log in via the navigation buttons or hero CTA. The modals use `AuthApiClient` and persist tokens through `AuthStateService`.
2. Once authenticated, admin role is detected via JWT claims. The UI gatekeeps privileged flows:
   - **Create Admin User**: Visible on `/user` profile pages if the current user has the `Admin` role. The modal posts to `POST /api/users`.
   - **Add New Movie**: Visible on the home page for admins. The modal posts to `POST /api/movies` with full movie metadata (including optional cast entries).
  - **Edit/Delete Movie**: Located on the movie detail page; only admins see the modal trigger.
3. `AuthTokenHandler` ensures API calls include the bearer token and will attempt a refresh if the response returns 401/403.

## Styling

- Component-scoped CSS files sit next to their `.razor` counterparts (`*.razor.css`). The build pipeline scopes class names automatically.
- Shared visual styles:
  - `Components/Shared/Modal.razor.css` defines the modal shell used across the app.
  - `Components/Layout/*.css` provides layout, navigation, and footer styling.
  - `Components/Pages/*.css` files hold feature-specific visuals (e.g., hero banner, movie cards, profile view).

## Data Fetching Patterns

- `HttpClient` instances are registered in `Program.cs`:
  - Named client `"AppClient"` includes the `AuthTokenHandler` so most page-level components simply call `HttpClient.GetFromJsonAsync`.
  - Named client `"AuthRefresh"` is used internally by `AuthTokenHandler` for silent refresh calls.
- Most page components call the API in `OnInitializedAsync`, caching the result in component state and showing placeholder data if the call fails (see `Movies.razor` and `User.razor` examples).

## Local Development Notes

1. Run the combined backend/frontend via `dotnet run`. Server-side Blazor and the REST API share the same Kestrel host.
2. Ensure the MongoDB connection string is available (appsettings or secrets) so API calls made by the Blazor components succeed.
3. Frontend changes (Razor/CSS) support hot reload when using `dotnet watch run`.

## Adding New UI Features

1. Create the component under `Components/Pages` or `Components/Shared` as appropriate.
2. Register the route in `Routes.razor` if it is a new page.
3. Use dependency injection for services (`@inject`) to obtain `HttpClient`, `AuthStateService`, or any custom services.
4. For modal experiences, wrap the UI inside the shared `<Modal>` component to inherit consistent spacing and keyboard behavior.
5. Add a `*.razor.css` file for scoped styling; avoid global selectors unless absolutely necessary.

This document provides a high-level tour so contributors can confidently navigate and extend the Blazor UI. For API details, see `docs/BACKEND.md`.
