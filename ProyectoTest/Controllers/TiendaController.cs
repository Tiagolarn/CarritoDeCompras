using ProyectoTest.Logica;
using ProyectoTest.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class TiendaController : Controller
    {
        private static Usuario oUsuario;
        //VISTA
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];

            return View();
        }

        //VISTA
        public ActionResult Producto(int idproducto = 0)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];

            Producto oProducto = new Producto();
            List<Producto> oLista = new List<Producto>();

            oLista = ProductoLogica.Instancia.Listar();
            oProducto = (from o in oLista
                      where o.IdProducto == idproducto
                      select new Producto()
                      {
                          IdProducto = o.IdProducto,
                          Nombre = o.Nombre,
                          Descripcion = o.Descripcion,
                          Precio = o.Precio,
                          RutaImagen = o.RutaImagen,
                          base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", "")
                      }).FirstOrDefault();

            return View(oProducto);
        }

        //VISTA
        public ActionResult Carrito()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];

            return View();
        }

        //VISTA
        public ActionResult Compras()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];

            return View();
        }






        [HttpPost]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();

            oLista = ProductoLogica.Instancia.Listar();
            oLista = (from o in oLista
                      select new Producto()
                      {
                          IdProducto = o.IdProducto,
                          Nombre = o.Nombre,
                          Descripcion = o.Descripcion,
                          Precio = o.Precio,
                          RutaImagen = o.RutaImagen,
                          base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", "")
                      }).ToList();


            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var json = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        [HttpPost]
        public JsonResult InsertarCarrito(Carrito oCarrito)
        {
            oCarrito.oUsuario = new Usuario() { IdUsuario = oUsuario.IdUsuario };
            int _respuesta = 0;
            _respuesta = CarritoLogica.Instancia.Registrar(oCarrito) ;
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int _respuesta = 0;
            _respuesta = CarritoLogica.Instancia.Cantidad(oUsuario.IdUsuario);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerCarrito()
        {
            List<Carrito> oLista = new List<Carrito>();
            oLista = CarritoLogica.Instancia.Obtener(oUsuario.IdUsuario);

            if (oLista.Count != 0) {
                oLista = (from d in oLista
                          select new Carrito()
                          {
                              IdCarrito = d.IdCarrito,
                              oProducto = new Producto()
                              {
                                  IdProducto = d.oProducto.IdProducto,
                                  Nombre = d.oProducto.Nombre,
                                  Precio = d.oProducto.Precio,
                                  RutaImagen = d.oProducto.RutaImagen,
                                  base64 = utilidades.convertirBase64(Server.MapPath(d.oProducto.RutaImagen)),
                                  extension = Path.GetExtension(d.oProducto.RutaImagen).Replace(".", ""),
                              }
                          }).ToList();
            }


            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(string IdCarrito,string IdProducto)
        {
            bool respuesta = false;
            respuesta = CarritoLogica.Instancia.Eliminar(IdCarrito, IdProducto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarSesion() {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public JsonResult RegistrarCompra(Compra oCompra)
        {
            bool respuesta = false;

            oCompra.IdUsuario = oUsuario.IdUsuario;
            respuesta = CompraLogica.Instancia.Registrar(oCompra);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpGet]
        public JsonResult ObtenerCompra()
        {
            List<Compra> oLista = new List<Compra>();

            oLista = CarritoLogica.Instancia.ObtenerCompra(oUsuario.IdUsuario);

            oLista = (from c in oLista
                      select new Compra()
                      {
                          Total = c.Total,
                          FechaTexto = c.FechaTexto,
                          oDetalleCompra = (from dc in c.oDetalleCompra
                                            select new DetalleCompra() {
                                                oProducto = new Producto() {
                                                    Nombre = dc.oProducto.Nombre,
                                                    RutaImagen = dc.oProducto.RutaImagen,
                                                    base64 = utilidades.convertirBase64(Server.MapPath(dc.oProducto.RutaImagen)),
                                                    extension = Path.GetExtension(dc.oProducto.RutaImagen).Replace(".", ""),
                                                },
                                                Total = dc.Total,
                                                Cantidad = dc.Cantidad
                                            }).ToList()
                      }).ToList();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarReporte()
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = CarritoLogica.Instancia.ObtenerCompraReporte();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


    }
}