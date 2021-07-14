
import {Editor, EditorContent} from '@tiptap/vue-2'
import StarterKit from '@tiptap/starter-kit'

import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'

import lowlight from 'lowlight'

window.tiptapEditor = Editor;
window.tiptapEditorContent = EditorContent;
window.tiptapStarterKit = StarterKit;

window.tiptapLink = Link;
window.tiptapPlaceholder = Placeholder;
window.tiptapCodeBlockLowlight = CodeBlockLowlight;
window.tiptapUnderline = Underline;
window.tiptapImage = Image;

window.lowlight = lowlight;