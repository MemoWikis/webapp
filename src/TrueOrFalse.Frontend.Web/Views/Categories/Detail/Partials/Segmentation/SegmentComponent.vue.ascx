<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentModel>" %>

<segment-component inline-template :edit-mode="editMode" ref="segment<%= Model.Category.Id %>" title="<%= Model.Title %>" child-category-ids="<%= Model.ChildCategoryIds %>" category-id="<%= Model.Category.Id %>">
    <div class="segment" :data-category-id="categoryId" :data-child-category-ids="currentChildCategoryIds" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
        <div class="segmentSubHeader">
            <div class="segmentHeader">
                <div class="segmentTitle">
                    <h2>
                        <%= Model.Title %>
                    </h2>
                    <span class="Button Pin" data-category-id="<%= Model.Category.Id %>">
                        <a href="#" class="noTextdecoration" style="font-size: 18px; height: 10px;padding-right:4px">
                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.Category.IsInWishknowledge(), displayAdd:false)) %>
                        </a>
                    </span>
                </div>
                <div v-if="showHover" class="Button dropdown DropdownButton segmentDropdown">
                    <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li><a @click="removeChildren"><i class="fas fa-unlink"></i>&nbsp;Verknüpfungen entfernen</a></li>
                    </ul>
                </div>
            </div>
            
            <div class="segmentKnowledgeBar">
                <div class="KnowledgeBarWrapper">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                </div>
            </div>
        </div>
        <div class="topicNavigation" :key="cardsKey">
            <% foreach (var category in Model.ChildCategories) {%>
                <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
            <%} %>
            <div class="col-xs-6 addCategoryCard" @click="addCategory" :id="addCategoryId">
                <div>
                     <i class="fas fa-plus"></i> Neues Thema hinzufügen
                </div>
            </div>
        </div>
    </div>
</segment-component>
