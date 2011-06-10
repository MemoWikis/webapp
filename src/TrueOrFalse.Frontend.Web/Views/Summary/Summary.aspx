<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrueOrFalse.View.Web.Views.Summary.SummaryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">


	<script type="text/javascript">

		var chart;
		$(document).ready(function () {
		    chart = new Highcharts.Chart({
		        chart: {
		            renderTo: 'container',
		            defaultSeriesType: 'line',
		            marginRight: 130,
		            marginBottom: 25
		        },
		        title: {
		            text: 'Wissensentwicklung der letzten 6 Monate',
		            x: -20 //center
		        },
		        subtitle: {
		            text: 'Fragen die ich beantworkten können möchte./ Fragen die ich beantworten kann.',
		            x: -20
		        },
		        xAxis: {
		            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
						'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
		        },
		        yAxis: {
		            title: {
		                text: 'Anzahl Fragen'
		            },
		            plotLines: [{
		                value: 0,
		                width: 1,
		                color: '#808080'
		            }]
		        },
		        tooltip: {
		            formatter: function () {
		                return 'Im ' + this.x + '<b>konntest</b> Du ' + this.y + ' ' +this.series.name + ' beantworten.';
		            }
		        },
		        legend: {
		            layout: 'vertical',
		            align: 'right',
		            verticalAlign: 'top',
		            x: 0,
		            y: 100,
		            borderWidth: 0
		        },
		        series: [{
		            name: 'Ist Fragen',
		            data: [0, 0, 4, 72, 58, 64, 142, 170, 110, 165, 242, 468]
		        }, {
		            name: 'Soll Fragen',
		            data: [-0, 25, 72, 142, 324, 286, 390, 400, 381, 315, 472, 476]
		        }]
		    });


		});

		$(function() {
		    //hover states on the static widgets
		    $('.button').hover(
		        function() { $(this).addClass('ui-state-hover'); },
		        function() { $(this).removeClass('ui-state-hover'); }
				    );
		});		    
				
	</script> 
		


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Willkommen Robert Mischke, schön das Du da bist.</h2>

<%--<button>
<%= Html.ActionLink("Frage erstellen", Links.QuestionCreate, Links.CreateQuestionController)%>
</button>--%>


<style type="text/css">
	/*demo page css*/
	a.button {padding: .4em 1em .4em 20px;text-decoration: none;position: relative;}
	a.button span.ui-icon {margin: 0 5px 0 0;position: absolute;left: .2em;top: 50%;margin-top: -8px;}
</style>

<div>
<a href="#" class="button ui-state-default ui-corner-all"><span class="ui-icon ui-icon-triangle-1-e"></span>Neue Frage erstellen</a>
</div>


<h3>Wissensentwicklung</h3>

<%= Html.DropDownList("ddlKnowledgeType", Model.KenDevelopmentTypes) %>
Innerhalb von: <%= Html.DropDownList("ddlPeriod", Model.KenDevelopmentPeriod) %>

<div id="container" style="width: 600px; height: 350px; margin: 0 auto;"></div>
 
<h3>Interessante Fragen</h3>

<h3>Zuletzt beantwortete Fragen</h3>

</asp:Content>

<asp:Content ID="RightMenu" ContentPlaceHolderID="RightMenu" runat="server">
    Suche <span style="float:right;">[filter]</span>
    <%= Html.TextBox("search") %>
</asp:Content>

