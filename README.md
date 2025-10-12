# Book Management System - Project Structure

## 📁 Solution Structure

```
BookManagementSystem/
│
├── src/
│   ├── BookManagement.Domain/             								
│   │   ├── Entities/														
│   │   │   ├── User.cs														
│   │   │   ├── Book.cs														
│   │   │   ├── BorrowRecord.cs												
│   │   │   └── Reservation.cs												
│   │   ├── Enums/															
│   │   │   ├── BookStatus.cs												
│   │   │   ├── BorrowStatus.cs												
│   │   │   └── UserRole.cs													
│   │   └── Common/															
│   │       └── BaseEntity.cs												
│   │
│   ├── BookManagement.Application/         								
│   │   ├── DTOs/															
│   │   │   ├── Common/														
│   │   │   │   ├── ApiResponse.cs											
│   │   │   │   ├── PagedResult.cs											
│   │   │   ├── Admin														
│   │   │   │   ├── DashboardStatsDto.cs									
│   │   │   ├── Auth/														
│   │   │   │   ├── RegisterDto.cs											
│   │   │   │   ├── LoginDto.cs											
│   │   │   │   ├── RefreshTokenDto.cs										
│   │   │   │   ├── UserDto.cs													
│   │   │   │   └── TokenResponseDto.cs									
│   │   │   ├── Books/														
│   │   │   │   ├── BookDto.cs												
│   │   │   │   ├── CreateBookDto.cs										
│   │   │   │   ├── UpdateBookDto.cs										
│   │   │   │   └── BookStatusDto.cs										
│   │   │   └── Borrowing/													
│   │   │       ├── BorrowRequestDto.cs									
│   │   │       ├── BorrowRecordDto.cs										
│   │   │       ├── CurrentBorrowerDto.cs									
│   │   │       └── BorrowHistoryDto.cs									
│   │   ├── Interfaces/														
│   │   │   ├── IAuthService.cs												
│   │   │   ├── IBookService.cs												
│   │   │   ├── IBorrowingService.cs										
│   │   │   └── ITokenService.cs											
│   │   ├── Services/														
│   │   │   ├── AuthService.cs												
│   │   │   ├── BookService.cs												
│   │   │   ├── BorrowingService.cs										
│   │   │   └── TokenService.cs												
│   │   ├── Validators/														
│   │   │   ├── RegisterDtoValidator.cs									
│   │   │   ├── LoginDtoValidator.cs										
│   │   │   ├── CreateBookDtoValidator.cs									
│   │   │   └── BorrowRequestValidator.cs									
│   │   ├── Exceptions/														
│   │   │   ├── BookNotAvailableException.cs								
│   │   │   ├── MaxBorrowLimitException.cs								
│   │   │   ├── NotFoundException.cs										
│   │   │   ├── InvalidCredentialsException.cs							
│   │   │   └── UserAlreadyExistsException.cs								
│   │   └── Mappings/														
│   │       └── MappingProfile.cs											
│   │
│   ├── BookManagement.Infrastructure/      								
│   │   ├── Data/															
│   │   │   └── ApplicationDbContext.cs									 
│   │   ├── Repositories/													
│   │   │   ├── Interfaces/													
│   │   │   │   ├── IRepository.cs											
│   │   │   │   ├── IBookRepository.cs										
│   │   │   │   ├── IUserRepository.cs										
│   │   │   │   └── IBorrowRecordRepository.cs							
│   │   │   └── Implementations/											
│   │   │       ├── Repository.cs											
│   │   │       ├── BookRepository.cs										
│   │   │       ├── UserRepository.cs										
│   │   │       └── BorrowRecordRepository.cs								
│   │   └── Configurations/													
│   │       ├── UserConfiguration.cs										
│   │       ├── BookConfiguration.cs										
│   │       ├── BorrowRecordConfiguration.cs								
│   │       └── ReservationConfiguration.cs								
│   └── BookManagement.API/                 								
│       ├── Controllers/														
│       │   ├── AuthController.cs											
│       │   ├── BooksController.cs											
│       │   ├── BorrowingController.cs										
│       │   └── AdminController.cs											
│       ├── Middleware/														
│       │   ├── ExceptionHandlingMiddleware.cs							
│       │   └── RequestLoggingMiddleware.cs
│       ├── Filters/															
│       │   └── ValidationFilter.cs										
│       ├── Extensions/														
│       │   └── ServiceExtensions.cs										
│       ├── appsettings.json												
│       ├── appsettings.Development.json									
│       └── Program.cs														
│
└── tests/
    ├── BookManagement.UnitTests/
    │   └── Services/
    │       ├── AuthServiceTests.cs
    │       ├── BookServiceTests.cs
    │       ├── BorrowingServiceTests.cs
    │       └── TokenServiceTests.cs
    │   
    └── BookManagement.IntegrationTests/
        └── Controllers/
	        ├── AdminControllerTests.cs
            ├── AuthControllerTests.cs
	        ├── BorrowingControllerTests.cs
            └── BooksControllerTests.cs
```

## 📦 NuGet Packages

### BookManagement.Domain
```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
```

### BookManagement.Application
```xml
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="FluentValidation" Version="11.9.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
```

### BookManagement.Infrastructure
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
```

### BookManagement.API
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
```

### BookManagement.UnitTests
```xml
<PackageReference Include="xUnit" Version="2.6.2" />
<PackageReference Include="Moq" Version="4.20.69" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
```

## 🔧 appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BookManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJWTTokenGeneration123!",
    "Issuer": "BookManagementAPI",
    "Audience": "BookManagementClient",
    "ExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 7
  },
  "BorrowingSettings": {
    "MaxBooksPerUser": 3,
    "BorrowingPeriodDays": 14,
    "MaxRenewals": 2,
    "LateFeePerDay": 1.5
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
```

## 🗄️ Database Migrations Commands

```bash
# Infrastructure loyihasiga o'tish
cd src/BookManagement.Infrastructure

# Initial Migration
dotnet ef migrations add InitialCreate --startup-project ../BookManagement.API --context ApplicationDbContext

# Database Update
dotnet ef database update --startup-project ../BookManagement.API

# Migration Remove (agar xato bo'lsa)
dotnet ef migrations remove --startup-project ../BookManagement.API
```

## 🚀 API Endpoints Structure

### Authentication
- `POST /api/auth/register` - Ro'yxatdan o'tish
- `POST /api/auth/login` - Tizimga kirish
- `POST /api/auth/refresh-token` - Token yangilash
- `GET /api/auth/me` - Current user info

### Books (User va Admin)
- `GET /api/books` - Barcha kitoblar (pagination, filter)
- `GET /api/books/{id}` - Bitta kitob ma'lumotlari
- `GET /api/books/{id}/status` - Kitob holati va tarixi
- `POST /api/books` - Kitob qo'shish [Admin]
- `PUT /api/books/{id}` - Kitobni yangilash [Admin]
- `DELETE /api/books/{id}` - Kitobni o'chirish [Admin]

### Borrowing (User)
- `POST /api/borrowing/checkout/{bookId}` - Kitobni olish
- `POST /api/borrowing/return/{borrowRecordId}` - Kitobni qaytarish
- `POST /api/borrowing/renew/{borrowRecordId}` - Muddatni uzaytirish
- `GET /api/borrowing/my-books` - Mening kitoblarim
- `GET /api/borrowing/history` - Mening tarixim

### Admin
- `GET /api/admin/dashboard` - Dashboard statistics
- `GET /api/admin/borrows/active` - Aktiv borrowlar
- `GET /api/admin/borrows/overdue` - Kechikkan kitoblar
- `GET /api/admin/users` - Foydalanuvchilar ro'yxati

## 🎨 Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Error 1", "Error 2"]
}
```

### Pagination Response
```json
{
  "success": true,
  "data": {
    "items": [...],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 5,
    "totalCount": 47
  }
}
```

## 🔐 JWT Token Structure

```json
{
  "sub": "user_id",
  "phoneNumber": "+998901234567",
  "role": "User",
  "jti": "unique_token_id",
  "exp": 1234567890,
  "iss": "BookManagementAPI",
  "aud": "BookManagementClient"
}
```

## 📝 Development Steps

1. **Domain Layer** - Entities va Enums yaratish
2. **Infrastructure Layer** - DbContext va Repositories
3. **Application Layer** - DTOs, Services, Validators
4. **API Layer** - Controllers va Middleware
5. **Testing** - Unit va Integration tests
6. **Documentation** - Swagger configuration
7. **Deployment** - Production settings

## 🎯 Key Features

✅ Phone number authentication (UZB format: +998XXXXXXXXX)
✅ Role-based authorization (Admin, User)
✅ JWT authentication with refresh tokens
✅ Book CRUD operations
✅ Borrowing system with validation
✅ Book status tracking with history
✅ Reservation system
✅ Late fee calculation
✅ Global exception handling
✅ Request/Response logging
✅ FluentValidation
✅ AutoMapper
✅ Swagger documentation
✅ Unit tests
