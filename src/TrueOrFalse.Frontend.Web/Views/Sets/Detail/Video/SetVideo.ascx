<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<div class="row">
    <div class="col-xs-12">
        <h4 style="margin-top:-10px; margin-bottom: 20px;">Video zum Fragesatz</h4>
    </div>
</div>
<div>
    <div class="row">
        <div class="col-md-12">
            <%= Html.Raw(YoutubeVideo.GetIframe("KZp15FYS_ds")) %>
        </div>
    </div>                
</div>
