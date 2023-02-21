<script lang="ts" setup>
import { Tab } from './tabsEnum'

interface Props {
    tab: Tab
    badgeCount: number
    maxBadgeCount: number
    isCurrentUser: boolean
}

const props = defineProps<Props>()


const { isMobile } = useDevice()

const emit = defineEmits(['setTab'])
</script>

<template>
    <perfect-scrollbar>
        <div id="ProfileTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

            <button class="tab" @click="emit('setTab', Tab.Overview)">
                <div class="tab-label" :class="{ 'active': props.tab == Tab.Overview }">Übersicht</div>
                <div class="active-tab" v-if="props.tab == Tab.Overview"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Wishknowledge)">
                <div class="tab-label" :class="{ 'active': props.tab == Tab.Wishknowledge }">Wunschwissen</div>
                <div class="active-tab" v-if="props.tab == Tab.Wishknowledge"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Badges)">
                <div class="tab-label learning-tab" :class="{ 'active': props.tab == Tab.Badges }">Badges
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
                <div class="tab-label" :class="{ 'active': props.tab == Tab.Settings }">Einstellungen</div>
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

<style scoped lang="less">
@import '~~/assets/tabs-bar.less';
</style>
