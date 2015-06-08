CREATE TABLE `imagemetadata` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Type` int(11) DEFAULT NULL,
  `TypeId` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `Source` int(11) DEFAULT NULL,
  `SourceUrl` varchar(255) DEFAULT NULL,
  `LicenceInfo` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;