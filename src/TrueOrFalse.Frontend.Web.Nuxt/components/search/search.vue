<script lang="ts" setup>
import { ref, PropType } from 'vue'
import _ from 'underscore'
import { FullSearch, QuestionItem, SearchType, TopicItem, UserItem } from './searchHelper'
import { ImageStyle } from '../image/imageStyleEnum'

const props = defineProps({
    searchType: Number as PropType<SearchType>,
    id: { type: [String, Number], required: true },
    showSearchIcon: Boolean,
    showSearch: Boolean,
})


const emit = defineEmits(['selectItem'])

const selectedItem = ref(null as TopicItem | QuestionItem | UserItem | null)
watch(selectedItem, (item: TopicItem | QuestionItem | UserItem | null) => {
    emit('selectItem', item);
})

const showDropdown = ref(false)
const searchTerm = ref('')

watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value == false) {
        showDropdown.value = true;
        search()
    }
    else
        showDropdown.value = false;
})

function inputValue(e: Event) {
    const target = e.target as HTMLTextAreaElement
    searchTerm.value = target.value
}

onBeforeMount(() => {
    switch (props.searchType) {
        case SearchType.Category:
            searchUrl.value = '/apiVue/Search/Category'
            break;
        case SearchType.CategoryInWiki:
            searchUrl.value = '/apiVue/Search/CategoryInWiki'
            break
        default:
            searchUrl.value = '/apiVue/Search/All'
    }
})

const searchUrl = ref('')
const lockDropdown = ref(false)
const noResults = ref(false)
const categoryCount = ref(0)
const questionCount = ref(0)
const userCount = ref(0)
const userSearchUrl = ref('')

const categories = ref([] as TopicItem[])
const questions = ref([] as QuestionItem[])
const users = ref([] as UserItem[])
const config = useRuntimeConfig()

async function search() {
    showDropdown.value = true

    var result = await $fetch<FullSearch>(searchUrl.value + `?term=${encodeURIComponent(searchTerm.value)}`, {
        mode: 'no-cors',
        credentials: 'include'
    })
    if (result != null) {
        categories.value = result.categories;
        questions.value = result.questions;
        users.value = result.users;
        noResults.value = result.categories.length + result.questions.length + result.users.length <= 0;
        categoryCount.value = result.categoryCount;
        questionCount.value = result.questionCount;
        userCount.value = result.userCount;
        userSearchUrl.value = result.userSearchUrl;
    }
}

function selectItem(item: TopicItem | QuestionItem | UserItem) {
    selectedItem.value = item;
}
function openUsers() {
    location.href = userSearchUrl.value
}
</script>

<template>
    <div class="search-category-component" :id="id + 'Container'">
        <form v-on:submit.prevent>
            <div class="form-group dropdown searchAutocomplete" :class="{ 'open': showDropdown }">
                <div class="searchInputContainer">
                    <input ref="searchInput" class="form-control dropdown-toggle"
                        :class="{ 'hasSearchIcon': props.showSearchIcon }" type="text" v-bind:value="searchTerm"
                        @input="event => inputValue(event)" autocomplete="off" @click="lockDropdown = false"
                        placeholder="Suche" />
                </div>

                <VDropdown :distance="6" :triggers="[]" :shown="showDropdown" no-auto-focus>
                    <template #popper>
                        <ul :aria-labelledby="id + 'Dropdown'">
                            <li v-show="categories.length > 0" class="searchBanner">
                                <div>Themen </div>
                                <div>{{ categoryCount }} Treffer</div>
                            </li>
                            <li class="searchResultItem" v-for="c in categories" @click="selectItem(c)"
                                data-toggle="tooltip" v-tooltip="c.Name">
                                <Image :src="c.ImageUrl" />
                                <div class="searchResultLabelContainer">
                                    <div class="searchResultLabel body-m">{{ c.Name }}</div>
                                    <div class="searchResultSubLabel body-s">{{ c.QuestionCount }} Frage<template
                                            v-if="c.QuestionCount != 1">n</template></div>
                                </div>
                            </li>
                            <li v-show="questions.length > 0" class="searchBanner">
                                <div>Fragen </div>
                                <div>{{ questionCount }} Treffer</div>
                            </li>
                            <li class="searchResultItem" v-for="q in questions" @click="selectItem(q)"
                                data-toggle="tooltip" data-placement="top" :title="q.Name">
                                <Image :src="q.ImageUrl" />
                                <div class="searchResultLabelContainer">
                                    <div class="searchResultLabel body-m">{{ q.Name }}</div>
                                    <div class="searchResultSubLabel body-s"></div>
                                </div>
                            </li>
                            <li v-show="users.length > 0" class="searchBanner">
                                <div>Nutzer </div>
                                <div class="link" @click="openUsers()">zeige {{ userCount }} Treffer</div>
                            </li>
                            <li class="searchResultItem" v-for="u in users" @click="selectItem(u)" data-toggle="tooltip"
                                data-placement="top" :title="u.Name">
                                <Image class="authorImg" :src="u.ImageUrl" :style="ImageStyle.Author" />
                                <div class="searchResultLabelContainer">
                                    <div class="searchResultLabel body-m">{{ u.Name }}</div>
                                    <div class="searchResultSubLabel body-s"></div>
                                </div>
                            </li>
                            <li v-show="noResults">
                                <div>Kein Treffer</div>
                            </li>
                        </ul>
                    </template>
                </VDropdown>

            </div>

        </form>
    </div>
</template>

<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.searchButton {
    svg.fa-xmark {
        color: @memo-blue;

        &:hover {
            color: @memo-green;
        }
    }
}

input {
    border-radius: 24px;
}
</style>