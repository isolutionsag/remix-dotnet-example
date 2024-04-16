# Security Demo Project: Remix with .NET API 

## Introduction

This project is a demonstration of a Remix Web application with a .NET API application. The use case for this demonstration is a simple ToDo app. As Identity Provider (IdP) we use Microsoft Entra ID.

## Requirements

### .NET Version
The project is built with .NET 8.0. You can download it from the official .NET download page [.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Node Version

This project requires Node.js version 20. If you don't have it installed, you can download it from [Node.js official website](https://nodejs.org/).

### Azure App Registration

Before you start, you need to setup an App Registrations in Azure. It has to be configured, that the Role of the user is included in the access token. https://damienbod.com/2021/02/01/implement-app-roles-authorization-with-azure-ad-and-asp-net-core/

### Azure Redis
This project uses Azure Redis for caching. To run this repository, you will need to set up your own Azure Redis instance. Please follow the official Azure Redis documentation to set up and configure your Azure Redis instance.


## Getting Started
1. Clone the repository:
```sh
git clone <repository-url>
```

### Configuration

#### For the Remix application

1. You need to create a `.env` file in the `frontend` root folder. This file contain secrets and configurations. This should be done in a manner analogous to the `.env.example` file. Depending on your environment, you may need to create separate files.

1. Make sure you have created a certificate for localhost: use the `setupSsl.ps1` script to do so.

#### For the .NET API
1. Adapt the `appsettings.json` file located in the `backend/todo.API` directory. Replace the placeholders with your secrets and configuration related to the app registration. Related to your environment, you can create separate files.


*Please be careful not to commit your changes to this file if it contains sensitive information. Consider using a secrets manager for production environments.*

### Installation
#### For the Remix application
```sh
cd frontend
npm install
```

#### For the .NET API
```sh
cd backend
dotnet restore
dotnet build
``` 

### Running Locally
1. Run the .NET API
```sh
cd backend/todo.API
dotnet run
```
The .NET API will run on https://localhost:7156

2. In a new terminal, start the Remix application: 
```sh
cd frontend
npm run dev
```
The Remix application will run on https://localhost:3000