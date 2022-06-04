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

        [HttpGet] //Por default siempre es un HttpsGet y no recibe elementos y solo devuelve la vista
        public ActionResult Registrar() {

            return View();
        }
        
        [HttpPost] //HttpPost se pueden enviar elementos
        public ActionResult Registrar(Contacto ocontacto) {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            { //Crea una nueva conexion 
                SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);   //Se crea un nuevo comando sql para consultas
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);  //Creamos los valores como se creo el precedure de la DB sin el "@"
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;     //Definir que es de un procedimiento almacenado
                oconexion.Open();

                cmd.ExecuteNonQuery();  //Ejecuta el procedimiento                
            }

            return RedirectToAction("Inicio", "Contacto"); //Rederigimos a la pagina        
        }


        [HttpGet] 
        public ActionResult Editar(int? idcontacto)   //El parametro puede recibir null con "?"
        {
            if (idcontacto == null)
                return RedirectToAction("Inicio","Contacto");

            Contacto ocontacto = listaContacto.Where(c => c.IdContacto == idcontacto).FirstOrDefault();  //Retorna el primer contacto que coincida con el idcontacto

            return View(ocontacto);            
        }

        [HttpPost]
        public ActionResult Editar(Contacto ocontacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            { //Crea una nueva conexion 
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);   //Se crea un nuevo comando sql para consultas
                cmd.Parameters.AddWithValue("IdContacto", ocontacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);  //Creamos los valores como se creo el precedure de la DB sin el "@"
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;     //Definir que es de un procedimiento almacenado
                oconexion.Open();

                cmd.ExecuteNonQuery();  //Ejecuta el procedimiento                
            }

            return RedirectToAction("Inicio", "Contacto"); //Rederigimos a la pagina 

        }

        [HttpGet]
        public ActionResult Eliminar(int? idcontacto)   //El parametro puede recibir null con "?"
        {
            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contacto");

            Contacto ocontacto = listaContacto.Where(c => c.IdContacto == idcontacto).FirstOrDefault();  //Retorna el primer contacto que coincida con el idcontacto

            return View(ocontacto);
        }

        [HttpPost]
        public ActionResult Eliminar(string IdContacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            { //Crea una nueva conexion 
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);   //Se crea un nuevo comando sql para consultas
                cmd.Parameters.AddWithValue("IdContacto", IdContacto);                
                cmd.CommandType = CommandType.StoredProcedure;     //Definir que es de un procedimiento almacenado
                oconexion.Open();

                cmd.ExecuteNonQuery();  //Ejecuta el procedimiento                
            }

            return RedirectToAction("Inicio", "Contacto"); //Rederigimos a la pagina 

        }
    }
}