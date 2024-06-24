using ProyectoTest.Models;
using ProyectoTest.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace ProyectoTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Producto()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Reporte()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Cliente()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Tienda()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpGet]
        public JsonResult ListarCliente()
        {
            List<Cliente> oLista = new List<Cliente>();
            oLista = ClienteLogica.Instancia.ObtenerCliente();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase imagenArchivo)
        {

            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Producto oProducto = new Producto();
                oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

                string GuardarEnRuta = "~/Imagenes/Productos/";
                string physicalPath = Server.MapPath("~/Imagenes/Productos");

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                if (oProducto.IdProducto == 0)
                {
                    int id = ProductoLogica.Instancia.Registrar(oProducto);
                    oProducto.IdProducto = id;
                    oresponse.resultado = oProducto.IdProducto == 0 ? false : true;

                }
                else
                {
                    oresponse.resultado = ProductoLogica.Instancia.Modificar(oProducto);
                }

                
                if (imagenArchivo != null && oProducto.IdProducto != 0)
                {
                    string extension = Path.GetExtension(imagenArchivo.FileName);
                    GuardarEnRuta = GuardarEnRuta + oProducto.IdProducto.ToString() + extension;
                    oProducto.RutaImagen = GuardarEnRuta;

                    imagenArchivo.SaveAs(physicalPath + "/" + oProducto.IdProducto.ToString() + extension );

                    oresponse.resultado = ProductoLogica.Instancia.ActualizarRutaImagen(oProducto);
                }

            }
            catch (Exception e)
            {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }

            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            respuesta = ProductoLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }

    public class Response {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}