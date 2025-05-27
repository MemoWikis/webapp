//based on: https://github.com/coolswitch/tiptap-extension-image-upload

import { Plugin } from '@tiptap/pm/state'
import { Decoration, DecorationSet } from '@tiptap/pm/view'
import Image from './imageResizeExtension'
import { EditorView } from '@tiptap/pm/view'
import { Schema } from '@tiptap/pm/model'
import './upload-image.css'
import { resizeBase64Img } from '../../utils/utils'

export interface UploadFn {
    (file: File): Promise<string>
}
export interface CustomImageOptions {
    /**
     * Controls if the image node should be inline or not.
     * @default false
     * @example true
     */
    inline: boolean

    /**
     * Controls if base64 images are allowed. Enable this if you want to allow
     * base64 image urls in the `src` attribute.
     * @default false
     * @example true
     */
    allowBase64: boolean

    /**
     * HTML attributes to add to the image element.
     * @default {}
     * @example { class: 'foo' }
     */
    HTMLAttributes: Record<string, any>

    /**
     * Function to upload image
     */
    uploadFn: UploadFn
}

declare module '@tiptap/core' {
    interface Commands<ReturnType> {
        customImage: {
            /**
             * Add an image
             * @example
             * editor
             *   .commands
             *   .addImage()
             */
            addImage: () => ReturnType

            /**
             * Add an image
             * @param options The image attributes
             * @example
             * editor
             *   .commands
             *   .setImage({ src: 'https://tiptap.dev/logo.png', alt: 'tiptap', title: 'tiptap logo' })
             */
            setImage: (options: {
                src: string
                alt?: string
                title?: string
            }) => ReturnType
            addBase64Image: (base64String: string) => ReturnType
        }
    }
}

const UploadImage = Image.extend<CustomImageOptions>({
    name: 'uploadImage',

    onCreate() {
        if (typeof this.options.uploadFn !== 'function') {
            console.warn('uploadFn should be a function')
            return
        }
    },

    addOptions() {
        return {
            ...this.parent?.(),
            uploadFn: async () => {
                return ''
            },
        }
    },

    addProseMirrorPlugins() {
        return [placeholderPlugin]
    },

    addCommands() {
        return {
            ...this.parent?.(),
            addImage: () => () => {
                const fileHolder = document.createElement('input')
                fileHolder.setAttribute('type', 'file')
                fileHolder.setAttribute('accept', 'image/*')
                fileHolder.setAttribute('style', 'visibility:hidden')
                document.body.appendChild(fileHolder)

                const view = this.editor.view
                const schema = this.editor.schema
                const uploadFn = this.options.uploadFn // Use uploadFn from this instance

                fileHolder.addEventListener('change', (e: Event) => {
                    if (
                        view.state.selection.$from.parent.inlineContent &&
                        (<HTMLInputElement>e.target)?.files?.length
                    ) {
                        startImageUpload(
                            view,
                            (<HTMLInputElement>e.target)?.files![0],
                            schema,
                            uploadFn
                        )
                    }
                    view.focus()
                })
                fileHolder.click()
                return true
            },
            addBase64Image: (base64String: string) => () => {
                resizeAndUploadBase64Image(
                    base64String,
                    this.editor.view,
                    this.editor.schema,
                    this.options.uploadFn
                )
                return true
            },
        }
    },
})

const resizeAndUploadBase64Image = async (
    base64String: string,
    view: EditorView,
    schema: Schema,
    uploadFn: UploadFn
) => {
    const resizedBase64String = await resizeBase64Img(base64String, 800)
    // Convert base64 string to Blob
    const byteString = atob(resizedBase64String.split(',')[1])
    const mimeString = resizedBase64String
        .split(',')[0]
        .split(':')[1]
        .split('')[0]
    const ab = new ArrayBuffer(byteString.length)
    const ia = new Uint8Array(ab)
    for (let i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i)
    }
    const blob = new Blob([ab], { type: mimeString })

    // Create a File object from the Blob
    const file = new File([blob], 'image.jpg', { type: mimeString })

    // Use the existing startImageUpload function
    startImageUpload(view, file, schema, uploadFn)
    view.focus()
    return true
}

//Plugin for placeholder
const placeholderPlugin = new Plugin({
    state: {
        init() {
            return DecorationSet.empty
        },
        apply(tr, set) {
            // Adjust decoration positions to changes made by the transaction
            set = set.map(tr.mapping, tr.doc)
            // See if the transaction adds or removes any placeholders
            const action = tr.getMeta(this as any)
            if (action && action.add) {
                const widget = document.createElement('div')
                const img = document.createElement('img')
                widget.classList.value = 'image-uploading'
                img.src = ''
                widget.appendChild(img)
                const deco = Decoration.widget(action.add.pos, widget, {
                    id: action.add.id,
                })
                set = set.add(tr.doc, [deco])
            } else if (action && action.remove) {
                set = set.remove(
                    set.find(
                        undefined,
                        undefined,
                        (spec) => spec.id == action.remove.id
                    )
                )
            }
            return set
        },
    },
    props: {
        decorations(state) {
            return this.getState(state)
        },
    },
})

//Find the placeholder in editor
function findPlaceholder(state: any, id: any) {
    const decos = placeholderPlugin.getState(state)
    const found = decos?.find(undefined, undefined, (spec) => spec.id == id)

    return found?.length ? found[0].from : null
}

function startImageUpload(
    view: EditorView,
    file: File,
    schema: Schema,
    uploadFn: UploadFn
) {
    const imagePreview = URL.createObjectURL(file)
    const id = {}
    const tr = view.state.tr
    if (!tr.selection.empty) tr.deleteSelection()
    tr.setMeta(placeholderPlugin, { add: { id, pos: tr.selection.from } })
    view.dispatch(tr)

    uploadFn(file).then(
        (url) => {
            const pos = findPlaceholder(view.state, id)
            if (pos == null) return
            view.dispatch(
                view.state.tr
                    .replaceWith(
                        pos,
                        pos,
                        schema.nodes.uploadImage.create({ src: url })
                    )
                    .setMeta(placeholderPlugin, { remove: { id } })
            )
        },
        (e) => {
            view.dispatch(tr.setMeta(placeholderPlugin, { remove: { id } }))
        }
    )
}

export { UploadImage }

export default UploadImage
