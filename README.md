# TaskProximity Issue Tracker - README

TaskProximity Issue Tracker is an open-source web application designed to help teams manage their projects and track issues effectively. It provides features for user registration and authentication, project management, commenting, collaboration, notifications, and team membership.

## Features

- **User Registration and Authentication:**
  - Users can sign up and log in securely to the application using their email and password.
  - Optionally, users can log in using their Google accounts for faster access.

- **Project Management:**
  - Users can create, view, update, and delete projects.
  - Projects include details like name, description, created by, status, and assigned team members.

- **Commenting and Collaboration:**
  - Users can comment on projects to discuss issues, progress, and updates.
  - Team members can collaborate within projects to foster efficient communication.

- **Notification System:**
  - Users receive in-app notifications for project updates and new comments.
  - Users also receive email notifications for project invitations and updates.

- **Teams and Team Membership:**
  - Users can create teams and invite others to collaborate.
  - Each team has a manager who can invite and remove members.
  - Team members can have different roles like "Manager" or "Member".

## Backend Implementation

The backend of TaskProximity Issue Tracker is built using C# .NET Core. It follows best practices for creating web APIs, data management using Entity Framework Core, and user authentication with JWT tokens.

### Project Structure

- **Controllers:** Contains API controllers for handling user, project, comment, and team-related operations.
- **Models:** Defines the data models for users, projects, comments, teams, and team membership.
- **Services:** Includes service classes to handle business logic for authentication, notifications, and team management.
- **Data:** Contains the database context class and migration files for managing the database schema.

### Database

The application uses Entity Framework Core to interact with the database. The database schema includes tables for users, projects, comments, teams, team membership, and invitations.

### Authentication and Authorization

User authentication is implemented using JWT (JSON Web Tokens) for secure access to API endpoints. The application uses a secret key stored in the environment variables for token generation and validation.

### Notifications

Notifications are sent to users in-app and via email when they are assigned to a project, receive a comment, or are invited to a team.

## Next Steps

The current state of the project focuses on backend development, including user registration, authentication, project management, commenting, team creation, and team membership.

The next steps involve frontend development to create the user interface and integrate it with the backend APIs. Additionally, frontend implementation will include user interfaces for projects, comments, teams, and user management.

