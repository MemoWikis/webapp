<script lang="ts" setup>
import { useActivityPointsStore } from '~/components/activityPoints/activityPointsStore'
import { FontSize, useUserStore } from '~/components/user/userStore'
import { ImageFormat } from '~/components/image/imageFormatEnum'

const userStore = useUserStore()
const activityPointsStore = useActivityPointsStore()
const { t } = useI18n()

const { isMobile } = useDevice()

const { locale, locales, setLocale, localeProperties } = useI18n()
const showLanguages = ref(false)

const settingsUrl = computed(() => {
    const { resolve } = useRouter()

    switch (locale.value) {
        case 'de':
            return resolve({ name: 'userSettingsPageDE' }).href
        case 'fr':
            return resolve({ name: 'userSettingsPageFR' }).href
        case 'es':
            return resolve({ name: 'userSettingsPageES' }).href
        case 'en':
        default:
            return resolve({ name: 'userSettingsPageEN' }).href
    }
})
</script>

<template>
    <VDropdown aria-id="VD-HeaderUserDropdown" :distance="6">
        <div class="header-btn">
            <Image :src="userStore.imgUrl" :format="ImageFormat.Author" class="header-author-icon"
                :alt="`${userStore.name}'s profile picture'`" />
            <div class="header-user-name" v-if="!isMobile">
                {{ userStore.name }}
            </div>
            <div class="user-dropdown-chevron">
                <font-awesome-icon icon="fa-solid fa-chevron-down" />
            </div>
        </div>
        <template #popper="{ hide }">

            <div class="user-dropdown">

                <template v-if="isMobile">
                    <div class="user-dropdown-name">
                        <div class="user-dropdown-label word-break">
                            {{ userStore.name }}
                        </div>
                    </div>
                    <div class="divider"></div>
                </template>

                <div class="user-dropdown-info">
                    <div class="user-dropdown-label">{{ t('userDropdown.activitySection.title') }}</div>

                    <div class="user-dropdown-container level-info">
                        <div class="primary-info">
                            <i18n-t keypath="userDropdown.activitySection.text" tag="p">
                                <template #learningPoints>
                                    <b>
                                        {{ t('userDropdown.activitySection.learningPoints', activityPointsStore.points) }}
                                    </b>
                                </template>
                                <template #breakpoint>
                                    <br />
                                </template>
                                <template #level>
                                    <b>
                                        {{ t('userDropdown.activitySection.level', activityPointsStore.level) }}
                                    </b>
                                </template>
                            </i18n-t>
                        </div>
                        <div class="progress-bar-container">
                            <div class="p-bar">
                                <div class="p-bar-a" :style="`left: -${100 - activityPointsStore.activityPointsPercentageOfNextLevel}%`">
                                </div>
                                <div class="p-bar-label-grey" v-if="activityPointsStore.activityPointsPercentageOfNextLevel < 30">
                                    {{ activityPointsStore.activityPointsPercentageOfNextLevel }}%
                                </div>
                                <div class="p-bar-label" v-else :style="`width: ${activityPointsStore.activityPointsPercentageOfNextLevel}%`">
                                    {{ activityPointsStore.activityPointsPercentageOfNextLevel }}%
                                </div>

                            </div>
                        </div>
                        <div class="secondary-info">
                            <i18n-t keypath="userDropdown.activitySection.nextLevelText" tag="p">
                                <template #pointsToNextLevel>
                                    <b> {{ activityPointsStore.activityPointsTillNextLevel }} </b>
                                </template>
                                <template #breakpoint>
                                    <br />
                                </template>
                                <template #nextLevel>
                                    <b>
                                        {{ activityPointsStore.level + 1 }}
                                    </b>
                                </template>
                            </i18n-t>
                        </div>
                    </div>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-font-size-selector">
                    <div class="user-dropdown-label">{{ t('label.fontSize') }}</div>
                    <div class="user-dropdown-container">
                        <div class="font-size-selector">
                            <div @click="userStore.setFontSize(FontSize.Small)" class="font-size-selector-btn small" :class="{ 'is-active': userStore.fontSize === FontSize.Small }">{{ t('label.fontSizeSample') }}</div>
                            <div @click="userStore.setFontSize(FontSize.Medium)" class="font-size-selector-btn medium" :class="{ 'is-active': userStore.fontSize === FontSize.Medium }">{{ t('label.fontSizeSample') }}</div>
                            <div @click="userStore.setFontSize(FontSize.Large)" class="font-size-selector-btn large" :class="{ 'is-active': userStore.fontSize === FontSize.Large }">{{ t('label.fontSizeSample') }}</div>
                        </div>
                    </div>
                </div>

                <div class="user-dropdown-language-selector">
                    <div class="user-dropdown-label language-header" @click.prevent="showLanguages = !showLanguages">
                        {{ t('label.language') }}
                        <CircleFlags :country="localeProperties.flag" class="country-flag" />
                    </div>
                    <div class="user-dropdown-container" :class="{ 'hidden': !showLanguages }">

                        <Transition name="collapse">
                            <div class="language-selector" v-if="showLanguages">
                                <div v-for="l in locales">
                                    <div @click.prevent.stop="setLocale(l.code)" class="language-selector-btn" :class="{ 'is-active': l.code === locale }">
                                        {{ l.name }}
                                    </div>
                                </div>
                            </div>
                        </Transition>

                    </div>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-social">
                    <NuxtLink :to="`/Nutzer/${encodeURI(userStore.name)}/${userStore.id}`" @click="hide()">
                        <div class="user-dropdown-label">{{ t('label.yourProfilePage') }}</div>
                    </NuxtLink>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-managment">
                    <NuxtLink @click="hide()" :to="settingsUrl">
                        <div class="user-dropdown-label">
                            {{ t('label.accountSettings') }}
                        </div>
                    </NuxtLink>

                    <LazyNuxtLink to="/Maintenance" @click="hide()" v-if="userStore.isAdmin">
                        <div class="user-dropdown-label" @click="hide()">
                            {{ t('label.administrative') }}
                        </div>
                    </LazyNuxtLink>

                    <div class="user-dropdown-label" @click="userStore.logout(), hide()">
                        {{ t('label.logout') }}
                    </div>
                </div>
            </div>
        </template>
    </VDropdown>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.header-author-icon {
    height: 32px;
    width: 32px;
    margin-left: -8px;
    margin-right: 4px;
}

.v-popper {
    height: 100%;
    padding: 4px 0;
}

.header-btn {
    cursor: pointer;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    background: white;
    padding: 4px 12px;
    border-radius: 24px;
    transition: filter 0.1s;
    user-select: none;

    &:hover {
        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }

    .header-user-name {
        font-weight: 600;
        padding: 0 4px;
    }

    .unread-msg-badge-container {
        position: relative;
        width: 100%;

        .unread-msg-badge {
            background: @memo-wish-knowledge-red;
            height: 12px;
            width: 12px;
            border-radius: 12px;
            position: absolute;
            border: 2px solid white;
            top: -2px;
            right: -2px;
        }
    }
}

.v-popper--shown {

    .user-dropdown-chevron {
        transform: rotate3d(1, 0, 0, 180deg);
    }
}

.user-dropdown {
    .word-break {
        word-break: break-all;
    }

    .user-dropdown-label {
        padding: 10px 25px;

        &:hover {
            background-color: @memo-grey-lighter;
            cursor: pointer;
        }

        &.has-badge {
            display: flex;
            align-items: center;

            .counter-badge {
                height: 16px;
                border-radius: 16px;
                padding: 0 5px;
                display: flex;
                justify-content: center;
                align-items: center;
                background: @memo-grey-light;
                color: @memo-grey-dark;
                font-size: 10px;
                font-weight: 700;
                margin-left: 8px;

                &.red-badge {
                    background: @memo-wish-knowledge-red;
                    color: white;
                }
            }
        }
    }

    .user-dropdown-container {
        padding: 10px 25px;

        &.hidden {
            padding: 0px;
        }
    }

    a {
        text-decoration: none;
    }

    color: @memo-grey-darker;
}

.user-dropdown-info {

    .user-dropdown-label {
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        padding-bottom: 0px;

        &:hover {
            background-color: unset;
            cursor: default;
        }
    }

    .progress-bar-container {
        background: @memo-grey-light;
        border-radius: 25px;
        height: 25px;
        width: 100%;
        margin: 6px 0;

        .p-bar {
            position: relative;
            width: 100%;
            height: 25px;
            overflow: hidden;
            border-radius: 25px;

            .p-bar-a {
                position: absolute;
                display: flex;
                justify-content: center;
                align-items: center;
                border-radius: 25px;
                height: 25px;
                background: @memo-green;
                width: 100%;
            }

            .p-bar-label,
            .p-bar-label-grey {
                position: absolute;
                display: flex;
                justify-content: center;
                align-items: center;
                height: 25px;
                font-weight: 600;
            }

            .p-bar-label {
                color: white;
            }

            .p-bar-label-grey {
                color: @memo-grey-darker;
                width: 100%;
            }
        }


    }

    .primary-info,
    .secondary-info {
        font-size: 12px;
        text-align: center;
        padding: 6px 0;

        b {
            font-weight: 600;
        }
    }

    .secondary-info {
        color: @memo-grey-dark;
    }
}

.user-dropdown-font-size-selector {
    .user-dropdown-label {
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        padding-bottom: 0px;

        &:hover {
            background-color: unset;
            cursor: default;
        }

        &:hover {
            background-color: unset;
            cursor: unset;
        }
    }

    .font-size-selector {
        display: flex;
        justify-content: space-around;
        padding: 10px 0;

        .is-active {
            background-color: @memo-grey-lighter;
        }

        .font-size-selector-btn {
            display: flex;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            border-radius: 24px;
            padding: 4px 12px;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &:active {
                background-color: @memo-grey-light;
            }

            &.small {
                font-size: 16px;
            }

            &.medium {
                font-size: 18px;
            }

            &.large {
                font-size: 20px;
            }
        }
    }
}

.user-dropdown-language-selector {
    .user-dropdown-label {
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        padding-bottom: 0px;

        &.language-header {
            background: white;
            user-select: none;
            padding-bottom: 10px;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &:active {
                background-color: @memo-grey-light;
            }

            .country-flag {
                height: 2rem;
                width: 2rem;
                margin-left: 8px;
            }
        }

        .user-dropdown-container {
            padding-top: none;
        }

    }

    .language-selector {
        display: flex;
        justify-content: center;
        flex-direction: column;
        padding: 10px 0;

        .is-active {
            background-color: @memo-grey-lighter;
        }

        .language-selector-btn {
            display: flex;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            border-radius: 24px;
            padding: 3px 12px;
            margin: 1px 0;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &:active {
                background-color: @memo-grey-light;
            }


        }
    }
}
</style>