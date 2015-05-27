<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

Datum min. <b><span id="spanEntryPrecision"></span></b>

<div style="padding-bottom: 5px;">
    <span id="spanEntryFeedback"></span>
    <i class="fa fa-exclamation-circle" id="iDateError" style="color:red; display: none; font-size: 16px;"></i> 
    <i class="fa fa-check-circle" id="iDateCorrect" style="color:green; display: none;  font-size: 16px;"></i> 
</div>

<input type="text" id="txtAnswer" class="form-control " rows="1" style=" width: 100%" placeholder="Gib hier bitte deine Antwort ein."/>