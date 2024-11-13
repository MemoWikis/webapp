<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../userStore'
import { Tab } from './tabsEnum'

const userStore = useUserStore()

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
    <ClientOnly>
        <div>
            <div class="tabs-bar" :class="{ 'is-mobile': isMobile }">
                <perfect-scrollbar>
                    <div id="ProfileTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

                        <button class="tab" @click="emit('setTab', Tab.Overview)">
                            <div class="tab-label active" v-if="props.tab == Tab.Overview" style="width: 113px;"
                                :style="getWidth(overviewLabelEl)">Übersicht
                            </div>
                            <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Overview }"
                                ref="overviewLabelEl">
                                Übersicht</div>
                            <div class="active-tab" v-if="props.tab == Tab.Overview"></div>
                            <div class="inactive-tab" v-else>
                                <div class="tab-border"></div>
                            </div>
                        </button>

                        <button class="tab" @click="emit('setTab', Tab.Wishknowledge)">

                            <div class="tab-label active" v-if="props.tab == Tab.Wishknowledge"
                                :style="getWidth(wishknowledgeLabelEl)" style="width: 156px;">
                                Wunschwissen</div>
                            <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Wishknowledge }"
                                ref="wishknowledgeLabelEl">
                                Wunschwissen</div>

                            <div class="active-tab" v-if="props.tab == Tab.Wishknowledge"></div>
                            <div class="inactive-tab" v-else>
                                <div class="tab-border"></div>
                            </div>
                        </button>

                        <button class="tab" @click="emit('setTab', Tab.Settings)"
                            v-if="userStore.isLoggedIn && props.isCurrentUser">
                            <div class="tab-label active" v-if="props.tab == Tab.Settings"
                                :style="getWidth(settingsLabelEl)" style="width: 145px;">
                                Einstellungen</div>
                            <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Settings }"
                                ref="settingsLabelEl">
                                Einstellungen
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
            </div>
        </div>
        <template #fallback>
            <div id="ProfileTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

                <button class="tab" @click="emit('setTab', Tab.Overview)">
                    <div class="tab-label active" v-if="props.tab == Tab.Overview" style="width: 113px;"
                        :style="getWidth(overviewLabelEl)">Übersicht
                    </div>
                    <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Overview }" ref="overviewLabelEl">
                        Übersicht</div>
                    <div class="active-tab" v-if="props.tab == Tab.Overview"></div>
                    <div class="inactive-tab" v-else>
                        <div class="tab-border"></div>
                    </div>
                </button>

                <button class="tab" @click="emit('setTab', Tab.Wishknowledge)">

                    <div class="tab-label active" v-if="props.tab == Tab.Wishknowledge"
                        :style="getWidth(wishknowledgeLabelEl)" style="width: 156px;">
                        Wunschwissen</div>
                    <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Wishknowledge }"
                        ref="wishknowledgeLabelEl">
                        Wunschwissen</div>

                    <div class="active-tab" v-if="props.tab == Tab.Wishknowledge"></div>
                    <div class="inactive-tab" v-else>
                        <div class="tab-border"></div>
                    </div>
                </button>

                <button class="tab" @click="emit('setTab', Tab.Settings)"
                    v-if="userStore.isLoggedIn && props.isCurrentUser">
                    <div class="tab-label active" v-if="props.tab == Tab.Settings" :style="getWidth(settingsLabelEl)" style="width: 145px;">
                        Einstellungen</div>
                    <div class="tab-label" :class="{ 'invisible-tab': props.tab == Tab.Settings }" ref="settingsLabelEl">
                        Einstellungen
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
        </template>
    </ClientOnly>
</template>

<style lang="less">
@import '~~/assets/tabs-bar.less';
</style>
