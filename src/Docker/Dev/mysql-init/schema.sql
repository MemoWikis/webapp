-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: memoWikis_dev
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `activitypoints`
--

DROP TABLE IF EXISTS `activitypoints`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitypoints` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DateEarned` datetime DEFAULT NULL,
  `Amount` int DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `ActionType` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `User_id` (`User_id`)
) ENGINE=InnoDB AUTO_INCREMENT=902340 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ai_usage_log`
--

DROP TABLE IF EXISTS `ai_usage_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ai_usage_log` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `User_id` int NOT NULL,
  `Page_id` int NOT NULL,
  `TokenIn` int NOT NULL,
  `TokenOut` int NOT NULL,
  `DateCreated` datetime NOT NULL,
  `Model` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `idx_user_id` (`User_id`),
  KEY `idx_page_id` (`Page_id`),
  KEY `idx_date` (`DateCreated`),
  KEY `idx_model` (`Model`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `answer`
--

DROP TABLE IF EXISTS `answer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `answer` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `QuestionId` int DEFAULT NULL,
  `QuestionViewGuid` varchar(36) DEFAULT NULL,
  `InteractionNumber` int DEFAULT NULL,
  `AnswerText` varchar(1000) DEFAULT NULL,
  `AnswerredCorrectly` int DEFAULT NULL,
  `Milliseconds` int DEFAULT NULL,
  `DateCreated` timestamp NULL DEFAULT NULL,
  `Round_id` int DEFAULT NULL,
  `Player_id` int DEFAULT NULL,
  `LearningSession_id` int DEFAULT NULL,
  `LearningSessionStepGuid` varchar(36) DEFAULT NULL,
  `Migrated` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Round_id` (`Round_id`),
  KEY `Player_id` (`Player_id`),
  KEY `AnswerredCorrectly` (`AnswerredCorrectly`),
  KEY `UserId` (`UserId`),
  KEY `QuestionId` (`QuestionId`),
  KEY `LearningSessionId` (`LearningSession_id`),
  KEY `IX_QuestionViewGuid` (`QuestionViewGuid`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7486160 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `comment`
--

DROP TABLE IF EXISTS `comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comment` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` int DEFAULT NULL,
  `TypeId` int DEFAULT NULL,
  `ShouldRemove` bit(1) DEFAULT NULL,
  `ShouldKeys` varchar(255) DEFAULT NULL,
  `Text` mediumtext,
  `ShouldImprove` bit(1) DEFAULT NULL,
  `IsSettled` bit(1) DEFAULT b'0',
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `AnswerTo` int DEFAULT NULL,
  `Creator_id` int DEFAULT NULL,
  `Title` text,
  PRIMARY KEY (`Id`),
  KEY `AnswerTo` (`AnswerTo`)
) ENGINE=InnoDB AUTO_INCREMENT=298 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `imagemetadata`
--

DROP TABLE IF EXISTS `imagemetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagemetadata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` int DEFAULT NULL,
  `TypeId` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Source` int DEFAULT NULL,
  `SourceUrl` varchar(255) DEFAULT NULL,
  `ApiResult` text,
  `ApiHost` varchar(255) DEFAULT NULL,
  `Author` text,
  `Description` text,
  `Markup` text,
  `MarkupDownloadDate` datetime DEFAULT NULL,
  `ManualEntries` text,
  `MainLicenseInfo` text,
  `AllRegisteredLicenses` varchar(1000) DEFAULT NULL,
  `Notifications` text,
  `LicenseState` tinyint DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Type-Index` (`Type`),
  KEY `TypeId-Index` (`TypeId`)
) ENGINE=InnoDB AUTO_INCREMENT=2571 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jobqueue`
--

DROP TABLE IF EXISTS `jobqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobqueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobQueueType` int DEFAULT NULL,
  `JobContent` text,
  `Priority` int DEFAULT '0',
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `learningsession`
--

DROP TABLE IF EXISTS `learningsession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `learningsession` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `SetToLearn_id` int DEFAULT NULL,
  `SetsToLearnIdsString` varchar(100) DEFAULT NULL,
  `SetListTitle` varchar(255) DEFAULT NULL,
  `PageToLearn_id` int DEFAULT NULL,
  `DateToLearn_id` int DEFAULT NULL,
  `IsWishSession` bit(1) DEFAULT b'0',
  `SettingLearningSessionType` int DEFAULT '0',
  `StepsJson` text,
  `IsCompleted` bit(1) DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `User_id` (`User_id`),
  KEY `SetToLearn_id` (`SetToLearn_id`),
  KEY `DateToLearn_id` (`DateToLearn_id`),
  CONSTRAINT `FK946A74881B215E3` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=26768 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `message`
--

DROP TABLE IF EXISTS `message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `message` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReceiverId` int DEFAULT NULL,
  `Subject` varchar(500) DEFAULT NULL,
  `Body` text,
  `MessageType` varchar(255) DEFAULT NULL,
  `IsRead` tinyint(1) DEFAULT NULL,
  `WasSendPerEmail` tinyint DEFAULT NULL,
  `WasSendToSmartphone` tinyint DEFAULT NULL,
  `TrainingDate_id` int DEFAULT NULL,
  `TrainingPlan_id` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `TrainingDate_id` (`TrainingDate_id`),
  KEY `TrainingPlan_id` (`TrainingPlan_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10308 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `messageemail`
--

DROP TABLE IF EXISTS `messageemail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `messageemail` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `User_id` int DEFAULT NULL,
  `MessageEmailType` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `User_id` (`User_id`)
) ENGINE=InnoDB AUTO_INCREMENT=81024 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `page`
--

DROP TABLE IF EXISTS `page`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `page` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Description` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `WikipediaURL` varchar(255) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `UrlLinkText` varchar(255) DEFAULT NULL,
  `CategoriesToExcludeIdsString` varchar(1000) DEFAULT NULL,
  `CategoriesToIncludeIdsString` varchar(1000) DEFAULT NULL,
  `DisableLearningFunctions` bit(1) DEFAULT NULL,
  `FeaturedSetsIdsString` varchar(100) DEFAULT NULL,
  `TopicMarkdown` text,
  `Content` mediumtext CHARACTER SET utf8 COLLATE utf8_general_ci,
  `CustomSegments` longtext,
  `AggregatedContentJson` text,
  `CountQuestionsAggregated` int(7) unsigned zerofill DEFAULT NULL,
  `CountSetsAggregated` int(7) unsigned zerofill DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT '50',
  `CorrectnessProbabilityAnswerCount` int DEFAULT '0',
  `TotalRelevancePersonalEntries` int DEFAULT NULL,
  `TypeJson` text,
  `DateCreated` timestamp NULL DEFAULT NULL,
  `DateModified` timestamp NULL DEFAULT NULL,
  `Creator_id` int DEFAULT NULL,
  `SkipMigration` bit(1) DEFAULT NULL,
  `Visibility` int DEFAULT '0',
  `IsWiki` bit(1) DEFAULT b'0',
  `AuthorIds` text,
  `TextIsHidden` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12296 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pagechange`
--

DROP TABLE IF EXISTS `pagechange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagechange` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Data` longtext,
  `Type` int DEFAULT NULL,
  `DataVersion` int DEFAULT NULL,
  `ShowInSidebar` bit(1) DEFAULT b'1',
  `DateCreated` datetime DEFAULT NULL,
  `Author_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `Parent_Page_Ids` text,
  PRIMARY KEY (`Id`),
  KEY `FK_Author_id` (`Author_id`),
  KEY `FK_Category_id` (`Page_id`),
  CONSTRAINT `FK_Author_id` FOREIGN KEY (`Author_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=69782 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pagerelation`
--

DROP TABLE IF EXISTS `pagerelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagerelation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Page_id` int NOT NULL,
  `Related_Id` int NOT NULL,
  `Previous_id` int DEFAULT NULL,
  `Next_id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `RCTRC_CategoryId` (`Related_Id`),
  KEY `RCTRC_RelatedId` (`Page_id`),
  CONSTRAINT `pagerelation_ibfk_1` FOREIGN KEY (`Related_Id`) REFERENCES `page` (`Id`),
  CONSTRAINT `pagerelation_ibfk_2` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=356661 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pages_to_questions`
--

DROP TABLE IF EXISTS `pages_to_questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pages_to_questions` (
  `Page_id` int NOT NULL,
  `Question_id` int NOT NULL,
  KEY `Category_id` (`Page_id`),
  KEY `Question_id` (`Question_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pagevaluation`
--

DROP TABLE IF EXISTS `pagevaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagevaluation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `PageId` int DEFAULT NULL,
  `RelevancePersonal` int DEFAULT NULL,
  `CountNotLearned` int DEFAULT NULL,
  `CountNeedsLearning` int DEFAULT NULL,
  `CountNeedsConsolidation` int DEFAULT NULL,
  `CountSolid` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5111 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pageview`
--

DROP TABLE IF EXISTS `pageview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pageview` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserAgent` varchar(1000) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `DateOnly` date GENERATED ALWAYS AS (cast(`DateCreated` as date)) VIRTUAL,
  PRIMARY KEY (`Id`),
  KEY `Category_id` (`Page_id`),
  KEY `User_id` (`User_id`),
  KEY `idx_dateonly` (`DateOnly`)
) ENGINE=InnoDB AUTO_INCREMENT=7132137 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `passwordrecoverytoken`
--

DROP TABLE IF EXISTS `passwordrecoverytoken`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `passwordrecoverytoken` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Token` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=508 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persistentlogin`
--

DROP TABLE IF EXISTS `persistentlogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `persistentlogin` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `LoginGuid` varchar(255) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1885 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `question`
--

DROP TABLE IF EXISTS `question`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `question` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Text` varchar(3000) DEFAULT NULL,
  `TextHtml` mediumtext,
  `TextExtended` text,
  `TextExtendedHtml` text,
  `Description` text,
  `DescriptionHtml` text,
  `License` int DEFAULT NULL,
  `Visibility` int DEFAULT NULL,
  `Creator_id` int DEFAULT NULL,
  `Solution` mediumtext,
  `SolutionType` tinyint unsigned DEFAULT NULL,
  `SolutionMetadataJson` varchar(7000) DEFAULT NULL,
  `TotalTrueAnswers` int DEFAULT NULL,
  `TotalFalseAnswers` int DEFAULT NULL,
  `TotalQualityAvg` int DEFAULT NULL,
  `TotalQualityEntries` int DEFAULT NULL,
  `TotalRelevanceForAllAvg` int DEFAULT NULL,
  `TotalRelevanceForAllEntries` int DEFAULT NULL,
  `TotalRelevancePersonalAvg` int DEFAULT NULL,
  `TotalRelevancePersonalEntries` int DEFAULT NULL,
  `SetsAmount` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT NULL,
  `CorrectnessProbabilityAnswerCount` int DEFAULT '0',
  `SetsTop5Json` varchar(1000) DEFAULT NULL,
  `IsWorkInProgress` bit(1) DEFAULT b'0',
  `DateCreated` timestamp NULL DEFAULT NULL,
  `DateModified` timestamp NULL DEFAULT NULL,
  `SkipMigration` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `QuestionCreaterId` (`Creator_id`),
  KEY `Visibility` (`Visibility`),
  KEY `TotalRelevancePersonalEntries` (`TotalRelevancePersonalEntries`),
  KEY `DateCreated` (`DateCreated`),
  CONSTRAINT `question_ibfk_1` FOREIGN KEY (`Creator_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11755 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `questionchange`
--

DROP TABLE IF EXISTS `questionchange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionchange` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Data` text,
  `Type` int DEFAULT NULL,
  `DataVersion` int DEFAULT NULL,
  `ShowInSidebar` bit(1) DEFAULT b'1',
  `DateCreated` datetime DEFAULT NULL,
  `Author_Id` int DEFAULT NULL,
  `Question_Id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Question_Author_Id` (`Author_Id`),
  KEY `FK_Question_Id` (`Question_Id`),
  CONSTRAINT `FK_Question_Author_Id` FOREIGN KEY (`Author_Id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14766 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `questionvaluation`
--

DROP TABLE IF EXISTS `questionvaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionvaluation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `QuestionId` int DEFAULT NULL,
  `Quality` int DEFAULT NULL,
  `RelevancePersonal` int DEFAULT NULL,
  `RelevanceForAll` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT NULL,
  `CorrectnessProbabilityAnswerCount` int DEFAULT '0',
  `KnowledgeStatus` int DEFAULT NULL,
  `DateCreated` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Unique_pair_of_userId_questionId` (`UserId`,`QuestionId`),
  KEY `QuestionId` (`QuestionId`),
  KEY `userId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=939778 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `questionview`
--

DROP TABLE IF EXISTS `questionview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionview` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Guid` varchar(36) DEFAULT NULL,
  `QuestionId` int NOT NULL,
  `UserId` int DEFAULT NULL,
  `Milliseconds` int DEFAULT NULL,
  `UserAgent` varchar(512) DEFAULT NULL,
  `Round_id` int DEFAULT NULL,
  `Player_id` int DEFAULT NULL,
  `WidgetView_id` int DEFAULT NULL,
  `LearningSession_id` int DEFAULT NULL,
  `LearningSessionStepGuid` varchar(36) DEFAULT NULL,
  `Migrated` bit(1) DEFAULT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DateOnly` date GENERATED ALWAYS AS (cast(`DateCreated` as date)) VIRTUAL,
  PRIMARY KEY (`Id`),
  KEY `FK_questionview_game_player` (`Player_id`),
  KEY `FK_questionview_game_round` (`Round_id`),
  KEY `FK_WidgetView` (`WidgetView_id`),
  KEY `QuestionId` (`QuestionId`),
  KEY `UserId` (`UserId`),
  KEY `idx_questionview_Guid` (`Guid`),
  KEY `idx_dateonly` (`DateOnly`),
  CONSTRAINT `FK_WidgetView` FOREIGN KEY (`WidgetView_id`) REFERENCES `widgetview` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13098087 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reference`
--

DROP TABLE IF EXISTS `reference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reference` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReferenceType` int DEFAULT NULL,
  `AdditionalInfo` text,
  `ReferenceText` text,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `Question_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Question_id` (`Question_id`),
  KEY `Category_id` (`Page_id`),
  CONSTRAINT `FK_reference_category` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5060 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `runningjob`
--

DROP TABLE IF EXISTS `runningjob`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `runningjob` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StartAt` datetime DEFAULT NULL,
  `Name` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6965 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `setting` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppVersion` int DEFAULT NULL,
  `SuggestedSetsIdString` varchar(800) DEFAULT NULL,
  `SuggestedGames` varchar(800) DEFAULT NULL,
  `DateCreated` timestamp NULL DEFAULT NULL,
  `DateModified` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PasswordHashedAndSalted` varchar(255) DEFAULT NULL,
  `Salt` varchar(255) DEFAULT NULL,
  `EmailAddress` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsEmailConfirmed` bit(1) DEFAULT NULL,
  `IsInstallationAdmin` bit(1) DEFAULT NULL,
  `AllowsSupportiveLogin` bit(1) DEFAULT b'0',
  `ShowWishKnowledge` bit(1) DEFAULT b'0',
  `KnowledgeReportInterval` int NOT NULL DEFAULT '0',
  `Birthday` timestamp NULL DEFAULT NULL,
  `Reputation` int DEFAULT NULL,
  `ReputationPos` int DEFAULT NULL,
  `WishCountQuestions` int DEFAULT NULL,
  `WishCountSets` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT '50',
  `CorrectnessProbabilityAnswerCount` int DEFAULT '0',
  `FacebookId` varchar(20) DEFAULT NULL,
  `GoogleId` varchar(25) DEFAULT NULL,
  `ActivityPoints` int NOT NULL DEFAULT '0',
  `ActivityLevel` int NOT NULL DEFAULT '0',
  `WidgetHostsSpaceSeparated` varchar(1000) DEFAULT NULL,
  `DateCreated` timestamp NULL DEFAULT NULL,
  `DateModified` timestamp NULL DEFAULT NULL,
  `FollowerCount` int DEFAULT NULL,
  `TotalInOthersWishknowledge` int DEFAULT NULL,
  `LearningSessionOptions` varchar(500) DEFAULT NULL,
  `StartPageId` int DEFAULT '0',
  `BouncedMail` bit(1) DEFAULT NULL,
  `MailBounceReason` text,
  `RecentlyUsedRelationTargetPages` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `StripeId` varchar(255) DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `SubscriptionStartDate` datetime DEFAULT NULL,
  `WikiIds` text,
  `FavoriteIds` text,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `FacebookId` (`FacebookId`),
  UNIQUE KEY `GoogleId` (`GoogleId`)
) ENGINE=InnoDB AUTO_INCREMENT=8773 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user_to_follower`
--

DROP TABLE IF EXISTS `user_to_follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_to_follower` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `User_id` int NOT NULL,
  `Follower_id` int NOT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `Foo` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Follower_id` (`Follower_id`),
  KEY `User_id` (`User_id`),
  CONSTRAINT `FKBFFF1C125FCC0E1C` FOREIGN KEY (`Follower_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FKBFFF1C1281B215E3` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=715 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `useractivity`
--

DROP TABLE IF EXISTS `useractivity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `useractivity` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `At` datetime DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `UserConcerned_id` int DEFAULT NULL,
  `Question_id` int DEFAULT NULL,
  `Set_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `Date_id` int DEFAULT NULL,
  `Game_id` int DEFAULT NULL,
  `UserIsFollowed_id` int DEFAULT NULL,
  `UserCauser_id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserConcerned_id` (`UserConcerned_id`),
  KEY `Question_id` (`Question_id`),
  KEY `Set_id` (`Set_id`),
  KEY `Category_id` (`Page_id`),
  KEY `Date_id` (`Date_id`),
  KEY `Game_id` (`Game_id`),
  KEY `UserIsFollowed_id` (`UserIsFollowed_id`),
  KEY `UserCauser_id` (`UserCauser_id`),
  CONSTRAINT `Category_id_FK` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`),
  CONSTRAINT `Question_id_FK` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`),
  CONSTRAINT `UserCauser_id` FOREIGN KEY (`UserCauser_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `UserConcerned_id_FK1` FOREIGN KEY (`UserConcerned_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `UserIsFollowed_id` FOREIGN KEY (`UserIsFollowed_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=154553 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `widgetview`
--

DROP TABLE IF EXISTS `widgetview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `widgetview` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Host` varchar(255) DEFAULT NULL,
  `WidgetKey` varchar(255) DEFAULT NULL,
  `WidgetType` tinyint NOT NULL,
  `EntityId` int NOT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `WidgetKey` (`WidgetKey`),
  KEY `Host` (`Host`)
) ENGINE=InnoDB AUTO_INCREMENT=559856 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

INSERT INTO `setting` (`Id`, `AppVersion`, `DateCreated`, `DateModified`)
VALUES (1, 279, NOW(), NOW());