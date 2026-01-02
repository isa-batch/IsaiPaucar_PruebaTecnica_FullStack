-- Script de creación de base de datos para Sistema de Gestión de Tareas
-- Base de datos: gestion_tareas_db

CREATE DATABASE IF NOT EXISTS gestion_tareas_db;
USE gestion_tareas_db;

-- TABLA: usuarios
CREATE TABLE `usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password_hash` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `estado` int NOT NULL DEFAULT '1',
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  `modificado_en` datetime DEFAULT NULL,
  `modificado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`),
  KEY `idx_email` (`email`),
  KEY `idx_estado` (`estado`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TABLA: proyectos
CREATE TABLE `proyectos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` varchar(500) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `propietario_id` int NOT NULL,
  `estado` int NOT NULL DEFAULT '1' COMMENT '0=eliminado, 1=activo',
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  `modificado_en` datetime DEFAULT NULL,
  `modificado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `creado_por` (`creado_por`),
  KEY `modificado_por` (`modificado_por`),
  KEY `idx_propietario` (`propietario_id`),
  KEY `idx_estado` (`estado`),
  CONSTRAINT `proyectos_ibfk_1` FOREIGN KEY (`propietario_id`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `proyectos_ibfk_2` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `proyectos_ibfk_3` FOREIGN KEY (`modificado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TABLA: proyecto_usuarios (relación N:N)
CREATE TABLE `proyecto_usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `proyecto_id` int NOT NULL,
  `usuario_id` int NOT NULL,
  `rol` int NOT NULL COMMENT '1=OWNER, 2=MEMBER',
  `estado` int NOT NULL DEFAULT '1' COMMENT '0=eliminado, 1=activo',
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_proyecto_usuario` (`proyecto_id`,`usuario_id`),
  KEY `creado_por` (`creado_por`),
  KEY `idx_proyecto` (`proyecto_id`),
  KEY `idx_usuario` (`usuario_id`),
  KEY `idx_estado` (`estado`),
  CONSTRAINT `proyecto_usuarios_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`) ON DELETE CASCADE,
  CONSTRAINT `proyecto_usuarios_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `proyecto_usuarios_ibfk_3` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TABLA: invitaciones_proyecto
CREATE TABLE `invitaciones_proyecto` (
  `id` int NOT NULL AUTO_INCREMENT,
  `proyecto_id` int NOT NULL,
  `usuario_id` int NOT NULL,
  `estado_invitacion` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'PENDIENTE' COMMENT 'PENDIENTE, ACEPTADA, RECHAZADA',
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `creado_por` (`creado_por`),
  KEY `idx_proyecto` (`proyecto_id`),
  KEY `idx_usuario` (`usuario_id`),
  KEY `idx_estado_invitacion` (`estado_invitacion`),
  CONSTRAINT `invitaciones_proyecto_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`) ON DELETE CASCADE,
  CONSTRAINT `invitaciones_proyecto_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `invitaciones_proyecto_ibfk_3` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TABLA: tareas
CREATE TABLE `tareas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `proyecto_id` int NOT NULL,
  `titulo` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` varchar(1000) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `estado_progreso` int NOT NULL DEFAULT '2' COMMENT '1=COMPLETADA, 2=PENDIENTE, 3=EN_PROGRESO',
  `prioridad` int NOT NULL DEFAULT '2' COMMENT '1=BAJA, 2=MEDIA, 3=ALTA',
  `estado` int NOT NULL DEFAULT '1' COMMENT '0=eliminado, 1=activo',
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  `modificado_en` datetime DEFAULT NULL,
  `modificado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `creado_por` (`creado_por`),
  KEY `modificado_por` (`modificado_por`),
  KEY `idx_proyecto` (`proyecto_id`),
  KEY `idx_estado_progreso` (`estado_progreso`),
  KEY `idx_prioridad` (`prioridad`),
  KEY `idx_estado` (`estado`),
  CONSTRAINT `tareas_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`) ON DELETE CASCADE,
  CONSTRAINT `tareas_ibfk_2` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `tareas_ibfk_3` FOREIGN KEY (`modificado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TABLA: tareas_usuarios (relación N:N - asignación)
CREATE TABLE `tareas_usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tarea_id` int NOT NULL,
  `usuario_id` int NOT NULL,
  `creado_en` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creado_por` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_tarea_usuario` (`tarea_id`,`usuario_id`),
  KEY `creado_por` (`creado_por`),
  KEY `idx_tarea` (`tarea_id`),
  KEY `idx_usuario` (`usuario_id`),
  CONSTRAINT `tareas_usuarios_ibfk_1` FOREIGN KEY (`tarea_id`) REFERENCES `tareas` (`id`) ON DELETE CASCADE,
  CONSTRAINT `tareas_usuarios_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `tareas_usuarios_ibfk_3` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- DATOS DE PRUEBA (OPCIONAL - COMENTAR SI NO SE NECESITA)
-- =====================================================

-- Usuario de prueba (password: "password123" hasheado con BCrypt)
-- INSERT INTO usuarios (nombre, email, password_hash, estado, creado_en)
-- VALUES ('Admin', 'admin@test.com', '$2a$11$YourHashedPasswordHere', 1, NOW());

-- =====================================================
-- FIN DEL SCRIPT
-- =====================================================
SELECT 'Base de datos creada exitosamente' AS mensaje;
