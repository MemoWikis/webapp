<%@ Control Language="C#" AutoEventWireup="true"
Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%--<div id="InlineEditToolbar" :class="{sticky : footerIsVisible}">
    <div class="floatingActionBtn" :class="{sticky : footerIsVisible}">
        <div class="trigger" @click.prevent="setEditMode()" :class="{ open : editMode }">
            <i class="fas fa-pen"></i>  
        </div>
    </div>

    <div class="toolbar" v-cloak :class="{ open : editMode , sticky : footerIsVisible }">
        
        <div class="inner" :class="{sticky : footerIsVisible}">
            <div class="btnLeft">
                <div class="button" :class="{ expanded : editMode }">
                    <div class="icon">
                        <i class="fas fa-question"></i>
                    </div>
                    <div class="btnLabel">
                        Hilfe
                    </div>
                </div>
            </div>

            <div class="btnRight">
                <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded : editMode }">
                    <div class="icon">
                        <i class="fas fa-times"></i>
                    </div>
                    <div class="btnLabel">
                        Verwerfen
                    </div>
                </div>

                <div class="button" @click.prevent="saveContent()" :class="{ expanded : editMode }">
                    <div class="icon">
                        <i class="fas fa-save"></i>
                    </div>
                    <div class="btnLabel">
                        Speichern
                    </div>
                </div>

            </div>
        </div>
        
        <div class="pseudo-circle" :class="{ open : editMode }">
            <div class="dialog" v-show="editMode" :class="{ open : editMode }"></div>
        </div>

    </div>

</div>--%>

<div id="FloatingActionButtonApp" class="">
    <floating-action-button inline-template>
        <div id="FABContainer">
            <div id="MainFABContainer">
                <div id="MainFAB" @click="toggleFAB()" :class="{'in-edit-mode': editMode }">
                    <i class="fas fa-pen" :class="{'is-open': isOpen }"></i>
                    <i class="fas fa-times" :class="{'is-open': isOpen }"></i>
<%--                    <div>Bearbeiten</div>--%>
                </div>
                <div id="MiniFABContainer" :class="{'is-open': isOpen }" v-show="showMiniFAB">
                    
                    <div v-if="isTopicTab">
                        <div class="mini-fab-container" @click="editCategoryContent()">
                            <div class="mini-fab pop-fast" >
                                <i class="fas fa-edit"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Thema bearbeiten
                            </div>
                        </div>

                        <div class="mini-fab-container" onclick="location.href='<%= Links.CategoryCreate(Model.Id) %>';" data-allowed="logged-in">
                            <div class="mini-fab pop-normal">
                                <i class="far fa-plus-square"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Thema erstellen
                            </div>
                        </div>

                        <div class="mini-fab-container" onclick="location.href='<%= Links.CreateQuestion(categoryId: Model.Id) %>';">
                            <div class="mini-fab pop-slow">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-slow">
                                Frage hinzufügen
                            </div>
                        </div>
                    </div>
                    
                    <div v-else>
                        
                        <div class="mini-fab-container" href="">
                            <div class="mini-fab pop-fast">
                                <i class="fas fa-pen"></i>
                            </div>
                            <div class="mini-fab-label pop-fast">
                                Frage bearbeiten
                            </div>

                        </div>

                        <div class="mini-fab-container" onclick="location.href='<%= Links.CreateQuestion(categoryId: Model.Id) %>';">
                            <div class="mini-fab pop-normal">
                                <i class="fas fa-plus"></i>
                            </div>
                            <div class="mini-fab-label pop-normal">
                                Frage hinzufügen
                            </div>
                        </div>
                        
                        <div class="mini-fab-container" onclick="location.href='<%= Links.CategoryCreate(Model.Id) %>';" data-allowed="logged-in">
                            <div class="mini-fab pop-slow">
                                <i class="far fa-plus-square"></i>
                            </div>
                            <div class="mini-fab-label pop-slow">
                                Thema erstellen
                            </div>
                        </div>

                    </div>


                </div>

            </div>
            <div id="EditModeBar" v-show="editMode">
                <div class="toolbar" :class="{'pseudo-sticky' : footerIsVisible}">
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

                <%--<div class="toolbar" v-cloak :class="{ open : editMode , sticky : footerIsVisible }">
        
                    <div class="inner" :class="{sticky : footerIsVisible}">
                        <div class="btnLeft">
                            <div class="button" :class="{ expanded : editMode }">
                                <div class="icon">
                                    <i class="fas fa-question"></i>
                                </div>
                                <div class="btnLabel">
                                    Hilfe
                                </div>
                            </div>
                        </div>

                        <div class="btnRight">
                            <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded : editMode }">
                                <div class="icon">
                                    <i class="fas fa-times"></i>
                                </div>
                                <div class="btnLabel">
                                    Verwerfen
                                </div>
                            </div>

                            <div class="button" @click.prevent="saveContent()" :class="{ expanded : editMode }">
                                <div class="icon">
                                    <i class="fas fa-save"></i>
                                </div>
                                <div class="btnLabel">
                                    Speichern
                                </div>
                            </div>

                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </floating-action-button>
    
</div>