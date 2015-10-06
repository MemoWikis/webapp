<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabVariousModel>" %>

<div class="row" >
    <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
        <h3>Verschiedenes</h3>
    </div>
</div>


<h3>Frage-Klassifizierung</h3>
<p>
    Eine Klassifzierung ist möglich, sobald mind. 5 Beispieldatensätze für jede Bedingung gegeben sind. 
    Also mind. bei 5 Nutzern alle Bedingungen für jeweils eine Frage erfüllt wurden.
    Eine Mehrfachklassifzierung ist nicht möglich. 
</p>

<h4>No-brainer</h4>
<p>Bedingungen:</p>
<ul>
    <li>80% richtig bei der ersten Antwort.</li>
    <li>Ab der 2. Wiederholung immer über 90 korrekt beantwortet</li>
</ul>

<h4>Einfach zu lernen</h4>
<p>Bedingungen:</p>
<ul>
    <li>min. 5 Wiederholungen u. mind. 0 Tage / max. 1 Tage Tag Abstand zur letzten Wiederholung -> 70% korrekt</li>
    <li>min. 10 Wiederholungen u. mind. 1 Tage / max. 3 Tage Abstand zur letzten Wiederholung -> 85% korrekt</li>
    <li>min. 10-... Wiederholungen u. mind. 3 Tage / max. 30 Tage Abstand zur letzten Wiederholung -> 90% korrekt</li>
</ul>
<h4>Mittelschwer zu lernen</h4>
<p>Bedingungen:</p>
<ul>
    <li>3-5 Wiederholungen u. mind. 0 Tage / max. 1 Tage Tag Abstand zur letzten Wiederholung -> 65% korrekt</li>
    <li>5-10 Wiederholungen u. mind. 1 Tage / max. 3 Tage Abstand zur letzten Wiederholung -> 75% korrekt</li>
    <li>10-20 Wiederholungen u. mind. 3 Tage / max. 7 Tage Abstand zur letzten Wiederholung -> 85% korrekt</li>
</ul>
<h4>Schwer zu lernen</h4>
<p>Bedingungen:</p>
<ul>
    <li>3-5 Wiederholungen u. mind. 0 Tage / max. 1 Tage Tag Abstand zur letzten Wiederholung -> 65% korrekt</li>
    <li>5-10 Wiederholungen u. mind. 1 Tage / max. 3 Tage Abstand zur letzten Wiederholung -> 75% korrekt</li>
    <li>10-20 Wiederholungen u. mind. 3 Tage / max. 7 Tage Abstand zur letzten Wiederholung -> 85% korrekt</li>
</ul>