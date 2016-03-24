<%@ Page Title="Mein Wissensstand" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Register Src="~/Views/Knowledge/TrainingDate.ascx" TagPrefix="uc1" TagName="TrainingDate" %>


<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        $(function () {
            $("#menuWishKnowledgeCount").html("134");
            $(".userName b").html("Pauline");
            $(".userImage").attr("src", "https://memucho.de/Images/Users/32_250.jpg?t=20150102040542");
            $(".list-group-item.sub").remove();

            var titles = ['Sicheres Wissen', 'Sollte gefestigt werden', 'Sollte dringend gelernt werden', 'Noch nie gelernt'];
            $("#totalKnowledgeSpark")
                .sparkline(
                    [<%= Model.KnowledgeSummary.Solid %>, <%= Model.KnowledgeSummary.NeedsConsolidation %>, <%= Model.KnowledgeSummary.NeedsLearning %>, <%= Model.KnowledgeSummary.NotLearned %>],
                    {
                        type: 'pie',
                        sliceColors: ['#3e7700', '#fdd648', '#B13A48', '#EFEFEF'],
                        tooltipFormat: '{{offset:slice}} {{value}} ({{percent.1}}%)',
                        tooltipValueLookups: {'slice': titles},
                    }
                );

            $("#totalKnowledgeOverTimeSpark").sparkline([5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5], {
                type: 'line',
                witdh: '250'
            });

            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#3e7700', '#B13A48'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
        });
    </script>
    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate1", 9, 2, 1, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate2", 4, 3, 2, 3) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate3", 1, 12, 4, 12) });
        google.setOnLoadCallback(drawActivityChart);

        //chartKnowledgeDate
        function drawKnowledgeChart(chartElementId) {

            if ($("#" + chartElementId).length === 0) {
                return;
            }

            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'link', 'Anteil in %'],
                ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', 24],
                ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', 21],
                ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', 10],
                ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', 5],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'labeled' },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10},
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver'}
                },
                pieStartAngle: 0
            };

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 2]);

            var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
            chart.draw(view, options);

            google.visualization.events.addListener(chart, 'select', selectHandler);

            function selectHandler(e) {
                var urlPart = data.getValue(chart.getSelection()[0].row, 1);
                location.href = urlPart;
            }
        }

        function drawKnowledgeChartDate(chartElementId, amountSolid, amountToConsolidate, amountToLearn, amountNotLearned) {

            var chartElement = $("#" + chartElementId);

            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'Anteil in %'],
                ['Sicheres Wissen', amountSolid],
                ['Solltest du festigen', amountToConsolidate],
                ['Solltest du lernen', amountToLearn],
                ['Noch nicht gelernt', amountNotLearned],
            ]);

            var options = {
                pieHole: 0.5,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 80,
                chartArea: { width: '90%', height: '90%', top: 0 },
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver' }
                },
                pieStartAngle: 0
            };

            var chart = new google.visualization.PieChart(chartElement.get()[0]);
            chart.draw(data, options);
        }

        function drawActivityChart() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Richtig beantwortet', 'Falsch beantwortet', { role: 'annotation' }
                ],
                ['10.02', 4, 2, ''], 
                ['11.02', 0, 0, ''], 
                ['12.02', 0, 0, ''], 
                ['13.02', 7, 8, ''], 
                ['14.02', 0, 0, ''], 
                ['15.02', 28, 25, ''], 
                ['16.02', 0, 0, ''], 
                ['17.02', 0, 0, ''], 
                ['18.02', 34, 20, ''], 
                ['19.02', 41, 10, ''], 
                ['20.02', 45, 3, ''], 
                ['21.02', 2, 0, ''], 
                ['22.02', 0, 0, ''], 
                ['23.02', 0, 0, ''], 
                ['24.02', 0, 0, ''], 
                ['25.02', 12, 12, ''], 
                ['26.02', 0, 0, ''], 
                ['27.02', 0, 0, ''], 
                ['28.02', 12, 1, ''], 
                ['29.02', 4, 0, ''], 
                ['01.03', 18, 15, ''], 
                ['02.03', 25, 8, ''], 
                ['03.03', 0, 0, ''], 
                ['04.03', 0, 0, ''], 
                ['05.03', 0, 0, ''], 
                ['06.03', 12, 16, ''], 
                ['07.03', 14, 10, ''], 
                ['08.03', 16, 9, ''], 
                ['09.03', 21, 1, ''], 
                ['10.03', 23, 0, ''],
            ]);

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 1,
                {
                    calc: "stringify",
                    sourceColumn: 1,
                    type: "string",
                    role: "annotation"
                },
                2]);

            var options = {
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534', 'lightsalmon'],
                isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartActivityLastDays"));
            chart.draw(view, options);
            <%--<% if (!Model.HasLearnedInLast30Days) { %>
                var infoDivNotLearned = document.createElement('div');
                infoDivNotLearned.innerHTML = '<p>Du hast in den letzten 30 Tagen keine Fragen beantwortet, daher kann hier keine Übersicht angezeigt werden.</p>'
                infoDivNotLearned.setAttribute('style', 'position: absolute; background-color: white; top: 145px;')
                document.getElementById("chartActivityLastDays").appendChild(infoDivNotLearned);
                document.getElementById("chartActivityLastDays").style.opacity = 0.5;
            <% } %>--%>

        }
    </script>
    
    <style>
        #totalKnowledgeOverTime{font-size: 18px; line-height:27px ;color: rgb(170, 170, 170);padding-top: 5px;display: inline-block;}
        #totalKnowledgeOverTimeSpark{ display: inline-block;}
        div.answerHistoryRow div{ display: inline-block; height: 22px;}
        div.answerHistoryRow .answerAmount{ color:blue; font-weight: bolder;}
        
        div.column  { width: 260px; float: left;}

        div.percentage{display: inline-block; width: 40px; background-color:beige; height: 22px;vertical-align: top;}
        div.percentage span{ font-size: 22px; color: green; position: relative; top: 2px; left: 4px;}
    </style>

    <%= Styles.Render("~/bundles/Knowledge") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.Message != null) { %>
        <div class="row">
            <div class="col-xs-12 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>        
    <% } %>

    <h2 style="color: black; margin-bottom: 5px; margin-top: 0px;"><span class="ColoredUnderline Knowledge">Hallo Pauline!</span></h2>
        
    <p style="margin-bottom: 12px;">Hier erhältst du eine Übersicht über dein Wunschwissen und deinen Wissensstand.</p>

    <% if(!Model.IsLoggedIn){ %>

        <div class="bs-callout bs-callout-danger">
            <h4>Anmelden oder registrieren</h4>
            <p>Um Wunschwissen zu verwenden, musst du dich <a href="/Anmelden">anmelden</a> oder <a href="/Registrieren">registrieren</a>.</p>
        </div>

    <% }else{  %>
    
        <%--<div class="row">
            <div class="col-md-12 col-cs-12">--%>
                <div class="number-box-reputation" style="float: left;">
                    <a href="<%= Links.UserDetail(Model.UserName, Model.UserId) %>">
                        <div style="padding-left: 14px; padding: 8px;">                        
                            <span>Reputation: <b><%= Model.ReputationTotal %></b>,</span>
                            <span>Rang: <b><%= 4 %></b>,</span>
                            erstellte Fragen: <b>49</b>,
                            erstellte Fragesammlungen: <b>3</b>
                        </div>
                    </a>
                </div>
           <%-- </div>
        </div> --%>     
        <div style="clear: left;"></div>   

        <p style="">
           <%-- Du hast momentan <%= Model.ReputationTotal %> Reputationspunkte <i class="fa fa-info-circle show-tooltip" title="Du gewinnst Reputationspunkte z.B., indem du gute Fragen, Fragesätze etc. erstellst. In der Hilfe erfährst du mehr."></i>
            (Rang 4) --%>
            &nbsp; <a href="<%= Links.UserDetail(Model.User) %>" style="font-size: 12px;">Details auf deiner Profilseite</a>
        </p>
        <div style="clear: left;">Erstelle noch 1 Frage und du erhältst die Trophäe "MehrAlsTausendWorte".</div>

    
        <div class="row" style="margin-top: 10px;">
            <div class="col-xs-12 col-md-6">
                <h3 style="margin-bottom: 3px; margin-top: 0;">Dein Wissensstand</h3>
                <p style="margin-bottom: 1px; font-size: 12px; color: silver;">Im Wunschwissen:</p>
                <%--<p>
                    In deinem Wunschwissen sind <%= Model.QuestionsCount %> Frage<%= StringUtils.Plural(Model.QuestionsCount,"n","","n") %> und <%= Model.SetCount %> Frage<%= StringUtils.Plural(Model.SetCount,"sätze","satz","sätze") %>. 
                    <i class="fa fa-info-circle show-tooltip" title="Erweitere dein Wunschwissen, indem du auf das Herz-Symbol neben einer Frage oder einem Fragesatz klickst."></i>
                </p>--%>
                <div class="row" style="line-height: 30px; margin-bottom: 20px;">
                    <div class="col-md-6">
                        <div class="number-box-questions" style="text-align: center;">
                            <a href="<%= Links.QuestionsWish() %>">
                                <div>
                                    <span style="font-weight: 900; font-size: 20px;"><%= 134 %></span>
                                    <span style="font-size: 14px">Fragen</span>
                                </div>
                            </a>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="number-box-sets" style="text-align: center;">
                            <a href="<%= Links.SetsWish() %>">
                                <div>
                                    <span style="font-weight: 900; font-size: 20px;"><%= 8 %></span>
                                    &nbsp;<span style="font-size: 14px">Fragesammlungen</span>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
                <% if(Model.KnowledgeSummary.Total == 0) { %> <%--Warum nicht "if sets == 0 & questions == 0 then"?--%>
                        <p>
                            MEMuchO kann deinen Wissensstand nicht zeigen, da du noch kein Wunschwissen hast.
                        </p>
                        <p>
                            Um dein Wunschwissen zu erweitern, suche dir interessante <a href="<%= Links.QuestionsAll() %>">Fragen</a>  
                            oder <a href="<%= Links.Sets() %>">Fragesätze</a> aus und klicke dort auf das Herzsymbol:
                            <ul style="list-style-type: none">
                                <li>
                                    <i class="fa fa-heart show-tooltip" style="color:#b13a48;" title="" data-original-title="In deinem Wunschwissen"></i>
                                    In deinem Wunschwissen
                                </li>                                
                                <li>
                                    <i class="fa fa-heart-o show-tooltip" style="color:#b13a48;" title="" data-original-title="Nicht Teil deines Wunschwissens."></i>
                                    <i>Nicht</i> in deinem Wunschwissen.
                                </li>
                            </ul>
                            
                        </p>
                <% }else { %>
                    <div id="chartKnowledge" style="margin-right: 20px; text-align: left;"></div>
                <% } %>
            </div>

            <div class="col-xs-12 col-md-6">

                <h3 style="margin-top: 0;">Training <span style="font-size: 12px; color: silver; padding-left: 15px;">letzte 30 Tage</span></h3>
                
                <div id="chartActivityLastDays" style="margin-right: 20px; text-align: left;"></div>
                
                <div class="row" style="font-size: 12px">
                    <div class="col-md-12">
                        <% var streak = Model.StreakDays; %>
                        <!-- -->
                        <span style="display: inline-block; width: 40%">Lerntage gesamt: 
                            <b><%= streak.TotalLearningDays %></b></span> <span style="color: silver; font-weight: bold;">
                            seit 27.09.2015
                        </span><br />
                        
                        <!-- LongestStreak -->
                        <span style="display: inline-block; width: 40%">
                            Längste Folge: <b>11</b>
                        </span>
                        <% if (streak.LongestLength == 0){ %>
                            <span style="color: silver; font-weight: bold;">zuletzt nicht gelernt</span>
                        <% } else { %>
                            <span style="color: silver; font-weight: bold;">
                                14.01 - 24.01.2016
                            </span><br />
                        <% } %>
                        
                        <!-- CurrentStreak -->
                        <span style="display: inline-block; width: 40%">
                            Aktuelle Folge: <b>5</b>
                        </span>
                        <% if (streak.LastLength == 0){ %>
                            <span style="color: silver; font-weight: bold;">zuletzt nicht gelernt</span>
                        <% } else { %>
                            06.03 - 10.03.2016
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    
        <%--<div class="row" style="margin-top: 20px">
            <div class="col-md-6 col-xs-12">
                <div class="row">                   
                    <div class="col-md-5 col-cs-12 hidden-sm hidden-xs" style="padding: 10px;">
                        <h3 style="display:inline">
                            Wunschwissen:
                        </h3>
                    </div>
                    
                </div>
            </div>
            
        </div>--%>

        <div class="row" style="margin-top: 20px;">
            <div class="col-xs-12 col-md-4" style="padding: 5px;">
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0;">Termine</h3>
                    <%--<% if (Model.Dates.Count ==0) { %>
                        <p>
                            Du hast momentan keine offenen Termine. Termine helfen dir dabei, dich optimal auf eine Prüfung vorzubereiten.
                        </p>
                        <p>
                            <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm">
                                <i class="fa fa-plus-circle"></i>&nbsp;Termin erstellen
                            </a>
                        </p>
                        <hr style="margin: 5px 0px;"/>
                    <% } else { %>
                        <%
                        var index = 0;    
                        foreach(var date in Model.Dates.Take(3)){
                            index++;
                            %>--%>
                            
                            <div class="row" style="margin-bottom: 3px;">
                                <div class="col-xs-12">
                                    <div style="font-weight: bold; margin-bottom: 3px;">
                                        <%--<%= date.GetTitle(true) %>--%>
                                        Klausur Deutsch
                                    </div>
                                    <span style="font-size: 12px;">Noch 
                                        <%= ( new DateTime(2016, 03, 19) - DateTime.Now).Days %>
                                         Tage für 
                                        37
                                        <%--<%= date.CountQuestions() %>--%> 
                                        Fragen aus:</span><br />
                                    <%--<% foreach(var set in date.Sets){ %>--%>
                                        <a href="#">
                                            <span class="label label-set" style="font-size: 70%;">Rhetorische Stilmittel</span>
                                            <span class="label label-set" style="font-size: 70%;">Goethes Faust</span>
                                        </a>                            
                                    <%--<% } %>--%>
                                </div>
                                <div class="col-xs-3" style="opacity: .4;">
                                    <div id="chartKnowledgeDate"></div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-xs-12">
                                    <a href="#" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="display: inline-block;">
                                        <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                                        Spiel starten
                                    </a>
                                    &nbsp;
                                    <a data-btn="startLearningSession" href="#" style="display: inline-block;">
                                        <i class="fa fa-line-chart"></i> 
                                        Jetzt üben
                                    </a>
                                </div>                                
                            </div>
                            <hr style="margin: 8px 0;"/>
                            <div class="row" style="margin-bottom: 3px;">
                                <div class="col-xs-12">
                                    <div style="font-weight: bold; margin-bottom: 3px;">
                                        <%--<%= date.GetTitle(true) %>--%>
                                        Mündliche Prüfung Mathe
                                    </div>
                                    <span style="font-size: 12px;">Noch 
                                       <%-- <%= (date.DateTime - DateTime.Now).Days %>--%>
                                        12 Tage für 27 Fragen aus:</span><br />
                                    <%--<% foreach(var set in date.Sets){ %>--%>
                                        <a href="#">
                                            <span class="label label-set" style="font-size: 70%;">Kurvendiskussion</span>
                                            <span class="label label-set" style="font-size: 70%;"></span>
                                        </a>                            
                                    <%--<% } %>--%>
                                </div>
                                <div class="col-xs-3" style="opacity: .4;">
                                    <div id="chartKnowledgeDate"></div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-xs-12">
                                    <a href="#" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="display: inline-block;">
                                        <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                                        Spiel starten
                                    </a>
                                    &nbsp;
                                    <a data-btn="startLearningSession" href="#" style="display: inline-block;">
                                        <i class="fa fa-line-chart"></i> 
                                        Jetzt üben
                                    </a>
                                </div>                                
                            </div>
                            <hr style="margin: 8px 0;"/> 
                        <%--<% } %>--%>
                        <% if (Model.Dates.Count > 3) { %>
                            <a href="<%= Links.Dates() %>">Du hast <%= (Model.Dates.Count - 3) %> <%= StringUtils.Plural(Model.Dates.Count - 3,"weitere Termine","weiteren Termin") %></a>
                            <hr style="margin: 8px 0px;"/>
                        <% } %>
                    <%--<% } %>--%>
                    <p>
                        <% if (Model.DatesInNetwork.Count > 0) { %>
                            <a href="<%= Links.Dates() %>"><%= Model.DatesInNetwork.Count %> Termin<%= StringUtils.Plural(Model.DatesInNetwork.Count,"e") %> in deinem Netzwerk</a>
                            &nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>
                        <% } else {  %>
                            Kein Termin in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>&nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>.
                            Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                        <% } %>
                            
                    </p>
                    <hr style="margin: 8px 0px;"/>
                    <p><a href="<%= Links.Dates() %>" style="font-size: 12px;">Zur Terminübersicht</a></p>
                </div>
                
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0;">Zuletzt gelernt</h3>
                    <%--<% foreach(var answer in Model.AnswerRecent){ 
                        var question = answer.Question;
                    %>--%>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3"> 
                            <img src="https://memucho.de/Images/Categories/224_128s.jpg?t=20150909022331" class="ItemImage JS-InitImage"><div class="SelectAreaCheckbox"><div class="CheckboxIconContainer"><i class="Checked-Icon fa fa-check-square-o"></i><i class="Unchecked-Icon fa fa-square-o"></i><div class="CheckboxText">Auswählen</div></div></div>
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#">Was ist die Hauptstadt von Frankreich?</a>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3"> 
                            <img src="https://memucho.de/Images/Questions/543_128s.jpg?t=20160315033952" class="ItemImage JS-InitImage"><div class="SelectAreaCheckbox"><div class="CheckboxIconContainer"><i class="Checked-Icon fa fa-check-square-o"></i><i class="Unchecked-Icon fa fa-square-o"></i><div class="CheckboxText">Auswählen</div></div></div>
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#">Was bedeutet Austeritätspolitik?</a>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3"> 
                            <img src="https://memucho.de/Images/no-question-128.png" class="ItemImage JS-InitImage"><div class="SelectAreaCheckbox"><div class="CheckboxIconContainer"><i class="Checked-Icon fa fa-check-square-o"></i><i class="Unchecked-Icon fa fa-square-o"></i><div class="CheckboxText">Auswählen</div></div></div>
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#">Bei welcher rhetorischen Figur steht ein Teil für das Ganze oder das Ganze für einen Teil?</a>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3"> 
                            <img src="https://memucho.de/Images/Categories/205_128s.jpg?t=20150727072515" class="ItemImage JS-InitImage"><div class="SelectAreaCheckbox"><div class="CheckboxIconContainer"><i class="Checked-Icon fa fa-check-square-o"></i><i class="Unchecked-Icon fa fa-square-o"></i><div class="CheckboxText">Auswählen</div></div></div>
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#">Wieviele Flüchtlinge nahm Deutschland 2015 auf?</a>
                        </div>
                    </div>

                    <%--<% } %>--%>
                
                    <div class="row" style="opacity: 0.4;">
                        <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                    </div>
                </div>
            </div>
            
            <div class="col-xs-12 col-md-4" style="padding: 5px;">
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0; margin-bottom: 3px;">Übungssitzungen</h3>
                    
                    <%
                        var trainingDates = new List<TrainingDateModel>()
                        {
                            new TrainingDateModel
                            {
                                DateTime = DateTime.Now.AddHours(4),
                                QuestionCount = 12,
                                Date = new Date { Details = "Klausur Deutsch"}
                            },
                            new TrainingDateModel()
                            {
                                DateTime = DateTime.Now.AddHours(8).AddMinutes(11),
                                QuestionCount = 11,
                                Date = new Date { Details = "Mündliche Prüfung Mathe"}
                            },
                            new TrainingDateModel()
                            {
                                DateTime = DateTime.Now.AddHours(22).AddMinutes(32),
                                QuestionCount = 12,
                                Date = new Date { Details = "Klausur Deutsch"}
                            },
                            new TrainingDateModel() {DateTime = DateTime.Now.AddHours(26),
                                QuestionCount = 7
                                 },
                        };%>

                    <div class="row" style="margin-bottom: 7px;">
                        <div class="col-md-12">
                            in den nächsten <b>2 Tagen</b>
                            <ul>
                                <li>ca. <%= trainingDates.Count %> Übungssitzungen</li>
                                <li>ca. <%= new TimeSpan(0, trainingDates.Sum(x => x.Minutes), 0).ToString(@"hh\:mm") %> h Lernzeit</li>
                            </ul>
                        </div>
                    </div>
                    
                        <% foreach (var training  in trainingDates) { %>
                        <% Html.RenderPartial("TrainingDate", training); %>
                    <% } %>
                    <div class="row" style="opacity: 0.4;">
                            <div class="col-xs-12"><a href="#" class="">alle</a></div>
                        </div>
                </div>
            </div>
                           
            <div class="col-xs-12 col-md-4" style="padding: 5px;">
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0; margin-bottom: 3px;">Im Netzwerk</h3>
                    <%--<% if (Model.NetworkActivities.Count == 0) { %>
                            Keine Aktivitäten in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>. 
                            Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                    <% } else { %>
                        <% foreach(var activity in Model.NetworkActivities){ %>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="<%= new UserImageSettings(activity.UserCauser.Id).GetUrl_128px_square(activity.UserCauser.EmailAddress).Url %>" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">vor <%= DateTimeUtils.TimeElapsedAsText(activity.At) %></div>
                                    <div style="clear: left;">
                                        <a href="<%= Links.UserDetail(activity.UserCauser) %>"><%= activity.UserCauser.Name %></a> <%= UserActivityTools.GetActionDescription(activity) %>
                                        <%= UserActivityTools.GetActionObject(activity) %>
                                    </div>
                                </div>
                            </div>
                        <% } %>--%>
                            <div style="color: silver; font-size: 12px; margin-bottom: 8px;">
                                Du folgst 12 Nutzern.<br/>
                                Dir folgen 9 Nutzer.<br/>
                                <a href="#" style="font-size: 12px;">Dein Netzwerk</a>
                            </div>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="/Images/Team/team_robert201509_155.jpg" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">
                                        vor <%= DateTimeUtils.TimeElapsedAsText(DateTime.Now.AddHours(-3)) %>
                                    </div>
                                    <div style="clear: left;">
                                        <a href="#">Robert</a>
                                        spielte ein Quiz mit
                                        <a href="#">Benjamin (nachspielen)</a>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="/Images/Team/team_jule201509-2_155.jpg" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">
                                        vor <%= DateTimeUtils.TimeElapsedAsText(DateTime.Now.AddHours(-28)) %>
                                    </div>
                                    <div style="clear: left;">
                                        <a href="#">Jule</a>
                                        folgt jetzt
                                        <a href="#">Markus</a>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="/Images/Team/team_christof201509_155.jpg" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">
                                        vor <%= DateTimeUtils.TimeElapsedAsText(DateTime.Now.AddDays(-2)) %>
                                    </div>
                                    <div style="clear: left;">
                                        <a href="#">Christof</a>
                                        erhielt die Trophäe "Superstreber"
                                        <a href="#"></a>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="/Images/Team/team_christof201509_155.jpg" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">
                                        vor <%= DateTimeUtils.TimeElapsedAsText(DateTime.Now.AddDays(-2)) %>
                                    </div>
                                    <div style="clear: left;">
                                        <a href="#">Christof</a>
                                        erstellte den Fragesatz
                                        <a href="#">Berühmte Sehenswürdigkeiten</a>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <img src="/Images/Team/team_christof201509_155.jpg" />
                                </div>
                                <div class="col-xs-9" style="">
                                    <div style="color: silver; font-size: 10px; margin: -4px 0;">
                                        vor <%= DateTimeUtils.TimeElapsedAsText(DateTime.Now.AddDays(-2)) %>
                                    </div>
                                    <div style="clear: left;">
                                        <a href="#">Christof</a>
                                        erstellte den Termin
                                        <a href="#">Mündliche Prüfung Mathe</a>
                                    </div>
                                </div>
                            </div>

                        <div class="row" style="opacity: 0.4;">
                            <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                        </div>
                    <%--<% } %>--%>
                </div>
            </div>
        </div>
    
        <div class="row" style="margin-top: 20px;">
            <div class="col-xs-12 col-md-4" style="padding: 5px;">
                
            </div>
        </div>

    <% } %>
</asp:Content>