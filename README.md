# HumanResourcesAPI


# the below is Ms sql schema

create database Human_Resources;

Use Human_Resources;

Create Table Department (
	Department_Id INT PRIMARY KEY IDENTITY(1,1),
	Department_Name Varchar(50) Not Null ,
	Description varchar(max) Not Null,
	CONSTRAINT UQ_Department_Department_Name Unique (Department_Name)
)

Create Table Role (
	Role_Id INT PRIMARY KEY IDENTITY(1,1),
	Role_Name VARCHAR(50) Not Null,
	Role_Description varchar(max) NOT NULL,
	CONSTRAINT UQ_Role_Role_Name Unique (Role_Name)
)

Create Table Permissions (
	Permissions_Id INT PRIMARY KEY IDENTITY(1,1),
	Permission_Name VARCHAR(50) Not Null, -- user or admin
	Permission_Description varchar(max) Not Null,
	CONSTRAINT UQ_Permissions_Permission_Name Unique (Permission_Name)
)


Create Table Team_Roles (
  Team_Role_Id INT PRIMARY KEY IDENTITY(1,1),
  Role_Name varchar(50) Not Null,   -- eg ( Team Lead , Team Member etc)
  Role_Description varchar(max) Not Null,
  CONSTRAINT UQ_Team_Roles_Role_Name Unique (Role_Name)
)  


Create Table Status (
	Status_Id INT PRIMARY KEY IDENTITY(1,1),
	Status_Name VARCHAR(50) Not Null,
	CONSTRAINT UQ_Status_Status_Name Unique (Status_Name)
)

Create Table HR (
	Hr_Id INT PRIMARY KEY IDENTITY(1,1),
	Hr_First_Name VARCHAR(50) Not Null,
	Hr_Last_Name VARCHAR(50) Not Null,
	Hr_Email VARCHAR(50) UNIQUE Not Null,
	Hr_Role_Id INT NOT Null,
	CreatedBy varchar(20) not null,
	CreatedOn date  not null DEFAULT GETDATE(),
	ChangedBy varchar(20),
	ChangedOn date  ,
	DeletedBy varchar(20),
	DeletedOn date  ,
	Constraint FK_HR_Role_ID FOREIGN KEY ( Hr_Role_Id ) REFERENCES  Role  ( Role_Id ) ON DELETE NO Action,
	constraint UQ_Hr_Name unique(Hr_First_Name, Hr_Last_Name, Hr_Email)
)


Create Table Teams (
    Team_Id INT PRIMARY KEY IDENTITY(1,1),
    Team_Name varchar(50) Not Null,  
    Description varchar(max) Not Null,
    Department_Id INT Not Null,
    team_lead_employee_id INT Not Null,
    Team_Role_Id INT NOT Null,
    CreatedBy int,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date ,
	DeletedBy int ,
	DeletedOn date ,
    CONSTRAINT  FK_Teams_Department_Id FOREIGN KEY ( Department_Id ) REFERENCES  Department  ( Department_Id ) On Delete Cascade, 
    CONSTRAINT  FK_Teams_Team_Role_Id FOREIGN KEY ( Team_Role_Id ) REFERENCES  Team_Roles  ( Team_Role_Id ) On Delete Cascade, 
    CONSTRAINT UQ_Teams_Team_Name Unique (Team_Name),
    CONSTRAINT  FK_Teams_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  HR  ( Hr_Id ),
    CONSTRAINT  FK_Teams_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ),
    CONSTRAINT  FK_Teams_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id )
)

Create Table Employee(
	Employee_Id INT PRIMARY KEY IDENTITY(1,1),
	First_Name VARCHAR(50) Not Null,
	Last_Name VARCHAR(50) Not Null,
	Email VARCHAR(50) UNIQUE Not Null,
	Phone VARCHAR(10) UNIQUE Not Null,
	Date_Joined DATE Not Null,
	Status_Id INT Not Null,
	Department_Id INT Not Null,
	Role_Id INT Not Null,
	Permissions_Id INT Not Null,
	Team_Id INT Null,
	CreatedBy int,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date ,
	DeletedBy int ,
	DeletedOn date ,
	CONSTRAINT  FK_Employee_Status_Id  FOREIGN KEY ( Status_Id ) REFERENCES  Status  ( Status_Id ) ON DELETE NO Action,
	CONSTRAINT  FK_Employee_Department_Id  FOREIGN KEY ( Department_Id ) REFERENCES  Department  ( Department_Id ) ON DELETE NO Action,
	CONSTRAINT  FK_Employee_Role_Id FOREIGN KEY ( Role_Id ) REFERENCES  Role  ( Role_Id ) ON DELETE NO Action,
	CONSTRAINT  FK_Employee_Permission_Id  FOREIGN KEY ( Permissions_Id ) REFERENCES  Permissions  ( Permissions_Id ) ON DELETE NO Action,
	constraint FK_Employee_Team_Id FOREIGN KEY ( Team_Id ) REFERENCES  Teams  ( Team_Id ) ON DELETE NO Action,
	CONSTRAINT UQ_Employee_Name Unique (First_Name, Last_Name, Email ),
	CONSTRAINT  FK_Employee_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Employee_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
    CONSTRAINT  FK_Employee_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
)
-- first run employee table 
alter table teams 
add CONSTRAINT  FK_Teams_team_lead_employee_id FOREIGN KEY ( team_lead_employee_id ) REFERENCES  Employee  ( Employee_Id ) ON DELETE NO Action;

Create Table Attendance(
	Attendance_Id INT PRIMARY KEY IDENTITY(1,1),
	Employee_Id INT Not Null,
	Date DATE Not Null DEFAULT CURRENT_TIMESTAMP,
	Check_In_Time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	Check_Out_Time DATETIME NULL,
	Status_Id INT Not Null, 
	ChangedBy int,
	ChangedOn date,
	CONSTRAINT  FK_Attendance_Status_Id  FOREIGN KEY ( Status_Id ) REFERENCES  Status  ( Status_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Attendance_Employee_Id  FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Attendance_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
)


Create Table Leave (
	Leave_Id INT PRIMARY KEY IDENTITY(1,1),
	Employee_Id INT Not Null,
	Leave_Type VARCHAR(50) Not Null,
	Start_Date DATE Not Null,
	End_Date DATE Not Null,
	Reason varchar(max) Not Null,
	Status_Id INT Not Null Default 4, 
	CreatedBy int Not NUll,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date,
	DeletedBy int ,
	DeletedOn date ,
	CONSTRAINT  FK_Leave_Status_Id  FOREIGN KEY ( Status_Id ) REFERENCES  Status  ( Status_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Leave_Employee_Id  FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Leave_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  Employee  ( Employee_Id )  ON DELETE NO ACTION,
	CONSTRAINT  FK_Leave_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
    CONSTRAINT  FK_Leave_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
)



Create Table Salary (
	Salary_Id INT PRIMARY KEY IDENTITY(1,1),
	Employee_Id INT Not Null,
	Base_Salary DECIMAL Not Null,
	Deductions DECIMAL Not Null,
	Total_Salary DECIMAL Not Null,
	Pay_Date DATE Not Null, 
	CreatedBy int Not NUll,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date,
	DeletedBy int ,
	DeletedOn date ,
	CONSTRAINT  FK_Salary_Employee_Id  FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Salary_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Salary_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
    CONSTRAINT  FK_Salary_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
)
-- Only For  the Lead Engineeer can Review 
Create Table Review (
	Review_Id INT PRIMARY KEY IDENTITY(1,1),
	Team_Id INT Not Null,
	Employee_Id INT Not Null,
	Reviewed_By INT Not Null,
	Review_Date Date Not Null,
	Comments varchar(max) Not Null,
	Performance_Rating Decimal Not Null, -- need to provide somme validation for which it cant go more then 10.00
	CONSTRAINT  FK_Review_Employee_Id  FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Review_Reviewed_By FOREIGN KEY ( Reviewed_By ) REFERENCES  Employee  ( Employee_Id ) On Delete No Action,
	CONSTRAINT  FK_Review_Team_Id FOREIGN KEY ( Team_Id ) REFERENCES  Teams  ( Team_Id ) On Delete No Action
)
Create Table Bonus (
	Bonus_Id INT PRIMARY KEY IDENTITY(1,1),
	Employee_Id INT Not Null,
	Bonus_Amount DECIMAL,
	Reason varchar(max) Not Null, 
	Date_Awarded DATE Not Null ,
	CreatedBy int Not NUll,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date,
	DeletedBy int ,
	DeletedOn date ,
	CONSTRAINT  FK_Bonus_Employee_Id FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Bonus_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Bonus_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
    CONSTRAINT  FK_Bonus_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
)



Create table Promotion (
	promotion_id INT PRIMARY KEY IDENTITY(1,1),
	Employee_Id INT Not Null,
	Old_Role_Id INT Not Null,
	New_Role_Id INT Not Null,
	Promotion_Date DATE Not Null,
	Reason varchar(max) Not Null,
	CreatedBy int Not NUll,
	CreatedOn date not null DEFAULT GETDATE() ,
	ChangedBy int,
	ChangedOn date,
	DeletedBy int ,
	DeletedOn date ,
	CONSTRAINT  FK_Promotion_Employee_Id FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id ) On Delete Cascade,
	CONSTRAINT  FK_Promotion_CreatedBy  FOREIGN KEY ( CreatedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
	CONSTRAINT  FK_Promotion_ChangedBy  FOREIGN KEY ( ChangedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION,
    CONSTRAINT  FK_Promotion_DeletedBy  FOREIGN KEY (  DeletedBy ) REFERENCES  HR  ( Hr_Id ) ON DELETE NO ACTION
);

Create Table Login (
   Login_Id INT PRIMARY KEY IDENTITY(1,1),
   Username VARCHAR(50) UNIQUE NULL,
   Password VARCHAR(255) NOT NULL, -- Stores the salted and hashed password
   SaltKey_Id INT NULL,        -- Links to the SaltKey table  need to add this in the Api
   Status_Id INT NOT NULL, -- ENUM("Approved", "Rejected", "Pending"), 3 represents "Pending"
   Employee_Id INT NOT NULL,         -- Links to an Employee record
   Login_Time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
   Logout_Time DATETIME Null, 
   CONSTRAINT  FK_Login_Status_Id FOREIGN KEY ( Status_Id ) REFERENCES  Status  ( Status_Id ) On Delete NO Action,
   CONSTRAINT  FK_Login_Employee_Id FOREIGN KEY ( Employee_Id ) REFERENCES  Employee  ( Employee_Id )  On Delete Cascade,
)

select * from Employee

select * from Login

select * from Status

select * from Role;

select * from Department;

select * from Permissions;

select * from Teams;

use human_Resources

select * from Employee
right Join Login On Employee.Employee_Id = Login.Employee_Id


select * from attendance

select * from bonus

select * from Employee
select * from status


select * from role

select * from Status

CREATE PROCEDURE InsertInStatus
    @StatusName VARCHAR(50)
AS
BEGIN
    INSERT INTO Status (Status_Name)
    VALUES (@StatusName)
END
GO


CREATE PROCEDURE InsertInDepartment
    @DepartmentName VARCHAR(50) ,
	@DepartmentDescription varchar(max)
AS
BEGIN
    INSERT INTO Department (Department_Name, Description)
    VALUES (@DepartmentName , @DepartmentDescription)
END
GO
