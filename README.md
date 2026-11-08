# 4VGym API con Entity Framework

Proyecto de Acceso a Datos que junta los dos proyectos de clase:
- **4VGymAPI**: la API, que antes guardaba los datos en listas y diccionarios.
- **4VGymEF**: los modelos y la base de datos con Entity Framework y SQL Server.

La API tiene los mismos endpoints que 4VGymAPI, pero ahora coge los datos de la base de datos.

## Endpoints
- `GET /activity-types`: lista de tipos de actividad
- `POST /activity-types`: crear tipo de actividad
- `GET /instructors`: lista de instructores (se puede filtrar con `id` o `first_initial`)
- `POST /instructors`: crear instructor
- `POST /activities`: crear actividad

## Cómo probarlo
1. Cambiar la cadena de conexión en `appsettings.json` si el servidor no es `.\SQLEXPRESS`.
2. Crear la base de datos con la migración:
   ```
   dotnet ef database update
   ```
3. Ejecutar el script `Scripts/datos.sql` en SQL Server para meter los datos.
4. Arrancar el proyecto y probar desde Swagger.

## Decisiones

**¿Separo modelos de BBDD y DTOs?**
Sí. En `Models` están las clases de la base de datos y en `Dtos` las que usa la API. Lo he hecho así porque no tienen la misma forma (por ejemplo, la actividad en la API recibe una lista de ids de instructores y en la base de datos tiene una lista de objetos) y porque si devuelves los modelos directamente se forma un bucle al pasarlos a JSON por las propiedades de navegación.

**¿Dónde hago las validaciones?**
- Las que no necesitan la base de datos (campos obligatorios, formatos, fechas...) en el DTO con DataAnnotations o en el controlador, como estaban en 4VGymAPI.
- Las que necesitan la base de datos (que exista el tipo de actividad, que existan los instructores, que no esté repetido el nombre o el email) en los repositorios.

**POST**
Como todavía no hemos visto inserciones con Entity Framework, los POST validan los datos pero no los guardan en la base de datos.
