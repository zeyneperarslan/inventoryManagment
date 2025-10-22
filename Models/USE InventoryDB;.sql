USE InventoryDB;
GO

--------------------------
-- 1) ROL & KULLANICI
--------------------------
IF NOT EXISTS (SELECT 1 FROM roles WHERE Name = 'admin')
INSERT INTO roles (Name) VALUES ('admin');

IF NOT EXISTS (SELECT 1 FROM users WHERE Email = 'zeynep@example.com')
INSERT INTO users (FirstName, LastName, PersonnelId, CardNumber, Email, PasswordHash, IsActive, RoleId)
VALUES ('Zeynep', 'Erarslan', 'P-0001', 'CARD-001', 'zeynep@example.com', 'hash', 1,
        (SELECT TOP 1 Id FROM roles WHERE Name = 'admin'));

--------------------------
-- 2) DEPO & RAF
--------------------------
IF NOT EXISTS (SELECT 1 FROM warehouse WHERE Name = 'Ana Depo')
INSERT INTO warehouse (Name) VALUES ('Ana Depo');

DECLARE @wid INT = (SELECT TOP 1 Id FROM warehouse WHERE Name = 'Ana Depo');

IF NOT EXISTS (SELECT 1 FROM shelves WHERE Name = 'A-1' AND WarehouseId = @wid)
INSERT INTO shelves (Name, ShelfLevel, WarehouseId) VALUES ('A-1', 1, @wid);

--------------------------
-- 3) MALZEME
--------------------------
IF NOT EXISTS (SELECT 1 FROM materials WHERE Barcode = 'MAT-0001')
INSERT INTO materials (Barcode, Name, DoorType, UnitOfMeasure, Description)
VALUES ('MAT-0001', 'M3 Vida', 'Regular', 'PIECE', 'Other');

--------------------------
-- 4) Hızlı kontrol
--------------------------
SELECT TOP 5 Id, Name FROM roles;
SELECT TOP 5 Id, FirstName, LastName, Email FROM users;
SELECT TOP 5 Id, Name FROM warehouse;
SELECT TOP 5 Id, Name, ShelfLevel, WarehouseId FROM shelves;
SELECT TOP 5 Id, Barcode, Name FROM materials;
