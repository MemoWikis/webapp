<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>

<div id="modalImageDetail" class="modal">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3 class="modal-title">Bilddetail - Youtube-Vorschaubild</h3>
            </div>

            <div class="modal-body" id="modalBody">
                <div class="ImageContainer">
                    
                    <img src="<%= YoutubeVideo.GetPreviewImage(Model) %>" />

                    <div class="ImageInfo">
                        Youtube-Vorschaubild: <a href="<%= YoutubeVideo.GetUrl(Model) %>">Lizenzdetails auf Youtube</a>
                     </div>
                </div>
            </div>
            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="ButtonContainer float-none-xxs">
                    <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                </div>
            </div>
        </div>
    </div>
</div>