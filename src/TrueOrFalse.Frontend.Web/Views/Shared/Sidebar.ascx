<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="Sidebar">
    <div id="SidebarDivider">

    </div>
    <div id="SidebarContent">
        <div class="sidebar-card">
            <div class="overline-m no-line"><a class="" href="<%= Links.CategoryDetail("Zur Doku",RootCategory.IntroCategoryId) %>">Zur Doku</a><br/></div>
        </div>

        <div class="sidebar-card">
            <div class="overline-m no-line"><a href="https://discord.com/invite/nXKwGrN" target="_blank"><i class="fab fa-discord" aria-hidden="true">&nbsp;</i>Discord</a><br/></div>
            <div>
                <div class="body-m grey-darker">
                    Du willst dich mit uns unterhalten?
                    <br />
                    Dann triff dich mit uns auf <a href="https://discord.com/invite/nXKwGrN" target="_blank">Discord</a>!
                </div>
            </div>
        </div>
        
        <div id="SidebarIndex" class="sidebar-card"></div>
    </div>
</div>