
CREATE TABLE dbo.Tmp_User
	(
	Id int NOT NULL IDENTITY (1, 1),
	PasswordHashedAndSalted nvarchar(255) NULL,
	Salt nvarchar(255) NULL,
	EmailAddress nvarchar(255) NULL,
	Name nvarchar(255) NULL,
	IsEmailConfirmed bit NULL,
	IsInstallationAdmin bit NULL,
	Birthday datetime NULL,
	DateCreated datetime NULL,
	DateModified datetime NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.Tmp_User SET (LOCK_ESCALATION = TABLE)

SET IDENTITY_INSERT dbo.Tmp_User ON

IF EXISTS(SELECT * FROM dbo.[User])
	 EXEC('INSERT INTO dbo.Tmp_User (Id, PasswordHashedAndSalted, Salt, EmailAddress, Name, IsEmailConfirmed, Birthday, DateCreated, DateModified)
		SELECT Id, PasswordHashedAndSalted, Salt, EmailAddress, Name, IsEmailConfirmed, Birthday, DateCreated, DateModified FROM dbo.[User] WITH (HOLDLOCK TABLOCKX)')

SET IDENTITY_INSERT dbo.Tmp_User OFF

ALTER TABLE dbo.Question
	DROP CONSTRAINT QuestionCreaterId

ALTER TABLE dbo.Category
	DROP CONSTRAINT CategoryCreaterId

DROP TABLE dbo.[User]

EXECUTE sp_rename N'dbo.Tmp_User', N'User', 'OBJECT' 

ALTER TABLE dbo.[User] ADD CONSTRAINT
	UserId PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



ALTER TABLE dbo.Category ADD CONSTRAINT
	CategoryCreaterId FOREIGN KEY
	(
	Creator_id
	) REFERENCES dbo.[User]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Category SET (LOCK_ESCALATION = TABLE)

ALTER TABLE dbo.Question ADD CONSTRAINT
	QuestionCreaterId FOREIGN KEY
	(
	Creator_id
	) REFERENCES dbo.[User]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Question SET (LOCK_ESCALATION = TABLE)

