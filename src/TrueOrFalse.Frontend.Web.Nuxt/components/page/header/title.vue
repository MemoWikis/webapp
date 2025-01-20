<script lang="ts" setup>
import { VueElement } from 'vue'
import { Page, usePageStore } from '../pageStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { Author } from '~~/components/author/author'
import { ImageFormat } from '~~/components/image/imageFormatEnum'
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { Visibility } from '~/components/shared/visibilityEnum'

const pageStore = usePageStore()
const tabsStore = useTabsStore()
const outlineStore = useOutlineStore()

const textArea = ref()

const mobileFirstAuthor = ref<Author>()
const firstAuthors = computed(() => {
    if (isMobile) {
        mobileFirstAuthor.value = pageStore.authors[0]
        return [] as Author[]
    }
    else
        return pageStore.authors.length <= 4 ? pageStore.authors : pageStore.authors.slice(0, 4)
})

const groupedAuthors = computed(() => {
    if (isMobile)
        return pageStore.authors
    else
        return pageStore.authors.length > 4 ? pageStore.authors.slice(4, pageStore.authors.length + 1) : [] as Author[]
})

function resize() {
    outlineStore.titleIsFocused = true
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "42px"
        element.style.height = element.scrollHeight + "px"
    }
}

const readonly = ref(false)
onMounted(() => {
    if (tabsStore.activeTab != Tab.Text) {
        readonly.value = true
    }
})
watch(() => tabsStore.activeTab, (val: any) => {

    if (val === Tab.Text)
        readonly.value = false
    else {
        readonly.value = true
    }
})

const autoSaveTimer = ref()
const autoSave = () => {
    if (pageStore.visibility != Visibility.Owner)
        return

    if (autoSaveTimer.value) {
        clearTimeout(autoSaveTimer.value)
    }
    autoSaveTimer.value = setTimeout(() => {
        pageStore.saveName()
    }, 200)
}

onBeforeMount(() => {
    window.addEventListener('resize', resize)

    watch(() => pageStore.name, (newName) => {
        if (pageStore.initialName != newName) {
            if (pageStore.visibility === Visibility.Owner)
                autoSave()
            else
                pageStore.nameHasChanged = true
        }
    })
})

onMounted(async () => {
    await nextTick()
    resize()
})

onUnmounted(() => {
    window.removeEventListener('resize', resize)
})

async function scrollToChildPages() {
    if (tabsStore.activeTab != Tab.Text) {
        tabsStore.activeTab = Tab.Text
        await nextTick()
        await nextTick()
    }
    const s = document.getElementById('PageGrid')
    if (s)
        s.scrollIntoView({ behavior: 'smooth' })
}

const { isMobile } = useDevice()

const page = useState<Page>('page')

const viewsLabel = computed(() => {
    if (pageStore.views === 1)
        return `1 Aufruf`

    let viewCount = pageStore.views.toString()

    if (pageStore.views >= 10000) {
        const formatter = new Intl.NumberFormat('de-DE')
        viewCount = formatter.format(pageStore.views)
    }
    return `${viewCount} Aufrufe`
})

const pageTitle = ref()
function focus() {
    outlineStore.titleIsFocused = true
}
function blur() {
    outlineStore.titleIsFocused = false
}

const showParents = computed(() => {
    if (pageStore.isWiki)
        return pageStore.parentPageCount > 0
    else return pageStore.parentPageCount > 1
})
const ariaId = useId()
const ariaId2 = useId()

</script>

<template>
    <div id="PageHeaderContainer">
        <h1 id="PageTitle" ref="pageTitle">
            <textarea placeholder="Gib deiner Seite einen Titel" @input="resize()" ref="textArea"
                v-model="pageStore.name" v-if="pageStore" :readonly="readonly" @focus="focus" @blur="blur"></textarea>
            <template v-else-if="page">
                {{ page.name }}
            </template>
        </h1>
        <div id="PageHeaderDetails" :class="{ 'is-mobile': isMobile }">
            <div v-if="pageStore.childPageCount > 0 && !isMobile" class="page-detail clickable" @click="scrollToChildPages()" v-tooltip="'Alle UnterSeiten'">
                <font-awesome-icon icon="fa-solid fa-sitemap" class="page-fa-icon" />
                <div class="page-detail-label">{{ pageStore.childPageCount }}</div>
            </div>

            <div class="page-detail-spacer" v-if="showParents && pageStore.childPageCount > 0">
            </div>

            <VDropdown :aria-id="ariaId" :distance="6">
                <button v-show="showParents" class="parent-tree-btn">

                    <div class="page-detail">
                        <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" class="page-fa-icon" />
                        <div class="page-detail-label">{{ pageStore.parentPageCount }}</div>
                    </div>

                </button>
                <template #popper>
                    <template v-for="parent in pageStore.parents">
                        <LazyNuxtLink class="dropdown-row" v-if="parent.id > 0"
                            :to="$urlHelper.getPageUrl(parent.name, parent.id)">
                            <div class="dropdown-icon">
                                <Image :src="parent.imgUrl" :format="ImageFormat.Page" class="header-page-icon" />
                            </div>
                            <div class="dropdown-label">{{ parent.name }}</div>
                        </LazyNuxtLink>
                    </template>

                </template>
            </VDropdown>

            <div class="page-detail-spacer" v-if="pageStore.views > 0 && (pageStore.childPageCount > 1 && !isMobile || pageStore.parentPageCount > 1)">
            </div>

            <div v-if="pageStore.views > 0" class="page-detail">
                <div class="page-detail-label">
                    {{ viewsLabel }}
                </div>
            </div>

            <div v-if="pageStore.views > 0 ||
                (pageStore.childPageCount > 0 || pageStore.parentPageCount > 0)" class="page-detail-spacer">
            </div>

            <template v-for="author in firstAuthors">
                <LazyNuxtLink v-if="author.id > 0" :to="$urlHelper.getUserUrl(author.name, author.id)"
                    v-tooltip="author.name" class="header-author-icon-link">
                    <Image :src="author.imgUrl" :format="ImageFormat.Author" class="header-author-icon"
                        :alt="`${author.name}'s profile picture'`" />
                </LazyNuxtLink>
            </template>

            <VDropdown :aria-id="ariaId2" :distance="6">
                <div v-if="isMobile && groupedAuthors.length === 1 && mobileFirstAuthor && mobileFirstAuthor.id > 0" :to="$urlHelper.getUserUrl(mobileFirstAuthor.name, mobileFirstAuthor.id)" class="header-author-icon-link">
                    <Image :src="mobileFirstAuthor.imgUrl" :format="ImageFormat.Author" class="header-author-icon" :alt="`${mobileFirstAuthor.name}'s profile picture'`" />
                </div>
                <div v-else-if="groupedAuthors.length > 1" class="additional-authors-btn" :class="{ 'long': groupedAuthors.length > 9 }">
                    <span>
                        +{{ groupedAuthors.length }}
                    </span>
                </div>
                <template #popper>
                    <template v-for="author in groupedAuthors">
                        <LazyNuxtLink class="dropdown-row" v-if="author.id > 0"
                            :to="$urlHelper.getUserUrl(author.name, author.id)">
                            <div class="dropdown-icon">
                                <Image :src="author.imgUrl" :format="ImageFormat.Author" class="header-author-icon" />
                            </div>
                            <div class="dropdown-label">{{ author.name }}</div>
                        </LazyNuxtLink>
                    </template>
                </template>
            </VDropdown>


        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.header-author-icon {
    height: 20px;
    width: 20px;
    min-height: 20px;
    min-width: 20px;
    margin: 0 4px;
    max-height: 20px;
    max-width: 20px;
}

#PageHeaderContainer {
    padding-left: 20px;
    width: 100%;
    color: @memo-grey-dark;

    #PageTitle {
        min-height: 49px;
        margin: 0;
        line-height: 1.1;

        textarea {
            line-height: 1.1;
            width: 100%;
            border: none;
            outline: none;
            min-height: 18px;
            resize: none;
            padding: 0;
            padding-left: 0;
            height: 42px;
            overflow: hidden;
        }
    }

    .parent-tree-btn {
        background: none;
    }

    .additional-authors-btn {
        border-radius: 10px;
        min-width: 20px;
        height: 20px;
        line-height: 20px;
        text-align: center;
        justify-content: center;
        align-items: center;
        display: inline-flex;
        font-size: 10px;
        font-weight: 700;
        border: solid 1px @memo-grey-light;
        padding: 0 2px;
        background: none;
        cursor: pointer;
        transition: all .1s ease-in-out;

        &.long {
            padding: 0 4px;
        }

        &:hover {
            background: @memo-blue;
            color: white;
            transition: all .1s ease-in-out;
            border: solid 1px @memo-blue;
        }

        span {
            position: relative;
            left: -1px;
        }
    }

    #PageHeaderDetails {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        min-height: 21px;

        .header-author-icon-link {
            margin-right: 8px;
        }

        .header-author-icon,
        .header-page-icon {
            height: 20px;
            width: 20px;
            min-height: 20px;
            min-width: 20px;
            margin: 0 2px;
        }

        .page-detail {
            margin-right: 8px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .page-fa-icon {
                margin-right: 6px;
            }

            .page-detail-label {
                padding-left: 0px;
            }

            &.clickable {
                cursor: pointer;

                &:hover {
                    color: @memo-blue-link;
                }

                transition: color ease-in-out 0.2s;
            }
        }

        .page-detail-spacer {
            height: 100%;
            width: 1px;
            background: @memo-grey-light;
            margin-right: 8px;
            min-height: 12px;

        }

        .page-detail-flex-breaker {
            flex-basis: 100%;
        }
    }
}
</style>