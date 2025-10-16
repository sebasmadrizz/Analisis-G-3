CREATE TABLE Empleado_Incapacidades (
    incapacidad_id UNIQUEIDENTIFIER PRIMARY KEY ,
    empleado_id UNIQUEIDENTIFIER NOT NULL,
    tipo_incapacidad VARCHAR(50) NOT NULL,  
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    dias_incapacidad AS DATEDIFF(DAY, fecha_inicio, fecha_fin),  -- Calculado automáticamente
    institucion_medica NVARCHAR(100) NULL,  
    
   
    fecha_registro DATETIME,

    CONSTRAINT FK_Empleado_Incapacidades_Empleado 
        FOREIGN KEY (empleado_id) REFERENCES Empleado(empleado_id)
);