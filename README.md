# 🌟 eShop Microservices Architecture

Welcome to the **eShop Microservices Architecture**! This robust e-commerce system is built on **.NET 8** and designed for scalability, modularity, and seamless integration with modern technologies. The architecture comprises independent services communicating via **RESTful APIs** and **Kafka** for asynchronous messaging, ensuring a responsive and efficient shopping experience.

This README provides a comprehensive overview of the system architecture, its components, and their interactions, enabling core e-commerce functionalities such as managing product catalogs and shopping carts.

---

## 🏗️ System Architecture

The architecture is grounded in the following principles:

- **Scalability**: Services operate independently, allowing for horizontal scaling of individual components.
- **Resiliency**: Integration with Kafka ensures seamless communication through asynchronous message processing.
- **Extensibility**: Each service adheres to clean architecture principles and supports modern technologies, including JWT authentication, API versioning, and logging.
- **Separation of Concerns**: Each service is dedicated to a specific domain within the e-commerce ecosystem, enhancing modularity and maintainability.

### 📊 High-Level Diagram
![alt text](https://github.com/Eugene851001/eshop/blob/main/eshop.drawio.png)

### 🛠️ Components Overview

1. **Catalog Service**
2. **Cart Service**
3. **Authentication Service**
4. **Infrastructure Services**
   - Message Broker (Kafka)
   - Databases
     - Catalog Database (SQL Server)
     - Cart Database (MongoDB)

---

## 🛠️ Services

### 1. Catalog Service

**Description:**  
The Catalog Service manages product catalogs, enabling CRUD operations for products and providing endpoints for querying and searching the catalog. It integrates with Kafka to emit events when catalog data changes.

**Features:**
- REST API with JWT authentication.
- API versioning and Swagger UI for documentation.
- Clean layered architecture (BLL, DAL).
- Kafka producer for forwarding events (e.g., product updates).
- SQL Server for persistence.

**Technology Stack:**
- .NET 8
- Entity Framework Core
- Kafka integration
- JWT Authentication
- Swagger for API documentation

**Key Files/Projects:**
- `eShop.CatalogService.API`: Exposes catalog endpoints.
- `eShop.CatalogService.BLL`: Business logic layer.
- `eShop.CatalogService.DAL`: Data access layer.

---

### 2. Cart Service

**Description:**  
The Cart Service handles shopping cart operations such as adding, updating, or removing items from a user’s cart. It integrates with Kafka for consuming messages (e.g., sync with inventory) and MongoDB for persistence.

**Features:**
- OpenAPI/Swagger for documentation.
- JWT authentication for secure access.
- Kafka consumer to process background messages (e.g., inventory updates).
- MongoDB storage for scalability.
- API versioning with support for v1 and v2.

**Technology Stack:**
- .NET 8
- MongoDB and Kafka
- AutoMapper and MediatR for clean architecture.

**Key Files/Projects:**
- `eShop.CartService.API`: Provides RESTful cart CRUD APIs.
- `eShop.CartService.Console`: Background worker for Kafka message processing.
- `eShop.CartService.ConfluentKafka`: Handles Kafka integration.

---

### 3. Authentication Service

- Implemented with **KeyCloak**.
- Manages user authentication across all microservices using JWT tokens.
- Centralized identity and access management.

---

### 🏗️ Infrastructure Services

#### Message Broker (Kafka)
- Kafka enables decoupled communication between services.
- Producers and consumers are utilized for event-driven features like updating carts, syncing inventory, or real-time notifications.

#### Databases:
- **Catalog Database**: SQL Server for relational catalog data.
- **Cart Database**: MongoDB for flexible and scalable cart data storage.

---

## 🔄 Interactions Between Services

- **Catalog Service ↔️ Cart Service**:  
  When a product is updated in the catalog, an event is sent via Kafka to update relevant cart items.

- **Cart Service ↔️ Delivery Service**:  
  The cart service emits messages (e.g., order placed) that the delivery service consumes to initiate shipment workflows.

- **Authentication**:  
  Used by all services to handle JWT-based authentication and authorization.

---

## 🛠️ Prerequisites to Run the System

### Required Tools:
- [Docker and Docker Compose](https://www.docker.com/)
- [Kafka](https://kafka.apache.org/)
- [MongoDB](https://www.mongodb.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

---
