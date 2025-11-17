# ZenFlow - Health and Fitness Management System

ZenFlow is a comprehensive microservices-based application that helps users achieve their health and fitness goals.

## Architecture

The application consists of the following microservices:

1. **User Service** - User management and authentication
2. **Catalog Service** - Product and nutrition database management
3. **Nutrition Service** - Meal plans and nutrition tracking
4. **Workout Service** - Exercise programs and workout tracking
5. **Progress Service** - Progress tracking and reporting
6. **Notification Service** - Notification management
7. **Reminder Service** - Reminders and scheduled tasks
8. **API Gateway** - Single entry point for all services

## Technologies

- **Backend**: .NET 8, C#
- **Databases**: SQL Server (users), MongoDB (all other data)
- **API**: RESTful API, JWT Authentication
- **Container**: Docker, Docker Compose
- **AI**: Google Gemini API integration
- **Test**: xUnit

## Detailed Service Structure

### API Gateway
- **Directory**: `ApiGateway/`
- **Files**: 
  - `Program.cs` - Main application entry point, Ocelot configuration
  - `ocelot.json` - API routing and authentication settings
  - `Dockerfile` - Docker image configuration

### User Service
- **Directory**: `UserService/`
- **Features**: User management, authentication, JWT token operations
- **Main Files**:
  - `Controllers/UsersController.cs` - User operation endpoints
  - `Models/` - User data models
  - `Services/` - User service implementations
  - `Data/` - Database connections

### Catalog Service
- **Directory**: `CatalogService/`
- **Features**: Product catalog management, product CRUD operations
- **Main Files**:
  - `Controllers/ProductsController.cs` - Product endpoints
  - `Models/Product.cs` - Product data model
  - `Services/ProductService.cs` - Product service implementation

### Nutrition Service
- **Directory**: `NutritionService/`
- **Features**: Meal plans, nutrition tracking, personalized plans
- **Main Files**:
  - `Controllers/NutritionController.cs` - Nutrition endpoints
  - `Models/` - Nutrition data models
  - `Services/` - Meal plan services

### Workout Service
- **Directory**: `WorkoutService/`
- **Features**: Workout programs, exercises, workout sessions
- **Main Files**:
  - `Controllers/WorkoutController.cs` - Workout endpoints
  - `Models/` - Exercise and workout data models
  - `Services/` - Workout service implementation

### Progress Service
- **Directory**: `ProgressService/`
- **Features**: Progress tracking, reporting, graphical data visualization
- **Main Files**:
  - `Controllers/ProgressController.cs` - Progress endpoints
  - `Models/` - Progress data models
  - `Services/` - Progress tracking services

### Notification Service
- **Directory**: `NotificationService/`
- **Features**: Notification management, unread notifications
- **Main Files**:
  - `Controllers/NotificationController.cs` - Notification endpoints
  - `Models/` - Notification data models
  - `Services/` - Notification service implementation

### Reminder Service
- **Directory**: `ReminderService/`
- **Features**: Reminders, scheduled tasks
- **Main Files**:
  - `Controllers/ReminderController.cs` - Reminder endpoints
  - `Models/` - Reminder data models
  - `Services/` - Reminder service implementation

### Docker Configuration
- **Files**:
  - `docker-compose.yml` - Docker orchestration for all services
  - `docker-compose.override.yml` - Development environment override settings
  - Each service has its own `Dockerfile`

### Tests
- **Directory**: `ZenFlow.Tests/`
- **Files**: `UserServiceTests.cs` - Unit tests for user service

### Shared Library
- **Directory**: `Shared/`
- **Purpose**: Common DTOs and models used across services

## Getting Started

### Requirements

- Docker and Docker Compose
- .NET 8 SDK (for development)

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd ZenFlowApp
   ```

2. Start the Docker containers:
   ```bash
   docker-compose up -d
   ```

3. The application will start running on the following ports:
   - API Gateway: http://localhost:5000
   - User Service: http://localhost:5001
   - Catalog Service: http://localhost:5002
   - Nutrition Service: http://localhost:5003
   - Workout Service: http://localhost:5004
   - Progress Service: http://localhost:5005
   - Notification Service: http://localhost:5006
   - Reminder Service: http://localhost:5007

### Usage

1. First, create a user:
   ```bash
   curl -X POST http://localhost:5000/api/users/register \
     -H "Content-Type: application/json" \
     -d '{
       "email": "user@example.com",
       "password": "password123",
       "firstName": "Test",
       "lastName": "User"
     }'
   ```

2. Login:
   ```bash
   curl -X POST http://localhost:5000/api/users/login \
     -H "Content-Type: application/json" \
     -d '{
       "email": "user@example.com",
       "password": "password123"
     }'
   ```

3. Start using other services.

## Development

### Building the Project

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Adding a New Service

1. Create a new folder: `NewService`
2. Create the `NewService.csproj` file
3. Add the required models and services
4. Add the service to `docker-compose.yml`
5. Add the route to `ocelot.json`

## Contributing

1. Fork the repository
2. Create a new branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. See the `LICENSE` file for more information.

