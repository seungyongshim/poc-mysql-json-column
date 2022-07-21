CREATE DATABASE IF NOT EXISTS `poc`;
USE `poc`;


CREATE TABLE IF NOT EXISTS `Histories`
(
    `Id` CHAR(36)  NOT NULL PRIMARY KEY,
    `Object`  JSON       NULL,
    `CreateAt`    DATETIME    NOT NULL,
    `UpdateAt`    DATETIME    NULL
)
DEFAULT CHARACTER SET = 'utf8mb4';
