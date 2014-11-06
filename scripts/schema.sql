CREATE DATABASE `ltbdb2` DEFAULT CHARACTER SET utf8;

USE `ltbdb2`;

CREATE TABLE `books` (
  `bookid` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  `number` int(11) NOT NULL,
  `catid` int(11) NOT NULL,
  `image` text,
  `added` datetime NOT NULL,
  `stories` text,
  PRIMARY KEY (`bookid`),
  KEY `TEXT` (`name`(20)) USING BTREE,
  KEY `fk_cat` (`catid`),
  CONSTRAINT `fk_cat` FOREIGN KEY (`catid`) REFERENCES `categories` (`catid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `categories` (
  `catid` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`catid`),
  KEY `TEXT` (`name`(20)) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `tag2book` (
  `tag2book` int(11) NOT NULL AUTO_INCREMENT,
  `tagid` int(11) NOT NULL,
  `bookid` int(11) NOT NULL,
  PRIMARY KEY (`tag2book`),
  KEY `fk_tag_idx` (`tagid`),
  KEY `fk_book_idx` (`bookid`),
  CONSTRAINT `fk_book2` FOREIGN KEY (`bookid`) REFERENCES `books` (`bookid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_tag` FOREIGN KEY (`tagid`) REFERENCES `tags` (`tagid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `tags` (
  `tagid` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`tagid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;