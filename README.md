# MobileDevelopment - FitTracker

ENG

FitTracker is a full-stack fitness mobile application built with a React Native client and an ASP.NET Core Web API backend. The app supports workout logging, diet tracking, gyms, exercises, achievements, user profiles, health calculators, Redis-backed caching, and API observability through health checks and Scalar documentation.

## Features

- User authentication with JWT access tokens and refresh tokens.
- Workout sessions with sets, RPE, duration, historical dates, and dashboard summaries.
- Exercise catalog with muscle groups, difficulty, images, user-created exercises, and filtering.
- Diet plans, diet days, meals, macro assumptions, and daily nutrition summaries.
- Gym directory with user/admin-created gyms.
- Profile view with avatar upload, monthly workout statistics, average workout time, achievements, and theme switching.
- Achievement system with background processing.
- Health calculators: BMI, one-rep max, BMR, YMCA body fat, and ideal weight.
- Distributed caching with Redis and in-memory fallback.
- API health checks, Scalar API documentation, Serilog logging, CQRS with MediatR, and FluentValidation.

## Tech Stack

### Backend

- ASP.NET Core Web API, .NET 10
- Entity Framework Core
- SQL Server 2022
- Redis 7
- MediatR / CQRS
- FluentValidation
- Serilog
- Scalar API reference
- ASP.NET Core Health Checks
- Background services for token cleanup and achievements
- Docker / Docker Compose

### Mobile Client

- React Native 0.84
- React 19
- TypeScript
- React Navigation
- Axios
- Zustand
- AsyncStorage
- React Native Vector Icons
- React Native Image Picker

## Project Structure

```text
.
├── API/
│   ├── MobileDevelopment.API/              # ASP.NET Core API entrypoint
│   ├── MobileDevelopment.API.Domain/       # Domain entities, enums, auth settings
│   ├── MobileDevelopment.API.Models/       # DTOs, wrappers, pagination
│   ├── MobileDevelopment.API.Persistence/  # EF Core DbContext, repositories, migrations, seeding
│   ├── MobileDevelopment.API.Services/     # Services, CQRS commands/queries, calculators, background workers
│   └── MobileDevelopment.API.UnitTests/    # xUnit tests
├── Client/                                 # React Native mobile app
└── compose.yaml                            # API + SQL Server + Redis
```

## Requirements

- Docker Desktop
- .NET 10 SDK
- Node.js 22.11+
- npm
- Android Studio / Android emulator for Android development
- Xcode / iOS Simulator for iOS development on macOS

## Quick Start With Docker

The easiest way to run the backend stack is Docker Compose. It starts:

- API on `http://localhost:8080`
- SQL Server on host port `1434`
- Redis on host port `6379`

From the repository root:

```bash
docker compose up --build
```

The API applies EF Core migrations automatically on startup and seeds sample data.

Useful backend URLs:

- API base: `http://localhost:8080/api/v1/mobile`
- Health check: `http://localhost:8080/health`
- Readiness check: `http://localhost:8080/health/ready`
- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- Scalar docs: `http://localhost:8080/scalar/v1`

If you want to stop the stack:

```bash
docker compose down
```

To remove volumes as well:

```bash
docker compose down -v
```

## Running the API Locally

You can also run the API directly with the .NET SDK while keeping SQL Server and Redis in Docker.

1. From the repository root, start SQL Server and Redis:

```bash
docker compose up db redis
```

1. Configure connection strings and JWT settings. The Docker Compose defaults are:

```text
ConnectionStrings__MsSqlDb=Server=localhost,1434;Database=MobileDevelopmentDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;MultipleActiveResultSets=true
ConnectionStrings__Redis=localhost:6379
Jwt__SecretKey=CHANGE-ME-TO-A-SECURE-KEY-AT-LEAST-32-CHARS-LONG!!
Jwt__Issuer=FitTrackerAPI
Jwt__Audience=FitTrackerMobileClient
```

1. In a second terminal, run the API:

```bash
cd API
dotnet run --project MobileDevelopment.API/MobileDevelopment.API.csproj
```

1. Run tests:

```bash
dotnet test MobileDevelopment.API.UnitTests/MobileDevelopment.API.UnitTests.csproj
```

## Running the Mobile App

Install dependencies:

```bash
cd Client
npm install
```

Start Metro:

```bash
npm start
```

Run Android:

```bash
npm run android
```

Run iOS:

```bash
npm run ios
```

### API URL Configuration

The mobile app API base URL is configured in:

```text
Client/src/api/api-config.tsx
```

Current development defaults:

- Android: `http://192.168.0.10:8080/api/v1/mobile`
- iOS: `http://localhost:8080/api/v1/mobile`

If your API runs on a different machine or your local network IP changes, update the Android URL. Android emulators often need either your host LAN IP or `10.0.2.2`, depending on how the app is run.

## Example Application Flow

1. Register or log in.
2. The API returns JWT tokens and the client stores authentication state.
3. The dashboard loads the current user's workout and diet summary.
4. The user creates a workout session and adds sets with exercises, weight, reps, duration, and RPE.
5. The backend recalculates workout/session statistics and invalidates related dashboard cache entries.
6. The user returns to Dashboard or Profile, and the client refreshes current data on screen focus.
7. Background workers periodically clean expired/revoked refresh tokens and evaluate achievements.
8. Achievements become visible in the profile and achievements views.

Other common flows:

- Create a diet plan, define daily calorie and macro assumptions, then add meals to diet days.
- Browse exercises by muscle group, create custom exercises, and use them in workout sessions.
- Browse gyms, create user gyms, and navigate to gyms that have coordinates.
- Use Tools to calculate BMI, one-rep max, BMR, YMCA body fat, or ideal weight.

## API Documentation

In Development mode, the API exposes OpenAPI and Scalar:

- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- Scalar UI: `http://localhost:8080/scalar/v1`

Scalar is useful for browsing and testing mobile API endpoints directly from the browser.

## Health Checks

The API exposes:

```text
GET /health
GET /health/ready
```

The current health check setup includes the EF Core database check. These endpoints are useful for Docker, deployment probes, and quick local diagnostics.

## Caching

The backend uses `IDistributedCache` through a custom `ICacheService`.

- Redis is used when `ConnectionStrings:Redis` is configured.
- In-memory distributed cache is used as fallback.
- Versioned cache areas are used for predictable invalidation.

Currently cached areas include examples such as:

- Dashboard summary per user
- Exercises and muscle groups
- Gyms
- Achievements

`OutputCache` middleware is enabled, but the project primarily uses service-level cache because most mobile endpoints are authenticated and user-specific.

## Logging

Logging is configured with Serilog in:

```text
API/MobileDevelopment.API/appsettings.json
```

Logs are written to:

- Console
- Rolling files under `Logs/log-.txt`

The API also contains a request time middleware that logs slow requests with structured logging.

## Database and Seeding

EF Core migrations are stored in:

```text
API/MobileDevelopment.API.Persistence/Migrations
```

The database is seeded with sample users, profiles, exercises, muscle groups, gyms, diets, workouts, achievements, posts, and related data.

On API startup, migrations are applied automatically:

```csharp
context.Database.Migrate();
```

## Testing and Linting

Backend tests:

```bash
cd API
dotnet test MobileDevelopment.API.UnitTests/MobileDevelopment.API.UnitTests.csproj
```

Backend build:

```bash
cd API
dotnet build MobileDevelopment.API/MobileDevelopment.API.csproj
```

Client lint:

```bash
cd Client
npm run lint
```

Client tests:

```bash
cd Client
npm test
```

## Notes for Development

- Keep CQRS handlers thin. Business logic should live in services, with persistence hidden behind repositories.
- Use DTOs from `MobileDevelopment.API.Models` for API contracts.
- Use `Result<T>` wrappers consistently in API responses.
- Keep user-specific data out of generic output cache policies unless the cache key varies safely per user.
- Update both API DTOs and client TypeScript interfaces when changing API contracts.

===========================================================================================================================================================================================

PL

FitTracker to aplikacja fitness składająca się z aplikacji mobilnej React Native oraz backendu ASP.NET Core Web API. Projekt obsługuje dziennik treningowy, dietę, siłownie, ćwiczenia, osiągnięcia, profil użytkownika, kalkulatory zdrowotne, cache z Redisem oraz podstawową obserwowalność API przez health checki i dokumentację Scalar.


## Funkcjonalności

- Logowanie i rejestracja użytkowników z JWT oraz refresh tokenami.
- Tworzenie treningów, serii, RPE, czasu trwania i historycznych dat treningu.
- Katalog ćwiczeń z grupami mięśniowymi, poziomem trudności, zdjęciami i filtrowaniem.
- Plany diet, dni diety, posiłki, makroskładniki i dzienne podsumowania.
- Lista siłowni z możliwością dodawania przez użytkownika lub administratora.
- Profil użytkownika ze zdjęciem, statystykami treningowymi, osiągnięciami i zmianą motywu.
- System osiągnięć obsługiwany przez background worker.
- Kalkulatory: BMI, 1RM, BMR, YMCA body fat oraz idealna waga.
- Rozproszony cache z Redisem oraz fallbackiem do pamięci.
- Health checki, Scalar, Serilog, CQRS z MediatR i FluentValidation.

## Stack technologiczny

### Backend

- ASP.NET Core Web API, .NET 10
- Entity Framework Core
- SQL Server 2022
- Redis 7
- MediatR / CQRS
- FluentValidation
- Serilog
- Scalar API Reference
- ASP.NET Core Health Checks
- Background services
- Docker / Docker Compose

### Klient mobilny

- React Native 0.84
- React 19
- TypeScript
- React Navigation
- Axios
- Zustand
- AsyncStorage
- React Native Vector Icons
- React Native Image Picker

## Struktura projektu

```text
.
├── API/
│   ├── MobileDevelopment.API/              # Główna aplikacja ASP.NET Core API
│   ├── MobileDevelopment.API.Domain/       # Encje domenowe, enumy, ustawienia auth
│   ├── MobileDevelopment.API.Models/       # DTO, wrappery odpowiedzi, paginacja
│   ├── MobileDevelopment.API.Persistence/  # DbContext, repozytoria, migracje, seeding
│   ├── MobileDevelopment.API.Services/     # Serwisy, CQRS, kalkulatory, workery
│   └── MobileDevelopment.API.UnitTests/    # Testy xUnit
├── Client/                                 # Aplikacja mobilna React Native
└── compose.yaml                            # API + SQL Server + Redis
```

## Wymagania

- Docker Desktop
- .NET 10 SDK
- Node.js 22.11+
- npm
- Android Studio / emulator Androida
- Xcode / iOS Simulator dla iOS na macOS

## Szybkie uruchomienie przez Docker

Najprościej uruchomić backend przez Docker Compose. Startowane są:

- API: `http://localhost:8080`
- SQL Server: port hosta `1434`
- Redis: port hosta `6379`

Z katalogu głównego repozytorium:

```bash
docker compose up --build
```

API automatycznie uruchamia migracje EF Core i seeduje przykładowe dane.

Przydatne adresy:

- API base: `http://localhost:8080/api/v1/mobile`
- Health check: `http://localhost:8080/health`
- Readiness check: `http://localhost:8080/health/ready`
- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- Scalar docs: `http://localhost:8080/scalar/v1`

Zatrzymanie kontenerów:

```bash
docker compose down
```

Zatrzymanie i usunięcie wolumenów:

```bash
docker compose down -v
```

## Lokalne uruchomienie API

Możesz uruchomić API bezpośrednio przez .NET SDK, zostawiając SQL Server i Redis w Dockerze.

1. Z katalogu głównego repozytorium uruchom SQL Server i Redis:

```bash
docker compose up db redis
```

1. Skonfiguruj connection stringi i JWT. Domyślne wartości z Docker Compose:

```text
ConnectionStrings__MsSqlDb=Server=localhost,1434;Database=MobileDevelopmentDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;MultipleActiveResultSets=true
ConnectionStrings__Redis=localhost:6379
Jwt__SecretKey=CHANGE-ME-TO-A-SECURE-KEY-AT-LEAST-32-CHARS-LONG!!
Jwt__Issuer=FitTrackerAPI
Jwt__Audience=FitTrackerMobileClient
```

1. W drugim terminalu uruchom API:

```bash
cd API
dotnet run --project MobileDevelopment.API/MobileDevelopment.API.csproj
```

1. Uruchom testy:

```bash
dotnet test MobileDevelopment.API.UnitTests/MobileDevelopment.API.UnitTests.csproj
```

## Uruchomienie aplikacji mobilnej

Instalacja zależności:

```bash
cd Client
npm install
```

Start Metro:

```bash
npm start
```

Android:

```bash
npm run android
```

iOS:

```bash
npm run ios
```

### Konfiguracja adresu API

Adres API dla aplikacji mobilnej znajduje się w:

```text
Client/src/api/api-config.tsx
```

Domyślne wartości developerskie:

- Android: `http://192.168.0.10:8080/api/v1/mobile`
- iOS: `http://localhost:8080/api/v1/mobile`

Jeśli API działa na innym adresie lub zmieni się IP komputera w sieci lokalnej, trzeba zaktualizować adres dla Androida. W emulatorze Androida czasami używa się również `10.0.2.2`, zależnie od konfiguracji.

## Przykładowe flow aplikacji

1. Użytkownik rejestruje konto albo loguje się.
2. API zwraca tokeny JWT, a klient zapisuje stan autoryzacji.
3. Dashboard pobiera podsumowanie treningów i diety aktualnego użytkownika.
4. Użytkownik tworzy trening i dodaje serie z ćwiczeniami, ciężarem, powtórzeniami, czasem oraz RPE.
5. Backend przelicza statystyki treningu i unieważnia powiązane wpisy cache.
6. Po powrocie na Dashboard lub Profil klient odświeża dane przy fokusie ekranu.
7. Workery w tle czyszczą stare tokeny i sprawdzają osiągnięcia.
8. Odblokowane osiągnięcia są widoczne w profilu i na ekranie osiągnięć.

Inne typowe flow:

- Utworzenie planu diety, ustawienie kalorii i makro, a następnie dodawanie posiłków.
- Przeglądanie ćwiczeń po grupach mięśniowych i tworzenie własnych ćwiczeń.
- Przeglądanie siłowni i nawigacja do siłowni z uzupełnionymi współrzędnymi.
- Korzystanie z kalkulatorów BMI, 1RM, BMR, body fat i idealnej wagi.

## Dokumentacja API

W trybie Development API udostępnia OpenAPI i Scalar:

- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- Scalar UI: `http://localhost:8080/scalar/v1`

Scalar pozwala wygodnie przeglądać i testować endpointy API z poziomu przeglądarki.

## Health checki

API udostępnia:

```text
GET /health
GET /health/ready
```

Obecnie health check sprawdza m.in. połączenie z bazą danych przez EF Core. Endpointy są przydatne do lokalnej diagnostyki, Docker Compose oraz deployment probes.

## Cache

Backend używa `IDistributedCache` przez własny `ICacheService`.

- Redis jest używany, gdy skonfigurowano `ConnectionStrings:Redis`.
- Jeśli Redis nie jest skonfigurowany, aplikacja używa cache w pamięci.
- Cache jest wersjonowany obszarami, co ułatwia unieważnianie danych po zapisach.

Przykładowe obszary cache:

- Dashboard użytkownika
- Ćwiczenia i grupy mięśniowe
- Siłownie
- Osiągnięcia

Middleware `OutputCache` jest włączony, ale główny cache aplikacji jest na poziomie serwisów, ponieważ większość endpointów mobilnych jest autoryzowana i zależna od użytkownika.

## Logowanie

Logowanie jest skonfigurowane przez Serilog w:

```text
API/MobileDevelopment.API/appsettings.json
```

Logi trafiają do:

- konsoli,
- plików rotowanych `Logs/log-.txt`.

API ma również middleware mierzący czas żądań i logujący wolniejsze requesty.

## Baza danych i seeding

Migracje EF Core znajdują się w:

```text
API/MobileDevelopment.API.Persistence/Migrations
```

Baza jest seedowana przykładowymi użytkownikami, profilami, ćwiczeniami, grupami mięśniowymi, siłowniami, dietami, treningami, osiągnięciami, postami i powiązanymi danymi.

Przy starcie API migracje są aplikowane automatycznie:

```csharp
context.Database.Migrate();
```

## Testy i lint

Testy backendu:

```bash
cd API
dotnet test MobileDevelopment.API.UnitTests/MobileDevelopment.API.UnitTests.csproj
```

Build backendu:

```bash
cd API
dotnet build MobileDevelopment.API/MobileDevelopment.API.csproj
```

Lint klienta:

```bash
cd Client
npm run lint
```

Testy klienta:

```bash
cd Client
npm test
```

## Notatki developerskie

- Handlery CQRS powinny być możliwe jak najmniej rozbudowane. Logika biznesowa powinna żyć w serwisach, a dostęp do bazy w repozytoriach.
- Kontrakty API powinny korzystać z DTO z `MobileDevelopment.API.Models`.
- Odpowiedzi API powinny konsekwentnie używać wrapperów `Result<T>`.
- Dane zależne od użytkownika nie powinny trafiać do ogólnego OutputCache bez bezpiecznego vary po użytkowniku.
- Przy zmianach kontraktów API trzeba aktualizować zarówno DTO backendu, jak i typy TypeScript w kliencie.
