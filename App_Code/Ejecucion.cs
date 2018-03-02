using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>
/// Descripción breve de Ejecucion
/// </summary>
public class Ejecucion
{
    SqlConnection conexionBD;
    SqlCommand cmd;
    DataSet ds;
    DataTable dt;
    SqlDataAdapter da;

	public Ejecucion()
	{
        
	}

    public bool scalarBool(string query) {
        bool retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = convierteIntToBool(Convert.ToInt32(cmd.ExecuteScalar()));
        }
        catch (Exception)
        {
            retorno = false;
        }
        finally {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }

    public object[] scalarBooloBJ(string query)
    {
        object[] retorno = new object[2];
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno[0] = true;
            retorno[1] = convierteIntToBool(Convert.ToInt32(cmd.ExecuteScalar()));
        }
        catch (Exception ex)
        {
            retorno[0] = false;
            retorno[1] = ex.Message;
        }
        finally
        {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }

    public string scalarString(string query)
    {
        string retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToString(cmd.ExecuteScalar());            
        }
        catch (Exception)
        {
            retorno = "";
        }
        finally
        {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }

    public DataSet obtieneInformacion(string query) {
        ds = new DataSet();
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception)
        {
            ds.Clear();
        }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return ds;
    }

    public DataTable obtieneInformacionDt(string query)
    {
        dt = new DataTable();
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        catch (Exception)
        {
            dt.Clear();
        }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return dt;
    }

    private bool convierteIntToBool(int valor) {
        if (valor > 0)
            return true;
        else
            return false;        
    }

    public int scalarInt(string query)
    {
        int retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception)
        {
            retorno = 0;
        }
        finally
        {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }

    public bool update_insert_delete(string query)
    {
        bool retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            cmd.ExecuteNonQuery();
            retorno = true;
        }
        catch (Exception) { retorno = false; }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    public string scalarStringDdlBx(string query)
    {
        string retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (Exception)
        {
            retorno = "0";
        }
        finally
        {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }

    public bool insertUpdateEmpresa(string query, bool tieneImagen, byte[] imagen)
    {
        bool retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            if (tieneImagen)
                cmd.Parameters.AddWithValue("imagen", imagen);
            cmd.ExecuteNonQuery();
            retorno = true;
        }
        catch (Exception) { retorno = false; }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }



    internal object[] scalarObjectArreglo(string sql)
    {
        object[] retorno = new object[12];
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            SqlDataReader lectura = cmd.ExecuteReader();
            while (lectura.Read())
            {
                retorno[0] = lectura.GetString(0);
                retorno[1] = lectura.GetString(1);
                retorno[2] = lectura.GetString(2);
                retorno[3] = lectura.GetString(3);
                retorno[4] = lectura.GetString(4);
                retorno[5] = lectura.GetString(5);
                retorno[6] = lectura.GetString(6);
                retorno[7] = lectura.GetString(7);
                retorno[8] = lectura.GetString(8);
                retorno[9] = lectura.GetString(9);
                retorno[10] = lectura.GetString(10);
                retorno[11] = lectura.GetString(11);
            }
        }
        catch (Exception)
        {
            retorno = null;
        }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] insertAdjuntos(string sql, byte[] imagen)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.Parameters.AddWithValue("imagen", imagen);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    public double scalarDouble(string sql)
    {
        double retorno;
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToDouble(cmd.ExecuteScalar());
        }
        catch (Exception)
        {
            retorno = 0;
        }
        finally
        {
            conexionBD.Close();
            conexionBD.Dispose();
        }
        return retorno;
    }



    internal bool update_insert_delete_img(string sql, byte[] imagen)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.Parameters.AddWithValue("imagen", imagen);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return Convert.ToBoolean(retorno[0]);
    }

    internal byte[] scalarByteArreglo(string sql)
    {
        byte[] doctos = null;
        try
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand(sql, conexionBD);
            SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                doctos = (byte[])lectura[0];
            }
        }
        catch (Exception) { doctos = null; }
        finally { conexionBD.Close(); }
        return doctos;
    }
}