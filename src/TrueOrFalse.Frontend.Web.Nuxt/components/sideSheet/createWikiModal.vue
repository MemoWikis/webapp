<script lang="ts" setup>
import { messages } from '../alert/messages'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { useUserStore } from '../user/userStore'
const userStore = useUserStore()
const spinnerStore = useSpinnerStore()
interface Props {
    showModal: boolean
}
const props = defineProps<Props>()

const { $logger } = useNuxtApp()

const primaryBtnLabel = ref('Wiki erstellen')

const name = ref('')
const showErrorMsg = ref(false)
const privatePageLimitReached = ref(false)
const existingPageUrl = ref('')
const forbiddenWikiName = ref('')
const errorMsg = ref('')

const validateName = async () => {
    type PageNameValidationResult = {
        name: string
        url?: string
    }

    const result = await $api<FetchResult<PageNameValidationResult>>('/apiVue/PageRelationEdit/ValidateName', {
        method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success)
        return true
    else if (result.success == false) {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        forbiddenWikiName.value = result.data.name
        if (result.data.url)
            existingPageUrl.value = result.data.url
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return false
    }
}

const createWiki = async () => {
    if (!userStore.isLoggedIn) {
        userStore.showLoginModal = true
        return
    }
    spinnerStore.showSpinner()

    const nameIsValid = await validateName()

    if (!nameIsValid)
        return

    type QuickCreateResult = {
        name: string
        id: number
        cantSavePrivateWiki?: boolean
    }

    const result = await $api<FetchResult<QuickCreateResult>>('/apiVue/SideSheetCreateWikiModal/CreateWiki', {
        method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success) {
        spinnerStore.hideSpinner()
        emit('closeWikiModal')
        await nextTick()
        navigateTo(`/${result.data.name}/${result.data.id}`)
    } else if (result.success == false) {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        showErrorMsg.value = true

        if (result.data.cantSavePrivateWiki) {
            privatePageLimitReached.value = true
        }
        spinnerStore.hideSpinner()
    }
}

const emit = defineEmits(['closeWikiModal', 'wikiCreated'])

</script>

<template>
    <LazyModal :show="props.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="createWiki" @close="emit('closeWikiModal')" :show-cancel-btn="true">
        <template v-slot:header>
            <h4 class="modal-title">
                Wiki erstellen
            </h4>
        </template>
        <template v-slot:body>
            <form v-on:submit.prevent="createWiki">
                <div class="form-group">
                    <input class="form-control create-input" v-model="name"
                        placeholder="Bitte gib den Namen des Themas ein" />
                    <small class="form-text text-muted"></small>
                </div>
            </form>
            <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                <NuxtLink :href="existingPageUrl" class="alert-link">{{ forbiddenWikiName }}</NuxtLink>
                {{ errorMsg }}
            </div>
            <div class="link-to-sub-container" v-if="privatePageLimitReached">
                <NuxtLink to="/Preise" class="btn-link link-to-sub"><b>{{ messages.info.joinNow }}</b></NuxtLink>
            </div>
            <div class="pageIsPrivate" v-else>
                <p>
                    <b>Dieses Wiki ist privat.</b> Du kannst sie später im Dreipunkt-Menü oder direkt über das
                    Schloss-Icon veröffentlichen.
                </p>
            </div>
        </template>
    </LazyModal>
</template>

<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.link-to-sub-container {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 24px;
}

.create-input {
    border-radius: 0px;
}
</style>
