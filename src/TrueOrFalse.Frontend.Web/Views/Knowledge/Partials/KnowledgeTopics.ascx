<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Styles.Render("~/bundles/KnowledgeTopics") %>


<script src="https://cdnjs.cloudflare.com/ajax/libs/vue-resource/1.3.4/vue-resource.common.js"></script>
<script type="text/javascript" src="http://cdn.jsdelivr.net/vue.table/1.5.3/vue-table.min.js"></script>








<div id="app">
    <table class="table">
        <tr>
            <th>Titel</th>
        </tr>
       <tr  v-for="(message, key) in messages">
           <td><image v-bind:src="message.ImageFrontendData.ImageMetaData.SourceUrl" style="width: 30px;"></image></td>
           <td>{{message.CategoryTitel}}</td>
           <td><div class="KnowledgeBarWrapper" v-html="message.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div></td>
           <td><a data-toggle="modal" v-bind:data-dateid="message.CategoryId" href="#" v-on:click="unpinCategory(message.CategoryId,key)">
                   <i class="fa fa-trash-o show-tooltip" title="" data-placement="top" data-original-title="Löschen">

                   </i> 
               </a>
               <!-- Dropdownmenu -->
               <div class="Button dropdown" style="transform: rotate(90deg);">
                   <% var buttonId = Guid.NewGuid(); %>
                   <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                       <i class="fa fa-ellipsis-v"></i>
                   </a>
                   <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>" style="transform: rotate(270deg)">
                       <% if (true)
                          { %>
                           <li><a href="#" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"><i class="fa fa-calendar"></i>&nbsp;Thema zum Termin lernen</a></li>         <%-- <%= Links.DateCreateForCategory(Model.Id) %>--%>
                        
                           <li><a href="#" rel="nofollow" data-allowed="logged-in" data-allowed-type="game"><i class="fa fa-gamepad"></i>&nbsp;Spiel starten</a></li>           <%-- <%= Links.GameCreateFromCategory(Model.Id) %>--%>
                       <% }
                          if(true){ %>
                           <li><a href="#"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>     <%--<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>--%>
                       <% }
                      if (Model.IsInstallationAdmin){ %>
                           <li><a href="#"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>      <%--<%= Links.CreateQuestion(categoryId: Model.Id) %>--%>
                       <% } %>
                   </ul>
               </div>
           </td>
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
            },
            unpinCategory(categoryId,key) {                
                console.log(key);
                $.post("/Api/Category/Unpin/", { categoryId: categoryId }, () => {
                    this.messages.splice(key, 1);
                });
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
                        console.log(Data[i].CategoryId);
                    }
                },
                error: function(error) {
                    console.log(error);
                }
            });
        }
    });
    
</script>


