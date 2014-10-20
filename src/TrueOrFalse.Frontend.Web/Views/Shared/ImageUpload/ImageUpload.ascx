<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="modalImageUpload" class="modal">
    <div class="modal-dialog">
        <div class="modal-content">      
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3 class="modal-title">Bild hochladen</h3>
            </div>

            <div class="modal-body" id="modalBody">
                <div class="alert alert-info">
                    <strong>Achtung:</strong> Bildrechte sind ein sensibles Thema. 
                    Bitte lade nur Bilder hoch, die gemeinfrei sind oder die du selbst erstellt hast. 
                </div>
       
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-xs-6 xxs-stack">
                            <div class="radio">
                                <label>
                                    <input type="radio" checked="checked" name="imgSource" id="rdoImageWikimedia"/>Bilder von Wikimedia verwenden.
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-6 xxs-stack">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="imgSource" id="rdoImageUpload" />Eigene Bilder
                                </label>                        
                            </div>
                        </div>
                    </div>
                </div>

                <%-- Wikimedia --%>
                <div id="divWikimedia">
                    <div class="Clearfix" style="margin-top:10px;">
                        <p>
                            Bei Wikipedia/ Wikimedia sind viele Millionen Bilder zu finden, die frei genutzt werden können. 
                            Auf <a href="http://commons.wikimedia.org/wiki/Hauptseite?uselang=de">Wikimedia-Commons</a> kannst du gezielt nach Inhalten suchen. 
                        </p>
                        <p>
                            Tipp: Wenn du bei Wikipedia oder Wikimedia-Commons auf das gewünschte Bild klickst, kommst du zur Detailansicht. Kopiere einfach die Url dieser Seite.
                        </p>
                    </div>
        
                    <form class="form-horizontal" style="margin-top: 15px;">
                        <div class="form-group">
                            <label class="col-xs-4 col-sm-3 xxs-stack">Wikimedia-URL <i class="fa fa-question-circle show-tooltip tooltip-min-200" title="Hier kann für Bilder von Wikipedia/ Wikimedia wahlweise die Url der Detailseite, die Url der Bildanzeige im Media Viewer, die Url der Bilddatei oder der Dateiname (inkl. Dateiendung) angegeben werden." data-placement="top"></i></label>
                            <div class="col-xs-8 col-sm-9 xxs-stack">
                                <input class="input-sm" style="width: 100%" id="txtWikimediaUrl" type="text" placeholder="http://">
                            </div>
                        </div>
                    </form>
                    <div class="hide2" style="padding-left:114px; margin-top: 15px;" id="divWikimediaSpinner">
                        <i class="fa fa-spinner fa-spin fa-2x pull-left"></i><span style="position: relative; top: 1px;">Vorschau wird geladen</span>
                    </div>
                    <div class="hide2" style="padding-left:114px; margin-top: 15px;" id="divWikimediaError">
                        <i class="fa fa-warning" style="color:orange"></i> Das Bild konnte nicht geladen werden.
                    </div>
                
                    <div id="previewWikimediaImage" class="hide2" style="padding-left:109px; margin-top:7px;"></div>
                    
                </div>
        
                <div id="divUpload" class="hide2">
                    <div style="margin-top:10px;">
                        <div id="fileUpload" class="btn btn-success">
                            <i class="fa fa-arrow-circle-o-up"></i> Bild upload (klicke oder verwende drag und drop)
                        </div>
                    </div>
        
                    <div id="previewImage" class="hide2" style="margin-top:18px;"></div>
                    <div id="divUploadProgress" class="hide2" style="margin-top:8px;"></div>

                    <div id="divLegalInfo" class="hide2">
                
                        <div style="margin-top:18px;">
                            <b>Urheberrechtsinformation:</b><br/>
                            Wir benötigen Urheberrechtsinformationen für dieses Bild, damit wir sicherstellen können, 
                            dass Inhalte auf Richtig-oder-Falsch legal frei weiterverwendet werden können. 
                            Richtig-oder-Falsch folgt dem Wikipedia Prinzip [mehr erfahren].
                        </div>
        
                        <div style="width: 100%">
                            <div class="radio">
                                <label >
                                    <input type="radio" name="imgLiceneType" id="rdoLicenceByUloader"/>Diese Bild ist meine eigene Arbeit.
                                </label>                                 
                            </div>
                            <div id="divLicenceUploader" style="padding-left: 20px;" class="hide2">
                                Ich, <input type="text" class="form-control" id="txtLicenceOwner" name="txtLicenceOwner" 
                                        style="height: 19px; font-size: 13px; display: inline; width: 200px; margin-bottom: 0px; position: relative; top: -2px;"/> 
                                , der Rechteinhaber dieses Werks gewähre unwiderruflich jedem das Recht, 
                                es gemäß der „Creative Commons“-Lizenz „Namensnennung, Weitergabe 
                                unter gleichen Bedingungen 3.0“ <a href="http://creativecommons.org/licenses/by-sa/3.0/deed.de">(Text der Lizenz)</a> zu nutzen.
                            </div>
                        </div>
        
                        <div style="width: 100%">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="imgLiceneType" id="rdoLicenceForeign" />Dieses Bild ist nicht meine eigene Arbeit. 
                                </label>                                
                            </div>

                            <div id="divLicenceForeign" style="padding-left: 20px;" class="hide2">
                                Wir bitten Dich das Bild auf <a href="http://commons.wikimedia.org">Wikimedia</a> hochzuladen und so einzubinden. 
                            </div>
                        </div>                
                    </div>
                </div>
            </div>

            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="ButtonContainer float-none-xxs">
                    <a id="modalImageUploadDismiss" href="#" class="btn btn-default" data-dismiss="modal">Abbrechen</a>
                    <a href="#" class="btn btn-primary" id="aSaveImage">Bild speichern</a>
                </div>
            </div>
        </div>
    </div>
</div>