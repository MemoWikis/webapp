<script lang="ts" setup>
import { ref } from 'vue'
const props = defineProps(['editor', 'heading'])
const focused = ref(false)

async function command(fn, e) {
    e.preventDefault();
    switch (fn) {
        case 'bold':
            props.editor.commands.toggleBold()
            break
        case 'italic':
            props.editor.commands.toggleItalic()
            break
        case 'strike':
            props.editor.commands.toggleStrike()
            break
        case 'underline':
            props.editor.commands.toggleUnderline()
            break
        case 'h2':
            props.editor.commands.toggleHeading({ level: 2 })
            break
        case 'h3':
            props.editor.commands.toggleHeading({ level: 3 })
            break
        case 'bulletList':
            props.editor.commands.toggleBulletList()
            break
        case 'orderedList':
            props.editor.commands.toggleOrderedList()
            break
        case 'blockquote':
            props.editor.commands.toggleBlockquote()
            break
        case 'codeBlock':
            props.editor.commands.toggleCodeBlock()
            break
        case 'setLink':
            const linkUrl = window.prompt('Link URL')
            props.editor.commands.extendMarkRange('link').setLink({ href: linkUrl })
            if (props.editor.view.state.selection.empty) {
                var transaction = this.editor.state.tr.insertText(linkUrl);
                props.editor.view.dispatch(transaction)
            }
            break;
        case 'unsetLink':
            props.editor.commands.unsetLink().focus()
            break
        case 'addImage':
            const imgUrl = window.prompt('Bild URL')
            props.editor.commands.setImage({ src: imgUrl })
            break
        case 'horizontalRule':
            props.editor.commands.setHorizontalRule()
            break
        case 'undo':
            props.editor.commands.undo()
            break
        case 'redo':
            props.editor.commands.redo()
            break
    }
    await nextTick()
    props.editor.commands.focus()
}

props.editor.on('focus', () => {
    focused.value = true
})
props.editor.on('blur', () => {
    focused.value = false
})
</script>
<template>

    <floating-menu :editor="props.editor" :tippy-options="{ duration: 100 }" v-if="props.editor">
        <div class="menubar is-hidden" :class="{ 'is-focused': focused }">

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('bold') }"
                @mousedown="command('bold', $event)" @mouseup="props.editor.commands.focus()">
                <font-awesome-icon icon="fa-solid fa-bold" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('italic') }"
                @mousedown="command('italic', $event)">
                <font-awesome-icon icon="fa-solid fa-italic" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('strike') }"
                @mousedown="command('strike', $event)">
                <font-awesome-icon icon="fa-solid fa-strikethrough" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('underline') }"
                @mousedown="command('underline', $event)">
                <font-awesome-icon icon="fa-solid fa-underline" />
            </button>

            <button v-if="heading" class="menubar__button"
                :class="{ 'is-active': props.editor.isActive('heading', { level: 2 }) }"
                @mousedown="command('h2', $event)">
                <b>H1</b>
            </button>

            <button v-if="heading" class="menubar__button"
                :class="{ 'is-active': props.editor.isActive('heading', { level: 3 }) }"
                @mousedown="command('h3', $event)">
                <b>H2</b>
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('bulletList') }"
                @mousedown="command('bulletList', $event)">
                <font-awesome-icon icon="fa-solid fa-list-ul" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('orderedList') }"
                @mousedown="command('orderedList', $event)">
                <font-awesome-icon icon="fa-solid fa-list-ol" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('blockquote') }"
                @mousedown="command('blockquote', $event)">
                <font-awesome-icon icon="fa-solid fa-quote-right" />
            </button>

            <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('codeBlock') }"
                @mousedown="command('codeBlock', $event)">
                <font-awesome-icon icon="fa-solid fa-code" />
            </button>

            <button class="menubar__button" @mousedown="command('setLink', $event)"
                :class="{ 'is-active': props.editor.isActive('link') }">
                <font-awesome-icon icon="fa-solid fa-link" />
            </button>

            <button v-if="props.editor.isActive('link')" class="menubar__button"
                @mousedown="command('unsetLink', $event)">
                <font-awesome-icon icon="fa-solid fa-link-slash" />
            </button>

            <button class="menubar__button" @mousedown="command('addImage', $event)">
                <font-awesome-icon icon="fa-solid fa-image" />
            </button>

            <button class="menubar__button" @mousedown="command('horizontalRule', $event)">
                <b>
                    —
                </b>
            </button>

            <button class="menubar__button" @mousedown="command('undo', $event)">
                <font-awesome-icon icon="fa-solid fa-rotate-left" />
            </button>

            <button class="menubar__button" @mousedown="command('redo', $event)">
                <font-awesome-icon icon="fa-solid fa-rotate-right" />
            </button>

        </div>
    </floating-menu>

</template>

<style lang="less" scoped>
.menubar {
    top: 60px;
    position: sticky;
    background: white;
    margin-top: -36px;
    z-index: 10;
    box-shadow: 0 6px 6px -6px rgba(0, 0, 0, 0.16);
    border-radius: 2px;
    font-size: 0;
    height: 36px;

    &.is-hidden {
        visibility: hidden;
        opacity: 0;
        pointer-events: none;
    }

    &.is-focused {
        visibility: visible;
        opacity: 1;
        transition: visibility .2s, opacity .2s;
        pointer-events: auto !important;
    }
}
</style>