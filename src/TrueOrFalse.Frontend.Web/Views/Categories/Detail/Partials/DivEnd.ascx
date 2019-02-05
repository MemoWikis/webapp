<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DivEndModel>" %>
    
        </ul>
    </div>
    <span markdown="<%: Model.Markdown %>"></span>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>