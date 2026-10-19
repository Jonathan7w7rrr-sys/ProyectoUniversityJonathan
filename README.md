# ProyectoUniversityJonathan

Proyecto académico de ASP.NET Core Web API con SQL Server.

## Estado

Web API con .NET 10 y Entity Framework Core 10. Incluye cuatro controladores CRUD, validaciones y migraciones para SQL Server.

## Ejecutar en Visual Studio

1. Abrir `ProyectoUniversityJonathan.sln`.
2. Seleccionar `University.Api` como proyecto de inicio y el perfil `http`.
3. Restaurar los paquetes NuGet.
4. Desde una terminal en la carpeta del repositorio, ejecutar:

```powershell
dotnet tool restore
dotnet ef database update --project src/University.Api
dotnet run --project src/University.Api
```

Abrir http://localhost:5180/swagger. En Visual Studio también se puede iniciar con F5.

La conexión usa `localhost`, autenticación de Windows y la base `UniversityJonathanDb`. `TrustServerCertificate=True` se utiliza exclusivamente para el certificado del servidor local de prácticas. La aplicación escucha solo en localhost y no incluye autenticación de usuarios; está preparada para demostración local.

## Modelo de datos

`UniversityDbContext` expone `Cursos`, `Estudiantes`, `Docentes` y `Asignaciones` mediante `DbSet`. Cada asignación referencia un curso, estudiante y docente. Las eliminaciones de registros referenciados devuelven 409; las referencias inexistentes y los datos inválidos devuelven 400. No se eliminan asignaciones en cascada.

## Rutas

Las rutas `/api/cursos`, `/api/estudiantes`, `/api/docentes` y `/api/asignaciones` aceptan GET y POST. Las rutas con `/{id}` aceptan GET, PUT y DELETE. Swagger muestra los cuerpos JSON y permite probarlas. Los identificadores se generan en SQL Server.

Ejemplo POST `/api/cursos`:

```json
{"nombre":"Desarrollo web"}
```

## Verificación

Con la aplicación ejecutándose, `pwsh -File tests/Smoke-Test.ps1` prueba creación, consulta, actualización, eliminación, persistencia y errores esperados. Crea datos temporales y elimina únicamente esos datos al terminar.

## Documentación técnica

- https://learn.microsoft.com/ef/core/dbcontext-configuration/
- https://learn.microsoft.com/ef/core/providers/sql-server/

## Clases previstas

- Curso
- Estudiante
- Docente
- Asignacion

El plan de propiedades, relaciones y controladores está en [docs/ClasesYControladores.md](docs/ClasesYControladores.md).
