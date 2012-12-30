CREATE TABLE `questionset` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Text` text,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `Creator_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Creator_id` (`Creator_id`),
  CONSTRAINT `FKFFF30CD2DE1D1D36` FOREIGN KEY (`Creator_id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `questionset_to_question` (
  `QuestionSet_id` int(11) NOT NULL,
  `Question_id` int(11) NOT NULL,
  KEY `Question_id` (`Question_id`),
  KEY `QuestionSet_id` (`QuestionSet_id`),
  CONSTRAINT `FKF5E8FE5D3A925A27` FOREIGN KEY (`QuestionSet_id`) REFERENCES `questionset` (`Id`),
  CONSTRAINT `FKF5E8FE5D20054BCB` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;