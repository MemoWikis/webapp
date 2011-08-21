<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>


<div class="span-20" style="border-top:1px solid; margin-top:10px; background-color:silver;">
    <div class="span-4" style="height: 100px;">
        <div>Gemerkt: <img src="/Images/star.png"/> </div>
        <div>Relevanz: 4 (&#216;   4,2)</div>
        <div>Qualtität: 3 (&#216;   4,6)</div>        
    </div>

    <div class="span-12" style="background-color: seashell; ">
        <div style="height: 20px; background-color: cadetblue">Von X Mustermann</div>
        <div style="height: 100%; font-weight:normal; font-size:large;"><%=Model.QuestionShort%></div>   
    </div>

    <div class="span-4 last">
       asdfasdf
    </div>
</div>