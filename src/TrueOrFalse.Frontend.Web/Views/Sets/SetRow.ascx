<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase set-row" data-questionSetId="<%= Model.Id %>">
    <div class="column-1" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="85"/>
    </div>
    
    <div class="column-2" style="height: 87px; position: relative;">
        <div style="font-size:large;">
            <% if(Model.QuestionCount != 0){ %>
                (<%= Model.QuestionCount %>)
            <% }else{ %>
                <span style="color: darkgray">(0)</span>
            <% } %>            
            <a href="<%= Model.DetailLink(Url) %>"><%= Model.Name %></a>
        </div>

        <div>
            <% foreach (var category in Model.Categories){ %>
                <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category"><%= category.Name %></span></a>    
            <% } %>
        </div>
        
        <%= Model.DescriptionShort %>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:-3px;">
            <% if (Model.IsOwner){%>
                <a data-toggle="modal" data-SetId="<%= Model.Id %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>

                <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>">
                    <img src="/Images/edit.png"/> 
                </a>
            <% } %>
        </div>
    </div>
    
    <div class="column-3" style="position: relative; ">
        
        <div style="float: right; position:relative;  padding-bottom:2px; padding-top:0px;  <% if(Model.RelevancePersonal == -1){ %>display:none<% } %>" class="sliderContainer">
            <div class="slider ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 90px; margin-left:5px; float: left;" data-setId="<%= Model.Id %>"> 
                <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
            </div>
            <div style="float:left; margin-top: -2px" class="sliderAnotation">
                <a href="#"><span class="sliderValue"><%= Model.RelevancePersonal %></span></a> <a href="#" class="removeRelevance"><i class="fa fa-minus"></i></a>
            </div>
        </div>
        <a href="#" class="addRelevance" style="<% if(Model.RelevancePersonal != -1){ %>display:none;<% } %>; float: right;" ><i class="fa fa-plus-circle "></i> merken</a>
        
        <br />
        <%if(Model.TotalRelevancePersonalEntries != "0"){ %>
            <div style="margin-top: 2px; float:right; width: 150px; text-align: right;">
                <span class="totalRelevanceEntries"><%= Model.TotalRelevancePersonalEntries %></span> x 
                <a href="">gemerkt (&#216;   <span class="totalRelevanceAvg"><%= Model.TotalRelevancePersonalAvg %></span></a> <span class="piePersonalRelevanceTotal" data-avg="<%= Model.TotalRelevancePersonalAvg  %>"></span> )
            </div>
        <%} %>
        
        <div style="position: absolute; bottom:10px; right: 0px;">
            von <a href="<%= Model.UserLink(Url)  %>" class="userPopover" rel="popover" data-creater-id="<%= Model.CreatorId %>" data-original-title="<%=Model.CreatorName %>">
                    <%= Model.CreatorName %>
                </a>
        </div>

    </div>
</div>