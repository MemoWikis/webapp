<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="col-md-6"> 
    <div class="rowBase category-row">
    
        <div class="column-Image">
            <div class="ImageContainer ShortLicenseLinkText">
                <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Model.DetailLink(Url)) %>
            </div>
        </div>
    
        <div class="column-MainContent">
            <div class="MainContentUpper">
                <div class="TitleText" style="line-height: 21px; margin-top: 6px;">
                    
                    <div class="Pin" data-category-id="<%= Model.CategoryId %>">
                        <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </a>
                    </div>
                    
                    <a href="<%= Model.DetailLink(Url) %>">
                        <% if (Model.HasMarkdownContent) { %>
                            <i class="fa fa-star show-tooltip" data-original-title="Themenseite mit zusätzlichen Inhalten">&nbsp;</i><% 
                        }
                        if (Model.IsMediaCategory)
                        {%>
                            <i class="fa fa-book show-tooltip" data-original-title="Medium: <%= Model.CategoryTypeName %>")>&nbsp;</i><%}
                        if (Model.IsEducationCategory)
                        {%>
                            <i class="fa fa-graduation-cap show-tooltip" data-original-title="Bildungsstufe: <%= Model.CategoryTypeName %>")>&nbsp;</i><%
                        }
                        %><%=Model.CategoryName.TruncateAtWord(55) %>
                    </a> 
                </div>
            
                <div style="margin-top: 1px;">
                    <%--<a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow">Enthält <%= Model.QuestionCount + " Frage" + StringUtils.PluralSuffix(Model.QuestionCount, "n") %></a>--%>
                    <%= Model.QuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %> und <%= Model.SetCount %> Frage<%= StringUtils.PluralSuffix(Model.SetCount, "sätze", "satz") %>
                </div>
            </div>

            <div class="MainContentLower">

                <% if(Model.UserCanEdit){ %>
                    <span style="font-size: small; position: relative; top: 4px;">
                        <a data-toggle="modal" data-categoryId="<%= Model.CategoryId %>" href="#modalDeleteCategory"><i class="fa fa-trash-o"></i></a>

                        <a href="<%= Links.CategoryEdit(Url, Model.CategoryName, Model.CategoryId) %>" style="margin-right: 10px;">
                            <i class="fa fa-pencil"></i> 
                        </a>
                    </span>
                <% } %>
            
                <span class="show-tooltip" title="erstellt: <%= Model.DateCreatedLong %>" style="font-size: small; position: relative; top: 4px;">
                    Erstellt am <%= Model.DateCreated %>
                </span>
                <div style="float: right;">
                    <a class="btn btn-primary btn-sm <%= Model.QuestionCount == 0 ? "disabled " : "" %>show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.CategoryName,Model.CategoryId) %>" title="Teste dein Wissen in diesem Thema" rel="nofollow">
                        <i class="fa fa-play-circle">&nbsp;</i> Wissen testen
                    </a>
                </div>
                <div class="clearfix"></div>
            </div>
        
            <%= Model.DescriptionShort %>

        </div>
    </div>
</div>