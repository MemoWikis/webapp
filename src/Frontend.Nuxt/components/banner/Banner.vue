<script lang="ts" setup>

const { t } = useI18n()

interface Props {
    cookieName?: string
    mainText?: string
    subText?: string
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
        document.cookie = `${props.cookieName}=notFirstTime; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/`
    }
}

onBeforeMount(() => {
    cookie.value = document.cookie.match('(^|;)\\s*' + props.cookieName + '\\s*=\\s*([^;]+)')?.pop() || ''
})

onMounted(() => {
    loadInfoBanner()
})

function hideInfoBanner() {
    document.cookie = `${props.cookieName}=hide; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/`
    skipAnimation.value = false
    showBanner.value = false
}

const slots = useSlots()

</script>

<template>
    <div class="banner" :class="{ 'skip-animation': skipAnimation, 'show-banner': showBanner }" v-if="showBanner">
        <div class="banner-container container">
            <div class="banner-row row">
                <div class="banner-text col-xs-12 col-sm-7 memoWikis-info-partial">
                    <font-awesome-icon :icon="['fas', 'xmark']" @click="hideInfoBanner()"
                        class="visible-xs close-banner mobile-close" />
                    <div class="row fullWidth">
                        <div class="col-xs-12">
                            <div class="sub-text" v-if="slots.subText">
                                <slot name="subText"></slot>
                            </div>

                            <div class="main-text">
                                <slot name="mainText"></slot>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="banner-action col-xs-12 col-sm-5 memoWikis-info-partial">
                    <slot name="action">
                    </slot>
                    <font-awesome-icon :icon="['fas', 'xmark']" @click="hideInfoBanner()"
                        class="hidden-xs close-banner" />
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.banner {
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

        .banner-row {
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

            .banner-text {
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
                        color: @memo-wuwi-red;
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

            .banner-action {
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

            .banner-row {
                min-height: 95px;
            }
        }
    }

    &.sidesheet-open {

        @media (max-width: 1500px) {
            .banner-container {
                width: calc(100vw - 40px);
            }

            .banner-row {
                padding-left: 420px;
                margin-right: 10px;
                width: 100%;
            }

            .page {
                &.col-lg-9 {
                    width: 100%;
                }
            }
        }

        @media (min-width: 1501px) and (max-width: 1980px) {

            .banner-row {
                padding-left: clamp(260px, 20vw, 0px);
                margin-right: 10px;
                width: 100%;
            }

        }
    }

}
</style>
