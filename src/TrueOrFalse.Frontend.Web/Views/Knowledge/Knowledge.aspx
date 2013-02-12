<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
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

            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#1BE022', 'red'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
            
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#1BE022', 'red'] });
        });

    </script>
    
    <style>
        #totalKnowledge{font-size: 30px; margin-top: 20px; display: inline-block; color: darkgray;font-weight: bolder;}
        #totalKnowledgeOverTime{font-size: 18px; line-height:27px ;color: rgb(170, 170, 170);padding-top: 5px;display: inline-block;}
        #totalKnowledgeOverTimeSpark{ display: inline-block;}
        #headerAnswered{font-size: 30px; line-height: 30px; margin-top: 20px; display: inline-block; color: darkgray;font-weight: bolder;padding-bottom: 4px;}
        div.answerHistoryRow div{ display: inline;font-size: 17px; color: rgb(170, 170, 170)}
        div.answerHistoryRow .answerAmount{ color:blue; font-weight: bolder;}
        
        div.column  { width: 260px; float: left;}
    </style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="span10">
        <h2 style="color: black">Hallo <span style="color: darkgrey "><%= Model.UserName %></span>, Dein Wissen:</h2>

        <div id="container" style=""></div>
        <div>
            <div style="float: left; margin-right: 25px; ">
                <span id="totalKnowledge"><span style="padding-top:2px; display: inline-block">Wunschwissen: <span><%= Model.WishKnowledgeCount %></span></span> <span id="totalKnowledgeSpark"></span></span><br/>
                <span id="totalKnowledgeOverTime">Entwicklung über Zeit: <span id="totalKnowledgeOverTimeSpark"></span></span>
            </div>

            <div style="float: left;">
                <div id="headerAnswered">Beantwortet:</div>
                <div class="answerHistoryRow">    
                    <div>Diese Woche <span class="answerAmount"><%= Model.TotalAnswerThisWeek %></span> <span id="answeredThisWeekSparkle"></span></div> 
                    <div>Diesen Monat <span class="answerAmount"><%= Model.TotalAnswerThisMonth %></span> <span id="answeredThisMonthSparkle"></span></div>
                </div>
                <div class="answerHistoryRow">
                    <div>Letzte Woche <span class="answerAmount"><%= Model.TotalAnswerPreviousWeek %></span> <span id="answeredLastWeekSparkle"></span></div> 
                    <div>Letzten Monat <span class="answerAmount"><%= Model.TotalAnswerLastMonth %></span> <span id="answeredLastMonthSparkle"></span></div> 
                </div>
            </div>
            <div style="clear:both;"></div>
        </div>

        <div style="padding-top:20px; height: 200px; ">
 
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
    
             <div class="column">
                <h3>Fragesätze</h3>
                <div>
                    <span>Noch keine Fragesätze</span>
                </div>
            </div>

            <div class="column">
                <h3>Fragen (175)</h3>
                <table>
                    <tr>
                       <td>Wann wurde xy geboren </td> 
                       <td>14x <span id="question-1"></span> </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Content>