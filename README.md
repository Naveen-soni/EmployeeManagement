This is the backend service for the **BOOKXPERT Employee Management System**.  
Built using **ASP.NET Core Web API**, this project provides RESTful endpoints for managing employee data, retrieving states, and supporting frontend operations like chart rendering, PDF export, and report generation.

---

## ⚙️ Technology Stack

| Layer       | Technology                         |
|-------------|-------------------------------------|
| Framework   | ASP.NET Core 8.0 Web API            |
| Data Access | ADO.NET (Raw SQL, No EF)            |
| Database    | MySQL                               |
| Architecture| MVC + Layered (Controller, BLL, DAL)|
| Reporting   | RDLC / Crystal Report     |

---

## 📁 Project Structure
EmployeeManagement/
├── Controllers/
│ └── EmployeeController.cs
├── BLL/
│ └── EmployeeService.cs
├── DAL/
│ └── EmployeeRepository.cs
├── Models/
│ ├── Employee.cs
│ └── State.cs
├── appsettings.json
├── Program.cs


| Endpoint                | Method | Description                 |
|-------------------------|--------|-----------------------------|
| `/api/employee`         | GET    | Get all employees           |
| `/api/employee`         | POST   | Add new employee            |
| `/api/employee/{id}`    | PUT    | Update employee by ID       |
| `/api/employee/{id}`    | DELETE | Delete employee by ID       |
| `/api/employee/bulk`    | DELETE | Delete multiple employees   |
| `/api/state`            | GET    | Get all states (for dropdown)|

> API returns JSON responses and supports standard HTTP status codes.

---

## 🧑‍💼 Employee Table Schema

Use the following SQL to create the required tables in **MySQL**:

`Employees` Table

CREATE TABLE `employees` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Designation` varchar(100) DEFAULT NULL,
  `DateOfJoin` date NOT NULL,
  `DateOfBirth` date NOT NULL,
  `Salary` decimal(10,2) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `State` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

`states` Table

CREATE TABLE `states` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


Setup Instructions
Prerequisites
.NET 8 SDK

MySQL Server (local or hosted)

Visual Studio 2022 or VS Code

🛠 Configuration
Update your database connection in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;user=root;password=yourpassword;database=bookxpert_db;"
}

Run the Project 

Author
Naveen Soni
ASP.NET Backend Developer – BOOKXPERT Assignment


