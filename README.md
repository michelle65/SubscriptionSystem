# Subscription System

A subscription management system with admin and user roles, invitation-based registration, and fiscal code management.

## Technologies Used

### Frontend
- React 18
- Bootstrap 5
- Axios

### Backend
- .NET 8
- Entity Framework Core
- SQL Server Express
- JWT Authentication
- MailerSend (Email Service)

## Quick Start

### Frontend
```bash
cd subscription-system-frontend
npm install
npm start
```

### Backend
```bash
cd SubscriptionSystemBackend/SubscriptionSystemBackend
dotnet build

dotnet ef database update

dotnet run
```


The application will be available at:
- Frontend: http://localhost:3000
- Backend API: http://localhost:5001
- Swagger UI: http://localhost:5001

## Database Setup

### Prerequisites
- SQL Server Express (or SQL Server)
- Entity Framework Core tools installed globally:
```bash
dotnet tool install --global dotnet-ef
```

### Database Commands

**Update database with existing migrations:**
```bash
cd SubscriptionSystemBackend/SubscriptionSystemBackend
dotnet ef database update
```

**Remove the last migration (if not applied to database):**
```bash
dotnet ef migrations remove
```

**Reset database (drop and recreate):**
```bash
dotnet ef database update
```

### Database Connection
The application uses SQL Server Express with the following connection string:
- Server: `.\SQLEXPRESS`
- Database: `SubscriptionSystemDb`