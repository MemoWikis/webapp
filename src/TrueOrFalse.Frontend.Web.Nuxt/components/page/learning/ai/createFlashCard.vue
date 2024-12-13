<script lang="ts" setup>
import { GeneratedFlashCard, usePageStore } from '../../pageStore'
const pageStore = usePageStore()

const show = ref(false)

const acceptFlashCards = async () => {
    interface Result {
        success: boolean
        ids?: number[]
        messageKey?: string
    }
    const result = await $api<Result>(`/apiVue/AiCreateFlashCard/Create/`, {
        method: 'POST',
        body: {
            pageId: pageStore.id,
            flashCards: flashcards.value,
        },
        mode: 'cors',
        credentials: 'include',
    })

    if (result.success && result.ids) {
        console.log(result.ids)
    } else if (result.messageKey) {
        console.log(result.messageKey)
    }

    show.value = false
}

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

const deleteFlashcard = (index: number) => {
    flashcards.value.splice(index, 1)
}

</script>


<template>
    <Modal :show="show" @close="show = false" @primary-btn="acceptFlashCards" :show-cancel-btn="true" :primary-btn-label="'Karteikarte erstellen'" content-class="wide-modal" :fullscreen="false" container-class="wide-modal"
        :show-close-button="true">
        <!-- <template #header>
            <h3>Vorschau</h3>
        </template> -->
        <template #body>
            <div id="AiFlashCard">
                <PageLearningAiFlashCard v-for="(flashcard, i) in flashcards" :flash-card="flashcard" :index="i" @delete-flashcard="deleteFlashcard" />
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
    padding: 0 24px;

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
</style>
