<script lang="ts" setup>
const { t } = useI18n()

const showWishKnowledge = ref(false)

const showAlert = ref(false)
const msg = ref('')
const success = ref(false)

interface DefaultResult {
    success: boolean
    message: string
}

async function saveWishKnowledgeVisibility() {
    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangeWishKnowledgeVisibility', {
        mode: 'cors',
        method: 'POST',
        body: {
            showWishKnowledge: showWishKnowledge.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = t(result.message)
        success.value = true
        showAlert.value = true
    } else {
        msg.value = t(result.message)
        success.value = false
        showAlert.value = true
    }
}

function resetAlert() {
    showAlert.value = false
    msg.value = ''
    success.value = false
}

defineExpose({
    resetAlert
})
</script>

<template>
    <div class="content">
        <div v-if="showAlert" class="settings-section">
            <div v-if="success" class="alert alert-success">{{ msg }}</div>
            <div v-else class="alert alert-danger">{{ msg }}</div>
        </div>
        <div class="settings-section">
            <label class="checkbox-section">
                <div class="checkbox-container">
                    <input v-model="showWishKnowledge" type="checkbox" name="answer" :value="true" class="hidden" />
                    <font-awesome-icon v-if="showWishKnowledge" icon="fa-solid fa-square-check" class="checkbox-icon" />
                    <font-awesome-icon v-else icon="fa-regular fa-square" class="checkbox-icon" />
                </div>
                <div class="checkbox-label">
                    <div class="overline-s no-line">
                        {{ t('settings.wishKnowledge.showWishKnowledge') }}
                    </div>
                    <p>
                        {{ t('settings.wishKnowledge.explanation') }}
                    </p>
                </div>
            </label>
        </div>

        <div class="settings-section">
            <button class="memo-button btn btn-primary" @click="saveWishKnowledgeVisibility()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                {{ t('settings.button.save') }}
            </button>
        </div>
    </div>
</template>
