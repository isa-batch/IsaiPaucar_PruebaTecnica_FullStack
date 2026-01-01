-- Script de creación de base de datos para Sistema de Gestión de Tareas
-- Base de datos: gestion_tareas_db

CREATE DATABASE IF NOT EXISTS gestion_tareas_db;
USE gestion_tareas_db;

-- =====================================================
-- TABLA: usuarios
-- =====================================================
CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    estado INT NOT NULL DEFAULT 1 COMMENT '0=eliminado, 1=activo',
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    modificado_en DATETIME NULL,
    modificado_por INT NULL,
    INDEX idx_email (email),
    INDEX idx_estado (estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- TABLA: proyectos
-- =====================================================
CREATE TABLE IF NOT EXISTS proyectos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500) NULL,
    propietario_id INT NOT NULL,
    estado INT NOT NULL DEFAULT 1 COMMENT '0=eliminado, 1=activo',
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    modificado_en DATETIME NULL,
    modificado_por INT NULL,
    FOREIGN KEY (propietario_id) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (modificado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    INDEX idx_propietario (propietario_id),
    INDEX idx_estado (estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- TABLA: proyecto_usuarios (relación N:N)
-- =====================================================
CREATE TABLE IF NOT EXISTS proyecto_usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    proyecto_id INT NOT NULL,
    usuario_id INT NOT NULL,
    rol INT NOT NULL COMMENT '1=OWNER, 2=MEMBER',
    estado INT NOT NULL DEFAULT 1 COMMENT '0=eliminado, 1=activo',
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    FOREIGN KEY (proyecto_id) REFERENCES proyectos(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    UNIQUE KEY uk_proyecto_usuario (proyecto_id, usuario_id),
    INDEX idx_proyecto (proyecto_id),
    INDEX idx_usuario (usuario_id),
    INDEX idx_estado (estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- TABLA: invitaciones_proyecto
-- =====================================================
CREATE TABLE IF NOT EXISTS invitaciones_proyecto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    proyecto_id INT NOT NULL,
    usuario_id INT NOT NULL,
    estado_invitacion VARCHAR(50) NOT NULL DEFAULT 'PENDIENTE' COMMENT 'PENDIENTE, ACEPTADA, RECHAZADA',
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    FOREIGN KEY (proyecto_id) REFERENCES proyectos(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    INDEX idx_proyecto (proyecto_id),
    INDEX idx_usuario (usuario_id),
    INDEX idx_estado_invitacion (estado_invitacion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- TABLA: tareas
-- =====================================================
CREATE TABLE IF NOT EXISTS tareas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    proyecto_id INT NOT NULL,
    titulo VARCHAR(200) NOT NULL,
    descripcion VARCHAR(1000) NULL,
    estado_progreso INT NOT NULL DEFAULT 2 COMMENT '1=COMPLETADA, 2=PENDIENTE, 3=EN_PROGRESO',
    prioridad INT NOT NULL DEFAULT 2 COMMENT '1=BAJA, 2=MEDIA, 3=ALTA',
    estado INT NOT NULL DEFAULT 1 COMMENT '0=eliminado, 1=activo',
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    modificado_en DATETIME NULL,
    modificado_por INT NULL,
    FOREIGN KEY (proyecto_id) REFERENCES proyectos(id) ON DELETE CASCADE,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (modificado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    INDEX idx_proyecto (proyecto_id),
    INDEX idx_estado_progreso (estado_progreso),
    INDEX idx_prioridad (prioridad),
    INDEX idx_estado (estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- TABLA: tareas_usuarios (relación N:N - asignación)
-- =====================================================
CREATE TABLE IF NOT EXISTS tareas_usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tarea_id INT NOT NULL,
    usuario_id INT NOT NULL,
    creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    creado_por INT NULL,
    FOREIGN KEY (tarea_id) REFERENCES tareas(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT,
    FOREIGN KEY (creado_por) REFERENCES usuarios(id) ON DELETE RESTRICT,
    UNIQUE KEY uk_tarea_usuario (tarea_id, usuario_id),
    INDEX idx_tarea (tarea_id),
    INDEX idx_usuario (usuario_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

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
