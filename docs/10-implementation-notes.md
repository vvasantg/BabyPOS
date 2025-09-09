## Implementation Notes
- **Concurrency Control**: Database and API will be designed to support optimistic locking or row versioning for order management, to prevent double-ordering and support future scaling.
- **Multi-language UI**: The UI will support both Thai and English from the beginning, using resource files for easy extension.
- **Config Separation**: All database and storage connection settings will be separated into config files to allow easy changes and future upgrades.
- **Authentication**: JWT will be used for authentication, and endpoints for social login (Google/Facebook) will be prepared for future implementation.
- **EF Core & PostgreSQL**: Entity Framework Core will be used for ORM with PostgreSQL for ease of development and maintainability.
