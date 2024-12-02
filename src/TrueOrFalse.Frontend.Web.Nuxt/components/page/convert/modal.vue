<script lang="ts" setup>
import { useConvertStore, ConversionTarget } from './convertStore'
const convertStore = useConvertStore()

</script>


<template>
    <LazyModal :show="convertStore.showModal" :primary-btn-label="'Bestätigen'" @primary-btn="convertStore.confirmConversion" @close="convertStore.closeModal" :show-cancel-btn="true">
        <template v-slot:header>
            <h4 class="modal-title">Konvertierung bestätigen</h4>
        </template>
        <template v-slot:body>
            <p>
                Möchten Sie wirklich
                {{ convertStore.conversionTarget === ConversionTarget.Wiki
                    ? 'die Seite "' + convertStore.name + '" in ein Wiki'
                    : 'das Wiki "' + convertStore.name + '" in eine Seite' }}
                umwandeln?
            </p>
            <div class="alert alert-warning" role="alert" v-if="convertStore.showErrorMsg">
                {{ convertStore.errorMsg }}
            </div>
        </template>
    </LazyModal>
</template>
