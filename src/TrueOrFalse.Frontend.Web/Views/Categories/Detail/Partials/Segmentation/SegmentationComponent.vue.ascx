<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%= Styles.Render("~/bundles/Segmentation") %>

<segmentation-component inline-template :edit-mode="editMode" :category-id="<%= Model.Category.Id %>">
    <div :key="componentKey" id="Segmentation" v-cloak>
        <div v-if="hasCustomSegment" class="segmentationHeader overline-l">
            Alle Unterthemen
        </div>
        <div v-else class="segmentationHeader overline-l">
            Untergeordnete Themen
            <div v-if="editMode" class="Button dropdown DropdownButton segmentDropdown">
                <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                    <li><a @click="moveToNewCategory"><i class="fas fa-code-branch"></i>&nbsp;In neues Thema verschieben</a></li>
                    <li><a @click="removeChildren"><i class="fas fa-unlink"></i>&nbsp;Themen entfernen</a></li>
                </ul>
            </div>
        </div>

        <div id="CustomSegmentSection">
            <%if (Model.Segments != null  && Model.Segments.Any()) {
                  foreach (var segment in Model.Segments)
                  { %>
                    <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment)) %>
            <% }
              } %>
        </div>
        <div id="GeneratedSegmentSection" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
            <div class="segmentHeader" v-if="hasCustomSegment">
                <div class="segmentTitle">
                    <h2>
                        Weitere untergeordnete Themen
                    </h2>
                </div>
                <div class="Button dropdown DropdownButton segmentDropdown" :class="{ hover : showHover }">
                    <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li><a @click="moveToNewCategory"><i class="fas fa-code-branch"></i>&nbsp;In neues Thema verschieben</a></li>
                        <li><a @click="removeChildren"><i class="fas fa-unlink"></i>&nbsp;Themen entfernen</a></li>
                    </ul>
                </div>
            </div>
            <%if(!String.IsNullOrEmpty(Model.Title)){%>
                <h2><%: Model.Title %></h2>
            <% }
            if(!String.IsNullOrEmpty(Model.Text)){%>
                 <p><%: Model.Text %></p>
             <% } %>
        
                <div class="topicNavigation row">
                    <%if (Model.CategoryList.Any()) {
                      foreach (var category in Model.NotInSegmentCategoryList)
                      { %>
                        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
                    <% } %>
                    <%}else { %>
                        <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
                    <%} %>
                    <div id="AddToCurrentCategoryBtn" class="col-xs-6 addCategoryCard" @click="addCategory">
                        <div>
                             <i class="fas fa-plus"></i> Neues Thema hinzufügen
                        </div>                    
                    </div>
                </div>

        </div>
    </div>
</segmentation-component>
