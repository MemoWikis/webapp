<%@  Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<div class="container-fluid">
    <div class="row">
        <%--<div class="col-md-3 "></div>--%>
        <div class="col-md-7 col-md-offset-3">
            <p>Hallo Kurt,Das Dashboard zeigt kompakte und übersichtliche Informationen. Mit seinen Grafiken und Anzeigen kommt es dem englischen Wort für "Armaturenbrett" ziemlich nah. In deinem Wunschwissen befinden sich aktuell Fragen, verteilt auf Lernsets. Stöbere durch die Themenseiten, um Inhalte zu finden, die zu dir passen. Benutze dieses Symbol ( ), um Themen, Lernsets und Fragen zu deinem Wunschwissen hinzuzufügen. Nichts gefunden, was du wissen willst? Erstelle Fragen und Themen ganz nach deinen Ansprüchen! </p>
        </div>
        <div class="row">
            <div class="col-md-7 col-md-offset-3">
                <button class="btn btn-primary" id="randThemes">zufällige Themenseite</button>
                <a class="btn btn-primary" href="/Fragen/Erstelle">Frage erstellen</a>
            </div>

        </div>
    </div>
</div>
<script>
    $("#randThemes").on("click", function (e) {
        e.preventDefault;
        $.ajax({
            url: "/Knowledge/RandomThemes",
            type: "POST",
            dataType:"text",
            success: function (result) {
                console.log(result);
                window.location.href = result;
            }, error: function (xhr) {
                console.log('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
            }
        });
    })
</script>