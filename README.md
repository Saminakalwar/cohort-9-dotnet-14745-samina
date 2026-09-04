# TaskFlow — Task Management Tool

A full-stack Task Management application built using **ASP.NET Core Web API, React.js, Entity Framework Core, SQL Server, ASP.NET Core Identity, and JWT authentication**.

The application provides:

- User registration and login
- JWT-based authentication
- Role-based authorization
- Admin and regular-user functionality
- Task management
- Task categories
- Task assignment
- User profile
- Responsive React frontend
- SQL Server persistence
- Swagger/OpenAPI API documentation
- Logging and centralized exception handling
- Unit and integration testing structure
- SonarQube Cloud code-quality analysis
- GitHub Actions CI

---

# Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Architecture](#project-architecture)
- [Prerequisites](#prerequisites)
- [Project Setup](#project-setup)
  - [Clone Repository](#1-clone-the-repository)
  - [Backend Setup](#2-backend-setup)
  - [Database Setup](#3-database-setup)
  - [Apply Database Migrations](#4-apply-database-migrations)
  - [Run Backend](#5-run-the-backend)
  - [Open Swagger](#6-open-swagger)
  - [Frontend Setup](#7-frontend-setup)
  - [Run Frontend](#8-run-the-frontend)

- [Application Usage](#application-usage)
  - [Registration](#registration)
  - [Login](#login)
  - [JWT Authentication](#jwt-authentication)
  - [Dashboard](#dashboard)
  - [Task Management](#task-management)
  - [Categories](#categories)
  - [Profile](#profile)
  - [Logout](#logout)

- [Role-Based Access](#role-based-access)
- [How to Test Authentication](#how-to-test-authentication)
- [How to Test Authorization](#how-to-test-authorization)
- [Swagger API Testing](#swagger-api-testing)
- [Frontend API Communication](#frontend-api-communication)
- [Database and EF Core](#database-and-ef-core)
- [Logging and Exception Handling](#logging-and-exception-handling)
- [Automated Testing](#automated-testing)
- [SonarQube and GitHub Actions](#sonarqube-and-github-actions)
- [Troubleshooting](#troubleshooting)
- [Security Notes](#security-notes)
- [Final Verification Checklist](#final-verification-checklist)

---

# Project Overview

**TaskFlow** is a full-stack task management system.

The backend is implemented using ASP.NET Core Web API and follows a **Clean Architecture** structure. The frontend is implemented using React.js and communicates with the backend through REST APIs.

The application uses:

```text
React.js
   │
   │ HTTP / Axios
   ▼
ASP.NET Core Web API
   │
   ├── Authentication / Authorization
   ├── Application Services
   ├── Domain
   ├── Infrastructure
   └── Persistence
           │
           ▼
       SQL Server
```

Authentication is implemented using:

```text
ASP.NET Core Identity
        +
JWT Bearer Authentication
        +
Role-Based Authorization
```

---

# Features

## Authentication

- User registration
- User login
- Password validation using ASP.NET Core Identity
- JWT token generation
- JWT token validation
- Protected API endpoints
- Automatic JWT attachment to frontend API requests
- Logout through frontend token removal

## Authorization

The application supports:

- Administrator users
- Regular users

The frontend displays different navigation options according to the user's role, while the backend remains responsible for actual authorization enforcement.

## Task Management

Tasks support information such as:

- Title
- Description
- Due date
- Priority
- Status
- Category
- Assigned user

## Task Priority

The application supports:

```text
Low
Medium
High
Critical
```

## Task Status

The application supports:

```text
Pending
InProgress
Completed
```

## Categories

Tasks can be associated with categories.

A category can contain multiple tasks.

## User Profile

The profile page displays authenticated user information such as:

- First name
- Last name
- Email
- Role information

## Responsive Frontend

The frontend includes:

- Responsive sidebar
- Mobile menu
- Header
- Dashboard
- Tasks page
- Profile page
- Login page
- Registration page

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
- Tailwind CSS
- JavaScript
- Vite

The frontend package configuration contains scripts for development, production build, linting, and preview.

## Development and CI Tools

- Visual Studio Code
- C# Dev Kit
- SQL Server Developer Edition
- SQL Server Management Studio
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

The solution contains the API, Application, Domain, Infrastructure, Persistence, Unit Tests, and Integration Tests projects.

## Domain

Contains the core business entities and enums.

Main entities include:

```text
ApplicationUser
TaskItem
Category
```

Main enums include:

```text
TaskPriority
TaskStatus
UserRole
```

The Domain layer contains business concepts and does not depend on API implementation details.

## Application

Contains application-level logic and abstractions, including:

```text
DTOs
Interfaces
Services
```

Application interfaces define contracts used by implementations in other layers.

## Persistence

Responsible for database-related functionality, including:

- Entity Framework Core
- DbContext
- Identity configuration
- Database configuration
- Entity relationships
- EF Core migrations

## Infrastructure

Contains implementations of application services and external concerns.

Authentication-related functionality, including authentication/JWT services, is handled here.

## API

Contains:

- Controllers
- Middleware
- Dependency injection configuration
- Authentication configuration
- Authorization configuration
- Swagger configuration
- Application startup

---

# Prerequisites

Install the following before running the project:

1. .NET 10 SDK
2. Node.js
3. npm
4. SQL Server Developer Edition
5. SQL Server Management Studio (SSMS)
6. Git

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

The repository contains both the backend and frontend.

---

# 2. Backend Setup

Navigate to the backend:

```bash
cd Backend
```

Restore NuGet dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

The solution should build successfully before running the API.

---

# 3. Database Setup

The application uses **SQL Server with Entity Framework Core**.

Make sure SQL Server is installed and running.

Open SQL Server Management Studio and make sure you can connect to your local SQL Server instance.

The application reads its database connection string from:

```text
Backend/src/TaskManagement.API/appsettings.json
```

The connection string configuration is:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_LOCAL_SQL_SERVER_CONNECTION_STRING"
  }
}
```

Use a connection string appropriate for the SQL Server installation on the machine where the project is being run.

For example, a local SQL Server setup may use:

```text
Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Do not use this example blindly if your SQL Server instance has a different server or instance name.

---

# 4. Apply Database Migrations

The project uses Entity Framework Core migrations to create/update the database schema.

First make sure the EF CLI is installed:

```bash
dotnet ef --version
```

If it is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Verify again:

```bash
dotnet ef --version
```

Then apply the existing migrations:

```bash
dotnet ef database update
```

If the EF CLI requires explicit project selection because of the multi-project architecture, use:

```bash
dotnet ef database update \
  --project src/TaskManagement.Persistence \
  --startup-project src/TaskManagement.API
```

After the migration completes, verify the database in SSMS.

The database should contain the application tables as well as the ASP.NET Core Identity tables.

> **Important:** Use the migration command that matches the solution's EF Core configuration. Do not create a new migration just to run the existing application.

---

# 5. Run the Backend

From the `Backend` directory:

```bash
dotnet run --project src/TaskManagement.API
```

The configured HTTP development URL is:

```text
http://localhost:5135
```

The application also has an HTTPS profile configured for:

```text
https://localhost:7025
```

The development launch configuration starts Swagger automatically.

For the simplest local setup, use:

```text
http://localhost:5135
```

---

# 6. Open Swagger

Once the backend is running, open:

```text
http://localhost:5135/swagger
```

Swagger provides an interactive interface for testing the API.

Use Swagger to test:

- Registration
- Login
- Protected endpoints
- Task APIs
- Category APIs
- Profile-related APIs
- Administrative APIs

Swagger is configured with Bearer JWT authentication in the API.

---

# 7. Frontend Setup

Open a second terminal.

From the repository root:

```bash
cd Frontend
```

Install frontend dependencies:

```bash
npm install
```

The project uses React, Axios, React Router, React Hot Toast, Tailwind CSS and Vite.

---

# Frontend Environment Configuration

Create/configure the frontend `.env` file:

```env
VITE_API_URL=http://localhost:5135/api
```

This is the API URL currently used by the frontend.

The Axios configuration reads `VITE_API_URL` and uses `http://localhost:5135/api` as its local fallback.

The current development environment configuration points to:

```text
http://localhost:5135/api
```

> After changing `.env`, restart the Vite development server.

---

# 8. Run the Frontend

From the `Frontend` directory:

```bash
npm run dev
```

Vite will display the frontend URL in the terminal.

The usual development URL is:

```text
http://localhost:5173
```

The frontend package defines `npm run dev` as the Vite development command.

Open the URL shown by Vite in your browser.

---

# Complete Startup Order

For the first run, follow this exact order:

```text
1. Start SQL Server
        ↓
2. Configure database connection string
        ↓
3. Apply EF Core migrations
        ↓
4. Start Backend
        ↓
5. Open Swagger
        ↓
6. Start Frontend
        ↓
7. Open React application
        ↓
8. Register a user
        ↓
9. Login
        ↓
10. Test Dashboard
        ↓
11. Test Tasks
        ↓
12. Test Profile
        ↓
13. Test Admin/User behavior
        ↓
14. Test Logout
```

---

# Application Usage

## Registration

Open the frontend registration page.

Enter valid user information and submit the form.

The registration request is sent to the ASP.NET Core API.

ASP.NET Core Identity validates the user information and stores the user in the database.

After successful registration, proceed to login.

---

# Login

Open the login page.

Enter the registered user's credentials.

The frontend sends the login request to the backend.

On successful authentication:

```text
Credentials
     ↓
ASP.NET Core Identity
     ↓
Credentials validated
     ↓
JWT generated
     ↓
JWT returned to frontend
     ↓
Token stored in browser
     ↓
User enters authenticated application
```

The JWT is then used for protected API requests.

---

# JWT Authentication

The application uses JWT Bearer Authentication.

A protected request contains:

```http
Authorization: Bearer <JWT_TOKEN>
```

The backend validates:

1. Token signature
2. Signing key
3. Issuer
4. Audience
5. Token lifetime
6. Authentication information

The API configures JWT validation through `JwtSettings`.

The current configuration contains:

```json
{
  "JwtSettings": {
    "SecretKey": "your-development-secret",
    "Issuer": "TaskManagement",
    "Audience": "TaskManagementClient",
    "ExpiryMinutes": 60
  }
}
```

The actual application configuration uses the `JwtSettings` section, with issuer `TaskManagement`, audience `TaskManagementClient`, and a 60-minute token expiry.

> Never commit a real production JWT secret to source control.

---

# Authentication Middleware

The API configures authentication and authorization as part of the ASP.NET Core request pipeline.

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

`AddAuthentication()` configures the authentication services.

`UseAuthentication()` authenticates the incoming request.

`UseAuthorization()` checks whether the authenticated user is allowed to access the requested endpoint.

The application uses the middleware in this order:

```csharp
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

This order is configured in the API startup code.

---

# Dashboard

After login, the user is redirected to the authenticated application.

The dashboard provides an overview of the task-management application and authenticated user experience.

---

# Task Management

The Tasks section allows authenticated users to work with tasks according to their role and permissions.

A task can contain:

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

Typical task flow:

```text
Create Task
     ↓
Enter Title
     ↓
Enter Description
     ↓
Select Category
     ↓
Assign User
     ↓
Set Priority
     ↓
Set Status
     ↓
Set Due Date
     ↓
Save
     ↓
Update / Complete Task
```

---

# Task Priority

Available priorities:

```text
Low
Medium
High
Critical
```

---

# Task Status

Available statuses:

```text
Pending
InProgress
Completed
```

---

# Categories

Tasks can be associated with categories.

Conceptually:

```text
Category
│
├── Id
├── Name
└── Tasks
```

A category can be associated with multiple tasks.

---

# Profile

The Profile page displays information about the currently authenticated user.

The frontend generates the user's initials from their first and last name.

For example:

```text
Samina Kalwar
      ↓
SK
```

The profile/header can also display administrator information when the authenticated user has the Admin role.

---

# Logout

Logout removes the JWT token stored by the frontend.

The flow is:

```text
Click Logout
     ↓
Remove JWT token
     ↓
Show logout notification
     ↓
Navigate to /login
```

The frontend removes the token using browser storage and redirects the user to the login page.

After logout, the user must authenticate again before accessing protected functionality.

---

# Role-Based Access

The application supports two main roles:

```text
Admin
User
```

---

# Administrator

An administrator can access administrative functionality and view all tasks.

The administrator navigation includes:

```text
Dashboard
All Tasks
Profile
```

The frontend also displays:

```text
ADMIN
```

for an administrator.

---

# Regular User

A regular user sees:

```text
Dashboard
My Tasks
Profile
```

The `ADMIN` indicator is not displayed for a regular user.

---

# Important Security Principle

The frontend role check is for the user interface only.

For example, hiding:

```text
All Tasks
```

from a regular user does not provide security by itself.

The backend must enforce authorization.

Therefore:

```text
Frontend
   ↓
Controls UI visibility

Backend
   ↓
Enforces actual permissions
```

This ensures users cannot gain administrative access simply by manually calling an API.

---

# How to Test Authentication

## Test 1 — Registration

1. Start SQL Server.
2. Start the backend.
3. Start the frontend.
4. Open the registration page.
5. Enter valid user information.
6. Submit the form.
7. Confirm that registration succeeds.

---

## Test 2 — Login

1. Open the login page.
2. Enter the registered credentials.
3. Submit the form.
4. Confirm that login succeeds.
5. Confirm that the user reaches the authenticated application.
6. Confirm that the JWT is stored by the frontend.

---

## Test 3 — Invalid Login

Enter an incorrect password.

Expected result:

```text
401 Unauthorized
```

The frontend should display an appropriate error message.

---

## Test 4 — Protected Endpoint Without JWT

Use Swagger or an API client to call a protected endpoint without authentication.

Example:

```http
GET /api/Auth/protected
```

Expected result:

```text
401 Unauthorized
```

---

## Test 5 — Protected Endpoint With JWT

Login and obtain the JWT.

Provide the JWT through Swagger's **Authorize** button.

Then call:

```http
GET /api/Auth/protected
```

Expected result:

```text
200 OK
```

This confirms that JWT authentication is working.

---

# How to Test Authorization

Authorization should be tested using both administrator and regular-user accounts.

## Admin Test

Login using an administrator account.

Verify:

```text
Dashboard
All Tasks
Profile
ADMIN
```

are available/displayed as expected.

---

## Regular User Test

Login using a normal user account.

Verify:

```text
Dashboard
My Tasks
Profile
```

are displayed.

Verify that:

```text
ADMIN
```

is not displayed.

---

# Testing an Existing User as Admin

If an administrator account needs to be assigned to an existing user, use the project's administrative endpoint through Swagger if it is available in the running API.

For example, the project supports the make-admin operation using the user's email.

After changing a user's role:

1. Logout.
2. Login again.
3. Use the newly issued JWT.

This is important because role information is included in the JWT. A previously issued token may not contain newly assigned role information.

---

# Swagger API Testing

Swagger is available during development at:

```text
http://localhost:5135/swagger
```

---

## Swagger Authentication Procedure

### Step 1 — Start Backend

```bash
dotnet run --project src/TaskManagement.API
```

### Step 2 — Open Swagger

```text
http://localhost:5135/swagger
```

### Step 3 — Login

Use the login endpoint.

Copy the JWT returned by the API.

### Step 4 — Authorize Swagger

Click:

```text
Authorize
```

Enter:

```text
Bearer <your-jwt-token>
```

### Step 5 — Test Protected Endpoints

Execute a protected endpoint.

A valid token should result in successful authentication.

---

# Frontend API Communication

The React application uses Axios for communication with the backend.

The configured API base URL is:

```text
http://localhost:5135/api
```

The frontend reads this value from:

```text
VITE_API_URL
```

The Axios instance is configured to use the environment variable and has the same localhost API URL as a fallback.

---

# Automatic JWT Attachment

The frontend automatically reads the token from browser storage.

For authenticated API requests it adds:

```http
Authorization: Bearer <token>
```

This means protected API requests do not need to manually add the token from every component.

---

# Handling Expired/Invalid Authentication

The frontend also handles `401 Unauthorized` responses.

When a `401` response is received:

```text
401 Unauthorized
       ↓
Remove stored JWT
       ↓
Redirect to /login
```

This behavior is implemented through the Axios response interceptor.

---

# Responsive Frontend

The frontend includes a responsive sidebar.

On smaller screens:

```text
Mobile Menu
     ↓
Sidebar Opens
     ↓
Overlay Appears
     ↓
User Selects Navigation
     ↓
Sidebar Closes
```

The header includes:

- Mobile menu
- TaskFlow branding
- Current page title
- User initials
- User name
- Admin indicator where applicable

---

# Database and EF Core

The application uses:

```text
ASP.NET Core
      ↓
Entity Framework Core
      ↓
SQL Server
```

EF Core is responsible for mapping application entities to database tables.

The Persistence project contains the database context, Identity configuration, relationships, and migrations.

---

# Identity Database

ASP.NET Core Identity manages application users and authentication-related data.

After applying migrations, Identity-related tables should be present in the SQL Server database.

---

# Database Migration Workflow

When database schema changes are introduced:

```text
Modify Entity
     ↓
Create Migration
     ↓
Review Migration
     ↓
Apply Migration
     ↓
Database Updated
```

Example migration command:

```bash
dotnet ef migrations add MigrationName
```

Then apply it:

```bash
dotnet ef database update
```

For this multi-project solution, explicit project arguments may be required:

```bash
dotnet ef migrations add MigrationName \
  --project src/TaskManagement.Persistence \
  --startup-project src/TaskManagement.API
```

and:

```bash
dotnet ef database update \
  --project src/TaskManagement.Persistence \
  --startup-project src/TaskManagement.API
```

---

# Logging and Exception Handling

The API uses **Serilog** for application logging.

The application configures:

- Console logging
- Daily rolling file logs

Log files are written under:

```text
logs/
```

The API also uses centralized exception-handling middleware.

The middleware is registered in the request pipeline before the remaining request processing.

This provides a centralized place to handle unexpected application exceptions.

---

# Automated Testing

The solution contains:

```text
TaskManagement.UnitTests
TaskManagement.IntegrationTests
```

Run tests from the backend/solution directory:

```bash
dotnet test
```

A successful result indicates that the configured automated tests have passed.

For a more complete verification:

```bash
dotnet restore
dotnet build
dotnet test
```

---

# Frontend Validation

The frontend provides the following npm scripts:

```bash
npm run dev
npm run build
npm run lint
npm run preview
```

These scripts are defined in the project's `package.json`.

For frontend validation:

```bash
npm run lint
```

To verify that the production build succeeds:

```bash
npm run build
```

---

# SonarQube and GitHub Actions

The project includes SonarQube Cloud code-quality analysis and GitHub Actions CI.

The CI workflow is intended to automatically verify the project during GitHub workflow execution.

The workflow includes the required Java setup for SonarQube analysis.

To verify CI:

1. Push changes to the configured branch.
2. Open the repository on GitHub.
3. Open the **Actions** tab.
4. Select the project's build/CI workflow.
5. Verify that the workflow completes successfully.
6. Review SonarQube analysis results when available.

---

# Git Workflow

Development is organized using feature branches.

Typical workflow:

```text
develop
   ↓
Create Feature Branch
   ↓
Implement Feature
   ↓
Test Locally
   ↓
Commit
   ↓
Push Feature Branch
   ↓
Create Pull Request
   ↓
Review / CI
   ↓
Merge
```

Before pushing changes:

```bash
git status
```

Review the changed files.

Then:

```bash
git add .
git commit -m "your commit message"
git push
```

---

# Troubleshooting

## Backend Does Not Start

Check:

```text
.NET SDK installed
SQL Server running
Connection string configured
Correct project selected
```

Try:

```bash
dotnet restore
dotnet build
dotnet run --project src/TaskManagement.API
```

---

# Swagger Does Not Open

Make sure the backend is running.

Use:

```text
http://localhost:5135/swagger
```

Swagger is configured for the Development environment.

---

# Frontend Cannot Connect to Backend

Check that the backend is running at:

```text
http://localhost:5135
```

Then verify the frontend `.env`:

```env
VITE_API_URL=http://localhost:5135/api
```

Restart the frontend after changing `.env`.

Also check the browser console for Axios/network errors.

---

# CORS Error

The backend currently allows the frontend development origin:

```text
http://localhost:5173
```

The API configures a CORS policy for this frontend origin.

If the frontend is running on a different port, update the backend CORS configuration accordingly.

---

# API Returns 401 Unauthorized

Check:

1. The user is logged in.
2. A JWT exists in browser storage.
3. The JWT has not expired.
4. The Authorization header contains:

   ```text
   Bearer <token>
   ```

5. The issuer is correct.
6. The audience is correct.
7. The signing key is correct.
8. `UseAuthentication()` is configured.
9. `UseAuthorization()` is configured.
10. The endpoint actually requires authentication.

---

# JWT Configuration Error

If the application reports a JWT signing-key error, verify that the configuration section is named:

```text
JwtSettings
```

and contains:

```text
SecretKey
Issuer
Audience
ExpiryMinutes
```

The API reads the `JwtSettings` section directly.

---

# Admin Role Not Appearing

If a user has recently been assigned the Admin role:

1. Logout.
2. Login again.
3. Obtain a new JWT.
4. Refresh the application.

The frontend determines the current role from the JWT claims.

---

# Database Connection Error

Check:

- SQL Server service is running.
- SQL Server instance name is correct.
- Database name is correct.
- Authentication method is correct.
- Connection string is correct.
- Required database migrations have been applied.

---

# Port Conflict

If port `5135` is already in use, stop the process using that port or use the configured launch profile/port appropriate for your environment.

If the backend port changes, update:

```env
VITE_API_URL=http://localhost:<new-port>/api
```

and restart the frontend.

---

# Security Notes

Do not commit real secrets to GitHub.

Do not commit:

- Production JWT secrets
- Database passwords
- API keys
- Production connection credentials

For production environments, use environment variables, secret managers, or deployment-specific configuration.

The development JWT configuration should be treated as development-only configuration.

---

# Complete End-to-End Testing Procedure

Use the following sequence when demonstrating or evaluating the application.

## Backend

```bash
cd Backend
dotnet restore
dotnet build
dotnet ef database update
dotnet run --project src/TaskManagement.API
```

Verify:

```text
http://localhost:5135/swagger
```

---

## Frontend

Open another terminal:

```bash
cd Frontend
npm install
npm run dev
```

Verify the frontend URL displayed by Vite, normally:

```text
http://localhost:5173
```

---

## Authentication Test

```text
Register
   ↓
Login
   ↓
JWT generated
   ↓
Dashboard
```

---

## Protected Endpoint Test

Without JWT:

```text
401 Unauthorized
```

With valid JWT:

```text
200 OK
```

---

## Regular User Test

Verify:

```text
Dashboard
My Tasks
Profile
```

and confirm:

```text
ADMIN
```

is not displayed.

---

## Admin Test

Login using an administrator account.

Verify:

```text
Dashboard
All Tasks
Profile
ADMIN
```

---

## Task Test

Verify:

```text
Create Task
View Task
Update Task
Change Priority
Change Status
Assign Category
Assign User
Set Due Date
Complete Task
```

---

## Profile Test

Verify that:

```text
First Name
Last Name
Email
Role
```

are displayed correctly.

---

## Logout Test

Click Logout.

Verify:

```text
JWT removed
      ↓
Redirect to Login
```

Attempt to access protected functionality again and verify that authentication is required.

---

# Final Verification Checklist

Before submitting the project, verify every item below.

## Environment

- [ ] .NET 10 SDK installed
- [ ] Node.js installed
- [ ] npm installed
- [ ] SQL Server installed and running
- [ ] SSMS can connect to SQL Server

## Backend

- [ ] `dotnet restore` succeeds
- [ ] `dotnet build` succeeds
- [ ] Database connection string is configured
- [ ] EF Core migrations are applied
- [ ] API starts successfully
- [ ] API runs on the expected localhost port
- [ ] Swagger opens successfully

## Database

- [ ] Database exists
- [ ] Identity tables exist
- [ ] Application tables exist
- [ ] Relationships are created correctly
- [ ] Migrations are applied successfully

## Authentication

- [ ] Registration works
- [ ] Login works
- [ ] Invalid login returns an appropriate error
- [ ] JWT is generated
- [ ] JWT is stored by the frontend
- [ ] JWT is sent with protected API requests
- [ ] Protected endpoints reject unauthenticated requests
- [ ] Protected endpoints accept valid JWTs
- [ ] Expired/invalid authentication is handled

## Authorization

- [ ] Admin role works
- [ ] Regular User role works
- [ ] Admin sees `All Tasks`
- [ ] Regular User sees `My Tasks`
- [ ] Admin indicator is shown for administrators
- [ ] Admin indicator is hidden for regular users
- [ ] Backend authorization is enforced

## Tasks

- [ ] Create task works
- [ ] Read/list tasks works
- [ ] Update task works
- [ ] Delete functionality works if provided
- [ ] Priority works
- [ ] Status works
- [ ] Category works
- [ ] Due date works
- [ ] Assignment works where applicable

## Frontend

- [ ] Login page works
- [ ] Registration page works
- [ ] Dashboard works
- [ ] Tasks page works
- [ ] Profile page works
- [ ] Responsive sidebar works
- [ ] Mobile menu works
- [ ] Header displays correct user information
- [ ] Toast notifications work
- [ ] Axios API communication works

## Logout

- [ ] Logout removes the JWT
- [ ] User is redirected to login
- [ ] Protected functionality cannot be accessed without authentication

## Testing and Quality

- [ ] `dotnet test` runs successfully
- [ ] `npm run lint` succeeds
- [ ] `npm run build` succeeds
- [ ] GitHub Actions workflow succeeds
- [ ] SonarQube analysis completes
- [ ] No production secrets are committed

---

# Quick Start

For an evaluator who wants to run the application quickly:

### 1. Start SQL Server

Make sure SQL Server is running.

### 2. Configure the backend database

Set the appropriate connection string in:

```text
Backend/src/TaskManagement.API/appsettings.json
```

### 3. Start Backend

```bash
cd Backend
dotnet restore
dotnet build
dotnet ef database update
dotnet run --project src/TaskManagement.API
```

Open:

```text
http://localhost:5135/swagger
```

### 4. Start Frontend

Open another terminal:

```bash
cd Frontend
npm install
```

Ensure `.env` contains:

```env
VITE_API_URL=http://localhost:5135/api
```

Then:

```bash
npm run dev
```

Open the frontend URL shown by Vite.

### 5. Demonstrate the Application

```text
Register
   ↓
Login
   ↓
Dashboard
   ↓
Create / View / Update Tasks
   ↓
Profile
   ↓
Test Role-Based Access
   ↓
Logout
```

For API-level verification:

```text
Swagger
   ↓
Login
   ↓
Copy JWT
   ↓
Authorize
   ↓
Call Protected Endpoint
```

---

# Conclusion

TaskFlow demonstrates a complete full-stack application using a Clean Architecture ASP.NET Core backend and a React.js frontend.

The project integrates:

- ASP.NET Core Web API
- Clean Architecture
- ASP.NET Core Identity
- JWT authentication
- Role-based authorization
- Entity Framework Core
- SQL Server
- React.js
- Axios
- Responsive UI
- Serilog logging
- Exception handling
- Unit and integration testing
- Swagger/OpenAPI
- SonarQube Cloud
- GitHub Actions

The complete application can be verified by running the backend and database, applying the EF Core migrations, starting the React frontend, and testing authentication, authorization, task management, profile functionality, API security, and logout according to the procedures described above.
