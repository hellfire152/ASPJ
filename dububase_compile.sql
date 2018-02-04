CREATE DATABASE  IF NOT EXISTS `dububase` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `dububase`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: dububase
-- ------------------------------------------------------
-- Server version	5.7.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `activation`
--

DROP TABLE IF EXISTS `activation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `activation` (
  `activationCode` varchar(45) NOT NULL,
  `userID` int(10) DEFAULT NULL,
  PRIMARY KEY (`activationCode`),
  UNIQUE KEY `activationCode_UNIQUE` (`activationCode`),
  KEY `userID_idx` (`userID`),
  CONSTRAINT `userID` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activation`
--

LOCK TABLES `activation` WRITE;
/*!40000 ALTER TABLE `activation` DISABLE KEYS */;
/*!40000 ALTER TABLE `activation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `beanpurchases`
--

DROP TABLE IF EXISTS `beanpurchases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `beanpurchases` (
  `beanName` varchar(40) NOT NULL,
  `beanPrice` double NOT NULL,
  `beanAmount` int(10) NOT NULL,
  `beanIcon` varchar(45) NOT NULL,
  PRIMARY KEY (`beanPrice`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `beanpurchases`
--

LOCK TABLES `beanpurchases` WRITE;
/*!40000 ALTER TABLE `beanpurchases` DISABLE KEYS */;
INSERT INTO `beanpurchases` VALUES ('Cup Of Beans',2.99,60,'fa-coffee'),('Basket Of Beans',5.99,150,'fa-shopping-basket'),('Bag Of Beans',12.99,400,'fa-shopping-bag'),('Box Of Beans',23.99,1100,'fa-archive'),('Truck Of Beans',52.99,3200,'fa-truck'),('Ship Of Beans',75.99,7000,'fa-ship');
/*!40000 ALTER TABLE `beanpurchases` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `beantransaction`
--

DROP TABLE IF EXISTS `beantransaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `beantransaction` (
  `transactionNo` varchar(20) NOT NULL,
  `transactionDesc` longtext,
  `priceOfBeans` double NOT NULL,
  `userBeansBefore` int(20) NOT NULL,
  `userBeansAfter` int(20) NOT NULL,
  `transactionUserID` int(11) NOT NULL,
  PRIMARY KEY (`transactionNo`),
  KEY `userID_idx` (`transactionUserID`),
  CONSTRAINT `transactionUserID` FOREIGN KEY (`transactionUserID`) REFERENCES `users` (`userID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `beantransaction`
--

LOCK TABLES `beantransaction` WRITE;
/*!40000 ALTER TABLE `beantransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `beantransaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chat`
--

DROP TABLE IF EXISTS `chat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chat` (
  `chatId` int(11) NOT NULL AUTO_INCREMENT,
  `chatMessage` varchar(200) NOT NULL,
  `chatDate` varchar(30) NOT NULL,
  `chatTime` varchar(30) NOT NULL,
  PRIMARY KEY (`chatId`),
  UNIQUE KEY `chatId_UNIQUE` (`chatId`)
) ENGINE=InnoDB AUTO_INCREMENT=179 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chat`
--

LOCK TABLES `chat` WRITE;
/*!40000 ALTER TABLE `chat` DISABLE KEYS */;
INSERT INTO `chat` VALUES (176,'SDVmw83CfHi5kGnYPdVv1g==','q6p1cQ4Nt90FDZknVrn5HQ==','50wRZJxRyXevmYrWTVnCmQ=='),(177,'yPPjqzG8XgEqhJEO6RQk/A==','q6p1cQ4Nt90FDZknVrn5HQ==','znLXj7ngCaFC0XPLGBehYQ=='),(178,'Rfu6CR4spzE8pfe0Mvno3w==','q6p1cQ4Nt90FDZknVrn5HQ==','c8e0dAyo3r4sQtpRbUfDZA==');
/*!40000 ALTER TABLE `chat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cheatlog`
--

DROP TABLE IF EXISTS `cheatlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cheatlog` (
  `userID` int(11) NOT NULL,
  `time` bigint(20) NOT NULL,
  PRIMARY KEY (`userID`,`time`),
  CONSTRAINT `cheatlogID` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cheatlog`
--

LOCK TABLES `cheatlog` WRITE;
/*!40000 ALTER TABLE `cheatlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `cheatlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comment`
--

DROP TABLE IF EXISTS `comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `content` varchar(500) NOT NULL,
  `username` varchar(45) NOT NULL,
  `date` datetime NOT NULL,
  `threadid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `threadId_idx` (`threadid`),
  CONSTRAINT `threadId` FOREIGN KEY (`threadid`) REFERENCES `thread` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comment`
--

LOCK TABLES `comment` WRITE;
/*!40000 ALTER TABLE `comment` DISABLE KEYS */;
INSERT INTO `comment` VALUES (47,'weee','Tommy merlyn','2018-02-04 13:04:54',11);
/*!40000 ALTER TABLE `comment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `equippeditems`
--

DROP TABLE IF EXISTS `equippeditems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `equippeditems` (
  `userID` int(11) NOT NULL,
  `equippedHat` int(11) DEFAULT NULL,
  `equippedOutfit` int(11) DEFAULT NULL,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `equippeditems`
--

LOCK TABLES `equippeditems` WRITE;
/*!40000 ALTER TABLE `equippeditems` DISABLE KEYS */;
INSERT INTO `equippeditems` VALUES (48,3,2),(49,3,2);
/*!40000 ALTER TABLE `equippeditems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inventory` (
  `userID` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  PRIMARY KEY (`userID`,`itemID`),
  KEY `itemID_idx` (`itemID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` VALUES (48,2);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itemtransaction`
--

DROP TABLE IF EXISTS `itemtransaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `itemtransaction` (
  `transactionNo` varchar(20) NOT NULL,
  `transactionDesc` varchar(45) DEFAULT NULL,
  `priceOfItem` int(20) NOT NULL,
  `userBeansBefore` int(20) NOT NULL,
  `userBeansAfter` int(20) NOT NULL,
  `userID` varchar(45) NOT NULL,
  PRIMARY KEY (`transactionNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itemtransaction`
--

LOCK TABLES `itemtransaction` WRITE;
/*!40000 ALTER TABLE `itemtransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `itemtransaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `premiumitem`
--

DROP TABLE IF EXISTS `premiumitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `premiumitem` (
  `itemID` int(11) NOT NULL,
  `itemName` varchar(45) NOT NULL,
  `itemType` varchar(45) NOT NULL,
  `itemDescription` varchar(60) NOT NULL,
  `beansPrice` double NOT NULL,
  `itemImage` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`itemID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Premium Item Data Table';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `premiumitem`
--

LOCK TABLES `premiumitem` WRITE;
/*!40000 ALTER TABLE `premiumitem` DISABLE KEYS */;
INSERT INTO `premiumitem` VALUES (1,'Fedora Hat','Hat','Mi\'lady.',100,'ImageFedora'),(2,'Suit Pajamas','Outfit','Nothing suits you like a suit.',200,'ImageSuit'),(3,'Karate Headband','Hat','Hiya!',150,'ImageKarate');
/*!40000 ALTER TABLE `premiumitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `resetpasswordrequest`
--

DROP TABLE IF EXISTS `resetpasswordrequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resetpasswordrequest` (
  `ID` varchar(45) NOT NULL,
  `userID` int(10) DEFAULT NULL,
  `resetRequestDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `idresetpasswordrequest_UNIQUE` (`ID`),
  KEY `userID_idx` (`userID`),
  CONSTRAINT `userID_REQ` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `resetpasswordrequest`
--

LOCK TABLES `resetpasswordrequest` WRITE;
/*!40000 ALTER TABLE `resetpasswordrequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `resetpasswordrequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `saveaccess`
--

DROP TABLE IF EXISTS `saveaccess`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `saveaccess` (
  `userID` int(11) NOT NULL,
  `code` varchar(128) NOT NULL,
  PRIMARY KEY (`userID`,`code`),
  CONSTRAINT `id` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saveaccess`
--

LOCK TABLES `saveaccess` WRITE;
/*!40000 ALTER TABLE `saveaccess` DISABLE KEYS */;
/*!40000 ALTER TABLE `saveaccess` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `savetime`
--

DROP TABLE IF EXISTS `savetime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `savetime` (
  `userID` int(11) NOT NULL,
  `time1` bigint(20) DEFAULT NULL,
  `time2` bigint(20) DEFAULT NULL,
  `time3` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`userID`),
  CONSTRAINT `user` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `savetime`
--

LOCK TABLES `savetime` WRITE;
/*!40000 ALTER TABLE `savetime` DISABLE KEYS */;
INSERT INTO `savetime` VALUES (50,0,1517752226703,1517752226703);
/*!40000 ALTER TABLE `savetime` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `thread`
--

DROP TABLE IF EXISTS `thread`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `thread` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(50) NOT NULL,
  `content` varchar(1000) NOT NULL,
  `date` datetime NOT NULL,
  `votes` int(11) NOT NULL DEFAULT '0',
  `imageName` varchar(45) DEFAULT NULL,
  `username` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `thread`
--

LOCK TABLES `thread` WRITE;
/*!40000 ALTER TABLE `thread` DISABLE KEYS */;
INSERT INTO `thread` VALUES (11,'Hello world','Testing yoyo','2014-12-01 00:00:00',0,NULL,'Malcolm Merlyn'),(21,'weeeeweeeeweeeeweeeeweeeeweeeeweeeeweeeeweeeeweee','SUP DUP','2018-02-04 11:59:24',0,'','Tommy merlyn');
/*!40000 ALTER TABLE `thread` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `firstName` varchar(45) NOT NULL,
  `lastName` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `phoneNumber` varchar(45) NOT NULL,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (50,'hellfire153','eTrD3BHNPT3n7dQm3qKskh1k5H99EeviZWcdvwANIFQ=','Ang','Kuan','angjinkuan@gmail.com','+6598785187');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vote`
--

DROP TABLE IF EXISTS `vote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vote` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `upvote` tinyint(1) NOT NULL DEFAULT '0',
  `downvote` tinyint(1) NOT NULL DEFAULT '0',
  `userid` varchar(45) DEFAULT NULL,
  `username` varchar(45) NOT NULL,
  `threadId` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `threadId_idx` (`threadId`),
  CONSTRAINT `threadIdReference` FOREIGN KEY (`threadId`) REFERENCES `thread` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=102 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vote`
--

LOCK TABLES `vote` WRITE;
/*!40000 ALTER TABLE `vote` DISABLE KEYS */;
INSERT INTO `vote` VALUES (101,1,0,NULL,'Tommy merlyn',21);
/*!40000 ALTER TABLE `vote` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-02-04 13:53:43
