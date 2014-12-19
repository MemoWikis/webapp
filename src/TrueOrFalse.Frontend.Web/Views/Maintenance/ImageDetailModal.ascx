<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ImageFrontendData>" %>
<%@ Import Namespace="TrueOrFalse" %>

<div id="modalImageDetail" class="modal">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3 class="modal-title">Bilddetailanzeige</h3>
            </div>

            <div class="modal-body" id="modalBody">
                <img src="<%= Model.GetImageUrl(1000).Url %>" style="display: block; float: left; padding-right: 10px; padding-bottom: 5px;"/>
            </div>
            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="ButtonContainer float-none-xxs">
                    <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                </div>
            </div>
        </div>
    </div>
</div>
<script id="ModalImageDetailScript" type="text/javascript">
    $(function() {
        fnInitPopover($('#modalImageDetail'));
    });
</script>