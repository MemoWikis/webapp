<script lang="ts" setup>
import { useConvertStore, ConversionTarget } from './convertStore'
const convertStore = useConvertStore()

</script>


<template>
    <LazyModal :show="convertStore.showModal" :primary-btn-label="'Bestätigen'" @primary-btn="convertStore.confirmConversion" @close="convertStore.closeModal" :show-cancel-btn="true">
        <template v-slot:header>
            <h4 class="modal-title">
                Konvertierung in {{ convertStore.conversionTarget === ConversionTarget.Wiki
                    ? 'Wiki'
                    : 'Seite' }} bestätigen
            </h4>
        </template>
        <template v-slot:body>
            <p>
                Möchtest Du wirklich
                {{ convertStore.conversionTarget === ConversionTarget.Wiki
                    ? 'die Seite "' + convertStore.name + '" in ein Wiki'
                    : 'das Wiki "' + convertStore.name + '" in eine Seite' }}
                umwandeln?
            </p>
            <div class="keep-parents-container" @click="convertStore.keepParents = !convertStore.keepParents" v-if="convertStore.conversionTarget === ConversionTarget.Wiki">
                <font-awesome-icon icon="fa-solid fa-square-check" class="keep-parents-checkbox active" v-if="convertStore.keepParents" />
                <font-awesome-icon icon="fa-regular fa-square" class="keep-parents-checkbox" v-else />

                <div class="">
                    Verknüpfungen zu Übergeordneten Wikis/Seiten beibehalten (optional)
                </div>
            </div>
            <div class="alert alert-warning" role="alert" v-if="convertStore.showErrorMsg">
                {{ convertStore.errorMsg }}
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.keep-parents-container {
    display: flex;
    align-items: center;
    margin-top: 24px;
    user-select: none;
    color: @memo-grey;
    cursor: pointer;
    font-size: 14px;

    .keep-parents-checkbox {
        font-size: 1.5em;
        margin-right: 10px;

        &.active {
            color: @memo-blue-link;
        }
    }
}
</style>
