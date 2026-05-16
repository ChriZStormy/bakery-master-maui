# Pasteles App

[![es](https://img.shields.io/badge/lang-es-yellow.svg)](README.md)
[![en](https://img.shields.io/badge/lang-en-red.svg)](README.en.md)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-10.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp)

Comprehensive application for bakery management. The project consists of a backend developed with ASP.NET Core (Web API) and a cross-platform client developed with .NET MAUI using a strict MVVM pattern.

## 🚀 Key Features

*   **Cake Management**: Complete CRUD operations (Create, Read, Update, Delete) for the cake menu, integrating real-time catalog pickers.
*   **Orders & Statuses**: Order history logging, waste reporting, and a robust status update system utilizing DTOs to bypass deep object tree validation issues.
*   **Dynamic System Catalogs**: Fully functional administration of catalogs (Sponges, Fillings, Frostings, Categories) with direct database operations (Add/Delete).
*   **Modern & Responsive UI/UX**: Attractive and intuitive design in the client application, offering a smooth user experience, contrast fixes, and standardized picker components.
*   **Decoupled MVVM Architecture**: Clear separation of concerns in the MAUI application, isolating all logic from Code-Behind into dedicated ViewModels (e.g. `DetallePedidoViewModel`).

## 🛠️ Technologies Used

### Backend (`PasteleriaAPI`)
*   **.NET 10.0**
*   **ASP.NET Core Web API**
*   **Entity Framework Core** for data access.
*   **SQL Server** as the relational database.
*   **OpenAPI/Swagger** for API documentation and testing.

### Frontend (`PasteleriaMaui`)
*   **.NET MAUI** (.NET Multi-platform App UI) targeting .NET 10.0.
*   **XAML** for designing user interfaces.
*   Cross-platform support: Windows, Android, iOS, and MacCatalyst.

## 📁 Project Structure

The solution contains two main projects:

1.  **`PasteleriaAPI`**: Backend project that exposes RESTful endpoints for business logic and data persistence.
2.  **`PasteleriaMaui`**: MAUI client project with Views, ViewModels, and presentation logic to consume the API.

## ⚙️ Setup and Execution

### Prerequisites
*   [Visual Studio 2022](https://visualstudio.microsoft.com/) (version compatible with .NET 10, MAUI, and ASP.NET workloads).
*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
*   SQL Server LocalDB or a configured SQL Server instance.

### Steps to run locally

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/ChriZStormy/Pasteles-App.git
    cd PasteleriaApp
    ```

2.  **Configure the Database (`PasteleriaAPI`)**:
    *   Verify the connection string in the `appsettings.json` file of the `PasteleriaAPI` project.
    *   Open the Package Manager Console in Visual Studio, select the `PasteleriaAPI` project, and run:
        ```powershell
        Update-Database
        ```
    *   Alternatively, you can use the .NET CLI in the API directory:
        ```bash
        dotnet ef database update
        ```

3.  **Configure the API URL (`PasteleriaMaui`)**:
    *   Ensure that the services in the MAUI app (e.g., `PastelService.cs`) point to the correct local or remote URL where `PasteleriaAPI` is running (default is usually `https://localhost:port`).

4.  **Run the Solution**:
    *   Open `PasteleriaApp.sln` in Visual Studio.
    *   To run both projects simultaneously, you can configure Visual Studio for "Multiple startup projects" and start both (`PasteleriaAPI` and `PasteleriaMaui`).

## 📝 License
This project is distributed under the terms specified in the repository.
