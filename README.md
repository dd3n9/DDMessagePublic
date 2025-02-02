# DDMessage

DDMessage is a microservices-based application designed for scheduling and sending email notifications to a large number of users simultaneously. It efficiently handles high email volumes by leveraging RabbitMQ for asynchronous messaging and gRPC for inter-service communication.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)

---

## Overview

DDMessage provides a robust solution for sending email notifications in bulk. The application is divided into several microservices, each handling a specific part of the process. This modular design improves scalability and maintainability while ensuring reliable and efficient email dispatch.

---

## Architecture

- **Microservices Architecture:**  
  The project is split into independent microservices:
  - **MessageService:** Manages the creation and dispatch of email messages.
  - **UserService:** Handles user data and recipient management.
  - **SchedulerService:** Schedules the sending of emails.
  - **NotificationService:** Manages notifications related to email dispatch.
  - **Gateway (Yarp):** Acts as an API gateway for routing requests to the appropriate services and handles user authentication.
  
- **Bulk Email Dispatch:**  
  Efficiently schedules and sends emails to a large number of recipients simultaneously.

- **Asynchronous Communication:**  
  Utilizes RabbitMQ for reliable message queuing and processing.

- **Fast Data Transfer:**  
  Employs gRPC for efficient communication between microservices.

- **Scalability & Orchestration:**  
  Uses Kubernetes for container orchestration and Nginx for load balancing and reverse proxying.

---

## Tech Stack

### Backend
- **Language:** C#
- **Framework:** .NET, ASP.NET Core
- **Communication:** gRPC, RabbitMQ
- **Database:** SQL/MSSQL (for storing email data and scheduling information)
- **Architecture:** Microservices, CQRS, Clean Architecture
- **Additional Libraries:** EntityFramework, MediatR, JWT

### Services
- **MessageService:** Handles email message creation and dispatch.
- **UserService:** Manages user-related data.
- **SchedulerService:** Schedules email sending tasks.
- **NotificationService:** Manages notifications for email events.
- **Gateway (Yarp):** Routes external requests to the appropriate microservices and manages user authentication.

### Infrastructure
- **Nginx:** Used for load balancing.
- **Kubernetes:** Orchestrates containerized services and manages scaling.
