
[![Build Status](https://dev.azure.com/mabrahamjohn/pms%20-Devops%20Agile/_apis/build/status%2Fmelvinabjohn.pms_software_security?branchName=main)](https://dev.azure.com/mabrahamjohn/pms%20-Devops%20Agile/_build/latest?definitionId=2&branchName=main)


# PMS Software Security

## Overview
The PMS Software Security project is a robust backend API solution for managing project management systems. Developed using Java Spring Boot, this project focuses on providing secure, scalable, and efficient APIs to handle various aspects of project management, including user authentication, project tasks, and team collaboration.

## Features
- **User Authentication and Authorization**: Secure login and registration using Spring Security.
- **Project Management**: Create, update, delete, and view projects.
- **Task Management**: Assign, track, and manage tasks within projects.
- **Team Collaboration**: Manage team members, assign roles, and permissions.
- **Notifications**: Real-time notifications for task updates and deadlines.
- **Reporting and Analytics**: Generate reports to track project and task progress.

## Technologies Used
- **Java**: Core programming language.
- **Spring Boot**: Framework for building the backend API.
- **Spring Security**: For authentication and authorization.
- **Spring Data JPA**: For database interactions.
- **Hibernate**: ORM for managing database entities.
- **MySQL**: Database management system.
- **Docker**: Containerization for deployment.
- **JUnit & Mockito**: Testing frameworks.
- **Swagger**: API documentation.

## Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/melvinabjohn/pms_software_security.git
   ```
2. **Navigate to the project directory**:
   ```bash
   cd pms_software_security
   ```
3. **Build the project**:
   ```bash
   mvn clean install
   ```
4. **Run the application**:
   ```bash
   mvn spring-boot:run
   ```

## Usage

### API Endpoints

- **User Management**
  - `POST /api/users/register` - Register a new user.
  - `POST /api/users/login` - Authenticate a user.

- **Project Management**
  - `GET /api/projects` - List all projects.
  - `POST /api/projects` - Create a new project.
  - `GET /api/projects/{id}` - Get project details.
  - `PUT /api/projects/{id}` - Update a project.
  - `DELETE /api/projects/{id}` - Delete a project.

- **Task Management**
  - `GET /api/tasks` - List all tasks.
  - `POST /api/tasks` - Create a new task.
  - `GET /api/tasks/{id}` - Get task details.
  - `PUT /api/tasks/{id}` - Update a task.
  - `DELETE /api/tasks/{id}` - Delete a task.

## Contributing
1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/NewFeature`).
3. Commit your changes (`git commit -m 'Add some NewFeature'`).
4. Push to the branch (`git push origin feature/NewFeature`).
5. Open a Pull Request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
**Melvin Ab John**  
[LinkedIn](https://www.linkedin.com/in/melvinjohnabraham/)  
[Email](mailto:melvinabjohn@gmail.com)
