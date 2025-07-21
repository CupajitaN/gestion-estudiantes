-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: estudiantesdb
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250719184910_EstudianteMigracion','8.0.13'),('20250720030509_AddUsuarioTable','8.0.13'),('20250720132528_AddDescripcionToMateria','8.0.13'),('20250720140722_SeedInicialConUsuarios','8.0.13'),('20250721024213_RelacionEstudianteUsuarioNueva','8.0.13');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estudiantematerias`
--

DROP TABLE IF EXISTS `estudiantematerias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estudiantematerias` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EstudianteId` int NOT NULL,
  `MateriaId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_EstudianteMaterias_EstudianteId` (`EstudianteId`),
  KEY `IX_EstudianteMaterias_MateriaId` (`MateriaId`),
  CONSTRAINT `FK_EstudianteMaterias_Estudiantes_EstudianteId` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_EstudianteMaterias_Materias_MateriaId` FOREIGN KEY (`MateriaId`) REFERENCES `materias` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estudiantematerias`
--

LOCK TABLES `estudiantematerias` WRITE;
/*!40000 ALTER TABLE `estudiantematerias` DISABLE KEYS */;
INSERT INTO `estudiantematerias` VALUES (1,1,1),(2,1,3),(3,1,4),(4,2,2),(5,2,3),(6,2,4);
/*!40000 ALTER TABLE `estudiantematerias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estudiantes`
--

DROP TABLE IF EXISTS `estudiantes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estudiantes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Apellidos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Correo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Programa` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Jornada` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreditosMaximos` int NOT NULL DEFAULT '9',
  `CreditosUtilizados` int NOT NULL,
  `UsuarioId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Estudiantes_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_Estudiantes_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estudiantes`
--

LOCK TABLES `estudiantes` WRITE;
/*!40000 ALTER TABLE `estudiantes` DISABLE KEYS */;
INSERT INTO `estudiantes` VALUES (1,'Camila','Ríos','camila@uni.edu','Ingeniería de Sistemas','Mañana',9,9,2),(2,'Juan','Mejía','juan@uni.edu','Administración','Tarde',9,9,3);
/*!40000 ALTER TABLE `estudiantes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `materias`
--

DROP TABLE IF EXISTS `materias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `materias` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Creditos` int NOT NULL DEFAULT '3',
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materias`
--

LOCK TABLES `materias` WRITE;
/*!40000 ALTER TABLE `materias` DISABLE KEYS */;
INSERT INTO `materias` VALUES (1,'Matemáticas I',3,'Álgebra, aritmética y lógica básica'),(2,'Física General',3,'Conceptos fundamentales de la física clásica'),(3,'Programación I',3,'Fundamentos de programación estructurada'),(4,'Programación II',3,'Estructuras de datos y POO'),(5,'Bases de Datos',3,'Modelado relacional, SQL y transacciones'),(6,'Estadística',3,'Probabilidad, distribuciones y análisis estadístico'),(7,'Inglés Técnico',3,'Lectura y comprensión de textos técnicos en inglés'),(8,'Redes I',3,'Fundamentos de redes de computadoras y protocolos'),(9,'Ingeniería de Software',3,'Ciclo de vida del software y metodologías ágiles'),(10,'Lógica de Programación',3,'Pensamiento lógico aplicado a algoritmos');
/*!40000 ALTER TABLE `materias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profesores`
--

DROP TABLE IF EXISTS `profesores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profesores` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Apellidos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Correo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Jornada` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MateriasMax` int NOT NULL DEFAULT '2',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profesores`
--

LOCK TABLES `profesores` WRITE;
/*!40000 ALTER TABLE `profesores` DISABLE KEYS */;
INSERT INTO `profesores` VALUES (1,'Ana','Pérez','ana@uni.edu','Mañana',2),(2,'Luis','Martínez','luis@uni.edu','Tarde',2),(3,'Carlos','López','carlos@uni.edu','Noche',2),(4,'Diana','Torres','diana@uni.edu','Mañana',2),(5,'Mateo','Gómez','mateo@uni.edu','Tarde',2);
/*!40000 ALTER TABLE `profesores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profesormaterias`
--

DROP TABLE IF EXISTS `profesormaterias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profesormaterias` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProfesorId` int NOT NULL,
  `MateriaId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ProfesorMaterias_MateriaId` (`MateriaId`),
  KEY `IX_ProfesorMaterias_ProfesorId` (`ProfesorId`),
  CONSTRAINT `FK_ProfesorMaterias_Materias_MateriaId` FOREIGN KEY (`MateriaId`) REFERENCES `materias` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ProfesorMaterias_Profesores_ProfesorId` FOREIGN KEY (`ProfesorId`) REFERENCES `profesores` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profesormaterias`
--

LOCK TABLES `profesormaterias` WRITE;
/*!40000 ALTER TABLE `profesormaterias` DISABLE KEYS */;
INSERT INTO `profesormaterias` VALUES (1,1,1),(2,1,2),(3,2,3),(4,3,4);
/*!40000 ALTER TABLE `profesormaterias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Correo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ContrasenaHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Rol` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EstudianteId` int DEFAULT NULL,
  `ProfesorId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Usuarios_ProfesorId` (`ProfesorId`),
  CONSTRAINT `FK_Usuarios_Profesores_ProfesorId` FOREIGN KEY (`ProfesorId`) REFERENCES `profesores` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'admin@correo.com','jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=','admin',NULL,NULL),(2,'camila@uni.edu','PIrFfCHXvPxnBJ2NTO9/1gn0O129EC0m+qwN0+43ml0=','estudiante',1,NULL),(3,'juan@uni.edu','9syz6NYJASI4wLOeYLLJYys83t6R4DXa0d5DRpdo9Mw=','estudiante',2,NULL),(4,'ana@uni.edu','6CgnsAssqGIL6zf4eXeMCCspKlInA5DP81tv4xV/Tos=','profesor',NULL,1),(5,'luis@uni.edu','7HkI3IJB8OQ0AmaZDf5gAbF1cITYkcZ1i/qsgmdQAJo=','profesor',NULL,2);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'estudiantesdb'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-21  9:21:35
