# Clases y controladores de ProyectoUniversityJonathan

Este documento define las clases y los controladores que implementaremos en la Web API universitaria de ASP.NET Core, de acuerdo con el diagrama de la actividad. Es un plan de implementación; no acredita que el código ya esté generado o funcionando.

## Clases del dominio

Los nombres de las propiedades proceden del diagrama. Los tipos C# son una propuesta: identificadores enteros y textos string. Las longitudes máximas y la obligatoriedad de los textos se definirán al implementar.

| Clase | Propiedad | Tipo propuesto | Función |
| --- | --- | --- | --- |
| Curso | CursoId | int | Clave primaria |
| Curso | Nombre | string | Nombre del curso |
| Estudiante | EstudianteId | int | Clave primaria |
| Estudiante | Nombres | string | Nombres del estudiante |
| Estudiante | Apellidos | string | Apellidos del estudiante |
| Estudiante | Direccion | string | Dirección del estudiante |
| Docente | DocenteId | int | Clave primaria |
| Docente | Nombres | string | Nombres del docente |
| Docente | Apellidos | string | Apellidos del docente |
| Asignacion | AsignacionId | int | Clave primaria |
| Asignacion | EstudianteId | int | Clave foránea a Estudiante |
| Asignacion | CursoId | int | Clave foránea a Curso |
| Asignacion | DocenteId | int | Clave foránea a Docente |

## Relaciones entre clases

Cada Asignacion vincula un estudiante, un curso y un docente. Cada estudiante, curso o docente puede tener cero o muchas asignaciones. Se proponen claves foráneas obligatorias para esas tres referencias.

Para Entity Framework Core, Asignacion tendrá las propiedades de navegación Estudiante, Curso y Docente. Las otras tres clases tendrán una colección Asignaciones. Estas propiedades permiten recorrer las relaciones y no añaden campos independientes al diagrama.

## Controladores correspondientes

Se implementarán CursosController, EstudiantesController, DocentesController y AsignacionesController. Cada controlador expondrá operaciones para consultar, crear, actualizar y eliminar registros de su clase.

| Controlador | Ruta base | Clase administrada |
| --- | --- | --- |
| CursosController | /api/cursos | Curso |
| EstudiantesController | /api/estudiantes | Estudiante |
| DocentesController | /api/docentes | Docente |
| AsignacionesController | /api/asignaciones | Asignacion |

## Operaciones de los controladores

Las siguientes operaciones se aplican a las cuatro rutas base. El parámetro {id} representa la clave primaria del registro.

| Método | Ruta relativa | Operación | Respuesta prevista |
| --- | --- | --- | --- |
| GET | / | Consultar todos | 200 OK |
| GET | /{id} | Consultar uno | 200 OK o 404 Not Found |
| POST | / | Crear | 201 Created o 400 Bad Request |
| PUT | /{id} | Actualizar | 204 No Content, 400 o 404 |
| DELETE | /{id} | Eliminar | 204 No Content o 404 |

AsignacionesController verificará que existan el estudiante, el curso y el docente antes de guardar. Una referencia inexistente devolverá 400 Bad Request con un mensaje claro. Para proteger las relaciones, se propone impedir eliminar cursos, estudiantes o docentes con asignaciones asociadas y responder 409 Conflict.

No se añadirá una restricción de asignaciones duplicadas sin definir antes la regla de la actividad: el diagrama no establece una clave única compuesta. Las respuestas deberán evitar referencias circulares al incluir relaciones.
