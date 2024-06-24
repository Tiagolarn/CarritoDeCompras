using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ProyectoTest.Logica
{
    public class ClienteLogica
    {
        private static ClienteLogica _instancia = null;

        public ClienteLogica()
        {

        }

        public static ClienteLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ClienteLogica();
                }

                return _instancia;
            }
        }

        public List<Cliente> ObtenerCliente()
        {

            List<Cliente> rptListaCliente = new List<Cliente>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerCliente", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaCliente.Add(new Cliente()
                        {
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                        });
                    }
                    dr.Close();

                    return rptListaCliente;

                }
                catch (Exception ex)
                {
                    rptListaCliente = null;
                    return rptListaCliente;
                }
            }
        }



    }
}