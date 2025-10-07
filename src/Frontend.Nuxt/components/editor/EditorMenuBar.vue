<script lang="ts" setup>
import { Editor } from '@tiptap/vue-3'

interface Props {
    editor: Editor
    heading?: boolean
    isPageContent?: boolean
    allowImages?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    heading: true,
    isPageContent: false,
    allowImages: true
})
const focused = ref(false)

const emit = defineEmits(['handleUndoRedo'])

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
        case 'h4':
            props.editor.commands.toggleHeading({ level: 4 })
            break
        case 'bulletList':
            props.editor.commands.toggleBulletList()
            break
        case 'orderedList':
            props.editor.commands.toggleOrderedList()
            break
        case 'taskList':
            props.editor.commands.toggleTaskList()
            break
        case 'blockquote':
            props.editor.commands.toggleBlockquote()
            break
        case 'codeBlock':
            props.editor.commands.toggleCodeBlock()
            break
        case 'setLink':
            var previousUrl = props.editor.getAttributes('link').href
            var linkUrl = window.prompt('URL', previousUrl)

            if (linkUrl == null)
                return

            if (linkUrl === '') {
                props.editor
                    .chain()
                    .focus()
                    .extendMarkRange('link')
                    .unsetLink()
                    .run()
                return
            }

            props.editor.chain().focus().extendMarkRange('link').setLink({ href: linkUrl }).run()
            if (props.editor.view.state.selection.empty) {
                var transaction = props.editor.state.tr.insertText(linkUrl)
                props.editor.view.dispatch(transaction)
            }

            break
        case 'unsetLink':
            props.editor.chain().unsetLink().focus().run()
            break
        case 'addImage':
            props.editor.commands.addImage()
            break
        case 'horizontalRule':
            props.editor.commands.setHorizontalRule()
            break
        case 'undo':
            props.editor.commands.undo()
            emit('handleUndoRedo')
            break
        case 'redo':
            props.editor.commands.redo()
            emit('handleUndoRedo')
            break
        case 'outdent':
            if (props.editor.can().liftListItem('listItem'))
                props.editor.chain().focus().liftListItem('listItem').run()
            else
                props.editor.chain().focus().outdent().run()
            break
        case 'indent':
            if (props.editor.can().sinkListItem('listItem'))
                props.editor.chain().focus().sinkListItem('listItem').run()
            else
                props.editor.chain().focus().indent().run()
            break
    }
    await nextTick()
    props.editor.commands.focus()
}
const showScrollbar = ref(false)
const scrollbarShown = ref(false)

props.editor.on('focus', () => {
    focused.value = true

    if (isMobile && !scrollbarShown.value) {
        showScrollbar.value = true
        setTimeout(() => { showScrollbar.value = false }, 1500)
        scrollbarShown.value = true
    }
})
props.editor.on('blur', () => {
    focused.value = false
})

const { isMobile } = useDevice()
const slots = useSlots()
</script>
<template>
    <div class="menubar-container" :class="{ 'is-focused': focused, 'is-mobile': isMobile }">

        <PerfectScrollbar :options="{ scrollYMarginOffset: 30 }" :class="{ 'ps--scrolling-x': showScrollbar }">
            <div class="menubar is-hidden" :class="{ 'is-focused': focused }" v-if="props.editor">

                <slot name="start"></slot>

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

                <template v-if="heading">
                    <div class="menubar__divider__container">
                        <div class="menubar__divider"></div>
                    </div>

                    <button class="menubar__button"
                        :class="{ 'is-active': props.editor.isActive('heading', { level: 2 }) }"
                        @mousedown="command('h2', $event)">
                        <b>H1</b>
                    </button>

                    <button class="menubar__button"
                        :class="{ 'is-active': props.editor.isActive('heading', { level: 3 }) }"
                        @mousedown="command('h3', $event)">
                        <b>H2</b>
                    </button>

                    <button class="menubar__button"
                        :class="{ 'is-active': props.editor.isActive('heading', { level: 4 }) }"
                        @mousedown="command('h4', $event)">
                        <b>H3</b>
                    </button>
                </template>

                <div class="menubar__divider__container">
                    <div class="menubar__divider"></div>
                </div>

                <button class="menubar__button" @mousedown="command('outdent', $event)">
                    <font-awesome-icon :icon="['fas', 'outdent']" />
                </button>

                <button class="menubar__button" @mousedown="command('indent', $event)">
                    <font-awesome-icon :icon="['fas', 'indent']" />
                </button>

                <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('bulletList') }" @mousedown="command('bulletList', $event)">
                    <font-awesome-icon icon="fa-solid fa-list-ul" />
                </button>

                <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('orderedList') }" @mousedown="command('orderedList', $event)">
                    <font-awesome-icon icon="fa-solid fa-list-ol" />
                </button>

                <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('taskList') }" v-if="props.isPageContent" @mousedown="command('taskList', $event)">
                    <font-awesome-icon :icon="['fas', 'list-check']" />
                </button>

                <div class="menubar__divider__container">
                    <div class="menubar__divider"></div>
                </div>

                <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('blockquote') }" @mousedown="command('blockquote', $event)">
                    <font-awesome-icon icon="fa-solid fa-quote-right" />
                </button>

                <button class="menubar__button" :class="{ 'is-active': props.editor.isActive('codeBlock') }" @mousedown="command('codeBlock', $event)">
                    <font-awesome-icon icon="fa-solid fa-code" />
                </button>

                <button class="menubar__button" @mousedown="command('setLink', $event)" :class="{ 'is-active': props.editor.isActive('link') }">
                    <font-awesome-icon icon="fa-solid fa-link" />
                </button>

                <button v-if="props.editor.isActive('link')" class="menubar__button" @mousedown="command('unsetLink', $event)">
                    <font-awesome-icon icon="fa-solid fa-link-slash" />
                </button>

                <button class="menubar__button" @mousedown="command('addImage', $event)" v-if="props.allowImages">
                    <font-awesome-icon icon="fa-solid fa-image" />
                </button>

                <button class="menubar__button" @mousedown="command('horizontalRule', $event)">
                    <font-awesome-icon :icon="['far', 'window-minimize']" transform="top-4" />
                </button>

                <div class="menubar__divider__container">
                    <div class="menubar__divider"></div>
                </div>

                <button class="menubar__button" :class="{ 'disabled': !props.editor.can().undo() }" @mousedown="command('undo', $event)">
                    <font-awesome-icon icon="fa-solid fa-rotate-left" />
                </button>

                <button class="menubar__button" :class="{ 'last-btn': !slots.end, 'disabled': !props.editor.can().redo() }" @mousedown="command('redo', $event)">
                    <font-awesome-icon icon="fa-solid fa-rotate-right" />
                </button>

                <slot name="end"></slot>
            </div>
        </PerfectScrollbar>

    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.ps__rail-x {
    cursor: pointer;
    height: 0px;

    &.ps--clicking {
        .ps__thumb-x {
            height: 14px;
            opacity: 0.5;
        }
    }

    .ps__thumb-x {
        transition: opacity 0.2s ease-in;
        transition: height 0.1s ease-in;
        height: 8px;
        max-height: 14px !important;
        opacity: 0.9;
    }
}

#AddQuestionBody {
    .menubar-container {
        max-width: calc(100vw - 62px)
    }
}

.tiptap-image-container {
    display: flex;

    &.image-left {
        justify-content: flex-start;
    }

    &.image-center {
        justify-content: center;
    }

    &.image-right {
        justify-content: flex-end;
    }
}

.position-controller {
    font-size: 0;
    height: 36px;
    display: flex;
    flex-wrap: nowrap;
    background: white;
    width: 100%;
    box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
    position: absolute;
    top: 0%;
    left: 50%;
    width: 126px; // 3x 42px (button width)
    border-radius: 4px;
    overflow: hidden;
    cursor: pointer;
    transform: translate(-50%, -50%);

    .menubar_button {
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
}
</style>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.menubar-container {
    top: 60px;
    position: sticky;
    z-index: 10;
    display: flex;
    height: 36px;
    margin-top: -36px;

    &.is-mobile {
        max-width: 100vw;
        padding: 0;
        top: 45px;
        z-index: 100;
    }

    .ps {
        border-radius: 4px;
        box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
        // flex-shrink: 1;
        visibility: hidden;
        max-width: 100vw;
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

:slotted(.menubar__divider__container),
.menubar__divider__container {
    background: white;
    padding: 6px;

    .menubar__divider {
        height: 100%;
        width: 1px;
        background: @memo-grey-lighter;
        min-height: 12px;
    }
}

:slotted(.menubar__button),
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

    &.ai-create {
        padding-left: 24px;

        &:hover {
            filter: brightness(0.85);
        }
    }

    &.disabled {
        color: @memo-grey-light;
        pointer-events: none;
    }
}
</style>

<style lang="less">
.sidesheet-open {
    #PageContent {
        .menubar-container {
            max-width: calc(100vw - 420px);
        }
    }
}

#PageContent {
    @media (min-width: 900px) {
        .menubar-container {
            max-width: calc(100vw - 100px);
        }
    }
}
</style>