using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;

/// <summary>
/// Descripción breve de Errores
/// </summary>
public class Errores
{
    SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
	public Errores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
}