<script lang="ts" setup>
import { UserSettingsTab } from './user-settings-tab.enum'

const { t } = useI18n()

interface Props {
    imageUrl?: string
    tab?: UserSettingsTab
}

const props = defineProps<Props>()

const activeContent = ref<UserSettingsTab>(UserSettingsTab.EditProfile)

onBeforeMount(() => {
    if (props.tab === UserSettingsTab.Membership) {
        activeContent.value = UserSettingsTab.Membership
    }
})

const emit = defineEmits(['updateProfile'])

// Component refs for resetting alerts
const editProfileRef = ref<InstanceType<typeof import('./EditProfileSettings.vue').default> | null>(null)
const passwordRef = ref<InstanceType<typeof import('./PasswordSettings.vue').default> | null>(null)
const deleteProfileRef = ref<InstanceType<typeof import('./DeleteProfileSettings.vue').default> | null>(null)
const wishKnowledgeRef = ref<InstanceType<typeof import('./WishKnowledgeSettings.vue').default> | null>(null)
const supportLoginRef = ref<InstanceType<typeof import('./SupportLoginSettings.vue').default> | null>(null)
const knowledgeReportRef = ref<InstanceType<typeof import('./KnowledgeReportSettings.vue').default> | null>(null)

watch(activeContent, () => {
    editProfileRef.value?.resetAlert()
    passwordRef.value?.resetAlert()
    deleteProfileRef.value?.resetAlert()
    wishKnowledgeRef.value?.resetAlert()
    supportLoginRef.value?.resetAlert()
    knowledgeReportRef.value?.resetAlert()
})

const getSelectedSettingsPageLabel = computed(() => {
    switch (activeContent.value) {
        case UserSettingsTab.EditProfile:
            return t('settings.navigation.editProfile')
        case UserSettingsTab.Password:
            return t('settings.navigation.password')
        case UserSettingsTab.DeleteProfile:
            return t('settings.navigation.deleteProfile')
        case UserSettingsTab.ShowWishKnowledge:
            return t('settings.navigation.showWishKnowledge')
        case UserSettingsTab.SupportLogin:
            return t('settings.navigation.supportLogin')
        case UserSettingsTab.Membership:
            return t('settings.navigation.membership')
        case UserSettingsTab.General:
            return t('settings.navigation.general')
        case UserSettingsTab.KnowledgeReport:
            return t('settings.navigation.knowledgeReport')
        case UserSettingsTab.AiUsage:
            return t('settings.navigation.aiUsage')
        default:
            return ''
    }
})

const ariaId = useId()
</script>

<template>
    <div class="user-settings-container">
        <div class="navigation">
            <div class="overline-s no-line">{{ t('settings.navigation.profileInfo') }}</div>
            <button :class="{ 'active': activeContent === UserSettingsTab.EditProfile }"
                @click="activeContent = UserSettingsTab.EditProfile">
                <font-awesome-icon :icon="['fas', 'user']" class="nav-icon" />
                {{ t('settings.navigation.editProfile') }}
            </button>
            <button :class="{ 'active': activeContent === UserSettingsTab.Password }"
                @click="activeContent = UserSettingsTab.Password">
                <font-awesome-icon :icon="['fas', 'key']" class="nav-icon" />
                {{ t('settings.navigation.password') }}
            </button>

            <div class="divider" />
            <div class="overline-s no-line">{{ t('settings.navigation.membership') }}</div>
            <button :class="{ 'active': activeContent === UserSettingsTab.Membership }"
                @click="activeContent = UserSettingsTab.Membership">
                <font-awesome-icon :icon="['fas', 'credit-card']" class="nav-icon" />
                {{ t('settings.navigation.membership') }}
            </button>
            <button :class="{ 'active': activeContent === UserSettingsTab.AiUsage }"
                @click="activeContent = UserSettingsTab.AiUsage">
                <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="nav-icon" />
                {{ t('settings.navigation.aiUsage') }}
            </button>

            <div class="divider" />
            <div class="overline-s no-line">{{ t('settings.navigation.settings') }}</div>
            <button :class="{ 'active': activeContent === UserSettingsTab.ShowWishKnowledge }"
                @click="activeContent = UserSettingsTab.ShowWishKnowledge">
                <font-awesome-icon :icon="['fas', 'heart']" class="nav-icon" />
                {{ t('settings.navigation.showWishKnowledge') }}
            </button>
            <button :class="{ 'active': activeContent === UserSettingsTab.KnowledgeReport }"
                @click="activeContent = UserSettingsTab.KnowledgeReport">
                <font-awesome-icon :icon="['fas', 'bell']" class="nav-icon" />
                {{ t('settings.navigation.knowledgeReport') }}
            </button>
            <button :class="{ 'active': activeContent === UserSettingsTab.SupportLogin }"
                @click="activeContent = UserSettingsTab.SupportLogin">
                <font-awesome-icon :icon="['fas', 'headset']" class="nav-icon" />
                {{ t('settings.navigation.supportLogin') }}
            </button>

            <div class="divider" />
            <div class="overline-s no-line">{{ t('settings.navigation.deleteProfile') }}</div>
            <button :class="{ 'active': activeContent === UserSettingsTab.DeleteProfile }"
                @click="activeContent = UserSettingsTab.DeleteProfile">
                <font-awesome-icon :icon="['fas', 'triangle-exclamation']" class="nav-icon" />
                {{ t('settings.navigation.deleteProfile') }}
            </button>
        </div>
        <div class="navigation-mobile">
            <div class="settings-dropdown">
                <VDropdown :aria-id="ariaId" :distance="0">
                    <div class="settings-select">
                        <div>
                            {{ getSelectedSettingsPageLabel }}
                        </div>
                        <font-awesome-icon :icon="['fas', 'bars']" />
                    </div>

                    <template #popper="{ hide }">
                        <div class="mobile-dropdown">
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.profileInfo') }}
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.EditProfile }"
                                @click="activeContent = UserSettingsTab.EditProfile; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'user']" class="nav-icon" />
                                    {{ t('settings.navigation.editProfile') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.Password }"
                                @click="activeContent = UserSettingsTab.Password; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'key']" class="nav-icon" />
                                    {{ t('settings.navigation.password') }}
                                </div>
                            </div>
                            <div class="divider" />
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.membership') }}
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.Membership }"
                                @click="activeContent = UserSettingsTab.Membership; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'credit-card']" class="nav-icon" />
                                    {{ t('settings.navigation.membership') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.AiUsage }"
                                @click="activeContent = UserSettingsTab.AiUsage; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="nav-icon" />
                                    {{ t('settings.navigation.aiUsage') }}
                                </div>
                            </div>
                            <div class="divider" />
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.settings') }}
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.ShowWishKnowledge }"
                                @click="activeContent = UserSettingsTab.ShowWishKnowledge; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'heart']" class="nav-icon" />
                                    {{ t('settings.navigation.showWishKnowledge') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.KnowledgeReport }"
                                @click="activeContent = UserSettingsTab.KnowledgeReport; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'bell']" class="nav-icon" />
                                    {{ t('settings.navigation.knowledgeReport') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.SupportLogin }"
                                @click="activeContent = UserSettingsTab.SupportLogin; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'headset']" class="nav-icon" />
                                    {{ t('settings.navigation.supportLogin') }}
                                </div>
                            </div>
                            <div class="divider" />
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.deleteProfile') }}
                            </div>
                            <div class="dropdown-row select-row"
                                :class="{ 'active': activeContent === UserSettingsTab.DeleteProfile }"
                                @click="activeContent = UserSettingsTab.DeleteProfile; hide()">
                                <div class="dropdown-label select-option">
                                    <font-awesome-icon :icon="['fas', 'triangle-exclamation']" class="nav-icon" />
                                    {{ t('settings.navigation.deleteProfile') }}
                                </div>
                            </div>
                        </div>
                    </template>
                </VDropdown>
            </div>
        </div>
        <div class="settings-content">
            <Transition>
                <UserSettingsEditProfileSettings v-if="activeContent === UserSettingsTab.EditProfile"
                    ref="editProfileRef" :image-url="imageUrl" @update-profile="emit('updateProfile')" />

                <UserSettingsPasswordSettings v-else-if="activeContent === UserSettingsTab.Password"
                    ref="passwordRef" />

                <UserSettingsDeleteProfileSettings v-else-if="activeContent === UserSettingsTab.DeleteProfile"
                    ref="deleteProfileRef" />

                <UserSettingsWishKnowledgeSettings v-else-if="activeContent === UserSettingsTab.ShowWishKnowledge"
                    ref="wishKnowledgeRef" />

                <UserSettingsSupportLoginSettings v-else-if="activeContent === UserSettingsTab.SupportLogin"
                    ref="supportLoginRef" />

                <UserSettingsMembershipSettings v-else-if="activeContent === UserSettingsTab.Membership" />

                <UserSettingsAiUsageSettings v-else-if="activeContent === UserSettingsTab.AiUsage" />

                <div v-else-if="activeContent === UserSettingsTab.General" class="content" />

                <UserSettingsKnowledgeReportSettings v-else-if="activeContent === UserSettingsTab.KnowledgeReport"
                    ref="knowledgeReportRef" />
            </Transition>
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.group-label {
    background-color: @memo-blue;
    color: white;
}

.user-settings-container {
    display: flex;
    flex-direction: row;
    gap: 48px;

    .email-confirmation-container {
        display: flex;
        justify-content: space-between;
        align-items: center;

        .email-verification-label {

            &.verified {
                svg {
                    color: @memo-green;

                }
            }

            &.not-verified {
                svg {
                    color: @memo-wish-knowledge-red;
                }
            }
        }

        svg {
            margin-right: 2px;
        }
    }

    .mobile-dropdown {
        width: calc(100vw - 20px);

    }

    .group-label {
        font-weight: 600;
    }

    p {
        margin: 10px 0;
        margin-top: 5px;
    }

    .settings-dropdown {
        padding-top: 30px;
        width: 100%;

        .settings-select {
            width: 100%;
            color: @memo-blue-link;
            font-weight: 600;
        }
    }

    .nav-icon {
        margin-right: 8px;
    }


    .interval-dropdown {
        width: 190px;
    }


    .v-popper--shown {

        .settings-select,
        .interval-select {

            .chevron {
                transform: rotate(180deg)
            }
        }
    }

    .settings-select,
    .interval-select {
        padding: 6px 12px;
        padding-left: 0;
        height: 34px;
        cursor: pointer;
        border: solid 1px @memo-grey-light;
        background: white;
        display: flex;
        justify-content: space-between;
        align-items: center;
        user-select: none;

        &:hover {
            color: @memo-blue;
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    .interval-select {
        width: 190px;
    }

    .checkbox-section {
        margin-top: -5px;
        cursor: pointer;
        display: flex;
        flex-wrap: nowrap;

        .checkbox-label {
            .overline-s {
                margin-top: 0;
                padding-top: 10px;
                margin-bottom: 10px;
            }
        }
    }

    .settings-input {
        resize: none;
        height: 44px;
        overflow: hidden;
        width: 100%;
        padding: 0 15px 0;
        border: solid @memo-grey-light 1px;
        box-shadow: none;
        color: @memo-grey-dark !important;
        outline: none;

        &:focus {
            border: solid 1px @memo-green;
            box-shadow: none;
        }

        &:active {
            border: solid 1px @memo-green;
            box-shadow: none;
        }
    }

    .settings-section {
        margin-bottom: 40px;

        &.plans {
            margin-left: -10px;
            margin-right: -10px;
            margin-bottom: 0;
        }
    }

    .password-section {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .profile-picture {
        width: 166px;
        min-width: 166px;
        height: 166px;
        margin: 10px 0;
    }

    .img-settings-btns {
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;

        .img-upload-btn,
        .img-delete-btn {
            input[type="file"] {
                display: none;
            }

            border-radius: 24px;

            color: @memo-blue-link;
            cursor: pointer;
            background: white;
            padding: 6px 12px;
            border: none;
            text-align: left;

            &:hover {
                color: @memo-blue;
            }
        }
    }

    .generic-btn-link {
        padding: 0px;
    }

    .divider {
        margin-top: 20px;
        height: 1px;
        background: @memo-grey-lighter;
        width: 100%;
        margin-bottom: 10px;
    }

    .navigation {
        width: 25%;
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;

        button {
            background: white;
            text-align: left;
            color: @memo-grey-dark;
            padding: 12px 20px;
            border-radius: 24px;
            outline: none;

            &.active {
                color: @memo-blue-link;
                font-weight: 600;
            }

            &:hover {
                color: @memo-blue;
            }

            &:active {
                color: @memo-blue;
            }
        }

        .overline-s,
        button {
            padding-left: 2px;
            padding-right: 20px;
        }

    }

    .overline-s {
        margin: 5px 0;
    }


    .navigation,
    .settings-content {
        padding-top: 10px;
    }

    .wish-knowledge-icon {
        color: @memo-wish-knowledge-red;
    }

    .settings-content {
        max-width: 1200px;
        width: 75%;
        flex-grow: 2;
    }

    @media (max-width: 900px) {

        flex-direction: column;

        .navigation {
            display: none;
        }

        .settings-content {
            width: 100%;
            padding-left: 0;
        }
    }

    @media (min-width: 901px) {
        .navigation {
            width: 25%;
        }

        .navigation-mobile {
            display: none;
        }

        .settings-content {
            width: 75%;
        }
    }
}

.sidesheet-open {
    .user-settings-container {
        @media (max-width: 1209px) {

            flex-direction: column;

            .navigation {
                display: none;
            }

            .settings-content {
                width: 100%;
            }
        }

        @media (min-width: 1210px) {
            .navigation {
                width: 25%;
            }

            .navigation-mobile {
                display: none;
            }

            .settings-content {
                width: 75%;
            }
        }
    }
}
</style>