# HeadHunter_CRUD_Api

This C# .NET 6.0 project implements CRUD api for Headhunters and Candidates interaction.

### Allowed actions:
  - Candidate:
    * Create, Update, Delete
  - Candidate's skillset:
    * Create, Delete
  - Company:
    * Create, Update, Delete
  - Open company's positions:
    * Create, Update, Delete
  - Applications:
    * Create, Delete
    
To successfully run the projects it requires to have database engine running.
> I was using MSSQL Express - connection string example can be found at DataModels/context.cs (line 22).

### To make and add migrations:
In NuGet console
```
add-migration <migration-name>
update-migration
```

### To connect to web interface (if on localhost) go to:
> http://localhost:5000/swagger/index.html
