<%@ Control Language="C#" AutoEventWireup="true"
Inherits="System.Web.Mvc.ViewUserControl<FloatingActionButtonModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<floating-action-button inline-template is-topic-tab="<%= Model.IsTopicTab %>" create-category-url="<%= Links.CategoryCreate(Model.Category.Id) %>" create-question-url="<%= Links.CreateQuestion(categoryId: Model.Category.Id) %>">
        <div class="fab-container">
            <div class="main-fab-container">
                <div class="main-fab" @click="toggleFAB()" :class="{'in-edit-mode': editMode && showFab, 'is-sticky': footerIsVisible && editMode && showFab  }" v-show="showFab">
<%--                    <div class="fab-label">Bearbeiten</div>--%>
                    <i class="fas fa-pen" :class="{'is-open': isOpen }"></i>
<%--                    <div class="fab-label">Abbrechen</div>--%>
                    <i class="fas fa-times" :class="{'is-open': isOpen }"></i>
                </div>
                <div class="mini-fab-list" :class="{'is-open': isOpen }" v-show="showMiniFAB">
                    
                    <% if (Model.IsTopicTab)
                       { %>
                        <div class="mini-fab-container" @click="editCategoryContent()">
                            <div class="mini-fab pop-fast" >
                                <i class="fas fa-edit"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Thema bearbeiten
                            </div>
                        </div>

                        <div class="mini-fab-container" @click="createCategory()">
                            <div class="mini-fab pop-normal">
                                <i class="far fa-plus-square"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Thema erstellen
                            </div>
                        </div>

                        <div class="mini-fab-container" @click="createQuestion()">
                            <div class="mini-fab pop-slow">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-slow">
                                Frage hinzufügen
                            </div>
                        </div><% }
                       else
                       { %>
                                        
                        <div class="mini-fab-container" @click="editQuestion()" v-if="showEditQuestionButton">
                            <div class="mini-fab pop-fast">
                                <i class="fas fa-pen"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Frage bearbeiten
                            </div>

                        </div>

                        <div class="mini-fab-container" @click="createQuestion()">
                            <div class="mini-fab pop-normal">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Frage hinzufügen
                            </div>
                        </div>
                
                        <div class="mini-fab-container" @click="createCategory()">
                            <div class="mini-fab pop-slow">
                                <i class="far fa-plus-square"></i>
                            </div>
                            <div class="mini-fab-label pop-slow">
                                Thema erstellen
                            </div>
                        </div>
                    <% } %>
                    
                        
                </div>

            </div>
            <%if (Model.IsTopicTab) {%>
                <div class="edit-mode-bar-container">
                    <div class="toolbar" :class="{'pseudo-sticky' : footerIsVisible, 'is-hidden' : !editMode}">
                        <div id="ButtonContainer">
                            <div class="btn-left">
                                <div class="button" :class="{ expanded : editMode }">
                                    <div class="icon">
                                        <i class="fas fa-question"></i>
                                    </div>
                                    <div class="btn-label">
                                        Hilfe
                                    </div>
                                </div>
                            </div>

                            <div class="btn-right">
                            
                                <div class="button" @click.prevent="saveContent()" :class="{ expanded : editMode }">
                                    <div class="icon">
                                        <i class="fas fa-save"></i>
                                    </div>
                                    <div class="btn-label">
                                        Speichern
                                    </div>
                                </div>

                                <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded : editMode }">
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