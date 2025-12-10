<script lang="ts" setup>
import { Page } from '../page/pageStore'
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()
const { t } = useI18n()

interface Props {
    documentation: Page
}

const props = defineProps<Props>()

const cookie = ref('')
const skipAnimation = ref(false)
const showBanner = ref(false)

function loadInfoBanner() {
    if (cookie.value === 'notFirstTime')
        skipAnimation.value = true
    if (cookie.value != 'hide') {
        showBanner.value = true
        document.cookie = "memoWikisInfoBanner=notFirstTime; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/"
    }
}

onBeforeMount(() => {
    cookie.value = document.cookie.match('(^|;)\\s*' + "memoWikisInfoBanner" + '\\s*=\\s*([^;]+)')?.pop() || ''
})

onMounted(() => {
    loadInfoBanner()
})

function hideInfoBanner() {
    document.cookie = "memoWikisInfoBanner=hide; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/"
    skipAnimation.value = false
    showBanner.value = false
}

const { $urlHelper } = useNuxtApp()

watch(showBanner, (val) => {
    userStore.showBanner = val
})
</script>

<template>
    <div id="MemoWikisInfoBanner" :class="{ 'skip-animation': skipAnimation, 'show-banner': showBanner }"
        v-if="props.documentation">
        <div id="InfoBannerContainer" class="container">
            <div id="BannerContainer" class="row">
                <div id="BannerText" class="col-xs-12 col-sm-7 memoWikis-info-partial">
                    <font-awesome-icon :icon="['fas', 'xmark']" @click="hideInfoBanner()"
                        class="visible-xs close-banner mobile-close" />
                    <div class="row fullWidth">
                        <div class="col-xs-12">
                            <div class="sub-text">
                                {{ t('banner.info.subText') }}
                            </div>
                            <div class="main-text">{{ t('banner.info.mainText') }}</div>
                        </div>
                    </div>
                </div>
                <div id="BannerRedirectBtn" class="col-xs-12 col-sm-5 memoWikis-info-partial">
                    <NuxtLink class="memo-button btn btn-primary"
                        :to="$urlHelper.getPageUrl(props.documentation.name, props.documentation.id)">
                        {{ t('banner.info.toDocumentation') }}
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

#MemoWikisInfoBanner {
    height: 0px;
    min-height: 0px;
    width: 100%;
    background: @memo-info;
    top: 0;
    z-index: 50;
    margin: 0;
    transform: translateY(-140px);
    transition: transform 0.5s ease-out, min-height 0.5s ease-out, opacity 0.5s ease-out, visibility 0.5s ease-out;
    opacity: 0;
    position: relative;
    pointer-events: none;
    visibility: hidden;

    &.skip-animation {
        transition: transform 0s ease-out, min-height 0s ease-out, opacity 0s ease-out, visibility 0s ease-out !important;
        height: unset !important;
    }

    .topBannerClass {
        padding-top: 45px;
    }

    .container {
        height: 100%;

        #BannerContainer {
            display: flex;
            align-items: center;
            height: 100%;
            flex-wrap: wrap;
            padding: 10px 0;

            .memoWikis-info-partial {
                display: flex;
                align-items: center;
                padding: 10px;
            }

            #BannerText {
                flex-direction: column;
                align-items: flex-start;
                color: @memo-blue;
                justify-content: center;

                .sub-text {
                    font-size: 14px;
                    font-weight: 600;
                    line-height: 20px;
                    letter-spacing: 1.25px;
                    width: 100%;
                    text-transform: uppercase;

                    .fa-heart {
                        @media (max-width: 767px) {
                            margin-top: 10px;
                        }

                        font-size: 20px;
                        color: @memo-wish-knowledge-red;
                    }

                    @media (max-width: 767px) {
                        text-align: center;
                    }
                }

                .main-text {
                    font-size: 25px;
                    font-weight: 400;
                    line-height: 34px;
                    letter-spacing: 0em;
                    width: 100%;

                    @media (max-width: 767px) {
                        text-align: center;
                    }
                }
            }

            #BannerRedirectBtn {
                justify-content: flex-end;

                @media (max-width: 767px) {
                    justify-content: center;
                }

                .close-banner {
                    margin-left: 34px;
                }
            }

            .close-banner {
                color: @memo-grey-dark;
                padding: 10px;
                font-size: 20px;
                transition: all 0.3s;
                cursor: pointer;

                &:hover {
                    color: @memo-blue;
                }

                &.mobile-close {
                    position: absolute;
                    right: 0px;
                    top: 0px;
                    padding-top: 5px;
                    z-index: 2;
                }
            }
        }
    }

    &.show-banner {
        opacity: 1;
        min-height: 96px;
        height: unset;
        transform: translateY(0px);
        pointer-events: unset;
        visibility: unset;

        .container {
            min-height: 96px;

            #BannerContainer {
                min-height: 95px;
            }
        }
    }
}
</style>