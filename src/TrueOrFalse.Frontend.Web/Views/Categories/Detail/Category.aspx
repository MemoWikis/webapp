<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<CategoryModel>" Title="Kategorie" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="span2">
        <div>
            <div class="box">
                <img src="<%= Model.ImageUrl %>"/>
            </div>
        </div>
    </div>
            
    <div class="span7 category">
        <h2 class="pull-left"><%= Model.Name %></h2>
        <div class="pull-right">
            <div>
                <a href="/Categories" style="font-size: 12px; margin: 0px;"><i class="icon-list"></i>&nbsp;zur Übersicht</a><br/>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="icon-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
            </div>
        </div>
        <div style="clear: both;">
            <%= Model.Description %>
        </div>
    </div>
    
    <div class="span10">    
        <div style="height: 200px; ">
            <div class="column">
                <h3>Fragen (175)</h3>
                <table>
                    <tr>
                        <td>Wann wurde xy geboren </td> 
                        <td>14x <span id="question-1"></span> </td>
                    </tr>
                </table>
            </div>
        
            <div class="column">
                <h3>Fragesätze (14)</h3>
                <div>
                    <span>Noch keine Fragesätze</span>
                </div>
            </div>
 
            <div class="column">
                <h3>Ersteller (14)</h3>
                <table>
                    <tr>
                        <td>Musik</td> 
                        <td>72</td> 
                        <td><span id="inCategory-1"></span></td>
                        <td><span id="inCategoeryOverTime-1"></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Content>