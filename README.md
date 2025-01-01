# Apollo

Apollo is a web platform designed to connect esports players with teams across multiple games including CS:GO, Valorant, and League of Legends. The platform facilitates team recruitment and player discovery in the competitive gaming space.

## Features

- **Player Management**
  - Player registration and authentication
  - Profile creation and management
  - Email verification system
  - Account status management (active/frozen)

- **Team Management**
  - Team registration and authentication
  - Team profile management
  - Email verification system
  - Account management capabilities

- **Game Support**
  - CS:GO
  - Valorant
  - League of Legends

## Technology Stack

- **Backend**
  - ASP.NET Core 5.0
  - Entity Framework Core
  - JWT Authentication
  - RESTful API architecture

- **Frontend**
  - React.js
  - React Bootstrap
  - i18n for internationalization
  - SCSS for styling
  - React Awesome Slider for animations

## Getting Started

### Prerequisites

- .NET 5.0 SDK
- Node.js and npm
- SQL Server (or your preferred database)

### Installation

1. Clone the repository
```bash
git clone [repository-url]
```

2. Navigate to the project directory
```bash
cd apollo
```

3. Install backend dependencies
```bash
dotnet restore
```

4. Navigate to the ClientApp directory and install frontend dependencies
```bash
cd ClientApp
npm install
```

5. Return to the root directory and start the application
```bash
cd ..
dotnet run
```

The application will start running at `https://localhost:5001` by default.

## Project Structure

- `/Controllers` - API endpoints for players, teams, and general functionality
- `/ClientApp` - React frontend application
- `/Services` - Business logic layer
- `/Data` - Data access layer and database context
- `/Entities` - Data models
- `/Pages` - Email templates and other razor pages

## License

This project is licensed under the Apache License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.