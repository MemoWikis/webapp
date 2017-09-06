<%@ Control Language="C#" Inherits="ViewUserControl<EditCategoryTypeModel>" %>

<!-- temp accordion html start-->
                    <div class="panel-group" id="accordion">
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class="Colored Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">
            1. Buch/Ausgabe auswählen
                            </a>
                          </h4>
                        </div>
                        <div id="collapse1" class="panel-collapse collapse in">
                          <div class="panel-body">
<!-- temp accordion html end-->
<div class="FormSection">
    <div class="form-group">
        <div class="noLabel columnControlsFull">
            <p class="form-control-static" style="margin-bottom: 10px;">Bitte prüfe zuerst, ob das Buch bzw. die Ausgabe schon angelegt wurden:</p>
            <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder Autor">
            <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
                <li class="ui-menu-item" role="presentation">
                    <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                        <img src="/Images/no-category-picture-50.png">
                        <div class="CatDescription">
                            <span class="cat-name">Thomas Mann:</span>
                            <span class="" style="font-weight: bold;">Der Zauberberg</span>
                            <span class="NumberQuestions">ISBN: 978-3103481099</span>
                        </div>
                    </a>
                </li>
                <li class="ui-menu-item" role="presentation">
                    <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                        <img src="/Images/no-category-picture-50.png">
                        <div class="CatDescription">
                            <span class="cat-name">Thomas Mann:</span>
                            <span class="" style="font-weight: bold;">Der Zauberberg</span>
                            <span class="NumberQuestions">ISBN: 978-3596294336</span>
                        </div>
                    </a>
                </li>
                <li class="ui-menu-item" role="presentation">
                    <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                        <img src="/Images/no-category-picture-50.png">
                        <div class="CatDescription">
                            <span class="cat-name">Thomas Mann:</span>
                            <span class="" style="font-weight: bold;">Der Zauberberg</span>
                            <span class="NumberQuestions">andere Ausgabe</span>
                        </div>
                    </a>
                </li>
                <li class="ui-menu-item" role="presentation">
                    <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                        <div class="CatDescription">
                            <span class="" style="font-weight: bold;">- kein Treffer -</span>
                            <span class="NumberQuestions">(Buch neu anlegen)</span>
                        </div>
                    </a>
                </li>
            </ul>
        </div>
    </div>
</div>
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class="Colored Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">
            2.1 Ausgabe ausgewählt
                            </a>
                          </h4>
                        </div>
                        <div id="collapse2" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->

                              
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class="Colored Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">
            2.2 Buch (andere Ausgabe) ausgewählt
           
                            </a>
                          </h4>
                        </div>
                        <div id="collapse3" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->

                              
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                        <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class="Colored Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse4">
            2.3 "Neues Buch anlegen (Kein Treffer)" ausgewählt
                            </a>
                          </h4>
                        </div>
                        <div id="collapse4" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->
                              
<div class="form-group">
    <label class="columnLabel control-label" for="">Autor: Nachname</label>
    <div class="columnControlsFull">
        <input class="form-control" name="" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="">Autor: Vorname</label>
    <div class="columnControlsFull">
        <input class="form-control" name="" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>
<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn">weiteren Autor hinzufügen</button>
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="">Buchtitel</label>
    <div class="columnControlsFull">
        <input class="form-control show-tooltip" name="" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="">Buchtitel</label>
    <div class="columnControlsFull">
        <input class="form-control show-tooltip" name="" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>

<div class="form-group">
    <label class="columnLabel control-label" for="">Vorlage</label>
    <div class="columnControlsFull">
        <input class="form-control show-tooltip" name="" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                    </div>
<!-- temp accordion html end-->
<div class="form-group">
    <label class="columnLabel control-label" for="ISBN_Nummer">ISBN - Nummer</label>
    <div class="col-xs-4">
        <input class="form-control show-tooltip" name="ISBN_Nummer" type="text" value="" title="help text" data-placement="right" data-trigger="focus">    
    </div>
</div>

<div class="form-group" style="padding-top: 20px; padding-bottom: 20px;">
    <label class="columnLabel control-label" for="Url">
        (Offizielle) Webseite zum Kapitel (z.B. vom Verlag oder Autor)
        <i class="fa fa-question-circle show-tooltip" 
           title="Falls es eine Seite zum Buch beim Verlag gibt, gib bitte hier den Link an" data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="Url" type="text" value="<%= Model.Url %>">
    </div>
    <label class="columnLabel control-label" for="UrlLinkText">
        Angezeigter Link-Text (optional)
        <i class="fa fa-question-circle show-tooltip" 
           title="Gib hier einen Text an, der den Link beschreibt, zum Beispiel 'Webseite des Autors zum Kapitel'. Lässt du das Feld leer, wird die Link-Adresse angezeigt." data-placement="<%= CssJs.TooltipPlacementLabel %>">
        </i>
    </label>
    <div class="columnControlsFull">
        <input class="form-control" name="UrlLinkText" type="text" maxlength="50" value="<%= Model.UrlLinkText %>">
    </div>
</div>
<div class="form-group">
    <label class="columnLabel control-label" for="WikipediaUrl">Wikipedia-Artikel</label>
    <div class="columnControlsFull">
        <input class="form-control" name="WikipediaUrl" type="text" value="<%= Model.WikipediaUrl %>">
    </div>
</div>