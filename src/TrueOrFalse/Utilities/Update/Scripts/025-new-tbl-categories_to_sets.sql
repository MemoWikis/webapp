CREATE TABLE `categories_to_sets` (
  `Set_id` int(11) NOT NULL,
  `Category_id` int(11) NOT NULL,
  KEY `Category_id` (`Category_id`),
  KEY `Set_id` (`Set_id`),
  CONSTRAINT `FK937EA8C286903877` FOREIGN KEY (`Set_id`) REFERENCES `questionset` (`Id`),
  CONSTRAINT `FK937EA8C2253EAD1B` FOREIGN KEY (`Category_id`) REFERENCES `category` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;