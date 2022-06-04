using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppCRUD.Models;

namespace WebAppCRUD.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString(); //Conexcion con la base de datos en el archivo Web.config

        private static List<Contacto> listaContacto;

        // GET: Contacto
        public ActionResult Inicio()
        {
            listaContacto = new List<Contacto>();

            using (SqlConnection oconexion = new SqlConnection(conexion)) { //Crea una nueva conexion 
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oconexion);   //Se crea un nuevo comando sql para consultas
                cmd.CommandType = CommandType.Text;     //Definir que es de tipo texto
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader()) {   //Devuelve el data reader (todos los datos de la db)
                    while (dr.Read()) {
                        Contacto nuevoContacto = new Contacto();

                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        listaContacto.Add(nuevoContacto);
                    }
                }   
            }


            return View(listaContacto);
        }
    }
}