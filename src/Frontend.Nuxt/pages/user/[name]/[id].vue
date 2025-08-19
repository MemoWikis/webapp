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

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const userStore = useUserStore()
const { $logger, $urlHelper } = useNuxtApp()
const { t } = useI18n()

interface Overview {
    activityPoints: {
        total: number
        questionsInOtherWishknowledges: number
        questionsCreated: number
        publicWishknowledges: number
    }
    publicQuestionsCount: number
    publicPagesCount: number
    privateQuestionsCount: number
    privatePagesCount: number
    wuwiCount: number
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
    skills?: PageData[]
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

interface Question {
    id: number
    title: string
    knowledgeStatus: 'not-learned' | 'needs-learning' | 'needs-consolidation' | 'solid'
    popularity: number
    creationDate: string
    wikiId?: number
}

// Add fake questions data
const fakeQuestions: Question[] = [
    {
        id: 1,
        title: "What is the capital of France?",
        knowledgeStatus: "solid",
        popularity: 12250,
        creationDate: "2024-01-15T10:30:00Z",
        wikiId: 1
    },
    {
        id: 2,
        title: "How do photosynthesis work in plants?",
        knowledgeStatus: "needs-consolidation",
        popularity: 851239,
        creationDate: "2024-02-20T14:45:00Z",
        wikiId: 2
    },
    {
        id: 3,
        title: "What are the fundamental laws of thermodynamics?",
        knowledgeStatus: "needs-learning",
        popularity: 62337,
        creationDate: "2024-03-10T09:15:00Z",
        wikiId: 3
    },
    {
        id: 4,
        title: "Who wrote Romeo and Juliet?",
        knowledgeStatus: "not-learned",
        popularity: 234,
        creationDate: "2024-01-05T16:20:00Z",
        wikiId: 4
    },
    {
        id: 5,
        title: "What is the difference between HTML and CSS?",
        knowledgeStatus: "solid",
        popularity: 112,
        creationDate: "2024-02-28T11:00:00Z",
        wikiId: 5
    }
]

const hasWikis = computed(() => {
    return profile.value?.wikis && profile.value.wikis.length > 0
})

const hasSkills = computed(() => {
    return profile.value?.skills && profile.value.skills.length > 0 && profile.value.skills.some(skill => skill.isPublic && skill.knowledgebarData?.total > 0)
})

const hasQuestions = computed(() => {
    return fakeQuestions.length > 0 || (profile.value?.overview.publicQuestionsCount ? profile.value.overview.publicQuestionsCount > 0 : false)
})

const showSkills = computed(() => {
    return profile.value?.user.id === userStore.id || hasSkills.value
})

const showAddSkillModal = ref(false)

function handleAddSkillClick() {
    showAddSkillModal.value = true
}

async function handleSkillAdded(skillId: number) {
    console.log('Skill added successfully:', skillId)
    // Refresh the profile to show the new skill
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
</script>

<template>
    <div v-if="profile && profile.user.id > 0" class="user-container">
        <div class="user-content">
            <div id="UserHeader">
                <div class="profile-header" ref="profileHeader">
                    <Image :format="ImageFormat.Author" :src="profile.user.imageUrl"
                        class="profile-picture-small" />

                    <div class="profile-header-info">
                        <h1>{{ profile.user.name }} <span class="profile-id">#{{ profile.user.id }}</span></h1>
                        <UserAboutMeSection :user-id="profile.user.id" :about-me="profile.user.aboutMeText" />
                    </div>
                </div>
            </div>

            <div class="panel-divider"></div>

            <LayoutPanel :id="UserSection.STATS_SECTION.id" :title="t(UserSection.STATS_SECTION.translationKey)">
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="1000" label="Reputation" :icon="['fas', 'star']" :icon-color="color.memoYellow" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="2" label="Rank" :icon="['fas', 'crown']" :icon-color="color.memoYellow" url-value="/Nutzer" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small" background-color="transparent">
                    <!-- Filler -->
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="31" label="Pages" :icon="['fas', 'file-lines']" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="31" label="Wikis" :icon="['fas', 'file-lines']" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <LayoutCounter :value="profile.overview.publicQuestionsCount + profile.overview.privateQuestionsCount" label="Questions" :icon="['fas', 'circle-question']" />
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="hasSkills || profile.isCurrentUser" :id="UserSection.SKILLS_SECTION.id" :title="t(UserSection.SKILLS_SECTION.translationKey)">
                <template v-for="skill in profile.skills">
                    <UserSkillCard :skill="skill" v-if="profile.isCurrentUser || (skill.knowledgebarData && skill.knowledgebarData.total > 0)" />
                </template>

                <LayoutCard v-if="!hasSkills" :size="LayoutCardSize.Large">
                    {{ t('user.profile.noSkills') }}
                </LayoutCard>

                <LayoutCard v-if="profile.isCurrentUser" :size="LayoutCardSize.Small" @click="handleAddSkillClick" class="add-skill-card">
                    <div class="add-skill-content">
                        <font-awesome-icon :icon="['fas', 'plus']" class="add-icon" />
                        <span>{{ t('user.skills.addSkill') }}</span>
                    </div>
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel v-if="hasWikis" :id="UserSection.WIKIS_SECTION.id" :title="t(UserSection.WIKIS_SECTION.translationKey)">
                <MissionControlGrid v-if="isMobile" :pages="profile.wikis!" :no-pages-text="t('missionControl.pageTable.noWikis')" />

                <LayoutCard v-else :no-padding="true">
                    <MissionControlTable :pages="profile.wikis!" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                </LayoutCard>
            </LayoutPanel>

            <DevOnly>
                <LayoutPanel v-if="hasQuestions" :id="UserSection.QUESTIONS_SECTION.id" :title="t(UserSection.QUESTIONS_SECTION.translationKey)">
                    <LayoutCard :no-padding="true">
                        <LayoutQuestionList :questions="fakeQuestions" :no-questions-text="t('user.profile.noQuestions')" />
                    </LayoutCard>
                </LayoutPanel>
            </DevOnly>
        </div>

        <SidebarUser :user="profile.user" :has-wikis="hasWikis" :has-questions="hasQuestions" :show-skills="showSkills" :margin-top="sideBarMarginTop" />

        <!-- Add Skill Modal -->
        <UserSkillsAddSkillModal v-if="profile?.user.id" :show="showAddSkillModal" :user-id="profile.user.id" @close="showAddSkillModal = false" @skill-added="handleSkillAdded" />
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

        h1 {
            margin-top: 0px;

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
</style>
