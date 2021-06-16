<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%= Styles.Render("~/bundles/Segmentation") %>

<segmentation-component inline-template :edit-mode="editMode" :category-id="<%= Model.Category.Id %>" child-category-ids="<%= Model.NotInSegmentCategoryIds %>" segment-json="<%= Model.SegmentJson %>" is-my-world-string="<%= Model.IsMyWorld %>">
    <div :key="componentKey" id="Segmentation" v-cloak>
        <div class="segmentationHeader overline-m">
            Untergeordnete Themen
            <div v-if="editMode" class="Button dropdown DropdownButton segmentDropdown">
                <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                    <li><a @click="moveToNewCategory">
                        <div class="dropdown-icon"><i class="fas fa-code-branch"></i></div>In neues Thema verschieben
                    </a></li>
                    <li><a @click="removeChildren">
                        <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Themen entfernen
                    </a></li>
                </ul>
            </div>
        </div>

        <div id="CustomSegmentSection" v-if="loadComponents" v-cloak>
                <template v-for="s in segments">
                    <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx") %>
                </template>
        </div>
        <div id="CustomSegmentSection" v-else>
            <%if (Model.Segments != null  && Model.Segments.Any()) {
                      foreach (var segment in Model.Segments)
                      { %>
                        <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.Placeholder.ascx", new SegmentModel(segment)) %>
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
                        <li><a @click="moveToNewCategory">
                            <div class="dropdown-icon"><i class="fas fa-code-branch"></i></div>In neues Thema verschieben
                        </a></li>
                        <li><a @click="removeChildren">
                            <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Themen entfernen
                        </a></li>
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
                    <template v-if="loadComponents" v-cloak>
                        <template v-for="id in currentChildCategoryIds">
                            <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx")%>
                        </template>
                    </template>
                    <%if (Model.CategoryList.Any()) { %>
                        <template v-else>
                            <% foreach (var category in Model.NotInSegmentCategoryList) {%>
                                <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.Placeholder.ascx", new SegmentationCategoryCardModel(category)) %>
                            <%} %>
                        </template>                    <%}else { %>
                        <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
                    <%} %>
                    <div id="AddToCurrentCategoryBtn" class="col-xs-6 addCategoryCard memo-button" @click="addCategory">
                        <div>
                             <i class="fas fa-plus"></i> Neues Thema hinzufügen
                        </div>                    
                    </div>
                </div>

        </div>
    </div>
</segmentation-component>
