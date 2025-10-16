  CREATE TABLE Empleado_Ausencias (
    ausencia_id UNIQUEIDENTIFIER PRIMARY KEY ,
    empleado_id UNIQUEIDENTIFIER NOT NULL,
    tipo_ausencia VARCHAR(50) NOT NULL,  
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    motivo NVARCHAR(255) NULL, 
    aprobado BIT DEFAULT 0, 
   
    fecha_registro DATETIME,

    CONSTRAINT FK_Empleado_Ausencias_Empleado 
        FOREIGN KEY (empleado_id) REFERENCES Empleado(empleado_id)
);