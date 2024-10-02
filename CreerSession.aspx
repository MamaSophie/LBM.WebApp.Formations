<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreerSession.aspx.cs" Inherits="CreerSession" %>
<%@ Register TagPrefix="AjaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>
<%@ Register Src="~/UserControls/UC_Header.ascx" TagPrefix="uc" TagName="UC_Header" %>
<%@ Register Src="~/UserControls/UC_ErrorPanel.ascx" TagPrefix="uc" TagName="UC_ErrorPanel" %>
<%@ Register Src="~/UserControls/UC_Footer.ascx" TagPrefix="uc" TagName="UC_Footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Session</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>       
    <uc:UC_Header ID="UC_Header1" runat="server" />    
    <div id="main" class="main"  style="margin-left:100px;" >
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >                                                  
        <ContentTemplate>      
        <asp:Image ID="Image1" runat="server"  ImageUrl="Styles/Images/Sessions.png" Width="30" />
        <asp:Label ID="Label1" runat="server" Text="Créer une session"  CssClass="PageTitle"   ></asp:Label>
        <table style="margin-top:20px;width:100%">
            <tr><td colspan="2" style="width:100%;vertical-align:top;text-align:left">
                <div style="text-align:left;vertical-align:top;">
                        <asp:Label ID="Label6" runat="server" Text="Formation:" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                         
                        <asp:DropDownList ID="DDLFormations" AutoPostBack="true" CssClass="form-control" Width="300" style="display:inline;"  runat="server"></asp:DropDownList>
                    </div>                       
                </td> </tr>
            <tr>                
                <td style="width:30%;vertical-align:top">    
                    
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label2" runat="server" Text="Fichier:" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:HyperLink ID="HLPdf" runat="server" Target="_blank" style="display:inline;" Visible="false"><asp:Image ID="ImgPdf" runat="server"  ImageUrl="Styles/Images/iconePdf.png" style="margin-left:30px" Width="20" /><br /><asp:Label ID="LabPdf" runat="server" Text="" style="margin-left:10px;display:inline;"  CssClass="LibelleTB"   ></asp:Label></asp:HyperLink>                        
                    </div>   
                     <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label3" runat="server" Text="Thème :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabTheme" runat="server" Text="" style="display:inline;" CssClass="LibelleTB"   ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label4" runat="server" Text="Contact :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabContact" runat="server" Text="" style="display:inline;"  CssClass="LibelleTB"  ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label5" runat="server" Text="Durée :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabDuree" runat="server" Text="" style="display:inline;"  CssClass="LibelleTB"  ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label8" runat="server" Text="Accès :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                         <asp:Label ID="LabAcces" runat="server" Text="" style="display:inline;" CssClass="LibelleTB"    ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label7" runat="server" Text="Enseigne:" CssClass="LibelleTB" style="display:inline-block;margin-right:0px;"    ></asp:Label>
                        <asp:Label ID="LabEnseigne" runat="server" Text="" style="display:inline;" CssClass="LibelleTB"   ></asp:Label>                                                 
                    </div>
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">                        
                        <asp:CheckBox ID="CBEmail" runat="server"  Enabled="false" style="display:inline-block;margin-top:0px;margin-left:0px;" />                        
                        <asp:Label ID="Label9" runat="server" Text="Envoyer des emails" CssClass="LibelleTB" style="display:inline-block;height:30px;margin-right:0px;margin-bottom:10px;"></asp:Label>
                    </div>                    
                    <div runat="server" id="DivInscrits" style="text-align:left;vertical-align:top;margin-top:0px;display:none;">                        
                        <asp:Label ID="Label18" runat="server" Text="Inscrits" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                                         
                              <asp:GridView ID="GridView2" runat="server" AllowSorting="true" Caption="" DataKeyNames="Id_Ins,Statut_Ins,fk_id_stagiaire" AutoGenerateColumns="false" CssClass="GridView" Width="80%" CellPadding="5" AllowPaging="true" PageSize="4">                                          
                             <Columns>            
                            <asp:BoundField  HeaderText="Nom" DataField="NomComplet" ItemStyle-Width="60%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Nom" />                                                                                                                                                    
                            <asp:TemplateField  HeaderText="Dpt"  ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Dpt" >
                                              <ItemTemplate>                                                      
                                                  <asp:Label runat="server" Text='<%# Eval("ShortDepartement") %>' ToolTip='<%# Eval("Departement.Lib_Dep") %>'></asp:Label>
                                               </ItemTemplate>                                    
                            </asp:TemplateField>                                      
                            <asp:TemplateField  HeaderText="Action" ItemStyle-Width="15%" >
                                               <ItemTemplate>                                     
                                                   <asp:ImageButton ID="BtnActivate" runat="server" CssClass="ImgActivate"  CommandName="Activate"   AlternateText="Supprimer" ToolTip="Supprimer"  OnClientClick="javascript: /*if(confirm('Confirmer vous la désactivation de la formation ?')){return true;}else return false;*/;" />                                                                                                     
                                               </ItemTemplate>
                                        </asp:TemplateField>          
                              </Columns>
                              <SelectedRowStyle CssClass="LigneSelected" />      
              <PagerStyle  CssClass="pagerStyle" />
              <PagerTemplate >                
              <table  class="pagerTable" >                    
                    <tr  class="pagerLine" >
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnFullLeft" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">|<<</asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnLeft" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri"><</asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:Label ID="LabFolio" CssClass="LabFolio" runat="server" Text="1"></asp:Label>
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnRight" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">></asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnFullRight" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">>>|</asp:LinkButton>                                                        
                        </td>
                    </tr>                    
                </table>
              </PagerTemplate>
                       </asp:GridView>
                    </div>
                </td>   
                <td runat="server" id="TDSession" style="width:70%;vertical-align:top;display:none;" >    
                     <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label10" runat="server" Text="Session" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                                         
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label11" runat="server" Text="Formateur:" CssClass="LibelleTB" style="display:inline;"></asp:Label>                                                                         
                        <asp:DropDownList ID="DDLFormateurs" AutoPostBack="false" CssClass="form-control" Width="200" style="display:inline;"  runat="server"></asp:DropDownList>
                        <asp:Label ID="Label12" runat="server" Text="MIN Inscrits:" CssClass="LibelleTB" style="display:inline;"></asp:Label>
                        <asp:TextBox ID="TBMinInscrits" AutoPostBack="false" Text="0" runat="server"  MaxLength="3" placeholder="" CssClass="form-control" Width="50" style="display:inline;" ></asp:TextBox>                                                                                                                   
                        <asp:Label ID="Label13" runat="server" Text="MAX Inscrits" CssClass="LibelleTB" style="display:inline;"></asp:Label>
                        <asp:TextBox ID="TBMaxInscrits" AutoPostBack="false"  Text="0" runat="server" MaxLength="3" placeholder="" CssClass="form-control" Width="50" style="display:inline;" ></asp:TextBox>                                                                                                                   
                        <asp:ImageButton ID="BtnEdit" runat="server"  ImageUrl="~/Styles/Images/icon_select.png" Width="20" Visible="false" style="margin-left:5px;margin-bottom:-5px;"/>
                        <asp:ImageButton ID="BtnEmail" runat="server" style="width:20px;height:15px;margin-bottom:-3px;" ImageUrl="~/Styles/Images/relance.png" Visible="false" />                                                   
                    </div>   
                    <div runat="server" id="DivBtnsSession" style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Button ID="BtnAddSession" runat="server" Text="Valider" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnCancelSession" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                    </div>   
                    <div runat="server" id="DivDates" visible="false">
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label14" runat="server" Text="Date(s)" CssClass="LibelleTB" style="display:inline;"></asp:Label>
                    </div>   
                     <div style="text-align:left;vertical-align:top;margin-top:0px;">
                         <asp:Label ID="Label15" runat="server" Text="Heure de début" CssClass="LibelleTB" style="margin-left:180px;display:inline;"></asp:Label>
                         <asp:Label ID="Label16" runat="server" Text="Heure de fin" CssClass="LibelleTB" style="margin-left:120px;display:inline;"></asp:Label>
                    </div>
                    <div style="text-align:left;vertical-align:top;margin-top:5px;">
                        <asp:TextBox ID="TBDATE"  Text="" runat="server"  placeholder="date" CssClass="form-control" Width="150" style="display:inline;" ></asp:TextBox>                                                                                                                   
                         <asp:DropDownList ID="DDLHDEB" AutoPostBack="false" CssClass="form-control" Width="80" style="display:inline;"  runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="DDLMDEB" AutoPostBack="false" CssClass="form-control" Width="80" style="display:inline;"  runat="server"></asp:DropDownList>
                         <asp:DropDownList ID="DDLHFIN" AutoPostBack="false" CssClass="form-control" Width="80" style="margin-left:20px;display:inline;"  runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="DDLMFIN" AutoPostBack="false" CssClass="form-control" Width="80" style="display:inline;"  runat="server"></asp:DropDownList>
                    </div>
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label17" runat="server" Text="Salle:" CssClass="LibelleTB" style="display:inline;"></asp:Label>
                        <asp:DropDownList ID="DDLSalles" AutoPostBack="false" CssClass="form-control" Width="200" style="margin-left:30px;display:inline;"  runat="server"></asp:DropDownList>
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Button ID="BtnAddDate" runat="server" Text="Ajouter" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnCancelDate" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                    </div>   
                    
                    <div style="text-align:left;vertical-align:top;margin-top:10px;margin-bottom:0px;">
                        <asp:GridView ID="GridView1" runat="server" AllowSorting="true" Caption="" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="GridView" Width="60%" CellPadding="5" AllowPaging="true" PageSize="5">                                          
                                        <Columns>            
                                        <asp:BoundField  HeaderText="Date" DataField="FullDate" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Date"/>                                                                                                            
                                        <asp:BoundField  HeaderText="Salle" DataField="Salle" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>                                                                                                            
                                        <asp:TemplateField  HeaderText="Action" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                               <ItemTemplate>         
                                                   <asp:ImageButton ID="BtnSelect" runat="server"  CssClass="ImgSelect" CommandName="Selecte"  />
                                                   <asp:ImageButton ID="BtnActivate" runat="server" CssClass="ImgActivate"  CommandName="Activate"  />                                                   
                                               </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        </Columns>                                            
          <SelectedRowStyle CssClass="LigneSelected" />      
              <PagerStyle  CssClass="pagerStyle" />
              <PagerTemplate >                
              <table  class="pagerTable" >                    
                    <tr  class="pagerLine" >
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnFullLeft" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">|<<</asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnLeft" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri"><</asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:Label ID="LabFolio" CssClass="LabFolio" runat="server" Text="1"></asp:Label>
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnRight" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">></asp:LinkButton>                                                        
                        </td>
                        <td style="text-align: center; width: 20%">
                            <asp:LinkButton ID="BtnFullRight" runat="server" OnClick="PagerIndexChange" style="font-size:15px;font-family:Calibri">>>|</asp:LinkButton>                                                        
                        </td>
                    </tr>                    
                </table>
              </PagerTemplate>
         </asp:GridView>       
                    </div>   
                 </div>
                 </td>
            </tr>
        </table>               
            <div runat="server" id="DivBtnsValidation" style="width:auto;float:right;vertical-align:top;margin-top:20px;margin-right:100px;" visible="false">
                        <asp:Button ID="BtnAdd" runat="server" Text="Créer" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnCancel" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push" UseSubmitBehavior="false"  style="margin-top:0px;margin-left:50px;margin-right:20px;display:inline;"/>
            </div>                           
         <AjaxToolkit:CalendarExtender
            runat="server"
            TargetControlID="TBDATE" 
            ID="CalendarExtender1"          
            Format="dd/MM/yyyy"            
            TodaysDateFormat="dd MMMM yyyy"                      
            /> 
        </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="PanelModal" runat="server" CssClass="panelmodal" style="position: fixed;left: 0px;top: 0px;width: 2000px; height: 2000px;margin-left:auto; margin-right:auto;z-index:10000;display:inline;background-color: Silver;opacity: 0.5;filter: alpha(opacity=50);-moz-opacity: 0.5;" Visible="false"></asp:Panel>
                <asp:Panel ID="PanelEmail" runat="server" CssClass="panelpopup" Width="600" Height="300"
                    Visible="false">                    
                        <div class="PageTitle"  style="width:100%;text-align:center;margin-top:10px;margin-bottom:25px;" > Relance</div>
                        <div style="text-align:left;margin-left:100px;"> Objet de l'email:</div>
                        <asp:TextBox ID="TBEmail"  CssClass="form-control" style="margin-left:auto;margin-right:auto;margin-top:10px;width:500px;" runat="server"></asp:TextBox>
                        <div runat="server" id="Div1" style="float:right;width:auto;vertical-align:top;margin-top:50px;margin-right:30px;" >
                        <asp:Button ID="BtnEnvoyerEmail" runat="server" Text="Envoyer" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:20px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnAnnulerEmail" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push" UseSubmitBehavior="false"  style="margin-top:20px;margin-left:10px;margin-right:20px;display:inline;"/>
                        </div>                           
                </asp:Panel>
                <AjaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender5" runat="server" TargetControlID="PanelEmail"
                    VerticalSide="Middle" VerticalOffset="10" HorizontalSide="Center" HorizontalOffset="10"
                    ScrollEffectDuration=".1" />
            </ContentTemplate>
            <Triggers>                
            </Triggers>
        </asp:UpdatePanel>
        <uc:UC_ErrorPanel ID="UC_ErrorPanel1" runat="server" />
    </div>
    <uc:UC_Footer  runat="server" ID="UC_Footer1"/>
    </form>
</body>
</html>
