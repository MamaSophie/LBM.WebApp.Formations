<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreerFormation.aspx.cs" Inherits="CreerFormation" %>
<%@ Register TagPrefix="AjaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>
<%@ Register Src="~/UserControls/UC_Header.ascx" TagPrefix="uc" TagName="UC_Header" %>
<%@ Register Src="~/UserControls/UC_ErrorPanel.ascx" TagPrefix="uc" TagName="UC_ErrorPanel" %>
<%@ Register Src="~/UserControls/UC_Footer.ascx" TagPrefix="uc" TagName="UC_Footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Formation</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>       
    <uc:UC_Header ID="UC_Header1" runat="server" />    
    <div id="main" class="main" style="width:75%;" >
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >                                                  
        <ContentTemplate>      
        <asp:Image ID="Image1" runat="server"  ImageUrl="Styles/Images/formations.png" Width="30" />
        <asp:Label ID="Label1" runat="server" Text="Créer une formation"  CssClass="PageTitle"   ></asp:Label>
        <table style="margin-top:20px;">
            <tr>                
                <td style="width:100%;vertical-align:top">    
                    <div style="text-align:left;vertical-align:top;">
                        <asp:Label ID="Label6" runat="server" Text="Nom:" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                         
                        <asp:TextBox ID="TBFormation" AutoPostBack="false" runat="server"  placeholder="Titre de la formation" CssClass="form-control" Width="400" style="margin-left:10px;display:inline;" ></asp:TextBox>                                                                                                                   
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">                    
                    <asp:UpdatePanel ID="UpdatePanel1" Visible="true" runat="server" UpdateMode="Always" >                                                  
                    <ContentTemplate>      
                    <div>                    
                    <asp:Label ID="Label2" runat="server" Text="Fichier:" CssClass="LibelleTB" style="display:inline;"></asp:Label>
                    <asp:Label ID="LabFile" runat="server" Text="" CssClass="LibelleTB" style="margin-left:10px;margin-right:10px;width:100px;display:inline;"></asp:Label>
                    <div class="fileUpload btn btn-dore hvr-push" style="margin-left:-3px;">                    
                    <span>Choisir</span>
                    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="false"  CssClass="upload" />                    
                    </div>                   
                   <asp:ImageButton ID="BtnUpload" runat="server"  ImageUrl="Styles/Images/BtnUpload.png"  ImageAlign="AbsMiddle"  />
                   <asp:Label ID="LabUpload" runat="server" Text="Envoyer" ></asp:Label>                   
                   </div>                                      
                   </ContentTemplate>
                         <Triggers>                             
                             <asp:PostBackTrigger ControlID="BtnUpload" />
                         </Triggers>
                   </asp:UpdatePanel>
                    </div>
                     <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label3" runat="server" Text="Thème :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                         <asp:DropDownList ID="DDLThemes" CssClass="form-control" Width="400" style="float:right;margin-right:190px;display:inline;"  runat="server"></asp:DropDownList>
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:40px;">
                        <asp:Label ID="Label4" runat="server" Text="Contact :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                         <asp:DropDownList ID="DDLContacts" CssClass="form-control" Width="400" style="float:right;margin-right:190px;display:inline;"  runat="server"></asp:DropDownList>
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:40px;">
                        <asp:Label ID="Label5" runat="server" Text="Durée :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:TextBox ID="TBDuree" AutoPostBack="false" runat="server"  placeholder="" CssClass="form-control" Width="70" style="margin-left:5px;display:inline;" ></asp:TextBox>                                                                                                                   
                        <asp:DropDownList ID="DDLDuree" CssClass="form-control" Width="70" style="margin-left:8px;display:inline;" runat="server">
                            <asp:ListItem Value="J" Text="J" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="H" Text="H"></asp:ListItem>
                        </asp:DropDownList>
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:30px;">
                        <asp:Label ID="Label8" runat="server" Text="Accès :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                         <asp:DropDownList ID="DDLAcces" CssClass="form-control" Width="400" style="float:right;margin-right:190px;display:inline;"  runat="server"></asp:DropDownList>
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:30px;">
                        <asp:Label ID="Label7" runat="server" Text="Enseigne:" CssClass="LibelleTB" style="display:inline-block;margin-right:0px;"    ></asp:Label>
                        <asp:CheckBoxList ID="CBEnseigne" SelectMethod="" runat="server" TextAlign="Right" CssClass="rbl"  style="display:inline-block;margin-bottom:-10px;margin-left:20px;"   RepeatDirection="Horizontal"  DataTextField="Libelle_Enseigne" DataValueField="Id_Typ_enseigne"></asp:CheckBoxList>                        
                    </div>
                    <div style="text-align:left;vertical-align:top;margin-top:30px;">                        
                        <asp:CheckBox ID="CBEmail" runat="server" Checked="true" style="display:inline-block;margin-top:0px;margin-left:0px;" />                        
                        <asp:Label ID="Label9" runat="server" Text="Envoyer des emails" CssClass="LibelleTB" style="display:inline-block;height:30px;margin-right:0px;margin-bottom:10px;"></asp:Label>
                    </div>                    
                </td>   
            </tr>
        </table>                        
            <div style="width:auto;float:right;vertical-align:top;margin-top:0px;margin-right:0px;">
                        <asp:Button ID="BtnAdd" runat="server" Text="Créer" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnCancel" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push" UseSubmitBehavior="false"  style="margin-top:0px;margin-left:50px;margin-right:20px;display:inline;"/>
            </div>                    
          </ContentTemplate>
        </asp:UpdatePanel>
        <uc:UC_ErrorPanel ID="UC_ErrorPanel1" runat="server" />
    </div>
    <uc:UC_Footer  runat="server" ID="UC_Footer1"/>
    </form>
</body>
</html>
