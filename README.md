# 📁 PortfolioHub

A professional full-stack personal portfolio project that showcases your work, skills, education, certifications, and blog posts — all hosted in a modern, secure, and containerized architecture.

## 🧠 Overview

PortfolioHub is a personal portfolio project designed and developed using a robust and professional architecture. It showcases my experience across full-stack development, containerization, security, and observability.

### 🔧 Architecture Highlights

```
[Client Browser]
    |
    | HTTPS
    v
[NGINX Reverse Proxy]
    | - Handles SSL termination (HTTPS)
    | - Redirects HTTP → HTTPS
    | - Forwards requests to backend
    | - Streams access logs to Fluent Bit
    v
[Backend ASP.NET Core (Modular Monolith)]
    | - Built using .NET 8
    | - Modular structure: Projects, Contact, Education, etc.
    | - Clean Architecture + SOLID Principles
    | - Logs to console (captured via Fluent Bit)
    |
    +--> [Email Service]
    |    - Sends user-submitted contact messages
    |
    +--> [Database]
         - Stores user data: projects, education, skills, and more

[Fluent Bit]
    | - Parses and forwards logs from NGINX and backend
    v
[Seq]
    - Centralized log viewer
    - Real-time debugging and filtering for API traffic
```

### 🔒 Security & Best Practices

- Enforced HTTPS across all services using self-signed certificates (locally)
- Reverse proxy secured with secret headers for internal APIs
- Docker networks isolate containers with internal-only access
- SSL setup with auto-forwarding HTTP → HTTPS

### 📦 DevOps & Deployment

- Multi-stage Dockerfile for .NET backend
- Docker Compose setup with multiple profiles (dev / prod)
- Health checks for logging services
- Environment-based configuration via `.env` files
