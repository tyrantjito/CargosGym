using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using PdfViewer1;
using E_Utilities;

/// <summary>
/// Descripción breve de Datos
/// </summary>
public class Datos
{
    SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
    Ejecucion ejecuta = new Ejecucion();
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    Fechas fechas = new Fechas();
    public Datos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool existeCorreo(string usuario, string correo)
    {
        string sql = "select count(*) from usuarios where id_usuario='" + usuario + "' and correo='" + correo + "'";
        return ejecuta.scalarBool(sql);
    }

    /*                 Empresas              */

    public DataSet llenaEmpresas()
    {
        string sql = "SELECT id_empresa, nombre FROM empresas WHERE estatus='A'";
        return ejecuta.obtieneInformacion(sql);
    }

    public int obtieneEmpresasTotales()
    {
        string sql = "select count(*) from empresas";
        return ejecuta.scalarInt(sql);
    }

    public int obtieneIdEmpresa()
    {
        string sql = "select id_empresa from empresas";
        return ejecuta.scalarInt(sql);
    }

    public int obtienecontador()
    {
        string sql = "select conta from contador";
        return ejecuta.scalarInt(sql);
    }


    /*   
     * Login                    */

    public bool existeUsuario(string usuario)
    {
        string sql = "select count(*) from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarBool(sql);
    }

    public object[] existeUsuarioVal(string usuario)
    {
        string sql = "select count(*) from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarBooloBJ(sql);
    }

    public bool verificaContraseña(string usuario, string contraseña)
    {
        string sql = "select COUNT(*) from usuarios where id_usuario='" + usuario + "' and contrasena='" + contraseña + "'";
        return ejecuta.scalarBool(sql);
    }

    public string obtieneEmpresaUsuario(string usuario)
    {
        string sql = "select id_empresa from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarStringDdlBx(sql);
    }

    public string obtieneNombreUsuario(string usuario)
    {
        string sql = "select nombre from usuarios where id_usuario='" + usuario + "'";
        string nombre = ejecuta.scalarString(sql);
        return nombre;
    }

    public string obtieneNombreEmpresa(int idEmpresa)
    {
        string sql = "select nombre from empresas where id_empresa=" + idEmpresa;
        string nombre = ejecuta.scalarString(sql);
        return nombre;
    }

    public bool[] obtienePermisos(string usuario)
    {
        bool[] permiso = new bool[23];
        for (int i = 0; i < 23; i++)
        {
            permiso[i] = false;
        }


        string sql = "";
        int tamaño = obtienePermisosTotales(usuario);
        if (tamaño != 0)
        {
            try
            {
                conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
                sql = "select id_permiso from usuarios_permisos where id_usuario='" + usuario + "'";
                conexionBD.Open();
                SqlCommand cmd = new SqlCommand(sql, conexionBD);
                SqlDataReader lectura = cmd.ExecuteReader();
                while (lectura.Read())
                {
                    permiso[lectura.GetInt32(0) - 1] = true;
                }
            }
            catch (Exception)
            {
                for (int i = 0; i < 23; i++)
                {
                    permiso[i] = false;
                }
            }
            finally
            {
                conexionBD.Dispose();
                conexionBD.Close();
            }
        }
        else
        {
            for (int i = 0; i < 23; i++)
            {
                permiso[i] = false;
            }
        }
        return permiso;
    }

    public int obtienePermisosTotales(string usuario)
    {
        int permisos = 0;
        string sql = "select count(id_permiso) from usuarios_permisos where id_usuario='" + usuario + "'";
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["eBills"].ToString());
        try
        {
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand(sql, conexionBD);
            permisos = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception)
        {
            permisos = 0;
        }
        finally
        {
            conexionBD.Dispose();
            conexionBD.Close();
        }
        return permisos;
    }


    public void creaTabla() {
        string sql = "IF OBJECT_ID (N'dbo.tmp_pagos', N'U') IS NOT NULL  DROP TABLE dbo.tmp_pagos CREATE TABLE [dbo].[tmp_pagos](	[id] [int] NOT NULL,	[afiliacion] [varchar](200) NOT NULL,	[referencia] [varchar](500) NOT NULL,	[cuenta] [varchar](50) NOT NULL,	[monto] [decimal](15, 2) NOT NULL, CONSTRAINT [PK_tmp_pagos] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])";
        ejecuta.update_insert_delete(sql);
    }
    
    /*               Usuarios                   */

    public DataSet cargaUsuarios()
    {
        string sql = "select id_usuario,contrasena,nombre, usuario_afiliacion,correo from Usuarios where id_usuario != 'Supervisor'";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataTable obtieneInformacionAltaUsuarios(string usuario, string empresa, DateTime fecha_inicio, DateTime fecha_final)
    {    
        string condicion = "";
        if (usuario == "")
            condicion = " (usuario_alta = '' or usuario_alta is null) ";
        else
            condicion = " usuario_alta = '" + usuario + "' ";
        string sql = "select year(fecha_inicio) as año,id_cliente,referencia,(nombre_tarjetahabiente + ' ' + ap_pat_tarjetahabiente + ' ' + ap_mat_tarjetahabiente) as nombre,(nombre + ' ' + apellido_paterno + ' ' + apellido_materno) as cliente,fecha_inicio, estatus_cliente,case estatus_cliente when 'A' then 'Activo' else 'Inactivo' end as valorEstatus from clientes where " + condicion + " and id_empresa = " + empresa + " and FECHA_INICIO between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' order by fecha_inicio";        
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtieneInformacionBajasUsuarios(string usuario, string empresa, DateTime fecha_inicio, DateTime fecha_final)
    {
       string sql = "select tabla.referencia,tabla.cliente,tabla.tarjeta_habiente,tabla.fecha_baja,case when tabla.usuarioBaja is null then isnull(tabla.usuario_baja, '') when tabla.usuarioBaja = '' then tabla.usuario_baja else isnull(tabla.usuarioBaja, '') end as usuarioBaja,case when tabla.usuarioAutorizo is null then isnull(tabla.usuario_aut, '') when tabla.usuarioAutorizo = '' then tabla.usuario_aut else isnull(tabla.usuarioAutorizo, '') end as usuarioAutorizo,tabla.motivo,tabla.bajaInmediata,tabla.usuario_baja,tabla.usuario_aut from(" +
                    "select c.referencia, (c.nombre + ' ' + c.apellido_paterno + ' ' + c.apellido_materno) as cliente, (c.nombre_tarjetahabiente + ' ' + c.ap_pat_tarjetahabiente + ' ' + c.ap_mat_tarjetahabiente) as tarjeta_habiente, c.fecha_baja, u.nombre as usuarioBaja, '' as usuarioAutorizo, c.motivo_baja as motivo, 'NO' as bajaInmediata, c.usuario_inactiva as usuario_baja, '' as usuario_aut from clientes c left join usuarios u on upper(u.id_usuario) = upper(c.usuario_inactiva) where c.estatus_cliente = 'I' and c.id_empresa = " + empresa + " and c.fecha_baja <> '1900-01-01' " +
                    ") as tabla where tabla.fecha_baja between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' and tabla.usuario_baja = '" + usuario + "' order by fecha_baja desc";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public bool actualizausuario(string usuario, string contraseña, string nombre, int usuarioafi,string correo)
    {
        string condicion = "";
        if (contraseña != "")
            condicion = " contrasena = '" + contraseña + "', ";

        string sql = "update usuarios set " + condicion + " nombre='" + nombre + "',  usuario_afiliacion=" + usuarioafi + ", correo='" + correo + "' where id_usuario='" + usuario.ToString() + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool borraUsuario(string claveusuario)
    {
        string sql = "delete from Usuarios where id_usuario= '" + claveusuario + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool obtieneUsaAfiliacion(string usuario)
    {
        string sql = "select usuario_afiliacion from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarBool(sql);
    }

    public bool agregaUsuario(string id_usuario, string contraseña, string nombre, int usaAfiliacion)
    {
        string sql = "insert into Usuarios (id_usuario, contrasena,nombre,usuario_afiliacion) values('" + id_usuario + "','" + contraseña + "','" + nombre + "'," + usaAfiliacion.ToString() + ") ";
        return ejecuta.update_insert_delete(sql);
    }

    public bool obtieneUsuarioAfiliacion()
    {
        string sql = "select count(*) from usuarios where usuario_afiliacion = 1";
        return ejecuta.scalarBool(sql);
    }

    public bool agregaFotoDanos(int idEmpresa, string nombre, byte[] imagenOriginal, string referencia, int cliente, string extension)
    {
        bool insertado = false;
        string sql = "";
        if (extension.ToUpper() != "PDF")
            sql = "insert into documentos values(" + idEmpresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + idEmpresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',@imagen,'" + extension + "', null,'M')";
        else
            sql = "insert into documentos values(" + idEmpresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + idEmpresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',null,'" + extension + "', @imagen,'M')";
        object[] ejecutado = ejecuta.insertAdjuntos(sql, imagenOriginal);
        try
        {
            bool fueInsertado = Convert.ToBoolean(ejecutado[0]);
            if (fueInsertado)
                insertado = true;
            else
                insertado = false;
        }
        catch (Exception )
        {
            insertado = false;
        }
        return insertado;
    }

    /*               Consultas                  */

    public DataSet llenaGridPag(int empresa, string periodo, string periodicidad, string condicion)
    {
        if (periodicidad == "")
            periodicidad = "M";
        string sql = "Select DISTINCT C.id_cliente  AS CLIENTE,C.referencia AS SOCIO, (C.nombre+' '+C.apellido_paterno+' '+C.apellido_materno) AS NOMBRE, C.fecha_fin AS 'TERMINO DE SUSCRIPCION',c.id_cliente from clientes C where C.estatus_CLIENTE='A' AND C.id_empresa=" + empresa.ToString() + " AND C.periodo_pago='" + periodicidad + "' and c.id_cliente in (select top 1 d.id_cliente from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + periodo + "' and d.movimiento IN ('P','M') and d.id_cliente=C.id_cliente order by d.id_cliente,d.consecutivo desc) " + condicion;
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet llenaGridRec(int empresa, string periodo, string periodicidad, string condicion)
    {
        if (periodicidad == "")
            periodicidad = "M";
        string sql = "Select DISTINCT C.id_cliente,C.referencia AS SOCIO, (C.nombre+' '+C.apellido_paterno+' '+C.apellido_materno) AS NOMBRE, dc.fecha_pago_rechazado,dc.motivo_rechzado from clientes C  left join detalle_cliente dc on dc.id_cliente=c.id_cliente and dc.consecutivo=(select top 1 d.consecutivo from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + periodo + "' and d.movimiento IN ('R') and d.id_cliente=C.id_cliente order by d.id_cliente,d.consecutivo desc) where C.estatus_CLIENTE='A' AND C.id_empresa=" + empresa.ToString() + " AND C.periodo_pago='" + periodicidad + "' and c.id_cliente in (select top 1 d.id_cliente from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + periodo + "' and d.movimiento IN ('R') and d.id_cliente=C.id_cliente order by d.id_cliente,d.consecutivo desc) and c.id_cliente not in (Select DISTINCT C.id_cliente from clientes C where C.estatus_CLIENTE='A' AND C.id_empresa=" + empresa.ToString() + " AND C.periodo_pago='" + periodicidad + "' and c.id_cliente in (select top 1 d.id_cliente from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + periodo + "' and d.movimiento IN ('P','M') and d.id_cliente=C.id_cliente order by d.id_cliente,d.consecutivo desc))" + condicion;
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtieneInfoEmpresaParametrosCorreo(string empresa)
    {
        string sql = "select isnull(usuario,''),isnull(contrasena,''),isnull(host,''),isnull(puerto,0),isnull(tipo_servidor,0),isnull(ssl_habilitado,0),isnull(mensajeCorreo,'') from empresas where id_empresa=" + empresa;
        return ejecuta.obtieneInformacion(sql);
    }

    public string[] obtieneFaltantesCliente(int empresa, string usuario)
    {
        string sql = "declare @empresa as int declare @cliente as int set @empresa = " + empresa + " set @cliente = " + usuario + " select cast((select count(*) from comentarios where id_cliente = @cliente and id_empresa = @empresa and leido = 0) as char(10)) +';' +cast(((select count(*) from clientes where id_cliente = @cliente and id_banco is null) + (select count(*) from clientes where id_cliente = @cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente = @cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente = @cliente and telefono is null)+(select count(*) from clientes where id_cliente = @cliente and celular is null)+(select count(*) from clientes where id_cliente = @cliente and correo is null)) as char(10))+';' +cast((select count(*) from documentos where id_empresa = @empresa and id_cliente = @cliente) as char(10))";
        string valor = ejecuta.scalarString(sql);
        string[] argumentos = valor.Split(new char[] { ';' });
        return argumentos;
    }

    /*               Clientes                  */

    public bool agregaCliente(string cliente, string nombre, string apPat, string apMat, double monto, string cuenta, string fechaIni, string fechaFin, string periodicidad, int empresa, int banco, string vigencia, string nomth, string apth, string amth, byte[] imagen, string usuarioAlta, string extension, string telefono, string celular, string correo, string usuarioAutoriza)
    {
        string sql = "insert into clientes(id_cliente, referencia,nombre,apellido_paterno,apellido_materno,monto,no_cuenta,fecha_inicio,fecha_fin,estatus_cliente,periodo_pago,id_empresa,id_banco,vigencia_cuenta,nombre_tarjetahabiente,ap_pat_tarjetahabiente,ap_mat_tarjetahabiente,usuario_alta,telefono,celular,correo,usuario_autoriza_alta) values((select isnull((select top 1 id_cliente from clientes order by id_cliente desc),0) + 1),'" + cliente + "','" + nombre + "','" + apPat + "','" + apMat + "'," + monto.ToString() + ",'" + cuenta + "','" + fechaIni + "','" + fechaFin + "','A','" + periodicidad + "'," + empresa.ToString() + "," + banco.ToString() + ",'" + vigencia + "','" + nomth + "','" + apth + "','" + amth + "','" + usuarioAlta + "','" + telefono + "','" + celular + "','" + correo + "','" + usuarioAutoriza + "')";
        bool insertado = ejecuta.update_insert_delete(sql);
        return insertado;
    }

    public bool agregaDocumentosCliente(int Idcliente, byte[] imagen, string extension, string cliente, int empresa, string identificador_proceso)
    {
        string sql = "";
        if (extension.ToUpper() != "PDF")
            sql = "insert into documentos values(" + empresa.ToString() + "," + Idcliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + Idcliente.ToString() + " order by id_documento desc),0)+1),'" + cliente + "',@imagen,'" + extension + "', null,'" + identificador_proceso + "')";
        else
            sql = "insert into documentos values(" + empresa.ToString() + "," + Idcliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + Idcliente.ToString() + " order by id_documento desc),0)+1),'" + cliente + "',null,'" + extension + "', @imagen,'" + identificador_proceso + "')";

        bool insertadoDocto = ejecuta.update_insert_delete_img(sql, imagen);
        return insertadoDocto;
    }


    public bool existeCliente(string cliente, string empresa)
    {
        string sql = "Select count(*) from clientes where referencia='" + cliente.Trim() + "' and id_Empresa='"+empresa+"'";
        return ejecuta.scalarBool(sql);
    }

    public bool existeClientePeriodo(string cliente, string periodo)
    {
        string sql = "Select count(*) from clientes where referencia='" + cliente.Trim() + "' and CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodo + "' as int)";
        return ejecuta.scalarBool(sql);
    }

    public DataSet obtieneClientes(int empresa, string periodo, string cadena, string estatus, string condicion, string nombreCompleto, string anio)
    {
        if (periodo == "")
            periodo = "M";
        int año = 0;
        try { año = Convert.ToInt32(anio); }
        catch (Exception) { año = fechaLocal.obtieneFechaLocal().Year; }
        string sql = "";

        if (estatus == "A")
        {
            if (cadena == "")
                sql = "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='" + estatus + "')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ")) " +
                    "union all " +
                    "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='I')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ")) and dateadd(mm,1,c.fecha_baja)>='" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' ";// + meses + " " + pendientes;
            else
                sql = "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='" + estatus + "')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + "))  and (" + condicion + ")  " +
                    "union all " +
                    "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='I')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + "))  and (" + condicion + ") and dateadd(mm,1,c.fecha_baja)>='" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' "; // +meses + " " + pendientes;
        }
        else
        {
            if (cadena == "")
                sql = "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12 ,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='" + estatus + "')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ")) ";// + meses + " " + pendientes;
            else
                sql = "SELECT c.id_cliente,c.referencia," + nombreCompleto + ",c.nombre_tarjetahabiente,c.ap_pat_tarjetahabiente,c.ap_mat_tarjetahabiente,c.nombre,c.apellido_paterno,c.apellido_materno, c.monto,c.no_cuenta as cuenta,convert(char(10), c.fecha_inicio,120) as fecha_ini,convert(char(10), c.fecha_fin,120) as fecha_fin,estatus_cliente,c.periodo_pago,c.id_empresa,isnull(c.id_banco,0) as id_banco,isnull(c.vigencia_cuenta,'') as vigencia_cuenta, isnull(b.nombre,'') as banco,(select isnull(p1,'') as p1 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P1,(select isnull(p2,'') as p2 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P2,(select isnull(p3,'') as p3 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P3,(select isnull(p4,'') as p4 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P4,(select isnull(p5,'') as p5 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P5,(select isnull(p6,'') as p6 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P6,(select isnull(p7,'') as p7 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P7,(select isnull(p8,'') as p8 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P8,(select isnull(p9,'') as p9 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P9,(select isnull(p10,'') as p10 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P10,(select isnull(p11,'') as p11 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P11,(select isnull(p12,'') as p12 from periodos_pagos where id_cliente=c.id_cliente and ano=" + año.ToString() + ") as P12 ,(SELECT COUNT(*) FROM DETALLE_CLIENTE WHERE ID_CLIENTE=c.id_cliente and id_empresa=c.id_empresa and noReconocido=1) as noReconocido,isnull(month(dateadd(m,1,c.fecha_baja)),1) as mesBaja,isnull(year(dateadd(m,1,c.fecha_baja)),1900) as anoBaja,(select cast((select count(*) from comentarios where id_cliente=c.id_cliente and id_empresa=c.id_empresa and leido=0) as char(10)) +';'+cast(((select count(*) from clientes where id_cliente=c.id_cliente and id_banco is null)+(select count(*) from clientes where id_cliente=c.id_cliente and vigencia_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and no_cuenta is null)+(select count(*) from clientes where id_cliente=c.id_cliente and telefono is null)+(select count(*) from clientes where id_cliente=c.id_cliente and celular is null)+(select count(*) from clientes where id_cliente=c.id_cliente and correo is null)) as char(10))+';'+cast((select count(*) from documentos where id_empresa=c.id_empresa and id_cliente=c.id_cliente) as char(10))) as extra FROM clientes c left join bancos b on b.id_banco=c.id_banco WHERE ((c.id_empresa = " + empresa.ToString() + ") AND (c.periodo_pago = '" + periodo + "') and (c.estatus_cliente='" + estatus + "')) and (c.id_cliente in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + ") OR c.id_cliente NOT in (select id_cliente from detalle_cliente where substring(periodo,1,4)='" + año.ToString() + "' and id_empresa=" + empresa.ToString() + "))  and (" + condicion + ")  ";// +meses + " " + pendientes;
        }
        sql = sql + " order by 2 asc";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtieneClientesRechazados(string idEmpresa, string año, string periodo)
    {
        string sql = "select  p.id_cliente,c.correo,c.referencia from periodos_pagos p left join clientes c on c.id_cliente = p.id_cliente  and c.id_empresa = " + idEmpresa + " and not c.correo is null where p.ano = " + año + " and p.p" + periodo + " = 'R' and c.correo <> ''";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtienePorcentajes(string cliente, string empresa, string año)
    {
        string sql = "declare @cliente int declare @empresa int declare @ano int set @cliente=" + cliente + " set @empresa=" + empresa + " set @ano=" + año + "  " +
            "select (select count(*) from detalle_cliente where id_cliente=@cliente and id_empresa=@empresa and substring(periodo,1,4)= @ano and movimiento='P') as pagados," +
            "(select count(*) from detalle_cliente where id_cliente = @cliente and id_empresa = @empresa and substring(periodo, 1, 4) = @ano and movimiento = 'R') as rechazados," +
            "(select count(*) from detalle_cliente where id_cliente = @cliente and id_empresa = @empresa and substring(periodo, 1, 4) = @ano and movimiento = 'M') as sucursal," +
            "(select count(*) from detalle_cliente where id_cliente = @cliente and id_empresa = @empresa and substring(periodo,1, 4)= @ano) as total";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataTable obtieneInformacionBajasInmeditasUsuarios(string usuario, string empresa, DateTime fecha_inicio, DateTime fecha_final)
    {
        string sql = "select tabla.referencia,tabla.cliente,tabla.tarjeta_habiente,tabla.fecha_baja,case when tabla.usuarioBaja is null then isnull(tabla.usuario_baja, '') when tabla.usuarioBaja = '' then tabla.usuario_baja else isnull(tabla.usuarioBaja, '') end as usuarioBaja,case when tabla.usuarioAutorizo is null then isnull(tabla.usuario_aut, '') when tabla.usuarioAutorizo = '' then tabla.usuario_aut else isnull(tabla.usuarioAutorizo, '') end as usuarioAutorizo,tabla.motivo,tabla.bajaInmediata,tabla.usuario_baja,tabla.usuario_aut from(" +
            "SELECT b.referencia, b.cliente, b.tarjeta_habiente, b.fecha_baja, u.nombre as usuarioBaja, ua.nombre as usuarioAutorizo, b.motivo, 'SI' as bajaInmediata, isnull(b.usuario_baja,'') as usuario_baja, b.usuario_aut from bajas_inmediatas b left join usuarios u on upper(u.id_usuario) = upper(b.usuario_baja) left join usuarios ua on upper(ua.id_usuario) = upper(b.usuario_aut) where id_empresa = " + empresa + " and referencia <> '' and b.fecha_baja <> '1900-01-01' " +
            ") as tabla where tabla.fecha_baja between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' and tabla.usuario_baja = '" + usuario + "' order by fecha_baja desc";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public bool actualizaEmpresaUsuario(string usuario, int empresa)
    {
        string sql = "update usuarios set id_empresa=" + empresa + " where id_usuario='" + usuario + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool borraCliente(string cliente)
    {
        string sql = "delete from detalle_cliente where id_cliente=" + cliente;
        bool detalle_eliminado = ejecuta.update_insert_delete(sql);
        if (detalle_eliminado)
        {
            sql = "delete from clientes where id_cliente=" + cliente;
            return ejecuta.update_insert_delete(sql);
        }
        else
            return false;
    }

    public bool inactivaCliente(string cliente, string estatus, string fechaBaja, string motivoBaja, string referencia, byte[] imagen, int empresa, string usuarioBaja, string extension, string usuarioAutoriza)
    {
        string info = "";
        string condicion = "";
        string usuarioAutorizaQ = "";
        if (usuarioAutoriza != "" && estatus == "I")
            usuarioAutorizaQ = " ,usuario_inactiva_inactiva='" + usuarioAutoriza + "' ";
        if (usuarioAutoriza != "" && estatus == "A")
            usuarioAutorizaQ = " ,usuario_autoriza_reactiva='" + usuarioAutoriza + "', fecha_reactiva='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "', hora_reactiva='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "' ";
        if (estatus == "A")
            info = ", usuario_reactiva='" + usuarioBaja + "' " + usuarioAutorizaQ;
        else if (estatus == "I")
            info = ", usuario_inactiva='" + usuarioBaja + "' " + usuarioAutorizaQ;
        else
            info = "";

        if (fechaBaja == "1900-01-01")
            condicion = "fecha_baja=null";
        else
            condicion = "fecha_baja='" + fechaBaja + "'";
        string sql = "update clientes set estatus_cliente='" + estatus + "', " + condicion + ", motivo_baja='" + motivoBaja + "'" + info + " where id_cliente=" + cliente.ToString();
        bool actualizado = ejecuta.update_insert_delete(sql);
        if (actualizado)
        {
            if (imagen != null)
            {
                if (extension.ToUpper() != "PDF")
                    sql = "insert into documentos values(" + empresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',@imagen,'" + extension + "', null,'" + estatus + "')";
                else
                    sql = "insert into documentos values(" + empresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',null,'" + extension + "', @imagen, '" + estatus + "')";
                bool insertado = ejecuta.update_insert_delete_img(sql, imagen);
            }
        }
        return actualizado;
    }
    public bool inactivaClientes(string cliente, string estatus, string fechaBaja, string motivoBaja, string referencia, byte[] imagen, int empresa, string usuarioBaja, string extension, string usuarioAutoriza)
    {
        string info = "";
        string condicion = "";
        string usuarioAutorizaQ = "";
        if (usuarioAutoriza != "" && estatus == "I")
            usuarioAutorizaQ = " ,usuario_inactiva_inactiva='" + usuarioAutoriza + "' ";
        if (usuarioAutoriza != "" && estatus == "A")
            usuarioAutorizaQ = " ,usuario_autoriza_reactiva='" + usuarioAutoriza + "', fecha_reactiva='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "', hora_reactiva='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "' ";
        if (estatus == "A")
            info = ", usuario_reactiva='" + usuarioBaja + "' " + usuarioAutorizaQ;
        else if (estatus == "I")
            info = ", usuario_inactiva='" + usuarioBaja + "' " + usuarioAutorizaQ;
        else
            info = "";

        if (fechaBaja == "1900-01-01")
            condicion = "fecha_baja=null";
        else
            condicion = "fecha_baja='" + fechaBaja + "'";
        string sql = "update clientes set estatus_cliente='" + estatus + "', " + condicion + ", motivo_baja='" + motivoBaja + "'" + info + " where id_cliente=" + cliente.ToString();
        bool actualizado = ejecuta.update_insert_delete(sql);
        if (actualizado)
        {
            if (imagen != null)
            {
                if (extension.ToUpper() != "PDF")
                    sql = "insert into documentos values(" + empresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',@imagen,'" + extension + "', null,'" + estatus + "')";
                else
                    sql = "insert into documentos values(" + empresa.ToString() + "," + cliente.ToString() + ",(select isnull((select top 1 id_documento from documentos where id_empresa=" + empresa.ToString() + " and id_cliente=" + cliente.ToString() + " order by id_documento desc),0)+1),'" + referencia + "',null,'" + extension + "', @imagen, '" + estatus + "')";
                bool insertado = ejecuta.update_insert_delete_img(sql, imagen);
            }
        }
        return actualizado;
    }

    public bool actualizaCliente(string cliente, string nombre, string apPat, string apMat, double montoD, string cuenta, string fecha_ini, string fecha_fin, string banco, string vigencia, string nomth, string apth, string amth, byte[] imagen, int empresa, string referencia, string extension, string telefono, string celular, string correo)
    {
        string sql = "update clientes set nombre='" + nombre + "', apellido_paterno='" + apPat + "', apellido_materno='" + apMat + "', monto=" + montoD.ToString() + ", no_cuenta='" + cuenta + "', fecha_inicio='" + fecha_ini + "', fecha_fin='" + fecha_fin + "', id_banco=" + banco + ", vigencia_cuenta='" + vigencia + "', nombre_tarjetahabiente='" + nomth + "', ap_pat_tarjetahabiente='" + apth + "', ap_mat_tarjetahabiente='" + amth + "', telefono='" + telefono + "', celular='" + celular + "', correo='" + correo + "' where id_cliente=" + cliente;
        bool actualizado = ejecuta.update_insert_delete(sql);        
        return actualizado;
    }

    public DataSet obtieneDetalleCliente(string cliente, int empresa)
    {
        string sql = "select case convert(char(10), isnull(dd.fecha_pago,'1900-01-01'),120) when '1900-01-01' then dd.fecha_pago_rechazado else dd.fecha_pago end as fecha,dd.motivo_rechzado, "
                    + "case substring(dd.periodo,5,2) when '01' then 'Enero' when '02' then 'Febrero' when '03' then 'Marzo' when '04' then 'Abril' when '05' then 'Mayo' when '06' then 'Junio' when '07' then 'Julio' when '08' then 'Agosto' when '09' then 'Septiembre' when '10' then 'Octubre' when '11' then 'Noviembre' when '12' then 'Diciembre' else '' end as periodo, "
                    + "dd.movimiento,case dd.movimiento when 'P' then 'Pagado' when 'R' then 'Rechazado' when 'M' then 'Pagado' else '' end as Estatus,dd.CONSECUTIVO,isnull(dd.noReconocido,0) as noReconocido, dd.id_usuario as usuario, "
                    + "case when (select count(*) from detalle_cliente d where d.id_cliente=dd.id_cliente and d.id_empresa = dd.id_empresa  and d.periodo=dd.periodo and d.noReconocido=1 and d.consecutivo=dd.consecutivo)>0 then 'C' else " +//(select top 1 d.movimiento from detalle_cliente d where d.id_cliente=dd.id_cliente and d.id_empresa = dd.id_empresa and d.periodo=dd.periodo order by d.consecutivo desc)
                    " dd.movimiento end as movs "
                    + "from detalle_cliente dd "
                    + "where dd.id_cliente=" + cliente + " and dd.id_empresa=" + empresa.ToString() + " order by dd.consecutivo desc";
        return ejecuta.obtieneInformacion(sql);
    }

    public int obtienePeriodos(int año, int periodo, int cliente)
    {
        string parametro = "p" + periodo;
        string sql = "select top 1 case " + parametro + " when 'R' then 1 when '' then 0 else 0 end from periodos_pagos where id_cliente=" + cliente + " and ano=" + año;
        return ejecuta.scalarInt(sql);
    }

    public DataSet obtieneAñosCliente(string id_cliente, int empresa)
    {
        string sql = "select cast(substring(periodo,1,4) as int) as ano from detalle_cliente where id_cliente=" + id_cliente + " and id_empresa=" + empresa.ToString() + " group by cast(substring(periodo,1,4) as int) order by 1 desc";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtieneMesesCliente(string id_cliente, int año, int empresa)
    {
        string sql = "select case cast(substring(periodo,5,2) as int) when 1 then 'Enero' when 2 then 'Febrero' when 3 then 'Marzo' when 4 then 'Abril' when 5 then 'Mayo' when 6 then 'Junio' when 7 then 'Julio' when 8 then 'Agosto' when 9 then 'Septiembre' when 10 then 'Octubre' when 11 then 'Noviembre' else 'Diciembre' end as mes,"
                + " cast(substring(periodo,5,2) as int) as no_mes from detalle_cliente where id_cliente=" + id_cliente + " and id_empresa=" + empresa.ToString() + " and cast(substring(periodo,1,4) as int)=" + año.ToString() + " group by cast(substring(periodo,5,2) as int) order by cast(substring(periodo,5,2) as int) asc";
        return ejecuta.obtieneInformacion(sql);
    }

    public string obtieneNombreCliente(string cliente, string tipo)
    {
        string sql = "";
        if (tipo == "C")
            sql = "select (nombre_tarjetahabiente+' '+ap_pat_tarjetahabiente+' '+ap_mat_tarjetahabiente) as nombre_completo from clientes where id_cliente=" + cliente;
        else
            sql = "select (nombre+' '+apellido_paterno+' '+apellido_materno) as nombre_completo from clientes where id_cliente=" + cliente;
        return ejecuta.scalarString(sql);
    }

    public int obtieneEmpresasTotalesActivas()
    {
        string sql = "select count(*) from empresas where estatus='A'";
        return ejecuta.scalarInt(sql);
    }

    public string validaEmpresaUsuario(string usuario, int empresa)
    {
        string sql = "select id_empresa from usuarios_empresas where id_usuario='" + usuario + "' and id_empresa=" + empresa.ToString();
        return ejecuta.scalarStringDdlBx(sql);
    }

    public bool existeImagenEmpresa(int id_empresa)
    {
        string sql = "select count(*) from empresas where id_empresa=" + id_empresa + " and not imagen_logo is null";
        return ejecuta.scalarBool(sql);
    }

    public DataSet llenaGridPagFiltro(int id_empresa, string periodo, string periodicidad, string filtro)
    {
        if (periodicidad == "")
            periodicidad = "M";
        string sql = "Select DISTINCT C.id_cliente  AS CLIENTE,C.referencia AS SOCIO, "
                      + "(C.nombre+' '+C.apellido_paterno+' '+C.apellido_materno) AS NOMBRE, C.fecha_fin AS 'TERMINO DE SUSCRIPCION' "
                      + "from clientes C "
                      + "INNER JOIN detalle_cliente D ON D.id_cliente=C.id_cliente AND D.id_empresa=C.id_empresa "
                      + "where C.estatus_CLIENTE='A' AND C.id_empresa=" + id_empresa.ToString() + " AND D.movimiento='P' AND D.periodo='" + periodo + "' AND C.periodo_pago='" + periodicidad + "' and (c.referencia like '%" + filtro + "%' or c.nombre like '%" + filtro + "%' or c.apellido_paterno like '%" + filtro + "%' or c.apellido_materno like '%" + filtro + "%')";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet llenaGridRecFiltro(int id_empresa, string periodo, string periodicidad, string filtro)
    {
        if (periodicidad == "")
            periodicidad = "M";
        string sql = "Select DISTINCT C.id_cliente,C.referencia AS SOCIO, "
                        + " (C.nombre+' '+C.apellido_paterno+' '+C.apellido_materno) AS NOMBRE, d.fecha_pago_rechazado,"
                        + " d.motivo_rechzado"
                        + " from clientes C"
                        + " INNER JOIN detalle_cliente D ON D.id_cliente=C.id_cliente AND D.id_empresa=C.id_empresa "
                        + " where C.estatus_CLIENTE='A' AND C.id_empresa=" + id_empresa.ToString() + " AND D.movimiento='R' AND C.periodo_pago='" + periodicidad + "' AND (c.referencia like '%" + filtro + "%' or c.nombre like '%" + filtro + "%' or c.apellido_paterno like '%" + filtro + "%' or c.apellido_materno like '%" + filtro + "%') AND C.periodo_pago='" + periodicidad + "' AND D.id_cliente NOT IN("
                        + " Select DISTINCT a.id_cliente "
                        + " from clientes a "
                        + " INNER JOIN detalle_cliente e ON e.id_cliente=a.id_cliente AND e.id_empresa=a.id_empresa "
                        + " where a.estatus_CLIENTE='A' AND a.id_empresa=" + id_empresa.ToString() + " AND e.movimiento='P' AND e.periodo='" + periodo + "' AND a.periodo_pago='" + periodicidad + "')";
        return ejecuta.obtieneInformacion(sql);
    }

    public decimal obtieneMontoEmpresa(int empresa)
    {
        string sql = "select mensualidad from empresas where id_empresa=" + empresa;
        decimal monto = Convert.ToDecimal(ejecuta.scalarDouble(sql));
        return monto;
    }

    public DataSet cargaEmpresas()
    {
        string sql = "select id_empresa,nombre,imagen_logo,estatus,mensualidad,num_archivo from empresas";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool actualizaEmpresa(int empresa, string nombre, byte[] imagen, decimal monto, string usuario, string contraseña, string puerto, string tipoServidor, bool ssl, string mensaje, string servidor)
    {
        string sql = "";
        bool tieneImagen;
        int sslActivado = 0;
        if (ssl)
            sslActivado = 1;
        if (imagen.Length != 0)
        {
            sql = "update empresas set  nombre='" + nombre + "' , imagen_logo = @imagen, mensualidad =" + monto.ToString() + ",usuario='" + usuario + "',contrasena='" + contraseña + "',host='" + servidor + "',puerto=" + puerto + ",tipo_servidor=" + tipoServidor + ",ssl_habilitado=" + sslActivado + ",mensajecorreo='" + mensaje + "' where id_empresa=" + empresa.ToString();
            tieneImagen = true;
        }
        else
        {
            sql = "update empresas set  nombre='" + nombre + "', mensualidad =" + monto.ToString() + ",usuario='" + usuario + "',contrasena='" + contraseña + "',host='" + servidor + "',puerto=" + puerto + ",tipo_servidor=" + tipoServidor + ",ssl_habilitado=" + sslActivado + ",mensajecorreo='" + mensaje + "' where id_empresa=" + empresa.ToString();
            tieneImagen = false;
        }
        return ejecuta.insertUpdateEmpresa(sql, tieneImagen, imagen);
    }

    public bool agregaEmpresa(string nombre, byte[] imagen, decimal monto)
    {
        string sql = "";
        bool tieneImagen;
        if (imagen.Length != 0)
        {
            sql = "insert into empresas (nombre,imagen_logo,estatus,mensualidad) values('" + nombre + "', @imagen, 'A'," + monto.ToString() + ") ";
            tieneImagen = true;
        }
        else
        {
            sql = "insert into empresas (nombre,estatus,mensualidad) values('" + nombre + "', 'A'," + monto.ToString() + ")";
            tieneImagen = false;
        }
        return ejecuta.insertUpdateEmpresa(sql, tieneImagen, imagen);
    }

    public bool borraEmpresa(string id_empresa)
    {
        string sql = "delete from empresas where id_empresa= '" + id_empresa.ToString() + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool autEstatus(string empresa)
    {
        string sql = "update empresas set estatus= 'A'where id_empresa='" + empresa.ToString() + "'"; ;
        return ejecuta.update_insert_delete(sql);
    }

    public bool desEstatus(string empresa)
    {
        string sql = "update empresas set estatus= 'I' where id_empresa='" + empresa.ToString() + "'"; ;
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet cargaUsuariosEmpresas()
    {
        string sql = "select u.id_usuario, e.nombre, u.id_empresa from usuarios_empresas u left join empresas e on u.id_empresa=e.id_empresa WHERE u.id_empresa<>0 ";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool borraUsuarioEmpresa(string claveusuario, int empresa)
    {
        string sql = "delete from usuarios_empresas where id_usuario= '" + claveusuario + "' and id_empresa=" + empresa.ToString();
        return ejecuta.update_insert_delete(sql);
    }

    public bool agregaUsuarioEmpresa(string id_usuario, int empresa)
    {
        string sql = "insert into usuarios_empresas values('" + id_usuario + "'," + empresa.ToString() + ") ";
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet cargaBancos()
    {
        string sql = "SELECT id_banco, nombre, clave FROM bancos ";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool borraBanco(string clavebanco)
    {
        string sql = "delete from bancos where id_banco= '" + clavebanco + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool agregaBanco(string nombre, string clave)
    {
        string sql = "insert into bancos (nombre,clave) values('" + nombre + "','" + clave + "') ";
        return ejecuta.update_insert_delete(sql);
    }

    public bool actualizaBanco(int banco, string nombre, string clave)
    {
        string sql = "update bancos set   nombre='" + nombre + "',  clave='" + clave + "' where id_banco='" + banco.ToString() + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool existeUsuarioEmpresa(string usuario, int empresa)
    {
        string sql = "SELECT count(*) FROM usuarios_empresas where id_usuario='" + usuario + "' and id_empresa=" + empresa;
        return ejecuta.scalarBool(sql);
    }

    public bool existeUsuarioContraseña(string usuario)
    {
        string sql = "select count(*) from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarBool(sql);
    }

    public string obtieneContraseña(string usuario)
    {
        string sql = "select contrasena from usuarios where id_usuario='" + usuario + "'";
        return ejecuta.scalarString(sql);
    }

    public bool actualizaContraseña(string usuario, string contraseña)
    {
        string sql = "update usuarios set  contrasena='" + contraseña + "' where id_usuario='" + usuario + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet obtieneCuentas(string usuario, int empresa, string periodo)
    {
        DateTime fechaActual = fechaLocal.obtieneFechaLocal();
        DateTime fechaAnterior = fechaActual.AddMonths(-1);
        string sql = "select tabla.no_afiliacion,tabla.no_cuenta from (select a.no_afiliacion,c.no_cuenta from clientes c, afiliaciones a where c.estatus_cliente='A' AND c.fecha_fin>=convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) AND c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and a.id_afiliacion = (select top 1 a.id_afiliacion from afiliaciones a inner join usuarios_afiliaciones ua on ua.id_afiliacion = a.id_afiliacion where ua.id_usuario='" + usuario + "' and fecha<=convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) order by a.id_afiliacion desc) group by c.no_cuenta,a.no_afiliacion union all select a.no_afiliacion,c.no_cuenta from clientes c, afiliaciones a where c.estatus_cliente='I' AND c.fecha_baja between convert(date,'" + fechaAnterior.ToString("yyyy-MM-dd") + "',120) and convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) AND c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and a.id_afiliacion = (select top 1 a.id_afiliacion from afiliaciones a inner join usuarios_afiliaciones ua on ua.id_afiliacion = a.id_afiliacion where ua.id_usuario='" + usuario + "' and fecha<=convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) order by a.id_afiliacion desc) group by a.no_afiliacion, c.no_cuenta) as tabla group by tabla.no_afiliacion,tabla.no_cuenta order by tabla.no_cuenta ";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtieneRegistros(string cuenta, string periodo, int empresa, DateTime periodoGenera)
    {
        DateTime fechaActual = periodoGenera;
        DateTime fechaFinBaja= fechaActual.AddMonths(1);
        fechaFinBaja = fechaFinBaja.AddDays(-1);
        DateTime fechaAnterior = periodoGenera.AddMonths(-1);
        string sql = "select c.referencia,c.monto from clientes c " +
                     "where c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and c.fecha_fin>=convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) and estatus_cliente='A' and c.no_cuenta='" + cuenta + "' and  CAST((Substring(Convert(char(10),c.fecha_inicio,126),1,4)+Substring(Convert(char(10),c.fecha_inicio,126),6,2)) as int)<=cast('" + fechaActual.ToString("yyyyMM") + "' as int)" +
                     "and (SELECT TOP 1 e.movimiento from detalle_cliente e where e.id_cliente=c.id_cliente and e.periodo='" + fechaActual.ToString("yyyyMM") + "' and e.id_empresa=c.id_empresa order by e.consecutivo desc) not in ('P','M') " +
                     "union all " +
                     "select c.referencia,c.monto from clientes c " +
                     "where c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and c.fecha_fin>=convert(date,'" + fechaActual.ToString("yyyy-MM-dd") + "',120) and estatus_cliente='A' and c.no_cuenta='" + cuenta + "' and  CAST((Substring(Convert(char(10),c.fecha_inicio,126),1,4)+Substring(Convert(char(10),c.fecha_inicio,126),6,2)) as int)<=cast('" + fechaActual.ToString("yyyyMM") + "' as int)" +
                     "AND C.id_cliente not in(select d.id_cliente from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + fechaActual.ToString("yyyyMM") + "' group by d.id_cliente) " +
                     "union all " +
                     "select c.referencia,c.monto from clientes c " +
                     "where c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and c.fecha_baja between convert(date,'" + fechaAnterior.ToString("yyyy-MM-dd") + "',120) and convert(date,'" + fechaFinBaja.ToString("yyyy-MM-dd") + "',120) and estatus_cliente='I' and c.no_cuenta='" + cuenta + "' and  CAST((Substring(Convert(char(10),c.fecha_inicio,126),1,4)+Substring(Convert(char(10),c.fecha_inicio,126),6,2)) as int)<=cast('" + fechaActual.ToString("yyyyMM") + "' as int)" +
                     "and (SELECT TOP 1 e.movimiento from detalle_cliente e where e.id_cliente=c.id_cliente and e.periodo='" + fechaActual.ToString("yyyyMM") + "' and e.id_empresa=c.id_empresa order by e.consecutivo desc) not in ('P','M') " +
                     "union all " +
                     "select c.referencia,c.monto from clientes c " +
                     "where c.id_empresa=" + empresa.ToString() + " and c.periodo_pago='" + periodo + "' and c.fecha_baja between convert(date,'" + fechaAnterior.ToString("yyyy-MM-dd") + "',120) and convert(date,'" + fechaFinBaja.ToString("yyyy-MM-dd") + "',120) and estatus_cliente='I' and c.no_cuenta='" + cuenta + "' and  CAST((Substring(Convert(char(10),c.fecha_inicio,126),1,4)+Substring(Convert(char(10),c.fecha_inicio,126),6,2)) as int)<=cast('" + fechaActual.ToString("yyyyMM") + "' as int)" +
                     "AND C.id_cliente not in(select d.id_cliente from detalle_cliente d where d.id_empresa=" + empresa.ToString() + " and d.periodo='" + fechaActual.ToString("yyyyMM") + "' group by d.id_cliente)";
        return ejecuta.obtieneInformacion(sql);
    }

    public string obtieneDigitosAfiliacion(string usuario)
    {
        string sql = "select top 1 no_afiliacion from afiliaciones a inner join usuarios_afiliaciones ua on ua.id_afiliacion = a.id_afiliacion where ua.id_usuario='" + usuario + "' and fecha<=convert(date,'" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',120) order by a.id_afiliacion desc";
        return ejecuta.scalarString(sql);
    }

    public DataSet cargaUsuariosafi()
    {
        string sql = "select ua.id_usuario, a.id_afiliacion, a.no_afiliacion from usuarios_afiliaciones ua left join afiliaciones a on ua.id_afiliacion=a.id_afiliacion";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool borraUsuarioafi(string claveusuario, int afi)
    {
        string sql = "delete from usuarios_afiliaciones where id_usuario= '" + claveusuario + "' and id_afiliacion='" + afi.ToString() + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool existeAfiu(string usuario, int id_afiliacion)
    {
        string sql = "select count(*) from usuarios_afiliaciones where id_usuario='" + usuario + "' and id_afiliacion ='" + id_afiliacion.ToString() + "'";
        return ejecuta.scalarBool(sql);
    }

    public bool existeUsaafi(string usuario)
    {
        string sql = "select count(id_usuario) from usuarios_afiliaciones where id_usuario='" + usuario + "'";
        return ejecuta.scalarBool(sql);
    }

    public bool agregaUsuarioafi(string id_usuario, int afiliacion)
    {
        string sql = "insert into usuarios_afiliaciones (id_usuario, id_afiliacion) values('" + id_usuario + "'," + afiliacion.ToString() + ") ";
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet cargaAfiliacion()
    {
        string sql = "SELECT a.id_afiliacion, a.no_afiliacion, a.criterio, a.dias, a.fecha, b.nombre, a.comision,b.id_banco FROM afiliaciones a left join bancos b on a.id_banco=b.id_banco ";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool borraAfiliacion(string claveafi)
    {
        string sql = "delete from afiliaciones where id_afiliacion= '" + claveafi + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool existeAfi(string afi, int id_afiliacion)
    {
        string sql;
        if (id_afiliacion != 0)
            sql = "select count(no_afiliacion) from afiliaciones where no_afiliacion='" + afi + "' and id_afiliacion <>'" + id_afiliacion.ToString() + "'";
        else
            sql = "select count(no_afiliacion) from afiliaciones where no_afiliacion='" + afi + "'";
        return ejecuta.scalarBool(sql);
    }

    public bool agregaAfiliacion(string noafi, int criterio, int dias, string fecha, string banco, double comision)
    {
        string sql = "insert into afiliaciones (no_afiliacion,criterio,dias,fecha,id_banco,comision) values('" + noafi.ToString() + "','" + criterio.ToString() + "','" + dias.ToString() + "','" + fecha + "','" + banco + "','" + comision.ToString() + "') ";
        return ejecuta.update_insert_delete(sql);
    }

    public bool actualizaAfiliacion(int afiliacion, string noafi, int criterio, int dias, string fecha, int id_banco, double com)
    {
        string sql = "update afiliaciones set  no_afiliacion='" + noafi.ToString() + "', criterio ='" + criterio.ToString() + "', dias ='" + dias.ToString() + "', fecha='" + fecha + "',  id_banco='" + id_banco.ToString() + "', comision=" + com.ToString() + " where id_afiliacion='" + afiliacion.ToString() + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public string obtieneAfibanco(string afi)
    {
        string sql = "select id_banco from afiliaciones where id_afiliacion=" + afi;
        return ejecuta.scalarString(sql);
    }

    public bool obtieneRelacion(string afi)
    {
        string sql = "select count(*) from usuarios_afiliaciones where id_afiliacion=" + afi.ToString();
        return ejecuta.scalarBool(sql);
    }

    public bool agregaClientesMasivos(string referencia, string nombre, string apPat, string apMat, double monto, string cuenta, string fechaIni, string fechaFin, string periodo, int id_empresa)
    {
        string sql = "INSERT INTO clientes (id_cliente,referencia,nombre_tarjetahabiente,ap_pat_tarjetahabiente,ap_mat_tarjetahabiente,monto,no_cuenta,fecha_inicio,fecha_fin,estatus_cliente,periodo_pago,id_empresa, nombre, apellido_paterno,apellido_materno) VALUES((select isnull((select top 1 id_cliente from clientes order by id_cliente desc),0))+1,'" + referencia + "','" + nombre + "','" + apPat + "','" + apMat + "'," + monto.ToString() + ",'" + cuenta + "','" + fechaIni + "','" + fechaFin + "','A','" + periodo + "'," + id_empresa.ToString() + ",'" + nombre + "','" + apPat + "','" + apMat + "')";
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet llenaGridRespuesta(string usuario)
    {
        string sql = "select referencia,periodo,fecha,cuenta,movimiento,motivo_rechazo,monto from temp_carga_datos where usuario='" + usuario + "' order by referencia asc";
        return ejecuta.obtieneInformacion(sql);
    }

    public int obtienePagPag()
    {
        string sql = "select count(*) from temp_carga_datos where movimiento='P'";
        return ejecuta.scalarInt(sql);
    }

    public int obtienePagRec()
    {
        string sql = "select count(*) from temp_carga_datos where movimiento='R'";
        return ejecuta.scalarInt(sql);
    }

    public double obtieneMontoPagPag()
    {
        string sql = "select isnull(sum(monto),0) as monto from temp_carga_datos where movimiento='P'";
        return ejecuta.scalarDouble(sql);
    }

    public double obtieneMontoPagRec()
    {
        string sql = "select isnull(sum(monto),0) as monto from temp_carga_datos where movimiento='R'";
        return ejecuta.scalarDouble(sql);
    }

    public int obtieneIdCliente(string referencia)
    {
        string sql = "select id_cliente from clientes where referencia='" + referencia + "'";
        return ejecuta.scalarInt(sql);
    }

    public int obtieneIdConsecutivoDetalleCliente(int idCliente)
    {
        string sql = "select max(consecutivo) from detalle_cliente where id_cliente='" + idCliente + "'";
        return ejecuta.scalarInt(sql);
    }

    public bool agregaDetalleCliente(int idCliente, string periodo, string fechaRechazo, string motivoRechazo, string movimiento, string idEmpresa, int consecutivo, string usuario)
    {
        string sql = "";
        if (movimiento == "R")
            sql = "insert into detalle_cliente (id_cliente,periodo,fecha_pago_rechazado,motivo_rechzado,movimiento,id_empresa,consecutivo,id_usuario) values (" + idCliente.ToString() + ",'" + periodo + "','" + fechaRechazo + "','" + motivoRechazo + "','" + movimiento + "'," + idEmpresa + "," + consecutivo + ",'" + usuario + "')";
        else
            sql = "insert into detalle_cliente (id_cliente,periodo,fecha_pago,movimiento,id_empresa,consecutivo,id_usuario) values (" + idCliente.ToString() + ",'" + periodo + "','" + fechaRechazo + "','" + movimiento + "'," + idEmpresa + "," + consecutivo + ",'" + usuario + "')";
        return ejecuta.update_insert_delete(sql);
    }

    public bool agregaRegistroTMP(int idCliente, string referencia, string periodo, string fecha, string cuenta, string movimiento, string motivoRechazo, string monto, string usuario)
    {
        string sql = " insert into temp_carga_datos (id_cliente,referencia,periodo,fecha,cuenta,movimiento,motivo_rechazo,monto,usuario) values (" + idCliente + ",'" + referencia + "'," + periodo + ",'" + fecha + "','" + cuenta + "','" + movimiento + "','" + motivoRechazo + "'," + monto + ",'" + usuario + "')";
        return ejecuta.update_insert_delete(sql);
    }

    public void borrarTempCargaDatos()
    {
        string sql = "delete from temp_carga_datos";
        ejecuta.update_insert_delete(sql);
    }

    public int obtieneConsecutivo(string periodo, int empresa)
    {
        //string sql = "select count(*) from archivo_cobranza where periodo='" + periodo + "'";
        string sql = "select num_archivo from empresas where id_empresa=" + empresa;
        return ejecuta.scalarInt(sql);
    }

    public bool llenaRegistroCobranza(string periodo, int consecutivo, string nombre, int elementos, double montoTotal, string ruta, string empresa)
    {
        string sql = "insert into archivo_cobranza values('" + periodo + "'," + consecutivo.ToString() + ",'" + nombre + "'," + elementos.ToString() + "," + montoTotal.ToString("0.00") + ",'" + ruta + "') update empresas set num_archivo=" + consecutivo;
        return ejecuta.update_insert_delete(sql);
    }

    public bool llenaPermisos(string usuariolog, string usuario)
    {
        string sql = "delete from permisos_tmp where usuario='" + usuariolog + "'";
        bool borrada = ejecuta.update_insert_delete(sql);
        if (borrada)
        {
            sql = "INSERT INTO permisos_tmp SELECT id_permiso,nombre,'" + usuariolog + "' FROM PERMISOS P WHERE id_permiso NOT IN(SELECT id_permiso FROM usuarios_permisos where id_usuario='" + usuario + "' )";
            bool creada = ejecuta.update_insert_delete(sql);
            if (creada)
            {
                sql = "DELETE FROM usuarios_permisos_tmp WHERE usuario='" + usuariolog + "'";
                bool borrada1 = ejecuta.update_insert_delete(sql);
                if (borrada1)
                    sql = "INSERT INTO usuarios_permisos_tmp SELECT id_usuario,id_permiso,'" + usuariolog + "' FROM usuarios_permisos WHERE id_usuario='" + usuario + "'";
            }
        }
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet cargaPermisos(string usuariolog)
    {
        string sql = "SELECT id_permiso,nombre FROM permisos_tmp WHERE usuario='" + usuariolog + "'";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet cargaUPermisos(string usuario, string usuarioLog)
    {
        string sql = "SELECT U.id_permiso,P.nombre FROM usuarios_permisos_tmp U inner join permisos p on p.id_permiso=u.id_permiso where u.usuario='" + usuarioLog + "' and u.id_usuario='" + usuario + "'";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool borraPermisos(string usuario, string usuariolog)
    {
        string sql = "delete from usuarios_permisos_tmp where id_usuario= '" + usuario + "'";
        bool borrado = ejecuta.update_insert_delete(sql);
        if (borrado)
        {
            sql = "DELETE FROM PERMISOS_TMP";
            bool borrado1 = ejecuta.update_insert_delete(sql);
            if (borrado1)
                sql = "INSERT INTO permisos_tmp SELECT id_permiso,nombre,'" + usuariolog + "' FROM PERMISOS P WHERE id_permiso NOT IN(SELECT id_permiso FROM usuarios_permisos where id_usuario='" + usuario + "' )";
        }
        return ejecuta.update_insert_delete(sql);
    }

    public bool borraPermiso(string usuario, string usuarioLog, int permiso)
    {
        string sql = "DELETE FROM usuarios_permisos_tmp WHERE usuario='" + usuarioLog + "' AND id_permiso=" + permiso.ToString() + " AND id_usuario='" + usuario + "'";
        bool borrado = ejecuta.update_insert_delete(sql);
        if (borrado)
        {
            string nombre = ejecuta.scalarString("SELECT NOMBRE FROM PERMISOS WHERE id_permiso=" + permiso.ToString());
            sql = "INSERT INTO permisos_tmp values(" + permiso.ToString() + ",'" + nombre + "','" + usuarioLog + "')";
        }
        return ejecuta.update_insert_delete(sql);
    }

    public bool agregaPermiso(string usuario, int permiso, string usuarioLog)
    {
        string sql = "DELETE FROM permisos_tmp WHERE usuario='" + usuarioLog + "' AND id_permiso=" + permiso.ToString() + "";
        bool borrado = ejecuta.update_insert_delete(sql);
        if (borrado)
        {
            sql = "DELETE FROM usuarios_permisos_tmp WHERE usuario='" + usuarioLog + "'";
            bool borrado1 = ejecuta.update_insert_delete(sql);
            if (borrado1)
                sql = "INSERT INTO usuarios_permisos_tmp  SELECT '" + usuario + "', id_permiso,'" + usuarioLog + "' FROM permisos WHERE id_permiso NOT IN (SELECT ID_PERMISO FROM PERMISOS_TMP WHERE usuario='" + usuarioLog + "')";
        }
        return ejecuta.update_insert_delete(sql);
    }

    public bool agregaPermisos(string usuario, string usuarioLog)
    {
        string sql = "DELETE FROM permisos_tmp WHERE usuario='" + usuarioLog + "' ";
        bool borrado = ejecuta.update_insert_delete(sql);
        if (borrado)
        {
            sql = "DELETE FROM usuarios_permisos_tmp WHERE usuario='" + usuarioLog + "'";
            bool borrado1 = ejecuta.update_insert_delete(sql);
            if (borrado1)
                sql = "INSERT INTO usuarios_permisos_tmp SELECT '" + usuario + "',id_permiso,'" + usuarioLog + "' FROM permisos WHERE id_permiso NOT IN (SELECT ID_PERMISO FROM PERMISOS_TMP WHERE usuario='" + usuarioLog + "')";
        }
        return ejecuta.update_insert_delete(sql);
    }

    public bool validaPermiso(string usuario, string usuarioLog)
    {
        string sql = "DELETE FROM usuarios_permisos where id_usuario='" + usuario + "'";
        bool borrado = ejecuta.update_insert_delete(sql);
        if (borrado)
            sql = "INSERT INTO usuarios_permisos SELECT ID_USUARIO,ID_PERMISO FROM usuarios_permisos_tmp WHERE usuario='" + usuarioLog + "' AND id_usuario='" + usuario + "'";
        return ejecuta.update_insert_delete(sql);
    }

    public bool tienePermisso15(string usuario, int permiso)
    {
        string sql = "select count(*) from usuarios_permisos where id_usuario='" + usuario + "' and id_permiso=" + permiso;
        return ejecuta.scalarBool(sql);
    }

    public bool obtienePago(string usuario, int periodo)
    {
        int año = fechaLocal.obtieneFechaLocal().Year;
        string valor = año.ToString() + periodo.ToString().PadLeft(2, '0');
        string sql = "select top 1 case movimiento when 'R' then 0 when 'P' then 1 when 'M' then 1 else 0 end as Ene from detalle_cliente where periodo='" + valor + "' and id_cliente=" + usuario + " order by periodo,consecutivo desc";
        return ejecuta.scalarBool(sql);
    }

    public string obtienePagoMov(string usuario)
    {
        string periodo = fechaLocal.obtieneFechaLocal().Year.ToString() + fechaLocal.obtieneFechaLocal().Month.ToString().PadLeft(2, '0');
        string sql = "select top 1 movimiento from detalle_cliente where periodo='" + periodo + "' and id_cliente=" + usuario + " order by periodo,consecutivo desc";
        return ejecuta.scalarString(sql);
    }

    public bool agregaPagMan(string periodo, string fechaPag, string motivo, int empresa, int consecutivo, int cliente, string usuario)
    {
        string sql = "insert into detalle_cliente(id_cliente,periodo,fecha_pago,motivo_rechzado,movimiento,id_empresa,consecutivo,id_usuario) values(" + cliente + ",'" + periodo + "','" + fechaPag + "','" + motivo + "','M'," + empresa.ToString() + "," + consecutivo.ToString() + ",'" + usuario + "')";
        return ejecuta.update_insert_delete(sql);
    }

    public DataSet obtieneInfoCliente(string cliente)
    {
        string sql = "select referencia,nombre,apellido_paterno, apellido_materno,monto,no_cuenta,fecha_inicio,fecha_fin,id_banco,vigencia_cuenta,nombre_tarjetahabiente,ap_pat_tarjetahabiente,ap_mat_tarjetahabiente,telefono,celular,correo from clientes where id_cliente=" + cliente;
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtienePagos(string usuario, int empresa, int año)
    {
        string sql = "select " +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "01' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P1," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "02' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) AS P2," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "03' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P3," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "04' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P4," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "05' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P5," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "06' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P6," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "07' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P7," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "08' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P8," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "09' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P9," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "10' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P10," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "11' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P11," +
"(select top 1 movimiento from detalle_cliente where id_cliente=" + usuario + " and periodo='" + año.ToString() + "12' and id_empresa=" + empresa.ToString() + " order by consecutivo desc) as P12";
        return ejecuta.obtieneInformacion(sql);
    }

    public string obtieneClientesActivosInactivos(string estatus, int empresa)
    {
        string sql = "select count(*) from clientes where id_empresa=" + empresa.ToString() + " and estatus_cliente='" + estatus + "'";
        return ejecuta.scalarString(sql);
    }

    public bool actualizaMonto(double monto)
    {
        string sql = "Update clientes set monto=" + monto.ToString();
        return ejecuta.update_insert_delete(sql);
    }

    public bool obtieneRelacionU(string user)
    {
        string sql = "select sum(tabla.registros) as registros from(select count(*) as registros from usuarios_afiliaciones  where id_usuario= '" + user + "' union all select count(*) as registros from usuarios_empresas  where id_usuario='" + user + "' union all select count(*) as registros from usuarios_permisos where id_usuario='" + user + "') as tabla";
        return ejecuta.scalarBool(sql);
    }

    public int existeRelacionBancos(string idBanco)
    {
        string sql = "select sum(tabla.registros) as registros from(select count(*) as registros from afiliaciones  where id_banco=" + idBanco.ToString() + " union all select count(*) as registros from clientes where id_banco=" + idBanco.ToString() + " ) as tabla";
        return ejecuta.scalarInt(sql);
    }

    public int obtieneCoincidenciasEmpresas(int idEmpresa)
    {
        string sql = "select sum(tabla.registros) as registros from(select count(*) as registros from clientes  where id_empresa=" + idEmpresa.ToString() + " union all select count(*) as registros from usuarios_empresas  where id_empresa=" + idEmpresa.ToString() + " union all select count(*) as registros from detalle_cliente where id_empresa=" + idEmpresa.ToString() + " union all select count(*) as registros from bitacora where id_empresa=" + idEmpresa.ToString() + " union all select count(*) as registros from detalle_bitacora where id_empresa=" + idEmpresa.ToString() + ") as tabla";
        return ejecuta.scalarInt(sql);
    }

    public int validaPagoRechazado(int idCliente, string periodo)
    {
        string sql = "select count(*) from detalle_cliente where id_cliente='" + idCliente.ToString() + "' and periodo='" + periodo + "' and movimiento='R'";
        return ejecuta.scalarInt(sql);
    }

    public int validaPagoRealizado(int idCliente, string periodo)
    {
        string sql = "select count(*) from detalle_cliente where id_cliente='" + idCliente.ToString() + "' and periodo='" + periodo + "' and movimiento in ('P','M')";
        return ejecuta.scalarInt(sql);
    }

    public object[] revisaPeriodosPagados(int idCliente, string año)
    {
        string sql = "select p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12 from periodos_pagos where id_cliente=" + idCliente.ToString() + " and ano=" + año;
        return ejecuta.scalarObjectArreglo(sql);
    }
    public bool agregarPeriodosPagos(int idCliente, string movimiento, string año, string mes)
    {
        string periodo = switchPeriodo(Convert.ToInt32(mes));
        string sql = "insert into periodos_pagos (id_cliente,ano," + periodo + ") values(" + idCliente.ToString() + "," + año + ",'" + movimiento + "')";
        return ejecuta.update_insert_delete(sql);
    }

    public int cuentaPeriodosPagosPorAño(int idCliente, string año)
    {
        string sql = "select count(*) from periodos_pagos where id_cliente=" + idCliente.ToString() + " and ano=" + año;
        return ejecuta.scalarInt(sql);
    }

    public bool actualizarPeriodosPagos(int idCliente, string movimiento, string año, string mes)
    {
        string periodo = switchPeriodo(Convert.ToInt32(mes));
        string sql = "update periodos_pagos set " + periodo + "='" + movimiento + "' where id_cliente=" + idCliente.ToString() + " and ano=" + año;
        return ejecuta.update_insert_delete(sql);
    }

    private string switchPeriodo(int mes)
    {
        string periodo = "";
        switch (mes)
        {
            case 01:
                periodo = "p1";
                break;
            case 02:
                periodo = "p2";
                break;
            case 03:
                periodo = "p3";
                break;
            case 04:
                periodo = "p4";
                break;
            case 05:
                periodo = "p5";
                break;
            case 06:
                periodo = "p6";
                break;
            case 07:
                periodo = "p7";
                break;
            case 08:
                periodo = "p8";
                break;
            case 09:
                periodo = "p9";
                break;
            case 10:
                periodo = "p10";
                break;
            case 11:
                periodo = "p11";
                break;
            case 12:
                periodo = "p12";
                break;
        }
        return periodo;
    }

    public DataTable obtienePagosRetrasados(int empresa, int cliente, int año, string periodo)
    {
        string sql = "";
        DataTable valores = new DataTable();
        valores.Columns.Add("periodo");
        valores.Columns.Add("movimiento");
        int primerPeriodo = 0;
        try { primerPeriodo = Convert.ToInt32(ejecuta.scalarString("select periodo from detalle_cliente where id_cliente=" + cliente.ToString() + " and id_empresa=" + empresa.ToString() + " and consecutivo=1")); }
        catch (Exception) { primerPeriodo = 0; }
        if (primerPeriodo > 0)
        {
            int primerPago = 0;
            try { primerPago = Convert.ToInt32(ejecuta.scalarString("SELECT case when fecha_reactiva is null then substring(convert(char(10),fecha_inicio,126),1,4)+substring(convert(char(10),fecha_inicio,126),6,2) else substring(convert(char(10),fecha_reactiva,126),1,4)+substring(convert(char(10),fecha_reactiva,126),6,2) end as fecha_inicio FROM CLIENTES where id_cliente=" + cliente.ToString() + " and id_empresa=" + empresa.ToString())); }
            catch (Exception) { primerPago = 0; }
            if (primerPago == 0)
            {
                object[] regis = new object[2] { fechaLocal.obtieneFechaLocal().ToString("yyyyMM"), "" };
                valores.Rows.Add(regis);
            }
            else
            {
                string fechaIni = primerPago.ToString().Substring(0, 4) + "-" + primerPago.ToString().Substring(4, 2) + "-01";
                string fechaFin = periodo.Substring(0, 4) + "-" + periodo.Substring(4, 2) + "-01";
                DateTime peridos = Convert.ToDateTime(fechaIni);
                DateTime periodoActual = Convert.ToDateTime(fechaFin);
                while (peridos <= periodoActual)
                {
                    sql = "select top 1 isnull(movimiento,'') from detalle_cliente where id_cliente=" + cliente.ToString() + " and cast(periodo as int) =" + peridos.ToString("yyyyMM") + " and id_empresa=" + empresa.ToString() + " order by consecutivo desc";
                    string movimiento = ejecuta.scalarString(sql);
                    if (movimiento != "P" && movimiento != "M")
                    {
                        object[] regis = new object[2] { peridos.ToString("yyyyMM"), movimiento };
                        valores.Rows.Add(regis);
                    }
                    peridos = peridos.AddMonths(1);
                }
            }
        }
        else
        {
            int primerPago = 0;
            try { primerPago = Convert.ToInt32(ejecuta.scalarString("SELECT substring(convert(char(10),fecha_inicio,126),1,4)+substring(convert(char(10),fecha_inicio,126),6,2) as fecha_inicio FROM CLIENTES where id_cliente=" + cliente.ToString() + " and id_empresa=" + empresa.ToString())); }
            catch (Exception) { primerPago = 0; }
            if (primerPago == 0)
            {
                object[] regis = new object[2] { fechaLocal.obtieneFechaLocal().ToString("yyyyMM"), "" };
                valores.Rows.Add(regis);
            }
            else
            {
                string fechaIni = primerPago.ToString().Substring(0, 4) + "-" + primerPago.ToString().Substring(4, 2) + "-01";
                string fechaFin = periodo.Substring(0, 4) + "-" + periodo.Substring(4, 2) + "-01";
                DateTime peridos = Convert.ToDateTime(fechaIni);
                DateTime periodoActual = Convert.ToDateTime(fechaFin);
                while (peridos <= periodoActual)
                {
                    object[] regis = new object[2] { peridos.ToString("yyyyMM"), "" };
                    valores.Rows.Add(regis);
                    peridos = peridos.AddMonths(1);
                }
            }
        }
        return valores;
    }

    public int verificaPagos(int cliente, string periodo)
    {
        string sql = "select count(tabla.movimiento) from (select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "01' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "02' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "03' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "04' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "05' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "06' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "07' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "08' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "09' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "10' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "11' order by consecutivo desc) as movimiento union all select (select top 1 movimiento from detalle_cliente where movimiento='R' and id_cliente=" + cliente.ToString() + " and periodo = '" + periodo.ToString() + "12' order by consecutivo desc) as movimiento ) as tabla where not tabla.movimiento is null";
        return ejecuta.scalarInt(sql);
    }

    public DataSet llenaAñoConsultas(int idEmpresa)
    {
        string sql = "select distinct SUBSTRING(periodo,1,4)as periodo from detalle_cliente where id_empresa=" + idEmpresa.ToString() + " group by periodo order by periodo desc";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet llenaSuperConsultas(string año, string mes, string estatusCliente, string periodicidad, string estatusPago, string filtroCampo, string filtroTetxo, string clienteTarjeta, int idEmpresa)
    {
        string sql = "", filtro = " ";
        if (filtroCampo == "0")
        {
            if (clienteTarjeta == "C")
                filtro = " and (c.referencia like '%" + filtroTetxo + "%' or c.nombre like '%" + filtroTetxo + "%' or c.apellido_paterno like '%" + filtroTetxo + "%' or c.apellido_materno like '%" + filtroTetxo + "%' or (c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) like '%" + filtroTetxo + "%') ";
            else
                filtro = " and (c.referencia like '%" + filtroTetxo + "%' or c.nombre_tarjetahabiente like '%" + filtroTetxo + "%' or c.ap_pat_tarjetahabiente like '%" + filtroTetxo + "%' or c.ap_mat_tarjetahabiente like '%" + filtroTetxo + "%' or (c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) like '%" + filtroTetxo + "%') ";
        }
        else if (filtroCampo == "1")
            filtro = " and c.referencia like '%" + filtroTetxo + "%' ";
        else if (filtroCampo == "2")
        {
            if (clienteTarjeta == "C")
                filtro = " and (c.nombre like '%" + filtroTetxo + "%' or c.apellido_paterno like '%" + filtroTetxo + "%' or c.apellido_materno like '%" + filtroTetxo + "%' or (c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) like '%" + filtroTetxo + "%') ";
            else
                filtro = " and (c.nombre_tarjetahabiente like '%" + filtroTetxo + "%' or c.ap_pat_tarjetahabiente like '%" + filtroTetxo + "%' or c.ap_mat_tarjetahabiente like '%" + filtroTetxo + "%' or (c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) like '%" + filtroTetxo + "%') ";
        }
        if (estatusPago != "0")
        {
            sql = "select dc.id_cliente,c.referencia,(c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno)as nombre,(c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente)as tarjetahabiente,dc.fecha_pago,dc.fecha_pago_rechazado,dc.motivo_rechzado " +
                  "from detalle_cliente dc " +
                  "inner join clientes c on c.id_cliente=dc.id_cliente " +
                  "where dc.periodo like '%" + año + mes + "%' and dc.movimiento='" + estatusPago + "' and c.estatus_cliente='" + estatusCliente + "'" + filtro + "and c.periodo_pago = '" + periodicidad + "' " +
                  "and consecutivo in (select max(consecutivo) from detalle_cliente where periodo=dc.periodo and id_cliente =dc.id_cliente) and dc.id_empresa=" + idEmpresa.ToString();
        }
        else
        {
            sql = "select distinct c.id_cliente,c.referencia,(c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno)as nombre,(c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente)as tarjetahabiente,'' as fecha_pago,'' as fecha_pago_rechazado,'' as motivo_rechzado from clientes c left join detalle_cliente dc on dc.id_cliente=c.id_cliente and dc.id_empresa=c.id_empresa where c.estatus_cliente='" + estatusCliente + "'" + filtro + " and c.periodo_pago= '" + periodicidad + "' and c.id_cliente not in (select id_cliente from detalle_cliente where id_empresa=c.id_empresa AND periodo='" + año + mes + "' and id_cliente=c.id_cliente) and c.id_empresa=" + idEmpresa.ToString();
        }
        return ejecuta.obtieneInformacion(sql);
    }

    public int clientesAPagarEnPeriodo(int idEmpresa, string año, string mes)
    {
        string fecha = "01/" + mes + "/" + año;
        DateTime fechaIni = Convert.ToDateTime(fecha).AddMonths(-1);
        DateTime fechaFin = Convert.ToDateTime(fecha).AddDays(-1);
        string sql = "select sum(tabla.registros) from(select count(*) as registros from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "' " +
                     "union all " +
                     "select count(*) as registros from clientes where estatus_cliente='A' and fecha_fin>'" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and id_empresa=" + idEmpresa.ToString() + " and id_cliente not in(select id_cliente " +
                     "from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "')) as tabla";
        return ejecuta.scalarInt(sql);
    }

    public double montoTotalAPagarEnPeriodo(int idEmpresa, string año, string mes)
    {
        string fecha = "01/" + mes + "/" + año;
        DateTime fechaIni = Convert.ToDateTime(fecha).AddMonths(-1);
        DateTime fechaFin = Convert.ToDateTime(fecha).AddDays(-1);
        string sql = "select isnull( sum(tabla.monto),0) from(select isnull(sum(isnull(monto,0)),0) as monto from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "' union all select isnull(sum(isnull(monto,0)),0) as monto from clientes where estatus_cliente='A' and fecha_fin>'" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and id_empresa=" + idEmpresa + " and id_cliente not in(select id_cliente from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "'  )) as tabla";
        return ejecuta.scalarDouble(sql);
    }

    public DataSet llenaSuperConsultasMesNuevo(int idEmpresa, string año, string mes)
    {
        string fecha = "01/" + mes + "/" + año;
        DateTime fechaIni = Convert.ToDateTime(fecha).AddMonths(-1);
        DateTime fechaFin = Convert.ToDateTime(fecha).AddDays(-1);
        string sql = "select id_cliente,referencia,(nombre+' '+apellido_paterno+' '+apellido_materno)as nombre,(nombre_tarjetahabiente+' '+ap_pat_tarjetahabiente+' '+ap_mat_tarjetahabiente)as tarjetahabiente,'' as fecha_pago,'' as fecha_pago_rechazado,'' as motivo_rechzado " +
                     "from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "' " +
                     "union all " +
                     "(select id_cliente,referencia,(nombre+' '+apellido_paterno+' '+apellido_materno)as nombre,(nombre_tarjetahabiente+' '+ap_pat_tarjetahabiente+' '+ap_mat_tarjetahabiente)as tarjetahabiente,'' as fecha_pago,'' as fecha_pago_rechazado,'' as motivo_rechzado " +
                     "from clientes where estatus_cliente='A' and fecha_fin>'" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and id_empresa=" + idEmpresa.ToString() + " and id_cliente not in(select id_cliente " +
                     "from clientes where estatus_cliente='I' and id_empresa=" + idEmpresa + " and fecha_baja between '" + fechaFin.ToString("yyyy-MM-dd") + "' and '" + fechaIni.ToString("yyyy-MM-dd") + "'))";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool agregaComentario(string cliente, string comentario, string usuario, DateTime fecha, int empresa)
    {
        string sql = "insert into comentarios values(" + cliente + ",(select isnull((select top 1 id_comentario from comentarios where id_cliente=" + cliente + " order by id_comentario desc),0))+1,'" + comentario + "','" + usuario + "','" + fecha.ToString("yyyy-MM-dd") + "','" + fecha.ToString("HH:mm:ss") + "',0," + empresa + ",'',null,null)";
        return ejecuta.update_insert_delete(sql);
    }

    public bool actualizaComentario(string cliente, int empresa, string consecutivo, string usuario)
    {
        E_Utilities.Fechas fechas = new E_Utilities.Fechas();
        string sql = "update comentarios set leido=1,usuario_lectura='" + usuario + "',fecha_lectura='" + fechas.obtieneFechaLocal().ToString("yyyy-dd-MM") + "',hora_lectura='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "' where id_cliente=" + cliente + " and id_comentario=" + consecutivo;
        return ejecuta.update_insert_delete(sql);
    }

    public int cuentaClientesEstatus(int idEmpresa, string estatus)
    {
        string sql = "select count(*) from clientes where estatus_cliente='" + estatus + "' and id_empresa=" + idEmpresa.ToString();
        return ejecuta.scalarInt(sql);
    }

    public int cuentaClientesMovimientoPeriodoEstatus(int idEmpresa, string año, string mes, string estatusCliente, string estatusPago)
    {
        string sql = "select count(dc.id_cliente) from detalle_cliente dc where dc.movimiento='" + estatusPago + "' and dc.periodo='" + año + mes + "' and dc.consecutivo in (select top 1 consecutivo from detalle_cliente where id_cliente=dc.id_cliente and id_empresa=dc.id_empresa and periodo=dc.periodo order by consecutivo desc) and dc.id_cliente in (select c.id_cliente from clientes c where c.id_empresa=dc.id_empresa and c.estatus_cliente='" + estatusCliente + "') and dc.id_empresa=" + idEmpresa.ToString();
        return ejecuta.scalarInt(sql);
    }

    public string cuentaTotalClientes(int idEmpresa)
    {
        string sql = "select count(*) from clientes where id_empresa=" + idEmpresa.ToString();
        return ejecuta.scalarInt(sql).ToString();
    }

    public string calculaMontoPagoRealizadoPeriodo(int IdEmpresa, string estatusCliente, string año, string mes, string movimiento)
    {
        string sql = "select sum(monto) from clientes where id_empresa=" + IdEmpresa.ToString() + " and estatus_cliente='" + estatusCliente + "' and id_cliente in " +
                    "(select dc.id_cliente from detalle_cliente dc where periodo='" + año + mes + "' and movimiento='" + movimiento + "' and id_empresa=" + IdEmpresa.ToString() + ")";
        return ejecuta.scalarInt(sql).ToString();
    }

    public int cuentaClientesPendientesPeriodoEstatus(int idEmpresa, string año, string mes, string estatusCliente, string estatusPago)
    {
        string sql = "select count(*) from clientes where estatus_cliente='" + estatusCliente + "' and id_cliente not in " +
                     "(select dc.id_cliente from detalle_cliente dc where periodo='" + año + mes + "' and id_empresa=" + idEmpresa.ToString() + ") and id_empresa=" + idEmpresa.ToString();
        return ejecuta.scalarInt(sql);
    }

    public bool existeReferencia(int empresa, string referencia)
    {
        string sql = "Select count(*) from clientes where upper(referencia)='" + referencia.Trim().ToUpper() + "' and id_empresa='"+empresa+"'";
        return ejecuta.scalarBool(sql);
    }

    public void borraDetallePago(int empresa, int cliente, string periodoPago)
    {
        string sql = "delete from detalle_cliente where id_cliente=" + cliente.ToString() + " and periodo='" + periodoPago + "' and id_empresa=" + empresa.ToString();
        bool borrado = ejecuta.update_insert_delete(sql);
    }

    public bool actualizaReferencia(int cliente, string referencia)
    {
        string sql = "update clientes set referencia='" + referencia.Trim() + "' where id_cliente=" + cliente.ToString();
        return ejecuta.update_insert_delete(sql);
    }

    public DataTable obtienePagosEstadisticos(System.Web.UI.WebControls.Label[] periodos, int empresa)
    {
        string sql = "select 'Ene' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[0].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Feb' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[1].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Mar' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[2].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Abr' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[3].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'May' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[4].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Jun' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[5].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Jul' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[6].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Ago' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[7].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Sep' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[8].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Oct' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[9].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Nov' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[10].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Dic' as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[11].Text + "' as int)),0) AS Pendientes ";
        return ejecuta.obtieneInformacionDt(sql);

    }

    public DataTable obtieneMontosEstadisticos(System.Web.UI.WebControls.Label[] periodos, int empresa)
    {
        string sql = "select 'Ene' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[0].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Feb' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[1].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Mar' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[2].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Abr' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[3].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'May' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[4].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Jun' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[5].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Jul' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[6].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Ago' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[7].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Sep' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[8].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Oct' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[9].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Nov' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[10].Text + "' as int)),0) AS Pendientes " +
"union all " +
"select 'Dic' as mes," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='P')),0) as Pagados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='R')),0) as Rechazados," +
"isnull((select sum(monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='M')),0) as Sucursal," +
"isnull((select SUM(MONTO) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12 not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + periodos[11].Text + "' as int)),0) AS Pendientes ";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public bool generaBitacora(string periodoConvertido, string idEmpresa, string archivo, string usuario)
    {
        archivo = archivo.Substring(0, archivo.IndexOf('.'));
        string sql = "insert into archivos_aplicados(nombre_archivo,periodo_aplicado,fecha,hora,usuario,id_empresa) values('" + archivo + "','" + periodoConvertido + "','" + fechaLocal.obtieneFechaLocal().ToString("yyyy-dd-MM") + "','" + fechaLocal.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario + "'," + idEmpresa + ")";
        return ejecuta.update_insert_delete(sql);
    }

    public bool existeCargaPervia(string periodoConvertido, string idEmpresa, string archivo, string usuario)
    {
        archivo = archivo.Substring(0, archivo.IndexOf('.'));
        string sql = "Select count(*) from archivos_aplicados where ltrim(rtrim(nombre_archivo))='" + archivo.ToString().Trim() + "' and id_consecutivo=" + idEmpresa.ToString();
        return ejecuta.scalarBool(sql);
    }

    public string obtienePeridoAplicado(string idEmpresa, string archivo)
    {
        archivo = archivo.Substring(0, archivo.IndexOf('.'));
        string sql = "Select periodo_aplicado from archivos_aplicados where ltrim(rtrim(nombre_archivo))='" + archivo.ToString().Trim() + "' and id_empresa=" + idEmpresa.ToString();
        return ejecuta.scalarString(sql);
    }

    public DataTable obtienePagoEstadisticos(System.Web.UI.WebControls.Label label, int empresa, string periodo)
    {
        string sql = "select case " + periodo.ToString() + " when 1 then 'Ene' when 2 then 'Feb' when 3 then 'Mar' when 4 then 'Abr' when 5 then 'May' when 6 then 'Jun' when 7 then 'Jul' when 8 then 'Ago' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov' when 12 then 'Dic' else '' end  as mes," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='P')),0) as Pagados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='R')),0) as Rechazados," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='M')),0) as Sucursal," +
"isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + " not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + label.Text + "' as int)),0) AS Pendientes ";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtieneMontoEstadisticos(System.Web.UI.WebControls.Label label, int empresa, string periodo)
    {
        string sql = "select case " + periodo.ToString() + " when 1 then 'Ene' when 2 then 'Feb' when 3 then 'Mar' when 4 then 'Abr' when 5 then 'May' when 6 then 'Jun' when 7 then 'Jul' when 8 then 'Ago' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov' when 12 then 'Dic' else '' end  as mes," +
"Convert(money,isnull((select sum(c.monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='P')),0),1) as Pagados," +
"isnull((select sum(c.monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='R')),0) as Rechazados," +
"isnull((select sum(c.monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + "='M')),0) as Sucursal," +
"isnull((select sum(c.monto) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano=" + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo.ToString() + " not in('M','R','P')) AND CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)<=cast('" + label.Text + "' as int)),0) AS Pendientes ";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public bool registraBaja(int empresa, string referencia, string cliente, string tarhetaHabiente, string fechaBaja, string motivo, string usuario, string usuarioAut, string id_cliente, byte[] imagen, string extension)
    {
        string sql = "insert into bajas_inmediatas(id_baja,id_empresa,referencia,cliente,tarjeta_habiente,fecha_baja,hora_baja,usuario_baja,usuario_aut,fecha_registro,motivo,id_cliente) values((select case (select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + ") when 0 then 1 else (select top 1 id_baja from bajas_inmediatas where id_empresa=" + empresa.ToString() + " order by id_baja desc)+1 end)," + empresa.ToString() + ",'" + referencia + "','" + cliente + "','" + tarhetaHabiente + "','" + fechaBaja + "','" + fechaLocal.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario + "','" + usuarioAut + "','" + fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + motivo + "'," + id_cliente + ") " +
            "delete from clientes where id_cliente=" + id_cliente + " " +
            "delete from detalle_cliente where id_cliente=" + id_cliente;
        bool agregado = ejecuta.update_insert_delete(sql);        
        return agregado;
    }

    public string obtieneReferencia(string cliente)
    {
        string sql = "select referencia from clientes where id_cliente=" + cliente;
        return ejecuta.scalarString(sql);
    }

    public DataTable obtieneAltasEstadisticos(System.Web.UI.WebControls.Label[] periodos)
    {
        string sql = "select 'Ene' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[0].Text + "' as int)) as altas " +
"union all " +
"select 'Feb' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[1].Text + "' as int)) as altas " +
"union all " +
"select 'Mar' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[2].Text + "' as int)) as altas " +
"union all " +
"select 'Abr' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[3].Text + "' as int)) as altas " +
"union all " +
"select 'May' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[4].Text + "' as int)) as altas " +
"union all " +
"select 'Jun' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[5].Text + "' as int)) as altas " +
"union all " +
"select 'Jul' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[6].Text + "' as int)) as altas " +
"union all " +
"select 'Ago' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[7].Text + "' as int)) as altas " +
"union all " +
"select 'Sep' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[8].Text + "' as int)) as altas " +
"union all " +
"select 'Oct' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[9].Text + "' as int)) as altas " +
"union all " +
"select 'Nov' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[10].Text + "' as int)) as altas " +
"union all " +
"select 'Dic' as mes,(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + periodos[11].Text + "' as int)) as altas";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtieneAltasMensuales(System.Web.UI.WebControls.Label label, string periodo)
    {
        string sql = "select case " + periodo.ToString() + " when 1 then 'Ene' when 2 then 'Feb' when 3 then 'Mar' when 4 then 'Abr' when 5 then 'May' when 6 then 'Jun' when 7 then 'Jul' when 8 then 'Ago' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov' when 12 then 'Dic' else '' end  as mes," +
"(select count(*) from clientes where CAST((Substring(Convert(char(10),fecha_inicio,126),1,4)+Substring(Convert(char(10),fecha_inicio,126),6,2)) as int)=cast('" + label.Text + "' as int)) as altas ";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtieneBajasEstadisticos(System.Web.UI.WebControls.Label[] periodos, int empresa)
    {
        string sql = "select 'Ene' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[0].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[0].Text + " ' as int))) as bajas union all " +
"select 'Feb' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[1].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[1].Text + " ' as int))) as bajas union all " +
"select 'Mar' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[2].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[2].Text + " ' as int))) as bajas union all " +
"select 'Abr' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[3].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[3].Text + " ' as int))) as bajas union all " +
"select 'May' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[4].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[4].Text + " ' as int))) as bajas union all " +
"select 'Jun' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[5].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[5].Text + " ' as int))) as bajas union all " +
"select 'Jul' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[6].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[6].Text + " ' as int))) as bajas union all " +
"select 'Ago' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[7].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[7].Text + " ' as int))) as bajas union all " +
"select 'Sep' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[8].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[8].Text + " ' as int))) as bajas union all " +
"select 'Oct' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[9].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[9].Text + " ' as int))) as bajas union all " +
"select 'Nov' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[10].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[10].Text + " ' as int))) as bajas union all " +
"select 'Dic' as mes,((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[11].Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + periodos[11].Text + " ' as int))) as bajas";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtieneBajasMensuales(System.Web.UI.WebControls.Label label, string periodo, int empresa)
    {
        string sql = "select case " + periodo.ToString() + " when 1 then 'Ene' when 2 then 'Feb' when 3 then 'Mar' when 4 then 'Abr' when 5 then 'May' when 6 then 'Jun' when 7 then 'Jul' when 8 then 'Ago' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov' when 12 then 'Dic' else '' end  as mes," +
"((select count(*) from bajas_inmediatas where id_empresa=" + empresa.ToString() + "  and CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + label.Text + " ' as int))+(select count(*) from clientes where estatus_cliente='I' AND CAST((Substring(Convert(char(10),fecha_baja,126),1,4)+Substring(Convert(char(10),fecha_baja,126),6,2)) as int)=cast('" + label.Text + " ' as int))) as altas ";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtienePagosBancos(int empresa, int banco)
    {
        string sql = "select 12 as mes, sum(tabla.Pagados) as Pagados,sum(tabla.Rechazados) as Rechazados from(select 'Ene' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p1='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Feb' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p2='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Mar' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p3='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Abr' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p4='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'May' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p5='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Jun' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p6='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Jul' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p7='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Ago' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p8='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Sep' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p9='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Oct' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p10='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Nov' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p11='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados union all " +
"select 'Dic' as mes,isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p12='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados) as tabla";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public DataTable obtienePagoBanco(System.Web.UI.WebControls.Label label, int empresa, string periodo, int banco)
    {
        string sql = "select case " + periodo.ToString() + " when 1 then 'Ene' when 2 then 'Feb' when 3 then 'Mar' when 4 then 'Abr' when 5 then 'May' when 6 then 'Jun' when 7 then 'Jul' when 8 then 'Ago' when 9 then 'Sep' when 10 then 'Oct' when 11 then 'Nov' when 12 then 'Dic' else '' end  as mes,  isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo + "='P') and c.id_banco=" + banco.ToString() + "),0) as Pagados," +
       "isnull((select count(*) from clientes c where c.id_empresa=" + empresa.ToString() + " and c.id_cliente in (select p.id_cliente from periodos_pagos P where P.ano= " + fechaLocal.obtieneFechaLocal().Year.ToString() + " and p" + periodo + "='R') and c.id_banco=" + banco.ToString() + "),0) as Rechazados";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public string obtieneinfoDetalleCliente(string usuario, int empresa, string periodo)
    {
        string sql = "select top 1 case movimiento when 'P' THEN 'Fecha Pago: '+Convert(char(10),fecha_pago,126) when 'R' THEN 'Fecha de Rechazo: '+convert(char(10),fecha_pago_rechazado,126)+'. Motivo: '+motivo_rechzado when 'M' then 'Fecha Pago: '+convert(char(10),fecha_pago,126)+'. Folio: '+motivo_rechzado else 'Pendiente de Pago' end as info   from detalle_cliente where id_cliente=" + usuario + " and periodo='" + periodo + "' and id_empresa=" + empresa + "  order by consecutivo desc";
        return ejecuta.scalarString(sql);
    }

    public DataSet llenaAñoClien(int idEmpresa)
    {
        string sql = "select distinct tabla.ano,tabla.anio from (select distinct substring(Convert(char(10),fecha_inicio,126),1,4) as ano,"
+ "substring(Convert(char(10),fecha_inicio,126),1,4) as anio from clientes where id_empresa=" + idEmpresa.ToString() + " union all "
+ "select Convert(char(4),getdate(),126) as ano,Convert(char(4),getdate(),126) as anio) as tabla order by tabla.ano desc";
        return ejecuta.obtieneInformacion(sql);
    }

    public DataSet obtieneDoctos(string referencia, int empresa, int id)
    {
        string sql = "select id_documento,referencia,id_empresa,id_cliente,extension,identificador_proceso from documentos where ltrim(rtrim(referencia))='" + referencia + "' and id_empresa=" + empresa.ToString() + " and id_cliente=" + id.ToString() + " order by identificador_proceso asc";
        return ejecuta.obtieneInformacion(sql);
    }

    public bool eliminarFotoDanos(int idDocto, string referencia, int id_empresa, int idCliente)
    {
        string sql = "delete from documentos where id_documento=" + idDocto.ToString() + " and ltrim(rtrim(referencia))='" + referencia + "' and id_empresa=" + id_empresa.ToString() + " and id_cliente=" + idCliente.ToString();
        return ejecuta.update_insert_delete(sql);
    }

    public byte[] obtienePdf(int empresa, int idCliente, string referencia, int id)
    {
        string sql = "select pdfs from documentos where id_empresa=" + empresa.ToString() + " and referencia='" + referencia + "' and id_documento=" + id.ToString() + " and id_cliente=" + idCliente.ToString();
        return ejecuta.scalarByteArreglo(sql);

    }

    public bool actualizaNoReconocido(string cliente, int consecutivo, int check, string usuario, string periodoP, string periodoAño, string periodoMes)
    {
        string sqlPeriodosPagosUpdateNoRec = "update periodos_pagos set " + periodoP + "='C' where id_cliente=" + cliente + " and ano=" + periodoAño;
        string sqlPeriodosPagosUpdateSiRec = "update periodos_pagos set " + periodoP + "=(select top 1 movimiento from detalle_cliente where id_cliente = " + cliente + " and periodo = '" + periodoAño + periodoMes + "' order by consecutivo desc) where id_cliente=" + cliente + " and ano=" + periodoAño;
        string sql = "update detalle_cliente set noReconocido =" + check + ", id_usuario_noReconocido='" + usuario + "' where id_cliente=" + cliente + " and consecutivo=" + consecutivo.ToString();
        if (periodoP != "")
        {
            if (check == 1)
                return ejecuta.update_insert_delete(sql + "  " + sqlPeriodosPagosUpdateNoRec);
            else
                return ejecuta.update_insert_delete(sql + "  " + sqlPeriodosPagosUpdateSiRec);
        }
        else
            return false;
    }

    public bool tieneNoreconocido(string cliente)
    {
        string sql = "select count(*) from detalle_cliente where id_cliente=" + cliente ;
        return ejecuta.scalarBool(sql);
    }
    public string obtieneNombreUsuarioAlta(string cliente)
    {
        string sql = "select isnull(usuario_autoriza_alta,'')+';'+convert(char(10),fecha_inicio,126) from clientes where id_cliente=" + cliente;
        return ejecuta.scalarString(sql);
    }

    public void agregaPago(string afiliacion, string referencias, string cuenta, string monto)
    {
        string sql = "declare @existe int set @existe = (select count(*) from tmp_pagos where afiliacion='" + afiliacion + "' and referencia like '%" + referencias + "%' and cuenta='" + cuenta + "' and monto=" + monto + ") " +
"if(@existe=0) begin insert into tmp_pagos values((select isnull((select top 1 id from tmp_pagos order by id desc),0)+1),'" + afiliacion + "','" + referencias + "','" + cuenta + "'," + monto + ") end";
        ejecuta.update_insert_delete(sql);
    }

    public DataTable obtienePagosArchivo()
    {
        string sql = "select * from tmp_pagos";
        return ejecuta.obtieneInformacionDt(sql);
    }

    public double obtieneMontoPagar()
    {
        string sql = "select isnull((select sum(monto) from tmp_pagos),0)";
        return ejecuta.scalarDouble(sql);
    }

    public void borraTmp()
    {
        string sql = "delete from tmp_pagos";
        ejecuta.update_insert_delete(sql);
    }

    public DataTable obtieneTmpPagos()
    {
        string sql = "select * from tmp_pagos";
        return ejecuta.obtieneInformacionDt(sql);
    }
}