# Examination Project API

The Examination Project API is a backend system developed using ASP.NET Core Web API to automate the examination process.
It handles exam generation, answer submission, and automatic correction using SQL Server and stored procedures.

---

## Quick overview

- Purpose: Automate exam generation and grading
- Reduce manual correction effort
- Ensure fair and consistent exams
- Use database-level logic for performance
- Provide a clean and scalable backend design

---

## Technologies used

- ASP.NET Core Web API
- C#
- SQL Server
- Entity Framework Core
- ADO.NET
- SQL Stored Procedures
- DTO Pattern

---

## System roles

### Student
- Login to the system
- View enrolled courses
- Generate exams
- Submit answers
- View exam grades

### Instructor
- Login to the system
- Configure exam structure
- Control number of questions per course

---

## Project structure (conceptual)

examProj
│
├── Controllers
│   ├── LoginController
│   ├── StudentController
│   ├── InstructorController
│   ├── ExamController
│   ├── ExamSubEP
│   └── ExamCorrectionEP
│
├── Dto
│   └── Data Transfer Objects
│
├── Models
│   └── Database Entities
│
├── Data
│   └── DbContext & Configuration
│
├── Program.cs
└── examProj.csproj

---

## Authentication

Login check verifies user role and id and returns user data.

Cell: Login check (request)
```http
POST /api/login/check
Content-Type: application/json
```

```json
{
  "role": "student",
  "id": 5
}
```

Cell: Login check (response)
```json
{
  "role": "student",
  "page": "students",
  "id": 5,
  "name": "Seif"
}
```

---

## Student APIs

### Get student courses
Returns courses the student is enrolled in.

Cell: Get student courses (request)
```http
GET /api/student/{studentId}/courses
```

Cell: Get student courses (response)
```json
[
  {
    "crs_ID": 2,
    "crs_Name": "Databases"
  }
]
```

---

### Generate exam
Generates an exam based on course name.

Cell: Generate exam (request)
```http
POST /api/Exam/generate?courseName=Databases
```

Cell: Generate exam (response)
```json
[
  {
    "questionId": 1,
    "questionText": "What is SQL?",
    "choiceA": "Language",
    "choiceB": "Database",
    "choiceC": "OS",
    "choiceD": "Compiler"
  }
]
```

---

### Submit exam answers
Submits student answers in JSON format.

Cell: Submit exam answers (request)
```http
POST /api/ExamSubEP/submit
Content-Type: application/json
```

```json
{
  "ex_ID": 10,
  "st_ID": 5,
  "answers": [
    {
      "questionId": 1,
      "answer": "Language"
    }
  ]
}
```

Cell: Submit exam answers (response)
```json
{
  "message": "Exam answers submitted successfully."
}
```

---

### Correct exam
Automatically corrects the exam and calculates the total degree.

Cell: Correct exam (request)
```http
POST /api/ExamCorrectionEP/correct?exId=10
```

Cell: Correct exam (response)
```json
{
  "examId": 10,
  "totalDegree": 85
}
```

---

### Get student exam grades
Returns all corrected exams for a student.

Cell: Get student exam grades (request)
```http
GET /api/ExamCorrectionEP/student-grades/{studentId}
```

Cell: Get student exam grades (response)
```json
[
  {
    "ex_ID": 10,
    "courseName": "Databases",
    "studentDegree": 85,
    "totalExamDegree": 100,
    "examDate": "2024-06-01"
  }
]
```

---

## Instructor APIs

### Update exam configuration
Allows instructor to update exam settings for a course.

Cell: Update exam config (request)
```http
PUT /api/instructor/update-exam-config
Content-Type: application/json
```

```json
{
  "ins_ID": 3,
  "crs_ID": 2,
  "mcq_Num": 8,
  "tf_Num": 2
}
```

Cell: Update exam config (response)
```
Exam configuration updated successfully
```

---

## System workflow

1. Login
2. View courses
3. Generate exam
4. Submit answers
5. Automatic correction
6. View grades

---

## Design decisions

- SQL stored procedures handle:
  - Exam generation
  - Answer submission
  - Correction logic
- DTOs isolate API contracts
- Controllers act as a thin layer between API and database

---

## Key advantages

- Fully automated exam lifecycle
- High performance using database logic
- Clean and maintainable architecture
- Easy to extend and modify
- Suitable for academic use

---

## Author

Seif Aldeen Hany Shaaban Awad  
Computer Science Student – Alexandria University  
GitHub: https://github.com/Seifaldeen44