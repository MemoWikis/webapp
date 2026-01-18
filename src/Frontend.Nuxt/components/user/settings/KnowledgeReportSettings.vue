<script lang="ts" setup>
const { t } = useI18n()

const showAlert = ref(false)
const success = ref(false)

// NotificationInterval is UserSettingNotificationInterval in backend
enum NotificationInterval {
    NotSet = 0,
    Never = 1,
    Daily = 2,
    Weekly = 3,
    Monthly = 4,
    Quarterly = 5
}
const selectedNotificationInterval = ref<NotificationInterval>(NotificationInterval.Weekly)

const getNotificationIntervalText = computed(() => {
    switch (selectedNotificationInterval.value) {
        case NotificationInterval.Never:
            return t('settings.knowledgeReport.interval.never')
        case NotificationInterval.Daily:
            return t('settings.knowledgeReport.interval.daily')
        case NotificationInterval.Weekly:
            return t('settings.knowledgeReport.interval.weekly')
        case NotificationInterval.Monthly:
            return t('settings.knowledgeReport.interval.monthly')
        case NotificationInterval.Quarterly:
            return t('settings.knowledgeReport.interval.quarterly')
        default:
            return t('settings.knowledgeReport.interval.notSelected')
    }
})

const notificationIntervalChangeMsg = ref('')

interface DefaultResult {
    success: boolean
    message: string
}

async function saveNotificationIntervalPreferences() {
    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangeNotificationIntervalPreferences', {
        mode: 'cors',
        method: 'POST',
        body: {
            notificationInterval: selectedNotificationInterval.value
        },
        credentials: 'include'
    })

    if (result.success) {
        notificationIntervalChangeMsg.value = result.message
        success.value = true
        showAlert.value = true
    } else {
        notificationIntervalChangeMsg.value = t('error.default')
        success.value = false
        showAlert.value = true
    }
}

function resetAlert() {
    showAlert.value = false
    notificationIntervalChangeMsg.value = ''
    success.value = false
}

defineExpose({
    resetAlert
})

const ariaId = useId()
</script>

<template>
    <div class="content">
        <div v-if="showAlert" class="settings-section">
            <div v-if="success" class="alert alert-success" v-html="notificationIntervalChangeMsg" />
            <div v-else class="alert alert-danger" v-html="notificationIntervalChangeMsg" />
        </div>
        <div class="settings-section">
            <div class="overline-s no-line">
                {{ t('settings.knowledgeReport.emailReport') }}
            </div>
            <div class="interval-dropdown">
                <VDropdown :aria-id="ariaId" :distance="0">
                    <div class="interval-select">
                        <div>
                            {{ getNotificationIntervalText }}
                        </div>
                        <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                    </div>

                    <template #popper="{ hide }">
                        <div class="dropdown-row select-row"
                            :class="{ 'active': selectedNotificationInterval === NotificationInterval.Quarterly }"
                            @click="selectedNotificationInterval = NotificationInterval.Quarterly; hide()">
                            <div class="dropdown-label select-option">
                                {{ t('settings.knowledgeReport.interval.quarterly') }}
                            </div>
                        </div>
                        <div class="dropdown-row"
                            :class="{ 'active': selectedNotificationInterval === NotificationInterval.Monthly }"
                            @click="selectedNotificationInterval = NotificationInterval.Monthly; hide()">
                            <div class="dropdown-label select-option">
                                {{ t('settings.knowledgeReport.interval.monthly') }}
                            </div>
                        </div>
                        <div class="dropdown-row select-row"
                            :class="{ 'active': selectedNotificationInterval === NotificationInterval.Weekly }"
                            @click="selectedNotificationInterval = NotificationInterval.Weekly; hide()">
                            <div class="dropdown-label select-option">
                                {{ t('settings.knowledgeReport.interval.weekly') }}
                            </div>
                        </div>
                        <div class="dropdown-row select-row"
                            :class="{ 'active': selectedNotificationInterval === NotificationInterval.Daily }"
                            @click="selectedNotificationInterval = NotificationInterval.Daily; hide()">
                            <div class="dropdown-label select-option">
                                {{ t('settings.knowledgeReport.interval.daily') }}
                            </div>
                        </div>
                        <div class="dropdown-row select-row"
                            :class="{ 'active': selectedNotificationInterval === NotificationInterval.Never }"
                            @click="selectedNotificationInterval = NotificationInterval.Never; hide()">
                            <div class="dropdown-label select-option">
                                {{ t('settings.knowledgeReport.interval.never') }}
                            </div>
                        </div>
                    </template>
                </VDropdown>
            </div>

            <p>
                {{ t('settings.knowledgeReport.description') }}
                <font-awesome-icon icon="fa-solid fa-heart" class="wish-knowledge-icon" />
                {{ t('settings.knowledgeReport.additionalInfo') }}
            </p>
        </div>

        <div class="settings-section">
            <button class="memo-button btn btn-primary" @click="saveNotificationIntervalPreferences()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                {{ t('settings.button.save') }}
            </button>
        </div>
    </div>
</template>
