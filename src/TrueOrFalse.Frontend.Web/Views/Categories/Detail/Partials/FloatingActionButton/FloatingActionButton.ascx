<%@ Control Language="C#" AutoEventWireup="true"
Inherits="System.Web.Mvc.ViewUserControl<FloatingActionButtonModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<floating-action-button inline-template is-topic-tab="<%= Model.IsTopicTab %>" category-id="<%= Model.Category.Id %>" create-question-url="<%= Links.CreateQuestion(categoryId: Model.Category.Id) %>" ref="fabContainer" v-cloak>
        <div class="fab-container" v-show="contentIsReady">
            <%if (Model.IsTopicTab) {%>
                <div class="edit-mode-bar-container" v-show="showBar">
                    <div class="toolbar" :class="{'pseudo-sticky' : footerIsVisible, 'is-hidden' : !editMode, 'shrink' : shrink, 'expand' : expand }" :style="{ width: width + 'px' }">
                        <div class="toolbar-btn-container">
                            <div class="btn-left">
                            </div>
                            <div class="centerText" v-show="showLoginReminder">
                                <div>
                                    Um zu speichern, musst du&nbsp;<a href="#" data-btn-login="true" style="padding-top: 4px">angemeldet</a>&nbsp;sein.
                                </div>
                            </div>

                            <div v-if="showSaveMsg" class="saveMsg">
                                <div>
                                    {{saveMsg}}
                                </div>
                            </div>

                            <div class="btn-right" v-show="contentHasChanged" v-else>
                                <div class="button" @click.prevent="saveContent()" :class="{ expanded : editMode }" v-ripple="{center,class: 'r-green'}">
                                    <div class="icon">
                                        <i class="fas fa-save"></i>
                                    </div>
                                    <div class="btn-label">
                                        Speichern
                                    </div>
                                </div>

                                <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded : editMode }" v-ripple="{center,class: 'r-green'}">
                                    <div class="icon">
                                        <i class="fas fa-times"></i>
                                    </div>
                                    <div class="btn-label">
                                        Verwerfen
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            <%} %>
        </div>
    </floating-action-button>

