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
        <h4 class="header">Du lernst <b>{{selectedQuestionCount}}</b> Fragen aus diesem Thema ({{allQuestionsCountFromCategory}})</h4>
        <div id="ButtonAndDropdown">
            <%: Html.Partial("~/Views/Questions/QuestionList/SessionConfigComponent.vue.ascx", Model) %>

            <div id="QuestionListHeaderDropDown" class="Button dropdown">
            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                <li><a onclick="eventBus.$emit('open-edit-question-modal', {
                            categoryId: <%= Model.CategoryId %>,
                            edit: false
                        })" data-allowed="logged-in">
                    <div class="dropdown-icon"><i class="fa fa-plus-circle"></i></div><span>Frage hinzufügen</span>
                </a></li>
                <li @click="toggleQuestionsList()" style="cursor: pointer"><a>
                    <div class="dropdown-icon"><i class="fa fa-angle-double-down"></i></div><span>Alle Fragen erweitern</span>
                </a></li>
                <li style="cursor: pointer"><a data-allowed="logged-in" @click="startNewLearningSession()">
                    <div class="dropdown-icon"><i class="fa fa-play"></i></div><span>Fragen jetzt lernen</span>
                </a></li>
            </ul>
        </div>
        </div>
    </div>
    <%: Html.Partial("~/Views/Questions/QuestionList/QuestionListComponent.vue.ascx", Model) %>
</div>

<%= Scripts.Render("~/bundles/js/QuestionListApp") %>






