<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMetaData>" %>

<div id="modalImageMaintenance" class="modal">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">      
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3 class="modal-title">Bildverwaltung</h3>
            </div>

            <div class="modal-body" id="modalBody">
    <div>
        Id:
        <%= Model.Id %>
        <br/>
        <%= Model.Markup %>
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