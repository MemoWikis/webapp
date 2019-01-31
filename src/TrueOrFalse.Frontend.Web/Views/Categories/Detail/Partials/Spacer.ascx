<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SpacerModel>" %>

<content-module inline-template >
    <li class="module" markdown="<%: Model.Markdown %>" v-if="!isDeleted">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">
                
                <% for (var i = 0; i < Model.AmountSpaces; i++) { %>
                    <div class="SpacerDiv20<%= i == 0 && Model.AddBorderTop ? " SpacerBorderTop" : "" %>">
                    </div>       
                <% } %>

            </div>    
        </div>     
    </li>        
</content-module> 
