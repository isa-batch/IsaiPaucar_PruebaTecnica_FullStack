# ACME Task Management - Full Stack Junior

Este proyecto es una aplicación de gestión de tareas y proyectos desarrollada como parte de una prueba técnica. La solución incluye un backend robusto en .NET 8 y un frontend dinámico en Angular.

## 🚀 Tecnologías Utilizadas

### Backend
- **Framework:** .NET 8.0 (ASP.NET Core Web API)
- **Base de Datos:** MySQL
- **ORM:** Entity Framework Core
- **Autenticación:** JWT (JSON Web Tokens)
- **Arquitectura:** Repositorio y Unidad de Trabajo (Repository Pattern)

### Frontend
- **Framework:** Angular 17+
- **Estilos:** CSS3 (Vanilla)
- **Estado:** Gestión de estado reactiva con RxJS

---

## 📂 Estructura del Proyecto

```text
├── Acme.backend/         # Solución de backend (.NET 8)
│   ├── Repository/       # Lógica de acceso a datos
│   ├── Services/         # Lógica de negocio
│   └── WebApi/           # Endpoints de API y configuración
├── Acme.frontend/        # Aplicación cliente (Angular)
│   ├── src/app/          # Componentes, servicios y rutas
│   └── ...
└── Para creacion/        # Recursos para despliegue
    ├── create_database.sql # Script SQL para la BD
    └── Guia_Ejecucion.md   # Instrucciones detalladas de configuración
```

---

## 🛠️ Guía Rápida de Inicio

Para obtener instrucciones detalladas sobre cómo ejecutar el proyecto localmente, por favor consulta la **[Guía de Ejecución](Guia_Ejecucion.md)**.

1.  **Configurar Base de Datos:** Ejecutar `create_database.sql` en MySQL.
2.  **Configurar Backend:** Ajustar `appsettings.json` en `Acme.backend/WebApi`.
3.  **Iniciar Frontend:** Ejecutar `npm install` y `npm start` en `Acme.frontend`.

---

## 👤 Autor
**Isai Altaf Paucar Lazo** - Full Stack Junior
