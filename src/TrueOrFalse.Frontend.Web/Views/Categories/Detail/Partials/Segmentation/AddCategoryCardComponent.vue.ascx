<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<add-category-card-component inline-template :category-id="categoryId">
    <div>
        <div class="addCategoryCard">

        </div>
        <div class="categoryCardModal"></div>
    </div>
</add-category-card-component>
