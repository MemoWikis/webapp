<script lang="ts" setup>
import { useUserStore } from './userStore'
import { useEditor, EditorContent, JSONContent, BubbleMenu } from '@tiptap/vue-3'
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
const readonly = computed(() => props.userId != null && props.userId !== userStore.id)
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
    editable: isCurrentUser.value, // Only editable for current user
    onUpdate: ({ editor }) => {
        const html = editor.getHTML()
        aboutMeHtml.value = html
        // Save changes with debounce
        if (isCurrentUser.value) {
            debouncedSave(html)
        }
    },
    onTransaction: ({ editor }) => {
        collapsed.value = false
        aboutMeHtml.value = editor.getHTML()
    },
})

</script>

<template>
    <div class="about-me-section">
        <!-- <div v-html="aboutMeModel"></div> -->

        <bubble-menu :editor="editor" v-if="editor && isCurrentUser">
            <div class="bubble-menu">
                <button @click="editor.chain().focus().toggleBold().run()" :class="{ 'is-active': editor.isActive('bold') }">
                    <font-awesome-icon icon="fa-solid fa-bold" />
                </button>
                <button
                    @click="editor.chain().focus().toggleItalic().run()"
                    :class="{ 'is-active': editor.isActive('italic') }">
                    <font-awesome-icon icon="fa-solid fa-italic" />
                </button>
            </div>
        </bubble-menu>
        <editor-content v-if="editor" :editor="editor" class="about-me-text" :class="{ 'show-full': !collapsed }" />

        <!-- Gradient overlay when collapsed -->
        <div v-if="collapsed" class="gradient-overlay"></div>

        <div class="collapse-button" @click="collapsed = !collapsed">
            {{ collapsed ? t('userAboutMe.showMore') : t('userAboutMe.showLess') }}
        </div>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.about-me-section {
    font-size: 18px;
    margin-bottom: 10px;
    color: @memo-grey;
    max-height: unset;
    min-height: 100px;
    width: 100%;

    p {
        max-width: 500px;
        line-height: 1.4;
        margin: 0;
    }

    .star {
        color: @memo-yellow;
    }

    .info-icon {
        color: @memo-grey-light;
        margin-right: 4px;
    }

    .about-me-text {
        max-height: 100px;
        overflow: hidden;
        max-width: 600px;

        &.show-full {
            max-height: none;
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

    .collapse-button {
        cursor: pointer;
        color: @memo-blue;
        text-decoration: underline;
        display: flex;
        justify-content: center;

        &:hover {
            text-decoration: none;
        }
    }

}
</style>