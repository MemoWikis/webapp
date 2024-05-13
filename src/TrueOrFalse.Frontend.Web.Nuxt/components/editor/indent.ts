/* eslint-disable no-param-reassign */
// Sources:
// https://github.com/ueberdosis/tiptap/issues/1036#issuecomment-981094752
// https://github.com/django-tiptap/django_tiptap/blob/main/django_tiptap/templates/forms/tiptap_textarea.html#L453-L602

import {
  CommandProps,
  Extension,
  Extensions,
  isList,
  KeyboardShortcutCommand,
} from '@tiptap/core'
import { TextSelection, Transaction } from 'prosemirror-state'

declare module '@tiptap/core' {
  interface Commands<ReturnType> {
    indent: {
      indent: () => ReturnType
      outdent: () => ReturnType
    }
  }
}

type IndentOptions = {
  names: Array<string>
  indentRange: number
  minIndentLevel: number
  maxIndentLevel: number
  defaultIndentLevel: number
  HTMLAttributes: Record<string, any>
}
export const Indent = Extension.create<IndentOptions, never>({
  name: 'indent',

  addOptions() {
    return {
      names: ['heading', 'paragraph'],
      indentRange: 24,
      minIndentLevel: 0,
      maxIndentLevel: 24 * 10,
      defaultIndentLevel: 0,
      HTMLAttributes: {},
    }
  },

  addGlobalAttributes() {
    return [
      {
        types: this.options.names,
        attributes: {
          indent: {
            default: this.options.defaultIndentLevel,
            renderHTML: (attributes) => ({
              style: `margin-left: ${attributes.indent}px!important;`,
            }),
            parseHTML: (element) =>
              parseInt(element.style.marginLeft, 10) ||
              this.options.defaultIndentLevel,
          },
        },
      },
    ]
  },

  addCommands(this) {
    return {
      indent: () => ({ tr, state, dispatch, editor }: CommandProps) => {
        const { selection } = state
        tr = tr.setSelection(selection)
        tr = updateIndentLevel(
          tr,
          this.options,
          editor.extensionManager.extensions,
          'indent'
        )
        if (tr.docChanged && dispatch) {
          dispatch(tr)
          return true
        }
        return false
      },
      outdent: () => ({ tr, state, dispatch, editor }: CommandProps) => {
        const { selection } = state
        tr = tr.setSelection(selection)
        tr = updateIndentLevel(
          tr,
          this.options,
          editor.extensionManager.extensions,
          'outdent'
        )
        if (tr.docChanged && dispatch) {
          dispatch(tr)
          return true
        }
        return false
      },
    }
  },

  addKeyboardShortcuts() {
    return {
      Tab: getIndent(),
      'Shift-Tab': getOutdent(false),
      Backspace: getOutdent(true),
      'Mod-]': getIndent(),
      'Mod-[': getOutdent(false),
    }
  },
  onUpdate() {
    const { editor } = this
    // インデントされたparagraphがlistItemに変更されたらindentをリセット
    if (editor.isActive('listItem')) {
      const node = editor.state.selection.$head.node()
      if (node.attrs.indent) {
        editor.commands.updateAttributes(node.type.name, { indent: 0 })
      }
    }
  },
})

export const clamp = (val: number, min: number, max: number): number => {
  if (val < min) {
    return min
  }
  if (val > max) {
    return max
  }
  return val
}

function setNodeIndentMarkup(
  tr: Transaction,
  pos: number,
  delta: number,
  min: number,
  max: number
): Transaction {
  if (!tr.doc) return tr
  const node = tr.doc.nodeAt(pos)
  if (!node) return tr
  const indent = clamp((node.attrs.indent || 0) + delta, min, max)
  if (indent === node.attrs.indent) return tr
  const nodeAttrs = {
    ...node.attrs,
    indent,
  }
  return tr.setNodeMarkup(pos, node.type, nodeAttrs, node.marks)
}

type IndentType = 'indent' | 'outdent'
const updateIndentLevel = (
  tr: Transaction,
  options: IndentOptions,
  extensions: Extensions,
  type: IndentType
): Transaction => {
  const { doc, selection } = tr
  if (!doc || !selection) return tr
  if (!(selection instanceof TextSelection)) {
    return tr
  }
  const { from, to } = selection
  doc.nodesBetween(from, to, (node, pos) => {
    if (options.names.includes(node.type.name)) {
      tr = setNodeIndentMarkup(
        tr,
        pos,
        options.indentRange * (type === 'indent' ? 1 : -1),
        options.minIndentLevel,
        options.maxIndentLevel
      )
      return false
    }
    return !isList(node.type.name, extensions)
  })
  return tr
}

export const getIndent: () => KeyboardShortcutCommand = () => ({ editor }) => {
  if (editor.can().sinkListItem('listItem')) {
    return editor.chain().focus().sinkListItem('listItem').run()
  }
  return editor.chain().focus().indent().run()
}
export const getOutdent: (
  outdentOnlyAtHead: boolean
) => KeyboardShortcutCommand = (outdentOnlyAtHead) => ({ editor }) => {
  if (outdentOnlyAtHead && editor.state.selection.$head.parentOffset > 0) {
    return false
  }
  if (
    /**
     * editor.state.selection.$head.parentOffset > 0があるのは
     * ```
     * - Hello
     * |<<ここにカーソル
     * ```
     * この状態でBackSpaceを繰り返すとlistItemのtoggleが繰り返されるのを防ぐため
     */
    (!outdentOnlyAtHead || editor.state.selection.$head.parentOffset > 0) &&
    editor.can().liftListItem('listItem')
  ) {
    return editor.chain().focus().liftListItem('listItem').run()
  }
  return editor.chain().focus().outdent().run()
}