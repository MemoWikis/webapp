<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<script>
    google.load("visualization", "1", { packages: ["corechart"] });

        // Heading h1 Knowledgewheel 
        google.setOnLoadCallback(chartKnowledgeH1);

        function chartKnowledgeH1() {

            var options = {
                pieHole: 0.6,
                tooltip: { isHtml: false },
                legend: { position: "none" },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10 },
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver' },
                    4: { color: '#dddddd' }
                },
                pieStartAngle: 0
            }

            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'link', 'Anteil in %'],
                ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', <%= Model.KnowledgeSummary.Solid %>],
                ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', <%= Model.KnowledgeSummary.NeedsConsolidation %>],
                ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', <%= Model.KnowledgeSummary.NeedsLearning %>],
                ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', <%= Model.KnowledgeSummary.NotLearned %>],
                ['Nicht im Wunschwissen', '', <%= Model.KnowledgeSummary.NotInWishknowledge %>]
            ]);
            
            var view = new google.visualization.DataView(data);
            view.setColumns([0, 2]);

            var chart = new google.visualization.PieChart(document.getElementById('chartKnowledgeH1'));
            chart.draw(view, options);

            google.visualization.events.addListener(chart, 'select', selectHandler);

            function selectHandler() {
                var urlPart = data.getValue(chart.getSelection()[0].row, 1);
                location.href = urlPart;
            }
    }
               
</script>

    <div class="row">
        <div id="chartKnowledgeH1" class="col-md-6 heading-chart-knowledge"></div>
        <%--<div class="col-md-6"><h1>Deine Wissenszentrale</h1></div>--%>
    </div>

     <div id="wishKnowledge" class="row">
            <div class="col-lg-12">
                <h3>Themen und Lernsets in deinem Wunschwissen</h3>
                
                <% if (!Model.CatsAndSetsWish.Any())
                    { %>
                    <div class="alert alert-info" style="max-width: 600px; margin: 30px auto 10px auto;">
                        <p>
                            Du hast keine Themen oder Lernsets in deinem Wunschwissen. Finde interessante Themen aus den Bereichen 
                            <a href="<%= Links.CategoryDetail("Schule", 682) %>">Schule</a>,
                            <a href="<%= Links.CategoryDetail("Studium", 687) %>">Studium</a>,
                            <a href="<%= Links.CategoryDetail("Zertifikate", 689) %>">Zertifikate</a> und 
                            <a href="<%= Links.CategoryDetail("Allgemeinwissen", 709) %>">Allgemeinwissen</a>
                            und füge sie deinem Wunschwissen hinzu. Dann hast du deinen Wissensstand hier immer im Blick.
                        </p>
                    </div>
                <% } %>
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
