<script lang="ts" setup>
import { VueElement } from 'vue'

interface Props {
    allUserCount: number
}
const props = defineProps<Props>()
const { isMobile } = useDevice()
const { t } = useI18n()

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
                {{ t('usersOverview.tabs.allUsers') }}
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
