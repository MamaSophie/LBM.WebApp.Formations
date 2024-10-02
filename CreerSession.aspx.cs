using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public partial class CreerSession : System.Web.UI.Page
{
	private string uploadPdfFolder = "Upload/Pdf/Formations/";
	private string strSortExpression = "Date";
	private string strSortExpressionGV2 = "Nom";
	private string strIdParam = "Id";
	private int heuresParJour = Constants.DureeJourEnHeures;
	private decimal dureeFormationEnJours;
	ILogger _log = LogHelper.Logger;
	private List<DateSessionXml> sourceData
	{
		get
		{
			if (DDLFormations.SelectedIndex == 0) return null;
			var retour = listeDates;
			if (sortDirection == SortDirection.Ascending.ToString())
			{
				switch (sortExpression)
				{
					case "Date":
						retour.Sort((s1, s2) => DateTime.Compare(s1.DateSession, s2.DateSession));
						break;
				}
			}
			else
			{
				switch (sortExpression)
				{
					case "Date":
						retour.Sort((s2, s1) => DateTime.Compare(s1.DateSession, s2.DateSession));
						break;
				}
			}
			return retour;
		}
	}
	private List<Formation> formations
	{
		get
		{
			return (List<Formation>)Session["Formations"];
		}
		set
		{
			Session["Formations"] = value;
		}
	}
	private string sortDirection
	{
		get
		{
			if (ViewState["sortDirection"] == null)
			{
				ViewState["sortDirection"] = "Ascending";
			}
			return ViewState["sortDirection"].ToString();
		}
		set
		{
			ViewState["sortDirection"] = value;
		}
	}
	private string sortExpression
	{
		get
		{
			if (ViewState["sortExpression"] == null)
			{
				ViewState["sortExpression"] = strSortExpression;
			}
			return ViewState["sortExpression"].ToString();
		}
		set
		{
			ViewState["sortExpression"] = value;
		}
	}
	private SessionXml sessionFormation
	{
		get
		{
			SessionXml retour = null;
			if (ViewState["SessionSerialized"] != null)
			{
				retour = (SessionXml)ToolBox.DeserializeXmlViewState(ViewState["SessionSerialized"].ToString(), typeof(SessionXml));
			}
			return retour;
		}
		set
		{
			ViewState["SessionSerialized"] = ToolBox.SerializeXmlViewState(value);
		}
	}
	private List<DateSessionXml> listeDates
	{
		get
		{
			List<DateSessionXml> retour = new List<DateSessionXml>();
			if (ViewState["DateSessionsSerialized"] != null)
			{
				retour = (List<DateSessionXml>)ToolBox.DeserializeXmlViewState(ViewState["DateSessionsSerialized"].ToString(), typeof(List<DateSessionXml>));
			}
			return retour;
		}
		set
		{
			ViewState["DateSessionsSerialized"] = ToolBox.SerializeXmlViewState(value);
		}
	}
	private List<Inscrit> listeInscrits
	{
		get
		{
			var retour = new ModelManager().GetInscritsSession(idSession).FindAll(i => i.Statut_Ins);
			if (sortDirectionGV2 == SortDirection.Ascending.ToString())
			{
				switch (sortExpressionGV2)
				{
					case "Nom":
						retour.Sort((s1, s2) => string.Compare(s1.User.NomComplet, s2.User.NomComplet));
						break;
					case "Dpt":
						retour.Sort((s1, s2) => string.Compare(s1.Departement.Lib_Dep, s2.Departement.Lib_Dep));
						break;
				}
			}
			else
			{
				switch (sortExpressionGV2)
				{
					case "Nom":
						retour.Sort((s2, s1) => string.Compare(s1.User.NomComplet, s2.User.NomComplet));
						break;
					case "Dpt":
						retour.Sort((s2, s1) => string.Compare(s1.Departement.Lib_Dep, s2.Departement.Lib_Dep));
						break;
				}
			}
			return retour;
		}
	}
	private string sortDirectionGV2
	{
		get
		{
			if (ViewState["sortDirectionGV2"] == null)
			{
				ViewState["sortDirectionGV2"] = "Ascending";
			}
			return ViewState["sortDirectionGV2"].ToString();
		}
		set
		{
			ViewState["sortDirectionGV2"] = value;
		}
	}
	private string sortExpressionGV2
	{
		get
		{
			if (ViewState["sortExpressionGV2"] == null)
			{
				ViewState["sortExpressionGV2"] = strSortExpressionGV2;
			}
			return ViewState["sortExpressionGV2"].ToString();
		}
		set
		{
			ViewState["sortExpressionGV2"] = value;
		}
	}
	private int idSession
	{
		get
		{
			return Convert.ToInt32(ViewState["idSession"].ToString());
		}
		set
		{
			ViewState["idSession"] = value;
		}
	}
	private bool isCreateMode
	{
		get
		{
			return (bool)ViewState["isCreateMode"];
		}
		set
		{
			ViewState["isCreateMode"] = value;
		}
	}
	private bool isEmailToSend
	{
		get
		{
			return ViewState["isEmailToSend"] != null ? (bool)ViewState["isEmailToSend"] : false;
		}
		set
		{
			ViewState["isEmailToSend"] = value;
		}
	}
	//private bool isFormationAllowsEmails { get {  return new ModelManager().GetFormations().Exists(f=>f.Id_fm==int.Parse(DDLFormations.SelectedValue) && f.eMail_Auto);}}
	private bool isFormationAllowsEmails { get { return new ModelManager().GetFormations(int.Parse(DDLFormations.SelectedValue)).Exists(f => f.eMail_Auto); } }
	protected void Page_Load(object sender, EventArgs e)
	{
		// CONTROLE HABILITATION //
		if (!UC_Header1.IsAdminRole)
		{
			_log.Info("Utilisateur n'appartient pas au groupe B-APPL-INSCRIPTION_FORMATION, redirection vers default.aspx");
			Response.Redirect("default.aspx");
		}
		// CONTROLE HABILITATION //END

		BtnAdd.Text = "Valider";
		BtnAdd.Click += BtnAdd_Click;
		BtnCancel.Click += BtnCancel_Click;
		BtnAddSession.Click += BtnAddsession_Click;
		BtnCancelSession.Click += BtnCancelSession_Click;
		BtnAddDate.Click += BtnAddDate_Click;
		BtnCancelDate.Click += BtnCancelDate_Click;
		BtnEdit.Click += (o, evt) => toggleReadOnlySession();
		BtnEmail.Click += (o, evt) => { PanelEmail.Visible = true; PanelModal.Visible = PanelEmail.Visible; };
		BtnAnnulerEmail.Click += (o, evt) => { TBEmail.Text = ""; PanelEmail.Visible = false; PanelModal.Visible = PanelEmail.Visible; };
		BtnEnvoyerEmail.Click += BtnEnvoyerEmail_Click;
		DDLFormations.SelectedIndexChanged += DDLFormations_SelectedIndexChanged;
		GridView1.DataBound += GridView_DataBound;
		GridView1.RowDataBound += GridView1_RowDataBound;
		GridView1.RowCommand += GridView1_RowCommand;
		GridView1.Sorting += GridView1_Sorting;
		GridView2.DataBound += GridView_DataBound;
		GridView2.RowDataBound += GridView2_RowDataBound;
		GridView2.RowCommand += GridView2_RowCommand;
		GridView2.Sorting += GridView2_Sorting;
		if (!Page.IsPostBack)
		{
			isCreateMode = true;
			listeDates = new List<DateSessionXml>();
			var tmpModel = new ModelManager();
			formations = tmpModel.GetFormations(0);
			DDLFormations.DataSource = (from f in formations.FindAll(f => f.Statut_fm) orderby f.Lib_fm select f).ToList();
			DDLFormations.DataValueField = "Id_fm";
			DDLFormations.DataTextField = "Lib_fm";
			DDLFormations.DataBind();
			DDLFormations.Items.Insert(0, new ListItem("Sélectionner une formation", "", true));
			DDLFormations.SelectedIndex = 0;

			var tmpListeFormateurs = tmpModel.GetFormateurs().FindAll(u => u.Statut_u);
			tmpListeFormateurs.Sort((u1, u2) => string.Compare(u1.Nom_U, u2.Nom_U));
			DDLFormateurs.DataSource = tmpListeFormateurs;
			DDLFormateurs.DataValueField = "Id_u";
			DDLFormateurs.DataTextField = "NomComplet";
			DDLFormateurs.DataBind();
			DDLFormateurs.Items.Insert(0, new ListItem("Sélectionner un formateur", "", true));
			DDLFormateurs.SelectedIndex = 0;

			DDLSalles.DataSource = tmpModel.GetSalles();
			DDLSalles.DataValueField = "Id_Salle";
			DDLSalles.DataTextField = "Lib_Salle";
			DDLSalles.DataBind();
			DDLSalles.Items.Insert(0, new ListItem("Sélectionner une salle", "", true));
			DDLSalles.SelectedIndex = 0;

			List<string> tmpHeures = new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
			DDLHDEB.DataSource = tmpHeures;
			DDLHDEB.DataBind();
			DDLHDEB.SelectedIndex = 0;
			DDLHFIN.DataSource = tmpHeures;
			DDLHFIN.DataBind();
			DDLHFIN.SelectedIndex = 0;
			List<string> tmpMinutes = new List<string>();
			for (int i = 0; i <= 59; i++)
			{
				tmpMinutes.Add(i < 10 ? "0" + i.ToString() : i.ToString());
			}
			DDLMDEB.DataSource = tmpMinutes;
			DDLMDEB.DataBind();
			DDLMFIN.SelectedIndex = 0;
			DDLMFIN.DataSource = tmpMinutes;
			DDLMFIN.DataBind();
			DDLMFIN.SelectedIndex = 0;
			var tmpSessions = tmpModel.GetSessions(null, null);
			if (Request["idSession"] != null)
			{

				idSession = Convert.ToInt32(Request["idSession"]);
				if (!tmpSessions.Exists(s => s.Id_Ses == idSession)) return;
				isCreateMode = false;
				DDLFormations.Enabled = false;
				DDLFormations.SelectedValue = tmpSessions.Find(s => s.Id_Ses == idSession).fk_id_fm.ToString();
				var tmpFormation = formations.Find(f => f.Id_fm.ToString() == DDLFormations.SelectedValue);
				bindFormation(tmpFormation);
				bindSession();

				GridView1.SelectedIndex = 0;

				var tmpId = GridView1.DataKeys[GridView1.SelectedIndex].Values["Id"];
				var tmpDate = listeDates.Find(s => s.Id == (int)tmpId);
				bindDate(tmpDate);
			}
			else
			{
				idSession = tmpModel.T_Sessions.Count() > 0 ? tmpModel.T_Sessions.Max(s => s.Id_Ses) + 1 : 1;
			}
			if (!isCreateMode)
			{
				BtnEmail.Visible = isFormationAllowsEmails && tmpSessions.Find(s => s.Id_Ses == idSession).StatutValidation == Constants.SessionValidee;
			}
		}
		Label1.Text = isCreateMode ? "Créer une session" : "Modifier une session";
	}
	void BtnEnvoyerEmail_Click(object sender, EventArgs e)
	{
		sendEmails();
	}
	void BtnCancelDate_Click(object sender, EventArgs e)
	{
		if (isCreateMode)
		{
			bindDate(null);
		}
		else
		{
			bindDate(listeDates.Find(d => d.Id == (int)GridView1.DataKeys[GridView1.SelectedIndex].Values["Id"]));
		}
	}
	void BtnAddDate_Click(object sender, EventArgs e)
	{
		var tmpTest = checkData(sender as WebControl);
		if (!string.IsNullOrEmpty(tmpTest))
		{
			UC_ErrorPanel1.DisplayError(tmpTest);
			return;
		}

		var tmpDates = listeDates;
		DateSessionXml tmpDate = null;
		var tmpModel = new ModelManager();
		var tmpSession = new T_Sessions();
		if (GridView1.SelectedIndex == -1)
		{
			var tmpMaxId = tmpModel.Session_Dates.Count() == 0 ? 1 : tmpModel.Session_Dates.ToList().Max(d => d.Date_Ses_Id) + 1;
			tmpMaxId = listeDates.Count > 0 ? Math.Max(tmpMaxId, listeDates.Max(s => s.Id) + 1) : tmpMaxId;
			var tmpid = tmpMaxId;
			tmpDate = new DateSessionXml(tmpid, idSession, DateTime.Parse(TBDATE.Text), DDLHDEB.SelectedValue + ":" + DDLMDEB.SelectedValue + ":00", DDLHFIN.SelectedValue + ":" + DDLMFIN.SelectedValue + ":00", Convert.ToInt32(DDLSalles.SelectedValue));
			tmpDates.Add(tmpDate);
		}
		else
		{
			tmpDate = tmpDates.Find(d => d.Id.ToString() == GridView1.DataKeys[GridView1.SelectedIndex].Values["Id"].ToString());
			// Envoi email si session validée sur changement date ou heure //
			if (tmpDate != null)
				tmpSession = tmpModel.GetSessions(tmpDate.IdSession, 0).FirstOrDefault();
			if (tmpSession != null && tmpSession.Inscrits.Any() && (tmpDate.DateSession != DateTime.Parse(TBDATE.Text) || tmpDate.HeureDebut != DDLHDEB.SelectedValue + ":" + DDLMDEB.SelectedValue + ":00" || tmpDate.HeureFin != DDLHFIN.SelectedValue + ":" + DDLMFIN.SelectedValue + ":00") && (tmpDate.IdSalle != Convert.ToInt32(DDLSalles.SelectedValue)))
			{
				isEmailToSend = true;
				tmpDate.IsDateModified = true;
				tmpDate.IsLocationModified = true;
			}
			else if (tmpSession != null && tmpSession.Inscrits.Any() && (tmpDate.DateSession != DateTime.Parse(TBDATE.Text) || tmpDate.HeureDebut != DDLHDEB.SelectedValue + ":" + DDLMDEB.SelectedValue + ":00" || tmpDate.HeureFin != DDLHFIN.SelectedValue + ":" + DDLMFIN.SelectedValue + ":00"))
			{
				isEmailToSend = true;
				tmpDate.IsDateModified = true;
			}
			else if (tmpSession != null && tmpSession.Inscrits.Any() && (tmpDate.IdSalle != Convert.ToInt32(DDLSalles.SelectedValue)))
			{
				isEmailToSend = isEmailToSend ? true : tmpSession.IsSessionValidee;
				tmpDate.IsLocationModified = true;
			}
			// Envoi email si session validée sur changement date ou heure //END
			tmpDate.DateSession = DateTime.Parse(TBDATE.Text);
			tmpDate.HeureDebut = DDLHDEB.SelectedValue + ":" + DDLMDEB.SelectedValue + ":00";
			tmpDate.HeureFin = DDLHFIN.SelectedValue + ":" + DDLMFIN.SelectedValue + ":00";
			tmpDate.IdSalle = Convert.ToInt32(DDLSalles.SelectedValue);
		}
		listeDates = tmpDates;
		GridView1.DataSource = sourceData;
		GridView1.DataBind();

		if (GridView1.SelectedIndex >= 0)
		{
			bindDate(sourceData.Find(d => d.Id == Convert.ToInt32(GridView1.DataKeys[GridView1.SelectedIndex].Values[strIdParam])));
		}
		else
		{
			bindDate(null);
		}
	}
	void BtnCancelSession_Click(object sender, EventArgs e)
	{
		if (isCreateMode)
		{
			sessionFormation = null;
			bindContolsToSession();
		}
		else
		{
			bindSession();
		}
	}
	void BtnAddsession_Click(object sender, EventArgs e)
	{
		string tmpError = "";
		tmpError += !ToolBox.IsNumeric(TBMinInscrits.Text) || int.Parse(TBMinInscrits.Text) <= 0 ? "Le nombre minimum d'inscrits doit être > 0. <br/> " : "";
		tmpError += !ToolBox.IsNumeric(TBMaxInscrits.Text) || int.Parse(TBMaxInscrits.Text) <= 0 ? "Le nombre maximum d'inscrits doit être > 0. <br/> " : "";
		if (DDLFormateurs.SelectedIndex == 0) tmpError += "Veuillez sélectionner un formateur.<br/>";
		if (!string.IsNullOrEmpty(tmpError))
		{
			UC_ErrorPanel1.DisplayError(tmpError);
			return;
		}
		var tmpModel = new ModelManager();
		var tmpMaxId = tmpModel.T_Sessions.Count() == 0 ? 1 : tmpModel.T_Sessions.ToList().Max(s => s.Id_Ses) + 1;
		var tmpid = isCreateMode ? tmpMaxId : idSession;
		var tmpStatutValidation = (from s in tmpModel.Type_Statuts where s.Libelle_Type_Statut.StartsWith(Constants.SessionPlanifiee) select s.Id_Typ_Statut).Single();
		sessionFormation = new SessionXml(tmpid, Convert.ToInt32(DDLFormations.SelectedValue), Convert.ToInt32(DDLFormateurs.SelectedValue), Convert.ToInt32(TBMinInscrits.Text), Convert.ToInt32(TBMaxInscrits.Text), tmpStatutValidation);
		toggleReadOnlySession();
	}
	void DDLFormations_SelectedIndexChanged(object sender, EventArgs e)
	{
		var tmpModel = new ModelManager();
		var tmpFormation = formations.Find(f => f.Id_fm.ToString() == DDLFormations.SelectedValue);
		bindFormation(tmpFormation);
		listeDates = null;
		idSession = tmpModel.T_Sessions.Count() > 0 ? tmpModel.T_Sessions.Max(s => s.Id_Ses) + 1 : 1;
		sessionFormation = null;
		bindContolsToSession();
		GridView1.DataSource = sourceData;
		GridView1.DataBind();
		displayDates(false);

	}
	void BtnCancel_Click(object sender, EventArgs e)
	{
		isEmailToSend = false;
		Response.Redirect("ListeSessions.aspx");
	}
	void BtnAdd_Click(object sender, EventArgs e)
	{
		_log.Info("CreerSession, Début Ajout Session");
		var tmpModel = new ModelManager();
		var tmpFormation = formations.Find(f => f.Id_fm.ToString() == DDLFormations.SelectedValue);
		dureeFormationEnJours = tmpFormation.Type_Duration.ToLower() == "j" ? tmpFormation.Duration : ((int)(tmpFormation.Duration / heuresParJour) + 1);
		if (((int)dureeFormationEnJours) != dureeFormationEnJours)
		{
			dureeFormationEnJours = (int)dureeFormationEnJours + 1;
		}
		if (listeDates.Count != dureeFormationEnJours)
		{
			UC_ErrorPanel1.DisplayError("Le nombre de dates doit être égal au nombre de journées de la formation.");
			return;
		}

		if (!isFormationAllowsEmails || !isEmailToSend)
		{
			valideSession(sender as WebControl);
			_log.Info("CreerSession, Fin Ajout Session");
			Response.Redirect("ListeSessions.aspx",false);
		}
		else
		{
			Action<WebControl> tmpValideSession = new Action<WebControl>(valideSession);
			var tmpMsg = "Un email avec les nouveaux éléments partira automatiquement aux inscrits.<br/>Confirmez-vous ?";
			UC_ErrorPanel1.DisplayConfirmAction(tmpMsg, tmpValideSession, null, "ListeSessions.aspx");
		}
	}
	void valideSession(WebControl pWebControl)
	{
		var tmpModel = new ModelManager();
		List<Session_Dates> tmpDates = new List<Session_Dates>();
		listeDates.ForEach(d => tmpDates.Add(new Session_Dates { Date_Ses_Id = d.Id, FK_Session_ID = sessionFormation.Id, Date_Session = d.DateSession, FK_Id_Salle = d.IdSalle, Heure_Deb = TimeSpan.Parse(d.HeureDebut), Heure_Fin = TimeSpan.Parse(d.HeureFin) }));
		tmpModel.SetSession(new T_Sessions() { Id_Ses = sessionFormation.Id, fk_id_fm = sessionFormation.IdFormation, fk_id_u_formateur = sessionFormation.IdFormateur, Max_Ins = sessionFormation.MaxInscrits, Min_Ins = sessionFormation.MinInscrits, Lib_Ses = "Session", Session_Dates = tmpDates, Statut_Ses = true, fk_Statut_validation_session = sessionFormation.StatutValidation });
		if (isFormationAllowsEmails && isEmailToSend) sendEmails();
		listeDates = null;
		sessionFormation = null;
		isEmailToSend = false;
	}
	void sendEmails()
	{
		var tmpUrl = HttpContext.Current.Request.Url.Authority + HttpRuntime.AppDomainAppVirtualPath;
		bool IsSucces = true;
		EmailManager.BuildEmails(listeInscrits.FindAll(i => i.Statut_Ins), tmpUrl, Constants.ConstEmailRelance, ref IsSucces, TBEmail.Text, listeDates);
		TBEmail.Text = "";
		PanelEmail.Visible = false;
		PanelModal.Visible = PanelEmail.Visible;
	}
	string checkData(WebControl sender)
	{
		string retour = "";
		if (DDLFormateurs.SelectedIndex == 0) retour += "Veuillez sélectionner un formateur.<br/>";
		if (!ToolBox.IsDate(TBDATE.Text)) retour += "La date est incorrecte.<br/>";
		if (ToolBox.IsDate(TBDATE.Text) && DateTime.Parse(TBDATE.Text) <= DateTime.Now) retour += "La date doit être postérieure à la date du jour.<br/>";
		if (DDLSalles.SelectedIndex == 0) retour += "Veuillez sélectionner une salle.<br/>";
		return retour;
	}
	void bindFormation(Formation pFormation)
	{
		if (pFormation != null)
		{
			LabPdf.Text = pFormation.File_fm;
			LabTheme.Text = pFormation.Theme.Lib_Theme;
			LabContact.Text = pFormation.ContactFormation;
			LabDuree.Text = pFormation.DureeFormation;
			LabAcces.Text = pFormation.Acces_Type.Lib_Acces;
			LabEnseigne.Text = pFormation.Enseignes;
			CBEmail.Checked = pFormation.eMail_Auto;
			HLPdf.Visible = !string.IsNullOrEmpty(LabPdf.Text);
			HLPdf.NavigateUrl = uploadPdfFolder + LabPdf.Text;
			TDSession.Style["display"] = "inline";
		}
		else
		{
			LabPdf.Text = "";
			LabTheme.Text = "";
			LabContact.Text = "";
			LabDuree.Text = "";
			LabAcces.Text = "";
			LabEnseigne.Text = "";
			CBEmail.Checked = false;
			HLPdf.Visible = false;
			TDSession.Style["display"] = "none";
		}
		listeDates = null;
	}
	void toggleReadOnlySession()
	{
		displayDates(!DivDates.Visible);
	}
	void displayDates(bool pVisibility)
	{
		DivBtnsValidation.Visible = pVisibility;
		DivBtnsSession.Visible = !pVisibility;
		DivDates.Visible = pVisibility;
		DDLFormateurs.Enabled = !pVisibility;
		TBMaxInscrits.Enabled = !pVisibility;
		TBMinInscrits.Enabled = !pVisibility;
		BtnEdit.Visible = pVisibility;
	}
	void bindSession() // Recupere la Session de la db et crée la session XML
	{
		sessionFormation = null;
		T_Sessions tmpSession = null;
		if (idSession > 0)
		{
			var tmpModel = new ModelManager();
			tmpSession = tmpModel.GetSessions(idSession, 0).FirstOrDefault();
			sessionFormation = new SessionXml() { Id = tmpSession.Id_Ses, IdFormateur = tmpSession.fk_id_u_formateur, IdFormation = tmpSession.fk_id_fm, MaxInscrits = tmpSession.Max_Ins, MinInscrits = tmpSession.Min_Ins, StatutValidation = tmpSession.fk_Statut_validation_session };
		}
		bindDates(tmpSession);
		bindContolsToSession();
		bindInscrits();
	}
	void bindContolsToSession()
	{
		if (sessionFormation == null)
		{
			TBDATE.Text = "";
			TBMaxInscrits.Text = "0";
			TBMinInscrits.Text = "0";
			DDLFormateurs.SelectedIndex = 0;
		}
		else
		{
			TBMaxInscrits.Text = sessionFormation.MaxInscrits.ToString();
			TBMinInscrits.Text = sessionFormation.MinInscrits.ToString();
			DDLFormateurs.SelectedValue = sessionFormation.IdFormateur.ToString();
			displayDates(true);
		}
	}
	void bindDates(T_Sessions pSession) // Recupere les Dates de la db et crée les Dates XML
	{
		if (pSession != null)
		{
			if (listeDates != null) listeDates.Clear();
			else listeDates = new List<DateSessionXml>();
			var tmpModel = new ModelManager();
			var tmpDates = new List<DateSessionXml>();
			pSession.Session_Dates.Where(d => d.FK_Session_ID == pSession.Id_Ses).ToList().ForEach(d => tmpDates.Add(new DateSessionXml(d.Date_Ses_Id, d.FK_Session_ID, d.Date_Session, d.Heure_Deb.ToString(), d.Heure_Fin.ToString(), d.FK_Id_Salle)));
			listeDates = tmpDates;
			GridView1.DataSource = sourceData;
			GridView1.DataBind();
		}
		if (isCreateMode) bindDate(null);
	}
	void bindInscrits()
	{
		if (idSession > 0)
		{
			GridView2.DataSource = listeInscrits;
			GridView2.DataBind();
			DivInscrits.Style["display"] = GridView2.Rows.Count > 0 ? "" : "none";
		}
	}
	void bindDate(DateSessionXml pDate)
	{
		TBDATE.Enabled = true;
		if (pDate == null)
		{
			TBDATE.Text = "";
			DDLHDEB.SelectedIndex = 0;
			DDLHFIN.SelectedIndex = 0;
			DDLMDEB.SelectedIndex = 0;
			DDLMFIN.SelectedIndex = 0;
			DDLSalles.SelectedIndex = 0;
			GridView1.SelectedIndex = -1;
			BtnAddDate.Text = "Ajouter";
		}
		else
		{
			TBDATE.Text = pDate.DateSession.ToShortDateString();
			DDLHDEB.SelectedValue = TimeSpan.Parse(pDate.HeureDebut).Hours.ToString("00");
			DDLHFIN.SelectedValue = TimeSpan.Parse(pDate.HeureFin).Hours.ToString("00");
			DDLMDEB.SelectedValue = TimeSpan.Parse(pDate.HeureDebut).Minutes.ToString("00");
			DDLMFIN.SelectedValue = TimeSpan.Parse(pDate.HeureFin).Minutes.ToString("00");
			DDLSalles.SelectedValue = pDate.IdSalle.ToString();
			BtnAddDate.Text = "Modifier";
			TBDATE.Enabled = pDate.DateSession > DateTime.Now;
		}
		DDLHDEB.Enabled = TBDATE.Enabled;
		DDLHFIN.Enabled = TBDATE.Enabled;
		DDLMDEB.Enabled = TBDATE.Enabled;
		DDLMFIN.Enabled = TBDATE.Enabled;
		DDLSalles.Enabled = TBDATE.Enabled;
	}
	void GridView_DataBound(object sender, EventArgs e)
	{
		var tmpSortExpression = (sender as GridView).ID == GridView1.ID ? sortExpression : sortExpressionGV2;
		if ((sender as GridView).BottomPagerRow != null)
		{
			Label tmpLabel = (Label)(sender as GridView).BottomPagerRow.FindControl("LabFolio");
			tmpLabel.Text = (sender as GridView).PageIndex + 1 + "/" + (sender as GridView).PageCount;
		}
		if ((sender as GridView).HeaderRow == null) return;
		foreach (TableCell cell in (sender as GridView).HeaderRow.Cells)
		{
			cell.CssClass = "";
			if (cell.Controls.Count > 0 && (cell.Controls[0] as LinkButton) != null)
			{
				cell.CssClass = ToolBox.HtmlDecode(cell.CssClass = (cell.Controls[0] as LinkButton).Text).Contains(tmpSortExpression) ? "ColumnSorted" : "";
			}
		}

	}
	void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandSource.GetType() == typeof(GridView)) return;
		var tmpGV = (sender as GridView);
		var tmpIndex = ((GridViewRow)((WebControl)e.CommandSource).NamingContainer).RowIndex;
		switch (e.CommandName)
		{
			case "Selecte":
				tmpGV.SelectedIndex = tmpIndex;
				bindDate(sourceData.Find(s => s.Id.ToString() == tmpGV.DataKeys[tmpIndex].Values["Id"].ToString()));
				break;
			case "Activate":
				if (isCreateMode)
				{
					var tmpDate = listeDates.Find(s => s.Id.ToString() == tmpGV.DataKeys[tmpIndex].Values["Id"].ToString());
					var tmpDates = listeDates;
					tmpDates.RemoveAll(s => s.Id == tmpDate.Id);
					listeDates = tmpDates;
					GridView1.DataSource = sourceData;
					GridView1.DataBind();
					bindDate(null);
				}
				else
				{
					//PanelEmail.Visible = true;
				}
				break;
		}
	}
	void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		var tmpGV = sender as GridView;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// var tmpObj = sourceData.Find(s => s.Id == Convert.ToInt32(tmpGV.DataKeys[e.Row.RowIndex].Values[strIdParam]));
			(e.Row.FindControl("BtnSelect") as ImageButton).ImageUrl = "Styles/Images/icon_select.png";
			if (isCreateMode) (e.Row.FindControl("BtnActivate") as ImageButton).ImageUrl = "Styles/Images/icon_deactivate.png";
			else (e.Row.FindControl("BtnActivate") as ImageButton).Visible = false;
		}
	}
	void GridView1_Sorting(object sender, GridViewSortEventArgs e)
	{
		if (e.SortExpression == sortExpression)
		{
			e.SortDirection = sortDirection == "Descending" ? SortDirection.Ascending : SortDirection.Descending;
		}
		else
		{
			e.SortDirection = SortDirection.Ascending;
		}
		sortExpression = e.SortExpression.ToString();
		sortDirection = e.SortDirection.ToString();
		GridView1.DataSource = sourceData;
		GridView1.DataBind();
		if (isCreateMode)
		{
			GridView1.SelectedIndex = -1;
			throw new NotImplementedException();
		}
		else
		{
			var tmpId = GridView1.DataKeys[GridView1.SelectedIndex].Values["Id"];
			var tmpDate = listeDates.Find(s => s.Id == (int)tmpId);
			bindDate(tmpDate);
		}
	}
	void GridView2_Sorting(object sender, GridViewSortEventArgs e)
	{
		if (e.SortExpression == sortExpressionGV2)
		{
			e.SortDirection = sortDirectionGV2 == "Descending" ? SortDirection.Ascending : SortDirection.Descending;
		}
		else
		{
			e.SortDirection = SortDirection.Ascending;
		}
		sortExpressionGV2 = e.SortExpression.ToString();
		sortDirectionGV2 = e.SortDirection.ToString();
		GridView2.DataSource = listeInscrits;
		GridView2.DataBind();
	}
	void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandSource.GetType() == typeof(GridView)) return;
		var tmpGV = (sender as GridView);
		var tmpIndex = ((GridViewRow)((WebControl)e.CommandSource).NamingContainer).RowIndex;
		switch (e.CommandName)
		{
			case "Activate":
				var tmpMessageEmail = isFormationAllowsEmails ? "<br />Un email de désinscription partira suite à votre action pour l’informer." : "";
				var tmpMessageDelete = "Attention ! <br/>Vous allez désinscrire le collaborateur de la session." + tmpMessageEmail + "<br /> Confirmez-vous ?";
				Action<WebControl> tmpAction = new Action<WebControl>((o) =>
				{
					var tmpGV1 = (o.Page.FindControl("GridView2") as GridView);
					var tmpModel = new ModelManager();
					//var tmpInscrit = tmpModel.GetInscrits().Find(i2 => i2.Id_Ins == Convert.ToInt32(tmpGV1.DataKeys[tmpIndex].Values["Id_Ins"]));
					var tmpInscrit = tmpModel.GetInscrit(Convert.ToInt32(tmpGV1.DataKeys[tmpIndex].Values["Id_Ins"]));
					tmpModel.DeActivateInscrit(Convert.ToInt32(tmpGV1.DataKeys[tmpIndex].Values["Id_Ins"]));
					tmpGV1.DataSource = listeInscrits;
					tmpGV1.DataBind();
					if (isFormationAllowsEmails)
					{
						// ENVOI EMAIL DE DESINSCRIPTION //                                        
						//var tmpSession = tmpModel.GetSessions(null).Find(s => s.Id_Ses == idSession);
						var tmpSession = tmpModel.GetSessions(idSession, 0);
						if (tmpSession != null && tmpInscrit != null)
						{
							bool IsSucces = true;
							var tmpUrl = HttpContext.Current.Request.Url.Authority + HttpRuntime.AppDomainAppVirtualPath;
							EmailManager.BuildEmails(new List<Inscrit>() { tmpInscrit }, tmpUrl, Constants.ConstEmailDesinscription, ref IsSucces);
						}
						// ENVOI EMAIL DE DESINSCRIPTION // END                 
					}
				});
				UC_ErrorPanel1.DisplayConfirmAction(tmpMessageDelete, tmpAction, null);
				break;
		}
	}
	void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		var tmpGV = sender as GridView;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			(e.Row.FindControl("BtnActivate") as ImageButton).ImageUrl = "Styles/Images/icon_deactivate.png";
		}

	}
	public void PagerIndexChange(object sender, EventArgs e)
	{
		var tmpGV = (sender as WebControl).NamingContainer.NamingContainer as GridView;
		if (isCreateMode) tmpGV.SelectedIndex = -1;
		else tmpGV.SelectedIndex = 0;

		Control tmpControl = (Control)sender;
		if (tmpControl.ID == "BtnLeft")
		{
			tmpGV.PageIndex -= tmpGV.PageIndex == 0 ? 0 : 1;
		}
		if (tmpControl.ID == "BtnFullLeft")
		{
			tmpGV.PageIndex = 0;
		}
		if (tmpControl.ID == "BtnRight")
		{
			tmpGV.PageIndex += tmpGV.PageIndex == tmpGV.PageCount - 1 ? 0 : 1;
		}
		if (tmpControl.ID == "BtnFullRight")
		{
			tmpGV.PageIndex = tmpGV.PageCount - 1;
		}
		if (tmpGV == GridView1)
		{
			tmpGV.DataSource = sourceData;
			tmpGV.DataBind();
			if (tmpGV.SelectedIndex >= 0) bindDate(listeDates.Find(d => d.Id.ToString() == GridView1.SelectedValue.ToString()));
		}
		else
		{
			tmpGV.SelectedIndex = -1;
			tmpGV.DataSource = listeInscrits;
			tmpGV.DataBind();
		}
	}
	#region nested classes
	[Serializable]
	public class SessionXml
	{
		public SessionXml() { }
		public SessionXml(int pId, int pIdFormation, int pIdFormateur, int pMinInscrits, int pMaxInscrits, int pStatutvalidation)
		{
			Id = pId;
			IdFormation = pIdFormation;
			IdFormateur = pIdFormateur;
			MaxInscrits = pMaxInscrits;
			MinInscrits = pMinInscrits;
			StatutValidation = pStatutvalidation;
		}
		public int Id { get; set; }
		public int StatutValidation { get; set; }
		public int IdFormation { get; set; }
		public int IdFormateur { get; set; }
		public int MaxInscrits { get; set; }
		public int MinInscrits { get; set; }
	}
	#endregion
}