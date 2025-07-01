<script lang="ts" setup>
import { useConvertStore, ConversionTarget } from './convertStore'
const convertStore = useConvertStore()
const { t } = useI18n()
</script>

<template>
    <LazyModal
        :show="convertStore.showModal"
        :primary-btn-label="t('page.convert.modal.confirm')"
        @primary-btn="convertStore.confirmConversion"
        @close="convertStore.closeModal"
        :show-cancel-btn="!convertStore.showErrorMsg">
        <template v-slot:header>
            <h4 class="modal-title">
                {{ t(convertStore.conversionTarget === ConversionTarget.Wiki
                    ? 'page.convert.modal.title.toWiki'
                    : 'page.convert.modal.title.toPage') }}
            </h4>
        </template>
        <template v-slot:body>
            <div class="alert alert-warning" role="alert" v-if="convertStore.showErrorMsg">
                {{ t(convertStore.errorMsg) }}
            </div>
            <template v-else>
                <p>
                    {{ t(convertStore.conversionTarget === ConversionTarget.Wiki
                        ? 'page.convert.modal.body.toWiki'
                        : 'page.convert.modal.body.toPage',
                        { name: convertStore.name }) }}
                </p>
                <div class="keep-parents-container"
                    @click="convertStore.keepParents = !convertStore.keepParents"
                    v-if="convertStore.conversionTarget === ConversionTarget.Wiki">
                    <font-awesome-icon icon="fa-solid fa-square-check"
                        class="keep-parents-checkbox active"
                        v-if="convertStore.keepParents" />
                    <font-awesome-icon icon="fa-regular fa-square"
                        class="keep-parents-checkbox"
                        v-else />

                    <div class="">
                        {{ t('page.convert.modal.keepParents') }}
                    </div>
                </div>
            </template>
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
