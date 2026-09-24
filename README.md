# 🚀 TaskFlow — Task Management System

A full-stack Task Management application built with **ASP.NET Core Web API, React.js, Entity Framework Core, SQL Server, ASP.NET Core Identity, and JWT authentication**.

The application supports:

* 🔐 User registration and JWT-based login
* 👥 Role-based access for **Admin** and **User**
* ✅ Task creation and management
* 📂 Task categories
* 👤 Task assignment
* 📅 Due dates, priorities, and statuses
* 👤 User profile
* 🛡️ Protected API endpoints
* 📖 Swagger API documentation
* 📝 Logging and centralized exception handling
* 🧪 Unit and integration testing structure
* 🔎 SonarQube Cloud analysis
* ⚙️ GitHub Actions CI

The backend follows a **Clean Architecture** structure, while the frontend is built using React.js and communicates with the backend through REST APIs.


# 🚀 Run the Project Locally

Follow the steps below to clone, configure, and run the Task Management System on your local machine.

## 1. 📁 Create a Project Folder

Create a folder anywhere on your PC, for example:

```text
Task Management
```

Open **Git Bash** inside this folder and clone the repository from the `develop` branch:

```bash
git clone -b develop https://github.com/Saminakalwar/cohort-9-dotnet-14745-samina.git
```

Move into the cloned project:

```bash
cd cohort-9-dotnet-14745-samina
```

Inside the project, you will see the main folders:

```text
Frontend
Backend
```

Move into the backend:

```bash
cd Backend
```

---

## 2. 🗄️ Set Up SQL Server

Open **SQL Server Management Studio (SSMS)**.
In my setup, I used **SSMS 22**.

Create/connect to your local SQL Server instance. For the local setup, the connection will use:

```text
Server: localhost
Authentication: Windows Authentication
```
<img width="1726" height="905" alt="Screenshot 2026-09-24 103839" src="https://github.com/user-attachments/assets/f7ed3e1d-dc57-4134-bacf-53dd6c576d12" />

The application uses a connection string stored through **.NET User Secrets**, so the database password/connection details do not need to be committed to the repository.

---

## 3. 🔐 Check the Current Connection String

From the `Backend` folder, run:

```bash
dotnet user-secrets list --project src/TaskManagement.API
```

If the project was previously configured to use Azure SQL, you may see the Azure connection string here.

---

## 4. 🔄 Configure the Local Database Connection

To connect the application to your local SQL Server database, set the connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;" --project "src/TaskManagement.API"
```

Then verify that it was updated successfully:

```bash
dotnet user-secrets list --project "src/TaskManagement.API"
```

You should see:

```text
ConnectionStrings:DefaultConnection = Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

---

## 5. 🏗️ Create/Update the Database Tables

Run the EF Core database update command from the `Backend` folder:

```bash
dotnet ef database update --project src/TaskManagement.Persistence --startup-project src/TaskManagement.API
```
This takes the existing migrations/changes and applies them to the actual database.

A successful result should look similar to:

```text
Build succeeded.
Done.
```

This will create/update the `TaskManagementDb` database and apply all available Entity Framework Core migrations.

### 🔎 Verify the Tables

Go back to **SQL Server Management Studio** and refresh:

```text
Databases
└── TaskManagementDb
    └── Tables
```

If the tables are not immediately visible, right-click **Databases → Refresh**.

---
<img width="360" height="566" alt="Screenshot 2026-09-24 105618" src="https://github.com/user-attachments/assets/84a90b82-98a0-4164-81d3-c087619e8005" />


## 6. ▶️ Run the Backend API

Once the database and tables have been created successfully, move into the API project:

```bash
cd src/TaskManagement.API
```

Run the application:

```bash
dotnet run
```

You should see output similar to:

```text
[10:58:05 INF] Now listening on: http://localhost:xxxx
[10:58:05 INF] Application started. Press Ctrl+C to shut down.
[10:58:05 INF] Hosting environment: Development
[10:58:05 INF] Content root path: D:\Task\BE\cohort-9-dotnet-14745-samina\Backend\src\TaskManagement.API
```

> ⚠️ **Important:** Keep this terminal running while using the API.

---

## 7. 📖 Open Swagger

Copy the HTTP URL shown in the terminal, for example:

```text
http://localhost:xxxx
```

Add `/swagger` at the end and open it in your browser:

```text
http://localhost:xxxx/swagger
```

Swagger will display all available API endpoints and allow you to test them directly.

<img width="1231" height="944" alt="Screenshot 2026-09-24 105848" src="https://github.com/user-attachments/assets/9984e737-8d9a-4468-8dfb-bbbb24c404b2" />


---

## 8. 🔑 Test Authentication

Run the APIs in the following sequence.

### 1️⃣ Register

```text
POST /api/Auth/register
```

Create a new user by providing the required registration details.

### 2️⃣ Login

```text
POST /api/Auth/login
```

Copy the **JWT token** returned by the login API.

### 3️⃣ Authorize Swagger

Click the **Authorize 🔒** button at the top of Swagger.

Paste the returned token into the token field and authorize.

### 4️⃣ Test Protected API

Now test:

```text
GET /api/Auth/protected
```

This verifies that JWT authentication and authorization are working correctly.

---

## 9. 📂 Test Categories

Create a category using:

```text
POST /api/Categories
```

Provide the category name in the request body.

Then verify it using:

```text
GET /api/Categories
```

---

## 10. ✅ Test Tasks

Finally, test the task API:

```text
GET /api/Task
```

At this point, the backend should be running locally with the SQL Server database connected and the APIs available through Swagger.

---

## ⚡ Quick Setup Summary

For an already-cloned project, the main commands are:

```bash
cd Backend

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;" --project "src/TaskManagement.API"

dotnet ef database update --project src/TaskManagement.Persistence --startup-project src/TaskManagement.API

cd src/TaskManagement.API

dotnet run
```

Then open:

```text
http://localhost:xxxx/swagger
```

### 🔄 API Testing Flow

```text
Register
   ↓
Login
   ↓
Copy JWT Token
   ↓
Authorize 🔒
   ↓
Protected API
   ↓
Create Category
   ↓
Get Categories
   ↓
Get Tasks
```



# 🎨 Run the Frontend Locally

## 1. 📁 Go to the Frontend Folder

Open another terminal and run:

```bash
cd Frontend

## 2. 📦 Install Dependencies

```bash
npm install
```

## 3. ▶️ Start the Frontend

```bash
npm run dev
```

You should see:

```text
➜  Local:   http://localhost:5173/
```

## 4. 🌐 Open the Application

Open:

```text
http://localhost:5173/
```

> ⚠️ Keep the terminal running while using the frontend.

---



## 👤 User Flow


### 📝 Register

<img width="1920" height="993" alt="Screenshot 2026-09-24 121852" src="https://github.com/user-attachments/assets/7af9d893-4b5e-4b80-ac88-d4f315c94c8f" />


### 🔐 Login

<img width="1920" height="990" alt="Screenshot 2026-09-24 115753" src="https://github.com/user-attachments/assets/aa7f70ca-c094-4187-a9b7-1a560f395c82" />


### 👤 Profile

<img width="1920" height="985" alt="Screenshot 2026-09-24 115832" src="https://github.com/user-attachments/assets/e6d00647-1a12-40cd-a3ee-fbe03c4196df" />


### 🔐 Authorize with the same token generated in browser to create Category

<img width="1920" height="983" alt="Screenshot 2026-09-24 120124" src="https://github.com/user-attachments/assets/38f19015-399f-4a25-9275-1fe18221c6c3" />


### 🔐 Create Category 

<img width="1920" height="982" alt="Screenshot 2026-09-24 120219" src="https://github.com/user-attachments/assets/ce7e774b-f587-417a-9cd9-de256508e0fe" />


### ➕ Create Task

<img width="1920" height="999" alt="Screenshot 2026-09-24 120403" src="https://github.com/user-attachments/assets/56c648a6-2ddb-4fa1-8465-1c39b8d83d96" />


### 📋 Tasks

<img width="1920" height="1080" alt="Screenshot 2026-09-24 121019" src="https://github.com/user-attachments/assets/fb749ddd-805b-4499-9ff9-bac4a81af719" />


### 🔎 Search Tasks

<img width="1920" height="976" alt="Screenshot 2026-09-24 120912" src="https://github.com/user-attachments/assets/cde19a02-6345-4d89-8396-d8e8b4d46972" />

### 📊 Dashboard

<img width="1920" height="979" alt="Screenshot 2026-09-24 123300" src="https://github.com/user-attachments/assets/edf7f6c2-f32b-42da-927e-b17297a6b1e1" />


### 🗑️ Delete Task

<img width="1920" height="987" alt="Screenshot 2026-09-24 120934" src="https://github.com/user-attachments/assets/8a80f637-4e16-459a-b4f6-9555dbd4cac6" />


### 👁️ View Task

<img width="1920" height="985" alt="Screenshot 2026-09-24 121003" src="https://github.com/user-attachments/assets/9079f22c-2b45-420d-8f9a-c7735cb4b553" />



## 👑 Admin Role Setup

### 1. 👤 Create a User

First, create a normal user through the **Register** page.

Enter the required user details and complete registration.

### 2. 🔐 Login and Get JWT Token

Login with the newly created user's credentials.

Copy the JWT token from the login response.

### 3. 🛡️ Authorize in Swagger

Open Swagger and click **Authorize**.

Enter the JWT token:

```text
Bearer <your-token>
```

Click **Authorize**.

### 4. 👑 Make the User an Admin

In Swagger, use:

```text
POST /api/Auth/make-admin/{email}
```

Enter the email address of the user you want to make an admin.

Execute the request.

<img width="1920" height="990" alt="Screenshot 2026-09-24 124938" src="https://github.com/user-attachments/assets/c9aabba1-4ddc-4b15-9097-2a77c66b6e61" />

If successful, the user's role is updated from **User** to **Admin**.

### 5. 🚪 Logout from the Frontend

Go back to the frontend application and **Logout**.

This is required because the previously issued JWT still contains the old user role.

### 6. 🔑 Login Again

Login again using the same user's credentials.

A new JWT token is generated with the updated **Admin** role.

### 7. 👑 Access Admin Dashboard

After logging in again, the user can now access the **Admin Dashboard**.

<img width="1920" height="981" alt="Screenshot 2026-09-24 125036" src="https://github.com/user-attachments/assets/944312f2-0bae-4c6e-9b74-8c8271973760" />

## 👑 Admin Profile

<img width="1920" height="1003" alt="Screenshot 2026-09-24 125113" src="https://github.com/user-attachments/assets/64a48e28-ccc1-49e0-825f-9cd245db806a" />


Admin users can access additional functionality such as:

* 👥 View all users
* 📋 View users' tasks
* ✏️ Manage tasks
* 🗂️ Access admin-level functionality
* 🔐 Access features restricted to Admin users

<img width="1920" height="964" alt="Screenshot 2026-09-24 125103" src="https://github.com/user-attachments/assets/0a335038-4358-48e9-98e2-b8e8964d2ec0" />


> ⚠️ **Important:** Logout and login again after changing the user's role so that a new JWT containing the updated role is issued.



---

# 📚 Technical Documentation & Concepts

## 🏗️ Clean Architecture

The backend follows **Clean Architecture** and is organized into the following projects:

```text
Backend/
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

### Layer Responsibilities

* **Domain** → Entities and core business models
* **Application** → DTOs, interfaces, and application services
* **Persistence** → EF Core, DbContext, Identity, database configuration, and migrations
* **Infrastructure** → Authentication/JWT service implementations and external concerns
* **API** → Controllers, middleware, authentication/authorization, Swagger, and application startup

```text
React Frontend
      ↓
ASP.NET Core Web API
      ↓
Application / Domain
      ↓
Persistence + Infrastructure
      ↓
SQL Server
```

## 🔐 Authentication & Authorization

**Authentication** answers:

> Who is the user?

In this project, users register and log in using their email and password.

**Authorization** answers:

> What is the user allowed to access?

The project has two roles:

* **User**
* **Admin**

After login, the user's role is included in the JWT token. Protected API endpoints use this information to allow or deny access.

---

## 🎟️ JWT Authentication Flow

JWT stands for **JSON Web Token**.

The authentication flow is:

```text
Register
   ↓
Login
   ↓
Backend verifies credentials
   ↓
JWT token is generated
   ↓
Frontend stores the token
   ↓
Token is sent with API requests
   ↓
Backend validates the token
   ↓
User is authenticated
```

For protected requests, the token is sent in the request header:

```text
Authorization: Bearer <token>
```

The backend reads the token and identifies the authenticated user and their role.

---

## 🛡️ ASP.NET Core Identity

**ASP.NET Core Identity** is used to manage users and roles.

It handles things such as:

* User registration
* Password management
* User IDs
* Roles
* User authentication data

The project uses a custom `ApplicationUser` class that extends ASP.NET Core Identity's `IdentityUser`.

Roles such as **User** and **Admin** are stored and managed through Identity.

---

## 🗄️ Entity Framework Core & Migrations

**Entity Framework Core (EF Core)** is used to communicate with SQL Server.

Instead of writing SQL for every database operation, the application works with C# entities such as:

* `ApplicationUser`
* `TaskItem`
* `Category`

EF Core maps these entities to database tables.

### Migrations

Migrations keep track of database structure changes.

For example:

```text
C# Entity Changes
       ↓
Create Migration
       ↓
Database Update
       ↓
SQL Server structure is updated
```

The project uses:

```bash
dotnet ef database update
```

to apply existing migrations to the database.

---

## 📝 Logging & Exception Handling

The project uses **Serilog** for application logging.

Logs help developers understand what is happening inside the application and make it easier to investigate errors.

The project also uses centralized exception handling so that unexpected errors can be handled in one place instead of repeating error-handling code in every controller.

This helps provide consistent API error responses and useful logs for debugging.

---

## 🧪 Testing

The project contains separate test projects for testing the backend:

```text
tests/
├── TaskManagement.UnitTests/
└── IntegrationTests/
```

### Unit Tests

Unit tests check individual pieces of application logic.

The project uses **xUnit** for unit testing.

Run tests using:

```bash
dotnet test
```

### Integration Tests

Integration tests check how multiple parts of the application work together, such as API, services, and database-related functionality.

---

## 🔎 SonarQube & Code Quality

**SonarQube Cloud** is used to analyze the codebase and identify potential code quality and security issues.

It can help detect:

* Code smells
* Bugs
* Security issues
* Duplicated code
* Maintainability problems

This allows code quality to be checked automatically as part of the development process.

---

## ⚙️ GitHub Actions & CI

**GitHub Actions** is used for Continuous Integration (CI).

When code is pushed or a Pull Request is created, the workflow can automatically run project checks such as:

```text
Code Push / Pull Request
        ↓
GitHub Actions
        ↓
Build
        ↓
Tests / Code Analysis
        ↓
Result
```

This helps catch problems before changes are merged into the main development branch.

<img width="1920" height="775" alt="Screenshot 2026-09-24 130957" src="https://github.com/user-attachments/assets/153ebe77-7bdd-4b15-b480-3eab3c55b4e2" />



---

## 🔄 Frontend–Backend Communication

The React frontend communicates with the ASP.NET Core backend through **REST APIs**.

The basic flow is:

```text
React Frontend
      ↓
HTTP Request
      ↓
ASP.NET Core API
      ↓
Application Services
      ↓
EF Core
      ↓
SQL Server
```

For example, when a user creates a task:

```text
React Form
    ↓
POST /api/Task
    ↓
Task Controller
    ↓
Application/Service Logic
    ↓
EF Core
    ↓
SQL Server
```

The backend then returns a response to the React frontend, which updates the user interface.























