CREATE DATABASE IF NOT EXISTS `poc`;
USE `poc`;


CREATE TABLE IF NOT EXISTS `Histories`
(
    `Id`          CHAR(36)    NOT NULL PRIMARY KEY,
    `Json`       JSON        NULL,
    `CreatedAt`    DATETIME    NOT NULL,
    `UpdatedAt`    DATETIME    Not NULL,
    INDEX `IdxCreateAt` (`CreatedAt`)
)
DEFAULT CHARACTER SET = 'utf8mb4';


CREATE TABLE IF NOT EXISTS `Persons`
(
    `Id`          CHAR(36)    NOT NULL PRIMARY KEY,
    `Json`       JSON        NULL,
    `CreatedAt`    DATETIME    NOT NULL,
    `UpdatedAt`    DATETIME    Not NULL,
    INDEX `IdxCreateAt` (`CreatedAt`)
)
DEFAULT CHARACTER SET = 'utf8mb4';

