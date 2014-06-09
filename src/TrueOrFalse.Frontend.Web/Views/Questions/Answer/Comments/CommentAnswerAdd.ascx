<%@ Control Language="C#" Inherits="ViewUserControl<CommentAnswerAddModel>" %>

<div class="panel-body" style="position: relative">
    <div class="col-lg-2">
        <img class="pull-right" style="width:50%;  border-radius:5px;" src="<%= Model.AuthorImageUrl %>">
    </div>
    <div class="col-lg-10" style="height: 100%; padding-bottom: 8px; ">
        <textarea style="width: 100%;" class="form-control" placeholder="Bitte höflich, freundlich und sachlich schreiben :-)"></textarea>
    </div>
        
    <div class="col-lg-12">
        <a class="btn btn-default pull-right saveAnswer" href="#">Speichern</a>
    </div>
</div>