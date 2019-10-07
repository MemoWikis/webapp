<%@ Page Title="Draft" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <div class="row">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-3 col-xs-push-9 xxs-stack">
                    <div class="navLinks">
                        <a href="/Kategorien" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                        
                            <a href="/Bearbeite/115" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                        
                        <a href="/Fragen/Erstelle?categoryId=115" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
                    </div>
                </div>
                <div class="PageHeader col-xs-9 col-xs-pull-3 xxs-stack category">
                    
                    <%--<h3 class="CategoryType">Artikel: TestArtikel – Untertitel</h3>--%>
                    
                    <h2 style="margin-top: 0; margin-bottom: 10px;"><span class="ColoredUnderline Category"> TestArtikel – Untertitel</span> <span style="font-size: 80%;">(Artikel)</span></h2>
                    <div class="Reference DailyArticle">
                        <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                        <div class="Title"><span>TestArtikel – Untertitel</span></div>
                        <div class="Author"><span>von Grube, Claire;&nbsp;Kante, Anna</span></div>
                        <div class="ParentIssue">
                                <span>TestZeitung vom 01.07.2014</span><span class="Pages">
                                    (S. 1–2)</span></div>       
                        <div class="Url"><a href="zeitung.de/artikel"><span>zeitung.de/artikel</span></a></div><div class="Description"><span>Beschreibung lorem ipsum</span></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-3 col-xs-push-9 xxs-stack">
            <div class="CategoryImage">
                <img src="/Images/no-category-picture-350.png" class="img-responsive" style="-ms-border-radius:5px; border-radius:5px;">
                
            </div>
        </div>
        <div class="col-xs-9 col-xs-pull-3 xxs-stack">
            <div class="CategoryRelations well">
                
                    <h4 style="margin-top: 0;">Elternthemen</h4>
                    <div>
                        
                            <a href="/TestZeitung/71/1"><span class="label label-category">TestZeitung</span></a>
                        
                            <a href="/TestZeitung_vom_01072014/114/1"><span class="label label-category">TestZeitung vom 01.07.2014</span></a>
                        
                    </div>
                

                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
                <div class="MainCategory"><span class="label label-category">TestArtikel – Untertitel</span></div>
                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>

                
                    <h4 style="margin-top: 0;">keine Kindthemen</h4>
                
            </div>
            
        </div>         
    </div>

        
</asp:Content>