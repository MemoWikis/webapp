<script lang="ts" setup>
import { ref, PropType } from 'vue';
import _ from 'underscore'
import { SearchType } from './searchTypeEnum';


const props = defineProps({
    searchType: Number as PropType<SearchType>,
    id: [String, Number],
    showSearchIcon: Boolean,
    showSearch: Boolean,
})


const emit = defineEmits(['selectItem'])
const open = ref(false)

const selectedItem = ref('')
watch(selectedItem, (item) => {
    emit('selectItem', item);
})

const debounceSearch = _.debounce(() => {
    search()
}, 500)

const showDropdown = ref(false)
const searchTerm = ref('')
watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value == false) {
        showDropdown.value = true;
        debounceSearch();
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
            searchUrl.value = '/api/Search/Category';
            break;
        case SearchType.CategoryInWiki:
            searchUrl.value = '/api/Search/CategoryInWiki'
            break;
        default:
            searchUrl.value = '/api/Search/All';
    }
})

const searchUrl = ref('')
const isMounted = ref(false)
const lockDropdown = ref(false)
const noResults = ref(false)
const categoryCount = ref(0)
const questionCount = ref(0)
const userCount = ref(0)
const userSearchUrl = ref('')

const categories = ref([])
const questions = ref([])
const users = ref([])

class CategoryItem {
    Id: number
    Name: string
    Url: string
    QuestionCount: number
    ImageUrl: string
    MiniImageUrl: string
    IconHtml: string
    Visibility: number
}

type QuestionItem = {
    Id: number
    Name: string
    Url: string
    ImageUrl: string
    Visibility: number
}

type UserItem = {
    Id: number
    Name: string
    Url: string
    ImageUrl: string
    Visibility: number
}

type FullSearch = {
    categories: CategoryItem[]
    categoryCount: number
    questions: QuestionItem[]
    questionCount: number
    users: UserItem[]
    userCount: number
    userSearchUrl: string
}

async function search() {
    showDropdown.value = true;
    var data = {
        term: searchTerm.value,
    };

    var result = await $fetch<FullSearch>(searchUrl.value, {
        body: data,
        method: 'POST',
        mode: 'cors',
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

function selectItem(item) {
    selectedItem.value = item;
}
function openUsers() {
    location.href = userSearchUrl.value;
}
</script>

<template>
    <div class="search-category-component" :id="id + 'Container'">
        <form v-on:submit.prevent>
            <div class="form-group dropdown searchAutocomplete" :class="{ 'open': showDropdown }">
                <div class="searchInputContainer">
                    <input ref="searchInput" class="form-control dropdown-toggle"
                        :class="{ 'hasSearchIcon': props.showSearchIcon }" type="text" v-bind:value="searchTerm"
                        @input="event => inputValue(event)" :id="props.id.toString" autocomplete="off"
                        @click="lockDropdown = false" aria-haspopup="true" placeholder="Suche" />
                </div>
                <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="id + 'Dropdown'">
                    <li v-if="categories.length > 0" class="searchBanner">
                        <div>Themen </div>
                        <div>{{ categoryCount }} Treffer</div>
                    </li>
                    <li class="searchResultItem" v-for="c in categories" @click="selectItem(c)" data-toggle="tooltip"
                        v-tooltip="c.Name">
                        <Image :src="c.ImageUrl" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ c.Name }}</div>
                            <div class="searchResultSubLabel body-s">{{ c.QuestionCount }} Frage<template
                                    v-if="c.QuestionCount != 1">n</template></div>
                        </div>
                    </li>
                    <li v-if="questions.length > 0" class="searchBanner">
                        <div>Fragen </div>
                        <div>{{ questionCount }} Treffer</div>
                    </li>
                    <li class="searchResultItem" v-for="q in questions" @click="selectItem(q)" data-toggle="tooltip"
                        data-placement="top" :title="q.Name">
                        <Image :src="q.ImageUrl" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ q.Name }}</div>
                            <div class="searchResultSubLabel body-s"></div>
                        </div>
                    </li>
                    <li v-if="users.length > 0" class="searchBanner">
                        <div>Nutzer </div>
                        <div class="link" @click="openUsers()">zeige {{ userCount }} Treffer</div>
                    </li>
                    <li class="searchResultItem" v-for="u in users" @click="selectItem(u)" data-toggle="tooltip"
                        data-placement="top" :title="u.Name">
                        <Image class="authorImg" :src="u.ImageUrl" style="author" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ u.Name }}</div>
                            <div class="searchResultSubLabel body-s"></div>
                        </div>
                    </li>
                    <li v-if="noResults">
                        <div>Kein Treffer</div>
                    </li>
                </ul>
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
</style>