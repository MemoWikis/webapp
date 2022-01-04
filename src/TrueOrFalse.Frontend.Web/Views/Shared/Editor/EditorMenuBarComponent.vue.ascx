<div
    class="menubar is-hidden"
    :class="{ 'is-focused': focused }">

    <button
        class="menubar__button"
        :class="{ 'is-active':  editor.isActive('bold') }"
        @mousedown="command('bold')">
        <i class="fas fa-bold"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active':  editor.isActive('italic') }"
        @mousedown="command('italic')">
        <i class="fas fa-italic"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('strike') }"
        @mousedown="command('strike')">
        <i class="fas fa-strikethrough"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('underline') }"
        @mousedown="command('underline')">
        <i class="fas fa-underline"></i>
    </button>

    <button
        v-if="heading"
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('heading', { level: 2 })}"
        @mousedown="command('h2')">
        <b>H1</b>
    </button>

    <button
        v-if="heading"
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('heading', { level: 3 })}"
        @mousedown="command('h3')">
        <b>H2</b>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('bulletList') }"
        @mousedown="command('bulletList')">
        <i class="fas fa-list-ul"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('orderedList') }"
        @mousedown="command('orderedList')">
        <i class="fas fa-list-ol"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('blockquote') }"
        @mousedown="command('blockquote')">
        <i class="fas fa-quote-right"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('codeBlock') }"
        @mousedown="command('codeBlock')">
        <i class="fas fa-file-code"></i>
    </button>

    <button
        class="menubar__button"
        @mousedown="command('setLink')"
        :class="{ 'is-active': editor.isActive('link') }">
        <i class="fas fa-link"></i>
    </button>

    <button
        v-if="editor.isActive('link')"
        class="menubar__button"
        @mousedown="command('unsetLink')">
        <i class="fas fa-unlink"></i>
    </button>
    
    <button
        class="menubar__button"
        @mousedown="command('addImage')">
        <i class="far fa-image"></i>
    </button>

    <button
        class="menubar__button"
        @mousedown="command('horizontalRule')">
        <b>
            —
        </b>
    </button>

    <button
        class="menubar__button"
        @mousedown="command('undo')">
        <i class="fas fa-undo-alt"></i>
    </button>

    <button
        class="menubar__button"
        @mousedown="command('redo')">
        <i class="fas fa-redo-alt"></i>
    </button>

</div>