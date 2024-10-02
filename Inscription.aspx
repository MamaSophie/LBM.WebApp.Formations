<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inscription.aspx.cs" Inherits="Inscription" %>
<%@ Register TagPrefix="AjaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>
<%@ Register Src="~/UserControls/UC_Header.ascx" TagPrefix="uc" TagName="UC_Header" %>
<%@ Register Src="~/UserControls/UC_ErrorPanel.ascx" TagPrefix="uc" TagName="UC_ErrorPanel" %>
<%@ Register Src="~/UserControls/UC_Footer.ascx" TagPrefix="uc" TagName="UC_Footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inscription</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>       
    <uc:UC_Header ID="UC_Header1" runat="server" />    
    <div id="main" class="main" style="margin-left:50px; width:96%">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >                                                  
        <ContentTemplate>      
        <asp:Image ID="Image1" runat="server"  ImageUrl="Styles/Images/Sessions.png" Width="30" style="margin-right:10px;" />
        <asp:Label ID="LabTitlepage" runat="server" Text="Inscrire des collaborateurs"  CssClass="PageTitle"   ></asp:Label>
        <table style="width:100%;">
            <tr>
           <td style="width:50%;vertical-align:top;">
           <table style="margin-top:10px;width:100%;">
            <tr><td style="width:100%;vertical-align:top;text-align:left">
                <div style="text-align:left;vertical-align:top;">
                        <asp:Label ID="Label6" runat="server" Text="Formation:" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                         
                        <asp:Label ID="LabFormation" runat="server" Text="" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                         
                        <asp:HyperLink ID="HLPdf" runat="server" style="margin-left:5px;margin-bottom:-5px;" Target="_blank">
                        <asp:Image ID="ImagePdf" runat="server" ImageUrl="Styles/Images/iconePdf.png" Width="20" /></asp:HyperLink>                        
                    </div>                       
                </td> </tr>
            <tr>                
                <td style="width:100%;vertical-align:top">                                            
                     <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label3" runat="server" Text="Thème :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabTheme" runat="server" Text="" style="margin-left:16px;display:inline;" CssClass="LibelleTB"   ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label4" runat="server" Text="Contact :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabContact" runat="server" Text="" style="margin-left:16px;display:inline;"  CssClass="LibelleTB"  ></asp:Label>                                                 
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label5" runat="server" Text="Durée :" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                 
                        <asp:Label ID="LabDuree" runat="server" Text="" style="margin-left:25px;display:inline;"  CssClass="LibelleTB"  ></asp:Label>                                                 
                    </div>                                                               
                    <div runat="server" id="DivSessions" style="text-align:left;vertical-align:top;margin-top:10px;">                                                
                            <asp:GridView ID="GridView1" runat="server" AllowSorting="true" Caption="" DataKeyNames="Id_Ses,IsComplete" AutoGenerateColumns="false" CssClass="GridView" Width="100%" CellPadding="5" AllowPaging="false" PageSize="4">                                          
                                        <Columns>            
                                        <asp:BoundField  HeaderText="Session" DataField="Lib_Ses" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />                                                                                                                                                    
                                        <asp:BoundField  HeaderText="Capacité" DataField="Capacity" ItemStyle-Width="15%"  ItemStyle-CssClass="centerHeader"  HeaderStyle-HorizontalAlign="Left" />                                                                                                                                                                                           
                                       <asp:TemplateField HeaderText="Dates"  ItemStyle-Width="60%">
                                        <ItemTemplate >         
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("AllDatesStringHtml") %>'></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>                                                                                                  
                                        <asp:TemplateField HeaderText=""  ItemStyle-Width="25%">
                                        <ItemTemplate >         
                                        <asp:RadioButton ID="RBChecked" runat="server" AutoPostBack="true" OnCheckedChanged="RBChecked_CheckedChanged" />                                        
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
            </tr>
        </table>          
                </td>
           <td style="vertical-align:top;width:5%;"></td> 
           <td style="vertical-align:top;width:45%;">
           <table style="margin-top:10px;width:100%;">
           <div id="DivUser" runat="server" visible="false">
            <tr>
                <td >
                    <div style="text-align:left;vertical-align:top;margin-top:0px;">                        
                        <asp:Label ID="Label12" runat="server" Text="Nom:" CssClass="LibelleTB" style="display:inline;"></asp:Label>                                                                                                 
                        <asp:Label ID="LABNOM" runat="server" Text="" CssClass="LibelleTB" style="margin-left:167px;display:inline;" ></asp:Label>                                                                                                 
                    </div>                       
                    <div style="text-align:left;vertical-align:top;margin-top:10px;">                        
                        <asp:Label ID="Label13" runat="server" Text="Prénom:" CssClass="LibelleTB" style="display:inline;"></asp:Label>                                                                                                 
                        <asp:Label ID="LABRENOM" runat="server" Text="" CssClass="LibelleTB" style="margin-left:150px;display:inline;" ></asp:Label>                                                                                                 
                    </div>                       
                    </td>
                </tr>
            </div>             
           <div id="DivAdmin0" runat="server">
            <tr runat="server" id="ROWCOLLABOSANSEMAIL">
                <td >
                    <div style="text-align:left;vertical-align:top;margin-top:0px;">
                        <asp:Label ID="Label10" runat="server" Text="Collaborateur avec email" CssClass="LibelleTB" style="display:inline;"></asp:Label>                                                                                                 
                    </div>   
                    <div style="text-align:center;vertical-align:top;margin-top:10px;">                        
                        <asp:TextBox ID="TBSearch" AutoPostBack="true" runat="server" placeholder="Recherche..."  CssClass="form-control" Width="50%" style="float:right;margin-right:150px;display:inline;" ></asp:TextBox>                                                                                                                                           
                        <span class="icon" style="position:absolute;margin-left:21%;margin-top:0px;display:none" ><i class="fa fa-search"></i></span>
                    </div>   
                    </td>
                </tr>
               <tr runat="server" id="ROWAVERTISSEMENT" visible ="false">
                <td  >
                    <div style="width:500px;text-align:center;padding:2px 2px 2px 2px;border-radius: 4px;border:solid;border-width:1px;background-color:rgba(230, 222, 222,1);font-size:14px;font-weight:bold;vertical-align:top;margin-left:20px;margin-top:0px;">
                       <span style="color:rgba(149,107,24,1)">Attention ! </span><asp:Label ID="Label14" runat="server" Text="Inscrire uniquement des collaborateurs"  style="display:inline;"></asp:Label><span style="color:rgba(149,107,24,1)"> sans email ! </span>
                    </div>                      
                    </td>
                </tr>
            </div>             
               <tr>
                   <td>
                    <div id="DivAdmin1" runat="server">
                        <div style="text-align:left;vertical-align:top;margin-top:10px;">
                            <asp:Label ID="Label7" runat="server" Text="Collaborateur sans email" CssClass="LibelleTB" style="display:inline;"    ></asp:Label>                                                                                                 
                        </div>   
                        <div style="text-align:left;vertical-align:top;margin-top:10px;">
                            <asp:Label ID="Label8" runat="server" Text="Nom :" CssClass="LibelleTB" style="display:inline;margin-left:50px;"    ></asp:Label>                                                                         
                            <asp:TextBox ID="TBNom" AutoPostBack="true" runat="server" CssClass="form-control" Width="50%"  style="float:right;margin-right:85px;display:inline;" ></asp:TextBox>                                                                                                                                           
                        </div>   
                        <div style="text-align:left;vertical-align:top;margin-top:20px;">
                            <asp:Label ID="Label9" runat="server" Text="Prénom :" CssClass="LibelleTB" style="display:inline;margin-left:50px;"    ></asp:Label>                                                                         
                            <asp:TextBox ID="TBPrenom" AutoPostBack="true" runat="server"  CssClass="form-control" Width="50%"  style="float:right;margin-right:85px;display:inline;" ></asp:TextBox>                                                                                                                                           
                        </div>   
                     </div>
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label2" runat="server" Text="Responsable :" CssClass="LibelleTB" style="display:inline;margin-left:0px;"    ></asp:Label>                                                                         
                        <asp:TextBox ID="TBResponsable" AutoPostBack="true" runat="server"  CssClass="form-control" Width="50%"  style="float:right;margin-right:85px;display:inline;" ></asp:TextBox>                                                                                                                                           
                    </div>   
                    <div style="text-align:left;vertical-align:top;margin-top:20px;">
                        <asp:Label ID="Label11" runat="server" Text="Département :" CssClass="LibelleTB" style="display:inline;margin-left:0px;"    ></asp:Label>                                                                         
                        <asp:DropDownList ID="DDLDepartements" runat="server" CssClass="form-control" Width="50%"  style="float:right;margin-right:85px;display:inline;"></asp:DropDownList>                        
                    </div>  
                   <div id="DivAdmin2" runat="server">
                        <div style="text-align:left;vertical-align:top;margin-top:20px;">
                            <asp:Button ID="BtnAddStagiaire" runat="server" Text="Ajouter stagiaire" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                            <asp:Button ID="BtnCancelStagiaire" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push"  UseSubmitBehavior="false"  style="width:200px;margin-top:0px;margin-left:0px;display:inline;"/>
                        </div>                        
                    <div runat="server"  id="DivSession" style="background-color:rgba(230, 222, 222,1);width:75%;text-align:left;margin-top:5px" visible="false">
                        <span style="height:30px;vertical-align:middle;">
                        <asp:Label ID="LabelSession" runat="server" Text="" CssClass="LibelleTB" style="margin-left:40%;"></asp:Label>                                                                                                
                       </span>
                    </div>  
                    <div  runat="server" id="DivGenerique" style="width:75%;height:200px;text-align:left;vertical-align:top;margin-top:5px;">                    
                        <asp:GridView ID="GridView2" runat="server" AllowSorting="true" Caption="" DataKeyNames="Id,IsAlreadyRegistered" AutoGenerateColumns="false" CssClass="GridView" Width="100%" CellPadding="5" AllowPaging="true" PageSize="4">                                          
                             <Columns>            
                            <asp:BoundField  HeaderText="Nom" DataField="NomComplet" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Nom" />                                                                                                                                                                                
                            <asp:TemplateField  HeaderText="Dpt"  ItemStyle-Width="45%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Dpt" >
                                              <ItemTemplate>                                                      
                                                  <asp:Label runat="server" Text='<%# Eval("ShortDepartement") %>' ToolTip='<%# Eval("Departement") %>'></asp:Label>
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

                       <div runat="server" id="DivMesCollabos" style="width:75%;height:200px;text-align:left;vertical-align:top;margin-top:5px;overflow:auto">                    
                        <asp:GridView ID="GridView3" runat="server" AllowSorting="true" Caption="" DataKeyNames="Id,idSession,IsAlreadyRegistered" AutoGenerateColumns="false" CssClass="GridView" Width="100%" CellPadding="5" AllowPaging="false" PageSize="4">                                          
                             <Columns>                                        
                            <asp:TemplateField  HeaderText="Session"  ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Session" >
                                              <ItemTemplate>                                                      
                                                  <asp:Label runat="server" Text='<%# getSessionIndex(Eval("idSession").ToString()) %>'></asp:Label>
                                               </ItemTemplate>                                    
                            </asp:TemplateField>          
                            <asp:BoundField  HeaderText="Nom" DataField="NomComplet" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Nom" />                                                                                                                                                                                
                            <asp:TemplateField  HeaderText="Dpt"  ItemStyle-Width="45%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="Dpt" >
                                              <ItemTemplate>                                                      
                                                  <asp:Label runat="server" Text='<%# Eval("ShortDepartement") %>' ToolTip='<%# Eval("Departement") %>'></asp:Label>
                                               </ItemTemplate>                                    
                            </asp:TemplateField>          
                            <asp:TemplateField  HeaderText="Action" ItemStyle-Width="15%" >
                                               <ItemTemplate>                                     
                                                   <asp:ImageButton ID="BtnActivate" runat="server" CssClass="ImgActivate"  CommandName="Activate"   AlternateText="Supprimer" ToolTip="Supprimer"  OnClientClick="javascript: /*if(confirm('Confirmer vous la désactivation de la formation ?')){return true;}else return false;*/;" />                                                                                                     
                                               </ItemTemplate>
                                        </asp:TemplateField>          
                              </Columns>
                              <SelectedRowStyle CssClass="LigneSelected" />                   
                       </asp:GridView>
                    </div>  


              </div>
                    <div runat="server" id="DivBtnsValidation" style="width:auto;float:right;vertical-align:top;margin-top:10px;margin-right:50px;" visible="true">
                        <asp:Button ID="BtnInscrire" runat="server" Text="Inscrire" CssClass="btn btn-dore hvr-push"  UseSubmitBehavior="false"  style="width:100px;margin-top:0px;margin-left:0px;display:inline;"/>
                        <asp:Button ID="BtnCancel" runat="server" Text="Annuler" CssClass="btn btn-default hvr-push" UseSubmitBehavior="false"  style="width:100px;margin-top:0px;margin-left:10px;margin-right:20px;display:inline;"/>
                    </div>        
                </td>
            </tr>
        </table>    
                </td>
            </tr>
        </table>
        </div>                      
         <AjaxToolkit:AutoCompleteExtender   ID="AutoCompleteExtender1"  TargetControlID="TBSearch"   runat="server" CompletionInterval="0"  ServicePath="Services.aspx" ServiceMethod="GetListUsersNameWithEmail" MinimumPrefixLength="2"   EnableCaching="true"  CompletionListCssClass="CompletionList" CompletionListItemCssClass="CompletionListItem" CompletionListHighlightedItemCssClass="CompletionListHighlightedItem"  />                            
         <AjaxToolkit:AutoCompleteExtender   ID="AutoCompleteExtender2"  TargetControlID="TBREsponsable"   runat="server" CompletionInterval="0"  ServicePath="Services.aspx" ServiceMethod="GetListUsersNameWithEmail" MinimumPrefixLength="2"  EnableCaching="true"  CompletionListCssClass="CompletionList" CompletionListItemCssClass="CompletionListItem" CompletionListHighlightedItemCssClass="CompletionListHighlightedItem"  />                            
        </ContentTemplate>
        </asp:UpdatePanel>        
        <uc:UC_ErrorPanel ID="UC_ErrorPanel1" runat="server" />
    </div>
    <uc:UC_Footer  runat="server" ID="UC_Footer1"/>
    </form>
</body>
</html>
