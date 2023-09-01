<script lang="ts" setup>
import { useDeleteTopicStore } from './deleteTopicStore'

const deleteTopicStore = useDeleteTopicStore()

const primaryBtnLabel = ref('Thema löschen')

watch(() => deleteTopicStore.topicDeleted, (val) => {
    if (val)
        primaryBtnLabel.value = 'Weiter'
    else primaryBtnLabel.value = 'Thema löschen'
})

async function handlePrimaryAction() {
    if (deleteTopicStore.topicDeleted) {
        deleteTopicStore.showModal = false
        deleteTopicStore.topicDeleted = false
        await navigateTo(deleteTopicStore.redirectURL)
    } else {
        deleteTopicStore.deleteTopic()
    }
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
                <div class="body-m">Möchtest Du '<strong>{{ deleteTopicStore.name }}</strong>' unwiderruflich löschen?</div>
                <div class="body-s">Fragen werden nicht gelöscht.</div>
            </template>

        </template>
        <template v-slot:footer>

        </template>
    </LazyModal>
</template>