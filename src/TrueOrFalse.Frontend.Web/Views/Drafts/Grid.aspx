<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<WelcomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <link href="/Style/site.css" rel="stylesheet" type="text/css" />
    <link href="/Views/Drafts/grid.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-9">
    
        <div class="row">

            <div class="col-md-7">
                <div class="box" style="min-height: 400px;"></div>
            </div>
            
            <div class="col-md-3">
                <div class="box" style="min-height: 200px;"></div>
            </div>
    
        </div>
        
        <div class="row">

            <div class="col-md-8">
                <div class="box" style="min-height: 400px;"></div>
            </div>
            
            <div class="col-md-2">
                <div class="box" style="min-height: 200px;"></div>
            </div>
    
        </div>
        
    </div>

</asp:Content>
