<script lang="ts" setup>
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { usePageStore } from '~/components/page/pageStore'
import { useUserStore } from '~~/components/user/userStore'

const { t } = useI18n()

const tabsStore = useTabsStore()
const pageStore = usePageStore()

const width = ref(0)
const editMode = ref(false)
const footerIsVisible = ref(false)
function footerCheck() {
    var pageContentElement = document.getElementById('PageContent')
    if (pageContentElement) {
        var contentWidth = pageContentElement.clientWidth
        var windowWidth = window.innerWidth
        const elFooter = document.getElementById('EditBarAnchor')

        if (elFooter) {
            var rect = elFooter.getBoundingClientRect()
            var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight)
            if (rect.top - viewHeight >= 0) {
                if (footerIsVisible.value && editMode.value) {
                    shrink.value = false
                    expand.value = true
                }
                footerIsVisible.value = false
                width.value = windowWidth
            } else {
                if (!footerIsVisible.value && editMode.value) {
                    expand.value = false
                    shrink.value = true
                }
                footerIsVisible.value = true
                width.value = contentWidth
            }
        }
    }
}

function handleSaveShortcut(e: KeyboardEvent) {
    if (e.ctrlKey && e.key === 's') {
        e.preventDefault()
        pageStore.saveContent()
        pageStore.saveName()
    }
}

watch(() => pageStore.contentHasChanged, (val) => {
    footerCheck()
    if (userStore.isLoggedIn) {
        if (val)
            document.addEventListener('keydown', handleSaveShortcut)
        else document.removeEventListener('keydown', handleSaveShortcut)
    }
})

watch(() => pageStore.nameHasChanged, (val) => {
    footerCheck()
    if (userStore.isLoggedIn) {
        if (val)
            document.addEventListener('keydown', handleSaveShortcut)
        else document.removeEventListener('keydown', handleSaveShortcut)
    }
})

watch(() => pageStore.id, () => {
    pageStore.contentHasChanged = false
    pageStore.nameHasChanged = false
})

const isExtended = ref(false)
function handleScroll() {
    if (window.scrollY === 0)
        isExtended.value = true
    else isExtended.value = false

    if (tabsStore.activeTab === Tab.Text)
        footerCheck()
}

onMounted(async () => {
    window.addEventListener('scroll', handleScroll)
    window.addEventListener('resize', footerCheck)
    await nextTick()
    footerCheck()
})

const shrink = ref(false)
const expand = ref(false)
const showSaveMsg = ref(false)
const saveMsg = ref('')

const userStore = useUserStore()
const { isMobile } = useDevice()
</script>

<template>
    <div id="EditBar" class="col-xs-12"
        :class="{ 'is-shown': (pageStore.contentHasChanged || pageStore.nameHasChanged) && tabsStore.activeTab === Tab.Text }">
        <div class="fab-container">
            <template v-if="tabsStore.activeTab === Tab.Text">
                <div class="edit-mode-bar-container" v-if="pageStore.contentHasChanged || pageStore.nameHasChanged">
                    <div class="toolbar"
                        :class="{ 'stuck': footerIsVisible, 'is-hidden': (!pageStore.contentHasChanged && !pageStore.nameHasChanged), 'shrink': shrink, 'expand': expand, 'not-logged-in': !userStore.isLoggedIn }">
                        <div class="toolbar-btn-container">
                            <div class="centerText mobile" v-if="isMobile && !userStore.isLoggedIn">
                                <div @click="userStore.openLoginModal()">
                                    {{ t('page.editBar.loginRequired.mobile') }}
                                </div>
                            </div>
                            <template v-else>
                                <div class="btn-left">
                                </div>
                                <div class="centerText" v-show="!userStore.isLoggedIn">
                                    <div>
                                        <i18n-t keypath="page.editBar.loginRequired.desktop" tag="span">
                                            <template #login>
                                                <span @click="userStore.openLoginModal()">{{ t('page.editBar.loginLink') }}</span>
                                            </template>
                                        </i18n-t>
                                    </div>
                                </div>
                            </template>


                            <div v-if="showSaveMsg" class="saveMsg">
                                <div>
                                    {{ saveMsg }}
                                </div>
                            </div>

                            <template v-else-if="userStore.isLoggedIn">
                                <div class="btn-right" v-show="pageStore.contentHasChanged || pageStore.nameHasChanged">
                                    <div class="button" @click.prevent="pageStore.saveContent(); pageStore.saveName()" :class="{ expanded: editMode }">
                                        <div class="icon">
                                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                                        </div>
                                        <div class="btn-label">
                                            {{ t('page.editBar.buttons.publish') }}
                                        </div>
                                    </div>

                                    <div class="button" @click.prevent="pageStore.reset()" :class="{ expanded: editMode }">
                                        <div class="icon">
                                            <font-awesome-icon icon="fa-solid fa-xmark" />
                                        </div>
                                        <div class="btn-label">
                                            {{ t('page.editBar.buttons.discard') }}
                                        </div>
                                    </div>
                                </div>
                            </template>
                            <div class="btn-right" v-else-if="!isMobile"></div>
                        </div>
                    </div>
                </div>
            </template>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#EditBar {

    @scale00: scale3d(0, 0, 1);
    @scale11: scale3d(1.1, 1.1, 1);
    @scale10: scale3d(1, 1, 1);

    position: sticky;
    bottom: 0;
    z-index: 16;
    height: 56px;
    pointer-events: none;

    &.is-shown {
        pointer-events: all;
    }

    .fab-container {
        display: flex;
        flex-direction: row-reverse;

        .main-fab-container {
            display: flex;
            flex-direction: column-reverse;
            cursor: pointer;
            z-index: 999;

            @media(min-width: 1200px) {
                margin-right: -28px;
            }

            @keyframes move-up {
                0% {
                    transform: translateY(50%) @scale00;
                    display: unset;
                }

                100% {
                    transform: translateY(0%) @scale10;
                    opacity: 1;
                }
            }

            @keyframes move-down {
                50% {
                    opacity: initial;
                    transform: translateY(0%) @scale10;
                }

                100% {
                    transform: translateY(50%) @scale00;
                    opacity: 0;
                    display: none;
                }
            }

            @keyframes move-right {
                0% {
                    position: unset;
                    transform: translateX(-200px) @scale00;
                    display: unset;
                }

                20% {
                    transform: translateX(-200px) @scale00;
                }

                100% {
                    transform: translateX(0px) @scale10;
                    opacity: 1;
                }
            }

            @keyframes move-left {
                50% {
                    opacity: initial;
                    transform: translateX(0px) @scale10;
                    position: absolute;
                    right: -48px;
                }

                100% {
                    transform: translateX(-200px) @scale00;
                    opacity: 0;
                    display: none;
                    position: unset;
                }
            }
        }
    }

    @keyframes pop-out-slow {
        0% {
            display: flex !important;
            opacity: 0;
        }

        40% {
            opacity: 0;
            transform: @scale00;
        }

        55% {
            transform: @scale11;
        }

        60% {
            transform: @scale10;
            opacity: 1;
        }

        100% {
            display: flex !important;
        }
    }

    @keyframes pop-out-normal {
        0% {
            display: flex !important;
            opacity: 0;
        }

        30% {
            opacity: 0;
            transform: @scale00;
        }

        45% {
            transform: @scale11;
        }

        50% {
            transform: @scale10;
            opacity: 1;
        }

        100% {
            display: flex !important;
        }
    }

    @keyframes pop-out-fast {
        0% {
            display: flex !important;
            opacity: 0;
        }

        20% {
            opacity: 0;
            transform: @scale00;
        }

        35% {
            transform: @scale11;
        }

        40% {
            transform: @scale10;
            opacity: 1;
        }

        100% {
            display: flex !important;
        }
    }

    @keyframes pop-in-slow {
        0% {}

        20% {
            opacity: 1;
            transform: @scale10;
        }

        40% {
            transform: @scale00;
            opacity: 0;
        }

        100% {
            display: none;
            opacity: 0;
        }
    }

    @keyframes pop-in-normal {
        0% {}

        10% {
            opacity: 1;
            transform: @scale10;
        }

        30% {
            transform: @scale00;
            opacity: 0;
        }

        100% {
            display: none;
            opacity: 0;
        }
    }

    @keyframes pop-in-fast {
        0% {
            opacity: 1;
            transform: @scale10;
        }

        20% {
            transform: @scale00;
            opacity: 0;
        }

        100% {
            display: none;
            opacity: 0;
        }
    }

    .edit-mode-bar-container {
        width: 100%;

        .toolbar {
            height: 56px;
            overflow: hidden;
            background-color: @memo-green;
            bottom: 0;
            left: 0;
            display: flex;
            justify-content: center;
            animation: pop-out-slow .8s forwards;
            transform-origin: 85% 25%;
            box-shadow: 0 -2px 6px rgba(0, 0, 0, .16);
            transition: box-shadow .3s;
            min-width: 1px;
            width: 100%;
            position: fixed;

            &.not-logged-in {
                background-color: @memo-yellow;
                // color: white;
            }

            @media (min-width: 1200px) {
                transform-origin: 65% 25%;
            }

            &.stuck {
                position: relative;
                border-radius: 4px;
                transform-origin: right;

                &:hover {
                    box-shadow: 0 0 3px rgba(0, 0, 0, 0.12), 0 3px 3px rgba(0, 0, 0, 0.24);
                }
            }

            &.shrink {
                &.pseudo-sticky {
                    @keyframes move-to-right {
                        0% {
                            left: -25%;
                        }

                        100% {
                            left: 0;
                        }
                    }

                    animation: move-to-right .1s forwards;
                    transition: width .1s;
                }
            }

            &.expand {
                transition: width .1s;

                @keyframes expand {
                    0% {
                        left: 25%
                    }

                    100% {
                        left: 0;
                    }
                }

                animation: expand .1s forwards;
            }

            &.is-hidden {
                animation: pop-in-fast .8s forwards
            }

            .toolbar-btn-container {
                display: flex;
                justify-content: space-between;
                flex-direction: row;
                width: 100%;
                max-width: 1140px;

                .centerText,
                .saveMsg {
                    display: inline-flex;
                    justify-content: center;
                    align-items: center;
                    font-size: 14px;
                    text-transform: uppercase;
                    font-weight: 600;

                    &.mobile {
                        width: 100%;
                        padding: 0 40px;
                        text-align: center;
                        font-size: 12px;
                    }
                }

                .saveMsg {
                    padding-right: 20px;
                    animation-name: opacity;
                    animation-duration: 3s;
                }

                @keyframes opacity {
                    0% {
                        opacity: 1;
                    }

                    50% {
                        opacity: 1;
                    }

                    100% {
                        opacity: 0;
                    }
                }

                .btn-left,
                .btn-right {
                    height: 56px;
                    display: flex;
                    flex-direction: row-reverse;

                    .button {
                        margin: auto;
                        padding: 0 20px;
                        display: flex;
                        align-content: center;
                        align-items: center;
                        flex-direction: column;
                        cursor: pointer;

                        .icon {
                            font-size: 25px;
                            height: 36px;
                        }

                        .btn-label {
                            font-size: 12px;
                            line-height: 12px;
                        }
                    }
                }

                .btn-left {
                    width: 100%;
                    max-width: 200px;
                }

                .btn-right {
                    width: 200px;
                }
            }
        }
    }
}
</style>
