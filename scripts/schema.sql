SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema ltbdb2
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `ltbdb2` DEFAULT CHARACTER SET utf8 ;
USE `ltbdb2` ;

-- -----------------------------------------------------
-- Table `ltbdb2`.`ltbdb_categories`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ltbdb2`.`ltbdb_categories` (
  `catid` INT(11) NOT NULL AUTO_INCREMENT,
  `name` TEXT NOT NULL,
  PRIMARY KEY (`catid`),
  INDEX `TEXT` USING BTREE (`name`(20) ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ltbdb2`.`ltbdb_books`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ltbdb2`.`ltbdb_books` (
  `bookid` INT(11) NOT NULL AUTO_INCREMENT,
  `name` TEXT NOT NULL,
  `number` INT(11) NOT NULL,
  `catid` INT(11) NOT NULL,
  `image` TEXT NULL DEFAULT NULL,
  `added` DATETIME NOT NULL,
  PRIMARY KEY (`bookid`),
  INDEX `TEXT` USING BTREE (`name`(20) ASC),
  INDEX `fk_cat` (`catid` ASC),
  CONSTRAINT `fk_cat`
    FOREIGN KEY (`catid`)
    REFERENCES `ltbdb2`.`ltbdb_categories` (`catid`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 377
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ltbdb2`.`ltbdb_stories`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ltbdb2`.`ltbdb_stories` (
  `storyid` INT(11) NOT NULL AUTO_INCREMENT,
  `name` TEXT NOT NULL,
  `bookid` INT(11) NOT NULL,
  PRIMARY KEY (`storyid`),
  INDEX `TEXT` USING BTREE (`name`(20) ASC),
  INDEX `fk_book` (`bookid` ASC),
  CONSTRAINT `fk_book`
    FOREIGN KEY (`bookid`)
    REFERENCES `ltbdb2`.`ltbdb_books` (`bookid`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 84
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ltbdb2`.`ltbdb_tags`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ltbdb2`.`ltbdb_tags` (
  `tagid` INT(11) NOT NULL AUTO_INCREMENT,
  `name` TEXT NOT NULL,
  PRIMARY KEY (`tagid`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ltbdb2`.`ltbdb_tag2book`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ltbdb2`.`ltbdb_tag2book` (
  `tag2book` INT(11) NOT NULL AUTO_INCREMENT,
  `tagid` INT(11) NOT NULL,
  `bookid` INT(11) NOT NULL,
  PRIMARY KEY (`tag2book`),
  INDEX `fk_tag_idx` (`tagid` ASC),
  INDEX `fk_book_idx` (`bookid` ASC),
  CONSTRAINT `fk_book2`
    FOREIGN KEY (`bookid`)
    REFERENCES `ltbdb2`.`ltbdb_books` (`bookid`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tag`
    FOREIGN KEY (`tagid`)
    REFERENCES `ltbdb2`.`ltbdb_tags` (`tagid`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
