// import Image from '@tiptap/extension-image'
// import { VueNodeViewRenderer, mergeAttributes } from '@tiptap/vue-3'

// async function uploadImageHandler() {

// }

// const isUploading = ref(false);

// // Props and other reactive sources
// const props = defineProps({
//     updateAttributes: Function
// })

// const src = ref('') // This should be reactive and changed from parent or internally

// watchEffect(async () => {
//     // Only upload the image when the src is a base64 string
//     if (uploadImageHandler && src.value.startsWith('data') && !isUploading.value) {
//         isUploading.value = true
//         const imgUrl = await uploadImageHandler()
//         props.updateAttributes({ src: imgUrl })
//         isUploading.value = false
//     }
// })

// async function getImageUrl() {
//     const result = await $fetch<{success: boolean, url: string}>(`/apiVue/UploadCustomImage or something like that/`, {
//         body: formData,
//         mode: 'cors',
//         credentials: 'include',
//         headers: {
//             'Content-Type': 'multipart/form-data',
//         }
//     })
// }

// const ImageUpload = Image.extend({
//     addAttributes() {
//         return {
//             ...this.parent?.(),
//             uploadImageHandler: { default: undefined },
//             async renderHTML({ HTMLAttributes }: any) {
//                 return [
//                     [
//                         'img',
//                         //This line removed uploadImageHandler from attributes
//                         mergeAttributes(HTMLAttributes, { src: getImageUrl }),
//                     ],
//                 ]   
//             }
//         }
//     }
// })