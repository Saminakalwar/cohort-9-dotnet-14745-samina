# cohort-9-dotnet-14745-samina

Cohort 9 — .NET Fullstack (.NET+ReactJS) assignment for Samina

# TaskFlow — Task Management Tool

A full-stack Task Management application built using **ASP.NET Core Web API, React.js, Entity Framework Core, SQL Server, ASP.NET Core Identity, and JWT authentication**.

The application provides secure user authentication, role-based authorization, task management, categories, user profiles, and a responsive frontend interface.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Architecture](#project-architecture)
- [Prerequisites](#prerequisites)
- [Project Setup](#project-setup)
  - [Clone the Repository](#1-clone-the-repository)
  - [Backend Setup](#2-backend-setup)
  - [Database Setup](#3-database-setup)
  - [Run the Backend](#4-run-the-backend)
  - [Frontend Setup](#5-frontend-setup)
  - [Run the Frontend](#6-run-the-frontend)

- [Authentication and Authorization](#authentication-and-authorization)
- [How to Test the Application](#how-to-test-the-application)
- [Testing the JWT Flow](#testing-the-jwt-flow)
- [Role-Based Access](#role-based-access)
- [Task Management Flow](#task-management-flow)
- [Frontend Structure](#frontend-structure)
- [Backend Structure](#backend-structure)
- [Database and Migrations](#database-and-migrations)
- [API Documentation](#api-documentation)
- [Logging and Error Handling](#logging-and-error-handling)
- [SonarQube](#sonarqube)
- [Git Workflow](#git-workflow)
- [Troubleshooting](#troubleshooting)
- [Future Improvements](#future-improvements)

---

# Project Overview

**TaskFlow** is a task management system designed to allow authenticated users to manage and track tasks.

The application follows a **Clean Architecture** approach on the backend to separate business logic, application logic, infrastructure concerns, and API responsibilities.

The system supports:

- User registration
- User login
- JWT-based authentication
- Protected API endpoints
- Role-based authorization
- Admin and regular-user behavior
- Task management
- Task categories
- User profiles
- Responsive React frontend
- SQL Server persistence
- API testing through Swagger
- Logging and exception handling
- Code quality analysis through SonarQube

---

# Features

## Authentication

- User registration
- User login
- Password validation using ASP.NET Core Identity
- JWT token generation
- JWT token validation
- Protected API endpoints
- Logout through frontend token removal

## Authorization

The application supports role-based access.

### Admin

Administrators can access administrative functionality and view all tasks.

### Regular User

Regular users can access their own task-related functionality.

The frontend reflects the user's role by displaying:

- **All Tasks** for administrators
- **My Tasks** for regular users
- An **ADMIN** indicator for administrators

> Frontend role visibility is only for user experience. Actual authorization is enforced by the backend.

---

# Technology Stack

## Backend

- C#
- ASP.NET Core
- .NET 10
- ASP.NET Core Web API
- ASP.NET Core Identity
- JWT Bearer Authentication
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- Serilog
- xUnit

## Frontend

- React.js
- React Router
- Axios
- React Hot Toast
- Tailwind CSS / CSS
- JavaScript

## Development Tools

- Visual Studio Code
- C# Dev Kit
- SQL Server Developer Edition
- SQL Server Management Studio (SSMS)
- Git
- GitHub
- GitHub Desktop
- Thunder Client
- Swagger
- SonarQube Cloud
- GitHub Actions

---

# Project Architecture

The backend follows Clean Architecture.

```text
Backend/
│
├── src/
│   ├── TaskManagement.API/
│   ├── TaskManagement.Application/
│   ├── TaskManagement.Domain/
│   ├── TaskManagement.Infrastructure/
│   └── TaskManagement.Persistence/
│
└── tests/
    ├── TaskManagement.UnitTests/
    └── TaskManagement.IntegrationTests/
```

## Domain

Contains the core business entities and enums.

Examples include:

- `ApplicationUser`
- `TaskItem`
- `Category`

Enums include:

```text
TaskPriority
TaskStatus
UserRole
```

The domain layer does not depend on infrastructure or API implementation details.

---

## Application

Contains application-level abstractions and DTOs.

Examples:

```text
DTOs/
Interfaces/
Services/
```

Interfaces define contracts that are implemented by infrastructure components.

---

## Persistence

Responsible for database-related functionality.

It contains:

- Entity Framework Core `DbContext`
- Identity configuration
- Database configuration
- Entity relationships
- EF Core migrations

---

## Infrastructure

Contains implementations of application services and external concerns.

Authentication-related functionality is handled here, including the authentication service and JWT service.

---

## API

The API layer contains:

- Controllers
- Middleware configuration
- Dependency injection setup
- Authentication configuration
- Authorization configuration
- Swagger configuration
- Application startup

---

# Prerequisites

Before running the project, install:

1. **.NET 10 SDK**
2. **Node.js and npm**
3. **SQL Server Developer Edition**
4. **SQL Server Management Studio**
5. **Git**

Verify .NET:

```bash
dotnet --version
```

Verify Node.js:

```bash
node --version
```

Verify npm:

```bash
npm --version
```

---

# Project Setup

## 1. Clone the Repository

Clone the repository:

```bash
git clone <repository-url>
```

Navigate into the project:

```bash
cd <repository-folder>
```

The repository contains both backend and frontend components.

---

# 2. Backend Setup

Navigate to the backend:

```bash
cd Backend
```

Restore dependencies:

```bash
dotnet restore
```

Build the complete backend:

```bash
dotnet build
```

The build should complete successfully before running the application.

---

# 3. Database Setup

The application uses **SQL Server** with Entity Framework Core.

Make sure SQL Server is running.

Create or update the connection string in the appropriate backend configuration file.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Use the connection string appropriate for the local SQL Server installation.

---

## Apply EF Core Migrations

From the `Backend` directory, run the EF migration command according to the solution/project configuration.

For example:

```bash
dotnet ef database update
```

If the EF CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

The database should now contain the tables required by the application, including Identity-related tables.

---

# 4. Run the Backend

From the backend directory:

```bash
dotnet run --project src/TaskManagement.API
```

The terminal will display the API URL.

For example:

```text
http://localhost:5135
```

or another configured port depending on the local launch configuration.

---

# 5. Verify Swagger

Open:

```text
http://localhost:<port>/swagger
```

Swagger should display the available API endpoints.

Swagger is useful for testing:

- Registration
- Login
- Protected endpoints
- Task endpoints
- Other API operations

---

# 6. Frontend Setup

Open another terminal.

Navigate to the frontend directory:

```bash
cd Frontend
```

Install dependencies:

```bash
npm install
```

Configure the frontend API URL using the project's environment configuration.

For local development, the frontend should point to the backend running on localhost.

Example:

```env
VITE_API_URL=http://localhost:<backend-port>
```

Use the actual environment variable name already defined by the project.

---

# 7. Run the Frontend

Start the React development server:

```bash
npm run dev
```

The terminal will provide the frontend URL, commonly something similar to:

```text
http://localhost:5173
```

Open the displayed URL in a browser.

---

# Authentication and Authorization

TaskFlow uses:

```text
ASP.NET Core Identity
        +
JWT Bearer Authentication
        +
Role-Based Authorization
```

---

## Authentication Flow

The authentication process works as follows:

```text
User
 │
 │ Login credentials
 ▼
React Frontend
 │
 │ POST Login
 ▼
ASP.NET Core API
 │
 ▼
AuthService
 │
 ▼
ASP.NET Core Identity
 │
 │ Validate user credentials
 ▼
JWT Service
 │
 │ Generate JWT
 ▼
API
 │
 │ JWT token
 ▼
React Frontend
 │
 │ Store token
 ▼
Protected API Requests
```

---

# JWT Authentication

After successful login, the backend generates a JWT token.

The frontend stores the token and sends it with requests to protected endpoints.

The request uses:

```http
Authorization: Bearer <JWT_TOKEN>
```

The backend then:

1. Reads the Authorization header.
2. Extracts the bearer token.
3. Validates the JWT.
4. Validates the signing key.
5. Validates issuer/audience if configured.
6. Validates expiration.
7. Creates the authenticated user principal.
8. Allows the request to continue.

---

# Authentication Middleware

The API configures authentication and authorization during application startup.

Conceptually:

```text
AddAuthentication()
        ↓
Configure JWT Bearer
        ↓
UseAuthentication()
        ↓
UseAuthorization()
        ↓
Controller
```

### `AddAuthentication`

Registers authentication services and configures JWT bearer authentication.

### `UseAuthentication`

Runs authentication middleware for incoming requests.

It determines whether the request contains a valid identity.

### `UseAuthorization`

Checks whether the authenticated identity has permission to access the requested endpoint.

Both configuration and middleware are required.

---

# How to Test the Application

The recommended testing flow is:

```text
1. Start SQL Server
2. Start Backend
3. Verify Swagger
4. Start Frontend
5. Register/Login
6. Obtain JWT
7. Access protected endpoints
8. Test role-based behavior
9. Create and manage tasks
10. Test logout
```

---

# Testing Registration

Open the frontend registration page.

Provide valid user information.

Submit the form.

Expected result:

```text
Registration successful
```

The user should be stored in the Identity database.

---

# Testing Login

Navigate to the login page.

Enter valid credentials.

The frontend sends a request to the backend login endpoint.

Expected result:

- Login succeeds.
- Backend returns a JWT.
- Frontend stores the token.
- User is redirected to the application/dashboard.

---

# Testing Invalid Login

Try an incorrect password.

Expected behavior:

```text
401 Unauthorized
```

The frontend should display an appropriate error message.

---

# Testing Protected Endpoints

A protected endpoint cannot be accessed without a valid JWT.

For example:

```http
GET /api/Auth/protected
```

### Without token

Expected:

```text
401 Unauthorized
```

### With valid token

Expected:

```text
200 OK
```

The endpoint should confirm that the request is authenticated.

---

# Swagger JWT Testing

Swagger can be used to test protected endpoints.

## Step 1 — Login

Use the login endpoint with valid credentials.

Copy the returned JWT.

## Step 2 — Authorize

Click the **Authorize** button in Swagger.

Enter:

```text
Bearer <your-token>
```

or use the format expected by the Swagger security configuration.

## Step 3 — Call Protected Endpoint

Execute the protected endpoint.

A valid token should result in successful authentication.

---

# JWT Configuration

The JWT configuration includes a secret signing key.

The secret must be configured correctly.

For example:

```json
{
  "Jwt": {
    "SecretKey": "your-secure-secret-key",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementClient"
  }
}
```

Do not commit real production secrets to source control.

---

# Important JWT Issue Encountered During Development

During JWT testing, the API initially returned an error similar to:

```text
IDX10703: Cannot create Microsoft.IdentityModel.Tokens.SymmetricSecurityKey,
key length is zero.
```

The problem was caused by a mismatch between the configuration key expected by the JWT service and the key actually present in `appsettings`.

The JWT service was expecting:

```text
SecretKey
```

while the configuration initially used a different property name.

After correcting the configuration and ensuring the signing key was populated, JWT authentication worked successfully.

This demonstrates the importance of keeping:

```text
appsettings
        ↓
JWT configuration
        ↓
JWT service
```

consistent.

---

# Role-Based Access

The application supports different behavior for administrators and normal users.

## Administrator

An administrator sees:

```text
Dashboard
All Tasks
Profile
```

The header/sidebar can also display:

```text
ADMIN
```

## Normal User

A normal user sees:

```text
Dashboard
My Tasks
Profile
```

The `ADMIN` label is not displayed.

---

# Important Security Principle

The frontend should **never be considered the security boundary**.

For example, hiding:

```text
All Tasks
```

from a normal user does not prevent that user from manually calling the API.

The backend must enforce authorization.

Conceptually:

```text
Frontend UI
     │
     │ visibility
     ▼
User experience

Backend Authorization
     │
     │ permission enforcement
     ▼
Actual security
```

---

# Task Management Flow

A typical task flow is:

```text
Login
  ↓
Dashboard
  ↓
Tasks
  ↓
Create Task
  ↓
Assign Category / User
  ↓
Set Priority
  ↓
Set Status
  ↓
Set Due Date
  ↓
Update / Complete Task
```

---

# Task Entity

The `TaskItem` entity contains information such as:

- Title
- Description
- Due Date
- Priority
- Status
- Category ID
- Category
- Assigned User ID

Example conceptual structure:

```text
TaskItem
│
├── Id
├── Title
├── Description
├── DueDate
├── Priority
├── Status
├── CategoryId
├── Category
└── AssignedUserId
```

---

# Task Priority

The application defines priorities such as:

```text
Low
Medium
High
Critical
```

---

# Task Status

The application defines statuses such as:

```text
Pending
InProgress
Completed
```

---

# Category

Tasks can be associated with categories.

Conceptually:

```text
Category
│
├── Id
├── Name
└── Tasks
```

A category can contain multiple tasks.

---

# Frontend Structure

The React frontend contains reusable components and pages.

Important frontend concepts include:

```text
React Router
Axios API communication
Authentication state
Protected routes
Sidebar
Header
Dashboard
Tasks
Profile
Login
Registration
```

The sidebar supports responsive behavior.

On smaller screens:

```text
Mobile menu
     ↓
Sidebar opens
     ↓
Overlay displayed
     ↓
Navigation selected
     ↓
Sidebar closes
```

---

# Header

The header displays:

- Mobile menu
- TaskFlow branding
- Current page title
- User avatar/initials
- User name
- ADMIN indicator when applicable

The user's initials are generated from first and last name.

For example:

```text
Samina Kalwar
      ↓
SK
```

---

# Logout

Logout removes the stored JWT token from browser storage.

The flow is:

```text
Click Logout
    ↓
Remove token
    ↓
Show success notification
    ↓
Navigate to /login
```

Conceptually:

```javascript
localStorage.removeItem("token");
```

After logout, the user must authenticate again to access protected functionality.

---

# Backend Structure

A simplified backend structure is:

```text
Backend/
│
├── src/
│   │
│   ├── TaskManagement.API/
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── TaskManagement.Application/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── ...
│   │
│   ├── TaskManagement.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── Common/
│   │
│   ├── TaskManagement.Infrastructure/
│   │   ├── Services/
│   │   └── ...
│   │
│   └── TaskManagement.Persistence/
│       ├── DbContext/
│       ├── Configurations/
│       └── ...
│
└── tests/
    ├── TaskManagement.UnitTests/
    └── TaskManagement.IntegrationTests/
```

---

# Database and Migrations

Entity Framework Core is used for database access.

The database contains application data as well as ASP.NET Core Identity data.

Typical Identity tables include users, roles, claims, and related authentication tables.

After changing an entity or database model, a new migration can be created.

Example:

```bash
dotnet ef migrations add <MigrationName>
```

Then apply it:

```bash
dotnet ef database update
```

---

# API Documentation

Swagger/OpenAPI is enabled for the backend.

After starting the API, open:

```text
http://localhost:<port>/swagger
```

Swagger allows developers/testers to:

- View endpoints
- Inspect request models
- Execute API calls
- Test authentication
- Test protected endpoints
- Inspect API responses

---

# Logging and Error Handling

The backend uses logging to help diagnose application behavior.

Serilog is used as part of the application's logging setup.

Logging is particularly useful for:

- Authentication failures
- Database issues
- API exceptions
- Background/application errors
- Debugging production-like behavior

The application also uses appropriate exception handling rather than exposing unnecessary internal details to API consumers.

---

# SonarQube

SonarQube Cloud is integrated into the repository to perform automated code quality analysis.

The project includes GitHub Actions workflow configuration.

The workflow builds the application and performs SonarQube analysis.

The CI workflow is triggered through GitHub Actions.

The build workflow was configured and successfully completed during development.

Important CI configuration considerations include:

- Correct .NET setup
- Correct Java setup for SonarQube tooling
- SonarQube project configuration
- GitHub secrets
- Correct source paths
- Successful application build

The GitHub Actions Java setup was updated to:

```yaml
actions/setup-java@v5
```

as part of the CI configuration updates.

---

# Git Workflow

Development follows a feature-branch workflow.

The general process is:

```text
develop
   │
   ├── feature/authentication
   │
   ├── feature/jwt-authentication
   │
   ├── feature/frontend
   │
   └── other feature branches
```

A feature should normally be created from the latest `develop`.

Example:

```bash
git checkout develop
git pull origin develop
git checkout -b feature/<feature-name>
```

After implementation:

```bash
git add .
git commit -m "Implement <feature>"
git push origin feature/<feature-name>
```

Then create a Pull Request targeting:

```text
develop
```

---

# Recommended Complete Test Procedure

For someone evaluating the project, the following sequence provides a complete demonstration.

## Step 1 — Start SQL Server

Make sure the SQL Server instance is running.

---

## Step 2 — Update Configuration

Verify:

- Connection string
- JWT secret key
- JWT issuer/audience if configured
- Frontend API URL

---

## Step 3 — Update Database

From the backend:

```bash
dotnet ef database update
```

---

## Step 4 — Start API

```bash
dotnet run --project src/TaskManagement.API
```

---

## Step 5 — Open Swagger

Navigate to:

```text
http://localhost:<port>/swagger
```

Verify that the API is running.

---

## Step 6 — Register a User

Use the registration endpoint or frontend registration page.

Create a normal user.

---

## Step 7 — Login

Use valid credentials.

Confirm that a JWT is returned and the frontend successfully authenticates.

---

## Step 8 — Test Protected Endpoint

Call a protected endpoint without a token.

Expected:

```text
401 Unauthorized
```

Then provide a valid JWT and call it again.

Expected:

```text
200 OK
```

---

## Step 9 — Test Frontend

Open the React application.

Verify:

```text
Login
   ↓
Dashboard
   ↓
Tasks
   ↓
Profile
```

---

## Step 10 — Test Tasks

Verify that the user can perform the task operations available to their role.

Check:

- Task creation
- Task listing
- Task update
- Task status
- Priority
- Category
- Due date
- Assigned user where applicable

---

## Step 11 — Test Admin Behavior

Login with an administrator account.

Verify:

```text
All Tasks
```

is displayed instead of:

```text
My Tasks
```

Verify that:

```text
ADMIN
```

is displayed in the UI.

---

## Step 12 — Test Normal User Behavior

Login with a regular user.

Verify:

```text
My Tasks
```

is displayed.

The:

```text
ADMIN
```

indicator should not appear.

---

## Step 13 — Test Logout

Click Logout.

Verify:

```text
Token removed
       ↓
Redirect to Login
```

Then try accessing protected functionality again.

The user should need to authenticate again.

---

# Troubleshooting

## API returns 401 Unauthorized

Check:

1. Is the JWT token present?
2. Is the Authorization header correct?
3. Is the token expired?
4. Is the signing key correct?
5. Are issuer/audience settings consistent?
6. Is `UseAuthentication()` configured?
7. Is `UseAuthorization()` configured?
8. Is the endpoint protected intentionally?

---

## API returns 500 with JWT key error

If an error similar to:

```text
IDX10703: Cannot create SymmetricSecurityKey, key length is zero
```

appears, check the JWT configuration.

Make sure the configuration property expected by the JWT service matches the actual appsettings property.

For example:

```text
Jwt:SecretKey
```

must exist and contain a valid secret.

---

## Frontend cannot connect to backend

Check that:

```text
Frontend API URL
        ↓
Backend URL
```

matches the actual running backend.

For local development, use:

```text
localhost
```

with the correct API port.

Also verify that the backend is running before testing frontend API requests.

---

## Swagger works on localhost but not on IP address

For local development, prefer:

```text
http://localhost:<port>/swagger
```

unless the API has explicitly been configured to listen on the machine's network interface.

The application must be bound to the appropriate interface if it needs to accept requests through the machine's LAN IP.

---

## Database connection fails

Check:

- SQL Server service is running
- Server/instance name is correct
- Database name is correct
- Authentication method is correct
- Connection string is valid
- `TrustServerCertificate=True` is present when required for local development

---

# Security Notes

The project uses JWT authentication, but production deployments should use stronger security practices.

Do not commit:

- Production JWT secrets
- Database passwords
- API keys
- Connection credentials

Sensitive values should be stored using environment variables, secret managers, or deployment-specific configuration.

---

# Development Summary

The project development covered the following major stages:

```text
Project Setup
     ↓
Clean Architecture
     ↓
Domain Entities
     ↓
Persistence / EF Core
     ↓
SQL Server
     ↓
ASP.NET Core Identity
     ↓
Authentication
     ↓
JWT Authentication
     ↓
JWT Testing
     ↓
Authorization
     ↓
Protected Endpoints
     ↓
React Frontend
     ↓
Role-Based UI
     ↓
Task Management
     ↓
Testing
     ↓
SonarQube
     ↓
GitHub Actions / CI
```

---

# Key Learning Outcomes

The project demonstrates practical understanding of:

- ASP.NET Core Web API development
- Clean Architecture
- Dependency Injection
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT authentication
- Role-based authorization
- Protected API endpoints
- React frontend development
- API integration with Axios
- Client-side routing
- Responsive UI
- Logging
- Exception handling
- Unit/integration testing structure
- Git feature branching
- Pull Requests
- GitHub Actions
- SonarQube code-quality analysis

---

# Final Verification Checklist

Before submitting or demonstrating the project, verify:

- [ ] SQL Server is running
- [ ] Database connection string is correct
- [ ] Database migrations are applied
- [ ] Backend builds successfully
- [ ] Backend starts successfully
- [ ] Swagger opens successfully
- [ ] Frontend dependencies are installed
- [ ] Frontend API URL is correct
- [ ] Frontend starts successfully
- [ ] Registration works
- [ ] Login works
- [ ] JWT is generated
- [ ] JWT is stored by the frontend
- [ ] Protected endpoints reject unauthenticated requests
- [ ] Protected endpoints accept valid JWTs
- [ ] Admin role is recognized
- [ ] Normal user role is recognized
- [ ] Admin sees `All Tasks`
- [ ] Normal user sees `My Tasks`
- [ ] Admin indicator appears only for admins
- [ ] Task functionality works
- [ ] Profile page works
- [ ] Logout removes the authentication token
- [ ] SonarQube workflow completes successfully
- [ ] No secrets are committed to Git

---

# Conclusion

TaskFlow combines a secure ASP.NET Core backend with a React frontend to provide a complete task management experience.

The backend uses Clean Architecture to maintain separation of concerns, ASP.NET Core Identity for user management, JWT for stateless authentication, and role-based authorization for access control.

The React frontend provides the user interface and communicates with the protected backend APIs.

The project also incorporates database migrations, logging, automated code-quality analysis, Git-based collaboration, and CI/CD through GitHub Actions.

The recommended demonstration flow is:

```text
Run SQL Server
      ↓
Apply Database Migration
      ↓
Run ASP.NET Core API
      ↓
Open Swagger
      ↓
Register / Login
      ↓
Obtain JWT
      ↓
Test Protected Endpoint
      ↓
Run React Frontend
      ↓
Test Dashboard
      ↓
Test Tasks
      ↓
Test Admin/User Roles
      ↓
Test Logout
```

This provides a complete end-to-end verification of the TaskFlow application.
