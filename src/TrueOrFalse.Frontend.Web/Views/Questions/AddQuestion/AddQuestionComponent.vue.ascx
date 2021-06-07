<%@ Control Language="C#" Inherits="ViewUserControl<AddQuestionComponentModel>" %>
<add-question-component inline-template current-category-id="<%: Model.CategoryId %>">
    <div id="AddInlineQuestionContainer" v-if="isLoggedIn">

        <div id="AddQuestionHeader" class="">
            <div class="add-inline-question-label main-label">
                Frage hinzufügen 
                <span>(Karteikarte)</span>
                <div @click="createQuestion()">erweiterte Optionen</div>
            </div>
            <div class="heart-container wuwi-red" @click="addToWishknowledge = !addToWishknowledge">
                <div>
                    <i class="fa fa-heart" :class="" v-if="addToWishknowledge"></i>
                    <i class="fa fa-heart-o" :class="" v-else></i>
                </div>
                <div class="Text">
                    <span v-if="addToWishknowledge">Hinzugefügt</span>
                    <span v-else class="wuwi-grey">Hinzufügen</span>
                </div>
            </div>
        </div>

        <div id="AddQuestionBody">
            <div id="AddQuestionFormContainer"  class="inline-question-editor">
                <div>
                    <div class="add-inline-question-label s-label">Frage</div>
                    <editor-menu-bar :editor="questionEditor" v-slot="{ commands, isActive, focused }">
                        <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
                    </editor-menu-bar>
                    
                    <editor-content :editor="questionEditor" />
                </div>
                <div>
                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/FlashCard/FlashCardComponent.vue.ascx") %>

                </div>
                <div id="AddQuestionPrivacyContainer">
                <div class="add-inline-question-label s-label">                
                    Sichtbarkeit
                </div>
                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/PrivacySelector.vue.ascx") %>

            </div>
                <div>
                    <div class="btn btn-lg btn-primary memo-button" @click="addFlashcard()">Hinzufügen</div>
                </div>
            </div>
        </div>
    </div>
</add-question-component>