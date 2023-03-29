<script lang="ts" setup>
import { Editor } from '@tiptap/vue-3';

interface Props {
    editor: Editor
    heading?: boolean
}
const props = defineProps<Props>()
const focused = ref(false)

async function command(commandString: string, e: Event) {
    e.preventDefault()
    switch (commandString) {
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
            if (linkUrl) {
                props.editor.chain().extendMarkRange('link').setLink({ href: linkUrl })
                if (props.editor.view.state.selection.empty) {
                    var transaction = props.editor.state.tr.insertText(linkUrl)
                    props.editor.view.dispatch(transaction)
                }
            }

            break;
        case 'unsetLink':
            props.editor.chain().unsetLink().focus()
            break
        case 'addImage':
            const imgUrl = window.prompt('Bild URL')
            if (imgUrl)
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
    <div class="menubar-container" :class="{ 'is-focused': focused }">

        <perfect-scrollbar :options="{
            scrollYMarginOffset: 30
        }">
            <div class="menubar is-hidden" :class="{ 'is-focused': focused }" v-if="props.editor">

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
                    <font-awesome-icon icon="fa-solid fa-window-minimize" />
                </button>

                <button class="menubar__button" @mousedown="command('undo', $event)">
                    <font-awesome-icon icon="fa-solid fa-rotate-left" />
                </button>

                <button class="menubar__button last-btn" @mousedown="command('redo', $event)">
                    <font-awesome-icon icon="fa-solid fa-rotate-right" />
                </button>
            </div>
        </perfect-scrollbar>

    </div>
</template>

<style lang="less">
.ps__rail-x {
    pointer-events: none;

    &.ps--clicking {
        .ps__thumb-x {
            height: 8px;
        }
    }
}

#AddQuestionBody {
    .menubar-container {
        max-width: calc(100vw - 62px)
    }
}
</style>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.menubar-container {
    top: 60px;
    position: sticky;
    z-index: 10;
    display: flex;
    max-width: min(100%, calc(100vw - 60px));
    height: 36px;
    margin-top: -36px;

    .ps {
        max-width: min(100%, calc(100vw - 20px));
        border-radius: 4px;
        box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
        // flex-shrink: 1;
        width: 100%;
        visibility: hidden;
    }

    &.is-focused {
        .ps {
            visibility: visible;
        }
    }
}



.menubar {
    font-size: 0;
    height: 36px;
    display: flex;
    flex-wrap: nowrap;
    background: white;
    width: 100%;

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

.menubar__button {
    background: white;
    border: hidden;
    font-size: 18px;
    width: 36px;
    height: 36px;
    margin: 0px;
    color: @memo-grey-darker;
    text-align: center;
    padding: 0px 21px;
    display: flex;
    justify-content: center;
    align-items: center;
    transition: filter 0.1s;

    &:hover {
        filter: brightness(0.85);
    }

    &.is-active {
        background: @memo-grey-light;
    }

    &:active {
        filter: brightness(0.7);
    }

    &.last-btn {
        border-top-right-radius: 4px;
        border-bottom-right-radius: 4px;
    }
}
</style>