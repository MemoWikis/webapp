<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { ImageFormat } from '~~/components/image/imageFormatEnum.js'
import { useUserStore } from '~~/components/user/userStore'
import { SiteType } from '~/components/shared/siteEnum'
import { ErrorCode } from '~/components/error/errorCodeEnum'
import { LayoutCardSize } from '~/composables/layoutCardSize'
import { PageData } from '~/composables/missionControl/pageData'
import { color } from '~/constants/colors'
import UserSection from '~/constants/userSections'
import { Question } from '~~/components/layout/LayoutQuestionList.vue'
import { useSnackbarStore, SnackbarData } from '~/components/snackBar/snackBarStore'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const userStore = useUserStore()
const { $logger, $urlHelper } = useNuxtApp()
const { t } = useI18n()
const { removeSkill } = useUserSkills()
const snackbarStore = useSnackbarStore()

interface Overview {
    activityPoints: {
        total: number
        questionsInOtherWishknowledges: number
        questionsCreated: number
        publicWishknowledges: number
    }
    publicQuestionsCount: number
    privateQuestionsCount: number
    publicPagesCount: number
    privatePagesCount: number
    wuwiCount: number
    publicWikisCount: number
    reputation: number
    rank: number
}

interface User {
    id: number
    name: string
    wikiName: string
    wikiId?: number
    imageUrl: string
    reputationPoints: number
    rank: number
    showWuwi: boolean
    aboutMeText?: string
}

interface ProfileData {
    user: User
    overview: Overview
    isCurrentUser: boolean
    messageKey?: string
    errorCode?: ErrorCode
    wikis?: PageData[]
    pages?: PageData[]
    skills?: PageData[]
    questions?: Question[]
}

const { data: profile, refresh: refreshProfile } = await useFetch<ProfileData>(`/apiVue/User/Get/${route.params.id ? route.params.id : userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

if (profile.value && profile.value.messageKey && profile.value?.messageKey != "") {

    $logger.warn(`User: ${profile.value.messageKey} route ${route.fullPath}`)
    throw createError({ statusCode: profile.value.errorCode, statusMessage: t(profile.value.messageKey) })
}

const router = useRouter()

const emit = defineEmits(['setBreadcrumb', 'setPage'])

function handleBreadcrumb() {

    if (profile.value?.user.id && profile.value.user.id > 0) {
        const newPath = $urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)
        router.push({ path: newPath })

        const breadcrumbItems: BreadcrumbItem[] = [
            {
                name: t('url.users'),
                url: `/${t('url.users')}`
            },
            {
                name: `${profile.value.user.name}`,
                url: $urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)
            }]
        emit('setBreadcrumb', breadcrumbItems)
    } else {
        emit('setBreadcrumb', [{ name: 'Fehler', url: '' }])
    }
}

onMounted(() => {
    emit('setPage', SiteType.User)
    handleBreadcrumb()
})

watch(() => userStore.isLoggedIn, () => {
    refreshProfile()
})

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: profile.value ? `${config.public.officialBase}${$urlHelper.getUserUrl(profile.value.user.name, profile.value?.user.id)}` : '',
        },
    ],
    meta: [
        {
            property: 'og:title',
            content: profile.value ? $urlHelper.sanitizeUri(profile.value.user.name) : ''
        },
    ]
}))

const { isMobile } = useDevice()

const hasWikis = computed(() => {
    return profile.value?.wikis && profile.value.wikis.length > 0
})

const hasSkills = computed(() => {
    return profile.value?.skills && profile.value.skills.length > 0 && profile.value.skills.some(skill => skill.isPublic && skill.knowledgebarData?.total > 0)
})

const hasPages = computed(() => {
    return profile.value?.pages && profile.value.pages.length > 0
})

const hasQuestions = computed(() => {
    return (profile.value?.questions && profile.value.questions.length > 0) || (profile.value?.overview.publicQuestionsCount ? profile.value.overview.publicQuestionsCount > 0 : false)
})

const showSkills = computed(() => {
    return profile.value?.user.id === userStore.id || hasSkills.value
})

const showAddSkillModal = ref(false)

function handleAddSkillClick() {
    showAddSkillModal.value = true
}

async function onSkillAdded() {
    await refreshProfile()
}

async function onRemoveSkill(skill: PageData) {
    const result = await removeSkill(skill.id)

    if (result.success) {
        const data: SnackbarData = {
            type: 'success',
            text: { message: t('success.skill.removed', { name: skill.name }) },
        }
        snackbarStore.showSnackbar(data)
    } else {
        const data: SnackbarData = {
            type: 'error',
            text: { message: t(result.errorMessageKey) },
        }
        snackbarStore.showSnackbar(data)
    }
    await refreshProfile()
}

const profileHeader = ref()
const profileHeaderHeight = ref(123) // Default height

const sideBarMarginTop = computed(() => {
    const defaultHeaderHeight = 123
    if (profileHeaderHeight.value > defaultHeaderHeight) {
        return profileHeaderHeight.value - defaultHeaderHeight + 25
    }
    return 25 // Default margin top if no profile header is available
})

// Watch for profile header height changes
onMounted(() => {
    if (profileHeader.value) {
        // Initial height measurement
        profileHeaderHeight.value = profileHeader.value.offsetHeight

        // Set up ResizeObserver to watch for height changes
        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                profileHeaderHeight.value = entry.contentRect.height
            }
        })

        resizeObserver.observe(profileHeader.value)

        // Cleanup on unmount
        onUnmounted(() => {
            resizeObserver.disconnect()
        })
    }
})
const ariaId = useId()

const showSkillCard = (skill: PageData) => {
    if (profile.value?.isCurrentUser && !userStore.showAsVisitor) {
        return true
    }
    else if (skill.knowledgebarData && skill.knowledgebarData.total > 0 && skill.isPublic) {
        return true
    }
    return false
}

</script>

<template>
    <div v-if="profile && profile.user.id > 0" class="user-container">
        <div class="user-content">
            <div id="UserHeader">
                <div class="profile-header" ref="profileHeader" :class="{ 'is-mobile': isMobile }">
                    <Image :format="ImageFormat.Author" :src="profile.user.imageUrl" class="profile-picture-small" />
                    <div class="profile-header-info">

                        <div v-if="profile.isCurrentUser" class="user-profile-header-container">

                            <VDropdown :aria-id="ariaId" :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
                                <div class="user-header-options-btn">
                                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                                </div>
                                <template #popper="{ hide }">
                                    <div @click="userStore.toggleShowAsVisitor()" class="dropdown-row">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-toggle-on" class="active toggle-icon" v-if="userStore.showAsVisitor" />
                                            <font-awesome-icon icon="fa-solid fa-toggle-off" class="inactive toggle-icon" v-else />
                                        </div>
                                        <div class="dropdown-label">
                                            {{ t('user.profile.showAsVisitor') }}
                                        </div>
                                    </div>
                                </template>
                            </VDropdown>
                        </div>

                        <h1>{{ profile.user.name }} <span class="profile-id">#{{ profile.user.id }}</span></h1>
                        <UserAboutMeSection :user-id="profile.user.id" :about-me="profile.user.aboutMeText" />
                    </div>
                </div>
            </div>

            <div class="panel-divider"></div>

            <LayoutPanel :id="UserSection.STATS_SECTION.id" :title="t(UserSection.STATS_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.reputation" :label="t('user.profile.stats.reputation')" v-tooltip="t('user.profile.stats.tooltips.reputation')" :icon="['fas', 'star']" :icon-color="color.memoYellow" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.rank" :label="t('user.profile.stats.rank')" v-tooltip="t('user.profile.stats.tooltips.rank')" :icon="['fas', 'crown']" :icon-color="color.memoYellow"
                        :url-value="`/${t('url.users')}`" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.publicPagesCount" :label="t('user.profile.stats.createdPages')" v-tooltip="t('user.profile.stats.tooltips.createdPages')" :icon="['fas', 'file-lines']" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.publicWikisCount" :label="t('user.profile.stats.createdWikis')" v-tooltip="t('user.profile.stats.tooltips.createdWikis')" :icon="['fas', 'file-lines']" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.publicQuestionsCount" :label="t('user.profile.stats.createdQuestions')" v-tooltip="t('user.profile.stats.tooltips.createdQuestions')" :icon="['fas', 'circle-question']" />
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="hasSkills || (profile.isCurrentUser && !userStore.showAsVisitor)" :id="UserSection.SKILLS_SECTION.id" :title="t(UserSection.SKILLS_SECTION.translationKey)"
                :labelTooltip="UserSection.SKILLS_SECTION.tooltipKey ? t(UserSection.SKILLS_SECTION.tooltipKey) : ''">
                <template v-for="skill in profile.skills">
                    <UserSkillCard :skill="skill" :can-edit="profile.isCurrentUser && !userStore.showAsVisitor" v-if="showSkillCard(skill)" @remove-skill="onRemoveSkill" />
                </template>

                <LayoutCard v-if="profile.isCurrentUser && !userStore.showAsVisitor" :size="LayoutCardSize.Small" @click="handleAddSkillClick" class="add-skill-card">
                    <div class="add-skill-content">
                        <font-awesome-icon :icon="['fas', 'plus']" class="add-icon" />
                        <span>{{ t('user.skills.addSkill') }}</span>
                    </div>
                </LayoutCard>

            </LayoutPanel>

            <LayoutPanel v-if="hasWikis || (profile.isCurrentUser && !userStore.showAsVisitor)" :id="UserSection.WIKIS_SECTION.id" :title="t(UserSection.WIKIS_SECTION.translationKey)"
                :labelTooltip="UserSection.WIKIS_SECTION.tooltipKey ? t(UserSection.WIKIS_SECTION.tooltipKey) : ''">

                <MissionControlGrid v-if="isMobile" :pages="profile.wikis!" :no-pages-text="t('missionControl.pageTable.noWikis')" />

                <LayoutCard v-else :no-padding="true">
                    <MissionControlTable :pages="profile.wikis!" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                </LayoutCard>

            </LayoutPanel>

            <LayoutPanel v-if="hasPages || (profile.isCurrentUser && !userStore.showAsVisitor)" :id="UserSection.PAGES_SECTION.id" :title="t(UserSection.PAGES_SECTION.translationKey)"
                :labelTooltip="UserSection.PAGES_SECTION.tooltipKey ? t(UserSection.PAGES_SECTION.tooltipKey) : ''">

                <MissionControlGrid v-if="isMobile" :pages="profile.pages!" :no-pages-text="t('missionControl.pageTable.noPages')" />

                <LayoutCard v-else :no-padding="true">
                    <MissionControlTable :pages="profile.pages!" :no-pages-text="t('missionControl.pageTable.noPages')" />
                </LayoutCard>

            </LayoutPanel>

            <LayoutPanel v-if="hasQuestions || (profile.isCurrentUser && !userStore.showAsVisitor)" :id="UserSection.QUESTIONS_SECTION.id" :title="t(UserSection.QUESTIONS_SECTION.translationKey)"
                :labelTooltip="UserSection.QUESTIONS_SECTION.tooltipKey ? t(UserSection.QUESTIONS_SECTION.tooltipKey) : ''">
                <LayoutCard :no-padding="true">
                    <LayoutQuestionList :questions="profile.questions || []" :no-questions-text="t('user.profile.noQuestions')" />
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="!hasSkills && (!profile.isCurrentUser || userStore.showAsVisitor)" :id="UserSection.SKILLS_PLACEHOLDER_SECTION.id" :title="t(UserSection.SKILLS_PLACEHOLDER_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Large" class="placeholder-card">
                    {{ t('user.profile.noSkills', { name: profile.user.name }) }}
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="!hasWikis && (!profile.isCurrentUser || userStore.showAsVisitor)" :id="UserSection.WIKIS_PLACEHOLDER_SECTION.id" :title="t(UserSection.WIKIS_PLACEHOLDER_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Large" class="placeholder-card">
                    {{ t('user.profile.noWikis', { name: profile.user.name }) }}
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="!hasPages && (!profile.isCurrentUser || userStore.showAsVisitor)" :id="UserSection.PAGES_PLACEHOLDER_SECTION.id" :title="t(UserSection.PAGES_PLACEHOLDER_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Large" class="placeholder-card">
                    {{ t('user.profile.noPages', { name: profile.user.name }) }}
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="!hasQuestions && (!profile.isCurrentUser || userStore.showAsVisitor)" :id="UserSection.QUESTIONS_PLACEHOLDER_SECTION.id" :title="t(UserSection.QUESTIONS_PLACEHOLDER_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Large" class="placeholder-card">
                    {{ t('user.profile.noQuestions', { name: profile.user.name }) }}
                </LayoutCard>
            </LayoutPanel>
        </div>

        <SidebarUser :user="profile.user" :show-wikis="hasWikis || (profile.isCurrentUser && !userStore.showAsVisitor)" :show-pages="hasPages || (profile.isCurrentUser && !userStore.showAsVisitor)"
            :show-questions="hasQuestions || (profile.isCurrentUser && !userStore.showAsVisitor)" :show-skills="hasSkills || (profile.isCurrentUser && !userStore.showAsVisitor)" :margin-top="sideBarMarginTop" />

        <!-- Add Skill Modal -->
        <UserSkillsAddSkillModal v-if="profile?.user.id" :show="showAddSkillModal" :user-id="profile.user.id" @close="showAddSkillModal = false" @skill-added="onSkillAdded" />
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.user-container {
    display: flex;
    flex-wrap: nowrap;
    gap: 0 1rem;
    width: 100%;

    .user-content {
        max-width: 1200px;
        width: calc(75% - 1rem);
        flex-grow: 2;

        .user-header {
            padding-top: 40px;
        }
    }

    @media (max-width: 900px) {
        .user-content {
            width: 100%;
        }
    }
}

#UserHeader {
    display: flex;
    margin: 25px 0;
    // height: 200px;

    .profile-header-info {
        display: flex;
        flex-direction: column;
        margin-top: 1rem;
        width: 100%;
        position: relative;

        h1 {
            margin-top: -10px;

            .profile-id {
                color: @memo-grey;
            }
        }
    }

    .profile-header {
        display: flex;
        flex-direction: row;
        flex-grow: 1;
        width: 100%;

        &.is-mobile {
            padding: 0 2rem;
        }

        .profile-picture-small {
            display: flex;
            align-items: flex-start;
            margin-right: 30px;

            :deep(img) {
                width: 96px;
                height: 96px;
                min-width: 96px;
            }
        }
    }
}

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
}

.overline-s {
    margin-bottom: 10px;
}

.sub-counter-container {
    display: flex;
    flex-wrap: nowrap;
    margin: 0 20px;
    margin-bottom: 10px;

    .count {
        font-weight: 700;
        min-width: 70px;
        text-align: right;
        margin-right: 10px;
        color: @memo-grey-darker;
    }

    .count-label {
        color: @memo-grey-dark;
    }
}

.wuwi-is-hidden {
    border: solid 1px @memo-grey-lighter;
    padding: 20px;
    width: 100%;
    margin-top: 20px;
}

.add-skill-card {
    cursor: pointer;
    border: 2px dashed @memo-grey-light;
    background-color: @memo-grey-lightest;
    transition: all 0.2s ease;

    &:hover {
        border: 2px solid @memo-green;
        background-color: darken(@memo-grey-lightest, 5%);
    }

    .add-skill-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 20px;
        text-align: center;
        color: @memo-grey-dark;

        .add-icon {
            font-size: 24px;
            margin-bottom: 10px;
            color: @memo-grey;
        }

        span {
            font-weight: 500;
        }
    }

    &:hover .add-skill-content {
        color: @memo-green;

        .add-icon {
            color: @memo-green;
        }
    }
}

.user-profile-header-container {
    position: absolute;
    top: -10px;
    right: 0px;

    .user-header-options-btn {
        cursor: pointer;
        background: white;
        border-radius: 24px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 18px;
        height: 30px;
        width: 30px;
        min-width: 30px;
        transition: filter 0.1s;
        color: @memo-grey-dark;

        &:hover {
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }


}

.placeholder-card {
    color: @memo-grey;
    font-style: italic;
    text-align: center;
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.dropdown-icon {
    .toggle-icon {
        &.active {
            color: @memo-blue-link;
        }
    }
}
</style>
