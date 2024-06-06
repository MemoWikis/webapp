import { Node, mergeAttributes } from '@tiptap/core'
import { NodeViewWrapper, VueNodeViewRenderer } from '@tiptap/vue-3'
import { Plugin, PluginKey } from '@tiptap/pm/state'
import { EditorView } from 'prosemirror-view'
import { Slice } from 'prosemirror-model'

const blobToBase64 = async (blob: Blob) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.readAsDataURL(blob)
    reader.onload = () => resolve(reader.result)
    reader.onerror = error => reject(error)
  })
}

let isUploading = false

export async function uploadImage(formData: FormData) {
  isUploading = true
  const result = await $fetch<{success: boolean, url: string}>(`/apiVue/ImageUpload/SaveCustomImage`, {
    body: formData,
    mode: 'cors',
    credentials: 'include',
    headers: {
      'Content-Type': 'multipart/form-data',
      }
  })
  console.log(result)
  isUploading = false
  return result
}

export const Image = Node.create({
  name: 'image',
  addOptions() {
    return {
      inline: false,
      allowBase64: false,
      HTMLAttributes: {},
    }
  },
  group: 'block',
  draggable: true,
  addAttributes() {
    return {
      src: {
        default: null,
      },
      alt: {
        default: null,
      },
      title: {
        default: null,
      },
    }
  },
  parseHTML() {
    return [
      {
        tag: 'img[src]:not([src^="data:"])',
      },
    ]
  },
  renderHTML({ HTMLAttributes }) {
    return [
      'img',
      mergeAttributes(this.options.HTMLAttributes, HTMLAttributes),
    ]
  },
  addNodeView() {
    return VueNodeViewRenderer({
      components: {
        NodeViewWrapper
      },
      template: `
        <NodeViewWrapper class="w-full">
          <img :src="node.attrs.src || src" class="w-full" />
        </NodeViewWrapper>
      `,
      setup(props) {
        const { node } = props
        const src = ref('')

        watchEffect(async () => {
          if (node.attrs.src && node.attrs.src.startsWith('data:') && !isUploading) {
            const formData = new FormData()
            const base64 = node.attrs.src.split(',')[1]
            const file = window.atob(base64)

            formData.set(
              'file',
              new Blob([file], { type: 'image/png' }),
              'image.png'
            )

            // const uploadedFile = await uploadImage(formData)

            // if (uploadedFile) {
            //   src.value = uploadedFile.url
            // }
          }
        })

        return { src }
      },
    })
  },
  addProseMirrorPlugins() {
    return [
      new Plugin({
        key: new PluginKey('imageDrop'),
        props: {
          handleDrop: (view: EditorView, event: DragEvent, slice: Slice, moved: boolean): boolean => {
            if (event.dataTransfer?.files.length) {
              const file = event.dataTransfer.files[0]
              if (file.type.startsWith('image/')) {
                event.preventDefault()
                const reader = new FileReader()
                reader.onload = async () => {
                  if (file == null)
                      return

                  const formData = new FormData()
                  formData.append('file', file)       
                  await uploadImage(formData)
                }
                reader.readAsDataURL(file)
                return true
              }
            }
            return false
          },
          handlePaste: (view: EditorView, event: ClipboardEvent) => {
            
          }
        },
      }),
    ]
  },
})
