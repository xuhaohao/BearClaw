/*
Navicat MySQL Data Transfer

Source Server         : 100.0.12.236
Source Server Version : 50717
Source Host           : 100.0.12.236:3306
Source Database       : bearclaw

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2018-10-19 19:50:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for jobs
-- ----------------------------
DROP TABLE IF EXISTS `jobs`;
CREATE TABLE `jobs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `Tel` varchar(255) DEFAULT NULL,
  `Sended` int(16) NOT NULL DEFAULT '0',
  `Address` varchar(512) DEFAULT NULL,
  `Ext1` varchar(255) DEFAULT NULL,
  `Ext2` varchar(255) DEFAULT NULL,
  `TimeTag` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of jobs
-- ----------------------------

-- ----------------------------
-- Table structure for params
-- ----------------------------
DROP TABLE IF EXISTS `params`;
CREATE TABLE `params` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FieldGroup` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Value` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of params
-- ----------------------------
