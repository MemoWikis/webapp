import Heading from '@tiptap/extension-heading'
import { slugify } from './utils'
import { useOutlineStore } from '~/components/sidebar/outlineStore'

export const CustomHeading = Heading.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      id: {
        default: null,
        parseHTML: element => {
            const id = element.getAttribute('id')
            if (id) 
              return id

            return slugify(element.innerText)
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