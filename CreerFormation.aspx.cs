using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;

public partial class CreerFormation : System.Web.UI.Page
{
    private int typeContact = 1;
    private string uploadPdfFolder = "Upload/Pdf/Formations/";
    ILogger _log = LogHelper.Logger;
    private bool isCreateMode
    {
        get { return (bool)ViewState["isCreateMode"]; }
        set { ViewState["isCreateMode"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        // CONTROLE HABILITATION //
        if (!UC_Header1.IsAdminRole)
        {
            _log.Info("Utilisateur n'appartient pas au groupe B-APPL-INSCRIPTION_FORMATION, redirection vers default.aspx");
            Response.Redirect("default.aspx");
        }
        // CONTROLE HABILITATION //END
        BtnAdd.Text = "Créer";
        BtnAdd.Click+=BtnAdd_Click;
        BtnCancel.Click+=BtnCancel_Click;
        BtnUpload.Click += BtnUpload_Click;
        if (!Page.IsPostBack)
        {
            isCreateMode = true;
            var tmpModel = new ModelManager();                       
            DDLThemes.DataSource = (from t in tmpModel.GetThemes().FindAll(t => t.Statut_tm) orderby t.Lib_Theme select t);
            DDLThemes.DataTextField = "lib_theme";
            DDLThemes.DataValueField = "id_theme";
            DDLThemes.DataBind();
            DDLThemes.Items.Insert(0, new ListItem("Sélectionner un thème", "", true));
            DDLThemes.SelectedIndex = 0;

            DDLContacts.DataSource = (from u in tmpModel.GetUsersCreerFormation(typeContact) orderby u.Nom_U select new { nomcomplet = u.Prenom_U + " " + u.Nom_U, id = u.Id_U });
            DDLContacts.DataTextField = "nomcomplet";
            DDLContacts.DataValueField = "id";
            DDLContacts.DataBind();
            DDLContacts.Items.Insert(0, new ListItem("Sélectionner un contact", "", true));
            DDLContacts.SelectedIndex = 0;

            DDLAcces.DataSource = tmpModel.Acces_Type.ToList();
            DDLAcces.DataValueField = "id_acces";
            DDLAcces.DataTextField = "lib_acces";
            DDLAcces.DataBind();
            DDLAcces.Items.Insert(0, new ListItem("Sélectionner un accès", "", true));
            DDLAcces.SelectedIndex = 0;

            CBEnseigne.DataBound += CBEnseigne_DataBound;
            CBEnseigne.DataSource = tmpModel.GetEnseignes();
            CBEnseigne.DataTextField = "Libelle_Enseigne";
            CBEnseigne.DataValueField = "Id_Typ_enseigne";
            CBEnseigne.DataBind();

            if (Request["idFormation"] != null)
            {
                _log.Info(string.Format("Modifier Formation, IdFormation = {0}", Convert.ToInt32(Request["idFormation"])));
                var tmpFormation = tmpModel.GetFormations(Convert.ToInt32(Request["idFormation"])).FirstOrDefault();
                if (tmpFormation != null) bindDataToControls(tmpFormation);
                isCreateMode = false;                
            }
        }
        BtnAdd.Text = !isCreateMode?"Modifier":"Créer";
        Label1.Text = !isCreateMode ? "Modifier une formation":"Créer une formation";
    }
    private void CBEnseigne_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem item in CBEnseigne.Items)
        {
          item.Attributes["style"] += "margin-right:10px";
        }
    }
    void BtnUpload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var tmpFileUpload = FileUpload1;
            string tmpFileFullName = "";
            if (tmpFileUpload.HasFile && tmpFileUpload.FileBytes.Length > 0 && (tmpFileUpload.FileName.EndsWith(".pdf")))
            {
                tmpFileFullName += tmpFileUpload.FileName;
                tmpFileUpload.SaveAs(MapPath(uploadPdfFolder) + tmpFileFullName);
                if (!File.Exists(MapPath(uploadPdfFolder) + tmpFileFullName))
                {
                    UC_ErrorPanel1.DisplayError("Fichier non téléchargé. Vérifiez les droits en écriture sur le serveur.");
                    return;
                }
            }
            else
            {
                UC_ErrorPanel1.DisplayError("Fichier non conforme.");
                return;
            }
            LabFile.Text = tmpFileFullName;
        }
        catch(Exception ex)
        {
            _log.Error("Erreur dans CreerFormation, BtnUpload_Click : ", ex); 
        }        
    }
    void BtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListeFormations.aspx");
    }
    void BtnAdd_Click(object sender, EventArgs e)
    {
        _log.Info("CreerFormation, Début Ajout Formation");
        try
        {
            var tmpCheckErrors = checkData();
            if (!string.IsNullOrEmpty(tmpCheckErrors))
            {
                UC_ErrorPanel1.DisplayError(tmpCheckErrors);
                return;
            }
            var tmpModel = new ModelManager();
            Formation tmpFormation = null;
            if (Request["idFormation"] != null)
            {
                _log.Info(string.Format("Modifier Formation, IdFormation = {0}", Convert.ToInt32(Request["idFormation"])));
                tmpFormation = tmpModel.GetFormations(Convert.ToInt32(Request["idFormation"])).FirstOrDefault();
                tmpFormation.Date_Update = DateTime.Now;
            }
            else
            {
                _log.Info("Request['idFormation'] = null");
                tmpFormation = new Formation();
                tmpFormation.Date_Creation = DateTime.Now;
            }
            tmpFormation.Duration = Convert.ToDecimal(TBDuree.Text);
            tmpFormation.Type_Duration = DDLDuree.SelectedValue;
            tmpFormation.eMail_Auto = CBEmail.Checked;
            tmpFormation.fk_Id_Acces = Convert.ToInt32(DDLAcces.SelectedValue);
            tmpFormation.fk_Id_Theme = Convert.ToInt32(DDLThemes.SelectedValue);
            tmpFormation.fk_id_u_Contact = Convert.ToInt32(DDLContacts.SelectedValue);
            tmpFormation.Lib_fm = TBFormation.Text;
            tmpFormation.Statut_fm = tmpFormation.Id_fm > 0 ? tmpFormation.Statut_fm : true;
            tmpFormation.File_fm = LabFile.Text;
            tmpFormation.Formation_Enseigne.Clear();
            foreach (ListItem cb in CBEnseigne.Items)
            {
                if (cb.Selected) tmpFormation.Formation_Enseigne.Add(new Formation_Enseigne { FK_ID_Typ_Enseigne = Convert.ToInt32(cb.Value), FK_Id_Form = tmpFormation.Id_fm });
            }
            var retour = tmpModel.SetFormation(tmpFormation);
            if (!string.IsNullOrEmpty(retour))
            {
                UC_ErrorPanel1.DisplayError(retour);
            }
            else
            {
                Response.Redirect("ListeFormations.aspx",false);
            }
        }
        catch(Exception ex)
        { 
            _log.Error("Erreur dans CreerFormation, BtnAdd_Click :", ex);
        }
        _log.Info("CreerFormation, Fin Ajout Formation");
    }
    string checkData()
    {
        string retour = "";
        if(string.IsNullOrEmpty(TBFormation.Text)) retour += "Veuillez saisir un nom de formation<br>";
        if (DDLThemes.SelectedIndex==0) retour += "Veuillez choisir un thème<br>";
        if (DDLContacts.SelectedIndex == 0) retour += "Veuillez choisir un contact<br>";
        if (string.IsNullOrEmpty(TBDuree.Text) || !ToolBox.IsNumeric(TBDuree.Text) || double.Parse(TBDuree.Text)<=0 ) retour += "Veuillez saisir une durée > 0<br>";        
        if (DDLAcces.SelectedIndex == 0) retour += "Veuillez choisir un accès<br>";
        if (CBEnseigne.SelectedIndex < 0) retour += "Veuillez sélectionner une enseigne <br>";        
        return retour;
    }
    void bindDataToControls(Formation pFormation)
    {
        try
        {
            TBFormation.Text = pFormation.Lib_fm;
            LabFile.Text = pFormation.File_fm;
            DDLThemes.SelectedValue = pFormation.fk_Id_Theme.ToString();
            DDLContacts.SelectedValue = pFormation.fk_id_u_Contact.ToString();
            TBDuree.Text = pFormation.Duration.ToString();
            DDLDuree.SelectedValue = pFormation.Type_Duration;
            DDLAcces.SelectedValue = pFormation.fk_Id_Acces.ToString();
            foreach (ListItem item in CBEnseigne.Items)
            {
                if (new ModelManager().Formation_Enseigne.ToList().Exists(e => e.FK_Id_Form == pFormation.Id_fm && e.FK_ID_Typ_Enseigne.ToString() == item.Value)) item.Selected = true;
            }
            CBEmail.Checked = pFormation.eMail_Auto;
            TBDuree.Enabled = !pFormation.T_Sessions.Any(s => s.Statut_Ses);
            DDLDuree.Enabled = TBDuree.Enabled;
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
        }
    }
}