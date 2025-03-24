# 📝 BlazorToDoApp

A **To-Do application** built with **Blazor WebAssembly (WASM)**. The app allows users to create, edit, delete, and mark tasks as completed. The frontend communicates with a **RESTful API** in the backend and utilizes **Azure AD for authentication**.

---

## 🚀 Current Status

- **Blazor WebAssembly (WASM) Frontend** for task management.
- **Backend (RESTful API) is fully implemented** and handles all CRUD operations.
- **Microsoft Authentication Library (MSAL)** for authentication via Azure AD.
- **Infinite Scroll Pagination**: Dynamically loads **10 new tasks** when scrolling.
- **Recycle Bin Feature** to manage completed tasks.
- **Multi-language support**: Supports **German, English, French, and Italian**.

---

## ✅ Features

- **View tasks** (retrieved from the API).
- **Add tasks** (enter title, description, and due date).
- **Edit tasks** (save modifications).
- **Delete tasks** (either permanently or move them to the recycle bin).
- **Restore tasks** (from the recycle bin).
- **Display total task count** using API headers `X-Total-Active-Count` and `X-Total-Bin-Count`.
- **Infinite Scroll Pagination** for seamless UX.
- **Authentication with Microsoft Login (MSAL)**.
- **Snackbars for error and success notifications** (MudBlazor).
- **Dynamic language switching** using **Localization (.resx Files + LanguageService.cs)**.

---

## 🔧 Tech Stack

### 🌍 Frontend
- **Blazor WebAssembly (.NET 9)**
- **MudBlazor** (UI components)
- **Syncfusion Blazor** (input & UI components)
- **MSAL (Microsoft Authentication Library)** for OAuth 2.0 login
- **HttpClient** for API communication
- **Virtualize component** for optimized list rendering
- **Bootstrap & CSS for styling**

### 🖥️ Backend
- **.NET Core Web API (C#)**
- **Entity Framework Core** for database communication
- **Microsoft SQL Server**
- **Azure Active Directory (AD) for authentication**

### 🔗 Networking & Tools
- **Swagger** for API documentation
- **Azure Portal & Azure DevOps** for deployment
- **Postman** for API testing

---

## 🌐 API Endpoints

| Method | Endpoint                  | Description                              |
|--------|---------------------------|------------------------------------------|
| GET    | `/api/todo`                | Retrieve all tasks                      |
| GET    | `/api/todo?completed=true` | Retrieve only completed tasks (Recycle Bin) |
| POST   | `/api/todo`                | Add a new task                          |
| PUT    | `/api/todo/{id}`           | Update a task                           |
| DELETE | `/api/todo/{id}`           | Delete a task                           |

---

## 📌 Installation & Usage

### 🚀 Requirements
- **.NET 7 SDK** installed ([Download here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
- **Visual Studio 2022** or another .NET-supported IDE
- **A registered Azure AD account** for MSAL login
- **Microsoft SQL Server** for database management

### 📥 **Clone & Start the Project**
```sh
# Clone the repository
git clone https://github.com/your-repository/BlazorToDoApp.git
cd BlazorToDoApp
```

### 🖥️ **Start the Backend**
```sh
cd BlazorToDoApp.API
# Start the API
dotnet run
```
➡ **API running at:** `http://localhost:5085`

### 🌍 **Start the Frontend**
```sh
cd BlazorToDoApp.Client
# Install dependencies
npm install
# Start Blazor
dotnet run
```
➡ **Web app running at:** `https://localhost:7032`

---

## 👥 Contributors

- **Ibrahim Zeqiraj** - Development  
[GitHub](https://github.com/ibrazqrj)

---
