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
Framework: ASP.NET Core MVC

Frontend: Razor Pages, HTML5, CSS3

Backend: C# (.NET Core)

Database: Entity Framework Core with SQLite

Cloud Storage: Microsoft Azure Blob Storage

Development Environment: Visual Studio 2022


## Project Structure
```
Lab5/
├── .config/ # Configuration files
├── Controllers/ # ASP.NET MVC controllers
├── Data/ # EF Core database context and seed data
├── Migrations/ # EF Core migration files
├── Models/ # Data models and view models
├── Properties/ # Project-specific settings and launch config
├── Views/ # Razor Views (.cshtml files)
├── wwwroot/css/ # Static CSS files
├── Lab5.csproj # Project file
├── Lab5.csproj.user # User-specific project settings
└── Program.cs # Application entry point
```

