## ClubManagerWeb
***ClubManagerWeb*** is a web application designed to perform full CRUD operations on club membership data. It allows users to register and log in, subscribe to clubs, manage personal information, view a list of all registered members, and upload profile pictures that are stored on Microsoft Azure Blob Storage.

This project was developed as part of coursework for the ***.NET Enterprise Application Development*** course and demonstrates a basic full-stack web development workflow using ASP.NET Core and Razor Pages.


## Features
User registration and login

Club subscription form

View and manage member profiles

Form input validation

Picture upload with Azure Blob Storage integration

Data persistence using Entity Framework Core

Responsive front-end layout with Razor Pages


## Built With
Framework: ASP.NET Core

Frontend: Razor Pages, HTML5, CSS3

Backend: C# (.NET Core)

Database: Entity Framework Core with SQLite

Cloud Storage: Microsoft Azure Blob Storage

Development Environment: Visual Studio 2022


## Project Structure
```
Root/
├── wwwroot/ # Static assets
├── Pages/ # Razor Pages
├── Models/ # Entity and View models
├── Data/ # EF Core database context
├── Services/ # Azure storage integration
├── Program.cs # Application entry point
├── appsettings.json # App configuration
└── Lab5.csproj
```

