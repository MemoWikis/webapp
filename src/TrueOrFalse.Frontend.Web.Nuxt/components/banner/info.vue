<script lang="ts" setup>
import { Topic } from '../topic/topicStore'
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()
interface Props {
    documentation: Topic
}

const props = defineProps<Props>()

const cookie = ref('')
const skipAnimation = ref(false)
const showBanner = ref(false)

function loadInfoBanner() {
    if (cookie.value == 'notFirstTime')
        skipAnimation.value = true
    if (cookie.value != 'hide') {
        showBanner.value = true
        document.cookie = "memuchoInfoBanner=notFirstTime; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/"
    }
}

onBeforeMount(() => {
    cookie.value = document.cookie.match('(^|;)\\s*' + "memuchoInfoBanner" + '\\s*=\\s*([^;]+)')?.pop() || ''
})

onMounted(() => {
    loadInfoBanner()
})

function hideInfoBanner() {
    document.cookie = "memuchoInfoBanner=hide; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/"
    skipAnimation.value = false
    showBanner.value = false
}

const { $urlHelper } = useNuxtApp()

watch(showBanner, (val) => {
    userStore.showBanner = val
})
</script>

<template>
    <div id="MemuchoInfoBanner" :class="{ 'skip-animation': skipAnimation, 'show-banner': showBanner }"
        v-if="!userStore.isLoggedIn && props.documentation">
        <div id="InfoBannerContainer" class="container">
            <div id="BannerContainer" class="row">
                <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                    <font-awesome-icon :icon="['fas', 'xmark']" @click="hideInfoBanner()"
                        class="visible-xs close-banner mobile-close" />
                    <div class="row fullWidth">
                        <div class="col-xs-12">
                            <div class="sub-text">
                                Alles an einem Ort
                            </div>
                            <div class="main-text">Wiki und Lernwerkzeuge vereint!</div>
                        </div>
                    </div>
                </div>
                <div id="BannerRedirectBtn" class="col-xs-12 col-sm-5 memucho-info-partial">
                    <NuxtLink class="memo-button btn btn-primary"
                        :to="$urlHelper.getTopicUrl(props.documentation.name, props.documentation.id)">
                        Zur Dokumentation
                    </NuxtLink>
                    <font-awesome-icon :icon="['fas', 'xmark']" @click="hideInfoBanner()"
                        class="hidden-xs close-banner" />
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.close-banner {
    cursor: pointer;

    // &:hover {
    //     color: @memo-grey-darker;
    // }
}

#BannerRedirectBtn {

    .close-banner {
        margin-left: 34px;
    }
}
</style>