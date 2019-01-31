<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<VideoWidgetModel>" %>

<content-module inline-template >
    <li class="module" markdown="<%: Model.Markdown %>" v-if="!isDeleted">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">
                
                <script src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="<%= Model.SetId %>" data-width="100%"></script>

            </div>    
        </div>     
    </li>        
</content-module> 

