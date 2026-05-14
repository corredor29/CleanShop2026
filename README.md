# CleanShop 2026

API REST construida en .NET 10 con arquitectura por capas (Clean Architecture): `Api`, `Application`, `Domain` e `Infrastructure`.

## Estructura del proyecto

```text
cleanshop2026/
├─ Api/                # Capa de presentación (controllers, DTOs, configuración web)
├─ Application/        # Casos de uso, validaciones, contratos (abstracciones)
├─ Domain/             # Modelo de dominio (entidades, value objects, enums)
├─ Infrastructure/     # Persistencia EF Core, repositorios, Unit of Work, migraciones
└─ cleanshop.slnx      # Solución
```

## Qué hace cada capa

### `Domain`
- Contiene la lógica de negocio central y reglas del dominio.
- Incluye el agregado `Product` y value objects como `Sku`, `Money`, `Email`, `Address`.
- No depende de otras capas.

### `Application`
- Implementa casos de uso con MediatR (por ejemplo `CreateProduct`, `UpdateProduct`).
- Define contratos (`IUnitOfWork`, `IProduct`) que la infraestructura implementa.
- Registra validadores con FluentValidation.

### `Infrastructure`
- Implementa acceso a datos con EF Core + PostgreSQL (`AppDbContext`).
- Implementa repositorios y `EfUnitOfWork`.
- Incluye migraciones en `Infrastructure/Data/Migrations`.

### `Api`
- Expone endpoints HTTP (por ejemplo `ProductsController`).
- Mapea DTOs y comandos con Mapster.
- Configura CORS, Swagger/OpenAPI y DI de las demás capas.

## Flujo de una petición (ejemplo: crear producto)

1. `Api/Controllers/ProductsController` recibe el `POST`.
2. Se mapea `CreateProductRequest` -> comando `CreateProduct`.
3. MediatR envía el comando a `CreateProductHandler` (Application).
4. El handler usa `IUnitOfWork` y `IProduct` para validar y persistir.
5. `Infrastructure` guarda con EF Core en PostgreSQL.
6. La API responde `201 Created` con el `id` creado.

## Dependencias principales

- .NET `net10.0`
- ASP.NET Core
- MediatR
- FluentValidation
- EF Core
- Npgsql (PostgreSQL)
- Mapster
- Swagger / OpenAPI

## Configuración y ejecución

### 1) Requisitos
- SDK de .NET 10
- PostgreSQL disponible

### 2) Configurar conexión
Actualmente `Infrastructure.DependencyInjection` espera una cadena llamada `Postgres`.

Agrega en `Api/appsettings.Development.json` (o por variables de entorno):

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=cleanshop;Username=postgres;Password=postgres"
  }
}
```

### 3) Restaurar y ejecutar

```bash
dotnet restore
dotnet run --project Api/Api.csproj
```

### 4) Documentación API
En ambiente de desarrollo queda habilitado Swagger/OpenAPI al ejecutar la API.

## Notas

- Se usa `QueryTrackingBehavior.NoTracking` por defecto en el `DbContext`.
- Para operaciones de escritura, los repositorios marcan explícitamente `Add/Update/Remove` y `SaveChangesAsync` se ejecuta vía Unit of Work.
- Las migraciones actuales están en `Infrastructure/Data/Migrations`.