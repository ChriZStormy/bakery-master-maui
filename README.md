# Pasteles App

[![es](https://img.shields.io/badge/lang-es-yellow.svg)](README.md)
[![en](https://img.shields.io/badge/lang-en-red.svg)](README.en.md)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-10.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp)

Aplicación integral para la gestión de una pastelería. El proyecto consta de un backend desarrollado con ASP.NET Core (Web API) y un cliente multiplataforma desarrollado con .NET MAUI utilizando el patrón estricto MVVM.

## 🚀 Características Principales

*   **Gestión de Pasteles**: Operaciones CRUD completas (Crear, Leer, Actualizar, Eliminar) para el menú de pasteles, integrando selectores en tiempo real.
*   **Pedidos y Estatus**: Registro del historial de pedidos, reporte de mermas y sistema de actualización de estatus sin bloqueos por validación de objetos profundos (implementación de DTOs).
*   **Catálogos de Sistema Dinámicos**: Administración 100% funcional de catálogos (Bizcochos, Rellenos, Glaseados, Categorías) con operaciones directas a la base de datos (Altas y Bajas).
*   **UI/UX Moderna y Responsiva**: Diseño atractivo e intuitivo en la aplicación cliente, ofreciendo una experiencia de usuario fluida, correcciones de contraste y uso de componentes Picker en lugar de texto libre.
*   **Arquitectura MVVM Desacoplada**: Separación clara de responsabilidades en la aplicación MAUI aislando toda la lógica del Code-Behind hacia los ViewModels (ej. `DetallePedidoViewModel`).

## 🛠️ Tecnologías Utilizadas

### Backend (`PasteleriaAPI`)
*   **.NET 10.0**
*   **ASP.NET Core Web API**
*   **Entity Framework Core** para el acceso a datos.
*   **SQL Server** como base de datos relacional.
*   **OpenAPI/Swagger** para documentación y prueba de la API.

### Frontend (`PasteleriaMaui`)
*   **.NET MAUI** (.NET Multi-platform App UI) apuntando a .NET 10.0.
*   **XAML** para el diseño de interfaces de usuario.
*   Soporte multiplataforma: Windows, Android, iOS y MacCatalyst.

## 📁 Estructura del Proyecto

La solución contiene dos proyectos principales:

1.  **`PasteleriaAPI`**: Proyecto backend que expone los endpoints RESTful para la lógica de negocio y persistencia de datos.
2.  **`PasteleriaMaui`**: Proyecto cliente MAUI con las vistas (Views), nombres de vista (ViewModels) y la lógica de presentación para consumir la API.

## ⚙️ Configuración y Ejecución

### Requisitos Previos
*   [Visual Studio 2022](https://visualstudio.microsoft.com/) (versión compatible con .NET 10 y cargas de trabajo de MAUI y ASP.NET).
*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
*   SQL Server LocalDB o una instancia de SQL Server configurada.

### Pasos para ejecutar localmente

1.  **Clonar el repositorio**:
    ```bash
    git clone https://github.com/ChriZStormy/Pasteles-App.git
    cd PasteleriaApp
    ```

2.  **Configurar la Base de Datos (`PasteleriaAPI`)**:
    *   Verifica la cadena de conexión en el archivo `appsettings.json` del proyecto `PasteleriaAPI`.
    *   Abre la Consola del Administrador de Paquetes en Visual Studio, selecciona el proyecto `PasteleriaAPI` y ejecuta:
        ```powershell
        Update-Database
        ```
    *   Alternativamente, puedes usar la CLI de .NET en el directorio de la API:
        ```bash
        dotnet ef database update
        ```

3.  **Configurar la URL de la API (`PasteleriaMaui`)**:
    *   Asegúrate de que los entregas en la app MAUI (ej. `PastelService.cs`) apunten a la URL local o remota correcta donde se esté ejecutando `PasteleriaAPI` (por defecto suele ser `https://localhost:port`).

4.  **Ejecutar la Solución**:
    *   Abre `PasteleriaApp.sln` en Visual Studio.
    *   Para ejecutar ambos proyectos simultáneamente, puedes configurar Visual Studio para "Proyectos de inicio múltiples" e iniciar ambos (`PasteleriaAPI` y `PasteleriaMaui`).

## 📝 Licencia
Este proyecto se distribuye bajo los términos especificados en el repositorio.
