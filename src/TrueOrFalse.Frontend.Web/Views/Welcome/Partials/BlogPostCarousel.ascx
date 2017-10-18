<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<IList<WordpressBlogPost>>" %>

<div id="carouselBlogposts" class="carousel slide" data-ride="carousel" data-interval="false">
    <ol class="carousel-indicators">
        <% for (int i = 0; i < Model.Count; i++) { %>
            <li data-target="#carouselBlogposts" data-slide-to="<%= i %>" <%= i==0 ? "class=\"active\"" : "" %>></li>
        <% } %>
    </ol>
    <div class="carousel-inner" role="listbox">
        <% 
        var firstIsShown = false;
        foreach (var post in Model) { %>
            <div class="item <%= firstIsShown ? "" : "active" %>">
                <div class="row">
                    <div class="col-sm-5 carousel-img">
                        <img class="" src="<%= post.FeaturedImageMediumSizedUrl %>">
                        <p class="img-caption Wrap"><%= Regex.Replace(post.FeaturedImageCaption, "<.*?>", String.Empty) %></p>
                    </div>
                    <div class="col-sm-7 carousel-text">
                        <p class="authorInfo"><%= post.DateCreated.ToString("dd.MM.yyy") %> | <%= post.AuthorName %></p>
                        <h4><%= post.Title %></h4>
                        <p><%= post.Excerpt.TruncateAtWordWithEllipsisText(300, "...") %></p>
                        <div class="blogpostButtons">
                            <a href="<%= post.Url %>" target="_blank" class="btn btn-lg btn-primary">Weiterlesen</a>
                            <a href="http://blog.memucho.de" target="_blank" class="btn btn-lg btn-default">Zum memucho-Blog</a>
                        </div>
                    </div>
                </div>
            </div>
                   
        <% 
            firstIsShown = true;
        } %>
    </div>
    <a class="left carousel-control" href="#carouselBlogposts" role="button" data-slide="prev">
        <span class="glyphicon glyphicon-chevron-left"></span>
        <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#carouselBlogposts" role="button" data-slide="next">
        <span class="glyphicon glyphicon-chevron-right"></span>
        <%--<i class="fa fa-chevron-right"></i>--%>
        <span class="sr-only">Next</span>
    </a>
</div>
