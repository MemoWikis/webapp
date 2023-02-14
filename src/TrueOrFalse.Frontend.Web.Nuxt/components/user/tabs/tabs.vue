<script lang="ts" setup>
import { Tab } from './tabsEnum'

interface Props {
    tab: Tab
    badgeCount: number
    maxBadgeCount: number
}

const props = defineProps<Props>()


const { isMobile } = useDevice()

const emit = defineEmits(['setTab'])
</script>

<template>
    <perfect-scrollbar>
        <div id="ProfileTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

            <div class="tab" @click="emit('setTab', Tab.Overview)">
                <div class="tab-label">Übersicht</div>
                <div class="active-tab" v-if="props.tab == Tab.Overview"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </div>

            <div class="tab" @click="emit('setTab', Tab.Wishknowledge)">
                <div class="tab-label">Wunschwissen</div>
                <div class="active-tab" v-if="props.tab == Tab.Wishknowledge"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </div>

            <div class="tab" @click="emit('setTab', Tab.Badges)">
                <div class="tab-label learning-tab">Badges
                    <div class="chip" v-if="props.maxBadgeCount > 0">
                        {{ props.badgeCount }}/{{ props.maxBadgeCount }}
                    </div>
                </div>
                <div class="active-tab" v-if="props.tab == Tab.Badges"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </div>

            <div class="tab" @click="emit('setTab', Tab.Settings)">
                <div class="tab-label">Einstellungen</div>
                <div class="active-tab" v-if="props.tab == Tab.Settings"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </div>

            <div class="tab-filler-container">
                <div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
                <div class="inactive-tab">
                    <div class="tab-border"></div>
                </div>
            </div>

        </div>
    </perfect-scrollbar>

</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.ps {
    max-width: calc(100vw - 20px);
    // flex-shrink: 1;
    width: 100%;
}

#ProfileTabBar {
    text-align: center;
    flex-grow: 1;
    color: @memo-grey-dark;
    display: flex;
    margin-top: 30px;

    ::-webkit-scrollbar {
        height: 4px;
    }

    ::-webkit-scrollbar-thumb {
        background-color: @memo-grey-dark;
        cursor: pointer;
        border-radius: 4px;

        &:hover {
            background-color: @memo-grey-darker;

        }
    }

    .tab,
    .tab-filler-container {
        height: 100%;
        justify-content: center;
        align-items: center;
        text-align: center;
        font-weight: 700;
        font-size: 18px;

        .tab-label,
        .tab-filler {
            padding: 4px 20px;
            height: 34px;
            white-space: nowrap;

            &.mobile {
                padding: 0;
            }
        }

        .tab-filler {
            width: 100%;
        }

        .active-tab,
        .inactive-tab {
            height: 5px;
            width: inherit;
            z-index: 2;
            bottom: 0;
        }

        .active-tab {
            background: @memo-blue;
            border-radius: 4px;
        }

        .inactive-tab {
            display: flex;
            justify-content: center;
            align-items: center;
            width: 100%;

            .tab-border {
                height: 1px;
                background: @memo-grey-light;
                width: 100%;
            }
        }
    }

    &.is-mobile {
        font-size: 16px;

        .tab-label,
        .tab-filler {
            padding: 4px 12px;
        }
    }

    .tab {

        .tab-label {
            border-radius: 12px;

            .chip {
                border-radius: 20px;
                display: flex;
                justify-content: center;
                align-items: center;
                padding: 0 8px;
                background: @memo-grey-lighter;
                font-size: 12px;
                margin-left: 4px;
                margin-right: -8px;
                height: 24px;
            }

            &.learning-tab {
                display: flex;
                flex-wrap: nowrap;
            }
        }

        &:hover {
            color: @memo-blue;
            cursor: pointer;
        }

        &:active {
            .tab-label {
                transition: filter 0.1s;
                background: white;
                border-radius: 24px;
                filter: brightness(0.95)
            }

        }
    }

    .tab-filler-container {
        width: 100%;
    }
}
</style>
