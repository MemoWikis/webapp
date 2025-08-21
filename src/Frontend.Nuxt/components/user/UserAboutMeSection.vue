<script lang="ts" setup>
import { useUserStore } from './userStore'
import { useEditor, EditorContent, BubbleMenu } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'

const props = defineProps<{
    userId?: number,
    aboutMe?: string
}>()

const userStore = useUserStore()
const { t } = useI18n()
const { updateAboutMe } = useUserProfile()

const aboutMeModel = ref<string>('')
const isCurrentUser = computed(() => props.userId === userStore.id)

const aboutMeHtml = ref<string>('')

onBeforeMount(() => {
    aboutMeModel.value = props.aboutMe || `<p>${t('userAboutMe.defaultText')}</p>`
})

const collapsed = ref(true)

// Debounced save function
let saveTimeoutId: NodeJS.Timeout | null = null
const debouncedSave = async (html: string) => {
    if (!isCurrentUser.value || !props.userId) return

    if (saveTimeoutId) {
        clearTimeout(saveTimeoutId)
    }

    saveTimeoutId = setTimeout(async () => {
        try {
            const result = await updateAboutMe(props.userId!, html)
            if (!result.success) {
                console.error('Failed to save about me text:', result.errorMessageKey)
            }
        } catch (error) {
            console.error('Error saving about me text:', error)
        }
    }, 1000) // 1 second debounce
}

const editor = useEditor({
    content: props.aboutMe ?? null,
    extensions: [
        StarterKit.configure({
            codeBlock: false,
        }),
        Link.configure({
            HTMLAttributes: {
                target: '_self',
                rel: 'noopener noreferrer nofollow'
            }
        }),
        Underline,
        Placeholder.configure({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            placeholder: t('userAboutMe.placeholder'),
            showOnlyCurrent: true,
        }),
    ],
    editorProps: {
        attributes: {
            id: 'AboutMe',
        },
    },
    editable: isCurrentUser.value,
    onUpdate: ({ editor }) => {
        const html = editor.getHTML()
        aboutMeHtml.value = html
        // Save changes with debounce (only for current user edits)
        if (isCurrentUser.value) {
            debouncedSave(html)
        }
    },
    onTransaction: ({ editor }) => {
        const html = editor.getHTML()
        aboutMeHtml.value = html
        // Only collapse on user transactions (when editor is focused/active)
        if (editor.isFocused && isCurrentUser.value) {
            collapsed.value = false
        }
    },
})

const editorRef = ref()

// Check if editor content is tall enough to need collapse functionality
const needsCollapse = ref(false)

const checkEditorHeight = () => {
    if (editorRef.value && editorRef.value.$el) {
        const editorElement = editorRef.value.$el
        needsCollapse.value = editorElement.scrollHeight > 116
    }
}

// Watch for content changes to update collapse state
watch(() => aboutMeHtml.value, () => {
    nextTick(() => {
        checkEditorHeight()
    })
})

onMounted(() => {
    nextTick(() => {
        checkEditorHeight()
    })
})

watch(() => userStore.showAsVisitor, (show) => {
    editor.value?.setEditable(show && isCurrentUser.value)
    if (!show) {
        collapsed.value = true
    }
})

</script>

<template>
    <div class="about-me-section">
        <!-- <div v-html="aboutMeModel"></div> -->

        <template v-if="editor">
            <bubble-menu :editor="editor" v-if="editor && isCurrentUser">
                <div class="bubble-menu">
                    <button class="menubar__button" @click="editor.chain().focus().toggleBold().run()" :class="{ 'is-active': editor.isActive('bold') }">
                        <font-awesome-icon icon="fa-solid fa-bold" />
                    </button>
                    <button class="menubar__button" @click="editor.chain().focus().toggleItalic().run()" :class="{ 'is-active': editor.isActive('italic') }">
                        <font-awesome-icon icon="fa-solid fa-italic" />
                    </button>
                </div>
            </bubble-menu>
            <editor-content v-if="editor" :editor="editor" class="about-me-text" :class="{ 'show-full': !collapsed }" ref="editorRef" />
        </template>

        <div v-else v-html="aboutMe" class="about-me-text placeholder" />


        <template v-if="collapsed && needsCollapse || editor == null">
            <!-- Gradient overlay when collapsed -->
            <div class="gradient-overlay"></div>

            <div class="collapse-button-section" v-if="editor">
                <div class="collapse-button" @click="collapsed = false" :class="{ 'collapsed': collapsed }">
                    {{ t('userAboutMe.readMore') }}
                </div>
            </div>
        </template>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.about-me-section {
    font-size: 18px;
    margin-bottom: 10px;
    color: @memo-grey;
    max-height: unset;
    min-height: 60px;
    width: 100%;
    max-width: 600px;

    :deep(p) {
        margin-bottom: 0.5rem;
    }

    .star {
        color: @memo-yellow;
    }

    .info-icon {
        color: @memo-grey-light;
        margin-right: 4px;
    }

    .about-me-text {
        max-height: 110px;
        overflow: hidden;
        max-width: 600px;

        &.show-full {
            max-height: none;
        }

        &.placeholder {
            max-height: 115px;
        }
    }

    .gradient-overlay {
        position: relative;
        height: 40px;
        margin-top: -40px;
        background: linear-gradient(to bottom, transparent 0%, white 100%);
        pointer-events: none;
        z-index: 1;
    }

    .collapse-button-section {
        display: flex;
        justify-content: center;
        position: relative;
        margin-top: 0;

        .collapse-button {
            cursor: pointer;
            color: @memo-blue;
            padding: 8px 16px;
            z-index: 2;
            font-size: 1.4rem;
            color: @memo-grey;
            background: white;
            border-radius: 24px;

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            &.collapsed {
                margin-top: -30px;
            }
        }
    }

}

.bubble-menu {
    font-size: 0;
    height: 36px;
    display: flex;
    flex-wrap: nowrap;
    background: white;
    box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
    border-radius: 4px;
    overflow: hidden;

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

        &:last-child {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        &:first-child {
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }
    }
}
</style>