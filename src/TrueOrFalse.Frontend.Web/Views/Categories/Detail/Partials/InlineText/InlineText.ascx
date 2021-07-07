<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>
<keep-alive>
    <text-component content="<%: HttpUtility.HtmlDecode(Model.Content)%>" inline-template>
    <div class="inline-text-editor" @click="contentIsChanged = true">
        <button
            class="menubar__button"
            :class="{ 'is-active': isActive.bold() }"
            @click="editor.toggleBold()"
        >
            <i class="fas fa-bold"></i>
        </button>
<%--        <editor-menu-bar :editor="editor" v-slot="{ commands, isActive, focused, getMarkAttrs }">
          <div
        class="menubar is-hidden"
        :class="{ 'is-focused': focused }"
      >

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.bold() }"
          @click="editor.bold"
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
              <b>
                  H2

              </b>
        </button>

        <button
          class="menubar__button"
          :class="{ 'is-active': isActive.heading({ level: 3 }) }"
          @click="commands.heading({ level: 3 })"
        >
            <b>
                H3
            </b>
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
            <i class="fas fa-file-code"></i>
        </button>
              
              <button
                  data-toggle="modal" data-target="#inlineEditLinkModal"
                  class="menubar__button"
                  @click="showLinkMenu(getMarkAttrs('link'))"
                  :class="{ 'is-active': isActive.link() }"
              >
                  <i class="fas fa-link"></i>
              </button>

              <button
              class="menubar__button"
              @click="commands.horizontal_rule"
          >
              <b>
                  —
              </b>
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
        </editor-menu-bar>--%>
        
        <editor-menu-bubble class="link-modal" :editor="editor" @hide="hideLinkMenu" v-slot="{ commands, isActive, getMarkAttrs, menu }">
            <div class="modal fade" id="inlineEditLinkModal" tabindex="-1" role="dialog" aria-labelledby="inlineEditLinkModalLabel">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body">
                            <form class="link-form" @submit.prevent="setLinkUrl(commands.link, linkUrl)">
                                <input class="link-input" type="text" v-model="linkUrl" placeholder="https://" ref="linkInput" @keydown.esc="hideLinkMenu"/>
                                <button class="link-btn btn btn-danger" @click="setLinkUrl(commands.link, null)" type="button">
                                    <i class="far fa-times-circle"></i>
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </editor-menu-bubble>



        <editor-content :editor="editor">
            <%: Html.Raw(Model.Content)  %>
        </editor-content>
    </div>

</text-component>
</keep-alive>
