<%@ Page Title="Mein Wissensstand" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        $(function () {
            var titles = ['Gewusst', 'Nicht gewusst', 'Unbekannt'];
            $("#totalKnowledgeSpark")
                .sparkline(
                    [<%= Model.KnowledgeSummary.Secure %>, <%= Model.KnowledgeSummary.Weak %>, <%= Model.KnowledgeSummary.Unknown %>],
                    {
                        type: 'pie',
                        sliceColors: ['#3e7700', '#B13A48', '#EFEFEF'],
                        tooltipFormat: '{{offset:slice}} {{value}} ({{percent.1}}%)',
                        tooltipValueLookups: {'slice': titles},
                    }
                );

            $("#totalKnowledgeOverTimeSpark").sparkline([5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5], {
                type: 'line',
                witdh: '250'
            });

            $("#answeredThisWeekSparkle").sparkline([<%= Model.AnswersThisWeek.TotalTrueAnswers %>, <%= Model.AnswersThisWeek.TotalFalseAnswers %>], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
            $("#answeredThisMonthSparkle").sparkline([<%= Model.AnswersThisMonth.TotalTrueAnswers %>, <%= Model.AnswersThisMonth.TotalFalseAnswers %>], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
            $("#answeredThisYearSparkle").sparkline([<%= Model.AnswersThisYear.TotalTrueAnswers %>, <%= Model.AnswersThisYear.TotalFalseAnswers %>], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });

            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#3e7700', '#B13A48'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
            
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
        });
    </script>
    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate1", 9, 2, 1) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate2", 4, 3, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate3", 1, 12, 4) });
        google.setOnLoadCallback(drawActivityChart);

        //chartKnowledgeDate

        function drawKnowledgeChart(chartElementId) {
            var data = google.visualization.arrayToDataTable([
                ['Task', 'Hours per Day'],
                ['Gewusst', 11],
                ['Nicht gewusst', 2],
                ['Unbekannt', 2],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'labeled' },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10},
                slices: {
                    0: { color: 'lightgreen' },
                    1: { color: 'lightsalmon' },
                    2: { color: 'silver' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
            chart.draw(data, options);
        }

        function drawKnowledgeChartDate(chartElementId, amountGood, amountBad, amountUnknown ) {

            var chartElement = $("#" + chartElementId);
            console.log(chartElement);

            var data = google.visualization.arrayToDataTable([
                ['Task', 'Hours per Day'],
                ['Gewusst', amountGood],
                ['Nicht gewusst', amountBad],
                ['Unbekannt', amountUnknown],
            ]);

            var options = {
                pieHole: 0.5,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 60,
                chartArea: { width: '85%', height: '85%', top: 0 },
                slices: {
                    0: { color: 'lightgreen' },
                    1: { color: 'lightsalmon' },
                    2: { color: 'silver' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(chartElement.get()[0]);
            chart.draw(data, options);
        }

        function drawActivityChart() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Richtig beantwortet', 'Falsch beantwortet', { role: 'annotation' }
                ],
                    ['30.05', 7, 0, ''],
                    ['29.05', 19, 0, ''],
                    ['28.05', 0, 8, ''],
                    ['27.05', 28, 3, ''],
                    ['26.05', 29, 3, ''],
                    ['25.05', 0, 0, ''],
                    ['24.05', 0, 2, ''],
                    ['23.05', 7, 1, ''],
                    ['22.05', 27, 0, ''],
                    ['21.05', 0, 0, ''],
                    ['20.05', 4, 0, ''],
                    ['19.05', 0, 9, ''],
                    ['18.05', 0, 0, ''],
                    ['17.05', 0, 0, ''],
                    ['16.05', 14, 2, ''],
                    ['15.05', 10, 8, ''],
                    ['14.05', 21, 0, ''],
                    ['13.05', 12, 0, ''],
                    ['12.05', 29, 4, ''],
                    ['11.05', 26, 0, ''],
                    ['10.05', 29, 5, ''],
                    ['09.05', 0, 0, ''],
                    ['08.05', 21, 0, ''],
                    ['07.05', 0, 3, ''],
                    ['06.05', 0, 1, ''],
                    ['05.05', 0, 0, ''],
                    ['04.05', 14, 2, ''],
                    ['03.05', 4, 4, ''],
                    ['02.05', 17, 0, ''],
                    ['01.05', 16, 2, ''],
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
                bar: { groupWidth: '95%' },
                chartArea: { 'width': '88%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['lightgreen', 'lightsalmon'],
                isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartActivityLastDays"));
            chart.draw(view, options);
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

    <h2 style="color: black; margin-bottom: 5px; margin-top: 0px;"><span class="ColoredUnderline Knowledge">Hallo <span class=".dark-blue"><%= Model.UserName %></span>, dein Wunschwissen</span>:</h2>
        
    <p style="margin-bottom: 10px;">Hier erhältst du eine Übersicht über dein Wunschwissen und deinen Wissensstand.</p>

    <% if(!Model.IsLoggedIn){ %>

        <div class="bs-callout bs-callout-danger">
            <h4>Anmelden oder registrieren</h4>
            <p>Um Wunschwissen zu verwenden, musst du dich <a href="/Anmelden">anmelden</a> oder dich <a href="/Registrieren">registrieren</a>.</p>
        </div>

    <% }else{  %>
    
        <div class="row">
            <div class="col-xs-12 col-md-4">
                <h3>Training <span style="font-size: 12px; color: silver; padding-left: 15px;">letzte 30 Tage</span></h3>
                
                <div id="chartActivityLastDays" style="margin-right: 20px; text-align: left;"></div>
                
                <div class="row" style="font-size: 12px">
                    <div class="col-md-12">
                        <span style="display: inline-block; width: 45%">Lerntage gesamt: <b>78</b></span> <span style="color: silver; font-weight: bold;">seit 14.07.2014</span><br />
                        <span style="display: inline-block; width: 40%">Längste Folge: <b>12</b></span> <span style="color: silver; font-weight: bold;">07.09 - 19.09.2014</span><br />
                        <span style="display: inline-block; width: 40%">Aktuelle Folge: <b>4</b></span> <span style="color: silver; font-weight: bold;" >26.05 - 30.05.2015</span>
                    </div>
                </div>

            </div>
            <div class="col-xs-12 col-md-5" style="">
                <h3>Dein Wissensstand</h3>
                <div id="chartKnowledge" style="margin-right: 20px; text-align: left;"></div>
            </div>
            <div class="col-xs-12 col-md-3">
                <div class="row">
                    <div class="col-cs-12">
                        <h3>Im Wunschwissen</h3>        
                    </div>
                    <div class="col-cs-12 number-box-questions">
                        <a href="<%= Links.QuestionsMine() %>">
                            <div>
                                <span style="font-weight: 900; font-size: 44px; padding-left: 9px;"><%= Model.QuestionsCount %></span>
                                <span style="font-size: 22px">Fragen</span>
                            </div>
                        </a>
                    </div>                    
                    <div class="col-cs-12 number-box-sets">
                        <a href="<%= Links.SetsMine() %>">
                            <div>
                                <span style="font-weight: 900; font-size: 44px; padding-left: 15px;"><%= Model.SetCount %></span>
                                &nbsp;<span style="font-size: 22px">Fragesätze</span>
                            </div>
                        </a>
                    </div>
                    <div class="col-cs-12 number-box-reputation">
                        <a href="<%= Links.UserDetail(Url, Model.UserName, Model.UserId) %>">
                            <div style="padding-left: 14px; padding: 8px;">                        
                                <span>Reputation <b><%= Model.ReputationTotal %></b></span><br />
                                <span>Platz <b><%= Model.ReputationRank %></b></span>
                            </div>
                        </a>
                    </div>
                </div>
            </div>            
        </div>
    
        <div class="row" style="margin-top: 15px;">
            <div class="col-xs-12 col-md-4">
                <h3 style="margin-bottom: 18px;">Termine</h3>
                
                <%
                var index = 0;    
                foreach(var date in Model.Dates){
                index++;
                %>
                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-xs-9">
                        <div style="font-weight: bold; margin-bottom: -3px;"><%= date.Details %></div>
                        <span style="font-size:12px">Noch <%= (date.DateTime - DateTime.Now).Days %> Tage</span><br />
                        <% foreach(var set in date.Sets){ %>
                            <a href="<%= Links.SetDetail(Url, set) %>">
                                <span class="label label-set"><%= set.Name %></span>
                            </a>                            
                        <% } %>
                    </div>
                    <div class="col-xs-3" style="">
                        <div id="chartKnowledgeDate<%=index %>"></div>
                    </div>
                </div>    
                <% } %>
                <div class="row">
                    <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                </div>
            </div>            
            <div class="col-xs-12 col-md-4">
                <h3 style="margin-bottom: 18px;">Zuletzt gelernt</h3>
                <% foreach(var answer in Model.AnswerRecent){ 
                    var question = answer.GetQuestion();
                %>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3">
                            <%= ImageFrontendData.Create(question).RenderHtmlImageBasis(50, true, ImageType.Question) %>
                        </div>
                        <div class="col-xs-9" style="">
                            <%= question.Text %>
                        </div>
                    </div>
                <% } %>
                
                <div class="row">
                    <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                </div>
            </div>
            <div class="col-xs-12 col-md-4">
                <h3>Im Netzwerk</h3>
                
                    <% var userRepo = Sl.Resolve<UserRepository>();
                       var user1 = userRepo.GetById(18);
                       var user2 = userRepo.GetById(31);
                    %>
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3">
                            <img src="<%= new UserImageSettings(user1.Id).GetUrl_128px_square(user1.EmailAddress).Url %>" />
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#"><%= user1.Name %></a> erstellte die Frage: 
                            <a href="#">"Wann wurde Galileo Galilei geboren?"</a>
                        </div>
                    </div>
                
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3">
                            <img src="<%= new UserImageSettings(user1.Id).GetUrl_128px_square(user1.EmailAddress).Url %>" />
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#"><%= user1.Name %></a> erstellte den Fragesatz: 
                            <span class="label label-set">Galileo Galilei</span>
                        </div>
                    </div>
                
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-3">
                            <img src="<%= new UserImageSettings(user2.Id).GetUrl_128px_square(user1.EmailAddress).Url %>" />
                        </div>
                        <div class="col-xs-9" style="">
                            <a href="#"><%= user2.Name %></a> erstellte die Kategorie: 
                            <span class="label label-category show-tooltip" title="" data-placement="top" data-original-title="Gehe zu Kategorie">Helmut Kohl</span>
                        </div>
                    </div>
                
                    <div class="row">
                        <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                    </div>

            </div>
        </div>

<%--        <div class="column">
            <h3>Wunschwissen</h3>
            <div class="answerHistoryRow">
                <div>
                    <a href="<%= Links.QuestionsWish() %>">
                        Fragen: <span><%= Model.QuestionsCount %> <span id="totalKnowledgeSpark"></span></span>
                    </a>
                </div>    
            </div>
            <div class="answerHistoryRow">
                <div>
                    <a href="<%= Links.SetsWish() %>">
                        Frageseätze: <span><%= Model.SetCount %> </span><br/>
                    </a>
                </div>
                
            </div>
            <span id="totalKnowledgeOverTime">Entwicklung über Zeit:<br /> 
            <span id="totalKnowledgeOverTimeSpark"></span></span>
        </div>
        
        <div class="column">
            <h3>Wissenstand</h3>
            <div class="show-tooltip" title="Die Menge der gewussten Fragen" style="padding-bottom: 10px;">Dein Wissensstand entspricht ca. <%= Model.KnowledgeSummary.SecurePercentage %>% deines Wunschwissens.</div>
            <div class="show-tooltip" title="Als 'gewusst' gilt eine Frage dann, wenn die Wahrscheinlichkeit, dass du die Frage richtig beantworten wirst, bei über 90% liegt.">Gewusst: <%= Model.KnowledgeSummary.SecurePercentage %>% (<%= Model.KnowledgeSummary.Secure %> Fragen) </div>
            <div class="show-tooltip" title="Als 'nicht gewusst' gilt eine Frage dann, wenn die Wahrscheinlichkeit, dass du die Frage richtig beantworten wirst, bei unter 90% liegt.">Nicht gewusst: <%= Model.KnowledgeSummary.WeakPercentage %>% (<%= Model.KnowledgeSummary.Weak %> Fragen)  </div>
            <div class="show-tooltip" title="Wenn du die Frage seltener als 4x beantwortet hast, ist unbekannt, ob du sie weißt.">Unbekannt: <%= Model.KnowledgeSummary.UnknownPercentage %>% (<%= Model.KnowledgeSummary.Unknown %> Fragen)</div>
        </div>

        <div class="column">
            <h3>Training</h3>
            <div class="answerHistoryRow" style="margin-bottom: 5px;">
                <div style="color:black;">
                    Antworten ges.: <%= Model.AnswersEver.TotalAnswers %>
                </div> 
            </div>
                
            <div class="answerHistoryRow">    
                <div>
                    Diese Woche 
                    <span class="answerAmount"><%= Model.AnswersThisWeek.TotalAnswers %></span> 
                    <span id="answeredThisWeekSparkle"></span>
                    
                    &nbsp;
                    Vorwoche
                    <%= Model.AnswersLastWeek.TotalTrueAnswers %>
                </div> 
            </div>
            <div class="answerHistoryRow">
                <div>
                    Diesen Monat 
                    <span class="answerAmount"><%= Model.AnswersThisMonth.TotalAnswers %></span> 
                    <span id="answeredThisMonthSparkle"></span>
                    
                    &nbsp;
                    Vormonat
                    <%= Model.AnswersLastMonth.TotalAnswers %>
                </div>
            </div>
            <div class="answerHistoryRow">
                <div>
                    Dieses Jahr 
                    <span class="answerAmount"><%= Model.AnswersThisYear.TotalAnswers %></span> 
                    <span id="answeredThisYearSparkle"></span>
                    
                    &nbsp;
                    Vorjahr
                    <%= Model.AnswersLastYear.TotalAnswers %>
                </div> 
            </div>
        </div>
        <div style="clear:both;"></div>--%>
        
<%--        <div style="padding-top:20px; height: 200px; ">
        
            <div class="column">
                <h3>Fragen (175)</h3>
                <table>
                    <tr>
                        <td>Wann wurde xy geboren </td> 
                        <td>14x <span id="question-1"></span> </td>
                    </tr>
                </table>
            </div>
        
            <div class="column">
                <h3>Fragesätze</h3>
                <div>
                    <span>Noch keine Fragesätze</span>
                </div>
            </div>
 
            <div class="column">
                <h3>Kategorien (34)</h3>
                <table>
                    <tr>
                        <td>Musik</td> 
                        <td>72</td> 
                        <td><span id="inCategory-1"></span></td>
                        <td><span id="inCategoeryOverTime-1"></span></td>
                    </tr>
                </table>
            </div>
        </div>--%>
    <% } %>
</asp:Content>