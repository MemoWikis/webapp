-- MySQL dump 10.13  Distrib 8.3.0, for Linux (x86_64)
--
-- Host: localhost    Database: memoWikis_dev
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `memoWikis_dev`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `memoWikis_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `memoWikis_dev`;

--
-- Table structure for table `activitypoints`
--

DROP TABLE IF EXISTS `activitypoints`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitypoints` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Amount` int DEFAULT NULL,
  `DateEarned` datetime DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activitypoints`
--

LOCK TABLES `activitypoints` WRITE;
/*!40000 ALTER TABLE `activitypoints` DISABLE KEYS */;
/*!40000 ALTER TABLE `activitypoints` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ai_usage_log`
--

DROP TABLE IF EXISTS `ai_usage_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ai_usage_log` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `User_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `TokenIn` int DEFAULT NULL,
  `TokenOut` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `Model` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ai_usage_log`
--

LOCK TABLES `ai_usage_log` WRITE;
/*!40000 ALTER TABLE `ai_usage_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `ai_usage_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aimodelwhitelist`
--

DROP TABLE IF EXISTS `aimodelwhitelist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aimodelwhitelist` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ModelId` varchar(100) NOT NULL,
  `DisplayName` varchar(200) DEFAULT NULL,
  `Provider` int DEFAULT NULL,
  `TokenCostMultiplier` decimal(10,2) DEFAULT NULL,
  `IsEnabled` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aimodelwhitelist`
--

LOCK TABLES `aimodelwhitelist` WRITE;
/*!40000 ALTER TABLE `aimodelwhitelist` DISABLE KEYS */;
/*!40000 ALTER TABLE `aimodelwhitelist` ENABLE KEYS */;
UNLOCK TABLES;

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
  `AnswerText` varchar(255) DEFAULT NULL,
  `AnswerredCorrectly` int DEFAULT NULL,
  `Milliseconds` int DEFAULT NULL,
  `Migrated` tinyint(1) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `QuestionId` (`QuestionId`),
  CONSTRAINT `FK_4975C7EB` FOREIGN KEY (`QuestionId`) REFERENCES `question` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answer`
--

LOCK TABLES `answer` WRITE;
/*!40000 ALTER TABLE `answer` DISABLE KEYS */;
INSERT INTO `answer` VALUES (1,1,6,'3ffb4d41-b2aa-4b7b-8e0c-072c0c60416e',1,'Answer 2 for page LearningUser Subtopic 2-1',0,5127,0,'2024-12-11 12:16:38'),(2,1,3,'1352f922-08ce-485e-87a9-bd0c196074b7',1,'Answer 1 for page LearningUser Subtopic 1-1',1,9622,0,'2024-12-11 10:51:38'),(3,1,2,'62c390c4-155a-4876-b2a2-7aa308bd99cc',1,'Incorrect answer',1,1941,0,'2024-12-11 08:32:42'),(4,1,5,'e49aa141-f99c-4128-9415-3e38db0aa96c',1,'Incorrect answer',1,1209,0,'2024-12-11 09:25:20'),(5,1,1,'eb5a68dc-ec2d-41d0-937d-d00592f81c1b',1,'Answer 1 for page LearningUser Topic 1',1,6097,0,'2024-12-11 21:43:46'),(6,1,4,'3f78fac7-d0ff-4e84-9725-57e15f0d89de',1,'Answer 2 for page LearningUser Subtopic 1-1',1,10135,0,'2024-12-11 14:22:38'),(7,1,5,'7db502d8-96cf-41e7-97ed-b8b5cbb34d3d',1,'Answer 1 for page LearningUser Subtopic 2-1',1,7374,0,'2024-12-13 09:46:01'),(8,1,6,'99ccf462-ca7b-430a-bbfd-5a5d19b1c5cf',1,'Answer 2 for page LearningUser Subtopic 2-1',1,6483,0,'2024-12-13 22:33:16'),(9,1,4,'47261e76-8c54-49ed-a3ed-06afe8e0d076',1,'Answer 2 for page LearningUser Subtopic 1-1',1,6952,0,'2024-12-13 11:27:36'),(10,1,1,'d9e24d75-e0ea-45f6-8888-705a815385b6',1,'Incorrect answer',1,10030,0,'2024-12-16 12:38:59'),(11,1,6,'4e93fff7-612e-4e37-9b03-dac82fb307b1',1,'Answer 2 for page LearningUser Subtopic 2-1',0,11488,0,'2024-12-16 20:47:24'),(12,1,3,'ecf57097-5fbb-4240-bfa9-5e7e765df658',1,'Answer 1 for page LearningUser Subtopic 1-1',1,12241,0,'2024-12-16 12:03:18'),(13,1,2,'0c913bcb-b71a-4fc5-a3c0-1627e3e28742',1,'Answer 2 for page LearningUser Topic 1',1,1064,0,'2024-12-16 22:21:32'),(14,1,4,'b0eff527-109e-4865-b110-439a31c61006',1,'Answer 2 for page LearningUser Subtopic 1-1',1,11649,0,'2024-12-16 20:36:47'),(15,1,5,'ff1f98fe-bb3e-45c5-bda7-8bb7a41ccc37',1,'Answer 1 for page LearningUser Subtopic 2-1',1,2087,0,'2024-12-16 21:42:10'),(16,1,3,'c7b97cab-effb-4c34-a9d0-762b33654663',1,'Answer 1 for page LearningUser Subtopic 1-1',1,6091,0,'2024-12-17 18:21:34'),(17,1,4,'f38bd3fd-6125-4bf2-b461-eeabc92fbf0c',1,'Answer 2 for page LearningUser Subtopic 1-1',0,2174,0,'2024-12-17 12:46:00'),(18,1,5,'50ce83a2-4da6-4b5e-bc33-63ad85fa28be',1,'Answer 1 for page LearningUser Subtopic 2-1',1,11983,0,'2024-12-17 12:01:32'),(19,1,1,'057d766d-72be-4e46-8020-39bae6bfa0f0',1,'Answer 1 for page LearningUser Topic 1',1,12976,0,'2024-12-17 09:21:44'),(20,1,2,'39af3b75-0104-4d95-a016-93dec9278912',1,'Answer 2 for page LearningUser Topic 1',0,13077,0,'2024-12-17 10:36:52'),(21,1,6,'bc94d604-6bd8-402c-938c-82454f12841b',1,'Incorrect answer',1,3291,0,'2024-12-17 20:54:13'),(22,1,1,'071e274b-bf25-4380-88b5-0245df76fb46',1,'Answer 1 for page LearningUser Topic 1',1,3313,0,'2024-12-19 17:40:14'),(23,1,6,'d7e41f2e-b13f-42f1-875e-e1f9fa97b129',1,'Incorrect answer',1,6073,0,'2024-12-19 09:01:45'),(24,1,3,'670f8350-3db7-47b8-a4b8-920a9b249d85',1,'Answer 1 for page LearningUser Subtopic 1-1',1,3323,0,'2024-12-19 20:48:53'),(25,1,5,'4cf302d0-173d-47f3-a242-d40cef1ab944',1,'Answer 1 for page LearningUser Subtopic 2-1',1,4885,0,'2024-12-19 14:31:11'),(26,1,1,'e853f7e2-55d9-427a-9696-a73b244dafe7',1,'Answer 1 for page LearningUser Topic 1',0,14025,0,'2024-12-25 11:51:58'),(27,1,6,'2dd1973b-a94f-4f62-ac9c-e4b272b8223d',1,'Answer 2 for page LearningUser Subtopic 2-1',1,2761,0,'2024-12-25 14:28:27'),(28,1,5,'1150ead6-7287-4bd6-935b-373e1d43742f',1,'Answer 1 for page LearningUser Subtopic 2-1',1,1549,0,'2024-12-25 16:16:04'),(29,1,2,'5dba573a-9810-483f-9d1a-513b5df4cec0',1,'Incorrect answer',1,4851,0,'2024-12-26 09:37:03'),(30,1,6,'a41f5248-6b47-4c0f-8ba9-a360428ef70a',1,'Answer 2 for page LearningUser Subtopic 2-1',0,10845,0,'2024-12-26 22:05:31'),(31,1,1,'79901d49-d400-4b17-b24e-3b88554c9be2',1,'Answer 1 for page LearningUser Topic 1',0,14567,0,'2024-12-26 21:53:12'),(32,1,4,'2a2a85ec-4481-484b-aa2d-9cf1694f46c9',1,'Answer 2 for page LearningUser Subtopic 1-1',1,5371,0,'2024-12-28 10:06:58'),(33,1,3,'aa0fdda0-e039-4e78-88db-b533425f389d',1,'Answer 1 for page LearningUser Subtopic 1-1',1,13813,0,'2024-12-28 15:16:35'),(34,1,2,'c230848e-12fa-4195-8369-2b403794a2fe',1,'Incorrect answer',1,12564,0,'2024-12-28 11:30:30'),(35,1,5,'8c8b464b-8348-41ad-8420-935488ef8f86',1,'Answer 1 for page LearningUser Subtopic 2-1',0,6456,0,'2024-12-28 16:49:09'),(36,1,2,'72a7f509-8f42-48cc-91b7-fdd6614a2ff4',1,'Answer 2 for page LearningUser Topic 1',0,11992,0,'2024-12-31 12:00:11'),(37,1,4,'f22287b4-724c-45ea-8f87-30925d2538c2',1,'Answer 2 for page LearningUser Subtopic 1-1',1,11554,0,'2024-12-31 19:19:07'),(38,1,1,'0de7bb8d-1159-4a1f-879e-eae50fbf7f4e',1,'Answer 1 for page LearningUser Topic 1',0,14156,0,'2024-12-31 22:52:34'),(39,1,6,'d143547a-22ea-4582-b31e-3f202313792b',1,'Answer 2 for page LearningUser Subtopic 2-1',0,11678,0,'2024-12-31 16:16:01');
/*!40000 ALTER TABLE `answer` ENABLE KEYS */;
UNLOCK TABLES;

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
  `AnswerTo` int DEFAULT NULL,
  `ShouldImprove` tinyint(1) DEFAULT NULL,
  `ShouldRemove` tinyint(1) DEFAULT NULL,
  `ShouldKeys` varchar(255) DEFAULT NULL,
  `IsSettled` tinyint(1) DEFAULT NULL,
  `Creator_id` int DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Text` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `AnswerTo` (`AnswerTo`),
  KEY `Creator_id` (`Creator_id`),
  CONSTRAINT `FK_A2E42DA9` FOREIGN KEY (`Creator_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_EC7E49AE` FOREIGN KEY (`AnswerTo`) REFERENCES `comment` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comment`
--

LOCK TABLES `comment` WRITE;
/*!40000 ALTER TABLE `comment` DISABLE KEYS */;
/*!40000 ALTER TABLE `comment` ENABLE KEYS */;
UNLOCK TABLES;

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
  `Author` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Markup` varchar(255) DEFAULT NULL,
  `MarkupDownloadDate` datetime DEFAULT NULL,
  `ManualEntries` varchar(255) DEFAULT NULL,
  `MainLicenseInfo` varchar(255) DEFAULT NULL,
  `AllRegisteredLicenses` varchar(255) DEFAULT NULL,
  `Notifications` varchar(255) DEFAULT NULL,
  `LicenseState` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagemetadata`
--

LOCK TABLES `imagemetadata` WRITE;
/*!40000 ALTER TABLE `imagemetadata` DISABLE KEYS */;
/*!40000 ALTER TABLE `imagemetadata` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `jobqueue`
--

DROP TABLE IF EXISTS `jobqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobqueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobQueueType` int DEFAULT NULL,
  `JobContent` varchar(255) DEFAULT NULL,
  `Priority` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jobqueue`
--

LOCK TABLES `jobqueue` WRITE;
/*!40000 ALTER TABLE `jobqueue` DISABLE KEYS */;
INSERT INTO `jobqueue` VALUES (1,1,'1',0,'2026-01-03 18:33:15'),(2,1,'1',0,'2026-01-03 18:33:15'),(3,1,'1',0,'2026-01-03 18:33:16'),(4,1,'1',0,'2026-01-03 18:33:16'),(5,1,'1',0,'2026-01-03 18:33:16'),(6,1,'1',0,'2026-01-03 18:33:16');
/*!40000 ALTER TABLE `jobqueue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `message`
--

DROP TABLE IF EXISTS `message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `message` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReceiverId` int DEFAULT NULL,
  `Subject` varchar(255) DEFAULT NULL,
  `Body` varchar(255) DEFAULT NULL,
  `MessageType` varchar(255) DEFAULT NULL,
  `IsRead` tinyint(1) DEFAULT NULL,
  `WasSendPerEmail` tinyint(1) DEFAULT NULL,
  `WasSendToSmartphone` tinyint(1) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `message`
--

LOCK TABLES `message` WRITE;
/*!40000 ALTER TABLE `message` DISABLE KEYS */;
/*!40000 ALTER TABLE `message` ENABLE KEYS */;
UNLOCK TABLES;

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
  KEY `User_id` (`User_id`),
  CONSTRAINT `FK_B048BCC5` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `messageemail`
--

LOCK TABLES `messageemail` WRITE;
/*!40000 ALTER TABLE `messageemail` DISABLE KEYS */;
/*!40000 ALTER TABLE `messageemail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `page`
--

DROP TABLE IF EXISTS `page`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `page` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `WikipediaURL` varchar(255) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `UrlLinkText` varchar(255) DEFAULT NULL,
  `DisableLearningFunctions` tinyint(1) DEFAULT NULL,
  `Creator_id` int DEFAULT NULL,
  `TopicMarkdown` varchar(255) DEFAULT NULL,
  `Content` mediumtext,
  `CustomSegments` varchar(255) DEFAULT NULL,
  `CountQuestionsAggregated` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT NULL,
  `CorrectnessProbabilityAnswerCount` int DEFAULT NULL,
  `TotalRelevancePersonalEntries` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `SkipMigration` tinyint(1) DEFAULT NULL,
  `Visibility` int DEFAULT NULL,
  `IsWiki` tinyint(1) DEFAULT NULL,
  `TextIsHidden` tinyint(1) DEFAULT NULL,
  `AuthorIds` varchar(255) DEFAULT NULL,
  `Language` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Creator_id` (`Creator_id`),
  CONSTRAINT `FK_34B9BBFB` FOREIGN KEY (`Creator_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `page`
--

LOCK TABLES `page` WRITE;
/*!40000 ALTER TABLE `page` DISABLE KEYS */;
INSERT INTO `page` VALUES (1,'LearningUser Topic 1',NULL,NULL,NULL,NULL,0,1,NULL,NULL,NULL,1,0,0,0,'2026-01-03 18:33:15','2026-01-03 18:33:15',0,0,1,0,'1','en'),(2,'LearningUser Subtopic 1-1',NULL,NULL,NULL,NULL,0,1,NULL,NULL,NULL,1,0,0,0,'2026-01-03 18:33:15','2026-01-03 18:33:16',0,0,0,0,'1','en'),(3,'LearningUser Subtopic 2-1',NULL,NULL,NULL,NULL,0,1,NULL,NULL,NULL,1,0,0,0,'2026-01-03 18:33:15','2026-01-03 18:33:16',0,0,0,0,'1','en');
/*!40000 ALTER TABLE `page` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pagechange`
--

DROP TABLE IF EXISTS `pagechange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagechange` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Page_id` int DEFAULT NULL,
  `Data` longtext,
  `ShowInSidebar` tinyint(1) DEFAULT NULL,
  `DataVersion` int DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `Author_id` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagechange`
--

LOCK TABLES `pagechange` WRITE;
/*!40000 ALTER TABLE `pagechange` DISABLE KEYS */;
INSERT INTO `pagechange` VALUES (1,1,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Topic 1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,0,1,'2026-01-03 18:33:15'),(2,1,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Topic 1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15'),(3,2,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 1-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,0,1,'2026-01-03 18:33:15'),(4,2,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 1-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,1,'2026-01-03 18:33:15'),(5,1,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Topic 1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2],\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,1,'2026-01-03 18:33:15'),(6,1,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Topic 1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2],\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15'),(7,2,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 1-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15'),(8,3,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 2-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,0,1,'2026-01-03 18:33:15'),(9,3,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 2-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[2],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,1,'2026-01-03 18:33:15'),(10,2,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 1-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":[3],\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,1,'2026-01-03 18:33:15'),(11,1,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Topic 1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2],\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15'),(12,2,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 1-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":[3],\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15'),(13,3,'{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"LearningUser Subtopic 2-1\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[2],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',1,2,7,49,'2026-01-03 18:33:15');
/*!40000 ALTER TABLE `pagechange` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pagerelation`
--

DROP TABLE IF EXISTS `pagerelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagerelation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Page_id` int DEFAULT NULL,
  `Related_id` int DEFAULT NULL,
  `Previous_id` int DEFAULT NULL,
  `Next_id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Page_id` (`Page_id`),
  KEY `Related_id` (`Related_id`),
  CONSTRAINT `FK_2EA216B` FOREIGN KEY (`Related_id`) REFERENCES `page` (`Id`),
  CONSTRAINT `FK_5E3C647C` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagerelation`
--

LOCK TABLES `pagerelation` WRITE;
/*!40000 ALTER TABLE `pagerelation` DISABLE KEYS */;
INSERT INTO `pagerelation` VALUES (1,2,1,NULL,NULL),(2,3,2,NULL,NULL);
/*!40000 ALTER TABLE `pagerelation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pages_to_questions`
--

DROP TABLE IF EXISTS `pages_to_questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pages_to_questions` (
  `Question_id` int NOT NULL,
  `Page_id` int NOT NULL,
  KEY `Page_id` (`Page_id`),
  KEY `Question_id` (`Question_id`),
  CONSTRAINT `FK_3DD43A7C` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`),
  CONSTRAINT `FK_B7E93C59` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pages_to_questions`
--

LOCK TABLES `pages_to_questions` WRITE;
/*!40000 ALTER TABLE `pages_to_questions` DISABLE KEYS */;
INSERT INTO `pages_to_questions` VALUES (1,1),(2,1),(3,2),(4,2),(5,3),(6,3);
/*!40000 ALTER TABLE `pages_to_questions` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagevaluation`
--

LOCK TABLES `pagevaluation` WRITE;
/*!40000 ALTER TABLE `pagevaluation` DISABLE KEYS */;
/*!40000 ALTER TABLE `pagevaluation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pageview`
--

DROP TABLE IF EXISTS `pageview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pageview` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Page_id` int DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `UserAgent` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateOnly` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Page_id` (`Page_id`),
  KEY `User_id` (`User_id`),
  CONSTRAINT `FK_1ABE64FC` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`),
  CONSTRAINT `FK_F7F3F28F` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pageview`
--

LOCK TABLES `pageview` WRITE;
/*!40000 ALTER TABLE `pageview` DISABLE KEYS */;
/*!40000 ALTER TABLE `pageview` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `passwordrecoverytoken`
--

LOCK TABLES `passwordrecoverytoken` WRITE;
/*!40000 ALTER TABLE `passwordrecoverytoken` DISABLE KEYS */;
/*!40000 ALTER TABLE `passwordrecoverytoken` ENABLE KEYS */;
UNLOCK TABLES;

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
  `Created` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `persistentlogin`
--

LOCK TABLES `persistentlogin` WRITE;
/*!40000 ALTER TABLE `persistentlogin` DISABLE KEYS */;
/*!40000 ALTER TABLE `persistentlogin` ENABLE KEYS */;
UNLOCK TABLES;

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
  `TotalTrueAnswers` int DEFAULT NULL,
  `TotalFalseAnswers` int DEFAULT NULL,
  `TotalQualityAvg` int DEFAULT NULL,
  `TotalQualityEntries` int DEFAULT NULL,
  `TotalRelevanceForAllAvg` int DEFAULT NULL,
  `TotalRelevanceForAllEntries` int DEFAULT NULL,
  `TotalRelevancePersonalAvg` int DEFAULT NULL,
  `TotalRelevancePersonalEntries` int DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT NULL,
  `CorrectnessProbabilityAnswerCount` int DEFAULT NULL,
  `Solution` mediumtext,
  `SolutionType` int DEFAULT NULL,
  `SolutionMetadataJson` varchar(7000) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `SkipMigration` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Creator_id` (`Creator_id`),
  CONSTRAINT `FK_A42E80A6` FOREIGN KEY (`Creator_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `question`
--

LOCK TABLES `question` WRITE;
/*!40000 ALTER TABLE `question` DISABLE KEYS */;
INSERT INTO `question` VALUES (1,'Question 1 for page LearningUser Topic 1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,77,0,'Answer 1 for page LearningUser Topic 1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:15','2026-01-03 18:33:15',0),(2,'Question 2 for page LearningUser Topic 1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,61,0,'Answer 2 for page LearningUser Topic 1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:15','2026-01-03 18:33:15',0),(3,'Question 1 for page LearningUser Subtopic 1-1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,78,0,'Answer 1 for page LearningUser Subtopic 1-1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:16','2026-01-03 18:33:16',0),(4,'Question 2 for page LearningUser Subtopic 1-1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,80,0,'Answer 2 for page LearningUser Subtopic 1-1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:16','2026-01-03 18:33:16',0),(5,'Question 1 for page LearningUser Subtopic 2-1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,40,0,'Answer 1 for page LearningUser Subtopic 2-1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:16','2026-01-03 18:33:16',0),(6,'Question 2 for page LearningUser Subtopic 2-1?',NULL,NULL,NULL,NULL,NULL,0,0,1,0,0,0,0,0,0,0,0,74,0,'Answer 2 for page LearningUser Subtopic 2-1',1,'{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}','2026-01-03 18:33:16','2026-01-03 18:33:16',0);
/*!40000 ALTER TABLE `question` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionchange`
--

DROP TABLE IF EXISTS `questionchange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionchange` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Question_id` int DEFAULT NULL,
  `Data` text,
  `ShowInSidebar` tinyint(1) DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `DataVersion` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `Author_Id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Question_id` (`Question_id`),
  CONSTRAINT `FK_4947E040` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionchange`
--

LOCK TABLES `questionchange` WRITE;
/*!40000 ALTER TABLE `questionchange` DISABLE KEYS */;
INSERT INTO `questionchange` VALUES (1,1,'{\"QuestionText\":\"Question 1 for page LearningUser Topic 1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 1 for page LearningUser Topic 1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:15',1),(2,2,'{\"QuestionText\":\"Question 2 for page LearningUser Topic 1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 2 for page LearningUser Topic 1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:15',1),(3,3,'{\"QuestionText\":\"Question 1 for page LearningUser Subtopic 1-1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 1 for page LearningUser Subtopic 1-1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:16',1),(4,4,'{\"QuestionText\":\"Question 2 for page LearningUser Subtopic 1-1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 2 for page LearningUser Subtopic 1-1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:16',1),(5,5,'{\"QuestionText\":\"Question 1 for page LearningUser Subtopic 2-1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 1 for page LearningUser Subtopic 2-1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:16',1),(6,6,'{\"QuestionText\":\"Question 2 for page LearningUser Subtopic 2-1?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 2 for page LearningUser Subtopic 2-1\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',1,0,1,'2026-01-03 18:33:16',1);
/*!40000 ALTER TABLE `questionchange` ENABLE KEYS */;
UNLOCK TABLES;

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
  `CorrectnessProbabilityAnswerCount` int DEFAULT NULL,
  `KnowledgeStatus` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  KEY `QuestionId` (`QuestionId`),
  CONSTRAINT `FK_6431BD4D` FOREIGN KEY (`QuestionId`) REFERENCES `question` (`Id`),
  CONSTRAINT `FK_67878C2E` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionvaluation`
--

LOCK TABLES `questionvaluation` WRITE;
/*!40000 ALTER TABLE `questionvaluation` DISABLE KEYS */;
/*!40000 ALTER TABLE `questionvaluation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionview`
--

DROP TABLE IF EXISTS `questionview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionview` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Guid` varchar(36) DEFAULT NULL,
  `QuestionId` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Milliseconds` int DEFAULT NULL,
  `UserAgent` varchar(255) DEFAULT NULL,
  `Migrated` tinyint(1) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateOnly` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Guid` (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionview`
--

LOCK TABLES `questionview` WRITE;
/*!40000 ALTER TABLE `questionview` DISABLE KEYS */;
/*!40000 ALTER TABLE `questionview` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reference`
--

DROP TABLE IF EXISTS `reference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reference` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Question_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `ReferenceType` int DEFAULT NULL,
  `AdditionalInfo` varchar(255) DEFAULT NULL,
  `ReferenceText` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Question_id` (`Question_id`),
  KEY `Page_id` (`Page_id`),
  CONSTRAINT `FK_4486FA46` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`),
  CONSTRAINT `FK_71B21AE6` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
/*!40000 ALTER TABLE `reference` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `runningjob`
--

DROP TABLE IF EXISTS `runningjob`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `runningjob` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StartAt` datetime DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `runningjob`
--

LOCK TABLES `runningjob` WRITE;
/*!40000 ALTER TABLE `runningjob` DISABLE KEYS */;
/*!40000 ALTER TABLE `runningjob` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schemaversion`
--

DROP TABLE IF EXISTS `schemaversion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schemaversion` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SchemaHash` varchar(64) DEFAULT NULL,
  `LastUpdated` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schemaversion`
--

LOCK TABLES `schemaversion` WRITE;
/*!40000 ALTER TABLE `schemaversion` DISABLE KEYS */;
INSERT INTO `schemaversion` VALUES (1,'66f3c02958c2faf438428b434493d96d6119282841e769b123b8e5c018d145c6','2026-01-03 17:33:14');
/*!40000 ALTER TABLE `schemaversion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `setting` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppVersion` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `setting`
--

LOCK TABLES `setting` WRITE;
/*!40000 ALTER TABLE `setting` DISABLE KEYS */;
INSERT INTO `setting` VALUES (1,2147483647,'2026-01-03 18:33:14','2026-01-03 18:33:14');
/*!40000 ALTER TABLE `setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shares`
--

DROP TABLE IF EXISTS `shares`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shares` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PageId` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Token` varchar(255) NOT NULL,
  `Permission` int DEFAULT NULL,
  `GrantedBy` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `FK_B8E3B644` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shares`
--

LOCK TABLES `shares` WRITE;
/*!40000 ALTER TABLE `shares` DISABLE KEYS */;
/*!40000 ALTER TABLE `shares` ENABLE KEYS */;
UNLOCK TABLES;

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
  `BouncedMail` tinyint(1) DEFAULT NULL,
  `MailBounceReason` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsEmailConfirmed` tinyint(1) DEFAULT NULL,
  `IsInstallationAdmin` tinyint(1) DEFAULT NULL,
  `AllowsSupportiveLogin` tinyint(1) DEFAULT NULL,
  `ShowWishKnowledge` tinyint(1) DEFAULT NULL,
  `KnowledgeReportInterval` int DEFAULT NULL,
  `TotalInOthersWishKnowledge` int DEFAULT NULL,
  `FollowerCount` int DEFAULT NULL,
  `LearningSessionOptions` varchar(255) DEFAULT NULL,
  `StripeId` varchar(255) DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `SubscriptionStartDate` datetime DEFAULT NULL,
  `CorrectnessProbability` int DEFAULT NULL,
  `CorrectnessProbabilityAnswerCount` int DEFAULT NULL,
  `WidgetHostsSpaceSeparated` varchar(255) DEFAULT NULL,
  `Reputation` int DEFAULT NULL,
  `ReputationPos` int DEFAULT NULL,
  `WishCountQuestions` int DEFAULT NULL,
  `WishCountSets` int DEFAULT NULL,
  `Birthday` datetime DEFAULT NULL,
  `FacebookId` varchar(255) DEFAULT NULL,
  `GoogleId` varchar(255) DEFAULT NULL,
  `ActivityPoints` int DEFAULT NULL,
  `ActivityLevel` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `RecentlyUsedRelationTargetPages` varchar(255) DEFAULT NULL,
  `FavoriteIds` varchar(255) DEFAULT NULL,
  `UiLanguage` varchar(255) DEFAULT NULL,
  `SubscriptionTokensBalance` int DEFAULT NULL,
  `PaidTokensBalance` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'13c0736b810a8d194dbcca380fe5a4ce','9b29debd-c384-451d-a366-25e3aa67a7a4','learning.user@example.com',0,NULL,'LearningUser',1,0,0,0,0,0,0,NULL,NULL,NULL,NULL,0,0,NULL,0,0,0,0,NULL,NULL,NULL,0,0,'2026-01-03 18:33:15','2026-01-03 18:33:15',NULL,NULL,'en',0,0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_to_follower`
--

DROP TABLE IF EXISTS `user_to_follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_to_follower` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Follower_id` int DEFAULT NULL,
  `User_id` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Follower_id` (`Follower_id`),
  KEY `User_id` (`User_id`),
  CONSTRAINT `FK_28BA5F5E` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_4FFDA6DE` FOREIGN KEY (`Follower_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_to_follower`
--

LOCK TABLES `user_to_follower` WRITE;
/*!40000 ALTER TABLE `user_to_follower` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_to_follower` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useractivity`
--

DROP TABLE IF EXISTS `useractivity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `useractivity` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserConcerned_id` int DEFAULT NULL,
  `At` datetime DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `Question_id` int DEFAULT NULL,
  `Page_id` int DEFAULT NULL,
  `UserIsFollowed_id` int DEFAULT NULL,
  `UserCauser_id` int DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserConcerned_id` (`UserConcerned_id`),
  KEY `Question_id` (`Question_id`),
  KEY `Page_id` (`Page_id`),
  KEY `UserIsFollowed_id` (`UserIsFollowed_id`),
  KEY `UserCauser_id` (`UserCauser_id`),
  CONSTRAINT `FK_5137F47B` FOREIGN KEY (`UserConcerned_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_5D4CBA5D` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`),
  CONSTRAINT `FK_6C836DBA` FOREIGN KEY (`UserIsFollowed_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_8091412B` FOREIGN KEY (`UserCauser_id`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_B3C89A36` FOREIGN KEY (`Page_id`) REFERENCES `page` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useractivity`
--

LOCK TABLES `useractivity` WRITE;
/*!40000 ALTER TABLE `useractivity` DISABLE KEYS */;
/*!40000 ALTER TABLE `useractivity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userskill`
--

DROP TABLE IF EXISTS `userskill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userskill` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `PageId` int DEFAULT NULL,
  `EvaluationJson` text,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userskill`
--

LOCK TABLES `userskill` WRITE;
/*!40000 ALTER TABLE `userskill` DISABLE KEYS */;
/*!40000 ALTER TABLE `userskill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'memoWikis_dev'
--

--
-- Dumping routines for database 'memoWikis_dev'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-01-03 17:33:19

