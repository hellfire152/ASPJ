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
-- Table structure for table `banhistory`
--

DROP TABLE IF EXISTS `banhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `banhistory` (
  `idbanhistory` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) DEFAULT NULL,
  `banPeriod` varchar(45) DEFAULT NULL,
  `banReason` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idbanhistory`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `banhistory`
--

LOCK TABLES `banhistory` WRITE;
/*!40000 ALTER TABLE `banhistory` DISABLE KEYS */;
INSERT INTO `banhistory` VALUES (1,'reason','1 month','u suck'),(2,'reas','1 month','u suck'),(3,'killjohn2017','0',NULL),(4,'killjohn2017','0',NULL),(5,'killjohn2017','0',NULL),(6,'killjohn2017','0',NULL),(7,'killjohn2017','0',NULL),(8,'killjohn2017','0','Cheating'),(9,'killjohn2017','0','SuspiciousTransaction'),(10,'killjohn2017','0','SuspiciousTransaction'),(11,'killjohn2017','0','SuspiciousTransaction'),(12,'killjohn2017','0','SuspiciousTransaction'),(13,'killjohn2017','0','SuspiciousTransaction'),(14,'killjohn2017','0','SuspiciousTransaction'),(15,'killjohn2017','0','SuspiciousTransaction'),(16,'jhn905',NULL,NULL),(17,'jhn905','1 Month','Cheating'),(18,'jhn905','1 Week','Cheating');
/*!40000 ALTER TABLE `banhistory` ENABLE KEYS */;
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
  `transactionNo` varchar(100) NOT NULL,
  `transactionDesc` longtext,
  `priceOfBeans` double NOT NULL,
  `userBeansBefore` int(20) DEFAULT NULL,
  `userBeansAfter` int(20) DEFAULT NULL,
  `status` varchar(45) NOT NULL,
  `dateOfTransaction` datetime NOT NULL,
  `userID` varchar(45) NOT NULL,
  PRIMARY KEY (`transactionNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `beantransaction`
--

LOCK TABLES `beantransaction` WRITE;
/*!40000 ALTER TABLE `beantransaction` DISABLE KEYS */;
INSERT INTO `beantransaction` VALUES ('5qk2lgHQItuNj7WleVTTn2V0SdlvKuMnLIB/ThzfYC8=','ml0jSK0XJ2S38zBdUDXmDr8QxTLSbWELDHfftkSUhGgD6VeZSPqUNzcnpmWBYgeN',12.99,9650,10050,'Successful','2018-02-05 01:40:49','v/hf1tBt1EfPLENvnwNpYg=='),('aqYsgBuQg7mteyB5d6apbSuZ2SB8O2OGaNajB3DhEBQ=','1P6zLLRNOiSrJdmrF88QodsDob7O6iOEqNk4wk1NxOGmxVxxWWAt80AwTiln4gAl',12.99,9900,10300,'Successful','2018-02-05 01:50:54','v/hf1tBt1EfPLENvnwNpYg=='),('bzqAap+m+mtp1fYvQHVmpn23a7ACwENKZCUDkoSY5E4=','f2LE+2WUJ7pYDFeJqmxgV+pRBSCdua1A7pEBb2invokNHAORpsB0ki9Ds3dtQ/br',23.99,0,1100,'Successful','2018-02-05 19:01:48','F4SbhsOOkMMPJOhDG2lLNA=='),('hDk/Y+U44nGLbLf+ZhoHIpzdl7xB9g4gMVVGZMlAzsw=','/w+JqsLIA4Kj8vz+2l7tMAf9wnV/0/zhUPgOipAwADaaObSm0uVZU5zidW/GYYJ3NMEzjz527SoaV917UvocYA==',23.99,8550,9650,'Successful','2018-02-05 01:32:58','v/hf1tBt1EfPLENvnwNpYg=='),('n891p+EnAqzgSlTm7kQQpxqG6zs8q+t01iPXm2KKP3Y=','3TsUCtXR8ABGldj5luyFl7tFgdWDcyLIaWvLJSJT9qHgIox1k3v2GdLTAklVjNR2',2.99,10150,10210,'Successful','2018-02-05 01:55:26','v/hf1tBt1EfPLENvnwNpYg=='),('nclET8vJI43kL5BGcJsPhmybYpmWkgdrgziBoa578oA=','f2LE+2WUJ7pYDFeJqmxgV+pRBSCdua1A7pEBb2invokNHAORpsB0ki9Ds3dtQ/br',23.99,9760,10860,'Successful','2018-02-05 14:18:34','v/hf1tBt1EfPLENvnwNpYg=='),('UbnV6m7e/RxjW/8ooPcsNhf1yDzhxbZEiMT3ClRg2FQ=','f2LE+2WUJ7pYDFeJqmxgV+pRBSCdua1A7pEBb2invokNHAORpsB0ki9Ds3dtQ/br',23.99,0,1100,'Successful','2018-02-06 07:30:54','fJtdpgyF1Af3D8l9K65rAg=='),('uWQOTke2COJaao7uyMXYonj79WvherOSz/1ZBzCVPLM=','GbYERhBbKIy+mudUz0FImuezZiS+DKKUW/1pSfUcnf//1ZMXD6IwXsMS++NeluUu',5.99,0,150,'Successful','2018-02-05 21:04:18','2DLJX7//XMTMUoiUKZVMyQ==');
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
  `username` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`chatId`),
  UNIQUE KEY `chatId_UNIQUE` (`chatId`)
) ENGINE=InnoDB AUTO_INCREMENT=238 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chat`
--

LOCK TABLES `chat` WRITE;
/*!40000 ALTER TABLE `chat` DISABLE KEYS */;
INSERT INTO `chat` VALUES (225,'BMxsoyEc+ni1Zdgxd+0OFA==','05/02/2018','05/02/2018 06:19 PM','fish123'),(226,'eNTDvni5z/oEKZ2QB5h/yQ==','05/02/2018','05/02/2018 06:19 PM','testuser'),(227,'qruoHJ8dvqQjBiVlv1QsP+lsw3xumaR8PmD+xT33ZyY=','05/02/2018','05/02/2018 06:19 PM','fish123'),(228,'KvMV6ZEl5aHeid+AIrwJeA==','05/02/2018','05/02/2018 06:19 PM','testuser'),(229,'Ndj/vLqwabyYlSngJTzdvw==','05/02/2018','05/02/2018 06:19 PM','fish123'),(230,'qIqXJUogBOGNrbPF7ir4+A==','05/02/2018','05/02/2018 06:19 PM','testuser'),(231,'Mhd2NQQeHv8hdhRAZulVyqKdeZhUWpatGfHAI0y07Ho=','05/02/2018','05/02/2018 07:03 PM','euniceSolo'),(232,'pMeVgbgA8cPKAwiEcUaRVg==','05/02/2018','05/02/2018 07:34 PM','euniceSolo'),(233,'Y05nt0yQGafshqy4zXjNLg==','05/02/2018','05/02/2018 07:34 PM','euniceSolo'),(234,'KA4vPtxeEl9scaBeJdJ/fw==','05/02/2018','05/02/2018 08:20 PM','euniceSolo'),(235,'uznmyRG0kkAs8EdF1yDjaQ==','05/02/2018','05/02/2018 08:20 PM','euniceSolo'),(236,'Fzj3LoTYfwLeTnuM1qw2eQ==','05/02/2018','05/02/2018 08:21 PM','euniceSolo'),(237,'xbdmECBonU1rwZ1mutwExA==','05/02/2018','05/02/2018 08:24 PM','euniceSolo');
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
INSERT INTO `cheatlog` VALUES (52,1517840053720),(55,1517864205443),(55,1517864298462);
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
INSERT INTO `equippeditems` VALUES (48,3,2),(49,3,2),(52,1,2),(53,3,NULL),(54,1,NULL),(55,1,NULL);
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
INSERT INTO `inventory` VALUES (52,1),(54,1),(55,1),(48,2),(52,2),(52,3),(53,3);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itemtransaction`
--

DROP TABLE IF EXISTS `itemtransaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `itemtransaction` (
  `transactionNo` varchar(100) NOT NULL,
  `transactionDesc` longtext,
  `priceOfItem` int(20) NOT NULL,
  `userBeansBefore` int(20) NOT NULL,
  `userBeansAfter` int(20) NOT NULL,
  `userID` varchar(45) NOT NULL,
  `dateOfTransaction` datetime NOT NULL,
  PRIMARY KEY (`transactionNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itemtransaction`
--

LOCK TABLES `itemtransaction` WRITE;
/*!40000 ALTER TABLE `itemtransaction` DISABLE KEYS */;
INSERT INTO `itemtransaction` VALUES ('0IwyXXNIw+qVH9qrdKOQd9iE+LaXmJGNiHwVWaU8Q6k=','AnGWpWx7fhdlyqndmasRPwTpsSn5EVFZWSbyGRTX38yHtSwA1qAvgB6E8tuUfrhA',100,150,50,'2DLJX7//XMTMUoiUKZVMyQ==','2018-02-05 21:05:45'),('Mb0sY8TuiStDzu9hJe9uT+T9DM1D615GL14F/NEWRE8=','AnGWpWx7fhdlyqndmasRPwTpsSn5EVFZWSbyGRTX38yHtSwA1qAvgB6E8tuUfrhA',100,9860,9760,'v/hf1tBt1EfPLENvnwNpYg==','2018-02-05 01:58:56'),('Mc3XNBF6awv1ThEZ4UM3njlATGYIgGj/T7NPYDyKrvE=','9/4acNiNgukLA/Taazb81ZWKOusWbz94owdA2nd5v5WHDIEMD2lzgAUwoUisReIX',200,10060,9860,'v/hf1tBt1EfPLENvnwNpYg==','2018-02-05 01:58:54'),('MVd4VewF/voIqEupMOt4Hap+cqNlslIYhLVIMlLBIi4=','AnGWpWx7fhdlyqndmasRPwTpsSn5EVFZWSbyGRTX38yHtSwA1qAvgB6E8tuUfrhA',100,1100,1000,'F4SbhsOOkMMPJOhDG2lLNA==','2018-02-05 19:01:56'),('QjWNZnFMfT21qvZWb9CQpcKBBliFO34/Sz1ksUx5ckA=','Zck+d32tzechVT9kiuH7m70NG3GzWR/sI3GeHAitEMfjSoL2Hp4GeZ5uF0bzqarn',150,1100,950,'fJtdpgyF1Af3D8l9K65rAg==','2018-02-06 07:31:07'),('y4pmuPWXMEvi7YHmsTDb+crBvKEtesi7Pk85dZSAkeQ=','Zck+d32tzechVT9kiuH7m70NG3GzWR/sI3GeHAitEMfjSoL2Hp4GeZ5uF0bzqarn',150,10210,10060,'v/hf1tBt1EfPLENvnwNpYg==','2018-02-05 01:55:39');
/*!40000 ALTER TABLE `itemtransaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `premiumitem`
--

DROP TABLE IF EXISTS `premiumitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `premiumitem` (
  `itemID` int(11) NOT NULL AUTO_INCREMENT,
  `itemName` varchar(45) NOT NULL,
  `itemType` varchar(45) NOT NULL,
  `itemDescription` varchar(60) NOT NULL,
  `beansPrice` double NOT NULL,
  `itemImage` varchar(45) DEFAULT NULL,
  `dateStart` datetime DEFAULT NULL,
  `dateEnd` datetime DEFAULT NULL,
  PRIMARY KEY (`itemID`),
  UNIQUE KEY `itemID_UNIQUE` (`itemID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='Premium Item Data Table';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `premiumitem`
--

LOCK TABLES `premiumitem` WRITE;
/*!40000 ALTER TABLE `premiumitem` DISABLE KEYS */;
INSERT INTO `premiumitem` VALUES (1,'Fedora Hat','Hat','Mi\'lady.',100,'ImageFedora',NULL,NULL),(2,'Suit Pajamas','Outfit','Nothing suits you like a suit.',200,'ImageSuit',NULL,NULL),(3,'Karate Headband','Hat','Hiya!',150,'ImageKarate',NULL,NULL);
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
INSERT INTO `savetime` VALUES (50,1517752226703,1517833984257,1517834029184),(52,1517840052804,1517840053720,1517840331731),(54,0,1517857265501,1517857265501),(55,1517864205443,1517864233435,1517864298462);
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
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
  `password` varchar(200) NOT NULL,
  `firstName` varchar(45) NOT NULL,
  `lastName` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `phoneNumber` varchar(130) NOT NULL,
  `beansAmount` int(20) NOT NULL DEFAULT '0',
  `role` varchar(45) NOT NULL DEFAULT 'Player',
  `isBan` varchar(45) NOT NULL DEFAULT 'False',
  `banTill` datetime DEFAULT NULL,
  `banID` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (50,'hellfire153','eTrD3BHNPT3n7dQm3qKskh1k5H99EeviZWcdvwANIFQ=','Ang','Kuan','angjinkuan@gmail.com','+6598785187',0,'Player','true','2018-02-06 03:42:13',NULL),(52,'jhn905','BISjcAZEEjFiw+wMqyZnD9WyRUST0s7G334KE/krG7Y=','John','Foo','johnfoohw@gmail.com','+6596345225',9760,'Player','true','2018-02-13 03:48:17',NULL),(53,'adminAccount','CYStlv3qVDCC0hMMVeF0w4WcSA/BOg3eFNFOQOSNiKYwiGiTlnAqqxU6CopvhRc7','Admin','Boss','janefoohw@gmail.com','Z/9dymncduKqQ5KSziVOBg==',950,'Admin','False',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vote`
--

LOCK TABLES `vote` WRITE;
/*!40000 ALTER TABLE `vote` DISABLE KEYS */;
INSERT INTO `vote` VALUES (106,1,0,NULL,'Tommy merlyn',21);
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

-- Dump completed on 2018-02-06  7:54:58
