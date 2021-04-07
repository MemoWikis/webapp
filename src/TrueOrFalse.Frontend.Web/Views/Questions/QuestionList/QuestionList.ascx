<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<script type="text/x-template" id="pin-wuwi-template">
    <%: Html.Partial("~/Views/Shared/PinComponentVue/PinComponent.vue.ascx") %>
</script>
<%= Styles.Render("~/bundles/QuestionList") %>
<%= Styles.Render("~/bundles/switch") %>
<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>
<div id="QuestionListApp" class="row" v-cloak :class="{'no-questions': hasNoQuestions }">
    <div class="col-xs-12 drop-down-question-sort" v-show="questionsCount > 0">
        <div class="header">Du lernst <b>{{selectedQuestionCount}}</b> Fragen aus diesem Thema ({{allQuestionsCountFromCategory}})</div>
        <div id="ButtonAndDropdown">
            <%: Html.Partial("~/Views/Questions/QuestionList/SessionConfigComponent.vue.ascx", Model) %>

            <div id="QuestionListHeaderDropDown" class="Button dropdown">
            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                <li><a href="<%= Links.CreateQuestion(Model.CategoryId) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i><span>Frage hinzufügen</span></a></li>
                <li @click="toggleQuestionsList()" style="cursor: pointer"><a><i class="fa fa-angle-double-down"></i><span>Alle Fragen erweitern</span></a></li>
                <li style="cursor: pointer"><a data-allowed="logged-in" @click="startNewLearningSession()"><i class="fa fa-play"></i><span>Fragen jetzt lernen</span></a></li>
            </ul>
        </div>
        </div>
    </div>
    <%: Html.Partial("~/Views/Questions/QuestionList/QuestionListComponent.vue.ascx", Model) %>

    </div>

<%: Html.Partial("~/Views/Questions/AddQuestion/EditQuestionModal.vue.ascx") %>

<%= Scripts.Render("~/bundles/js/QuestionListApp") %>






