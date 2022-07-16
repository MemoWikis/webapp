<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import { Topic, useTopicStore } from '~~/components/topic/topicStore'
const topic = useState<Topic>('topic')
const topicStore = useTopicStore()
const editor = useEditor({
    content: topic.value.Content,
    extensions: [
        StarterKit,
    ],
})
</script>

<template>
    <editor-content :editor="editor" />
</template>