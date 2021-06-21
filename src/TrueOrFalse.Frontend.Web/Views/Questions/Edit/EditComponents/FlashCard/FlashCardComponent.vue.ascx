<flashcard-component inline-template :solution="flashCardJson">
    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>

        <editor-menu-bar :editor="answerEditor" v-slot="{ commands, isActive, focused }">
            <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
        </editor-menu-bar>
        <editor-content :editor="answerEditor" />

    </div>
</flashcard-component>
