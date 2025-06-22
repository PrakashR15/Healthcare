USE Hospital
SELECT * FROM Users
SELECT * FROM Prescriptions
SELECT * FROM Appointments
SELECT * FROM HealthRecords
SELECT * FROM dbo.AspNetUsers
SELECT * FROM dbo.AspNetRoles
SELECT * FROM dbo.AspNetUserRoles
sp_help Appointments

DELETE FROM AspNetUsers
DELETE FROM Appointments
ALTER TABLE Appointments
ALTER COLUMN PatientID VARCHAR(450) NOT NULL;
ALTER TABLE Appointments
ALTER COLUMN DoctorID VARCHAR(450) NOT NULL;



ALTER TABLE Appointments
ALTER COLUMN DoctorID varchar(50) NOT NULL;


SELECT * FROM Appointments WHERE DoctorID IS NOT NULL;


ALTER TABLE Appointments DROP COLUMN AppointmentID;

ALTER TABLE Appointments ADD AppointmentID INT IDENTITY(1,1) PRIMARY KEY;


DELETE FROM Appointments WHERE DoctorID IS NULL OR PatientID IS NULL OR Date IS NULL OR Reason IS NULL OR Status IS NULL;
SELECT * FROM Appointments WHERE DoctorID IS NULL OR PatientID IS NULL OR Date IS NULL OR Reason IS NULL OR Status IS NULL;

UPDATE Appointments
SET Status = 'Pending'
WHERE Status IS NULL;


ALTER TABLE Appointments
ALTER COLUMN DoctorID NVARCHAR(450) NOT NULL;

ALTER TABLE Appointments
ALTER COLUMN PatientID INT NOT NULL;

ALTER TABLE Appointments
ALTER COLUMN Date DATETIME NOT NULL;

ALTER TABLE Appointments
ALTER COLUMN Reason NVARCHAR(MAX) NOT NULL;

ALTER TABLE Appointments
ALTER COLUMN Status NVARCHAR(50) NOT NULL;






ALTER TABLE Appointments ALTER COLUMN Reason VARCHAR(20) NOT NULL;
ALTER TABLE Appointments ALTER COLUMN Status VARCHAR(20) NOT NULL;
ALTER TABLE Appointments ALTER COLUMN Date DATETIME NOT NULL;
ALTER TABLE Appointments ALTER COLUMN PatientID INT NOT NULL;
ALTER TABLE Appointments ALTER COLUMN DoctorID VARCHAR(20) NOT NULL;

sp_help Appointments

SELECT COLUMN_NAME, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Appointments'


DELETE FROM Appointments
DELETE FROM AspNetUsers WHERE Role = 'Doctor';
DELETE FROM AspNetUsers WHERE Role = 'Patient';


DELETE FROM Appointments WHERE AppointmentID is NULL


sp_help Appointments
ALTER TABLE Appointments ALTER COLUMN AppointmentID INT;
ALTER TABLE Appointments ALTER COLUMN PatientID INT;
ALTER TABLE Appointments ALTER COLUMN DoctorID INT;
ALTER TABLE Appointments ALTER COLUMN Date DATETIME;



TRUNCATE TABLE dbo.AspNetUsers


sp_help Users
ALTER TABLE Users ALTER COLUMN UserID INT;
ALTER TABLE Users ALTER COLUMN CreatedAt DATETIME;



SELECT u.FullName, u.Email, a.Date, a.Status
FROM Users u
JOIN Appointments a ON u.UserID = a.UserID;
