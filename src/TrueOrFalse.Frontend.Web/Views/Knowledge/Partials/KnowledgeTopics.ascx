<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.View.Web.Views.Knowledge.Partials.KnowledgeTopicsModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%= Styles.Render("~/bundles/KnowledgeTopics") %>


<script src="https://cdnjs.cloudflare.com/ajax/libs/vue-resource/1.3.4/vue-resource.common.js"></script>

<script type="text/javascript" src="http://cdn.jsdelivr.net/vue.table/1.5.3/vue-table.min.js"></script



    <div class="row">
        <div id="chartKnowledgeH1" class="col-md-6 heading-chart-knowledge"></div>
       <div class="col-md-6"><h1>Deine Wissenszentrale</h1></div>
  </div>

  <div id="wishKnowledge" class="row">
            <div class="col-lg-12">
                <h3>Themen und Lernsets in deinem Wunschwissen</h3>
                
                
                    <div class="alert alert-info" id="noWishKnowledge" style="max-width: 600px; margin: 30px auto 10px auto; display:none ">
                        <p>
                            Du hast keine Themen oder Lernsets in deinem Wunschwissen. Finde interessante Themen aus den Bereichen 
                            <a href="<%= Links.CategoryDetail("Schule", 682) %>">Schule</a>,
                            <a href="<%= Links.CategoryDetail("Studium", 687) %>">Studium</a>,
                            <a href="<%= Links.CategoryDetail("Zertifikate", 689) %>">Zertifikate</a> und 
                            <a href="<%= Links.CategoryDetail("Allgemeinwissen", 709) %>">Allgemeinwissen</a>
                            und füge sie deinem Wunschwissen hinzu. Dann hast du deinen Wissensstand hier immer im Blick.
                        </p>
                    </div>
                
                <div class="row wishKnowledgeItems">
                    <% foreach (var catOrSet in Model.CatsAndSetsWish)
                        {
                            if (Model.CatsAndSetsWish.IndexOf(catOrSet) == 6 && Model.CatsAndSetsWish.Count > 8)
                            { %>
                                </div>
                                <div id="wishKnowledgeMore" class="row wishKnowledgeItems" style="display: none;">
                            <% } %>
                            <div class="col-xs-12 topic">
                                <% if (catOrSet is Category)
                                    { %>
                                       <% Html.RenderPartial("Partials/KnowledgeCardMiniCategory", new KnowledgeCardMiniCategoryModel((Category)catOrSet)); %>
                                <% }
                                    else if (catOrSet is Set)
                                    { %>
                                    <% Html.RenderPartial("Partials/KnowledgeCardMiniSet", new KnowledgeCardMiniSetModel((Set)catOrSet)); %>
                                <% } %>
                            </div>
                    <% } %>
                </div>
                <% if (Model.CatsAndSetsWish.Count > 8)
                    { %>
                    <div>
                        <a href="#" id="btnShowAllWishKnowledgeContent" class="btn btn-link btn-lg">Alle anzeigen (<%= Model.CatsAndSetsWish.Count-6 %> weitere) <i class="fa fa-caret-down"></i></a> 
                        <a href="#" id="btnShowLessWishKnowledgeContent" class="btn btn-link btn-lg" style="display: none;"> <i class="fa fa-caret-up"></i> Weniger anzeigen</a>
                    </div>
                <% } %>
            </div>
        </div>




<div id="app">
    <table class="table">
        <tr>
            <th>Titel</th>
        </tr>
       <tr  v-for="message in messages">
           <td><image v-bind:src="message.ImageFrontendData.ImageMetaData.SourceUrl" style="width: 30px;"></image></td>
           <td>{{message.CategoryTitel}}</td>
           <td><div class="KnowledgeBarWrapper" v-html="message.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div></td>
       </tr>
        

    </table>
</div>

<script>
    new Vue({
        el: '#app',
        data: {
            messages: []
        },methods: {
            mouseOver(){
                $('.show-tooltip').tooltip();   
            }
        },
        mounted: function() {
            var self = this;

            $.ajax({
                url: '/Knowledge/getCatsAndSetsWish',
                method: 'POST',
                datatype: "jsonp",
                success: function(Data) {
                    self.messages = Data;
                    for (var i = 0; i < Data.length;i++){
                        console.log(Data[i].KnowlegdeWishPartial);
                    }
                  
                    
                },
                error: function(error) {
                    console.log(error);
                }
            });
        }
    
    

    });
    
</script>
