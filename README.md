# Project Management Tool - Backend 🚀

This repository contains the backend implementation of the **Project Management Tool**, a platform designed to streamline project planning, task assignments, and collaboration. Built with **.NET Core**, this backend provides robust APIs, secure authentication, and data management features to support the frontend application or other client integrations.

## Technology Stack
- **.NET Core**
- **Entity Framework Core (EF Core)**
- **ASP.NET Core**
 - **MySQL**
---

## Features ✨

- **Project and Task Management:**  
  - Create, update, and delete projects and tasks.  
  - Assign tasks to team members with deadlines and priorities.

- **User Authentication and Authorization:**  
  - Secure user registration and login using JWT-based authentication.  
  - Role-based access control (e.g., Admin, Manager, User).

- **API Design:**  
  - RESTful APIs for seamless integration with frontend applications or external tools.  
  - Clear and consistent endpoint structure.

- **Error Handling and Validation:**  
  - Comprehensive input validation.  
  - Custom error responses for a better developer experience.

- **Database Integration:**  
  - Uses **Entity Framework Core** for data persistence.  
  - Support for SQL-based relational databases.

---

## Project Structure 🏗️

The project is organized into a multi-layered architecture for scalability and maintainability:

1. **Controllers**  
   - Handles HTTP requests and responses.  
   - Defines endpoints for user, project, and task management.

2. **Services**  
   - Contains the business logic of the application.  
   - Processes input data and interacts with repositories.

3. **Repositories**  
   - Manages database operations like CRUD (Create, Read, Update, Delete).  
   - Abstracts data persistence logic.

4. **Models**  
   - Represents entities like `User`, `Project`, and `Task`.  
   - Includes data annotations for validation.

5. **Utilities**  
   - Helper functions for tasks like JWT generation, password hashing, and logging.

---

## Key Technologies Used 🛠️

- **Backend Framework:** .NET Core  
- **Database:** SQL Server (or customizable for other relational databases)  
- **ORM:** Entity Framework Core  
- **Authentication:** JSON Web Tokens (JWT)  
- **Logging:** Integrated logging for debugging and monitoring  

---

## Installation and Setup 🔧

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)  
- A relational database (e.g., SQL Server, PostgreSQL).  

### Steps to Run Locally

1. Clone the repository:  
   ```bash
   git clone https://github.com/dogaaydinn/ProjectManagementTool.BE.git

 1.  Navigate to the project directory:

    ```bash

    `cd ProjectManagementTool.BE`

2.  Set up the database connection string in `appsettings.json`:

    json

    `{
      "ConnectionStrings": {
        "DefaultConnection": "Your-Database-Connection-String-Here"
      }
    }`

3.  Apply migrations and seed the database (if applicable):

    ```bash

    `dotnet ef database update`

4.  Build and run the application:

    ```bash
    `dotnet run`

* * * * *

API Endpoints 📡
----------------

### Authentication

-   **POST /api/auth/register**\
    Registers a new user.\
    **Request Body Example:**

    json


    `{
      "username": "john_doe",
      "email": "john.doe@example.com",
      "password": "securepassword"
    }`

-   **POST /api/auth/login**\
    Logs in a user and returns a JWT token.\
    **Request Body Example:**

    json


    `{
      "email": "john.doe@example.com",
      "password": "securepassword"
    }`

### Projects

-   **GET /api/projects**\
    Retrieves all projects for the authenticated user.

-   **POST /api/projects**\
    Creates a new project.\
    **Request Body Example:**


    -   `{
      "name": "New Project",
      "description": "Project description here."
    }`

### Tasks

-   **GET /api/tasks/{projectId}**\
    Retrieves all tasks for a specific project.

-   **POST /api/tasks**\
    Creates a new task under a project.\
    **Request Body Example:**

    json

    `{
      "projectId": 1,
      "title": "Design homepage",
      "description": "Create wireframe for the homepage.",
      "dueDate": "2024-11-30T00:00:00",
      "priority": "High"
    }`

* * * * *

Development Workflow 🛠️
------------------------

### Adding Migrations

If you make changes to the models, update the database schema by running:

    ```bash
    dotnet ef migrations add MigrationName
    dotnet ef database update`

### Debugging

-   Use integrated logging to monitor API requests and exceptions.
-   Run the application in debug mode through Visual Studio or a similar IDE.

* * * * *

Planned Features 🛠️
--------------------

-   **Notification System:**\
    Notify team members about task updates and deadlines.

-   **Reporting:**\
    Generate project and task reports for managers.

-   **Third-Party Integrations:**\
    Support tools like Slack or Microsoft Teams for real-time collaboration.

-   **Frontend Integration:**\
    Develop or connect to a modern frontend for a seamless user experience.

* * * * *

Contribution Guidelines 🤝
--------------------------

Contributions are welcome! Please follow these steps to contribute:

1.  Fork the repository.
2.  Create a feature branch:

    ```bash

    `git checkout -b feature/your-feature-name`

3.  Commit your changes:

    ```bash

    `git commit -m "Add your message here"`

4.  Push to your forked repository:

    ```bash

    `git push origin feature/your-feature-name`

5.  Open a pull request describing your changes.

* * * * *

License 📜
----------

This project is licensed under the MIT License. You are free to use, modify, and distribute this project as per the license terms.
