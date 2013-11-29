<%@ Page Title="Mein Wissensstand" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
    <script>
        $(function () {
            $("#totalKnowledgeSpark").sparkline([75, 22, 40], {
                type: 'pie',
                sliceColors: ['#1BE022', 'red', 'silver']
            });

            $("#totalKnowledgeOverTimeSpark").sparkline([5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5], {
                type: 'line',
                witdh: '250'
            });

            $("#answeredThisWeekSparkle").sparkline([14, 4], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#answeredThisMonthSparkle").sparkline([4, 8], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#answeredLastWeekSparkle").sparkline([4, 6], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#answeredLastMonthSparkle").sparkline([5, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            
            $("#examSparkle1").sparkline([1, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#examSparkle2").sparkline([15, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#examSparkle3").sparkline([71, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#examSparkle4").sparkline([1, 17], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            $("#examSparkle5").sparkline([10, 0], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            

            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#1BE022', 'red'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
        });
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

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-10">
        <h2 style="color: black; margin-bottom: 5px; margin-top: 0px;">Hallo <span style="color: #2E487B"><%= Model.UserName %></span>, Dein Wissen:</h2>
        
        <p style="margin-bottom: 10px;">Hier erhälst Du eine Übersicht über Dein Wunschwissen und Deinen Wissensstand.</p>
        
        <div class="alert" style="margin-bottom: 0px;">
          <strong>Kommende Prüfungen!</strong> 
          <ul>
              <li>14.09.2013: Führerschein (78 Fragen ) <span id="examSparkle3" style="position: relative; top: 3px; left: 10px"></span></li>
              <li>14.09.2013: BIO-Leistungskurs (12 Fragen )<span id="examSparkle2" style="position: relative; top: 3px; left: 10px"></span></li>
              <li>15.09.2013: Phyisk-Leistungskurs (171 Fragen ) <span id="examSparkle1" style="position: relative; top: 3px; left: 10px"></span></li>
          </ul>
          <strong>Kommende Termine!</strong> 
          <ul>
              <li>14.09.2013: Bewerbungsgespräch Fa. Meyer (12 Fragen ) <span id="examSparkle4" style="position: relative; top: 3px; left: 10px"></span>
              </li>
              <li>
                  14.09.2013: Date mit Mona (18 Fragen )
                  <span id="examSparkle5" style="position: relative; top: 3px; left: 10px"></span>
                  &nbsp;&nbsp;<i class="icon-thumbs-up" style="color: green;"></i>
                  
              </li>
          </ul>
        </div>

        <div class="column">
            <h3>Wunschwissen</h3>
            <div class="answerHistoryRow">
                <div>Fragen: <span><%= Model.QuestionsCount %> <span id="totalKnowledgeSpark"></span></span></div>    
            </div>
            <div class="answerHistoryRow">
                <div>
                    Frageseätze: <span><%= Model.QuestionsSetCount %> <span id="Span1"> (mit x Fragen)</span></span><br/>    
                </div>
                
            </div>
            <span id="totalKnowledgeOverTime">Entwicklung über Zeit:<br /> 
            <span id="totalKnowledgeOverTimeSpark"></span></span>
        </div>
        
        <div class="column">
            <h3>Wissenstand</h3>
            <div style="padding-bottom: 10px;">Dein Wissenstand entspricht ca. 83% Deines Wunschwissens.</div>
            <div>Gewusst: 70% (217 Fragen) </div>
            <div>Nicht gewusst 12% (xx Fragen)  </div>
            <div>Unbekannt 18% (xx Fragen)</div>
        </div>

        <div class="column">
            <h3>Training</h3>
            <div class="answerHistoryRow" style="margin-bottom: 5px;">
                <div style="color:black;">Antworten ges.: 4312 </div> (seit: 4.3.2012)
            </div>
                
            <div class="answerHistoryRow">    
                <div>Diese Woche <span class="answerAmount"><%= Model.TotalAnswerThisWeek %></span> 
                    <span id="answeredThisWeekSparkle"></span>
                    <div class="percentage"><span >74%</span></div>
                </div> 
            </div>
            <div class="answerHistoryRow">
                <div>Diesen Monat <span class="answerAmount"><%= Model.TotalAnswerThisMonth %></span> <span id="answeredThisMonthSparkle"></span>
                    <div class="percentage"><span>19%</span></div>
                </div>
            </div>
            <div class="answerHistoryRow">
                <div>Diese Jahr <span class="answerAmount"><%= Model.TotalAnswerPreviousWeek %></span> <span id="answeredLastWeekSparkle"></span></div> 
            </div>
            <div class="answerHistoryRow">
                <div>Letzten Monat <span class="answerAmount"><%= Model.TotalAnswerLastMonth %></span> <span id="answeredLastMonthSparkle"></span></div> 
            </div>
        </div>
        <div style="clear:both;"></div>
        
        <div style="padding-top:20px; height: 200px; ">
        
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
        </div>

    </div>
    
</asp:Content>