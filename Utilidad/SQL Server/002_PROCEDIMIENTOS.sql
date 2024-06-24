GO
USE TIENDA




go

--PROCEDIMIENTO PARA OBTENER REPORTE
USE TIENDA
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create proc [dbo].[sp_ObtenerReporte]
as
begin
	select dc.IdDetalleCompra,u.Nombres,u.Apellidos,p.Nombre as Producto,dc.Cantidad,c.FechaCompra from DETALLE_COMPRA dc
	inner join COMPRA c on c.IdCompra = dc.IdCompra
	inner join USUARIO u on u.IdUsuario = c.IdUsuario
	inner join PRODUCTO p on p.IdProducto = dc.IdProducto
	where dc.IdCompra = c.IdCompra
end

go
--PROCEDIMIENTO PARA OBTENER CLIENTES
create proc [dbo].[sp_ObtenerCliente]
as
begin
	select u.Nombres,u.Apellidos from USUARIO u
end
go
--PROCEDIMIENTO PARA REGISTRAR PRODUCTO
create proc sp_registrarProducto(
@Nombre varchar(500),
@Descripcion varchar(500),
@Precio numeric(10,2),
@RutaImagen varchar(500),
@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Descripcion = @Descripcion)
	begin
		insert into PRODUCTO(Nombre,Descripcion,Precio,RutaImagen) values (
		@Nombre,@Descripcion,@Precio,@RutaImagen)

		SET @Resultado = scope_identity()
	end
end

go
--PROCEDIMIENTO PARA EDITAR PRODUCTO
create proc sp_editarProducto(
@IdProducto int,
@Nombre varchar(500),
@Descripcion varchar(500),
@Precio numeric(10,2),
@Resultado bit output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Descripcion = @Descripcion and IdProducto != @IdProducto)
	begin
		update PRODUCTO set 
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		Precio =@Precio
		where IdProducto = @IdProducto

		SET @Resultado =1
	end
end
--PROCEDIMIENTO PARA ACTUALIZAR LA IMAGEN CUANDO SE AGREGA UN PRODUCTO
go
create proc sp_actualizarRutaImagen(
@IdProducto int,
@RutaImagen varchar(500)
)
as
begin
	update PRODUCTO set RutaImagen = @RutaImagen where IdProducto = @IdProducto
end

go
--PROCEDIMIENTO PARA OBTENER USUARIO
create proc sp_obtenerUsuario(
@Correo varchar(100),
@Contrasena varchar(100)
)
as
begin
	IF EXISTS (SELECT * FROM usuario WHERE correo = @Correo and contrasena = @Contrasena)
	begin
		SELECT IdUsuario,Nombres,Apellidos,Correo,Contrasena,EsAdministrador FROM usuario WHERE correo = @Correo and contrasena = @Contrasena
	end
end


go
--PROCEDIMIENTO PARA REGISTRAR USUARIO
create proc sp_registrarUsuario(
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Contrasena varchar(100),
@EsAdministrador bit,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
	begin
		insert into USUARIO(Nombres,Apellidos,Correo,Contrasena,EsAdministrador) values
		(@Nombres,@Apellidos,@Correo,@Contrasena,@EsAdministrador)

		SET @Resultado = scope_identity()
	end
end
go
--PROCEDIMIENTO PARA INSERTAR PRODUCTOS AL CARRO
create proc sp_InsertarCarrito(
@IdUsuario int,
@IdProducto int,
@Resultado int output
)
as
begin
	set @Resultado = 0
	if not exists (select * from CARRITO where IdProducto = @IdProducto and IdUsuario = @IdUsuario)
	begin
		insert into CARRITO(IdUsuario,IdProducto) values ( @IdUsuario,@IdProducto)
		set @Resultado = 1
	end
	
end

go
--PROCEDIMIENTO PARA OBTENER LOS PRODUCTOS DEL CARRO
create proc sp_ObtenerCarrito(
@IdUsuario int
)
as
begin
	select c.IdCarrito, p.IdProducto,p.Nombre,p.Precio,p.RutaImagen from carrito c
	inner join PRODUCTO p on p.IdProducto = c.IdProducto
	where c.IdUsuario = @IdUsuario
end

go

--PROCEDIMIENTO PARA REGISTRAR LA COMPRA
create proc sp_registrarCompra(
@IdUsuario int,
@TotalProducto int,
@Total numeric(10,2),
@QueryDetalleCompra nvarchar(max),
@Resultado bit output
)
as
begin
	begin try
		SET @Resultado = 0
		begin transaction
		
		declare @idcompra int = 0
		insert into COMPRA(IdUsuario,TotalProducto,Total) values
		(@IdUsuario,@TotalProducto,@Total)

		set @idcompra = scope_identity()

		set @QueryDetalleCompra = replace(@QueryDetalleCompra,'¡idcompra!',@idcompra)

		EXECUTE sp_executesql @QueryDetalleCompra

		delete from CARRITO where IdUsuario = @IdUsuario

		SET @Resultado = 1

		commit
	end try
	begin catch
		rollback
		SET @Resultado = 0
	end catch
end


go
--PROCEDIMIENTO PARA OBTENER LO QUE SE HA COMPRADO
create proc sp_ObtenerCompra(
@IdUsuario int)
as
begin
select c.Total,convert(char(10),c.FechaCompra,103)[Fecha],

(select  p.Nombre,p.RutaImagen,dc.Total,dc.Cantidad from DETALLE_COMPRA dc
inner join PRODUCTO p on p.IdProducto = dc.IdProducto
where dc.IdCompra = c.IdCompra
FOR XML PATH ('PRODUCTO'),TYPE) AS 'DETALLE_PRODUCTO'

from compra c
where c.IdUsuario = @IdUsuario
FOR XML PATH('COMPRA'), ROOT('DATA') 
end

--PROCEDIMIENTO PARA OBTENER PRODUCTOS
GO
create proc sp_obtenerProducto
as
begin
 select p.* from PRODUCTO p

end

go