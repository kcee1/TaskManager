# Task Management System

Task Management Api is an Api that can be used to manage user tasks.

## Table of Contents

- [About](#about)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)

## About 
Task Management Api is an Api that can be used to manage user tasks and send notification as required. Users can create projects and assign tasks to project. futhermore tasks can be assigned to other users.

## Architecture

This Application is built using N-tier Achitecture, Unit Of Work Pattern, Solid Principle, Repository Pattern, Seperation Of Concerns, Dependency Injection is strongly followed where necessary.

Layers:
TaskManagerApi - Contains All the endpoints and configuration for the application API.
TaskManager.DomainLayer - Contains all data base entity model and data transfer objects and Enums.
TaskManager.DAL - Data access layer contains all the Data base DBsets,Migrations,Service Extensions.
TaskManager.BusinessLogic - All the business logic for the application, validation, implementation for repositories, automapper, unitofwork are all encapsulated in this layer
ServiceLibrary - CustomLibrary for doing basic repetetive tasks.


## Getting Started
- Clone Project from repository
-  Restore Norget Packages
- Replace Connection string in program.cs file with that of your local machine
- Replace Email sending credentials with valid once for sending email notifications
- Build (f5) after selecting TaskManagerAPI as startup project.
- Interact with Enpoints using swagger :smile:

### Prerequisites

- Visual Studio
- MySql Database
- Microsoft Server Management Studio


