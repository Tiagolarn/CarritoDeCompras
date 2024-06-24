GO
USE TIENDA


go


insert into USUARIO(Nombres,Apellidos,Correo,Contrasena,EsAdministrador) values ('Admin','User','adminuser@gmail.com','1234',1)
insert into USUARIO(Nombres,Apellidos,Correo,Contrasena,EsAdministrador) values ('Cliente','Default','clientedef@gmail.com','1234',0)


insert into PRODUCTO(Nombre,Descripcion,Precio,RutaImagen) values('PC Gamer','
Procesador: INTEL
RAM: 16 GB
GPU: RTX 3060 TI
','3','~/Imagenes/Productos/1.jpg')



insert into PRODUCTO(Nombre,Descripcion,Precio,RutaImagen) values('Play 5','
Procesador: 514856XS
RAM: 16 GB
GPU: 584 SONY
','4','~/Imagenes/Productos/2.jpg')


insert into PRODUCTO(Nombre,Descripcion,Precio,RutaImagen) values('Jaula Gym','
Material: Acero
Peso Maximo: 300 Kg
','2','~/Imagenes/Productos/3.jpg')

insert into PRODUCTO(Nombre,Descripcion,Precio,RutaImagen) values('Moto Susuki','
Cilindraje: 150
Peso Maximo: 250 Kg
','1','~/Imagenes/Productos/4.jpg')

go
