<%@ Application Language="C#" %>

<script runat="server">

    ILogger _log = LogHelper.Logger;
    void Application_Start(object sender, EventArgs e)
    {
        log4net.Config.XmlConfigurator.Configure();
        _log.Info("******************* Application_Start *******************");
        // Code that runs on application startup
        //try
        //{            
        //    var tmpConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //    System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
        //    sqlCommand.Connection = new System.Data.SqlClient.SqlConnection(tmpConString);
        //    sqlCommand.CommandText = @"Update T_Sessions set Statut_Ses=0 where Id_Ses in (
        // SELECT id_ses from T_Sessions as Sessions inner join Session_Dates as Dates on Sessions.Id_Ses = Dates.FK_Session_ID
        // where (
        // Sessions.fk_Statut_validation_session = (select Id_Typ_Statut from Type_Statuts where Libelle_Type_Statut='Planifiée')
        //    and Sessions.Statut_Ses=1
        // and Dates.Date_Session<=GETDATE()))";
        //    sqlCommand.Connection.Open();
        //    sqlCommand.ExecuteNonQuery();
        //    sqlCommand.Connection.Close();        
        //}
        //catch(Exception err)
        //{
        //    _log.Error("Erreur Global.asax", err);
        //}        
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        _log.Info("******************* Application_End *******************");
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception err = Server.GetLastError();
        Application.Add("LastError", err);

        if (err is HttpUnhandledException)
            if (err.InnerException != null)
            {
                err = err.InnerException;
                _log.Error(err.Message, err);
            }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        _log.Info("******************* Session_Start *******************");
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        _log.Info("******************* Session_End *******************");
    }

</script>
