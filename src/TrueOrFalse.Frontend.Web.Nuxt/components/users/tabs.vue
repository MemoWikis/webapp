<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { Tab } from './tabsEnum'

const userStore = useUserStore()

interface Props {
    tab?: Tab
    allUserCount: number
    followingCount?: number
    followerCount?: number
}

const props = withDefaults(defineProps<Props>(), {
    followingCount: 0,
    followerCount: 0,
})



const { isMobile } = useDevice()

const emit = defineEmits(['setTab'])

const allUsersLabelEl = ref()
const networkLabelEl = ref()

function getWidth(e: VueElement) {
    if (e != null)
        return `width: ${e.clientWidth}px`
}
</script>

<template>
    <perfect-scrollbar>
        <div id="UsersTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

            <button class="tab" @click="emit('setTab', Tab.AllUsers)">

                <div class="tab-label chip-tab active" v-if="props.tab == Tab.AllUsers" :style="getWidth(allUsersLabelEl)">
                    Alle Nutzer
                    <div class="chip" v-if="props.allUserCount > 0">
                        {{ props.allUserCount }}
                    </div>
                </div>

                <div class="tab-label chip-tab" :class="{ 'invis': props.tab == Tab.AllUsers }" ref="allUsersLabelEl">
                    Alle Nutzer
                    <div class="chip" v-if="props.allUserCount > 0">
                        {{ props.allUserCount }}
                    </div>
                </div>


                <div class="active-tab" v-if="props.tab == Tab.AllUsers"></div>
                <div class="inactive-tab" v-else>
                    <div class="tab-border"></div>
                </div>
            </button>

            <button class="tab" @click="emit('setTab', Tab.Network)" v-if="userStore.isLoggedIn">

                <div class="tab-label chip-tab active" v-if="props.tab == Tab.Network" :style="getWidth(networkLabelEl)">
                    Mein Netzwerk
                    <div class="chip">
                        {{ props.followingCount }}
                        <font-awesome-icon icon="fa-solid fa-minus" />
                        {{ props.followerCount }}
                    </div>
                </div>
                <div class="tab-label chip-tab" :class="{ 'invis': props.tab == Tab.Network }" ref="networkLabelEl">
                    Mein Netzwerk
                    <div class="chip">
                        {{ props.followingCount }}
                        <font-awesome-icon icon="fa-solid fa-minus" />
                        {{ props.followerCount }}
                    </div>
                </div>
                <div class="active-tab" v-if="props.tab == Tab.Network"></div>
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
