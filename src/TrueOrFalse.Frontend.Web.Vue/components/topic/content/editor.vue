<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { lowlight } from 'lowlight/lib/core'

import { Topic, useTopicStore } from '~~/components/topic/topicStore'
const topic = useState<Topic>('topic')
const topicStore = useTopicStore()
const editor = useEditor({
    content: topicStore.content,
    extensions: [
        StarterKit.configure({
            heading: {
                levels: [2, 3],
                HTMLAttributes: {
                    class: 'inline-text-heading'
                }
            }
        }),
        Link.configure({
            HTMLAttributes: {
                rel: 'noopener noreferrer nofollow'
            },
            openOnClick: true,
        }),
        Placeholder.configure({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            placeholder: 'Klicke hier um zu tippen ...',
            showOnlyWhenEditable: true,
            showOnlyCurrent: true,
        }),
        Underline,
        Image.configure({
            inline: true,
            allowBase64: true,
        }),
        CodeBlockLowlight.configure({
            lowlight,
        }),
    ],
    onUpdate({ editor }) {
        topicStore.contentHasChanged = true
        topicStore.content = editor.getHTML()
    },

})

topicStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'reset')
            editor.value?.commands.setContent(topicStore.content)
    })
})
</script>

<template>
    <LazyEditorMenuBar :editor="editor" :heading="true" />
    <editor-content :editor="editor" />
</template>