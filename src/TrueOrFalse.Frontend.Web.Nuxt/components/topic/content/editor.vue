<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import Blockquote from '@tiptap/extension-blockquote'
import { lowlight } from 'lowlight/lib/core'
import { Topic, useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useAlertStore, AlertType } from '~~/components/alert/alertStore'
import _ from 'underscore'
import { messages } from '~~/components/alert/alertStore'

const alertStore = useAlertStore()

const topic = useState<Topic>('topic')
const topicStore = useTopicStore()
const editor = useEditor({
    content: topicStore.initialContent,
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
        Blockquote
    ],
    onUpdate({ editor }) {
        topicStore.contentHasChanged = true
        topicStore.content = editor.getHTML()
    },
    editorProps: {
        // handleKeyDown: (e, k) => {
        //     this.contentIsChanged = true;
        // },
        handleClick: (view, pos, event) => {
            // var _a;
            // var attrs = this.editor.getAttributes('link');
            // var href = Site.IsMobile ? event.target.href : attrs.href;
            // var link = (_a = event.target) === null || _a === void 0 ? void 0 : _a.closest('a');
            // if (link && href) {
            //     window.open(href, event.ctrlKey ? '_blank' : '_self');
            //     return true;
            // }
            // return false;
        },
        handlePaste: (view, pos, event) => {
            const firstNode = event.content.firstChild
            if (firstNode != null && firstNode.type.name == 'image') {
                if (!_.isEmpty(firstNode.attrs)) {
                    let src = firstNode.attrs.src;
                    if (src.length > 1048576 && src.startsWith('data:image')) {
                        alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                        return true
                    }
                }
            }

        },
        attributes: {
            id: 'InlineEdit',
        }
    },
})

topicStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'resetContent')
            editor.value?.commands.setContent(topicStore.content)
    })
})

const spinnerStore = useSpinnerStore()
onMounted(() => {
    spinnerStore.hideSpinner()
})
</script>

<template>
    <template v-if="editor">
        <EditorMenuBar :editor="editor" :heading="true" />
        <editor-content :editor="editor" class="col-xs-12" />
    </template>
    <div class="col-xs-12" v-else-if="topic != null">
        <div style="height:36px"></div>
        <div class="ProseMirror content-placeholder" v-html="topic.Content">

        </div>
    </div>
</template>

<style lang="less" scoped>
.content-placeholder {
    :deep(p:empty) {
        min-height: 20px
    }
}
</style>