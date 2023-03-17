<script lang="ts" setup>
import { useDeleteTopicStore } from './deleteTopicStore'

const deleteTopicStore = useDeleteTopicStore()

const primaryBtnLabel = ref('Thema löschen')

watch(() => deleteTopicStore.topicDeleted, (val) => {
    if (val)
        primaryBtnLabel.value = 'Weiter'
    else primaryBtnLabel.value = 'Thema löschen'
})

function handlePrimaryAction() {
    if (deleteTopicStore.topicDeleted) {
        deleteTopicStore.showModal = false
        deleteTopicStore.topicDeleted = false
        navigateTo(deleteTopicStore.redirectURL)
    } else {
        deleteTopicStore.deleteTopic()
    }
}

</script>

<template>
    <LazyModal :show="deleteTopicStore.showModal" :show-cancel-btn="!deleteTopicStore.topicDeleted"
        :primary-btn-label="primaryBtnLabel" @main-btn="handlePrimaryAction()" @close="deleteTopicStore.showModal = false">
        <template v-slot:header>
            <template v-if="deleteTopicStore.topicDeleted">
                '{{ deleteTopicStore.name }}' wurde gelöscht
            </template>
            <template v-else>
                Thema '{{ deleteTopicStore.name }}' löschen
            </template>
        </template>
        <template v-slot:body>
            <template v-if="deleteTopicStore.topicDeleted">
                Beim schliessen dieses Fensters wirst Du zum nächsten übergeordnetem Thema weitergeleitet
            </template>
            <template v-else>
                <div class="body-m">Möchtest Du "<strong>{{ deleteTopicStore.name }}</strong>" unwiderruflich löschen?</div>
                <div class="body-s">Fragen werden nicht gelöscht.</div>
            </template>

        </template>
        <template v-slot:footer>

        </template>
    </LazyModal>
</template>