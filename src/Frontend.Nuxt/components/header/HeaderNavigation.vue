<script lang="ts" setup>
import { type QuestionItem, SearchType, type PageItem, type UserItem } from '~~/components/search/searchHelper'
import { useUserStore } from '~/components/user/userStore'
import { useSideSheetStore } from '~/components/sideSheet/sideSheetStore'

const { t } = useI18n()

const userStore = useUserStore()
const sideSheetStore = useSideSheetStore()
const { sideSheetOpen } = useSideSheetState()
const { isDesktopOrTablet, isMobile } = useDevice()

const showSearch = ref(false)

const openUrl = async (val: PageItem | QuestionItem | UserItem) => {
    if (isMobile || window?.innerWidth < 480) {
        showSearch.value = false
    }
    return await navigateTo(val.url)
}

const handleResize = () => {
    if (showSearch.value) {
        return
    }
    if (window.innerWidth < 769) {
        showSearch.value = false
    }
}

onMounted(() => {
    if (isMobile || window?.innerWidth < 769) {
        showSearch.value = false
    }
    if (typeof window !== "undefined") {
        window.addEventListener('resize', handleResize)
    }
})

onUnmounted(() => {
    if (typeof window !== "undefined") {
        window.removeEventListener('resize', handleResize)
    }
})

const { $vfm } = useNuxtApp()
const { openedModals } = $vfm
const modalIsOpen = ref(false)

watch(openedModals, (val) => {
    if (val.length > 0) {
        modalIsOpen.value = true
    } else {
        modalIsOpen.value = false
    }
}, { deep: true, immediate: true })

const distance = computed(() => {
    return userStore.isLoggedIn ? 24 : 6
})
</script>

<template>
    <div id="HeaderNavigation">
        <div class="sidesheet-button" @click="sideSheetStore.showSideSheet = !sideSheetStore.showSideSheet">
            <font-awesome-layers>
                <font-awesome-icon :icon="['fas', 'bars']" />
                <ClientOnly>
                    <font-awesome-icon v-if="sideSheetStore.showSideSheet" :icon="['fas', 'caret-left']"
                        transform="right-2" class="angle-bg" />
                    <font-awesome-icon v-if="sideSheetStore.showSideSheet" :icon="['fas', 'angle-left']"
                        transform="right-5" class="animate-grow" />
                </ClientOnly>
            </font-awesome-layers>
        </div>

        <div class="nav-bar" :class="{ 'sidesheet-open': sideSheetOpen }">
            <div class="nav-inner">
                <div class="nav-left">
                    <NuxtLink to="/" class="nav-logo" aria-label="memoWikis home">
                        <Image src="/Images/Logo/Logo.svg" class="logo-full" alt="memoWikis logo" />
                        <Image src="/Images/Logo/LogoSmall.svg" class="logo-small" alt="memoWikis" />
                    </NuxtLink>

                    <div class="nav-search" :class="{ 'search-expanded': showSearch }">
                        <div class="search-toggle" @click="showSearch = !showSearch">
                            <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                            <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                        </div>
                        <div class="search-wrapper">
                            <Search :search-type="SearchType.all" :show-search="showSearch" placement="bottom-start"
                                :distance="distance" @select-item="openUrl" />
                        </div>
                    </div>
                </div>

                <div class="nav-center" :class="{ 'hidden-when-search': showSearch }">
                    <NuxtLink to="/wikis" class="nav-link">
                        <font-awesome-icon :icon="['fas', 'book']" class="nav-link-icon" />
                        <span class="nav-link-label">{{ t('nav.wikis') }}</span>
                    </NuxtLink>
                    <NuxtLink :to="`/${t('url.users')}`" class="nav-link">
                        <font-awesome-icon :icon="['fas', 'users']" class="nav-link-icon" />
                        <span class="nav-link-label">{{ t('nav.community') }}</span>
                    </NuxtLink>
                </div>

                <div class="nav-right">
                    <ClientOnly>
                        <HeaderUserDropdown v-if="userStore.isLoggedIn" />

                        <template v-else>
                            <button :class="{ 'login-modal-is-open': modalIsOpen }" class="nav-login-btn"
                                @click="userStore.openLoginModal()">
                                <font-awesome-icon icon="fa-solid fa-right-to-bracket" class="login-icon" />
                                <span class="login-label">{{ t('label.login') }}</span>
                            </button>
                            <NuxtLink v-if="isDesktopOrTablet" :to="`/${t('url.register')}`" class="nav-register-btn">
                                {{ t('label.register') }}
                            </NuxtLink>
                        </template>

                        <template #fallback>
                            <div class="nav-placeholder" />
                        </template>
                    </ClientOnly>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#HeaderNavigation {
    height: 50px;
    background-color: white;
    border-bottom: 1px solid @memo-grey-light;
    position: sticky;
    top: 0;
    z-index: 100;
    display: flex;
    align-items: center;
    width: 100%;

    .sidesheet-button {
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        width: 48px;
        min-width: 48px;
        user-select: none;
        border-right: 1px solid @memo-grey-light;

        @media (min-width: 900px) {
            position: absolute;
            left: 0;
            z-index: 2000;
            width: 80px;
            min-width: 80px;
            border-right: none;
        }

        .angle-bg {
            color: white;
            font-size: 24px;
        }

        &:hover {
            background-color: @memo-grey-lighter;
        }
    }

    .nav-bar {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 100%;

        @media (min-width: 900px) {
            padding-left: 80px;
        }

        &.sidesheet-open {
            padding-left: 410px;

            @media (max-width: 900px) {
                padding-left: 0;
            }

            @media (min-width: 1980px) {
                padding-left: clamp(80px, calc(410px - (100vw - 1980px)), 410px);
            }
        }
    }

    .nav-inner {
        display: flex;
        align-items: center;
        width: 100%;
        max-width: 1600px;
        height: 100%;
        padding: 0 16px;
        gap: 8px;
    }

    .nav-left {
        display: flex;
        align-items: center;
        gap: 12px;
        flex-shrink: 0;

        .nav-logo {
            display: flex;
            align-items: center;
            text-decoration: none;
            flex-shrink: 0;

            .logo-full {
                height: 28px;
                display: block;

                @media (max-width: 600px) {
                    display: none;
                }
            }

            .logo-small {
                height: 32px;
                display: none;

                @media (max-width: 600px) {
                    display: block;
                }
            }
        }

        .nav-search {
            display: flex;
            align-items: center;

            .search-toggle {
                display: flex;
                align-items: center;
                justify-content: center;
                width: 32px;
                height: 32px;
                border-radius: 6px;
                cursor: pointer;
                color: @memo-grey-dark;
                font-size: 16px;

                &:hover {
                    background-color: @memo-grey-lighter;
                }
            }

            .search-wrapper {
                width: 0;
                overflow: hidden;
                transition: width 0.2s ease;
            }

            &.search-expanded .search-wrapper {
                width: 240px;

                @media (max-width: 900px) {
                    width: 180px;
                }

                @media (max-width: 600px) {
                    width: 140px;
                }
            }
        }
    }

    .nav-center {
        display: flex;
        align-items: center;
        gap: 4px;
        flex: 1;
        justify-content: center;

        &.hidden-when-search {
            @media (max-width: 768px) {
                display: none;
            }
        }

        .nav-link {
            display: flex;
            align-items: center;
            gap: 6px;
            padding: 6px 14px;
            border-radius: 6px;
            text-decoration: none;
            color: @memo-grey-darkest;
            font-size: 14px;
            font-weight: 500;
            white-space: nowrap;
            transition: background-color 0.15s;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &.router-link-active {
                color: @memo-blue;
                background-color: fade(@memo-blue, 8%);
            }

            .nav-link-icon {
                font-size: 14px;
            }

            .nav-link-label {
                @media (max-width: 500px) {
                    display: none;
                }
            }
        }
    }

    .nav-right {
        display: flex;
        align-items: center;
        gap: 8px;
        flex-shrink: 0;

        .nav-login-btn {
            display: flex;
            align-items: center;
            gap: 6px;
            padding: 6px 12px;
            border: none;
            background: none;
            cursor: pointer;
            font-size: 14px;
            color: @memo-grey-darkest;
            border-radius: 6px;
            white-space: nowrap;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            .login-icon {
                font-size: 16px;
            }

            .login-label {
                @media (max-width: 500px) {
                    display: none;
                }
            }
        }

        .nav-register-btn {
            display: flex;
            align-items: center;
            padding: 6px 16px;
            background-color: @memo-green;
            color: white;
            text-decoration: none;
            border-radius: 20px;
            font-size: 14px;
            font-weight: 500;
            white-space: nowrap;
            transition: filter 0.15s;

            &:hover {
                filter: brightness(0.95);
            }
        }

        .nav-placeholder {
            width: 40px;
            height: 32px;
        }
    }
}

.animate-grow {
    animation: grow 0.15s ease-in-out;
}

@keyframes grow {
    0% {
        transform: scale(0);
        opacity: 0;
    }

    100% {
        transform: scale(1);
        opacity: 1;
    }
}
</style>
