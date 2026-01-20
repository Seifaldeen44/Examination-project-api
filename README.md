# Examination Project API

A backend API to automate the full examination lifecycle for academic courses. Built with ASP.NET Core Web API, SQL Server, Entity Framework Core, and ADO.NET. The system uses SQL Server stored procedures for exam generation, answer persistence, and grading to ensure performance and transactional integrity.

---

## Quick overview

- Purpose: Automate exam creation, delivery, submission, and grading.
- Roles: Student and Instructor (role-based JWT authorization).
- Core idea: Keep controllers thin, use services for orchestration, and execute heavy DB work inside stored procedures.
- Key features:
  - Automatic, per-student exam generation
  - Secure submission handling
  - Automatic grading for objective items
  - Centralized SQL Server stored procedures for deterministic behavior

---

## Technology stack

- ASP.NET Core Web API (C#)
- SQL Server (database + stored procedures)
- Entity Framework Core (domain models & migrations)
- ADO.NET (high-performance stored-procedure calls)
- JSON Web Tokens (JWT) for authentication
- Optional: AutoMapper, Xunit/NUnit for tests

---

## Repository layout (conceptual)

- Controllers/ — thin API controllers
- Services/ — business orchestration, calls stored procedures
- DTOs/ — request/response models for API surface
- Data/ — EF Core models, migrations, SQL script folder (stored procedures)
- Scripts/ — SQL for schema, seed data, stored procedures

---

## High-level workflow

1. Instructor configures and publishes an exam (question pools, weights, time limit).
2. Student requests a generated exam instance — API calls `sp_GenerateExamForStudent`.
3. Stored procedure constructs an ExamAttempt with randomized questions and returns the instance.
4. Student completes the exam and submits answers — API persists answers and calls `sp_SaveSubmission`.
5. API calls `sp_GradeExamAttempt` (or the stored procedure grades as part of the save) to compute scores and per-question breakdown.
6. Students and instructors query results and statistics.

---

## Stored procedures (important)
The core DB logic lives in stored procedures for consistency and performance:
- sp_GenerateExamForStudent(courseId, studentId, options)
- sp_SaveSubmission(examAttemptId, answersJson)
- sp_GradeExamAttempt(examAttemptId)

Make sure these are present and deployed to the target SQL Server before using generation or grading flows.

---

## Authentication

- Endpoint: POST /api/auth/login
- Uses JWT. Token contains role claim (`Student` or `Instructor`).
- All protected endpoints check the token and role claim.

Cell: Example authentication request and response
```http
POST /api/auth/login
Content-Type: application/json
```

```json
// Request
{
  "username": "student01",
  "password": "P@ssw0rd!"
}
```

```json
// Response
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "role": "Student",
  "userId": 123
}
```

---

## Representative API endpoints

Student endpoints
- GET  /api/students/{studentId}/courses
- GET  /api/students/{studentId}/exams/available
- POST /api/exams/generate
- GET  /api/exams/{examId}
- POST /api/exams/{examId}/submit
- GET  /api/students/{studentId}/grades
- GET  /api/exams/{examId}/grade

Instructor endpoints
- POST /api/instructors/{instructorId}/exams/configure
- GET  /api/instructors/{instructorId}/exams
- PUT  /api/instructors/{instructorId}/exams/{examId}/publish
- POST /api/instructors/{instructorId}/questions
- GET  /api/instructors/{instructorId}/exams/{examId}/results

Cell: Generate exam request (student)
```http
POST /api/exams/generate
Authorization: Bearer {token}
Content-Type: application/json
```

```json
{
  "courseId": 7,
  "studentId": 123,
  "options": {
    "timeLimitMinutes": 90,
    "shuffleQuestions": true,
    "shuffleChoices": true
  }
}
```

Cell: Generate exam response (server returns instance)
```json
{
  "examId": 1001,
  "courseId": 7,
  "studentId": 123,
  "timeLimitMinutes": 90,
  "startedAt": "2026-01-20T10:00:00Z",
  "questions": [
    {
      "questionId": 501,
      "sequence": 1,
      "text": "Which of the following is a value type in C#?",
      "choices": [
        {"choiceId": 1, "text": "int"},
        {"choiceId": 2, "text": "string"},
        {"choiceId": 3, "text": "object"},
        {"choiceId": 4, "text": "dynamic"}
      ],
      "type": "SingleChoice"
    },
    {
      "questionId": 502,
      "sequence": 2,
      "text": "Select all that apply: Which database operations are ACID-compliant?",
      "choices": [
        {"choiceId": 5, "text": "Transactions"},
        {"choiceId": 6, "text": "Indexes"},
        {"choiceId": 7, "text": "Stored Procedures"},
        {"choiceId": 8, "text": "Views"}
      ],
      "type": "MultipleChoice"
    }
  ]
}
```

Cell: Submit answers request
```http
POST /api/exams/1001/submit
Authorization: Bearer {token}
Content-Type: application/json
```

```json
{
  "examId": 1001,
  "studentId": 123,
  "submittedAt": "2026-01-20T11:15:00Z",
  "answers": [
    {
      "questionId": 501,
      "selectedChoiceIds": [1]
    },
    {
      "questionId": 502,
      "selectedChoiceIds": [5, 7]
    }
  ]
}
```

Cell: Submit response (acknowledgement + grading)
```json
{
  "examAttemptId": 2003,
  "status": "Submitted",
  "gradingStatus": "Completed",
  "grade": {
    "score": 92.5,
    "maxScore": 100,
    "details": [
      {"questionId": 501, "score": 50, "maxScore": 50},
      {"questionId": 502, "score": 42.5, "maxScore": 50}
    ]
  }
}
```

---

## Instructor: configure an exam (example)

Cell: Configure exam request
```http
POST /api/instructors/42/exams/configure
Authorization: Bearer {token}
Content-Type: application/json
```

```json
{
  "courseId": 7,
  "title": "Midterm - Data Structures",
  "timeLimitMinutes": 90,
  "questionPools": [
    {"poolId": 10, "count": 10},
    {"poolId": 11, "count": 5}
  ],
  "weights": {
    "SingleChoice": 1.0,
    "MultipleChoice": 1.5,
    "ShortAnswer": 2.0
  },
  "randomize": true,
  "publish": true
}
```

Cell: Configure response
```json
{
  "examId": 1001,
  "status": "Configured",
  "publishStatus": "Published"
}
```

---

## How to run (developer quick-start)

Prerequisites:
- .NET SDK (recommended LTS — e.g., .NET 6 or later)
- SQL Server instance
- Optional: Visual Studio or VS Code

Cell: Install dotnet-ef (if needed)
```bash
dotnet tool install --global dotnet-ef
```

Cell: Restore, build, run
```bash
cd examProj
dotnet restore
dotnet build
dotnet run
```

Default local URLs:
- https://localhost:5001
- http://localhost:5000

Environment variables commonly used:
- ASPNETCORE_ENVIRONMENT (Development / Production)
- ConnectionStrings__DefaultConnection
- Jwt__Key, Jwt__Issuer, Jwt__Audience

Database setup:
1. Create database (e.g., ExaminationDB).
2. Run schema and stored-procedure SQL scripts located in the repository (Scripts/ or Data/).
3. Alternatively, update connection string and run EF migrations:
   Cell: Apply EF migrations
   ```bash
   dotnet ef database update
   ```

Important: Ensure stored procedures (sp_GenerateExamForStudent, sp_SaveSubmission, sp_GradeExamAttempt) are deployed before using generation/grading flows.

---

## Design decisions (short)

- Stored procedures centralize heavy, multi-row operations for performance, atomicity, and auditability.
- DTOs decouple the external API contract from internal models.
- Thin controllers + service layer improve testability and maintainability.

---

## Testing

Run unit and integration tests (if included):
Cell: Run tests
```bash
dotnet test
```

---

## Notes & tips

- Keep sensitive keys (JWT signing key, DB credentials) out of source control — use environment variables or secret stores.
- Tests should mock stored-procedure calls or use a test database with scripted stored procedures for deterministic results.
- For high-concurrency scenarios, tune SQL Server settings and review stored-procedure transaction scopes.

---

## Author

Seif Aldeen Hany Shaaban Awad

---
