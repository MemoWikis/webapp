<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="modalTabInfoMyKnowledge" class="modal">
    <div class="modal-header">
        <button class="close" data-dismiss="modal">×</button>
        <h3>Kategoriebild hochladen</h3>
    </div>

    <div class="modal-body">
        <div class="alert">
            <strong>Achtung:</strong> Bildrechte sind ein sensibles Thema. 
            Bitte lade nur Bilder hoch die gemeinfrei sind oder Bilder die Du stelbst erstellt hast. 
        </div>
       
        <div>
            <label class="radio inline" style="width: 200px;">
                <input type="radio" checked="checked" name="imgSource"/>Bilder von Wikimedia verwenden.
            </label>
        
            <label class="radio inline" style="width: 130px;">
                <input type="radio" name="imgSource" />Andere Bilder
            </label>            
        </div>
        <div style="clear: both;"></div>
        
        <%-- Wikimedia --%>
        <div style="margin-top:10px;">
            In Wikipedia sind viele Millionen Bilder zu finden, die frei genutzt werden können. 
            Auf <a href="http://commons.wikimedia.org/wiki/Hauptseite?uselang=de">Wiki-Commons</a> kann gezielt nach Inhalten gesucht werden. 
            Wenn Du auf Wikipedia auf ein Bild klickst gelangst Du auf die Bild-Detailseite. Gib die URL der Detailseite hier an: 
        </div>
        
        <div style="margin-top: 10px;">
            <label style="position: relative; top: 5px;">Wikipedia URL:</label>
            <input class="span2" id="prependedInput" type="text" placeholder="http://" style="width: 250px;">      
        </div>

    </div>
    <div class="modal-footer">
        <a href="#" class="btn" data-dismiss="modal">Abbrechen.</a>
        <a href="#" class="btn btn-primary" data-dismiss="modal">Speichern</a>
    </div>
</div>