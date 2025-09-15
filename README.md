# App De Facturas - Prueba tecnica

Este repositorio contiene una API en **.NET** y una proyecto en **Angular** para crear y listar facturas.  
El objetivo es levantar ambos proyectos en local y conectarlos entre sí.

## Estructura del repositorio
En el repositorio estan 2 carpetas con los 2 proyectos a utilizar, la carpeta Backend contiene el proyecto de .Net y la carpeta Frontend tiene el proyecto de Angular.


## 🔧 Requisitos
- **.NET SDK 8.0+**
- **Node.js 22+** y **npm**
- **Angular CLI** (`npm i -g @angular/cli`)
- **SQL Server** en local o accesible por red (SQL Server 2019+)


## 🗄️ Base de datos (SQL Server)
1. Asegúrate de tener una instancia de SQL Server corriendo.
2. Se da por hecho de que ya existe la base de datos, en dado caso de que no exista, crea la base de datos y los stored Procedures requeridos (En la carpeta de Backend/Infraestructure/Scripts estan los scripts SQL para levantar las tablas y los store procedures (Es importante que la base de datos debe usar los mismos nombres de las tablas y propiedades que en la documentación dada).
3. Copia la **cadena de conexión** de tu instancia (servidor, db, usuario, contraseña). Ejemplo de cadena para usar: Server=localhost,1433;Database=DevLab;User Id=developer;Password=abc123ABC;Encrypt=False;TrustServerCertificate=True;


## Configuración del backend (.NET)
Edita `backend/WebApp/appsettings.json` para **apuntar a tu base de datos** y fijar el **puerto 5248**:

```json
{
    "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "database": "Server=localhost,1433;Database=DevLab;User Id=developer;Password=abc123ABC;Encrypt=False;TrustServerCertificate=True;"
  }
}
```

## Ejecución del proyecto Backend (.NET)
Debes ubicarte dentro de la caprte de backend para ejecutar los siguientes comandos:
- dotnet run --urls http://localhost:5248

En dado caso de tener errores es mejor usar visual studio y abrir la solución directamente y ejecutar con el IDE.


## Configuración del Frontend (Angular)
Debes tener en cuenta un ajuste importante para referenciar el proyecto de Backend con el de Frontend, en este caso debes ir a la archivo Fronted/Src/Environments/environments.ts y debes validar que la url sea la misma que el puerto del localhost donde esta corriendo el backend, ademas de poner /api seguido del localhost donde esta corriendo el backend. Si ejecutas el proyecto de .net en el puerto que mencioné ya no se debería preocupar por este ajuste, en dado caso que se ejecute en otro puerto se debe cambiar.

```export const API_BASE_URL = 'http://{localhost:puerto}/api';```</pre>


## Ejecución del proyecto Frontend (Angular)
Debes ubicarte dentro de la caprte de frontend para ejecutar los siguientes comandos:

- npm install
- ng serve --port 4200
