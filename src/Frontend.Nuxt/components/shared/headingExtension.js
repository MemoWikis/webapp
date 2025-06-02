import Heading from '@tiptap/extension-heading'
import { slugify } from '~/utils/utils'
import { nanoid } from 'nanoid'

export const CustomHeading = Heading.extend({
    addAttributes() {
        return {
            ...this.parent?.(),
            id: {
                default: null,
                parseHTML: (element) => {
                    const id = element.getAttribute('id')

                    if (id) {
                        if (document.getElementById(id))
                            return slugify(element.innerText) + `-${nanoid(5)}`
                        else return id
                    }

                    return slugify(element.innerText) + `-${nanoid(5)}`
                },
                renderHTML: (attributes) => {
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
