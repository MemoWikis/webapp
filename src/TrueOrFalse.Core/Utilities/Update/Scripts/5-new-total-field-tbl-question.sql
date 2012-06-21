
ALTER TABLE dbo.Question
	DROP CONSTRAINT QuestionCreaterId

ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)

CREATE TABLE dbo.Tmp_Question
	(
	Id int NOT NULL IDENTITY (1, 1),
	Text nvarchar(MAX) NULL,
	Description nvarchar(MAX) NULL,
	Visibility int NULL,
	Creator_id int NULL,
	Solution nvarchar(255) NULL,
	TotalTrueAnswers int NULL,
	TotalFalseAnswers int NULL,
	TotalQualityAvg int NULL,
	TotalQualityEntries int NULL,
	TotalRelevanceForAllAvg int NULL,
	TotalRelevanceForAllEntries int NULL,
	TotalRelevancePersonalAvg int NULL,
	TotalRelevancePersonalEntries int NULL,
	DateCreated datetime NULL,
	DateModified datetime NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
ALTER TABLE dbo.Tmp_Question SET (LOCK_ESCALATION = TABLE)
SET IDENTITY_INSERT dbo.Tmp_Question ON
IF EXISTS(SELECT * FROM dbo.Question)
	 EXEC('INSERT INTO dbo.Tmp_Question (Id, Text, Description, Visibility, Creator_id, Solution, TotalTrueAnswers, TotalFalseAnswers, DateCreated, DateModified)
		SELECT Id, Text, Description, Visibility, Creator_id, Solution, TotalTrueAnswers, TotalFalseAnswers, DateCreated, DateModified FROM dbo.Question WITH (HOLDLOCK TABLOCKX)')
SET IDENTITY_INSERT dbo.Tmp_Question OFF
ALTER TABLE dbo.CategoriesToQuestions
	DROP CONSTRAINT CategoryToQuestion_CategoryId
DROP TABLE dbo.Question
EXECUTE sp_rename N'dbo.Tmp_Question', N'Question', 'OBJECT' 
ALTER TABLE dbo.Question ADD CONSTRAINT
	QuestionId PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE dbo.Question ADD CONSTRAINT
	QuestionCreaterId FOREIGN KEY
	(
	Creator_id
	) REFERENCES dbo.[User]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.CategoriesToQuestions ADD CONSTRAINT
	CategoryToQuestion_CategoryId FOREIGN KEY
	(
	Question_id
	) REFERENCES dbo.Question
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 