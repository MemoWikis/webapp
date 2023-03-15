<script lang="ts" setup>
import { useDeleteTopicStore } from './deleteTopicStore'

const deleteTopicStore = useDeleteTopicStore()

const errorMsg = ref('')

</script>

<template>
    <LazyModal :show="deleteTopicStore.showModal" :show-cancel-btn="true" primary-btn-label="Thema löschen"
        @main-btn="deleteTopicStore.deleteTopic()" @close="deleteTopicStore.showModal = false">
        <template v-slot:header>
            Thema '{{ deleteTopicStore.name }}' löschen
        </template>
        <template v-slot:body>
            <div class="body-m">Möchtest Du "<strong>{{ deleteTopicStore.name }}</strong>" unwiderruflich löschen?</div>
            <div class="body-s">Fragen werden nicht gelöscht.</div>
            <div class="alert alert-warning" role="alert" v-if="errorMsg.length > 0">
                {{ errorMsg }}
            </div>
        </template>
        <template v-slot:footer>

        </template>
    </LazyModal>
</template>