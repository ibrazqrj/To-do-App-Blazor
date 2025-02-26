# BlazorToDoApp

Eine **To-Do-Anwendung**, die mit **Blazor WebAssembly (WASM)** entwickelt wurde. Die App ermöglicht es, Aufgaben zu erstellen, zu bearbeiten, zu löschen und als erledigt zu markieren. Das Frontend kommuniziert mit einer **RESTful API** im Backend und nutzt **Azure AD für die Authentifizierung**.

---

## 🚀 Aktueller Stand

- **Blazor WebAssembly (WASM) Frontend** zur Verwaltung von Aufgaben.
- **Backend (RESTful API) ist bereits vorhanden** und verarbeitet CRUD-Operationen für Aufgaben.
- **Microsoft Authentication Library (MSAL)** für Authentifizierung mit Azure AD.
- **Infinite Scroll Paginierung**: Lädt dynamisch **10 neue Aufgaben** beim Scrollen.
- **Papierkorb-Funktion** (Recycle Bin), um abgeschlossene Aufgaben zu verwalten.
- **Mehrsprachigkeit**: Unterstützung für **Deutsch, Englisch, Französisch und Italienisch**.

---

## ✅ Features

- **Aufgaben anzeigen** (von der API abgerufen).
- **Aufgaben hinzufügen** (Titel, Beschreibung, Datum eingeben).
- **Aufgaben bearbeiten** (Änderungen speichern).
- **Aufgaben löschen** (entweder direkt oder in den Papierkorb verschieben).
- **Aufgaben wiederherstellen** (Papierkorb-Funktion).
- **Anzeige der Gesamtanzahl von Aufgaben** mit API-Headern `X-Total-Active-Count` und `X-Total-Bin-Count`.
- **Paginierung mit Infinite Scroll** für eine nahtlose UX.
- **Authentifizierung mit Microsoft Login (MSAL)**.
- **Snackbars für Fehler- und Erfolgsbenachrichtigungen** (MudBlazor).
- **Dynamischer Sprachwechsel** mit **Localization (.resx Files + LanguageService.cs)**.

---

## 🔧 Tech-Stack

### 🌍 Frontend
- **Blazor WebAssembly (.NET 9)**
- **MudBlazor** (UI-Komponenten)
- **Syncfusion Blazor** (Eingabe- und UI-Komponenten)
- **MSAL (Microsoft Authentication Library)** für OAuth 2.0 Login
- **HttpClient** für API-Kommunikation
- **Virtualize-Komponente** für optimierte Listen-Darstellung
- **Bootstrap & CSS für das Design**

### 🖥️ Backend
- **.NET Core Web API (C#)**
- **Entity Framework Core** für die Datenbank-Kommunikation
- **Microsoft SQL Server**
- **Azure Active Directory (AD) für Authentifizierung**

### 🔗 Netzwerk & Tools
- **Swagger** für API-Dokumentation
- **Azure Portal & Azure DevOps** für Deployment
- **Postman** für API-Testing

---

## 🌐 API-Endpunkte

| Methode | Endpunkt             | Beschreibung                               |
|---------|----------------------|-------------------------------------------|
| GET     | `/api/todo`          | Alle Aufgaben abrufen                     |
| GET     | `/api/todo?completed=true` | Nur erledigte Aufgaben abrufen (Papierkorb) |
| POST    | `/api/todo`          | Neue Aufgabe hinzufügen                   |
| PUT   | `/api/todo/{id}`     | Aufgabe aktualisieren                     |
| DELETE  | `/api/todo/{id}`     | Aufgabe löschen                           |

---

## 📌 Installation & Nutzung

### 🚀 Voraussetzungen
- **.NET 7 SDK** installiert ([Download hier](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
- **Visual Studio 2022** oder eine andere IDE mit .NET-Unterstützung
- **Ein registriertes Azure AD-Konto** für MSAL-Login
- **Microsoft SQL Server** für die Datenbank

### 📥 **Projekt klonen & starten**
```sh
# Repository klonen
git clone https://github.com/dein-repository/BlazorToDoApp.git
cd BlazorToDoApp
```

### 🖥️ **Backend starten**
```sh
cd BlazorToDoApp.API
# API starten
dotnet run
```
➡ **API läuft auf:** `http://localhost:5085`

### 🌍 **Frontend starten**
```sh
cd BlazorToDoApp.Client
# Abhängigkeiten installieren
npm install
# Blazor starten
dotnet run
```
➡ **Web-App läuft auf:** `https://localhost:7032`

---

## 👥 Mitwirkende

- **Ibrahim Zeqiraj** - Entwicklung
[GitHub](https://github.com/ibrazqrj)

---

