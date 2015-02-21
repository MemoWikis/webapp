<%@ Page Title="FAQ" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="PageHeader">FAQ - Häufig gestellte Fragen</h2>
    
    <div class="panel-group" id="FaqAccordion" role="tablist" aria-multiselectable="true">
      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeading1">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqText1" aria-expanded="true" aria-controls="FaqText1">
              FAQ 1
            </a>
          </h4>
        </div>
        <div id="FaqText1" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeading1">
          <div class="panel-body">
            Text FAQ 1 Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
          </div>
        </div>
      </div>
      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeading2">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqText2" aria-expanded="false" aria-controls="FaqText2">
              FAQ 2
            </a>
          </h4>
        </div>
        <div id="FaqText2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeading2">
          <div class="panel-body">
            Text FAQ 2 Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
          </div>
        </div>
      </div>
      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeading3">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqText3" aria-expanded="false" aria-controls="FaqText3">
              FAQ 3
            </a>
          </h4>
        </div>
        <div id="FaqText3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeading3">
          <div class="panel-body">
            Text FAQ 3 Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
          </div>
        </div>
      </div>
    </div>
    
</asp:Content>


