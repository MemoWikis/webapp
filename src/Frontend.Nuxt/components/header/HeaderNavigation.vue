<script lang="ts" setup>
import { type QuestionItem, SearchType, type PageItem, type UserItem } from '~~/components/search/searchHelper'
import { useUserStore } from '~/components/user/userStore'
import { useSideSheetStore } from '~/components/sideSheet/sideSheetStore'

const { t } = useI18n()

const userStore = useUserStore()
const sideSheetStore = useSideSheetStore()
const { sideSheetOpen } = useSideSheetState()

const windowWidth = ref(1024)
const isWideViewport = computed(() => windowWidth.value >= 769)

const showSearch = ref(false)
const showMobileMenu = ref(false)

const openUrl = async (val: PageItem | QuestionItem | UserItem) => {
    if (!isWideViewport.value) {
        showSearch.value = false
    }
    return await navigateTo(val.url)
}

const handleResize = () => {
    windowWidth.value = window.innerWidth
    if (windowWidth.value >= 768) {
        showMobileMenu.value = false
    }
}

onMounted(() => {
    windowWidth.value = window.innerWidth
    window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
    if (typeof window !== 'undefined') {
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

const toggleMobileMenu = () => {
    showMobileMenu.value = !showMobileMenu.value
    if (showMobileMenu.value) {
        showSearch.value = false
    }
}

const toggleSearch = () => {
    showSearch.value = !showSearch.value
    if (showSearch.value) {
        showMobileMenu.value = false
    }
}
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

                    <div class="nav-search" :class="{ 'search-expanded': showSearch || isWideViewport }">
                        <div class="search-toggle" @click="toggleSearch">
                            <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                            <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                        </div>
                        <div class="search-wrapper">
                            <Search :search-type="SearchType.all" :show-search="showSearch || isWideViewport"
                                placement="bottom-start" :distance="distance" @select-item="openUrl" />
                        </div>
                    </div>
                </div>

                <div class="nav-center">
                    <NuxtLink :to="`/${t('url.news')}`" class="nav-link">
                        <font-awesome-icon :icon="['fas', 'newspaper']" class="nav-link-icon" />
                        <span class="nav-link-label">{{ t('nav.news') }}</span>
                    </NuxtLink>
                    <NuxtLink :to="`/${t('url.topics')}`" class="nav-link">
                        <font-awesome-icon :icon="['fas', 'layer-group']" class="nav-link-icon" />
                        <span class="nav-link-label">{{ t('nav.topics') }}</span>
                    </NuxtLink>
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
                    <div class="mobile-menu-toggle" @click="toggleMobileMenu">
                        <font-awesome-icon v-if="showMobileMenu" icon="fa-solid fa-xmark" />
                        <font-awesome-icon v-else icon="fa-solid fa-ellipsis-vertical" />
                    </div>

                    <ClientOnly>
                        <HeaderUserDropdown v-if="userStore.isLoggedIn" />

                        <template v-else>
                            <button :class="{ 'login-modal-is-open': modalIsOpen }" class="nav-login-btn"
                                @click="userStore.openLoginModal()">
                                <font-awesome-icon icon="fa-solid fa-right-to-bracket" class="login-icon" />
                                <span class="login-label">{{ t('label.login') }}</span>
                            </button>
                            <NuxtLink :to="`/${t('url.register')}`" class="nav-register-btn">
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

        <Transition name="mobile-menu">
            <div v-if="showMobileMenu" class="mobile-menu-dropdown">
                <NuxtLink :to="`/${t('url.news')}`" class="mobile-menu-link" @click="showMobileMenu = false">
                    <font-awesome-icon :icon="['fas', 'newspaper']" class="mobile-menu-icon" />
                    {{ t('nav.news') }}
                </NuxtLink>
                <NuxtLink :to="`/${t('url.topics')}`" class="mobile-menu-link" @click="showMobileMenu = false">
                    <font-awesome-icon :icon="['fas', 'layer-group']" class="mobile-menu-icon" />
                    {{ t('nav.topics') }}
                </NuxtLink>
                <NuxtLink to="/wikis" class="mobile-menu-link" @click="showMobileMenu = false">
                    <font-awesome-icon :icon="['fas', 'book']" class="mobile-menu-icon" />
                    {{ t('nav.wikis') }}
                </NuxtLink>
                <NuxtLink :to="`/${t('url.users')}`" class="mobile-menu-link" @click="showMobileMenu = false">
                    <font-awesome-icon :icon="['fas', 'users']" class="mobile-menu-icon" />
                    {{ t('nav.community') }}
                </NuxtLink>
                <ClientOnly>
                    <template v-if="!userStore.isLoggedIn">
                        <div class="mobile-menu-divider" />
                        <button class="mobile-menu-link" @click="userStore.openLoginModal(); showMobileMenu = false">
                            <font-awesome-icon icon="fa-solid fa-right-to-bracket" class="mobile-menu-icon" />
                            {{ t('label.login') }}
                        </button>
                        <NuxtLink :to="`/${t('url.register')}`" class="mobile-menu-link"
                            @click="showMobileMenu = false">
                            <font-awesome-icon icon="fa-solid fa-user-plus" class="mobile-menu-icon" />
                            {{ t('label.register') }}
                        </NuxtLink>
                    </template>
                </ClientOnly>
            </div>
        </Transition>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#HeaderNavigation {
    height: 56px;
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

        @media (min-width: 901px) {
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

        &.sidesheet-open .nav-inner {
            padding-left: 420px;

            @media (max-width: 900px) {
                padding-left: 10px;
            }

            @media (min-width: 1980px) {
                padding-left: clamp(90px, calc(420px - (100vw - 1980px)), 420px);
            }
        }
    }

    .nav-inner {
        display: flex;
        align-items: center;
        width: 100%;
        max-width: 1600px;
        height: 100%;
        padding: 0 10px;
        gap: 12px;

        @media (min-width: 901px) {
            padding-left: 90px;
        }

        @media (max-width: 600px) {
            padding: 0 8px;
            gap: 8px;
        }
    }

    .nav-left {
        display: flex;
        align-items: center;
        gap: 12px;
        flex-shrink: 0;

        @media (max-width: 768px) {
            flex: 1;
            min-width: 0;
        }

        @media (max-width: 600px) {
            gap: 8px;
        }

        .nav-logo {
            display: flex;
            align-items: center;
            text-decoration: none;
            flex-shrink: 0;

            .logo-full {
                height: 22px;
                display: block;

                @media (max-width: 600px) {
                    display: none;
                }
            }

            .logo-small {
                height: 22px;
                display: none;

                @media (max-width: 600px) {
                    display: block;
                }
            }
        }

        .nav-search {
            display: flex;
            align-items: center;

            @media (max-width: 768px) {
                flex: 1;
                min-width: 0;
            }

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

                @media (min-width: 769px) {
                    display: none;
                }

                &:hover {
                    background-color: @memo-grey-lighter;
                }
            }

            .search-wrapper {
                width: 0;
                overflow: hidden;
                transition: width 0.2s ease;

                :deep(.searchInputContainer) {
                    min-width: 0;
                }
            }

            &.search-expanded .search-wrapper {
                width: 260px;

                @media (max-width: 1100px) {
                    width: 200px;
                }

                @media (max-width: 900px) {
                    width: 160px;
                }

                @media (max-width: 768px) {
                    flex: 1;
                    width: auto;
                    max-width: none;
                }
            }
        }
    }

    .nav-center {
        display: flex;
        align-items: center;
        gap: 8px;
        flex: 1;
        justify-content: center;

        @media (max-width: 768px) {
            display: none;
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

            @media (max-width: 1000px) {
                padding: 6px 10px;
            }

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
                @media (max-width: 960px) {
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

        .mobile-menu-toggle {
            display: none;
            align-items: center;
            justify-content: center;
            width: 32px;
            height: 32px;
            border-radius: 6px;
            cursor: pointer;
            color: @memo-grey-dark;
            font-size: 16px;

            @media (max-width: 768px) {
                display: flex;
            }

            &:hover {
                background-color: @memo-grey-lighter;
            }
        }

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

            @media (max-width: 768px) {
                display: none;
            }

            &:hover {
                background-color: @memo-grey-lighter;
            }

            .login-icon {
                font-size: 16px;
            }

            .login-label {
                @media (max-width: 900px) {
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

            @media (max-width: 768px) {
                display: none;
            }

            &:hover {
                filter: brightness(0.95);
            }
        }

        .nav-placeholder {
            width: 40px;
            height: 32px;
        }
    }

    .mobile-menu-dropdown {
        position: absolute;
        top: 56px;
        right: 0;
        background: white;
        border: 1px solid @memo-grey-light;
        border-radius: 0 0 8px 8px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        min-width: 200px;
        z-index: 99;
        padding: 4px 0;

        .mobile-menu-link {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 10px 16px;
            text-decoration: none;
            color: @memo-grey-darkest;
            font-size: 14px;
            font-weight: 500;
            border: none;
            background: none;
            width: 100%;
            cursor: pointer;
            text-align: left;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &.router-link-active {
                color: @memo-blue;
            }
        }

        .mobile-menu-icon {
            width: 16px;
            text-align: center;
            color: @memo-grey-dark;
        }

        .mobile-menu-divider {
            height: 1px;
            background-color: @memo-grey-light;
            margin: 4px 0;
        }
    }
}

.mobile-menu-enter-active,
.mobile-menu-leave-active {
    transition: opacity 0.15s ease, transform 0.15s ease;
}

.mobile-menu-enter-from,
.mobile-menu-leave-to {
    opacity: 0;
    transform: translateY(-8px);
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
