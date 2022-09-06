CREATE DATABASE IF NOT EXISTS `poc`;
USE `poc`;

CREATE TABLE IF NOT EXISTS `PersonVirtualActor`
(
    `Id`          VARCHAR(60)    NOT NULL PRIMARY KEY,
    `Json`       JSON        NULL,
    `CreatedDate`    DATETIME    NOT NULL,
    `UpdatedDate`    DATETIME    Not NULL,
    INDEX `IdxCreateAt` (`CreatedDate`)
)
DEFAULT CHARACTER SET = 'utf8mb4';

