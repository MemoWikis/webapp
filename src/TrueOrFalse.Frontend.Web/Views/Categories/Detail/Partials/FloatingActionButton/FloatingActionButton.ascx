<%@ Control Language="C#" AutoEventWireup="true"
Inherits="System.Web.Mvc.ViewUserControl<FloatingActionButtonModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<floating-action-button inline-template is-topic-tab="<%= Model.IsTopicTab %>" create-category-url="<%= Links.CategoryCreate(Model.Category.Id) %>" create-question-url="<%= Links.CreateQuestion(categoryId: Model.Category.Id) %>" ref="fabContainer">
        <div class="fab-container" v-show="contentIsReady">
            <div class="main-fab-container">
                <div class="main-fab" @click="toggleFAB()" :class="{'in-edit-mode': editMode && showFAB, 'is-sticky': footerIsVisible && editMode && showFAB, 'extended': isExtended }" v-show="showFAB" v-ripple="{center,class: 'r-green'}">
                    <div class="fab-label" :class="{'extended': isExtended }">{{fabLabel}}</div>
                    <div class="fab-icon-container" :class="{'extended': isExtended }">
                        <i class="fas fa-pen" :class="{'is-open': isOpen }"></i>
                        <i class="fas fa-times" :class="{'is-open': isOpen }"></i>
                    </div>

                </div>
                
                <div class="mini-fab-list" :class="{'is-open': isOpen }" v-show="showMiniFAB && showFAB">
                    
                    <% if (Model.IsTopicTab)
                       { %>
                        <div class="mini-fab-container" @click="editCategoryContent()" v-ripple="{center,class: 'r-white'}">
                            <div class="mini-fab pop-fast" >
                                <i class="fas fa-edit"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Thema bearbeiten
                            </div>
                        </div>

                        <div class="mini-fab-container" @click="createCategory()" v-ripple="{center,class: 'r-white'}">
                            <div class="mini-fab pop-normal">
                                <i class="far fa-plus-square"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Thema erstellen
                            </div>
                        </div>

                        <div class="mini-fab-container" @click="createQuestion()" v-ripple="{center,class: 'r-white'}">
                            <div class="mini-fab pop-slow">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-slow">
                                Frage hinzufügen
                            </div>
                        </div><% }
                       else
                       { %>
                                        
                        <div class="mini-fab-container" @click="editQuestion()" v-if="showEditQuestionButton" v-ripple="{center,class: 'r-white'}">
                            <div class="mini-fab pop-fast">
                                <i class="fas fa-pen"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Frage bearbeiten
                            </div>

                        </div>

                        <div class="mini-fab-container" @click="createQuestion()" v-ripple="{center,class: 'r-white'}">
                            <div class="mini-fab pop-normal">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Frage hinzufügen
                            </div>
                        </div>
                
                        <div class="mini-fab-container" @click="createCategory()" v-ripple="{center,class: 'r-white'}">
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
                <div class="edit-mode-bar-container" v-show="showBar">
                    <div class="toolbar" :class="{'pseudo-sticky' : footerIsVisible, 'is-hidden' : !editMode}">
                        <div class="toolbar-btn-container">
                            <div class="btn-left">
                                <%--<div class="button" :class="{ expanded : editMode }" v-ripple="{center,class: 'r-green'}">
                                    <div class="icon">
                                        <i class="fas fa-question"></i>
                                    </div>
                                    <div class="btn-label">
                                        Hilfe
                                    </div>
                                </div>--%>
                            </div>

                            <div class="btn-right">
                            
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