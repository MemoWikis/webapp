<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSetRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase set-row" data-questionSetId="<%= Model.Id %>">
    <div class="column-1" style="line-height: 15px; font-size: 90%;">
        &nbsp;
    </div>
    
    <div class="column-2" style="height: 87px; position: relative;">
        <div style="font-size:large;">
            <a href="<%= Model.DetailLink(Url) %>"><%= Model.Name %></a>
        </div>
        Fragen: <%= Model.QuestionCount %>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:2px;">
            <% if (Model.IsOwner){%>
                <a data-toggle="modal" data-questionId="<%= Model.Id %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

                <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>">
                    <img src="/Images/edit.png"/> 
                </a>
            <% } %>
        </div>        

        <div style="text-align: right; width: 150px; position: absolute; bottom:0px; right: 10px;">
            von <a href="<%= Model.UserProfileLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" data-original-title="<%=Model.CreatorName %>">
                    <%=Model.CreatorName %>
                </a>
        </div>
    </div>
    
    <div class="column-3">
    </div>
</div>