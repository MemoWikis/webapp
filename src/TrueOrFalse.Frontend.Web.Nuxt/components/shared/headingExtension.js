import Heading from '@tiptap/extension-heading'
import { slugify } from './utils'
import { nanoid } from 'nanoid'

export const CustomHeading = Heading.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      id: {
        default: null,
        parseHTML: element => {
            return slugify(element.innerText) + `-${nanoid(4)}`
        },
        renderHTML: attributes => {
          if (!attributes.id) {
            return {}
          }
          return {
            id: attributes.id,
          }
        },
      },
    }
  },
})