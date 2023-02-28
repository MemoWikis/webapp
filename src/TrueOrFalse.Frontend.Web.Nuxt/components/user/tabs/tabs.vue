<script lang="ts" setup>
import { VueElement } from 'vue'
import { Tab } from './tabsEnum'

interface Props {
    tab?: Tab
    badgeCount: number
    maxBadgeCount: number
    isCurrentUser: boolean
}

const props = defineProps<Props>()

const { isMobile } = useDevice()

const emit = defineEmits(['setTab'])

const overviewLabelEl = ref()
const wishknowledgeLabelEl = ref()
const badgesLabelEl = ref()
const settingsLabelEl = ref()

function getWidth(e: VueElement) {
    if (e != null)
        return `width: ${e.clientWidth}px`
}
</script>

<template>
    <perfect-scrollbar>
        <div id="ProfileTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

            <button class="tab" @click="emit('setTab', Tab.Overview)">
                <div class="tab-label active" v-if="props.tab == Tab.Overview" style="width: 113px;"
                    :style="getWidth(overviewLabelEl)">Übersicht
                </div>
                <div class="tab-label" :class="{ 'invis': props.tab == Tab.Overview }" ref="overviewLabelEl">Übersicht</div>
                <div class="active-tab" v-if="props.tab == Tab.Overview"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Wishknowledge)">

                <div class="tab-label active" v-if="props.tab == Tab.Wishknowledge" :style="getWidth(wishknowledgeLabelEl)">
                    Wunschwissen</div>
                <div class="tab-label" :class="{ 'invis': props.tab == Tab.Wishknowledge }" ref="wishknowledgeLabelEl">
                    Wunschwissen</div>

                <div class="active-tab" v-if="props.tab == Tab.Wishknowledge"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Badges)">

                <div class="tab-label chip-tab active" v-if="props.tab == Tab.Badges" :style="getWidth(badgesLabelEl)">
                    Badges
                    <div class="chip" v-if="props.maxBadgeCount > 0">
                        {{ props.badgeCount }}/{{ props.maxBadgeCount }}
                    </div>
                </div>
                <div class="tab-label chip-tab" :class="{ 'invis': props.tab == Tab.Badges }" ref="badgesLabelEl">
                    Badges
                    <div class="chip" v-if="props.maxBadgeCount > 0">
                        {{ props.badgeCount }}/{{ props.maxBadgeCount }}
                    </div>
                </div>

                <div class="active-tab" v-if="props.tab == Tab.Badges"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Settings)" v-if="props.isCurrentUser">
                <div class="tab-label active" v-if="props.tab == Tab.Settings" :style="getWidth(settingsLabelEl)">
                    Einstellungen</div>
                <div class="tab-label" :class="{ 'invis': props.tab == Tab.Settings }" ref="settingsLabelEl">Einstellungen
                </div>

                <div class="active-tab" v-if="props.tab == Tab.Settings"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <div class="tab-filler-container">
                <div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
                <div class="inactive-tab">
                    <div class="tab-border"></div>
                </div>
            </div>

        </div>
    </perfect-scrollbar>
</template>

<style lang="less">
@import '~~/assets/tabs-bar.less';
</style>
