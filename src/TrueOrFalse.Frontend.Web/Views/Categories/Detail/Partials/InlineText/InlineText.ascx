<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <div v-if="textCanBeEdited">
        <inline-text-component/>
    </div>
    <div v-else @click="editInlineText()">
            <%: Html.Raw(HttpUtility.HtmlDecode(Model.Content))  %>
        <div v-if="!markdown && canBeEdited" style="text-align: center;color:#e3e3e3"> Hier klicken um Text zu bearbeiten</div>
    </div>
    
    <div class="Button Handle" v-if="hoverState">
        <i class="fa fa-bars"></i>
    </div>
                    
    <div class="Button dropdown" v-if="hoverState">
        <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
            <i class="fa fa-ellipsis-v"></i>
        </a>
        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="Dropdown" style="margin-top:-10px">
            <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</a></li>
            <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</a></li>
            <li class="delete"><a href="" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i> Löschen</a></li>
        </ul>
    </div>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>