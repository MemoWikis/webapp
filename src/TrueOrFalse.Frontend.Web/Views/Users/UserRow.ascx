<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase user-row <%= Model.IsCurrentUser ? "loggedInUser"  : "" %>" data-questionSetId="<%= Model.Id %>">
    <div class="column-1" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="85"/>
    </div>
    
    <div class="column-2" style="width: 635px; height: 87px; position: relative;">
        <div style="font-size:large;">
            <a href="<%= Model.UserLink(Url) %>"><%= Model.Name %></a>
            <span style="font-size: small;">(<%= Model.QuestionCount %> Fragen)</span>
        </div>
        
        <%= Model.DescriptionShort %>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:2px;">
            
            <a data-toggle="modal" data-questionId="<%= Model.Id %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

            <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>">
                <img src="/Images/edit.png"/> 
            </a>
        
            <% if(!Model.IsCurrentUser){ %>
                <a href=""> <i class=" icon-share-alt"></i> </a>
            <%} %>
            
        </div>        

        <div style="text-align: right; width: 150px; position: absolute; bottom:0px; right: 10px; ">
        </div>
    </div>
    
    <div class="column-3">
    </div>
</div>