

create database TIENDA

GO
USE TIENDA
GO


go

CREATE TABLE PRODUCTO(
IdProducto int primary key identity,
Nombre varchar(500),
Descripcion varchar(500),
Precio numeric(10,2) default 0,
RutaImagen varchar(100),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE USUARIO(
IdUsuario int primary key identity,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Contrasena varchar(100),
EsAdministrador bit,
Activo bit default 1,
FechaRegistro datetime default getdate()
)

go


CREATE TABLE CARRITO(
IdCarrito int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProducto int references PRODUCTO(IdProducto)
)

go

create table COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
TotalProducto int,
Total numeric(12,2),
Contacto varchar(50),
FechaCompra datetime default getdate()
)

go

create table DETALLE_COMPRA(
IdDetalleCompra int primary key identity,
IdCompra int references Compra(IdCompra),
IdProducto int references PRODUCTO(IdProducto),
Cantidad int,
Total numeric(12,2)
)

go


