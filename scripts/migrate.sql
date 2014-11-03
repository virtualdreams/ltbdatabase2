/*
-- Create Schema
CREATE SCHEMA `db` DEFAULT CHARACTER SET utf8 ;
show create table ltbdb3.ltbdb_stories;

-- Datenbank klonen
-- mysqldump -h [server] -u [user] -p[password] db1 | mysql -h [server] -u [user] -p[password] db2
-- mysqldump --no-data ...
*/

use ltbdb2;

-- rename
ALTER TABLE `ltbdb_books` 
	RENAME TO  `books`;

ALTER TABLE `ltbdb_categories` 
	RENAME TO `categories`;

ALTER TABLE `ltbdb_stories` 
	RENAME TO `stories`;
	
-- add new column	
ALTER TABLE `books` 
	ADD COLUMN `stories` TEXT NULL AFTER `name`;

-- copy stories to book
update `books` as b 
	set stories = 
	(
		select group_concat(s.name separator '|') from `stories` as s where b.bookid = s.bookid group by s.bookid
	);

-- drop table
DROP TABLE `stories`;

-- create table tags
CREATE TABLE `tags` (
  `tagid` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`tagid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- create table tag2book
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


