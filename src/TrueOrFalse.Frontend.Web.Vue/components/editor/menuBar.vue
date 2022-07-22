<script lang="ts" setup>
import { ref } from 'vue'
const props = defineProps(['editor', 'heading'])
const focused = ref(false)
const clicked = ref(false)
function command(fn) {
    switch (fn) {
        case 'bold':
            props.editor.commands.bold();
            break;
        case 'italic':
            props.editor.value?.chain().toggleItalic().focus().run();
            break;
        case 'strike':
            props.editor.value?.chain().toggleStrike().focus().run();
            break;
        case 'underline':
            props.editor.value?.chain().toggleUnderline().focus().run();
            break;
        case 'h2':
            props.editor.value?.chain().toggleHeading({ level: 2 }).focus().run();
            break;
        case 'h3':
            props.editor.value?.chain().toggleHeading({ level: 3 }).focus().run();
            break;
        case 'bulletList':
            props.editor.value?.chain().toggleBulletList().focus().run();
            break;
        case 'orderedList':
            props.editor.value?.chain().toggleOrderedList().focus().run();
            break;
        case 'blockquote':
            props.editor.value?.chain().toggleBlockquote().focus().run();
            break;
        case 'codeBlock':
            props.editor.value?.chain().toggleCodeBlock().focus().run();
            break;
        case 'setLink':
            const linkUrl = window.prompt('Link URL');
            props.editor.value?.chain().focus().extendMarkRange('link').setLink({ href: linkUrl }).run();
            if (props.editor.value?.view.state.selection.empty) {
                var transaction = this.editor.state.tr.insertText(linkUrl);
                props.editor.value?.view.dispatch(transaction);
            }
            break;
        case 'unsetLink':
            props.editor.value?.chain().focus().unsetLink().run();
            break;
        case 'addImage':
            const imgUrl = window.prompt('Bild URL');
            props.editor.value?.chain().focus().setImage({ src: imgUrl }).run();
            break;
        case 'horizontalRule':
            props.editor.value?.chain().setHorizontalRule().focus().run();;
            break;
        case 'undo':
            props.editor.value?.chain().undo().focus().run();
            break;
        case 'redo':
            props.editor.value?.chain().redo().focus().run();
            break;
    }
    clicked.value = true;
}
</script>
<template>

    <floating-menu :editor="props.editor" :tippy-options="{ duration: 100 }" v-if="props.editor">
        <div class="menubar is-hidden" :class="{ 'is-focused': focused }">

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('bold') }"
                @mousedown="command('bold')">
                <font-awesome-icon icon="fa-solid fa-bold" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('italic') }"
                @mousedown="command('italic')">
                <font-awesome-icon icon="fa-solid fa-italic" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('strike') }"
                @mousedown="command('strike')">
                <font-awesome-icon icon="fa-solid fa-strikethrough" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('underline') }"
                @mousedown="command('underline')">
                <font-awesome-icon icon="fa-solid fa-underline" />
            </button>

            <button v-if="heading" class="menubar__button"
                :class="{ 'is-active': props.editor.isActive('heading', { level: 2 }) }" @mousedown="command('h2')">
                <b>H1</b>
            </button>

            <button v-if="heading" class="menubar__button"
                :class="{ 'is-active': props.editor.isActive('heading', { level: 3 }) }" @mousedown="command('h3')">
                <b>H2</b>
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('bulletList') }"
                @mousedown="command('bulletList')">
                <font-awesome-icon icon="fa-solid fa-list-ul" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('orderedList') }"
                @mousedown="command('orderedList')">
                <font-awesome-icon icon="fa-solid fa-list-ol" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('blockquote') }"
                @mousedown="command('blockquote')">
                <font-awesome-icon icon="fa-solid fa-quote-right" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('codeBlock') }"
                @mousedown="command('codeBlock')">
                <font-awesome-icon icon="fa-solid fa-code" />
            </button>

            <button class="menubar__button" @mousedown="command('setLink')"
                :class="{ 'is-active': props.editor.isActive('link') }">
                <font-awesome-icon icon="fa-solid fa-link" />
            </button>

            <button v-if="props.editor.isActive('link')" class="menubar__button" @mousedown="command('unsetLink')">
                <font-awesome-icon icon="fa-solid fa-link-slash" />
            </button>

            <button class="menubar__button" @mousedown="command('addImage')">
                <font-awesome-icon icon="fa-solid fa-image" />
            </button>

            <button class="menubar__button" @mousedown="command('horizontalRule')">
                <b>
                    —
                </b>
            </button>

            <button class="menubar__button" @mousedown="command('undo')">
                <font-awesome-icon icon="fa-solid fa-rotate-left" />
            </button>

            <button class="menubar__button" @mousedown="command('redo')">
                <font-awesome-icon icon="fa-solid fa-rotate-right" />
            </button>

        </div>
    </floating-menu>

</template>