<script lang="ts" setup>
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import { GeneratedFlashCard, usePageStore } from '../../pageStore'
import { isEmpty } from 'underscore'
const pageStore = usePageStore()

const show = ref(false)



// const acceptFlashCard = async () => {
//     interface Result {
//         success: boolean
//         id?: number
//         messageKey?: string
//     }
//     const result = await $api<Result>(`/apiVue/AiCreateFlashCard/Create/`, {
//         method: 'POST',
//         body: {
//             pageId: pageStore.id,
//             front: questionHtml.value,
//             back: answerHtml.value,
//         },
//         mode: 'cors',
//         credentials: 'include',
//     })

//     if (result.success && result.id) {
//         console.log(result.id)
//     } else if (result.messageKey) {
//         console.log(result.messageKey)
//     }

//     show.value = false
// }

// const regenerateFlashCard = async () => {
//     const result = await pageStore.generateFlashCard()
//     if (result.front && result.back) {
//         setFlashCardData(result)
//     }
// }

const flashcards = ref<GeneratedFlashCard[]>([])

pageStore.$onAction(({ name, after }) => {
    if (name === 'generateFlashCard') {
        after((result) => {
            if (result) {
                show.value = true
                flashcards.value = result
            }
        })
    }
    if (name === 'reGenerateFlashCard') {
        after((result) => {
            if (result) {
                flashcards.value = result
            }
        })
    }
})

</script>


<template>
    <Modal :show="show" @close="show = false" @primary-btn="null" :show-cancel-btn="true" :primary-btn-label="'Karteikarte erstellen'" content-class="wide-modal">
        <!-- <template #header>
            <h3>Vorschau</h3>
        </template> -->
        <template #body>
            <div id="AiFlashCard">
                <PageLearningAiFlashCard v-for="flashcard in flashcards" :flash-card="flashcard" />

            </div>

            <!-- <button @click="regenerateFlashCard">Neu generieren</button> -->

        </template>
    </Modal>
</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AiFlashCard {}
</style>
<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#AiFlashCard {
    margin-top: 36px;

    .ProseMirror {
        text-align: center;
        border: 1px solid @memo-grey-light;
        padding: 24px;
        min-height: 240px;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.16);

        &.ProseMirror-focused {
            border: 1px @memo-grey-light solid;
        }

        ul {
            width: 100%;
        }
    }

}

.wide-modal {
    width: clamp(100%, 100%, 1400px);
}
</style>
