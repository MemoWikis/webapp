<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

<text-component content="<%: HttpUtility.HtmlDecode(Model.Content)  %>" inline-template>
    <div>
    <editor-menu-bar :editor="editor" v-slot="{ commands, isActive, focused }" v-show="editMode">
      <div
        class="menubar is-hidden"
        :class="{ 'is-focused': focused }"
          v-if="focused"
      >

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.bold() }"
          @click="commands.bold"
        >
            <i class="fas fa-bold"></i>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.italic() }"
          @click="commands.italic"
        >
            <i class="fas fa-italic"></i>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.strike() }"
          @click="commands.strike"
        >
            <i class="fas fa-strikethrough"></i>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.underline() }"
          @click="commands.underline"
        >
            <i class="fas fa-underline"></i>
        </button>

          <button
          class="menubar__button"
          :class="{ 'is-active': isActive.paragraph() }"
          @click="commands.paragraph"
        >
              <i class="fas fa-paragraph"></i>
        </button>

          <button
          class="menubar__button"
          :class="{ 'is-active': isActive.heading({ level: 2 }) }"
          @click="commands.heading({ level: 2 })"
        >
          H2
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.heading({ level: 3 }) }"
          @click="commands.heading({ level: 3 })"
        >
          H3
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.bullet_list() }"
          @click="commands.bullet_list"
        >
            <i class="fas fa-list-ul"></i>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.ordered_list() }"
          @click="commands.ordered_list"
        >
            <i class="fas fa-list-ol"></i>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.blockquote() }"
          @click="commands.blockquote"
        >
            <i class="fas fa-quote-right"></i>
        </button>
          
          <button
              class="menubar__button"
              :class="{ 'is-active': isActive.code() }"
              @click="commands.code"
          >
              <i class="fas fa-code"></i>
          </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.code_block() }"
          @click="commands.code_block"
        >
            <i class="far fa-file-code"></i>
        </button>
          
          <button
              class="menubar__button"
              @click="commands.horizontal_rule"
          >
              <i class="far fa-window-minimize"></i>
          </button>
          
          <button
              class="menubar__button"
              @click="commands.undo"
          >
              <i class="fas fa-undo-alt"></i>
          </button>

          <button
              class="menubar__button"
              @click="commands.redo"
          >
              <i class="fas fa-redo-alt"></i>
          </button>

      </div>
    </editor-menu-bar>
        
        <editor-floating-menu :editor="editor" v-slot="{ commands, isActive, menu, focused }">
            <div
                class="editor__floating-menu"
                :class="{ 'is-active': menu.isActive }"
                :style="`top: ${menu.top}px`"
                v-if="focused"
            >

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.heading({ level: 2 }) }"
                    @click="commands.heading({ level: 2 })"
                >
                    H2
                </button>

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.heading({ level: 3 }) }"
                    @click="commands.heading({ level: 3 })"
                >
                    H3
                </button>

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.bullet_list() }"
                    @click="commands.bullet_list"
                >
                    <i class="fas fa-list-ul"></i>
                </button>

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.ordered_list() }"
                    @click="commands.ordered_list"
                >
                    <i class="fas fa-list-ol"></i>
                </button>

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.blockquote() }"
                    @click="commands.blockquote"
                >
                    <i class="fas fa-quote-right"></i>
                </button>

                <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.code_block() }"
                    @click="commands.code_block"
                >
                    <i class="far fa-file-code"></i>
                </button>

            </div>
        </editor-floating-menu>
    <editor-content :editor="editor">
        <%: Html.Raw(Model.Content)  %>
    </editor-content>
    </div>

</text-component>
     

<%--    <div v-if="textCanBeEdited">
        <inline-text-component/>
    </div>
    <div v-else @click="editInlineText()">
            <%: Html.Raw(HttpUtility.HtmlDecode(Model.Content))  %>
        <div v-if="missingText" class="missingTextInModule"> Hier klicken um Text zu bearbeiten</div>
    </div>--%>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>