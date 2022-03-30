<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<script type="text/x-template" id="pin-wuwi-template">
    <%: Html.Partial("~/Views/Shared/PinComponentVue/PinComponent.vue.ascx") %>
</script>
<%: Html.Partial("~/Views/Questions/Modals/QuestionCommentSectionModalTemplate.vue.ascx") %>
<%= Styles.Render("~/bundles/QuestionList") %>
<%= Styles.Render("~/bundles/switch") %>
<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>

<div id="QuestionListApp" class="row" v-cloak>
    <session-config-component :is-logged-in="'<%= SessionUser.IsLoggedIn %>' == 'True'" :is-in-question-list="true" v-if="showFilter">
        <input id="hdnIsTestMode" hidden :value="isTestMode"/>
        <div class="col-xs-12 drop-down-question-sort">
            <div class="session-config-header">
                <span class="hidden-xs">Du lernst</span>
                <template v-if="currentQuestionCount == allQuestionCount">
                    <b>&nbsp;alle&nbsp;</b>
                </template>
                <template v-else>
                    <b>&nbsp;{{currentQuestionCount}}&nbsp;</b>
                </template>
                Fragen&nbsp;
                <span class="hidden-xs">aus diesem Thema</span>
                &nbsp;({{allQuestionCount}})
            </div>
            <div class="session-config-header" v-if="categoryHasNoQuestions && showError">Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.</div>

            <div id="ButtonAndDropdown">
                <div id="QuestionListHeaderDropDown" class="Button dropdown">
                    <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                        <li onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.CategoryId %>, edit: false })" data-allowed="logged-in">
                            <a>
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div><span>Frage hinzufügen</span>
                            </a>
                        </li>
                        <li v-if="isQuestionListToShow" @click="toggleQuestionsList()" style="cursor: pointer">
                            <a>
                                <div class="dropdown-icon">
                                    <i class="fa fa-angle-double-up"></i>
                                </div><span>Alle Fragen zuklappen</span>
                            </a>
                        </li>
                        <li v-else @click="toggleQuestionsList()" style="cursor: pointer">
                            <a>
                                <div class="dropdown-icon">
                                    <i class="fa fa-angle-double-down"></i>
                                </div><span>Alle Fragen erweitern</span>
                            </a>
                        </li>
                        <li style="cursor: pointer">
                            <a data-allowed="logged-in" @click="startNewLearningSession()">
                                <div class="dropdown-icon">
                                    <i class="fa fa-play"></i>
                                </div><span>Fragen jetzt lernen</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

    </session-config-component>
    <div class="session-configurator col-xs-12" v-else>
        <div class="session-config-header">
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                </div>
            </div>
        </div>
    </div>


    <%: Html.Partial("~/Views/Questions/QuestionList/QuestionListComponent.vue.ascx", Model) %>
    <div>
        <template>
            <question-comment-section-modal-component :comment-is-loaded="commentIsLoaded" :question-id="commentQuestionId"/>
        </template>
        <template>
            <delete-question-component/>
        </template>
    </div>
</div>

<%= Scripts.Render("~/bundles/js/QuestionListApp") %>