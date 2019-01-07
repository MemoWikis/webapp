<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
 
<div class="alert alert-info" style="max-width: 700px; margin-left: auto; margin-right: auto; margin-top: 55px; margin-bottom: 40px; padding: 15px;">
    <h3 style="margin-top: 0; font-size: 25px;">Schlaue Lernanalyse: Bald hier für dich</h3>
    <p style="font-size: 18px; margin-top: 15px;">
        Wir möchten, dass dir Lernen Spaß macht und du immer genau weißt, wo du stehst. 
        Deshalb werden wir dir bald an dieser Stelle zeigen, wie dein aktueller Wissensstand ist und 
        in welchen Teilbereichen du am dringendsten lernen solltest.
    </p>
    <p style="font-size: 18px; margin-top: 15px;">
        Weitere Statistiken zu deinem Lernstand und Fortschritt werden dir helfen, 
        deine Wunschwissen-Themen zu erschließen und dabei nie den Überblick zu verlieren. 
        Wie viel hast du schon gelernt? Wie viel fehlt noch? Was kommt als nächstes? Wir zeigen es dir.
    </p>
    <div class="row" style="text-align: center; margin-top: 20px;">
        <div class="col-sm-12">
            <img src="/Images/Various/tabAnalyticsMockup1.png" style="/*max-width: 400px;*/"/>
        </div>
        <div class="col-sm-12">
            <img src="/Images/Various/treemapChartPsychology.png" style="/*max-width: 400px;*/"/>
        </div>
        <div class="col-sm-12">
            <img src="/Images/Various/graphExampleTransp.png" style="/*max-width: 400px;*/"/>
        </div>
    </div>
</div>

<%--<%= Scripts.Render("~/bundles/js/AnalyticTab") %>--%>