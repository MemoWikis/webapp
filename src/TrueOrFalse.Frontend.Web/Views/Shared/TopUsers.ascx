<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="span-5 last">
<h2>Top DiesenMonat</h2>
<hr />

<% for (int i = 0; i < 3; i++)
   {%>
<div class="span-5 last">

	<div class="span-2">
	&nbsp;
	</div>
	<div class="span-3 last">
		<a href="" style="text-decoration:underline;">Tobias Krause</a><br />
		40 erstellt<br />
		217 editiert<br />
	</div>
	<hr />

</div>

<%
   }%>


</div>