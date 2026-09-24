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
This means Take the existing migrations/Changes and apply them to the actual database

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

```bash
Open another Terminal
cd Frontend
```

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


### 🔐 Login


### 📊 Dashboard


### 👤 Profile


### 📋 Tasks


### ➕ Create Task


### 🔎 Search Tasks





























