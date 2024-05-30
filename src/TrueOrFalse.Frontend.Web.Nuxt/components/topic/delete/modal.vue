<script lang="ts" setup>
import { Parent } from "~~/components/topic/delete/parent"
import { useDeleteTopicStore } from './deleteTopicStore'
import { QuestionItem, SearchType, TopicItem, UserItem } from '~~/components/search/searchHelper'

const deleteTopicStore = useDeleteTopicStore()
const showSearch = ref(true)

const parent = ref<Parent | null>(null)
const primaryBtnLabel = ref('Thema löschen')
const selectedParent = ref(0);
watch(() => deleteTopicStore.topicDeleted, (val) => {
    if (val)
        primaryBtnLabel.value = 'Weiter'
    else primaryBtnLabel.value = 'Thema löschen'
})

watch(() => deleteTopicStore.parent, (val) => {
    parent.value = val;
})

console.log("Delete-topic-store-in-delet-modal", deleteTopicStore);
async function handlePrimaryAction() {
    if (deleteTopicStore.topicDeleted) {
        deleteTopicStore.showModal = false
        deleteTopicStore.topicDeleted = false

        if (deleteTopicStore.redirect)
            await navigateTo(deleteTopicStore.redirectURL)
    } else {
        deleteTopicStore.deleteTopic(selectedParent.value)
    }
}

function setCategoryId(item: TopicItem) {
    var parentTemp: Parent = {
        id: item.id,
        name: item.name
    }
    parent.value = parentTemp
}
</script>
<template>
    <LazyModal :show="deleteTopicStore.showModal" :show-cancel-btn="!deleteTopicStore.topicDeleted"
        v-if="deleteTopicStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handlePrimaryAction()"
        @close="deleteTopicStore.showModal = false">
        <template v-slot:header>
            <template v-if="deleteTopicStore.topicDeleted">
                Thema gelöscht
            </template>
            <template v-else>
                Thema '{{ deleteTopicStore.name }}' löschen
            </template>
        </template>
        <template v-slot:body>
            <template v-if="deleteTopicStore.topicDeleted && deleteTopicStore.redirect">
                Beim schliessen dieses Fensters wirst Du zum nächsten übergeordnetem Thema weitergeleitet.
            </template>
            <template v-else-if="deleteTopicStore.topicDeleted && !deleteTopicStore.redirect">
                Das Thema '<strong>{{ deleteTopicStore.name }}</strong>' wurde erfolgreich gelöscht.
            </template>
            <template v-else>
                <div class="body-m">Möchtest Du '<strong>{{ deleteTopicStore.name }}</strong>' unwiderruflich löschen?
                </div>
                <div class="body-m">
                    Du kannst das vorausgewählte Thema nehmen oder dir ein Thema aussuchen wohin deine Fragen verschoben
                    werden sollen
                </div>
                <div class="body-s">Fragen werden nicht gelöscht.</div>

                <div>
                    <label class="flex">
                        <input type="radio" :value="parent?.id" v-model="selectedParent">
                        <div class="radio-button-description"> {{ parent?.name }}</div>
                    </label>
                </div>
                <div class="StickySearch">
                    <Search :placeholderLabel="'Suche hier...'" :showDefaultSearchIcon="true"
                        :search-type="SearchType.category" :show-search="showSearch" v-on:select-item="setCategoryId" />
                </div>
            </template>
        </template>
        <template v-slot:footer>
        </template>
    </LazyModal>
</template>
<style scoped lang="less">
.flex {
    display: flex;
    align-items: center;
}

.radio-button-description {
    margin-left: 4px;
}
</style>