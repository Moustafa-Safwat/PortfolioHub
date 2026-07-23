# Application User Roles and Permissions

This document defines the roles available in the application and describes the responsibilities and permissions associated with each role.

## 1. User

The **User** role represents a standard authenticated user of the application.

### Permissions

- Access the application.
- View available blog posts.
- Create comments on blog posts.
- Manage their own account information, where applicable.

### Restrictions

- Cannot create, edit, or delete blog posts.
- Cannot manage other users.
- Cannot access administrative features.

---

## 2. Contributor

The **Contributor** role represents a user who can manage blog content that they own.

### Permissions

- Access the application.
- View blog posts.
- Create comments on blog posts.
- Create new blog posts.
- Edit blog posts they created.
- Delete blog posts they created.

### Restrictions

- Cannot edit or delete blog posts created by other contributors unless explicitly granted permission.
- Cannot manage users or application-wide settings.
- Cannot access administrative features.

> The Contributor role is primarily intended for permissions related to blog management.

---

## 3. Admin

The **Admin** role represents a trusted person with full access to the application.

### Permissions

- Access all application features.
- Create, edit, and delete any blog post.
- Manage comments.
- Manage users and their assigned roles.
- Access administrative pages and settings.
- Perform any action available within the application.

### Restrictions

- Administrative permissions should only be assigned to trusted users.
- Sensitive actions should be logged for auditing and security purposes.

---

## 4. System

The **System** role is not assigned to a human user. It represents actions performed automatically by the application or its background services.

### Examples

- Running scheduled or background tasks.
- Sending automated emails or notifications.
- Processing queued jobs.
- Creating or updating records through automation.
- Performing maintenance and cleanup operations.
- Executing system-generated workflows.

### Restrictions

- The System role must not be used for interactive human login.
- Its permissions should be limited to the operations required by automated processes.
- System actions should be logged and identifiable as non-human actions.

---

## Role Permission Summary

| Permission | User | Contributor | Admin | System |
|---|:---:|:---:|:---:|:---:|
| Access the application | Yes | Yes | Yes | No interactive access |
| View blog posts | Yes | Yes | Yes | As required |
| Create comments | Yes | Yes | Yes | As required |
| Create blog posts | No | Yes | Yes | As required |
| Edit own blog posts | No | Yes | Yes | As required |
| Delete own blog posts | No | Yes | Yes | As required |
| Edit or delete any blog post | No | No | Yes | As required |
| Manage comments | No | No | Yes | As required |
| Manage users and roles | No | No | Yes | No |
| Manage application settings | No | No | Yes | As required |
| Run automated tasks | No | No | No | Yes |

## Recommended Role Names

The following role names are recommended for use in the application code and database:

```text
User
Contributor
Admin
System
```

Role names should remain consistent across authentication tokens, authorization policies, database records, and application code.
