import Link from '@tiptap/extension-link'
import { mergeAttributes } from '@tiptap/core'

export const CustomLink = Link.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      target: {
        default: '_self',
        parseHTML: element => {
          return '_self'
        },
      },
      renderHTML: ({HTMLAttributes}) => {
        return ['a', mergeAttributes(HTMLAttributes, { target: '_self' }), 0]
      },
      parseHTML: ({HTMLAttributes}) => {
        return [ 'a', mergeAttributes(HTMLAttributes, { target: '_self' }), 0 ]
      },
    }
  },
})