# Guía de Ejecución del Proyecto

Esta guía detalla los pasos necesarios para configurar y ejecutar el sistema de gestión de tareas en su entorno local.

## 📋 Prerrequisitos

Antes de comenzar, asegúrese de tener instalado lo siguiente:

- **.NET SDK 8.0**: [Descargar aquí](https://dotnet.microsoft.com/download/dotnet/8.0)
- **MySQL Server 8.0+**: [Descargar aquí](https://dev.mysql.com/downloads/mysql/)
- **Node.js (LTS)**: [Descargar aquí](https://nodejs.org/)
- **Angular CLI**: Ejecute `npm install -g @angular/cli`

---

## 🛠️ Pasos para la Configuración

### 1. Base de Datos (MySQL)

1.  Abra su cliente de MySQL (Workbench, SQLyog, o terminal).
2.  Ejecute el script de creación ubicado en:
    `Para creacion/create_database.sql`
3.  Esto creará la base de datos `gestion_tareas_db` y las tablas necesarias con datos de prueba.

### 2. Configuración del Backend

1.  Navegue a la carpeta del backend:
    `Acme.backend/WebApi`
2.  Localice y abra el archivo `appsettings.json`.
3.  Actualice la cadena de conexión `DefaultConnection` con sus credenciales locales:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=gestion_tareas_db;User=SU_USUARIO;Password=SU_CONTRASEÑA;"
}
```

> [!NOTE]
> Asegúrese de que el puerto coincida con su configuración de MySQL (usualmente 3306 o 3310).

4.  Ejecute el backend:
    ```bash
    dotnet run
    ```

### 3. Configuración del Frontend

1.  Abra una terminal en la carpeta del frontend:
    `Acme.frontend`
2.  Instale las dependencias necesarias:
    ```bash
    npm install
    ```
3.  Inicie la aplicación:
    ```bash
    npm start
    ```
4.  La aplicación debería estar disponible en `http://localhost:4200`.

---

## 🔑 Credenciales de Prueba

Si ejecutó el script SQL con los datos de prueba, puede usar las siguientes credenciales para ingresar:

- **Email:** `admin@acme.com`
- **Password:** `Admin123!`

Sino, puede crear un usuario desde el frontend.
---

## 📩 Soporte
Para cualquier duda o inconveniente en la ejecución, por favor contactar al desarrollador responsable.
