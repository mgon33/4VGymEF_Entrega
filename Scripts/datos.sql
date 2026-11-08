SET NOCOUNT ON;

BEGIN TRANSACTION;

DELETE FROM ActivityInstructor;
DELETE FROM Activities;
DELETE FROM Intructors;
DELETE FROM ActivityTypes;

SET IDENTITY_INSERT ActivityTypes ON;
INSERT INTO ActivityTypes (Id, Name, NumberMonitors) VALUES
    (1,  'BodyPump', 2),
    (2,  'Spinning', 1),
    (3,  'Pilates',  1),
    (40, 'CrossFit', 3);
SET IDENTITY_INSERT ActivityTypes OFF;

SET IDENTITY_INSERT Intructors ON;
INSERT INTO Intructors (Id, Name, Email, PhotoUrl, Phone) VALUES
    (1, N'Marcos Pérez',     'marcos_perez@gmail.com',     'https://www.example.com/fotos/marcos_perez.jpg',     '612345678'),
    (2, N'Lucía Fernández',  'lucia_fernandez@gmail.com',  'https://www.example.com/fotos/lucia_fernandez.jpg',  '623456789'),
    (3, N'Javier Ortega',    'javier_ortega@gmail.com',    'https://www.example.com/fotos/javier_ortega.jpg',    '634567890'),
    (4, N'Marina Sanz',      'marina_sanz@gmail.com',      'https://www.example.com/fotos/marina_sanz.jpg',      '645678901');
SET IDENTITY_INSERT Intructors OFF;

SET IDENTITY_INSERT Activities ON;
INSERT INTO Activities (Id, MaxParticipants, DateStart, DateEnd, ActivityTypeId) VALUES
    (1, 20, '2026-12-01T09:00:00', '2026-12-01T10:00:00', 1),
    (2, 15, '2026-12-01T18:00:00', '2026-12-01T19:00:00', 2),
    (3, 10, '2026-12-02T10:00:00', '2026-12-02T11:00:00', 3),
    (4, 25, '2026-12-03T19:00:00', '2026-12-03T20:30:00', 40);
SET IDENTITY_INSERT Activities OFF;

INSERT INTO ActivityInstructor (ActivitiesId, InstructorsId) VALUES
    (1, 1), (1, 2),
    (2, 3),
    (3, 2),
    (4, 1), (4, 3), (4, 4);

COMMIT TRANSACTION;

SELECT * FROM ActivityTypes;
SELECT * FROM Intructors;
SELECT * FROM Activities;
SELECT * FROM ActivityInstructor;