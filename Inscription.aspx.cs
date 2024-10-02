using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Xml.Serialization;
using System.Threading.Tasks;
using LDAPNS;

public partial class Inscription : System.Web.UI.Page
{
	/// <summary>
	/// POUR MEMOIRE LA TABLE DES USERS A 2 TYPES DE DOUBLON 
	/// QUELQUES USERS DISPOSANT D'EMAIL => anomalie ou disposant de plusieurs types d'user ?
	/// PRINCIPALEMENT LES USERS NON PRESENTS DANS L'AD ET SANS EMAIL QUI NE PEUVENT ETRE IDENTIFIES ET FONT L'OBJET D'UN AJOUT A CHAQUE INSCRIPTION => MODIF DE SCHEMA
	/// 
	/// </summary>

	private string strSortExpression = "Session"
	private string strSortExpressionGV23 = "Nom"
	private string uploadPdfFolder = "Upload/Pdf/Formations/";
	private int typeUser = 3;
	ILogger _log = LogHelper.Logger;
	private int maxInscritId // id user utilisée pour déterminer dans la liste des stagiares ceux qui sont nouvellement inscrits et qui n'existent pas encore en bdd.
	{
		get
		{
			return ViewState["maxInscritId"] != null ? int.Parse(ViewState["maxInscritId"].ToString()) : 0;
		}
		set
		{
			ViewState["maxInscritId"] = value;
		}
	}
	 private long test = typeUser
	private List<T_Sessions> sourceData
	{
		get
		{
			List<T_Sessions> retour = null;
			var tmpModel = new ModelManager();
			if (idSession > 0) retour = tmpModel.GetSessions(idSession, 0, true);
			if (idSession > 0) retour = tmpModel.GetSessions(idSession, 0, true);
			if (idFormation > 0) retour = tmpModel.GetSessions(0, idFormation, true);
			if (typeInscription != EnumInscription.ADMININSCRIPTIONS) retour.RemoveAll(s => s.FirstDate == null || (s.FirstDate != null && s.FirstDate.Date_Session <= DateTime.Now);
			if (sortDirection == SortDirection.Ascending.ToString())
			{
				switch (sortExpression)
				{
					case "Statut":
						retour.Sort((s1, s2) => string.Compare(s1.StatutValidation, s2.StatutValidation));
						break;
				}
			}
			else
			{
				switch (sortExpression)
				{
					case "Statut":
						retour.Sort((s2, s1) => string.Compare(s1.StatutValidation, s2.StatutValidation));
						break;
				}
			}
			retour.Sort((a1, a2) => DateTime.Compare(a1.FirstDate.Date_Session, a2.FirstDate.Date_Session));
			return retour;
		}
	}
	public List<Departement> Departements
	{
		get
		{
			return (List<Departement>)Session["Departements"];
		}
		set
		{
			Session["Departements"] = value;
		}
	}
	string sortDirection
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
	string sortExpression
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
	string sortDirectionGV23
	{
		get
		{
			if (ViewState["sortDirectionGV23"] == null)
			{
				ViewState["sortDirectionGV23"] = "Ascending";
			}
			return ViewState["sortDirectionGV23"].ToString();
		}
		set
		{
			ViewState["sortDirectionGV23"] = value;
		}
	}
	string sortExpressionGV23
	{
		get
		{
			if (ViewState["sortExpressionGV23"] == null)
			{
				ViewState["sortExpressionGV23"] = strSortExpressionGV23;
			}
			return ViewState["sortExpressionGV23"].ToString();
		}
		set
		{
			ViewState["sortExpressionGV23"] = value;
		}
	}
	private int idFormation
	{
		get
		{
			if (ViewState["idFormation"] != null) return Convert.ToInt32(ViewState["idFormation"].ToString());
			else return 0;
		}
		set
		{
			ViewState["idFormation"] = value;
		}
	}
	private int idSession
	{
		get
		{
			if (ViewState["idSession"] != null) return Convert.ToInt32(ViewState["idSession"].ToString());
			else return 0;
		}
		set
		{
			ViewState["idSession"] = value;
		}
	}
	private bool IsForOthers
	{
		get
		{
			if (ViewState["IsForOthers"] != null) return Convert.ToBoolean(ViewState["IsForOthers"].ToString());
			else return false;
		}
		set
		{
			ViewState["IsForOthers"] = value;
		}
	}
	private List<StagiaireXml> listeStagiaires
	{
		get
		{
			List<StagiaireXml> retour = new List<StagiaireXml>();
			if (ViewState["listeStagiaires"] != null)
			{
				retour = (List<StagiaireXml>)ToolBox.DeserializeXmlViewState(ViewState["listeStagiaires"].ToString(), typeof(List<StagiaireXml>));
				if (sortDirectionGV23 == SortDirection.Ascending.ToString())
				{
					switch (sortExpressionGV23)
					{
						case "Session":
							retour.Sort((s1, s2) =>
							{
								var index1 = int.Parse(getSessionIndex(s1.idSession.ToString()).Replace("S", ""));
								var index2 = int.Parse(getSessionIndex(s2.idSession.ToString()).Replace("S", ""));
								return index1 - index2;
							});
							break;
						case "Nom":
							retour.Sort((s1, s2) => string.Compare(s1.NomComplet.ToString(), s2.NomComplet.ToString()));
							break;
						case "Dpt":
							retour.Sort((s1, s2) => string.Compare(s1.Departement.ToString(), s2.Departement.ToString()));
							break;
					}
				}
				else
				{
					switch (sortExpressionGV23)
					{
						case "Session":
							retour.Sort((s1, s2) =>
							{
								var index2 = int.Parse(getSessionIndex(s1.idSession.ToString()).Replace("S", ""));
								var index1 = int.Parse(getSessionIndex(s2.idSession.ToString()).Replace("S", ""));
								return index1 - index2;
							});
							break;
						case "Nom":
							retour.Sort((s2, s1) => string.Compare(s1.NomComplet.ToString(), s2.NomComplet.ToString()));
							break;
						case "Dpt":
							retour.Sort((s2, s1) => string.Compare(s1.Departement.ToString(), s2.Departement.ToString()));
							break;
					}
				}
				return retour;
			}
			return retour;
		}
		set
		{
			ViewState["listeStagiaires"] = ToolBox.SerializeXmlViewState(value);
		}
	}
	private List<StagiaireXml> listeStagiairesToSee // Liste mise en place pour les inscriptions par managers qui ne doivent pas voir les stagiaires inscrits par d'autres
	{
		get
		{
			List<StagiaireXml> retour = null;
			switch (typeInscription)
			{
				case EnumInscription.COLLABOINSCRIPTION:
					retour = listeStagiaires.FindAll(s => s.Id > maxInscritId);
					break;
				default:
					retour = listeStagiaires;
					break;
			}
			return retour;
		}
	}
	private enum EnumInscription { AUTOINSCRIPTION, ADMININSCRIPTIONS, COLLABOINSCRIPTION }
	private EnumInscription typeInscription
	{
		get
		{
			EnumInscription retour;
			switch (ViewState["typeInscription"].ToString())
			{
				case "ADMININSCRIPTIONS":
					retour = EnumInscription.ADMININSCRIPTIONS;
					break;
				case "AUTOINSCRIPTION":
					retour = EnumInscription.AUTOINSCRIPTION;
					break;
				case "COLLABOINSCRIPTION":
					retour = EnumInscription.COLLABOINSCRIPTION;
					break;
				default:
					retour = EnumInscription.COLLABOINSCRIPTION;
					break;
			}
			return retour;
		}
		set
		{
			ViewState["typeInscription"] = value.ToString();
		}
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			GridView1.DataBound += GridView1_DataBound;
			GridView1.SelectedIndexChanged += GridView1_SelectedIndexChanged;
			GridView1.SelectedIndexChanging += GridView1_SelectedIndexChanging;
			GridView2.RowDataBound += GridView23_RowDataBound;
			GridView2.DataBound += GridView23_DataBound;
			GridView2.RowCommand += GridView23_RowCommand;
			GridView2.Sorting += GridView23_Sorting;

			GridView3.RowDataBound += GridView23_RowDataBound;
			GridView3.DataBound += GridView23_DataBound;
			GridView3.RowCommand += GridView23_RowCommand;
			GridView3.Sorting += GridView23_Sorting;

			BtnCancelStagiaire.Click += (o, evt) => clear();
			BtnAddStagiaire.Click += BtnAddStagiaire_Click;
			BtnInscrire.Click += BtnInscrire_Click;
			BtnCancel.Click += BtnCancel_Click;
			TBSearch.TextChanged += (o, evt) => { if (!string.IsNullOrEmpty(TBSearch.Text)) { TBNom.Enabled = false; TBPrenom.Enabled = false; } else { TBNom.Enabled = true; TBPrenom.Enabled = true; } };
			TBNom.TextChanged += (o, evt) => { if (!string.IsNullOrEmpty(TBNom.Text)) { TBNom.Text = TBNom.Text.Trim(); TBSearch.Enabled = false; ScriptManager1.SetFocus(TBPrenom); } };
			TBPrenom.TextChanged += (o, evt) => { if (!string.IsNullOrEmpty(TBPrenom.Text)) { TBPrenom.Text = TBPrenom.Text.Trim(); ScriptManager1.SetFocus(TBResponsable); } };
			TBResponsable.TextChanged += (o, evt) => { ScriptManager1.SetFocus(TBResponsable); }; // Implémenté pour le blocage IE 11 sur sélection départements + Tag Meta sur IE10 (sinon recentrage de la page)
			if (!Page.IsPostBack)
			{
				if (Request["idSession"] != null) idSession = Convert.ToInt32(Request["idSession"].ToString());
				if (Request["idFormation"] != null) idFormation = Convert.ToInt32(Request["idFormation"].ToString());
				if (Request["IsForOthers"] != null) IsForOthers = Convert.ToBoolean(Request["IsForOthers"].ToString());
				if (idFormation == 0 && idSession == 0) return;

				var tmpModel = new ModelManager();
				Departements = (from d in tmpModel.GetDepartements() orderby d.Lib_Dep select d).ToList();
				DDLDepartements.DataSource = Departements.FindAll(f => f.Statut_dep);
				DDLDepartements.DataValueField = "Id_Dep";
				DDLDepartements.DataTextField = "Lib_Dep";
				DDLDepartements.DataBind();
				DDLDepartements.Items.Insert(0, new ListItem("Sélectionner un département", "", true));
				DDLDepartements.SelectedIndex = 0;
				if (idSession > 0)
				{
					typeInscription = EnumInscription.ADMININSCRIPTIONS;
					LabTitlepage.Text = "Inscrire des collaborateurs";
				}
				else if (IsForOthers)
				{
					typeInscription = EnumInscription.COLLABOINSCRIPTION;
					LabTitlepage.Text = "Inscrire mes collaborateurs";
					Image1.ImageUrl = "styles/images/mescollaborateurs.png";
					var tmpUser = Ldap.GetUser(Page.User.Identity.Name);
					TBResponsable.Text = tmpUser.NomComplet;
					TBResponsable.Enabled = false;
				}
				else
				{
					typeInscription = EnumInscription.AUTOINSCRIPTION;
					LabTitlepage.Text = "S'inscrire";
					Image1.ImageUrl = "styles/images/sinscrire.png";
					DivAdmin0.Visible = false;
					DivAdmin1.Visible = false;
					DivAdmin2.Visible = false;
					DivUser.Visible = true;
					var tmpUser = Ldap.GetUser(Page.User.Identity.Name);
					LABNOM.Text = tmpUser.Nom;
					LABRENOM.Text = tmpUser.Prenom;
					var tmpManager = tmpUser.GetManager();
					TBResponsable.Text = tmpManager != null ? tmpManager.NomComplet : "";
					DivBtnsValidation.Visible = true;
					DivBtnsValidation.Style["margin-top"] = "200px";
					BtnInscrire.Text = "S'inscrire";
				}
				bindFormation();
				GridView1.DataSource = sourceData;
				GridView1.DataBind();

				DivGenerique.Visible = typeInscription != EnumInscription.COLLABOINSCRIPTION;
				GridView2.Visible = DivGenerique.Visible;
				DivMesCollabos.Visible = !DivGenerique.Visible;
				GridView3.Visible = DivMesCollabos.Visible;
				bindInscrits();
			}
		}
		catch (Exception ex)
		{ _log.Error("Erreur dans Inscription Page_Load", ex); }
	}
	void BtnCancel_Click(object sender, EventArgs e)
	{
		switch (typeInscription)
		{
			case EnumInscription.ADMININSCRIPTIONS:
				Response.Redirect("ListeSessions.aspx");
				break;
			case EnumInscription.AUTOINSCRIPTION:
				Response.Redirect("SyntheseFormations.aspx");
				break;
			case EnumInscription.COLLABOINSCRIPTION:
				Response.Redirect("SyntheseFormations.aspx");
				break;
		}
	}
	void BtnInscrire_Click(object sender, EventArgs e)
	{
		_log.Info("***************Inscription.aspx, Début BtnInscrire_Click***************");
		try
		{
			var tmpModel = new ModelManager();
			bindInscrits();
			if (typeInscription == EnumInscription.AUTOINSCRIPTION) // AUTOINSCRIPTION
			{
				_log.Debug("typeInscription=AUTOINSCRIPTION");
				// Ne pas inscrire si max inscrits atteints sauf admin rh
				if (listeStagiaires.FindAll(s => s.idSession == idSession).Count > 0)
				{
					var tmpCurrentSession = tmpModel.GetSessions(idSession,0).FirstOrDefault();
					if (listeStagiaires.FindAll(s => s.idSession == idSession).Count + 1 > tmpCurrentSession.Max_Ins)
					{
						UC_ErrorPanel1.DisplayError("Impossible d'inscrire plus de stagiaires que le maximum de la session");
						return;
					}
				}
				// Ne pas inscrire si max inscrits atteints sauf admin rh END
				var tmpListe = listeStagiaires;
				Ldap.LDAPUSER tmpLdapResponsable = Ldap.GetUserByNomComplet(TBResponsable.Text);
				if (string.IsNullOrEmpty(TBResponsable.Text) || tmpLdapResponsable == null)
				{
					UC_ErrorPanel1.DisplayError("Un nom de responsable présent dans l'annuaire est requis.");
					return;
				}
				StagiaireXml tmpStagiaire = null;
				Ldap.LDAPUSER tmpLdapUser = Ldap.GetUser(Page.User.Identity.Name);
				_log.Debug(string.Format("tmpLdapUser.SAMAccountName={0},tmpLdapUser.Email={1},tmpLdapUser.Nom={2}", tmpLdapUser.SAMAccountName, tmpLdapUser.Email, tmpLdapUser.Nom));
				if (tmpLdapUser == null)
				{
					_log.Info("Utilisateur LDAP = null");
					return;
				}
				User tmpUser = tmpModel.GetUser(tmpLdapUser.SAMAccountName);
				#region ENRICHISSEMENT DE LA TABLE USERS MEME SI INSCRIPTION NON VALIDEE 
				// ENRICHISSEMENT DE LA TABLE USERS MEME SI INSCRIPTION NON VALIDEE //       
				_log.Info("Enrichissement de la table Users");
				if (tmpUser == null)
				{
					_log.Debug("tmpUser=null");
					if (tmpLdapUser != null)
					{
						tmpUser = new User()
						{
							Nom_U = tmpLdapUser.Nom,
							Prenom_U = tmpLdapUser.Prenom,
							SAMAccountName = tmpLdapUser.SAMAccountName,
							Nom_Responsable = TBResponsable.Text,
							eMail_u = tmpLdapUser.Email,
							Tel = tmpLdapUser.Telephone
						};
					}
				}
				tmpUser.Nom_Responsable = TBResponsable.Text;
				if (tmpLdapUser != null)
				{
					tmpUser.eMail_u = tmpLdapUser.Email;
					tmpUser.Tel = tmpLdapUser.Telephone;
				};
				var errorMessage = tmpModel.SetUser(tmpUser, typeUser);
				if (!string.IsNullOrWhiteSpace(errorMessage))
				{
					UC_ErrorPanel1.DisplayError(errorMessage);
					return;
				}
				#region Inscription dans la table USERS du responsable
				// Inscription dans la table USERS du responsable //
				_log.Info("Inscription dans la table USERS du responsable");
				User tmpResponsable = tmpModel.GetUser(tmpLdapResponsable.SAMAccountName);
				if (tmpResponsable == null)
				{
					tmpResponsable = new User()
					{
						Nom_U = tmpLdapResponsable.Nom,
						Prenom_U = tmpLdapResponsable.Prenom,
						SAMAccountName = tmpLdapResponsable.SAMAccountName,
						eMail_u = tmpLdapResponsable.Email,
						Tel = tmpLdapResponsable.Telephone
					};
				}
				else
				{
					tmpResponsable.eMail_u = tmpLdapResponsable.Email;
					tmpResponsable.Tel = tmpLdapResponsable.Telephone;
				}
				errorMessage = tmpModel.SetUser(tmpResponsable, typeUser);
				if (!string.IsNullOrWhiteSpace(errorMessage))
				{
					UC_ErrorPanel1.DisplayError(errorMessage);
					return;
				}
				// Inscription dans la table USERS du responsable //END
				#endregion
				#endregion
				// ENRICHISSEMENT DE LA TABLE USERS MEME SI INSCRIPTION NON VALIDEE //END
				#region Ajout du sttagiaire dans la liste des stagiaires si non déjà inscrit
				// Ajout du sttagiaire dans la liste des stagiaires si non déjà inscrit//
				_log.Info("Ajout du stagiaire dans la liste des stagiaires si non deja inscrit");
				if (tmpModel.GetUsersInscrits(tmpUser.Id_U).Exists(i => i.T_Sessions.Id_Ses == idSession && i.Statut_Ins))
				{
					UC_ErrorPanel1.DisplayError("Impossible d'inscrire un utilisateur déjà inscrit.");
					return;
				}
				var tmpMaxId = listeStagiaires.Count > 0 ? listeStagiaires.Max(s => s.Id) + 1 : 1;
				int? tmpDept = DDLDepartements.SelectedIndex > 0 ? (int?)int.Parse(DDLDepartements.SelectedValue) : null;
				tmpStagiaire = new StagiaireXml(tmpMaxId, Convert.ToInt32(GridView1.SelectedValue), TBResponsable.Text, tmpDept, (tmpDept != null ? Departements.Find(d => d.Id_Dep == tmpDept).Lib_Dep : null), DateTime.Now);
				if (tmpUser != null)
				{
					tmpStagiaire.IdUser = tmpUser.Id_U;
					tmpStagiaire.Nom = tmpUser.Nom_U;
					tmpStagiaire.Prenom = tmpUser.Prenom_U;
				}
				tmpStagiaire.idSession = idSession;
				_log.Debug(string.Format("tmpStagiaire.IdUser={0},tmpStagiaire.Nom={1},tmpStagiaire.Prenom={2},tmpStagiaire.idSession={3}", tmpStagiaire.IdUser, tmpStagiaire.Nom, tmpStagiaire.Prenom, tmpStagiaire.idSession));
				tmpListe.Add(tmpStagiaire);
				listeStagiaires.Add(tmpStagiaire);
				listeStagiaires = tmpListe;
				// Ajout du sttagiaire dans la liste des stagiaires //END
				#endregion
			}
			if (!listeStagiairesToSee.Any())
			{
				UC_ErrorPanel1.DisplayError("Il faut au moins un stagiaire pour procéder à l'inscription.");
				return;
			}
			if (!listeStagiaires.Any())
			{
				UC_ErrorPanel1.DisplayError("Il faut au moins un stagiaire pour procéder à l'inscription.");
				return;
			}
			List<Inscrit> tmpInscrits = new List<Inscrit>();
			listeStagiaires.ForEach(s => tmpInscrits.Add(new Inscrit()
			{
				Date_Inscription = s.DateInscription,
				fk_id_dep = s.IdDepartement,
				fk_id_ses = s.idSession,
				Id_Ins = s.Id,
				Statut_Ins = true,
				fk_id_stagiaire = s.IdUser
			}));
			sourceData.ForEach(s => tmpModel.SetInscrits(tmpInscrits.FindAll(i => i.fk_id_ses == s.Id_Ses), s.Id_Ses));
			GridView1.DataSource = sourceData;
			GridView1.DataBind();

			#region ENVOI DES EMAILS
			// ENVOI DES EMAILS //
			_log.Debug("Début Envoie Emails");
			var tmpSession = new ModelManager().GetSessions(idSession, 0).FirstOrDefault();
			var tmpUrlEntete = HttpContext.Current.Request.Url.Authority + HttpRuntime.AppDomainAppVirtualPath;
			List<Inscrit> tmpInscritsToSendMail = new List<Inscrit>();
			if (typeInscription == EnumInscription.AUTOINSCRIPTION) tmpInscritsToSendMail.Add(tmpInscrits.Find(i => i.Id_Ins == tmpInscrits.Max(i2 => i2.Id_Ins)));
			else tmpInscritsToSendMail.AddRange(tmpInscrits.FindAll(i => i.Statut_Ins));
			var typeEmail = tmpSession.StatutValidation == Constants.SessionValidee ? Constants.ConstEmailInvitation : Constants.ConstEmailInscription;
			var tmpInscritTest = tmpInscritsToSendMail.Any() ? tmpInscritsToSendMail[0].NomComplet : "";
			bool tmpIsSucces = true;
			EmailManager.BuildEmails(tmpInscritsToSendMail, tmpUrlEntete, typeEmail, ref tmpIsSucces);
			if (!tmpIsSucces)
			{
				UC_ErrorPanel1.DisplayError("Une erreur s'est produite, veuillez réessayer");
				return;
			}
			_log.Debug("Fin Envoie Emails");
			// ENVOI DES EMAILS //END
			#endregion
		}
		catch (Exception err)
		{
			_log.Error("Erreur dans Inscription, BtnInscrire_Click : ", err);
		}
		switch (typeInscription)
		{
			case EnumInscription.ADMININSCRIPTIONS:
				_log.Debug("Redirection vers ListeSessions.aspx");
				_log.Info("***************Inscription, Fin BtnInscrire_Click***************");
				Response.Redirect("ListeSessions.aspx");
				break;
			case EnumInscription.AUTOINSCRIPTION:
				_log.Debug("Redirection vers SyntheseFormations.aspx");
				Response.Redirect("SyntheseFormations.aspx");
				_log.Info("***************Inscription, Fin BtnInscrire_Click***************");
				break;
			case EnumInscription.COLLABOINSCRIPTION:
				_log.Debug("Redirection vers SyntheseFormations.aspx");
				Response.Redirect("SyntheseFormations.aspx");
				_log.Info("***************Inscription, Fin BtnInscrire_Click***************");
				break;
		}
	}
	void BtnAddStagiaire_Click(object sender, EventArgs e)
	{
		_log.Info("***************Inscription.aspx, Début Ajout stagiaire***************");
		try
		{
			if (string.IsNullOrEmpty(TBSearch.Text) && string.IsNullOrEmpty(TBNom.Text)) return;

			Ldap.LDAPUSER tmpLdapResponsable = Ldap.GetUserByNomComplet(TBResponsable.Text);
			if (string.IsNullOrEmpty(TBResponsable.Text) || tmpLdapResponsable == null)
			{
				UC_ErrorPanel1.DisplayError("Un nom de responsable présent dans l'annuaire est requis.");
				return;
			}
			var tmpModel = new ModelManager();
			// Controle maximum d'inscrits //
			if (typeInscription != EnumInscription.AUTOINSCRIPTION && typeInscription != EnumInscription.ADMININSCRIPTIONS)
			{
				var tmpCurrentSession = tmpModel.GetSessions(idSession,0).FirstOrDefault();
				if (tmpCurrentSession == null) _log.Info("tmpCurrentSession = null");
				if (listeStagiaires.FindAll(i => i.idSession == idSession).Count + 1 > tmpCurrentSession.Max_Ins)
				{
					UC_ErrorPanel1.DisplayError("Impossible d'inscrire plus de stagiaires que le maximum de la session");
					return;
				}
			}
			// Controle maximum d'inscrits //END
			StagiaireXml tmpStagiaire = null;
			User tmpUser = null;
			Ldap.LDAPUSER tmpLdapUser = null;
			string tmpAccountName = "";
			string tmpSearchName = "";

			if (!string.IsNullOrEmpty(TBSearch.Text)) // VIENT DE L'AD //
			{
				// Traietement spécifique doublons FOFANA Mamou AD
				if (TBSearch.Text.Contains(" - LBM"))
				{
					tmpAccountName = "mafofana";
				}
				else if (TBSearch.Text.Contains(" - GEP"))
				{
					tmpAccountName = "mfofana";
				}

				List<string> badList = new List<string> { " - LBM", " - GEP" };
                foreach (string s in badList)
                {
					tmpSearchName = TBSearch.Text.Replace(s, string.Empty);
                }
				_log.Debug(string.Format("TBSearch.Text={0}", TBSearch.Text));

				tmpLdapUser = !string.IsNullOrEmpty(tmpAccountName) ? Ldap.GetUser(tmpAccountName) : Ldap.GetUserByNomComplet(tmpSearchName);

				if (tmpLdapUser == null) return;
				_log.Debug(string.Format("tmpLdapUser.SAMAccountName={0}", tmpLdapUser.SAMAccountName));

				tmpUser = tmpModel.GetUser(tmpLdapUser.SAMAccountName);

				if (tmpUser != null) _log.Debug(string.Format("tmpUser.eMail_u={0},tmpUser.Id_U={1},tmpUser.Nom_U={2}", tmpUser.eMail_u, tmpUser.Id_U, tmpUser.Nom_U));
			}
			else if (!string.IsNullOrEmpty(TBNom.Text) && !string.IsNullOrEmpty(TBPrenom.Text))
			{
				_log.Debug(string.Format("TBNom.Text={0},TBPrenom.Text={1}", TBNom.Text, TBPrenom.Text));
				//tmpUser = tmpModel.GetUser(TBNom.Text, TBPrenom.Text);
				//if (tmpUser != null) _log.Debug(string.Format("tmpUser.eMail_u={0},tmpUser.Id_U={1},tmpUser.Nom_U={2}", tmpUser.eMail_u, tmpUser.Id_U, tmpUser.Nom_U));
			}
			else
			{
				UC_ErrorPanel1.DisplayError("Informations insuffisantes");
				return;
			}
			// ENRICHISSEMENT DE LA TABLE USERS MEME SI INSCRIPTION NON VALIDEE //
			_log.Info("Enrichissement table Users");
			if (tmpUser == null)
			{
				_log.Debug("tmpUser=null");
				if (tmpLdapUser != null)
				{
					_log.Debug("tmpLdapUser != null");
					tmpUser = new User() { Nom_U = tmpLdapUser.Nom, Prenom_U = tmpLdapUser.Prenom, SAMAccountName = tmpLdapUser.SAMAccountName, Nom_Responsable = TBResponsable.Text, eMail_u = tmpLdapUser.Email, Tel = tmpLdapUser.Telephone };
					_log.Debug(string.Format("tmpLdapUser != null, tmpUser.Nom_U={0},tmpUser.SAMAccountName={1},tmpUser.Nom_Responsable={2},tmpUser.eMail_u={3},tmpUser.Tel={4}", tmpUser.Nom_U, tmpUser.SAMAccountName, tmpUser.Nom_Responsable, tmpUser.eMail_u, tmpUser.Tel));
				}
				else
				{
					tmpUser = new User() { Nom_U = TBNom.Text, Prenom_U = TBPrenom.Text, Nom_Responsable = TBResponsable.Text };
					_log.Debug(string.Format("tmpLdapUser = null, tmpUser.Nom_U={0},tmpUser.SAMAccountName={1},tmpUser.Nom_Responsable={2},tmpUser.eMail_u={3},tmpUser.Tel={4}", tmpUser.Nom_U, tmpUser.SAMAccountName, tmpUser.Nom_Responsable, tmpUser.eMail_u, tmpUser.Tel));
				}
			}
			tmpUser.Nom_Responsable = TBResponsable.Text;
		    string errorMessage = "";
			if (tmpLdapUser != null)
			{
				tmpUser.eMail_u = tmpLdapUser.Email;
				tmpUser.Tel = tmpLdapUser.Telephone;
				errorMessage = tmpModel.SetUser(tmpUser, typeUser);
			} 
			else
			{
				errorMessage = tmpModel.SetUser(tmpUser, typeUser, true);
			}
			if (!string.IsNullOrWhiteSpace(errorMessage))
			{
				UC_ErrorPanel1.DisplayError(errorMessage);
				return;
			}
			// Inscription dans la table USERS du responsable //
			_log.Info("Enrichissement table Users du responsable");
			User tmpResponsable = tmpModel.GetUser(tmpLdapResponsable.SAMAccountName);
			if (tmpResponsable == null)
			{
				_log.Debug("tmpResponsable = null");
				tmpResponsable = new User() { Nom_U = tmpLdapResponsable.Nom, Prenom_U = tmpLdapResponsable.Prenom, SAMAccountName = tmpLdapResponsable.SAMAccountName, eMail_u = tmpLdapResponsable.Email, Tel = tmpLdapResponsable.Telephone };
			}
			else
			{
				_log.Debug("tmpResponsable != null");
				tmpResponsable.eMail_u = tmpLdapResponsable.Email;
				tmpResponsable.Tel = tmpLdapResponsable.Telephone;
			}
			errorMessage = tmpModel.SetUser(tmpResponsable, typeUser);
			if (!string.IsNullOrWhiteSpace(errorMessage))
			{
				UC_ErrorPanel1.DisplayError(errorMessage);
				return;
			}
			// Inscription dans la table USERS du responsable //END

			// ENRICHISSEMENT DE LA TABLE USERS MEME SI INSCRIPTION NON VALIDEE //END
			// Ajout du sttagiaire dans la liste des stagiaires  si non déjà inscrit //
			_log.Info("Ajout du stagiaire dans la liste des stagiaires si non déjà inscrit");
			if (tmpModel.GetUsersInscrits(tmpUser.Id_U).Exists(i => i.T_Sessions.Id_Ses == idSession && i.Statut_Ins))
			{
				UC_ErrorPanel1.DisplayError("Impossible d'inscrire un utilisateur déjà inscrit.");
				return;
			}
			var tmpMaxId = listeStagiaires.Count > 0 ? listeStagiaires.Max(s => s.Id) + 1 : 1;
			int? tmpDept = DDLDepartements.SelectedIndex > 0 ? (int?)Convert.ToInt32(DDLDepartements.SelectedValue) : null;
			tmpStagiaire = new StagiaireXml(tmpMaxId, Convert.ToInt32(GridView1.SelectedValue), TBResponsable.Text, tmpDept, (tmpDept != null ? Departements.Find(d => d.Id_Dep == tmpDept).Lib_Dep : null), DateTime.Now);
			if (tmpUser != null)
			{
				tmpStagiaire.IdUser = tmpUser.Id_U;
				tmpStagiaire.Nom = tmpUser.Nom_U;
				tmpStagiaire.Prenom = tmpUser.Prenom_U;
			}
			if (tmpUser == null)
			{
				tmpStagiaire.Nom = TBNom.Text;
				tmpStagiaire.Prenom = TBPrenom.Text;
			}
			tmpStagiaire.idSession = idSession;
			_log.Debug(string.Format("tmpStagiaire.idSession={0},tmpStagiaire.Id={1}, tmpStagiaire.IdUser={2}, tmpStagiaire.Nom={3}", tmpStagiaire.idSession, tmpStagiaire.Id, tmpStagiaire.IdUser, tmpStagiaire.Nom));
			if (!listeStagiaires.FindAll(s => s.idSession == idSession).Exists(s => s.IdUser > 0 && s.IdUser == tmpStagiaire.IdUser))
			{
				var tmpListe = listeStagiaires;
				tmpListe.Add(tmpStagiaire);
				listeStagiaires = tmpListe;
				var tmpGV = typeInscription != EnumInscription.COLLABOINSCRIPTION ? GridView2 : GridView3;
				tmpGV.DataSource = listeStagiairesToSee;
				tmpGV.DataBind();
			}
			else
			{
				UC_ErrorPanel1.DisplayError("Impossible d'ajouter un stagiaire déjà inscrit.");
			}
			// Ajout du sttagiaire dans la liste des stagiaires //END

			resetUi();
			DivSession.Visible = typeInscription != EnumInscription.COLLABOINSCRIPTION;
		}
		catch(Exception ex)
		{
			_log.Error("Erreur dans Inscription BtnAddStagiaire_Click", ex);
		}
		_log.Info("***************Inscription.aspx, Fin Ajout stagiaire***************");

	}
	public void PagerIndexChange(object sender, EventArgs e)
	{
		var tmpGV = (sender as WebControl).NamingContainer.NamingContainer as GridView;
		//tmpGV.SelectedIndex = 0;
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
		if (tmpGV.ID == GridView1.ID)
		{
			tmpGV.DataSource = sourceData;
		}
		else
		{
			tmpGV.DataSource = listeStagiairesToSee;
		}
		tmpGV.DataBind();
	}
	void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		if (GridView1.SelectedRow != null)
		{
			GridView1.SelectedRow.Style.Clear();
			(GridView1.SelectedRow.FindControl("RBChecked") as RadioButton).Checked = false;
		}
	}
	void GridView1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (GridView1.Rows.Count == 0 || GridView1.SelectedIndex == -1) return;
		GridView1.SelectedRow.Style["background-color"] = "silver !important";
		LabelSession.Text = GridView1.SelectedRow.Cells[0].Text;
		DivSession.Visible = typeInscription != EnumInscription.COLLABOINSCRIPTION;
		(GridView1.SelectedRow.FindControl("RBChecked") as RadioButton).Checked = true;
		idSession = Convert.ToInt32(GridView1.SelectedValue);
		bindInscrits();
	}
	void GridView1_DataBound(object sender, EventArgs e)
	{
		int i = 0;
		foreach (GridViewRow row in (sender as GridView).Rows)
		{
			if (row.RowType == DataControlRowType.DataRow)
			{
				row.Cells[0].Style["padding-top"] = "15px";
				row.Cells[0].Style["padding-bottom"] = "15px";
				row.Cells[0].Text += "#" + (i + 1);
				if (typeInscription != EnumInscription.ADMININSCRIPTIONS && (bool)(sender as GridView).DataKeys[row.RowIndex].Values["IsComplete"] == true)
				{
					(row.Cells[2].Controls[1] as Label).Text = "Complète<br/>" + (row.Cells[2].Controls[1] as Label).Text;
					row.Cells[3].Controls[1].Visible = false;
				}
			}
			i++;
		}
		var tmpIndex = -1;
		if (idSession > 0)
		{
			foreach (DataKey key in GridView1.DataKeys)
			{
				tmpIndex++;
				if (Convert.ToInt32(key.Value) == idSession) break;
			}
		}
		if (tmpIndex == -1 && GridView1.Rows.Count > 0)
		{
			i = 0;
			foreach (GridViewRow row in (sender as GridView).Rows)
			{
				if (row.RowType == DataControlRowType.DataRow)
				{
					if (row.Cells[3].Controls[1].Visible == true)
					{
						tmpIndex = i;
						break;
					}
				}
				i++;
			}
		}
		if (GridView1.Rows.Count > 0 && tmpIndex >= 0)
			GridView1.SelectRow(tmpIndex);
	}
	void GridView23_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			(e.Row.FindControl("BtnActivate") as ImageButton).ImageUrl = "Styles/Images/icon_deactivate.png";
			if ((bool)(sender as GridView).DataKeys[e.Row.RowIndex].Values["IsAlreadyRegistered"])
			{
				e.Row.FindControl("BtnActivate").Visible = false;
			}
			if (typeInscription == EnumInscription.COLLABOINSCRIPTION)
			{
				if ((sender as GridView).DataKeys[e.Row.RowIndex].Values["idSession"].ToString() == GridView1.DataKeys[GridView1.SelectedIndex].Values["Id_Ses"].ToString())
				{
					e.Row.Style["background-color"] = "silver !important";
				}
			}

		}
	}
	void GridView23_DataBound(object sender, EventArgs e)
	{
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
				cell.CssClass = ToolBox.HtmlDecode(cell.CssClass = (cell.Controls[0] as LinkButton).Text).Contains(sortExpressionGV23) ? "ColumnSorted" : "";
			}
		}
	}
	void GridView23_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandSource.GetType() == typeof(GridView)) return;
		var tmpGV = (sender as GridView);
		var tmpIndex = ((GridViewRow)((WebControl)e.CommandSource).NamingContainer).RowIndex;
		switch (e.CommandName)
		{
			case "Activate":
				var tmpObj = listeStagiairesToSee.Find(s => s.Id == Convert.ToInt32(tmpGV.DataKeys[tmpIndex].Values["Id"]));
				var tmpStagiaires = listeStagiaires;
				tmpStagiaires.RemoveAll(s => s.Id == tmpObj.Id);
				listeStagiaires = tmpStagiaires;
				(sender as GridView).DataSource = listeStagiairesToSee;
				(sender as GridView).DataBind();
				break;
		}
	}
	void GridView23_Sorting(object sender, GridViewSortEventArgs e)
	{
		if (e.SortExpression == sortExpressionGV23)
		{
			e.SortDirection = sortDirectionGV23 == "Descending" ? SortDirection.Ascending : SortDirection.Descending;
		}
		else
		{
			e.SortDirection = SortDirection.Ascending;
		}
		sortExpressionGV23 = e.SortExpression.ToString();
		sortDirectionGV23 = e.SortDirection.ToString();
		(sender as GridView).DataSource = listeStagiairesToSee;
		(sender as GridView).DataBind();
		foreach (TableCell cell in (sender as GridView).HeaderRow.Cells)
		{
			cell.CssClass = "";
			if (cell.Controls.Count > 0 && (cell.Controls[0] as LinkButton) != null)
			{
				cell.CssClass = ToolBox.HtmlDecode(cell.CssClass = (cell.Controls[0] as LinkButton).Text).Contains(sortExpressionGV23) ? "ColumnSorted" : "";
			}
		}
	}
	protected void RBChecked_CheckedChanged(object sender, EventArgs e)
	{
		GridView1.SelectRow(((sender as RadioButton).NamingContainer as GridViewRow).RowIndex);
	}
	void bindFormation()
	{
		Formation tmpFormation = null;
		var tmpModel = new ModelManager();
		if (idSession > 0)
		{
			var tmpSession = tmpModel.GetSessions(idSession,0).FirstOrDefault();
			tmpFormation = tmpSession.Formation;
		}
		if (idFormation > 0)
		{
			tmpFormation = tmpModel.GetFormations(idFormation).FirstOrDefault();
		}
		LabFormation.Text = tmpFormation.Lib_fm;
		LabTheme.Text = tmpFormation.Theme.Lib_Theme;
		LabContact.Text = tmpFormation.ContactFormation;
		LabDuree.Text = tmpFormation.DureeFormation;
		HLPdf.NavigateUrl = uploadPdfFolder + tmpFormation.File_fm;
		ImagePdf.Visible = !string.IsNullOrEmpty(tmpFormation.PdfLink);
	}
	void bindInscrits() // Recupere les inscrits  de la db et crée le viewstate XML
	{
		var tmpModel = new ModelManager();
		sourceData.ForEach(s =>
		{
			var tmpInscrits = s.Inscrits.Where(i => i.Statut_Ins).ToList();
			var tmpStagiaires = listeStagiaires == null ? new List<StagiaireXml>() : listeStagiaires;
			tmpInscrits.ForEach(i =>
			{
				if (!listeStagiaires.Exists(u => u.IdUser == i.fk_id_stagiaire && u.idSession == i.fk_id_ses))
				{
					tmpStagiaires.Add(new StagiaireXml(i.Id_Ins, i.fk_id_ses, i.User.Nom_Responsable, i.fk_id_dep, i.Departement != null ? i.Departement.Lib_Dep : null, i.Date_Inscription)
					{   IdUser = i.fk_id_stagiaire,
						Nom = i.User.Nom_U,
						Prenom = i.User.Prenom_U,
						IsAlreadyRegistered = true
					});
					if (i.Id_Ins > maxInscritId)
					{
						maxInscritId = i.Id_Ins;
					}
				}
			}
				);
			listeStagiaires = tmpStagiaires;
		});
		var tmpGV = typeInscription != EnumInscription.COLLABOINSCRIPTION ? GridView2 : GridView3;
		tmpGV.DataSource = listeStagiairesToSee;
		tmpGV.DataBind();
	}
	void resetUi()
	{
		TBSearch.Text = "";
		TBSearch.Enabled = true;
		TBNom.Text = "";
		TBNom.Enabled = true;
		TBPrenom.Text = "";
		TBPrenom.Enabled = true;
		TBResponsable.Text = typeInscription != EnumInscription.COLLABOINSCRIPTION ? "" : TBResponsable.Text;
		DDLDepartements.SelectedIndex = 0;
	}

	void clear()
	{
		resetUi();
		if (typeInscription == EnumInscription.COLLABOINSCRIPTION)
		{
			var tmpListe = listeStagiaires;
			tmpListe.RemoveAll(p => listeStagiairesToSee.Any(c => c.NomComplet == p.NomComplet));
			listeStagiaires = tmpListe;
			var tmpGV = GridView3;
			tmpGV.DataSource = listeStagiairesToSee;
			tmpGV.DataBind();
		}
	}
	protected string getSessionIndex(string pIdSession)
	{
		var indexRow = 0;
		foreach (DataKey item in GridView1.DataKeys)
		{

			if (item.Values["Id_Ses"].ToString() == pIdSession)
			{
				var retour = GridView1.Rows[indexRow].Cells[0].Text;
				retour = retour.Substring(0, 1);
				retour += GridView1.Rows[indexRow].Cells[0].Text.Split('#')[1];
				return retour;
			}
			indexRow++;
		}
		return "";
	}
	#region nested classes
	[Serializable]
	public class StagiaireXml
	{
		public StagiaireXml()
		{
		}
		public StagiaireXml(int pId, int pIdSession, string pResponsable, int? pIdDepartement, string pDepartement, DateTime pDateInscription)
		{
			Id = pId;
			idSession = pIdSession;
			Responsable = pResponsable;
			IdDepartement = pIdDepartement;
			DateInscription = pDateInscription;
			Departement = pDepartement;
		}
		public int Id { get; set; }
		public int IdUser { get; set; } // Si User En Base
		public int idSession { get; set; }
		public DateTime DateInscription { get; set; }
		public string Nom { get; set; }// Si User pas En Base
		public string Prenom { get; set; }// Si User pas En Base
		public bool IsAlreadyRegistered { get; set; }// Si déja inscrit en bdd
		[XmlIgnore]
		public string NomComplet { get { return Nom + ", " + Prenom; } }
		public string Responsable { get; set; }
		public int? IdDepartement { get; set; }
		public string Departement { get; set; }
		//[XmlIgnore]
		// public string Departement { get { return IdDepartement!=null? new ModelManager().GetDepartements().Find(d => d.Id_Dep == IdDepartement).Lib_Dep:null; } }
		[XmlIgnore]
		public string ShortDepartement { get { return !string.IsNullOrEmpty(Departement) ? Departement.Length > 6 ? Departement.Substring(0, 6) : Departement : ""; } }
	}
	#endregion
}