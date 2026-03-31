-- MySQL dump 10.13  Distrib 8.3.0, for Linux (x86_64)
--
-- Host: localhost    Database: memoWikis_dev
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */
;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */
;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */
;
/*!50503 SET NAMES utf8mb4 */
;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */
;
/*!40103 SET TIME_ZONE='+00:00' */
;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */
;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */
;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */
;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */
;

--
-- Current Database: `memoWikis_dev`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `memoWikis_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `memoWikis_dev`;

--
-- Table structure for table `activitypoints`
--

DROP TABLE IF EXISTS `activitypoints`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `activitypoints` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Amount` int DEFAULT NULL,
    `DateEarned` datetime DEFAULT NULL,
    `User_id` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `activitypoints`
--

LOCK TABLES `activitypoints` WRITE;
/*!40000 ALTER TABLE `activitypoints` DISABLE KEYS */
;
/*!40000 ALTER TABLE `activitypoints` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `ai_usage_log`
--

DROP TABLE IF EXISTS `ai_usage_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `ai_usage_log` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `User_id` int DEFAULT NULL,
    `Page_id` int DEFAULT NULL,
    `TokenIn` int DEFAULT NULL,
    `TokenOut` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `Model` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `ai_usage_log`
--

LOCK TABLES `ai_usage_log` WRITE;
/*!40000 ALTER TABLE `ai_usage_log` DISABLE KEYS */
;
/*!40000 ALTER TABLE `ai_usage_log` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `aimodelwhitelist`
--

DROP TABLE IF EXISTS `aimodelwhitelist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `aimodelwhitelist` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ModelId` varchar(100) NOT NULL,
    `DisplayName` varchar(200) DEFAULT NULL,
    `Provider` int DEFAULT NULL,
    `TokenCostMultiplier` decimal(10, 2) DEFAULT NULL,
    `InputPricePerMillion` decimal(10, 4) DEFAULT NULL,
    `OutputPricePerMillion` decimal(10, 4) DEFAULT NULL,
    `IsEnabled` tinyint(1) DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 5 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `aimodelwhitelist`
--

LOCK TABLES `aimodelwhitelist` WRITE;
/*!40000 ALTER TABLE `aimodelwhitelist` DISABLE KEYS */
;
INSERT INTO
    `aimodelwhitelist`
VALUES (
        1,
        'claude-opus-4-5-20251101',
        'Claude Opus 4.5',
        0,
        3.00,
        5.0000,
        25.0000,
        1
    ),
    (
        2,
        'claude-haiku-4-5-20251001',
        'Claude Haiku 4.5',
        0,
        0.33,
        1.0000,
        5.0000,
        1
    ),
    (
        3,
        'claude-sonnet-4-5-20250929',
        'Claude Sonnet 4.5',
        0,
        1.00,
        3.0000,
        15.0000,
        1
    ),
    (
        4,
        'gpt-5.2',
        'gpt-5.2',
        1,
        0.80,
        175.0000,
        14.0000,
        1
    );
/*!40000 ALTER TABLE `aimodelwhitelist` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `answer`
--

DROP TABLE IF EXISTS `answer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `QuestionId` (`QuestionId`)
) ENGINE = InnoDB AUTO_INCREMENT = 58 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `answer`
--

LOCK TABLES `answer` WRITE;
/*!40000 ALTER TABLE `answer` DISABLE KEYS */
;
INSERT INTO
    `answer`
VALUES (
        1,
        1,
        39,
        'b43743c7-54a0-4362-be2d-d5da546067cd',
        1,
        'Partial',
        1,
        9569,
        0,
        '2024-12-11 17:27:51'
    ),
    (
        2,
        1,
        27,
        'b1cc8a62-16bf-4864-8736-8a405a349c2b',
        1,
        'Ivan Pavlov',
        1,
        10830,
        0,
        '2024-12-11 14:40:53'
    ),
    (
        3,
        1,
        20,
        'e374af67-7138-46bf-b2cb-11a437a07a58',
        1,
        'Thirty Years\' War',
        1,
        13716,
        0,
        '2024-12-11 14:25:56'
    ),
    (
        4,
        1,
        24,
        '44ec8366-44b2-4a54-9f61-3bce4b42a03e',
        1,
        'Aristotle',
        1,
        10999,
        0,
        '2024-12-11 14:18:50'
    ),
    (
        5,
        1,
        5,
        'd2c608ab-27c8-4ebc-b39a-e81676dd50b6',
        1,
        'Fighting imaginary problems',
        1,
        5354,
        0,
        '2024-12-11 13:01:30'
    ),
    (
        6,
        1,
        11,
        'a1e5f751-c651-44e7-9cde-2e66360b7bf3',
        1,
        'Private ownership of means of production, free markets, profit-driven enterprise',
        1,
        4820,
        0,
        '2024-12-11 09:29:50'
    ),
    (
        7,
        1,
        38,
        '011bf9b1-42a1-49cd-9ce9-cb397314ba03',
        1,
        'keyof',
        0,
        4551,
        0,
        '2024-12-14 12:14:45'
    ),
    (
        8,
        1,
        39,
        'd3d7583e-544d-43e3-8946-f61b0fd5ec92',
        1,
        'Partial',
        1,
        4375,
        0,
        '2024-12-14 21:37:40'
    ),
    (
        9,
        1,
        23,
        'fd9d638a-69ba-4bb8-bbdd-cc2849e5539c',
        1,
        'Paradigm Shifts, Methodological Progress, Cultural Influence, Ongoing Debates',
        0,
        11595,
        0,
        '2024-12-14 14:07:01'
    ),
    (
        10,
        1,
        11,
        '811d8ecc-4678-444f-a05d-7a9add9a4bda',
        1,
        'Private ownership of means of production, free markets, profit-driven enterprise',
        1,
        13588,
        0,
        '2024-12-14 09:51:48'
    ),
    (
        11,
        1,
        12,
        '9ee2485b-7a4b-4bfb-8c0d-f6305ce14bda',
        1,
        'Adam Smith, 1776',
        1,
        3600,
        0,
        '2024-12-14 17:26:31'
    ),
    (
        12,
        1,
        30,
        'd323b11e-9f21-4384-a1cf-6db9da806fcd',
        1,
        'Square brackets, e.g., [id].vue',
        1,
        7248,
        0,
        '2024-12-14 17:06:07'
    ),
    (
        13,
        1,
        36,
        '72c9a4aa-ca59-4fe1-ac79-f534e5f97462',
        1,
        'unknown',
        1,
        6136,
        0,
        '2024-12-15 13:37:40'
    ),
    (
        14,
        1,
        12,
        'f26d2620-e9d6-4e43-9926-38f3a730f52f',
        1,
        'Incorrect answer',
        1,
        4851,
        0,
        '2024-12-15 09:37:03'
    ),
    (
        15,
        1,
        30,
        'b0257bd0-028a-4f8d-803d-f1d5ed2418ce',
        1,
        'Square brackets, e.g., [id].vue',
        0,
        10845,
        0,
        '2024-12-15 22:05:31'
    ),
    (
        16,
        1,
        24,
        '186832c1-53b2-44e6-9246-adffecee7b70',
        1,
        'Aristotle',
        0,
        14567,
        0,
        '2024-12-15 21:53:12'
    ),
    (
        17,
        1,
        39,
        '6609e785-f5af-4676-95ab-b7db8dcfa33c',
        1,
        'Partial',
        1,
        1640,
        0,
        '2024-12-15 13:23:13'
    ),
    (
        18,
        1,
        31,
        '7eb32d7c-020e-40b7-b8e1-1b8feb0f05b2',
        1,
        'Hydration',
        1,
        14762,
        0,
        '2024-12-15 09:10:06'
    ),
    (
        19,
        1,
        27,
        '0b6f3d92-6faf-4448-b34b-62db8f8725e9',
        1,
        'Ivan Pavlov',
        1,
        2577,
        0,
        '2024-12-16 18:58:18'
    ),
    (
        20,
        1,
        20,
        'a721aa2e-2437-49a7-a51a-13b56525d1a2',
        1,
        'Incorrect answer',
        1,
        8114,
        0,
        '2024-12-16 08:11:47'
    ),
    (
        21,
        1,
        29,
        '48631540-d8ec-4a07-af28-183728071a2c',
        1,
        'Universal (SSR), SPA, Static (SSG), Hybrid',
        0,
        9646,
        0,
        '2024-12-16 12:07:45'
    ),
    (
        22,
        1,
        16,
        '655106a4-9c19-4101-8e4c-d39348056675',
        1,
        'Incorrect answer',
        1,
        7764,
        0,
        '2024-12-16 21:34:56'
    ),
    (
        23,
        1,
        8,
        '015e2528-5c80-4757-828a-2fabb132d79a',
        1,
        'Incorrect answer',
        1,
        3964,
        0,
        '2024-12-16 12:01:45'
    ),
    (
        24,
        1,
        31,
        '417b2cf8-f96c-4e53-9b8e-85081ff79c45',
        1,
        'Hydration',
        1,
        8599,
        0,
        '2024-12-17 15:59:47'
    ),
    (
        25,
        1,
        11,
        '520f3d0a-10bb-4f36-864d-6c28ed64ca55',
        1,
        'Incorrect answer',
        1,
        8449,
        0,
        '2024-12-17 19:50:00'
    ),
    (
        26,
        1,
        23,
        'de8da0af-809d-48dc-900a-ee37fbb129d6',
        1,
        'Paradigm Shifts, Methodological Progress, Cultural Influence, Ongoing Debates',
        1,
        12915,
        0,
        '2024-12-17 08:52:52'
    ),
    (
        27,
        1,
        39,
        '8dec9529-afde-4779-b040-6d88c0ce2ed5',
        1,
        'Incorrect answer',
        1,
        2509,
        0,
        '2024-12-17 11:39:49'
    ),
    (
        28,
        1,
        14,
        '19405d63-353e-48d6-b6a7-8d2dcbc3bd4c',
        1,
        'Karl Marx',
        1,
        11149,
        0,
        '2024-12-17 21:45:54'
    ),
    (
        29,
        1,
        20,
        'cb9b1a46-bfe3-42f5-9523-dd2a10931e75',
        1,
        'Thirty Years\' War',
        1,
        10625,
        0,
        '2024-12-17 12:08:24'
    ),
    (
        30,
        1,
        13,
        '8fe0b0c0-ba68-4b4e-8b17-3ad47a4416f2',
        1,
        'Dutch East India Company',
        1,
        3329,
        0,
        '2024-12-18 17:37:26'
    ),
    (
        31,
        1,
        31,
        '25f5d065-ee2e-4c1b-a0a0-a5b9a49713eb',
        1,
        'Hydration',
        1,
        12924,
        0,
        '2024-12-18 17:12:40'
    ),
    (
        32,
        1,
        39,
        '84bc919e-2378-4988-bc53-9035ad63e324',
        1,
        'Partial',
        1,
        8569,
        0,
        '2024-12-18 19:42:25'
    ),
    (
        33,
        1,
        8,
        '3bda5a2b-9e1f-43f8-a8fa-a1d240a3632a',
        1,
        'Incorrect answer',
        1,
        13685,
        0,
        '2024-12-23 21:12:17'
    ),
    (
        34,
        1,
        10,
        '5d3bb8e9-27a9-486c-8bbe-d7602bdcfec7',
        1,
        'Referencing something mentioned earlier in the conversation',
        1,
        10146,
        0,
        '2024-12-23 13:13:35'
    ),
    (
        35,
        1,
        5,
        '054d3bff-b45d-4814-8ed0-02df7ddf3414',
        1,
        'Incorrect answer',
        1,
        12028,
        0,
        '2024-12-23 14:05:09'
    ),
    (
        36,
        1,
        11,
        'b28861f4-3871-48c4-a1b5-fbc6b916700f',
        1,
        'Private ownership of means of production, free markets, profit-driven enterprise',
        1,
        3910,
        0,
        '2024-12-25 17:25:32'
    ),
    (
        37,
        1,
        12,
        'd86e7acd-9708-457e-a432-6878b1c59903',
        1,
        'Adam Smith, 1776',
        1,
        8968,
        0,
        '2024-12-25 11:03:18'
    ),
    (
        38,
        1,
        23,
        'aaa91368-7526-483b-b4f3-00e1da5756e4',
        1,
        'Paradigm Shifts, Methodological Progress, Cultural Influence, Ongoing Debates',
        0,
        14411,
        0,
        '2024-12-25 09:52:54'
    ),
    (
        39,
        1,
        8,
        '961784d3-2bd0-409a-9b58-cadf2cfb3628',
        1,
        'Answer 2 for page Conversation Starters That Work',
        1,
        7936,
        0,
        '2024-12-25 10:32:34'
    ),
    (
        40,
        1,
        36,
        '493c6463-35bf-4322-a8cd-af4ff0e1fadb',
        1,
        'unknown',
        1,
        9305,
        0,
        '2024-12-27 17:46:24'
    ),
    (
        41,
        1,
        24,
        'fca4fc3f-cb75-48c8-9db9-09d70d1ffe09',
        1,
        'Aristotle',
        1,
        10366,
        0,
        '2024-12-27 11:53:11'
    ),
    (
        42,
        1,
        11,
        'a6562c9a-6943-48ba-96c5-193402a85f43',
        1,
        'Private ownership of means of production, free markets, profit-driven enterprise',
        0,
        1675,
        0,
        '2024-12-27 09:47:49'
    ),
    (
        43,
        1,
        5,
        '2a4a9413-f1a6-4fd1-8882-edeb173757d0',
        1,
        'Incorrect answer',
        1,
        5280,
        0,
        '2024-12-27 15:26:28'
    ),
    (
        44,
        1,
        28,
        '0c585390-2bcb-4dc1-955f-883a6023c133',
        1,
        'Ulric Neisser',
        1,
        1418,
        0,
        '2024-12-27 22:14:18'
    ),
    (
        45,
        1,
        8,
        '0a1b5dcc-085b-4d7c-968b-35ba5f40bcaf',
        1,
        'Answer 2 for page Conversation Starters That Work',
        0,
        2510,
        0,
        '2024-12-28 14:06:00'
    ),
    (
        46,
        1,
        27,
        'c7d2c429-18aa-4e21-9278-7b3e9bebdaa6',
        1,
        'Ivan Pavlov',
        1,
        14135,
        0,
        '2024-12-28 12:37:00'
    ),
    (
        47,
        1,
        39,
        'fd797cd8-e6ab-4360-a418-2af558ea6738',
        1,
        'Partial',
        0,
        4468,
        0,
        '2024-12-28 19:21:14'
    ),
    (
        48,
        1,
        30,
        '3f3b7564-2c6f-4c26-865a-b853f7433b84',
        1,
        'Incorrect answer',
        1,
        12426,
        0,
        '2024-12-28 20:04:43'
    ),
    (
        49,
        1,
        29,
        '85ee173e-bbc0-4a90-bdae-00579f9bba68',
        1,
        'Universal (SSR), SPA, Static (SSG), Hybrid',
        1,
        5749,
        0,
        '2024-12-30 14:17:11'
    ),
    (
        50,
        1,
        5,
        '04438ecb-27b0-44be-972f-0d721de8056d',
        1,
        'Fighting imaginary problems',
        1,
        11185,
        0,
        '2024-12-30 21:08:32'
    ),
    (
        51,
        1,
        20,
        '285c902c-e839-4b2d-8a86-58cc3591e1b9',
        1,
        'Thirty Years\' War',
        1,
        8414,
        0,
        '2024-12-30 14:28:04'
    ),
    (
        52,
        1,
        14,
        '3b9c4792-ad65-4eea-8dcd-e6f000387248',
        1,
        'Incorrect answer',
        1,
        8111,
        0,
        '2024-12-30 14:33:07'
    ),
    (
        53,
        1,
        12,
        'd7bdbfab-d8cf-45fc-8927-69888ddd3263',
        1,
        'Adam Smith, 1776',
        1,
        7997,
        0,
        '2024-12-30 14:24:28'
    ),
    (
        54,
        1,
        16,
        '1058064e-baf0-464f-8664-5f19791eaf1e',
        1,
        'Reagan and Thatcher',
        0,
        12873,
        0,
        '2024-12-30 13:49:12'
    ),
    (
        55,
        1,
        12,
        'b037d71c-3842-4761-856c-bf935bcbaa0c',
        1,
        'Adam Smith, 1776',
        1,
        13979,
        0,
        '2024-12-31 18:53:11'
    ),
    (
        56,
        1,
        14,
        '6e77ea99-e554-47a8-92e6-82a21ebdc7bd',
        1,
        'Karl Marx',
        1,
        1473,
        0,
        '2024-12-31 10:20:48'
    ),
    (
        57,
        1,
        38,
        '83ce34e0-40dc-4362-a962-522de7bbffb0',
        1,
        'keyof',
        1,
        14451,
        0,
        '2024-12-31 10:21:31'
    );
/*!40000 ALTER TABLE `answer` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `comment`
--

DROP TABLE IF EXISTS `comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `Creator_id` (`Creator_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `comment`
--

LOCK TABLES `comment` WRITE;
/*!40000 ALTER TABLE `comment` DISABLE KEYS */
;
/*!40000 ALTER TABLE `comment` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `imagemetadata`
--

DROP TABLE IF EXISTS `imagemetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `imagemetadata`
--

LOCK TABLES `imagemetadata` WRITE;
/*!40000 ALTER TABLE `imagemetadata` DISABLE KEYS */
;
/*!40000 ALTER TABLE `imagemetadata` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `jobqueue`
--

DROP TABLE IF EXISTS `jobqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `jobqueue` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `JobQueueType` int DEFAULT NULL,
    `JobContent` varchar(255) DEFAULT NULL,
    `Priority` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 41 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `jobqueue`
--

LOCK TABLES `jobqueue` WRITE;
/*!40000 ALTER TABLE `jobqueue` DISABLE KEYS */
;
INSERT INTO
    `jobqueue`
VALUES (
        1,
        1,
        '1',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        2,
        1,
        '1',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        3,
        1,
        '1',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        4,
        1,
        '2',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        5,
        1,
        '2',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        6,
        1,
        '2',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        7,
        1,
        '2',
        0,
        '2026-01-29 12:12:41'
    ),
    (
        8,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        9,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        10,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        11,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        12,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        13,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        14,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        15,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        16,
        1,
        '2',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        17,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        18,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        19,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        20,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        21,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        22,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        23,
        1,
        '3',
        0,
        '2026-01-29 12:12:42'
    ),
    (
        24,
        1,
        '3',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        25,
        1,
        '3',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        26,
        1,
        '3',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        27,
        1,
        '3',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        28,
        1,
        '3',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        29,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        30,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        31,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        32,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        33,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        34,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        35,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        36,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        37,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        38,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        39,
        1,
        '4',
        0,
        '2026-01-29 12:12:43'
    ),
    (
        40,
        1,
        '4',
        0,
        '2026-01-29 12:12:44'
    );
/*!40000 ALTER TABLE `jobqueue` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `message`
--

DROP TABLE IF EXISTS `message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `message` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ReceiverId` int DEFAULT NULL,
    `Subject` varchar(255) DEFAULT NULL,
    `Body` text,
    `MessageType` varchar(255) DEFAULT NULL,
    `IsRead` tinyint(1) DEFAULT NULL,
    `WasSendPerEmail` tinyint(1) DEFAULT NULL,
    `WasSendToSmartphone` tinyint(1) DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `message`
--

LOCK TABLES `message` WRITE;
/*!40000 ALTER TABLE `message` DISABLE KEYS */
;
/*!40000 ALTER TABLE `message` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `messageemail`
--

DROP TABLE IF EXISTS `messageemail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `messageemail` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `User_id` int DEFAULT NULL,
    `MessageEmailType` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`),
    KEY `User_id` (`User_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `messageemail`
--

LOCK TABLES `messageemail` WRITE;
/*!40000 ALTER TABLE `messageemail` DISABLE KEYS */
;
/*!40000 ALTER TABLE `messageemail` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `page`
--

DROP TABLE IF EXISTS `page`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `Creator_id` (`Creator_id`)
) ENGINE = InnoDB AUTO_INCREMENT = 34 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `page`
--

LOCK TABLES `page` WRITE;
/*!40000 ALTER TABLE `page` DISABLE KEYS */
;
INSERT INTO
    `page`
VALUES (
        1,
        'Welcome to memoWikis',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        1,
        NULL,
        '<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:41',
        0,
        0,
        1,
        0,
        '1',
        'en'
    ),
    (
        2,
        'Creating Your First Wiki',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        1,
        NULL,
        '<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:41',
        0,
        0,
        0,
        0,
        '1',
        'en'
    ),
    (
        3,
        'Learning with Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        1,
        NULL,
        '<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:25',
        '2026-01-29 12:12:41',
        0,
        0,
        0,
        0,
        '1',
        'en'
    ),
    (
        4,
        'What to Know to Be Funny and Informed',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:25',
        '2026-01-29 12:12:41',
        0,
        0,
        1,
        0,
        '2',
        'en'
    ),
    (
        5,
        'Essential Cultural References',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:25',
        '2026-01-29 12:12:41',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        6,
        'Conversation Starters That Work',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:25',
        '2026-01-29 12:12:41',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        7,
        'Classic Anecdotes and Quips',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:26',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        8,
        'The Art of Observation',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:26',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        9,
        'History of Capitalism',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:26',
        '2026-01-29 12:12:42',
        0,
        0,
        1,
        0,
        '2',
        'en'
    ),
    (
        10,
        'Origins and Mercantilism',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:26',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        11,
        'Industrial Capitalism',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:27',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        12,
        'Financial and Global Capitalism',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:27',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        13,
        'Neoliberalism and Today',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        2,
        NULL,
        '<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:28',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '2',
        'en'
    ),
    (
        14,
        'History of the Uckermark and Lychen',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:28',
        '2026-01-29 12:12:42',
        0,
        0,
        1,
        0,
        '3',
        'en'
    ),
    (
        15,
        'Prehistoric and Slavic Period',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:28',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        16,
        'Medieval and Prussian Era',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:29',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        17,
        'Modern History to 1945',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:30',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        18,
        'GDR Era and Reunification',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:30',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        19,
        'History of Psychology',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:31',
        '2026-01-29 12:12:42',
        0,
        0,
        1,
        0,
        '3',
        'en'
    ),
    (
        20,
        'Ancient and Philosophical Roots',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:31',
        '2026-01-29 12:12:42',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        21,
        'Birth of Scientific Psychology',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:32',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        22,
        'Behaviorism and Its Critics',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:32',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        23,
        'Cognitive Revolution to Present',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        3,
        NULL,
        '<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:33',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '3',
        'en'
    ),
    (
        24,
        'Nuxt 3 Interview Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:34',
        '2026-01-29 12:12:43',
        0,
        0,
        1,
        0,
        '4',
        'en'
    ),
    (
        25,
        'Core Concepts Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:34',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        26,
        'SSR and Hydration Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:35',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        27,
        'Composables and State Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\nexport const useCounter = () => {\n  const count = ref(0)\n  const increment = () => count.value++\n  return { count, increment }\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:36',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        28,
        'Advanced Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\nexport default defineNuxtRouteMiddleware((to, from) => {\n  if (!isAuthenticated()) {\n    return navigateTo(\'/login\')\n  }\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:36',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        29,
        'TypeScript Interview Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:37',
        '2026-01-29 12:12:43',
        0,
        0,
        1,
        0,
        '4',
        'en'
    ),
    (
        30,
        'Fundamental Types Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: What is the difference between \'any\' and \'unknown\'?</h2><p><strong>Answer:</strong> Both can hold any value, but <code>unknown</code> is the type-safe counterpart of <code>any</code>. You must perform type checking before using an <code>unknown</code> value, while <code>any</code> bypasses all type checking.</p><pre><code>const a: any = 5;\na.foo(); // No error (but crashes at runtime)\n\nconst b: unknown = 5;\nb.foo(); // Error: Object is of type \'unknown\'\nif (typeof b === \'number\') {\n  console.log(b.toFixed(2)); // OK\n}</code></pre><h2>Q: What is the difference between \'type\' and \'interface\'?</h2><p><strong>Answer:</strong></p><ul><li><strong>Interface:</strong> Can be extended and merged, best for object shapes and class contracts</li><li><strong>Type:</strong> More flexible, supports unions, intersections, mapped types, and primitives</li></ul><pre><code>// Interface merging\ninterface User { name: string }\ninterface User { age: number } // Merged\n\n// Type union (not possible with interface)\ntype Result = Success | Error</code></pre>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:38',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        31,
        'Generics Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: What are generics and why are they useful?</h2><p><strong>Answer:</strong> Generics allow you to write reusable code that works with multiple types while maintaining type safety. They act as type variables that are determined when the function or class is used.</p><pre><code>function identity&lt;T&gt;(arg: T): T {\n  return arg;\n}\n\nconst num = identity&lt;number&gt;(42); // number\nconst str = identity(\'hello\'); // string (inferred)</code></pre><h2>Q: What are generic constraints?</h2><p><strong>Answer:</strong> Constraints limit what types can be used with a generic using the <code>extends</code> keyword:</p><pre><code>interface HasLength {\n  length: number;\n}\n\nfunction logLength&lt;T extends HasLength&gt;(arg: T): void {\n  console.log(arg.length); // Safe to access .length\n}\n\nlogLength(\'hello\'); // OK\nlogLength([1, 2, 3]); // OK\nlogLength(42); // Error: number doesn\'t have length</code></pre><h2>Q: Explain the keyof operator</h2><p><strong>Answer:</strong> <code>keyof</code> produces a union type of all property keys of an object type:</p><pre><code>type Person = { name: string; age: number }\ntype PersonKeys = keyof Person; // \'name\' | \'age\'</code></pre>',
        NULL,
        1,
        0,
        0,
        0,
        '2026-01-29 12:12:39',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        32,
        'Utility Types Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: Explain Partial, Required, and Readonly utility types</h2><p><strong>Answer:</strong></p><ul><li><strong>Partial&lt;T&gt;:</strong> Makes all properties optional</li><li><strong>Required&lt;T&gt;:</strong> Makes all properties required</li><li><strong>Readonly&lt;T&gt;:</strong> Makes all properties read-only</li></ul><pre><code>interface User {\n  name: string;\n  age?: number;\n}\n\ntype PartialUser = Partial&lt;User&gt;; // { name?: string; age?: number }\ntype RequiredUser = Required&lt;User&gt;; // { name: string; age: number }\ntype ReadonlyUser = Readonly&lt;User&gt;; // All properties immutable</code></pre><h2>Q: What is the Pick and Omit utility type?</h2><p><strong>Answer:</strong></p><ul><li><strong>Pick&lt;T, K&gt;:</strong> Creates a type with only the specified keys</li><li><strong>Omit&lt;T, K&gt;:</strong> Creates a type without the specified keys</li></ul><pre><code>interface User { id: number; name: string; email: string }\n\ntype UserPreview = Pick&lt;User, \'id\' | \'name\'&gt;;\ntype UserWithoutEmail = Omit&lt;User, \'email\'&gt;;</code></pre>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:40',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    ),
    (
        33,
        'Advanced Patterns Questions',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        4,
        NULL,
        '<h2>Q: What are type guards and how do you implement them?</h2><p><strong>Answer:</strong> Type guards narrow the type of a variable within a conditional block:</p><pre><code>// typeof guard\nif (typeof x === \'string\') { /* x is string */ }\n\n// instanceof guard\nif (error instanceof Error) { /* error is Error */ }\n\n// Custom type guard\nfunction isUser(obj: unknown): obj is User {\n  return typeof obj === \'object\' && obj !== null && \'name\' in obj;\n}</code></pre><h2>Q: Explain discriminated unions</h2><p><strong>Answer:</strong> Discriminated unions use a common property (discriminant) to narrow types:</p><pre><code>type Success = { status: \'success\'; data: string }\ntype Error = { status: \'error\'; message: string }\ntype Result = Success | Error\n\nfunction handle(result: Result) {\n  if (result.status === \'success\') {\n    console.log(result.data); // data is accessible\n  } else {\n    console.log(result.message); // message is accessible\n  }\n}</code></pre><h2>Q: What is the \'infer\' keyword?</h2><p><strong>Answer:</strong> <code>infer</code> is used in conditional types to extract a type:</p><pre><code>type ReturnType&lt;T&gt; = T extends (...args: any[]) => infer R ? R : never;\ntype Result = ReturnType&lt;() => string&gt;; // string</code></pre>',
        NULL,
        0,
        0,
        0,
        0,
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:43',
        0,
        0,
        0,
        0,
        '4',
        'en'
    );
/*!40000 ALTER TABLE `page` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `pagechange`
--

DROP TABLE IF EXISTS `pagechange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
) ENGINE = InnoDB AUTO_INCREMENT = 614 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `pagechange`
--

LOCK TABLES `pagechange` WRITE;
/*!40000 ALTER TABLE `pagechange` DISABLE KEYS */
;
INSERT INTO
    `pagechange`
VALUES (
        1,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        1,
        '2026-01-29 12:12:24'
    ),
    (
        2,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:24'
    ),
    (
        3,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        1,
        '2026-01-29 12:12:24'
    ),
    (
        4,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:24'
    ),
    (
        5,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:24'
    ),
    (
        6,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:24'
    ),
    (
        7,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:24'
    ),
    (
        8,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        9,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        10,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        11,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        12,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        13,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        14,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:25'
    ),
    (
        15,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        16,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        17,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        18,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:25'
    ),
    (
        19,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:25'
    ),
    (
        20,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        21,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        22,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        23,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        24,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        25,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:25'
    ),
    (
        26,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:25'
    ),
    (
        27,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:25'
    ),
    (
        28,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        29,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:25'
    ),
    (
        30,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        31,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        32,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:25'
    ),
    (
        33,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:25'
    ),
    (
        34,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        35,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        36,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:26'
    ),
    (
        37,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:26'
    ),
    (
        38,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:26'
    ),
    (
        39,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        40,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        41,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        42,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        43,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        44,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        45,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        46,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:26'
    ),
    (
        47,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:26'
    ),
    (
        48,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:26'
    ),
    (
        49,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        50,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        51,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        52,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        53,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        54,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        55,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        56,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        57,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:26'
    ),
    (
        58,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        59,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        60,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:26'
    ),
    (
        61,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        62,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        63,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        64,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        65,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        66,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:26'
    ),
    (
        67,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:27'
    ),
    (
        68,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        69,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        70,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        71,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        72,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        73,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        74,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        75,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        76,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        77,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        78,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        79,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        80,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:27'
    ),
    (
        81,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        82,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        83,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        84,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        85,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        86,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        87,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        88,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        89,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        90,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        91,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        92,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        93,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        94,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:27'
    ),
    (
        95,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        96,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:27'
    ),
    (
        97,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        98,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        99,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:27'
    ),
    (
        100,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        101,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        102,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        103,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        104,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        105,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:27'
    ),
    (
        106,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        107,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        108,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        109,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        2,
        '2026-01-29 12:12:28'
    ),
    (
        110,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:28'
    ),
    (
        111,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:28'
    ),
    (
        112,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        113,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        114,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        115,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        116,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        117,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        118,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        119,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        120,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        121,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        122,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        123,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        124,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        125,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:28'
    ),
    (
        126,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        127,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        128,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:28'
    ),
    (
        129,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        130,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        131,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        132,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        133,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        134,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        135,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        136,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        137,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        138,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:28'
    ),
    (
        139,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:28'
    ),
    (
        140,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:29'
    ),
    (
        141,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:29'
    ),
    (
        142,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:29'
    ),
    (
        143,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        144,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        145,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        146,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        147,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        148,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        149,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        150,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        151,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        152,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        153,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        154,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        155,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        156,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:29'
    ),
    (
        157,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:29'
    ),
    (
        158,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:29'
    ),
    (
        159,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:29'
    ),
    (
        160,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:29'
    ),
    (
        161,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        162,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        163,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:29'
    ),
    (
        164,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        165,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        166,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        167,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        168,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        169,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        170,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        171,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        172,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        173,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:29'
    ),
    (
        174,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:29'
    ),
    (
        175,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:29'
    ),
    (
        176,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:29'
    ),
    (
        177,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:30'
    ),
    (
        178,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:30'
    ),
    (
        179,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:30'
    ),
    (
        180,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        181,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        182,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        183,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        184,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        185,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        186,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        187,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        188,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        189,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        190,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        191,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        192,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        193,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        194,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        195,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        196,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        197,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:30'
    ),
    (
        198,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:30'
    ),
    (
        199,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:30'
    ),
    (
        200,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        201,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        202,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:30'
    ),
    (
        203,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        204,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        205,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        206,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        207,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        208,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        209,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        210,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        211,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        212,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:30'
    ),
    (
        213,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        214,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        215,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        216,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:30'
    ),
    (
        217,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        218,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:31'
    ),
    (
        219,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        220,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        221,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        222,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        223,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        224,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        225,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        226,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        227,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        228,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        229,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        230,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        231,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        232,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        233,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        234,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        235,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        236,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        237,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        238,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:31'
    ),
    (
        239,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:31'
    ),
    (
        240,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:31'
    ),
    (
        241,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        242,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        243,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:31'
    ),
    (
        244,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        245,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        246,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        247,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        248,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        249,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        250,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        251,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        252,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        253,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:31'
    ),
    (
        254,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        255,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        256,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        257,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:31'
    ),
    (
        258,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        259,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        260,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        261,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:32'
    ),
    (
        262,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:32'
    ),
    (
        263,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:32'
    ),
    (
        264,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        265,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        266,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        267,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        268,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        269,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        270,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        271,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        272,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        273,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        274,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        275,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        276,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        277,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        278,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        279,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        280,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        281,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        282,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        283,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        284,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:32'
    ),
    (
        285,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:32'
    ),
    (
        286,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:32'
    ),
    (
        287,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:32'
    ),
    (
        288,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        289,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        290,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:32'
    ),
    (
        291,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        292,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        293,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        294,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        295,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        296,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:32'
    ),
    (
        297,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        298,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        299,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        300,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        301,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        302,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        303,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        304,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        305,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        306,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        307,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        308,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        309,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        310,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        3,
        '2026-01-29 12:12:33'
    ),
    (
        311,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:33'
    ),
    (
        312,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:33'
    ),
    (
        313,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:33'
    ),
    (
        314,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:33'
    ),
    (
        315,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:33'
    ),
    (
        316,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        317,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        318,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        319,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        320,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        321,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        322,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        323,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        324,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        325,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:33'
    ),
    (
        326,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        327,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        328,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        329,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        330,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        331,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:33'
    ),
    (
        332,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        333,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        334,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        335,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        336,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:34'
    ),
    (
        337,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        338,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        339,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        340,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        341,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        342,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        343,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        344,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        345,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        346,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        347,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        348,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        349,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        350,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        351,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        352,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        353,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        354,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        355,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        356,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        357,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        358,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        359,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:34'
    ),
    (
        360,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:34'
    ),
    (
        361,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:34'
    ),
    (
        362,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:34'
    ),
    (
        363,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:34'
    ),
    (
        364,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        365,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        366,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:34'
    ),
    (
        367,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        368,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        369,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        370,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        371,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        372,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:34'
    ),
    (
        373,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        374,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        375,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        376,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        377,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        378,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        379,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        380,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        381,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        382,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        383,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        384,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        385,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        386,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        387,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:35'
    ),
    (
        388,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:35'
    ),
    (
        389,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:35'
    ),
    (
        390,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:35'
    ),
    (
        391,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:35'
    ),
    (
        392,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:35'
    ),
    (
        393,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:35'
    ),
    (
        394,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:35'
    ),
    (
        395,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        396,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        397,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        398,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        399,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        400,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        401,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        402,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        403,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        404,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:35'
    ),
    (
        405,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        406,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        407,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        408,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        409,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        410,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        411,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        412,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:35'
    ),
    (
        413,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        414,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        415,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        416,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        417,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        418,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:36'
    ),
    (
        419,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:36'
    ),
    (
        420,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:36'
    ),
    (
        421,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:36'
    ),
    (
        422,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:36'
    ),
    (
        423,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:36'
    ),
    (
        424,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        425,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        426,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        427,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        428,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        429,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        430,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        431,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        432,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        433,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:36'
    ),
    (
        434,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        435,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        436,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        437,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        438,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        439,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        440,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        441,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        442,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        443,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:36'
    ),
    (
        444,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        445,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        446,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        447,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:36'
    ),
    (
        448,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:36'
    ),
    (
        449,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:37'
    ),
    (
        450,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:37'
    ),
    (
        451,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        452,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        453,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        454,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        455,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        456,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        457,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        458,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        459,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        460,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        461,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        462,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        463,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        464,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        465,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        466,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        467,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        468,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        469,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        470,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        471,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        472,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        473,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:37'
    ),
    (
        474,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:37'
    ),
    (
        475,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:37'
    ),
    (
        476,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:37'
    ),
    (
        477,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:37'
    ),
    (
        478,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:37'
    ),
    (
        479,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:37'
    ),
    (
        480,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        481,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        482,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:37'
    ),
    (
        483,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        484,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        485,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        486,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        487,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        488,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:37'
    ),
    (
        489,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        490,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        491,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        492,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        493,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        494,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        495,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        496,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        497,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        498,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        499,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        500,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        501,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        502,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        503,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        504,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        505,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        506,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        507,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        508,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:38'
    ),
    (
        509,
        30,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Fundamental Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:38'
    ),
    (
        510,
        30,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Fundamental Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is the difference between \'any\' and \'unknown\'?</h2><p><strong>Answer:</strong> Both can hold any value, but <code>unknown</code> is the type-safe counterpart of <code>any</code>. You must perform type checking before using an <code>unknown</code> value, while <code>any</code> bypasses all type checking.</p><pre><code>const a: any = 5;\\na.foo(); // No error (but crashes at runtime)\\n\\nconst b: unknown = 5;\\nb.foo(); // Error: Object is of type \'unknown\'\\nif (typeof b === \'number\') {\\n  console.log(b.toFixed(2)); // OK\\n}</code></pre><h2>Q: What is the difference between \'type\' and \'interface\'?</h2><p><strong>Answer:</strong></p><ul><li><strong>Interface:</strong> Can be extended and merged, best for object shapes and class contracts</li><li><strong>Type:</strong> More flexible, supports unions, intersections, mapped types, and primitives</li></ul><pre><code>// Interface merging\\ninterface User { name: string }\\ninterface User { age: number } // Merged\\n\\n// Type union (not possible with interface)\\ntype Result = Success | Error</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:38'
    ),
    (
        511,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:38'
    ),
    (
        512,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:38'
    ),
    (
        513,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:38'
    ),
    (
        514,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:38'
    ),
    (
        515,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        516,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        517,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        518,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        519,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        520,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        521,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        522,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        523,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        524,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:38'
    ),
    (
        525,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        526,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        527,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        528,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:38'
    ),
    (
        529,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        530,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        531,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        532,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        533,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        534,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        535,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        536,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        537,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        538,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        539,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        540,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        541,
        30,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Fundamental Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is the difference between \'any\' and \'unknown\'?</h2><p><strong>Answer:</strong> Both can hold any value, but <code>unknown</code> is the type-safe counterpart of <code>any</code>. You must perform type checking before using an <code>unknown</code> value, while <code>any</code> bypasses all type checking.</p><pre><code>const a: any = 5;\\na.foo(); // No error (but crashes at runtime)\\n\\nconst b: unknown = 5;\\nb.foo(); // Error: Object is of type \'unknown\'\\nif (typeof b === \'number\') {\\n  console.log(b.toFixed(2)); // OK\\n}</code></pre><h2>Q: What is the difference between \'type\' and \'interface\'?</h2><p><strong>Answer:</strong></p><ul><li><strong>Interface:</strong> Can be extended and merged, best for object shapes and class contracts</li><li><strong>Type:</strong> More flexible, supports unions, intersections, mapped types, and primitives</li></ul><pre><code>// Interface merging\\ninterface User { name: string }\\ninterface User { age: number } // Merged\\n\\n// Type union (not possible with interface)\\ntype Result = Success | Error</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:39'
    ),
    (
        542,
        31,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Generics Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:39'
    ),
    (
        543,
        31,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Generics Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are generics and why are they useful?</h2><p><strong>Answer:</strong> Generics allow you to write reusable code that works with multiple types while maintaining type safety. They act as type variables that are determined when the function or class is used.</p><pre><code>function identity&lt;T&gt;(arg: T): T {\\n  return arg;\\n}\\n\\nconst num = identity&lt;number&gt;(42); // number\\nconst str = identity(\'hello\'); // string (inferred)</code></pre><h2>Q: What are generic constraints?</h2><p><strong>Answer:</strong> Constraints limit what types can be used with a generic using the <code>extends</code> keyword:</p><pre><code>interface HasLength {\\n  length: number;\\n}\\n\\nfunction logLength&lt;T extends HasLength&gt;(arg: T): void {\\n  console.log(arg.length); // Safe to access .length\\n}\\n\\nlogLength(\'hello\'); // OK\\nlogLength([1, 2, 3]); // OK\\nlogLength(42); // Error: number doesn\'t have length</code></pre><h2>Q: Explain the keyof operator</h2><p><strong>Answer:</strong> <code>keyof</code> produces a union type of all property keys of an object type:</p><pre><code>type Person = { name: string; age: number }\\ntype PersonKeys = keyof Person; // \'name\' | \'age\'</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:39'
    ),
    (
        544,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30,31],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:39'
    ),
    (
        545,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:39'
    ),
    (
        546,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:39'
    ),
    (
        547,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:39'
    ),
    (
        548,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        549,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        550,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        551,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        552,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        553,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        554,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        555,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        556,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        557,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:39'
    ),
    (
        558,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        559,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        560,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        561,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        562,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:39'
    ),
    (
        563,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        564,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        565,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        566,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        567,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        568,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        569,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        570,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        571,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        572,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        573,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30,31],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        574,
        30,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Fundamental Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is the difference between \'any\' and \'unknown\'?</h2><p><strong>Answer:</strong> Both can hold any value, but <code>unknown</code> is the type-safe counterpart of <code>any</code>. You must perform type checking before using an <code>unknown</code> value, while <code>any</code> bypasses all type checking.</p><pre><code>const a: any = 5;\\na.foo(); // No error (but crashes at runtime)\\n\\nconst b: unknown = 5;\\nb.foo(); // Error: Object is of type \'unknown\'\\nif (typeof b === \'number\') {\\n  console.log(b.toFixed(2)); // OK\\n}</code></pre><h2>Q: What is the difference between \'type\' and \'interface\'?</h2><p><strong>Answer:</strong></p><ul><li><strong>Interface:</strong> Can be extended and merged, best for object shapes and class contracts</li><li><strong>Type:</strong> More flexible, supports unions, intersections, mapped types, and primitives</li></ul><pre><code>// Interface merging\\ninterface User { name: string }\\ninterface User { age: number } // Merged\\n\\n// Type union (not possible with interface)\\ntype Result = Success | Error</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        575,
        31,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Generics Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are generics and why are they useful?</h2><p><strong>Answer:</strong> Generics allow you to write reusable code that works with multiple types while maintaining type safety. They act as type variables that are determined when the function or class is used.</p><pre><code>function identity&lt;T&gt;(arg: T): T {\\n  return arg;\\n}\\n\\nconst num = identity&lt;number&gt;(42); // number\\nconst str = identity(\'hello\'); // string (inferred)</code></pre><h2>Q: What are generic constraints?</h2><p><strong>Answer:</strong> Constraints limit what types can be used with a generic using the <code>extends</code> keyword:</p><pre><code>interface HasLength {\\n  length: number;\\n}\\n\\nfunction logLength&lt;T extends HasLength&gt;(arg: T): void {\\n  console.log(arg.length); // Safe to access .length\\n}\\n\\nlogLength(\'hello\'); // OK\\nlogLength([1, 2, 3]); // OK\\nlogLength(42); // Error: number doesn\'t have length</code></pre><h2>Q: Explain the keyof operator</h2><p><strong>Answer:</strong> <code>keyof</code> produces a union type of all property keys of an object type:</p><pre><code>type Person = { name: string; age: number }\\ntype PersonKeys = keyof Person; // \'name\' | \'age\'</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:40'
    ),
    (
        576,
        32,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Utility Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:40'
    ),
    (
        577,
        32,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Utility Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: Explain Partial, Required, and Readonly utility types</h2><p><strong>Answer:</strong></p><ul><li><strong>Partial&lt;T&gt;:</strong> Makes all properties optional</li><li><strong>Required&lt;T&gt;:</strong> Makes all properties required</li><li><strong>Readonly&lt;T&gt;:</strong> Makes all properties read-only</li></ul><pre><code>interface User {\\n  name: string;\\n  age?: number;\\n}\\n\\ntype PartialUser = Partial&lt;User&gt;; // { name?: string; age?: number }\\ntype RequiredUser = Required&lt;User&gt;; // { name: string; age: number }\\ntype ReadonlyUser = Readonly&lt;User&gt;; // All properties immutable</code></pre><h2>Q: What is the Pick and Omit utility type?</h2><p><strong>Answer:</strong></p><ul><li><strong>Pick&lt;T, K&gt;:</strong> Creates a type with only the specified keys</li><li><strong>Omit&lt;T, K&gt;:</strong> Creates a type without the specified keys</li></ul><pre><code>interface User { id: number; name: string; email: string }\\n\\ntype UserPreview = Pick&lt;User, \'id\' | \'name\'&gt;;\\ntype UserWithoutEmail = Omit&lt;User, \'email\'&gt;;</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:40'
    ),
    (
        578,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30,31,32],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:40'
    ),
    (
        579,
        1,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Welcome to memoWikis\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>What is memoWikis?</h2><p>memoWikis is a collaborative learning platform that combines wiki-style content creation with spaced repetition learning.</p><h2>Key Features</h2><ul><li><strong>Create and Share Knowledge:</strong> Build structured wikis on any topic</li><li><strong>Learn with Flashcards:</strong> Generate questions from your content</li><li><strong>Track Your Progress:</strong> Monitor your learning with analytics</li></ul><h2>Getting Started</h2><p>Start by exploring existing wikis or create your own. Add questions to test your knowledge and review them regularly for optimal learning.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[2,3],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:40'
    ),
    (
        580,
        2,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Creating Your First Wiki\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Step-by-Step Guide</h2><p>Creating a wiki is easy:</p><ol><li>Click the \'New Wiki\' button</li><li>Enter a title and description</li><li>Add your content using the rich text editor</li><li>Organize with subpages for complex topics</li></ol><h2>Tips for Good Wikis</h2><ul><li>Keep topics focused and well-structured</li><li>Use headings to organize content</li><li>Add sources for credibility</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:40'
    ),
    (
        581,
        3,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Learning with Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Power of Active Recall</h2><p>Questions help you retain knowledge better through active recall and spaced repetition.</p><h2>Creating Questions</h2><p>You can create questions manually or use AI to generate them from your wiki content.</p><h2>Review Schedule</h2><p>memoWikis uses spaced repetition to show you questions at optimal intervals for long-term retention.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[1],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        49,
        '2026-01-29 12:12:40'
    ),
    (
        582,
        4,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"What to Know to Be Funny and Informed\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Art of Witty Conversation</h2><p>The best conversationalists blend knowledge with humor. Being genuinely funny at dinner parties, social gatherings, or professional events requires understanding context, having interesting knowledge, and knowing when to deploy it.</p><h2>The Secret Formula</h2><p>Great wit combines:</p><ul><li><strong>Broad Knowledge:</strong> Understanding enough about many topics to make unexpected connections</li><li><strong>Timing:</strong> Knowing when a joke enhances versus derails conversation</li><li><strong>Self-Deprecation:</strong> The safest target for humor is yourself</li><li><strong>Cultural Awareness:</strong> Knowing what\'s trending and what\'s taboo</li></ul><h2>Topics Covered</h2><p>This wiki covers essential cultural literacy, conversation starters, classic anecdotes, and the art of the well-timed observation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[5,6,7,8],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        583,
        5,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Essential Cultural References\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Classic Literature You Should Know (Even If You Haven\'t Read)</h2><ul><li><strong>1984 (Orwell):</strong> Reference when discussing surveillance or authoritarian control</li><li><strong>Catch-22 (Heller):</strong> Any absurd bureaucratic contradiction</li><li><strong>Kafka:</strong> When situations are surreally nightmarish (\'very Kafkaesque\')</li><li><strong>Don Quixote:</strong> Tilting at windmills = fighting imaginary problems</li></ul><h2>Historical Figures for Witty Comparisons</h2><ul><li><strong>Nero:</strong> Fiddling while something burns (ignoring important problems)</li><li><strong>Marie Antoinette:</strong> \'Let them eat cake\' (out-of-touch elitism)</li><li><strong>Machiavelli:</strong> Cunning political maneuvering</li><li><strong>Sisyphus:</strong> Endlessly repeating futile tasks</li></ul><h2>Movie & TV References That Work</h2><p>The Godfather, Casablanca, Monty Python, The Office, and Seinfeld provide universal reference points across generations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        584,
        6,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Conversation Starters That Work\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Best Questions</h2><p>Great conversationalists ask questions that people enjoy answering:</p><ul><li>\'What\'s the most interesting thing you\'ve learned recently?\'</li><li>\'If you could have dinner with anyone in history, who would it be?\'</li><li>\'What\'s something you\'ve changed your mind about?\'</li><li>\'What\'s the best book/film/podcast you\'ve encountered lately?\'</li></ul><h2>Topics That Engage Almost Everyone</h2><ul><li><strong>Food and Travel:</strong> Almost universally enjoyable topics</li><li><strong>Childhood Nostalgia:</strong> What did you want to be when you grew up?</li><li><strong>Hypotheticals:</strong> \'Would you rather...\' questions spark debate</li><li><strong>Observations About the Moment:</strong> Comment on the event, venue, or shared experience</li></ul><h2>Topics to Generally Avoid</h2><p>Religion and politics can work, but only if you\'re skilled at navigating disagreement with grace. Avoid medical horror stories, complaints about exes, and one-upping others.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        585,
        7,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Classic Anecdotes and Quips\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Winston Churchill\'s Wit</h2><ul><li>Lady Astor: \'If you were my husband, I\'d poison your tea.\' Churchill: \'Madam, if you were my wife, I\'d drink it.\'</li><li>\'I have taken more out of alcohol than alcohol has taken out of me.\'</li><li>\'A fanatic is one who can\'t change his mind and won\'t change the subject.\'</li></ul><h2>Oscar Wilde\'s Brilliance</h2><ul><li>\'I can resist everything except temptation.\'</li><li>\'Be yourself; everyone else is already taken.\'</li><li>\'The only thing worse than being talked about is not being talked about.\'</li></ul><h2>Groucho Marx\'s Absurdism</h2><ul><li>\'I refuse to join any club that would have me as a member.\'</li><li>\'Outside of a dog, a book is man\'s best friend. Inside of a dog, it\'s too dark to read.\'</li></ul><h2>Modern Wit</h2><p>Douglas Adams, Terry Pratchett, and comedians like Mitch Hedberg offer quotable lines that work in various situations.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        586,
        8,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"The Art of Observation\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Seinfeld\'s Secret</h2><p>Jerry Seinfeld\'s comedy formula: observe the small absurdities of everyday life that everyone experiences but no one talks about. \'Did you ever notice...?\' is powerful because shared recognition creates connection.</p><h2>Practicing Observation</h2><ul><li>Notice the gap between what people say and what they do</li><li>Spot patterns in social behavior</li><li>Find the absurd in the mundane</li><li>Ask yourself: \'What would an alien think of this?\'</li></ul><h2>Timing Is Everything</h2><p>The best observations are delivered:</p><ul><li>With a pause before the punchline</li><li>When the conversation is already light</li><li>As an aside rather than the main focus</li><li>With confident understatement rather than overselling</li></ul><h2>The Callback</h2><p>One of the most sophisticated humor techniques: reference something mentioned earlier in the conversation. It shows you were listening and creates a satisfying \'full circle\' moment.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[4],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        587,
        9,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Evolution of the Economic System That Shaped the Modern World</h2><p>Capitalism, characterized by private ownership of the means of production, free markets, and profit-driven enterprise, has developed over centuries into the dominant global economic system.</p><h2>Why Study Capitalism\'s History?</h2><p>Understanding capitalism\'s evolution helps us:</p><ul><li><strong>Contextualize Debates:</strong> Current economic arguments have historical roots</li><li><strong>Recognize Patterns:</strong> Boom-bust cycles have repeated throughout history</li><li><strong>Evaluate Alternatives:</strong> Compare capitalism with other systems that emerged in response</li><li><strong>Anticipate Change:</strong> Economic systems evolve in response to technological and social changes</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[10,11,12,13],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        588,
        10,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Origins and Mercantilism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Pre-Capitalist Economies</h2><p>Before capitalism, most economies were based on feudalism (land-based agrarian systems with lords and serfs) or guild-based craftsmanship in cities.</p><h2>Mercantilism (16th-18th Century)</h2><p>Mercantilism was a precursor to capitalism where:</p><ul><li>Nations competed to accumulate gold and silver</li><li>Colonies provided raw materials and markets</li><li>Trade was heavily regulated by the state</li><li>The goal was a positive balance of trade</li></ul><h2>Key Developments</h2><p>The rise of banking (Italian city-states), joint-stock companies (Dutch East India Company, 1602), and global trade networks laid the groundwork for capitalism.</p><h2>Adam Smith\'s Breakthrough</h2><p>Adam Smith\'s \'The Wealth of Nations\' (1776) critiqued mercantilism and argued for free markets, the division of labor, and the \'invisible hand\' guiding self-interest toward collective benefit.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        589,
        11,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Industrial Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Industrial Revolution (1760-1840)</h2><p>Beginning in Britain, the Industrial Revolution transformed production:</p><ul><li><strong>Factory System:</strong> Replaced cottage industries and artisan production</li><li><strong>Steam Power:</strong> Enabled mechanization of manufacturing</li><li><strong>Urbanization:</strong> Workers moved from farms to industrial cities</li><li><strong>Capital Accumulation:</strong> Factory owners amassed unprecedented wealth</li></ul><h2>Social Consequences</h2><p>Early industrial capitalism brought both progress and misery: child labor, dangerous working conditions, 16-hour workdays, and urban poverty. These conditions sparked labor movements and calls for reform.</p><h2>The Rise of Marx</h2><p>Karl Marx (1818-1883) analyzed capitalism\'s contradictions in \'Das Kapital,\' predicting that exploitation of workers would lead to revolution. His ideas inspired socialist and communist movements worldwide.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        590,
        12,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Financial and Global Capitalism\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Gilded Age (1870-1900)</h2><p>Industrialization created vast fortunes for \'robber barons\' like Rockefeller, Carnegie, and Vanderbilt. This era saw:</p><ul><li>Monopolies and trusts dominating industries</li><li>Extreme wealth inequality</li><li>Anti-trust legislation (Sherman Act, 1890)</li><li>Rise of investment banking</li></ul><h2>The Great Depression (1929-1939)</h2><p>The stock market crash of 1929 revealed capitalism\'s instability. Mass unemployment, bank failures, and deflation led to:</p><ul><li>Keynesian economics (government intervention to manage demand)</li><li>New Deal policies in the US</li><li>Rise of fascism as an alternative in Europe</li></ul><h2>Post-WWII Golden Age</h2><p>The period 1945-1970 saw unprecedented growth, a strong middle class, and the welfare state in Western democracies. Bretton Woods established the dollar-based monetary system.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        591,
        13,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Neoliberalism and Today\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Neoliberal Turn (1980s)</h2><p>Reagan (US) and Thatcher (UK) championed neoliberalism:</p><ul><li>Deregulation of markets</li><li>Privatization of state enterprises</li><li>Reduction of union power</li><li>Lower taxes on capital and high incomes</li><li>Globalization of production and finance</li></ul><h2>Financialization</h2><p>The economy increasingly centered on financial markets. The 2008 Financial Crisis exposed risks of unregulated finance and led to massive bank bailouts.</p><h2>21st Century Debates</h2><p>Current challenges to capitalism include:</p><ul><li><strong>Inequality:</strong> Wealth concentration rivals the Gilded Age</li><li><strong>Climate Change:</strong> Can profit-driven systems address environmental limits?</li><li><strong>Automation:</strong> What happens when machines replace workers?</li><li><strong>Alternative Models:</strong> Universal Basic Income, stakeholder capitalism, and degrowth movements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[9],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        50,
        '2026-01-29 12:12:40'
    ),
    (
        592,
        14,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of the Uckermark and Lychen\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>A Journey Through Brandenburg\'s Northern Treasure</h2><p>The Uckermark, one of Germany\'s largest and most sparsely populated districts, lies in northeastern Brandenburg. At its heart is Lychen, a picturesque town surrounded by seven lakes, known as the \'Pearl of the Uckermark.\'</p><h2>Why This Region Matters</h2><ul><li><strong>Natural Beauty:</strong> Protected landscapes, ancient forests, and crystal-clear lakes</li><li><strong>Rich History:</strong> From Slavic settlements to Prussian development to GDR history</li><li><strong>Cultural Heritage:</strong> Unique blend of German and Slavic influences</li><li><strong>Contemporary Relevance:</strong> Model for sustainable rural development</li></ul><h2>Topics Covered</h2><p>This wiki explores the Uckermark\'s history from prehistoric times through medieval colonization, the Prussian era, the World Wars, life under the GDR, and the region\'s rebirth after reunification.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[15,16,17,18],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        593,
        15,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Prehistoric and Slavic Period\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Early Settlements</h2><p>Archaeological evidence shows human presence in the Uckermark since the Stone Age. The region\'s many lakes and forests provided abundant resources for early hunter-gatherers.</p><h2>Slavic Period (6th-12th Century)</h2><p>Slavic tribes, particularly the Ukrani (from whom \'Uckermark\' derives its name), settled the region around the 6th century. They established agricultural communities and trading posts.</p><ul><li><strong>The Ukrani:</strong> A Slavic tribe that gave the region its name</li><li><strong>Settlement Patterns:</strong> Villages along waterways and lake shores</li><li><strong>Religion:</strong> Slavic paganism with sacred groves and water spirits</li></ul><h2>German Colonization</h2><p>Beginning in the 12th century, German settlers arrived during the <strong>Ostsiedlung</strong> (Eastern Colonization). Cistercian monks established monasteries and cleared forests for agriculture. The Slavic population was gradually assimilated.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        594,
        16,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Medieval and Prussian Era\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Lychen\'s Foundation</h2><p>Lychen received its town charter in <strong>1248</strong>, making it one of the oldest towns in the Uckermark. The name likely derives from the Slavic word for \'forest clearing.\'</p><h2>Medieval Development</h2><ul><li>Construction of St. John\'s Church (13th century, brick Gothic)</li><li>Membership in the Hanseatic League for trade connections</li><li>Town walls and fortifications (remnants still visible)</li><li>Brewing rights and local crafts</li></ul><h2>Thirty Years\' War (1618-1648)</h2><p>The Uckermark was devastated during the Thirty Years\' War. Population declined by up to 90% in some areas due to war, plague, and famine.</p><h2>Prussian Recovery</h2><p>Under Prussian rule, the region slowly recovered:</p><ul><li>Frederick William I and Frederick the Great encouraged resettlement</li><li>Huguenot refugees brought new skills and crafts</li><li>Drainage projects created farmland</li><li>Road and infrastructure improvements</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        595,
        17,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Modern History to 1945\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>19th Century Development</h2><p>The Industrial Revolution largely bypassed the Uckermark, which remained agricultural. However:</p><ul><li>Railway connections reached the region (mid-19th century)</li><li>Lychen became a vacation destination for Berliners</li><li>Brick-making industry developed</li><li>Tourism grew around the lakes</li></ul><h2>World War I and Weimar Republic</h2><p>Many young men from the Uckermark served in WWI. The postwar period brought economic hardship, but Lychen maintained its appeal as a resort town.</p><h2>Nazi Period (1933-1945)</h2><p>The region was not immune to Nazi ideology:</p><ul><li>Jewish residents were persecuted and deported</li><li>The RavensbrÃ¼ck concentration camp was located south of the Uckermark</li><li>Forced labor camps operated in the region</li></ul><h2>End of World War II</h2><p>Soviet forces captured the Uckermark in April-May 1945. Significant destruction and displacement occurred as the war ended.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        596,
        18,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"GDR Era and Reunification\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Life in the GDR (1949-1990)</h2><p>Under East German rule, the Uckermark was part of the district of Neubrandenburg:</p><ul><li><strong>Collectivization:</strong> Farms consolidated into Agricultural Production Cooperatives (LPGs)</li><li><strong>Industry:</strong> Some industrial development, but region remained rural</li><li><strong>Tourism:</strong> FDGB (trade union) vacation homes in Lychen</li><li><strong>Emigration:</strong> Young people left for cities; population declined</li></ul><h2>The Wende (1989-1990)</h2><p>The peaceful revolution reached even small towns like Lychen. Church groups and civic movements organized discussions about the future.</p><h2>Reunification and After</h2><p>Post-1990 challenges included:</p><ul><li>Economic upheaval as GDR enterprises closed</li><li>Continued population decline</li><li>Property restitution disputes</li></ul><h2>Contemporary Revival</h2><p>Today, the Uckermark is experiencing a renaissance:</p><ul><li>Nature tourism and sustainable development</li><li>Artists and remote workers discovering the region</li><li>Organic farming and local food movements</li><li>Lychen positioned as a model \'transition town\'</li></ul>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[14],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:40'
    ),
    (
        597,
        19,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"History of Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>From Philosophy to Science</h2><p>Psychology\'s journey from philosophical speculation to empirical science spans centuries. Understanding its history illuminates why psychology looks the way it does today and how our understanding of the human mind has evolved.</p><h2>Why Study Psychology\'s History?</h2><p>The history of psychology reveals:</p><ul><li><strong>Paradigm Shifts:</strong> How dominant theories rise and fall</li><li><strong>Methodological Progress:</strong> The evolution from introspection to brain imaging</li><li><strong>Cultural Influence:</strong> How society shapes psychological theories</li><li><strong>Ongoing Debates:</strong> Many current controversies have deep historical roots</li></ul><h2>Key Themes</h2><p>This wiki traces psychology from ancient philosophy through the founding of experimental psychology, the rise of behaviorism, the cognitive revolution, and modern neuroscience.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[20,21,22,23],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:41'
    ),
    (
        598,
        20,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Ancient and Philosophical Roots\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Ancient Greece</h2><ul><li><strong>Plato:</strong> Argued the mind (soul) is separate from the body (dualism). Knowledge is innate (rationalism).</li><li><strong>Aristotle:</strong> Believed knowledge comes from experience (empiricism). Wrote \'De Anima\' (On the Soul).</li><li><strong>Hippocrates:</strong> Located mental functions in the brain, not the heart.</li></ul><h2>Medieval and Renaissance Period</h2><p>Psychology was largely subsumed under philosophy and theology. Descartes (17th century) renewed dualism with his mind-body distinction and the concept of reflex action.</p><h2>Enlightenment Philosophy</h2><ul><li><strong>John Locke:</strong> The mind is a \'blank slate\' (tabula rasa), shaped by experience</li><li><strong>Immanuel Kant:</strong> Argued some knowledge structures are innate</li><li><strong>Associationism:</strong> Ideas connect through contiguity, similarity, and contrast</li></ul><p>These philosophical debates about nature vs. nurture continue today.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:41'
    ),
    (
        599,
        21,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Birth of Scientific Psychology\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Wilhelm Wundt (1832-1920)</h2><p>Wundt founded the first psychology laboratory in Leipzig, Germany in 1879 - widely considered psychology\'s birth as a science. His approach, called <strong>Structuralism</strong>, used introspection to analyze conscious experience into its basic elements.</p><h2>William James (1842-1910)</h2><p>American philosopher and psychologist who founded <strong>Functionalism</strong>. Rather than asking what consciousness is made of, James asked what it does. His \'Principles of Psychology\' (1890) remains influential.</p><h2>Sigmund Freud (1856-1939)</h2><p>Freud developed <strong>Psychoanalysis</strong>, emphasizing:</p><ul><li>The unconscious mind driving behavior</li><li>Childhood experiences shaping adult personality</li><li>Defense mechanisms (repression, projection, etc.)</li><li>The id, ego, and superego</li></ul><p>While many specifics are disputed, Freud\'s emphasis on the unconscious transformed psychology.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:41'
    ),
    (
        600,
        22,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Behaviorism and Its Critics\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Rise of Behaviorism</h2><p><strong>John B. Watson</strong> (1913) declared psychology should study only observable behavior, not subjective experience. Key figures:</p><ul><li><strong>Ivan Pavlov:</strong> Discovered classical conditioning with his famous dog experiments</li><li><strong>B.F. Skinner:</strong> Developed operant conditioning (reinforcement and punishment)</li><li><strong>Edward Thorndike:</strong> Law of Effect - behaviors followed by satisfaction are strengthened</li></ul><h2>Behaviorist Principles</h2><p>Behaviorism dominated from ~1920-1960. It emphasized:</p><ul><li>Environmental determinism</li><li>The \'black box\' approach (ignore internal mental states)</li><li>Rigorous experimental methods</li><li>Learning as the key to understanding behavior</li></ul><h2>Criticisms</h2><p>Behaviorism was criticized for ignoring cognition, being unable to explain language acquisition (Chomsky vs. Skinner), and oversimplifying human complexity.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:41'
    ),
    (
        601,
        23,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Cognitive Revolution to Present\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>The Cognitive Revolution (1950s-1960s)</h2><p>The \'cognitive revolution\' returned mental processes to psychology:</p><ul><li><strong>Noam Chomsky:</strong> Argued language acquisition requires innate mental structures</li><li><strong>George Miller:</strong> \'The magical number seven\' - limits of short-term memory</li><li><strong>Ulric Neisser:</strong> Coined \'cognitive psychology\' (1967)</li><li><strong>Computer metaphor:</strong> The mind as an information processor</li></ul><h2>Humanistic Psychology</h2><p>Maslow and Rogers emphasized human potential, self-actualization, and subjective experience as a \'third force\' against behaviorism and psychoanalysis.</p><h2>Modern Developments</h2><ul><li><strong>Neuroscience:</strong> Brain imaging reveals neural correlates of mental processes</li><li><strong>Evolutionary Psychology:</strong> Understanding the mind through adaptation</li><li><strong>Positive Psychology:</strong> Seligman\'s focus on wellbeing, not just pathology</li><li><strong>Cultural Psychology:</strong> How culture shapes cognition</li></ul><p>Today\'s psychology integrates biological, cognitive, developmental, and social perspectives.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[19],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        51,
        '2026-01-29 12:12:41'
    ),
    (
        602,
        24,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Nuxt 3 Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Prepare for Your Nuxt 3 Interview</h2><p>This comprehensive guide covers the most commonly asked Nuxt 3 interview questions, from fundamentals to advanced topics.</p><h2>Topics Covered</h2><ul><li><strong>Core Concepts:</strong> Rendering modes, file-based routing, auto-imports</li><li><strong>Server-Side Rendering:</strong> Hydration, universal rendering, SSR pitfalls</li><li><strong>Composables & Reactivity:</strong> Custom composables, Vue 3 reactivity</li><li><strong>Performance:</strong> Optimization techniques, caching strategies</li></ul><h2>Interview Tips</h2><p>Be prepared to explain concepts with practical examples and discuss trade-offs between different approaches. Hands-on experience with Nuxt 3 projects will give you confidence in answering scenario-based questions.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[25,26,27,28],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        603,
        25,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Core Concepts Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are the different rendering modes in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports multiple rendering modes:</p><ul><li><strong>Universal (SSR):</strong> Server-side rendering with client-side hydration</li><li><strong>SPA:</strong> Single Page Application, client-only rendering</li><li><strong>Static (SSG):</strong> Pre-rendered at build time</li><li><strong>Hybrid:</strong> Different modes per route using <code>routeRules</code></li></ul><h2>Q: How does file-based routing work in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 automatically generates routes based on the file structure in the <code>pages/</code> directory. Dynamic routes use square brackets: <code>[id].vue</code>, and nested routes mirror the folder structure.</p><h2>Q: What is the difference between useFetch and useAsyncData?</h2><p><strong>Answer:</strong> <code>useFetch</code> is a wrapper around <code>useAsyncData</code> specifically for HTTP requests using <code>$fetch</code>. <code>useAsyncData</code> is more generic and can be used for any async operation.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        604,
        26,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"SSR and Hydration Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is hydration in the context of Nuxt 3?</h2><p><strong>Answer:</strong> Hydration is the process where the client-side Vue application takes over the server-rendered HTML. Vue attaches event listeners and makes the page interactive without re-rendering the entire DOM.</p><h2>Q: What causes hydration mismatches and how do you fix them?</h2><p><strong>Answer:</strong> Hydration mismatches occur when server and client render different content. Common causes include:</p><ul><li>Using browser-only APIs on the server (window, document)</li><li>Time-based values that differ between server and client</li><li>Random values without consistent seeding</li></ul><p><strong>Solutions:</strong> Use <code>&lt;ClientOnly&gt;</code> component, check <code>import.meta.client</code>, or use <code>useState</code> for shared state.</p><h2>Q: When would you use the ClientOnly component?</h2><p><strong>Answer:</strong> Use <code>&lt;ClientOnly&gt;</code> for components that rely on browser APIs, third-party libraries without SSR support, or content that should only render client-side to avoid hydration issues.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        605,
        27,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Composables and State Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you create a custom composable in Nuxt 3?</h2><p><strong>Answer:</strong> Create a file in the <code>composables/</code> directory with a function that starts with <code>use</code>:</p><pre><code>// composables/useCounter.ts\\nexport const useCounter = () => {\\n  const count = ref(0)\\n  const increment = () => count.value++\\n  return { count, increment }\\n}</code></pre><p>It will be auto-imported throughout your application.</p><h2>Q: What is the difference between ref and reactive?</h2><p><strong>Answer:</strong> <code>ref</code> wraps a single value and requires <code>.value</code> to access it. <code>reactive</code> creates a reactive proxy of an object but cannot be reassigned. <code>ref</code> is preferred for primitives and when reassignment is needed.</p><h2>Q: How do you share state between components in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>useState</code> for simple shared state, Pinia stores for complex state management, or provide/inject for component tree scoping. <code>useState</code> is SSR-friendly and auto-imported.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        606,
        28,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: How do you implement middleware in Nuxt 3?</h2><p><strong>Answer:</strong> Nuxt 3 supports three types of middleware:</p><ul><li><strong>Anonymous:</strong> Defined inline with <code>definePageMeta</code></li><li><strong>Named:</strong> Files in <code>middleware/</code> directory</li><li><strong>Global:</strong> Files with <code>.global.ts</code> suffix</li></ul><pre><code>// middleware/auth.ts\\nexport default defineNuxtRouteMiddleware((to, from) => {\\n  if (!isAuthenticated()) {\\n    return navigateTo(\'/login\')\\n  }\\n})</code></pre><h2>Q: How do you configure environment variables in Nuxt 3?</h2><p><strong>Answer:</strong> Use <code>runtimeConfig</code> in <code>nuxt.config.ts</code>. Public variables go under <code>public</code> key and are available client-side. Private variables stay on the server only.</p><h2>Q: Explain the Nuxt 3 module system</h2><p><strong>Answer:</strong> Modules extend Nuxt functionality through hooks and the module API. They can add components, plugins, composables, and modify the build process. Use <code>defineNuxtModule</code> to create custom modules.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[24],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        607,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30,31,32],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        608,
        30,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Fundamental Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What is the difference between \'any\' and \'unknown\'?</h2><p><strong>Answer:</strong> Both can hold any value, but <code>unknown</code> is the type-safe counterpart of <code>any</code>. You must perform type checking before using an <code>unknown</code> value, while <code>any</code> bypasses all type checking.</p><pre><code>const a: any = 5;\\na.foo(); // No error (but crashes at runtime)\\n\\nconst b: unknown = 5;\\nb.foo(); // Error: Object is of type \'unknown\'\\nif (typeof b === \'number\') {\\n  console.log(b.toFixed(2)); // OK\\n}</code></pre><h2>Q: What is the difference between \'type\' and \'interface\'?</h2><p><strong>Answer:</strong></p><ul><li><strong>Interface:</strong> Can be extended and merged, best for object shapes and class contracts</li><li><strong>Type:</strong> More flexible, supports unions, intersections, mapped types, and primitives</li></ul><pre><code>// Interface merging\\ninterface User { name: string }\\ninterface User { age: number } // Merged\\n\\n// Type union (not possible with interface)\\ntype Result = Success | Error</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        609,
        31,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Generics Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are generics and why are they useful?</h2><p><strong>Answer:</strong> Generics allow you to write reusable code that works with multiple types while maintaining type safety. They act as type variables that are determined when the function or class is used.</p><pre><code>function identity&lt;T&gt;(arg: T): T {\\n  return arg;\\n}\\n\\nconst num = identity&lt;number&gt;(42); // number\\nconst str = identity(\'hello\'); // string (inferred)</code></pre><h2>Q: What are generic constraints?</h2><p><strong>Answer:</strong> Constraints limit what types can be used with a generic using the <code>extends</code> keyword:</p><pre><code>interface HasLength {\\n  length: number;\\n}\\n\\nfunction logLength&lt;T extends HasLength&gt;(arg: T): void {\\n  console.log(arg.length); // Safe to access .length\\n}\\n\\nlogLength(\'hello\'); // OK\\nlogLength([1, 2, 3]); // OK\\nlogLength(42); // Error: number doesn\'t have length</code></pre><h2>Q: Explain the keyof operator</h2><p><strong>Answer:</strong> <code>keyof</code> produces a union type of all property keys of an object type:</p><pre><code>type Person = { name: string; age: number }\\ntype PersonKeys = keyof Person; // \'name\' | \'age\'</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        610,
        32,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Utility Types Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: Explain Partial, Required, and Readonly utility types</h2><p><strong>Answer:</strong></p><ul><li><strong>Partial&lt;T&gt;:</strong> Makes all properties optional</li><li><strong>Required&lt;T&gt;:</strong> Makes all properties required</li><li><strong>Readonly&lt;T&gt;:</strong> Makes all properties read-only</li></ul><pre><code>interface User {\\n  name: string;\\n  age?: number;\\n}\\n\\ntype PartialUser = Partial&lt;User&gt;; // { name?: string; age?: number }\\ntype RequiredUser = Required&lt;User&gt;; // { name: string; age: number }\\ntype ReadonlyUser = Readonly&lt;User&gt;; // All properties immutable</code></pre><h2>Q: What is the Pick and Omit utility type?</h2><p><strong>Answer:</strong></p><ul><li><strong>Pick&lt;T, K&gt;:</strong> Creates a type with only the specified keys</li><li><strong>Omit&lt;T, K&gt;:</strong> Creates a type without the specified keys</li></ul><pre><code>interface User { id: number; name: string; email: string }\\n\\ntype UserPreview = Pick&lt;User, \'id\' | \'name\'&gt;;\\ntype UserWithoutEmail = Omit&lt;User, \'email\'&gt;;</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        52,
        '2026-01-29 12:12:41'
    ),
    (
        611,
        33,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Patterns Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":null,\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        0,
        4,
        '2026-01-29 12:12:41'
    ),
    (
        612,
        33,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"Advanced Patterns Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Q: What are type guards and how do you implement them?</h2><p><strong>Answer:</strong> Type guards narrow the type of a variable within a conditional block:</p><pre><code>// typeof guard\\nif (typeof x === \'string\') { /* x is string */ }\\n\\n// instanceof guard\\nif (error instanceof Error) { /* error is Error */ }\\n\\n// Custom type guard\\nfunction isUser(obj: unknown): obj is User {\\n  return typeof obj === \'object\' && obj !== null && \'name\' in obj;\\n}</code></pre><h2>Q: Explain discriminated unions</h2><p><strong>Answer:</strong> Discriminated unions use a common property (discriminant) to narrow types:</p><pre><code>type Success = { status: \'success\'; data: string }\\ntype Error = { status: \'error\'; message: string }\\ntype Result = Success | Error\\n\\nfunction handle(result: Result) {\\n  if (result.status === \'success\') {\\n    console.log(result.data); // data is accessible\\n  } else {\\n    console.log(result.message); // message is accessible\\n  }\\n}</code></pre><h2>Q: What is the \'infer\' keyword?</h2><p><strong>Answer:</strong> <code>infer</code> is used in conditional types to extract a type:</p><pre><code>type ReturnType&lt;T&gt; = T extends (...args: any[]) => infer R ? R : never;\\ntype Result = ReturnType&lt;() => string&gt;; // string</code></pre>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":[29],\"ChildIds\":null,\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:41'
    ),
    (
        613,
        29,
        '{\"PageRelations\":null,\"ImageWasUpdated\":false,\"Name\":\"TypeScript Interview Questions\",\"Description\":null,\"PageMardkown\":null,\"Content\":\"<h2>Master TypeScript for Your Next Interview</h2><p>TypeScript has become the industry standard for large-scale JavaScript applications. This guide covers essential interview questions ranging from basic types to advanced patterns.</p><h2>Why TypeScript?</h2><p>Companies adopt TypeScript because it provides:</p><ul><li><strong>Type Safety:</strong> Catch errors at compile-time instead of runtime</li><li><strong>Better IDE Support:</strong> Intelligent autocompletion and refactoring</li><li><strong>Self-Documenting Code:</strong> Types serve as inline documentation</li><li><strong>Scalability:</strong> Easier to maintain large codebases</li></ul><h2>Key Topics</h2><p>This wiki covers fundamental types, generics, utility types, type guards, and advanced patterns commonly asked in interviews.</p>\",\"CustomSegments\":null,\"WikipediaURL\":null,\"DisableLearningFunctions\":false,\"Visibility\":0,\"ParentIds\":null,\"ChildIds\":[30,31,32,33],\"DeleteChangeId\":null,\"DeletedName\":null}',
        1,
        2,
        7,
        1,
        '2026-01-29 12:12:41'
    );
/*!40000 ALTER TABLE `pagechange` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `pagerelation`
--

DROP TABLE IF EXISTS `pagerelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `pagerelation` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Page_id` int DEFAULT NULL,
    `Related_id` int DEFAULT NULL,
    `Previous_id` int DEFAULT NULL,
    `Next_id` int DEFAULT NULL,
    PRIMARY KEY (`Id`),
    KEY `Page_id` (`Page_id`),
    KEY `Related_id` (`Related_id`)
) ENGINE = InnoDB AUTO_INCREMENT = 27 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `pagerelation`
--

LOCK TABLES `pagerelation` WRITE;
/*!40000 ALTER TABLE `pagerelation` DISABLE KEYS */
;
INSERT INTO
    `pagerelation`
VALUES (1, 2, 1, NULL, 3),
    (2, 3, 1, 2, NULL),
    (3, 5, 4, NULL, 6),
    (4, 6, 4, 5, 7),
    (5, 7, 4, 6, 8),
    (6, 8, 4, 7, NULL),
    (7, 10, 9, NULL, 11),
    (8, 11, 9, 10, 12),
    (9, 12, 9, 11, 13),
    (10, 13, 9, 12, NULL),
    (11, 15, 14, NULL, 16),
    (12, 16, 14, 15, 17),
    (13, 17, 14, 16, 18),
    (14, 18, 14, 17, NULL),
    (15, 20, 19, NULL, 21),
    (16, 21, 19, 20, 22),
    (17, 22, 19, 21, 23),
    (18, 23, 19, 22, NULL),
    (19, 25, 24, NULL, 26),
    (20, 26, 24, 25, 27),
    (21, 27, 24, 26, 28),
    (22, 28, 24, 27, NULL),
    (23, 30, 29, NULL, 31),
    (24, 31, 29, 30, 32),
    (25, 32, 29, 31, 33),
    (26, 33, 29, 32, NULL);
/*!40000 ALTER TABLE `pagerelation` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `pages_to_questions`
--

DROP TABLE IF EXISTS `pages_to_questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `pages_to_questions` (
    `Question_id` int NOT NULL,
    `Page_id` int NOT NULL,
    KEY `Page_id` (`Page_id`),
    KEY `Question_id` (`Question_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `pages_to_questions`
--

LOCK TABLES `pages_to_questions` WRITE;
/*!40000 ALTER TABLE `pages_to_questions` DISABLE KEYS */
;
INSERT INTO
    `pages_to_questions`
VALUES (1, 1),
    (2, 2),
    (3, 3),
    (4, 4),
    (5, 5),
    (6, 5),
    (7, 6),
    (8, 6),
    (9, 7),
    (10, 8),
    (11, 9),
    (12, 10),
    (13, 10),
    (14, 11),
    (15, 12),
    (16, 13),
    (17, 14),
    (18, 15),
    (19, 16),
    (20, 16),
    (21, 17),
    (22, 18),
    (23, 19),
    (24, 20),
    (25, 21),
    (26, 21),
    (27, 22),
    (28, 23),
    (29, 24),
    (30, 25),
    (31, 26),
    (32, 26),
    (33, 27),
    (34, 28),
    (35, 29),
    (36, 30),
    (37, 31),
    (38, 31),
    (39, 32),
    (40, 33);
/*!40000 ALTER TABLE `pages_to_questions` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `pagevaluation`
--

DROP TABLE IF EXISTS `pagevaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `pagevaluation`
--

LOCK TABLES `pagevaluation` WRITE;
/*!40000 ALTER TABLE `pagevaluation` DISABLE KEYS */
;
/*!40000 ALTER TABLE `pagevaluation` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `pageview`
--

DROP TABLE IF EXISTS `pageview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `pageview` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Page_id` int DEFAULT NULL,
    `User_id` int DEFAULT NULL,
    `UserAgent` varchar(255) DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateOnly` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`),
    KEY `Page_id` (`Page_id`),
    KEY `User_id` (`User_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `pageview`
--

LOCK TABLES `pageview` WRITE;
/*!40000 ALTER TABLE `pageview` DISABLE KEYS */
;
/*!40000 ALTER TABLE `pageview` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `passwordrecoverytoken`
--

DROP TABLE IF EXISTS `passwordrecoverytoken`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `passwordrecoverytoken` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Token` varchar(255) DEFAULT NULL,
    `Email` varchar(255) DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `passwordrecoverytoken`
--

LOCK TABLES `passwordrecoverytoken` WRITE;
/*!40000 ALTER TABLE `passwordrecoverytoken` DISABLE KEYS */
;
/*!40000 ALTER TABLE `passwordrecoverytoken` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `persistentlogin`
--

DROP TABLE IF EXISTS `persistentlogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `persistentlogin` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int DEFAULT NULL,
    `LoginGuid` varchar(255) DEFAULT NULL,
    `Created` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `persistentlogin`
--

LOCK TABLES `persistentlogin` WRITE;
/*!40000 ALTER TABLE `persistentlogin` DISABLE KEYS */
;
/*!40000 ALTER TABLE `persistentlogin` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `question`
--

DROP TABLE IF EXISTS `question`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `Creator_id` (`Creator_id`)
) ENGINE = InnoDB AUTO_INCREMENT = 41 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `question`
--

LOCK TABLES `question` WRITE;
/*!40000 ALTER TABLE `question` DISABLE KEYS */
;
INSERT INTO
    `question`
VALUES (
        1,
        'What are the three key features of memoWikis?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        44,
        0,
        'Create and Share Knowledge, Learn with Flashcards, Track Your Progress',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        2,
        'What is the first step in creating a new wiki?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        53,
        0,
        'Click the \'New Wiki\' button',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        3,
        'What learning technique does memoWikis use for reviewing questions?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        43,
        0,
        'Spaced repetition',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        4,
        'What are the four components that combine to make great wit?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        31,
        0,
        'Broad Knowledge, Timing, Self-Deprecation, Cultural Awareness',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        5,
        'What does \'tilting at windmills\' mean, from Don Quixote?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        38,
        0,
        'Fighting imaginary problems',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        6,
        'What literary reference is used when describing surreally nightmarish situations?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        75,
        0,
        'Kafkaesque (Kafka)',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        7,
        'Question 1 for page Conversation Starters That Work?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        41,
        0,
        'Answer 1 for page Conversation Starters That Work',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        8,
        'Question 2 for page Conversation Starters That Work?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        84,
        0,
        'Answer 2 for page Conversation Starters That Work',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:41',
        '2026-01-29 12:12:41',
        0
    ),
    (
        9,
        'Who said: \'I can resist everything except temptation\'?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        82,
        0,
        'Oscar Wilde',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        10,
        'What is the \'callback\' technique in humor?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        51,
        0,
        'Referencing something mentioned earlier in the conversation',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        11,
        'What are the three main characteristics of capitalism?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        36,
        0,
        'Private ownership of means of production, free markets, profit-driven enterprise',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        12,
        'Who wrote \'The Wealth of Nations\' and in what year?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        62,
        0,
        'Adam Smith, 1776',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        13,
        'What is the name of the world\'s first joint-stock company, founded in 1602?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        51,
        0,
        'Dutch East India Company',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        14,
        'Who wrote \'Das Kapital\' and analyzed capitalism\'s contradictions?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        73,
        0,
        'Karl Marx',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        15,
        'What 1890 legislation aimed to break up monopolies and trusts?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        65,
        0,
        'Sherman Act',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        16,
        'Which two political leaders championed neoliberalism in the 1980s?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        2,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        67,
        0,
        'Reagan and Thatcher',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        17,
        'What nickname is given to Lychen?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        74,
        0,
        'Pearl of the Uckermark',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        18,
        'Which Slavic tribe gave the Uckermark its name?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        75,
        0,
        'The Ukrani',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        19,
        'In what year did Lychen receive its town charter?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        54,
        0,
        '1248',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        20,
        'What war devastated the Uckermark with up to 90% population loss in some areas?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        44,
        0,
        'Thirty Years\' War',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        21,
        'Which concentration camp was located south of the Uckermark?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        71,
        0,
        'RavensbrÃ¼ck',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        22,
        'What were collective farms called in the GDR?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        42,
        0,
        'Agricultural Production Cooperatives (LPGs)',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        23,
        'What are four things studying psychology\'s history reveals?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        61,
        0,
        'Paradigm Shifts, Methodological Progress, Cultural Influence, Ongoing Debates',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        24,
        'Which Greek philosopher believed knowledge comes from experience (empiricism)?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        59,
        0,
        'Aristotle',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:42',
        '2026-01-29 12:12:42',
        0
    ),
    (
        25,
        'Who founded the first psychology laboratory and in what year?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        45,
        0,
        'Wilhelm Wundt, 1879',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        26,
        'What are the three parts of personality according to Freud?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        77,
        0,
        'Id, ego, superego',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        27,
        'Who discovered classical conditioning with dog experiments?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        30,
        0,
        'Ivan Pavlov',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        28,
        'Who coined the term \'cognitive psychology\' in 1967?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        3,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        59,
        0,
        'Ulric Neisser',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        29,
        'What are the four rendering modes available in Nuxt 3?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        57,
        0,
        'Universal (SSR), SPA, Static (SSG), Hybrid',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        30,
        'What syntax is used for dynamic routes in Nuxt 3 file-based routing?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        59,
        0,
        'Square brackets, e.g., [id].vue',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        31,
        'What is the process called when Vue takes over server-rendered HTML and makes it interactive?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        73,
        0,
        'Hydration',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        32,
        'Which Nuxt 3 component prevents server-side rendering for its children?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        57,
        0,
        'ClientOnly',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        33,
        'What prefix should composable function names start with in Nuxt 3?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        46,
        0,
        'use',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        34,
        'What file suffix makes a middleware global in Nuxt 3?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        47,
        0,
        '.global.ts',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        35,
        'What are the four main benefits of using TypeScript over JavaScript?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        68,
        0,
        'Type Safety, Better IDE Support, Self-Documenting Code, Scalability',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        36,
        'Which TypeScript type is the type-safe counterpart of \'any\'?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        47,
        0,
        'unknown',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        37,
        'What keyword is used to constrain generic types in TypeScript?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        52,
        0,
        'extends',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        38,
        'What TypeScript operator produces a union type of all property keys?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        82,
        0,
        'keyof',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        39,
        'Which utility type makes all properties of a type optional?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        40,
        0,
        'Partial',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    ),
    (
        40,
        'What keyword is used in conditional types to extract a type?',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        81,
        0,
        'infer',
        1,
        '{\"IsCaseSensitive\":false,\"IsExtracInput\":false,\"IsDate\":false,\"IsNumber\":false,\"IsText\":true}',
        '2026-01-29 12:12:43',
        '2026-01-29 12:12:43',
        0
    );
/*!40000 ALTER TABLE `question` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `questionchange`
--

DROP TABLE IF EXISTS `questionchange`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `Question_id` (`Question_id`)
) ENGINE = InnoDB AUTO_INCREMENT = 41 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `questionchange`
--

LOCK TABLES `questionchange` WRITE;
/*!40000 ALTER TABLE `questionchange` DISABLE KEYS */
;
INSERT INTO
    `questionchange`
VALUES (
        1,
        1,
        '{\"QuestionText\":\"What are the three key features of memoWikis?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Create and Share Knowledge, Learn with Flashcards, Track Your Progress\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        1
    ),
    (
        2,
        2,
        '{\"QuestionText\":\"What is the first step in creating a new wiki?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Click the \'New Wiki\' button\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        1
    ),
    (
        3,
        3,
        '{\"QuestionText\":\"What learning technique does memoWikis use for reviewing questions?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Spaced repetition\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        1
    ),
    (
        4,
        4,
        '{\"QuestionText\":\"What are the four components that combine to make great wit?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Broad Knowledge, Timing, Self-Deprecation, Cultural Awareness\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        2
    ),
    (
        5,
        5,
        '{\"QuestionText\":\"What does \'tilting at windmills\' mean, from Don Quixote?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Fighting imaginary problems\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        2
    ),
    (
        6,
        6,
        '{\"QuestionText\":\"What literary reference is used when describing surreally nightmarish situations?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Kafkaesque (Kafka)\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        2
    ),
    (
        7,
        7,
        '{\"QuestionText\":\"Question 1 for page Conversation Starters That Work?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 1 for page Conversation Starters That Work\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:41',
        2
    ),
    (
        8,
        8,
        '{\"QuestionText\":\"Question 2 for page Conversation Starters That Work?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Answer 2 for page Conversation Starters That Work\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        9,
        9,
        '{\"QuestionText\":\"Who said: \'I can resist everything except temptation\'?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Oscar Wilde\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        10,
        10,
        '{\"QuestionText\":\"What is the \'callback\' technique in humor?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Referencing something mentioned earlier in the conversation\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        11,
        11,
        '{\"QuestionText\":\"What are the three main characteristics of capitalism?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Private ownership of means of production, free markets, profit-driven enterprise\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        12,
        12,
        '{\"QuestionText\":\"Who wrote \'The Wealth of Nations\' and in what year?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Adam Smith, 1776\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        13,
        13,
        '{\"QuestionText\":\"What is the name of the world\'s first joint-stock company, founded in 1602?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Dutch East India Company\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        14,
        14,
        '{\"QuestionText\":\"Who wrote \'Das Kapital\' and analyzed capitalism\'s contradictions?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Karl Marx\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        15,
        15,
        '{\"QuestionText\":\"What 1890 legislation aimed to break up monopolies and trusts?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Sherman Act\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        16,
        16,
        '{\"QuestionText\":\"Which two political leaders championed neoliberalism in the 1980s?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Reagan and Thatcher\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        2
    ),
    (
        17,
        17,
        '{\"QuestionText\":\"What nickname is given to Lychen?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Pearl of the Uckermark\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        18,
        18,
        '{\"QuestionText\":\"Which Slavic tribe gave the Uckermark its name?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"The Ukrani\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        19,
        19,
        '{\"QuestionText\":\"In what year did Lychen receive its town charter?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"1248\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        20,
        20,
        '{\"QuestionText\":\"What war devastated the Uckermark with up to 90% population loss in some areas?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Thirty Years\' War\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        21,
        21,
        '{\"QuestionText\":\"Which concentration camp was located south of the Uckermark?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"RavensbrÃ¼ck\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        22,
        22,
        '{\"QuestionText\":\"What were collective farms called in the GDR?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Agricultural Production Cooperatives (LPGs)\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        23,
        23,
        '{\"QuestionText\":\"What are four things studying psychology\'s history reveals?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Paradigm Shifts, Methodological Progress, Cultural Influence, Ongoing Debates\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:42',
        3
    ),
    (
        24,
        24,
        '{\"QuestionText\":\"Which Greek philosopher believed knowledge comes from experience (empiricism)?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Aristotle\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        3
    ),
    (
        25,
        25,
        '{\"QuestionText\":\"Who founded the first psychology laboratory and in what year?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Wilhelm Wundt, 1879\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        3
    ),
    (
        26,
        26,
        '{\"QuestionText\":\"What are the three parts of personality according to Freud?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Id, ego, superego\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        3
    ),
    (
        27,
        27,
        '{\"QuestionText\":\"Who discovered classical conditioning with dog experiments?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Ivan Pavlov\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        3
    ),
    (
        28,
        28,
        '{\"QuestionText\":\"Who coined the term \'cognitive psychology\' in 1967?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Ulric Neisser\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        3
    ),
    (
        29,
        29,
        '{\"QuestionText\":\"What are the four rendering modes available in Nuxt 3?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Universal (SSR), SPA, Static (SSG), Hybrid\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        30,
        30,
        '{\"QuestionText\":\"What syntax is used for dynamic routes in Nuxt 3 file-based routing?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Square brackets, e.g., [id].vue\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        31,
        31,
        '{\"QuestionText\":\"What is the process called when Vue takes over server-rendered HTML and makes it interactive?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Hydration\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        32,
        32,
        '{\"QuestionText\":\"Which Nuxt 3 component prevents server-side rendering for its children?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"ClientOnly\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        33,
        33,
        '{\"QuestionText\":\"What prefix should composable function names start with in Nuxt 3?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"use\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        34,
        34,
        '{\"QuestionText\":\"What file suffix makes a middleware global in Nuxt 3?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\".global.ts\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        35,
        35,
        '{\"QuestionText\":\"What are the four main benefits of using TypeScript over JavaScript?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Type Safety, Better IDE Support, Self-Documenting Code, Scalability\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        36,
        36,
        '{\"QuestionText\":\"Which TypeScript type is the type-safe counterpart of \'any\'?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"unknown\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        37,
        37,
        '{\"QuestionText\":\"What keyword is used to constrain generic types in TypeScript?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"extends\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        38,
        38,
        '{\"QuestionText\":\"What TypeScript operator produces a union type of all property keys?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"keyof\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        39,
        39,
        '{\"QuestionText\":\"Which utility type makes all properties of a type optional?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"Partial\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:43',
        4
    ),
    (
        40,
        40,
        '{\"QuestionText\":\"What keyword is used in conditional types to extract a type?\",\"TextHtml\":null,\"QuestionTextExtended\":null,\"TextExtendedHtml\":null,\"Description\":null,\"DescriptionHtml\":null,\"ImageWasChanged\":true,\"License\":null,\"Visibility\":0,\"Solution\":\"infer\",\"SolutionDescription\":null,\"SolutionMetadataJson\":\"{\\\"IsCaseSensitive\\\":false,\\\"IsExtracInput\\\":false,\\\"IsDate\\\":false,\\\"IsNumber\\\":false,\\\"IsText\\\":true}\",\"CommentIds\":null}',
        1,
        0,
        1,
        '2026-01-29 12:12:44',
        4
    );
/*!40000 ALTER TABLE `questionchange` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `questionvaluation`
--

DROP TABLE IF EXISTS `questionvaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `QuestionId` (`QuestionId`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `questionvaluation`
--

LOCK TABLES `questionvaluation` WRITE;
/*!40000 ALTER TABLE `questionvaluation` DISABLE KEYS */
;
/*!40000 ALTER TABLE `questionvaluation` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `questionview`
--

DROP TABLE IF EXISTS `questionview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `questionview`
--

LOCK TABLES `questionview` WRITE;
/*!40000 ALTER TABLE `questionview` DISABLE KEYS */
;
/*!40000 ALTER TABLE `questionview` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `reference`
--

DROP TABLE IF EXISTS `reference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `Page_id` (`Page_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */
;
/*!40000 ALTER TABLE `reference` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `runningjob`
--

DROP TABLE IF EXISTS `runningjob`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `runningjob` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `StartAt` datetime DEFAULT NULL,
    `Name` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `runningjob`
--

LOCK TABLES `runningjob` WRITE;
/*!40000 ALTER TABLE `runningjob` DISABLE KEYS */
;
/*!40000 ALTER TABLE `runningjob` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `schemaversion`
--

DROP TABLE IF EXISTS `schemaversion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `schemaversion` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SchemaHash` varchar(64) DEFAULT NULL,
    `LastUpdated` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `schemaversion`
--

LOCK TABLES `schemaversion` WRITE;
/*!40000 ALTER TABLE `schemaversion` DISABLE KEYS */
;
INSERT INTO
    `schemaversion`
VALUES (
        1,
        '1e8e4156cb2c39417549b2610b85a1d926525ffdd7fa974d790fe5ecbbe38340',
        '2026-01-29 11:12:24'
    );
/*!40000 ALTER TABLE `schemaversion` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `setting` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AppVersion` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `setting`
--

LOCK TABLES `setting` WRITE;
/*!40000 ALTER TABLE `setting` DISABLE KEYS */
;
INSERT INTO
    `setting`
VALUES (
        1,
        2147483647,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:24'
    );
/*!40000 ALTER TABLE `setting` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `shares`
--

DROP TABLE IF EXISTS `shares`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `shares` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PageId` int DEFAULT NULL,
    `UserId` int DEFAULT NULL,
    `Token` varchar(255) NOT NULL,
    `Permission` int DEFAULT NULL,
    `GrantedBy` int NOT NULL,
    PRIMARY KEY (`Id`),
    KEY `UserId` (`UserId`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `shares`
--

LOCK TABLES `shares` WRITE;
/*!40000 ALTER TABLE `shares` DISABLE KEYS */
;
/*!40000 ALTER TABLE `shares` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    `FavoriteIds` text,
    `WikiOrder` text,
    `UiLanguage` varchar(255) DEFAULT NULL,
    `SubscriptionTokensBalance` int DEFAULT NULL,
    `PaidTokensBalance` int DEFAULT NULL,
    `PreferredAiModelId` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB AUTO_INCREMENT = 5 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */
;
INSERT INTO
    `user`
VALUES (
        1,
        '3782dbc3daef187d9e230c2122c2b1ff',
        '69107e83-9ff4-4290-8323-c8f715214d9d',
        'admin@memowikis.net',
        0,
        NULL,
        'Admin',
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        NULL,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:24',
        NULL,
        NULL,
        'en',
        0,
        0,
        NULL
    ),
    (
        2,
        'ca9c74813f6ec30fd8ba455c06cd9ea2',
        '9883beb3-1a9f-45f5-af85-3fd57abbbe59',
        'politics@memowikis.net',
        0,
        NULL,
        'Politics (Politics)',
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        NULL,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:24',
        NULL,
        NULL,
        'en',
        0,
        0,
        NULL
    ),
    (
        3,
        '96b07710a813d8e46bf2bd2f4d445c51',
        '32117446-f584-48c6-a32d-a80830306a97',
        'history@memowikis.net',
        0,
        NULL,
        'History (History)',
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        NULL,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:24',
        NULL,
        NULL,
        'en',
        0,
        0,
        NULL
    ),
    (
        4,
        '9cdb9ea2c515d49c4051c9832977f7a0',
        '78e9c21e-b28d-4b68-ba9c-25adc0ea83cf',
        'tech@memowikis.net',
        0,
        NULL,
        'Tech (Technology)',
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        0,
        NULL,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        0,
        0,
        '2026-01-29 12:12:24',
        '2026-01-29 12:12:24',
        NULL,
        NULL,
        'en',
        0,
        0,
        NULL
    );
/*!40000 ALTER TABLE `user` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `user_to_follower`
--

DROP TABLE IF EXISTS `user_to_follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `user_to_follower` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Follower_id` int DEFAULT NULL,
    `User_id` int DEFAULT NULL,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`),
    KEY `Follower_id` (`Follower_id`),
    KEY `User_id` (`User_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user_to_follower`
--

LOCK TABLES `user_to_follower` WRITE;
/*!40000 ALTER TABLE `user_to_follower` DISABLE KEYS */
;
/*!40000 ALTER TABLE `user_to_follower` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `useractivity`
--

DROP TABLE IF EXISTS `useractivity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
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
    KEY `UserCauser_id` (`UserCauser_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `useractivity`
--

LOCK TABLES `useractivity` WRITE;
/*!40000 ALTER TABLE `useractivity` DISABLE KEYS */
;
/*!40000 ALTER TABLE `useractivity` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Table structure for table `userskill`
--

DROP TABLE IF EXISTS `userskill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `userskill` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int DEFAULT NULL,
    `PageId` int DEFAULT NULL,
    `EvaluationJson` text,
    `DateCreated` datetime DEFAULT NULL,
    `DateModified` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `userskill`
--

LOCK TABLES `userskill` WRITE;
/*!40000 ALTER TABLE `userskill` DISABLE KEYS */
;
/*!40000 ALTER TABLE `userskill` ENABLE KEYS */
;
UNLOCK TABLES;

--
-- Dumping events for database 'memoWikis_dev'
--

--
-- Dumping routines for database 'memoWikis_dev'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */
;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */
;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */
;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */
;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */
;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */
;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */
;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */
;

-- Dump completed on 2026-01-29 11:12:47