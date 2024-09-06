<script lang="ts" setup>
import { useActivityPointsStore } from '~/components/activityPoints/activityPointsStore'
import { FontSize, useUserStore } from '~/components/user/userStore'
import { ImageFormat } from '~/components/image/imageFormatEnum'

const userStore = useUserStore()
const activityPointsStore = useActivityPointsStore()

const { isMobile } = useDevice()
const ariaId = useId()
</script>

<template>
    <VDropdown :aria-id="ariaId" :distance="6">
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
                    <div class="user-dropdown-label">Deine Lernpunkte</div>

                    <div class="user-dropdown-container level-info">
                        <div class="primary-info">
                            Mit {{ activityPointsStore.points }} <b>Lernpunkten</b> <br />
                            bist du in <b>Level {{ activityPointsStore.level }}</b>.
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
                            Noch {{ activityPointsStore.activityPointsTillNextLevel }} Punkte <br />
                            bis Level {{ activityPointsStore.level + 1 }}
                        </div>
                    </div>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-font-size-selector">
                    <div class="user-dropdown-label">Schriftgröße</div>
                    <div class="user-dropdown-container">
                        <div class="font-size-selector">
                            <div @click="userStore.setFontSize(FontSize.Small)" class="font-size-selector-btn small" :class="{ 'is-active': userStore.fontSize == FontSize.Small }">Aa</div>
                            <div @click="userStore.setFontSize(FontSize.Medium)" class="font-size-selector-btn medium" :class="{ 'is-active': userStore.fontSize == FontSize.Medium }">Aa</div>
                            <div @click="userStore.setFontSize(FontSize.Large)" class="font-size-selector-btn large" :class="{ 'is-active': userStore.fontSize == FontSize.Large }">Aa</div>
                        </div>
                    </div>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-social">
                    <NuxtLink :to="`/Nutzer/${encodeURI(userStore.name)}/${userStore.id}`" @click="hide()">
                        <div class="user-dropdown-label">Deine Profilseite</div>
                    </NuxtLink>
                </div>

                <div class="divider"></div>

                <div class="user-dropdown-managment">
                    <NuxtLink @click="hide()" :to="`/Nutzer/Einstellungen`">
                        <div class="user-dropdown-label">Konto-Einstellungen</div>
                    </NuxtLink>

                    <LazyNuxtLink to="/Maintenance" @click="hide()" v-if="userStore.isAdmin">
                        <div class="user-dropdown-label" @click="hide()">
                            Administrativ
                        </div>
                    </LazyNuxtLink>
                    <div class="user-dropdown-label" @click="userStore.logout(), hide()">
                        Ausloggen
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
            background: @memo-wuwi-red;
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
                    background: @memo-wuwi-red;
                    color: white;
                }
            }
        }
    }

    .user-dropdown-container {
        padding: 10px 25px;
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
</style>