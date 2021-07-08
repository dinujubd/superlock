CREATE DATABASE IF NOT EXISTS AppDBSuperLock /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

CREATE TABLE IF NOT EXISTS AppDBSuperLock.Locks (
  LockId char(50) NOT NULL,
  Code varchar(50) DEFAULT NULL,
  IsActive tinyint(1) NOT NULL DEFAULT 1,
  CreatedOn date NOT NULL,
  CreatedBy char(50) NOT NULL,
  UpdatedDate date DEFAULT NULL,
  UpdatedBy char(50) DEFAULT NULL,
  PRIMARY KEY (LockId),
  UNIQUE KEY Locks_UN (Code),
  KEY Locks_Code_IDX (Code) USING BTREE
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS AppDBSuperLock.Roles (
  RoleId char(50) NOT NULL,
  Name varchar(50) NOT NULL,
  IsActive tinyint(1) NOT NULL DEFAULT 1,
  CreatedOn date NOT NULL,
  CreatedBy char(50) NOT NULL,
  UpdatedDate date DEFAULT NULL,
  UpdatedBy char(50) DEFAULT NULL,
  PRIMARY KEY (RoleId),
  UNIQUE KEY Roles_UN (Name),
  KEY Roles_Name_IDX (Name) USING BTREE
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS AppDBSuperLock.Users (
  UserId char(50) NOT NULL,
  FirstName varchar(50) NOT NULL,
  LastName varchar(50) NOT NULL,
  UserName varchar(50) NOT NULL,
  Email varchar(50) NOT NULL,
  Secret varchar(100) NOT NULL,
  PhoneNo varchar(50) DEFAULT NULL,
  IsActive tinyint(1) NOT NULL DEFAULT 1,
  CreatedOn date NOT NULL,
  CreatedBy char(50) NOT NULL,
  UpdatedDate date DEFAULT NULL,
  UpdatedBy char(50) DEFAULT NULL,
  PRIMARY KEY (UserId),
  UNIQUE KEY Users_UN_UN (UserName),
  UNIQUE KEY Users_EM_UN (Email),
  UNIQUE KEY Users_PN_UN (PhoneNo),
  KEY Users_UserName_IDX (UserName) USING BTREE,
  KEY Users_Email_IDX (Email) USING BTREE,
  KEY Users_PhoneNo_IDX (PhoneNo) USING BTREE
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;


CREATE TABLE IF NOT EXISTS AppDBSuperLock.UserRoles (
  UserRoleId char(50) NOT NULL,
  UserId char(50) NOT NULL,
  RoleId char(50) NOT NULL,
  CreatedOn date NOT NULL,
  CreatedBy char(50) NOT NULL,
  UpdatedDate date DEFAULT NULL,
  UpdatedBy char(50) DEFAULT NULL,
  PRIMARY KEY (UserRoleId),
  KEY UserRoles_FK_1 (RoleId),
  KEY UserRoles_UserId_IDX (UserId) USING BTREE,
  CONSTRAINT UserRoles_FK FOREIGN KEY (UserId) REFERENCES Users (UserId),
  CONSTRAINT UserRoles_FK_1 FOREIGN KEY (RoleId) REFERENCES Roles (RoleId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE IF NOT EXISTS AppDBSuperLock.UserLocks (
  UserLockId char(50) NOT NULL,
  UserId char(50) NOT NULL,
  LockId char(50) NOT NULL,
  CreatedOn date NOT NULL,
  CreatedBy char(50) NOT NULL,
  UpdatedDate date DEFAULT NULL,
  UpdatedBy char(50) DEFAULT NULL,
  PRIMARY KEY (UserLockId),
  UNIQUE KEY `UserLocks_UN` (`LockId`),
  KEY UserLock_FK (UserId),
  KEY UserLock_FK_1 (LockId),
  KEY UserLocks_UserId_IDX (UserId) USING BTREE,
  KEY UserLocks_LockId_IDX (LockId) USING BTREE,
  CONSTRAINT UserLock_FK FOREIGN KEY (UserId) REFERENCES Users (UserId),
  CONSTRAINT UserLock_FK_1 FOREIGN KEY (LockId) REFERENCES Locks (LockId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;



CREATE TABLE IF NOT EXISTS AppDBSuperLock.UserUnlockActivity (
	UserUnlockActivityId char(50) NOT NULL,
	UserId char(50) NOT NULL,
  	LockId char(50) NOT NULL,
  	UserLockId char(50) NOT NULL,
	CreatedOn DATE NOT NULL,
    PRIMARY KEY (UserUnlockActivityId),
    KEY UserUnlockActivity_FK (UserId),
    KEY UserUnlockActivity_FK_1 (LockId),
    KEY UserUnlockActivity_FK_2 (UserLockId),
    CONSTRAINT UserUnlockActivity_FK FOREIGN KEY (UserId) REFERENCES Users (UserId),
    CONSTRAINT UserUnlockActivity_FK_1 FOREIGN KEY (LockId) REFERENCES Locks (LockId),
    CONSTRAINT UserUnlockActivity_FK_2 FOREIGN KEY (UserLockId) REFERENCES UserLocks (UserLockId)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;


DELETE FROM AppDBSuperLock.UserRoles;
DELETE FROM AppDBSuperLock.UserLocks;
DELETE FROM AppDBSuperLock.Users;
DELETE FROM AppDBSuperLock.Roles;
DELETE FROM AppDBSuperLock.Locks;


INSERT INTO AppDBSuperLock.Users (UserId, FirstName, LastName, UserName, Secret, Email, PhoneNo, IsActive, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy) VALUES 
('aa9d5e5b-5beb-4318-86ad-8790c3c23311', 'Admin First Name', 'Admin Last Name', 'admin_123', '$2a$11$zPS9rZluwedDhvd9BQU3mObgQvB.RQfFI7i2Dgx.cJEboxMHo5FpW' ,'admin@superlock.com', '+6664937', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('8745d54b-022c-48ac-b483-afe47d836066', 'User First Name', 'User Last Name', 'user_123', '$2a$11$StVpRAI8SICGv8JwNzF8Te0rwCHf6PJmrh75.YkrCyIQQ6Lkl87f2',  'user@superlock.com', '+665345', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL);


INSERT INTO AppDBSuperLock.Roles (RoleId, Name, IsActive, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy) VALUES
	('dbcc17d7-061e-4108-8093-c49a14fd7ebc', 'admin', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
	('bf1fe231-eb00-4f95-9ed9-0ea4b975aa3c', 'appuser', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL);
	



INSERT INTO AppDBSuperLock.UserRoles (UserRoleId, UserId, RoleId, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy) VALUES
('ddd5775a-3f77-420c-b23d-b4569eb3b71d', 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', 'dbcc17d7-061e-4108-8093-c49a14fd7ebc', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('8861e60c-375c-4b1a-ae02-d751ed3e2449', '8745d54b-022c-48ac-b483-afe47d836066', 'bf1fe231-eb00-4f95-9ed9-0ea4b975aa3c', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL);


INSERT INTO AppDBSuperLock.Locks (LockId, Code, IsActive, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy) VALUES
('42640298-a540-4f6c-ad0c-c44834a845d0', 'c44834a845d0', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('a2795184-1635-451b-98a5-51709dda17db', '51709dda17db', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('e375ec92-e34c-4659-9a8f-23d18c65db58', '23d18c65db58', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('ced139f8-0cff-4a75-87d3-c4b17e2c2528', 'c4b17e2c2528', 1, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('96b14c1f-7cfb-4ae0-a016-3471229336e7', '3471229336e7', 0, NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL);


INSERT INTO AppDBSuperLock.UserLocks (UserLockId, UserId, LockId, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy) VALUES
('eaee8eb7-d49c-4190-b0b0-f3c0b75eb255', 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', '42640298-a540-4f6c-ad0c-c44834a845d0', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('368b0a3c-8728-457e-9884-0261cbf4583b', 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', 'a2795184-1635-451b-98a5-51709dda17db', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('cdb457ea-2cd4-43e2-8285-6b4cff91f63d', '8745d54b-022c-48ac-b483-afe47d836066', 'e375ec92-e34c-4659-9a8f-23d18c65db58', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('937a2af8-facc-4d01-88b5-6ff1d3193d67', '8745d54b-022c-48ac-b483-afe47d836066', 'ced139f8-0cff-4a75-87d3-c4b17e2c2528', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL),
('4bfa09e4-0f44-4f07-ab91-43803c38714d', '8745d54b-022c-48ac-b483-afe47d836066', '96b14c1f-7cfb-4ae0-a016-3471229336e7', NOW(), 'aa9d5e5b-5beb-4318-86ad-8790c3c23311', NULL, NULL);



