<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { Tab } from './tabsEnum'

const userStore = useUserStore()

interface Props {
    allUserCount: number
    followingCount?: number
    followerCount?: number
}

const props = withDefaults(defineProps<Props>(), {
    followingCount: 0,
    followerCount: 0,
})



const { isMobile } = useDevice()

const allUsersLabelEl = ref()

function getWidth(e: VueElement) {
    if (e != null)
        return `width: ${e.clientWidth}px`
}

</script>

<template>
    <div id="UsersTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

        <div class="tab">

            <div class="tab-label chip-tab active not-absolute" ref="allUsersLabelEl">
                Alle Nutzer
                <div class="chip" v-if="props.allUserCount > 0">
                    {{ props.allUserCount }}
                </div>
            </div>


            <div class="active-tab"></div>
        </div>

        <div class="tab-filler-container">
            <div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
            <div class="inactive-tab">
                <div class="tab-border"></div>
            </div>
        </div>

    </div>
</template>

<style lang="less">
@import '~~/assets/tabs-bar.less';

.number-divider {
    padding: 0 4px;
}
</style>

