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


const emit = defineEmits(['select-item'])
const open = ref(false)

const selectedItem = ref('')
watch(selectedItem, (item) => {
    emit('select-item', item);
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
                        <img :src="c.ImageUrl" />
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
                        <img :src="q.ImageUrl" />
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
                        <img class="authorImg" :src="u.ImageUrl" />
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
@import (reference) '../../assets/includes/imports.less';

.searchAutocomplete {
    z-index: 105;
    margin: 0;

    .dropdown-menu {
        min-width: 280px;
        width: 100%;
        padding: 0;

        .dropdownFooter {
            line-height: 20px;
            padding: 8px;
            text-align: end;

            .dropdownLink {
                color: @memo-blue-link;
                cursor: pointer;

                &:hover {
                    text-decoration: underline;
                }
            }
        }
    }

    .searchBanner {
        background: @memo-blue;
        color: white;
        display: flex;
        justify-content: space-between;
        flex-direction: row;
        cursor: unset;
        border: 1px solid rgba(255, 255, 255, 30%);

        div {
            padding: 0 12px;
            cursor: unset;

            &.link {
                cursor: pointer;
                color: white;

                &:hover {
                    text-decoration: underline;
                }
            }
        }
    }

    .searchInputContainer {
        display: flex;
        flex-direction: row-reverse;
        flex-wrap: nowrap;

        .hasSearchIcon {
            padding-right: 54px;
        }

        .searchIconContainer {
            height: 34px;
            font-size: 25px;
            position: relative;
            left: -40px;
        }
    }

    .searchResultItem {
        padding: 4px 8px;
        display: flex;
        width: 100%;
        height: 70px;
        transition: .2s ease-in-out;
        cursor: pointer;

        &:hover {
            background: @memo-grey-lighter;
            color: @memo-blue;
        }

        .searchResultLabelContainer {
            width: 100%;
            height: 100%;
            overflow: hidden;
        }

        .searchResultLabel {
            height: 40px;
            line-height: normal;
            color: @memo-grey-darker;
            text-overflow: ellipsis;
            margin: 0;
            overflow: hidden;
            max-width: 408px;
            white-space: normal;
        }

        .searchResultSubLabel {
            color: @memo-grey-light;
            font-style: italic;
            height: 20px;
            line-height: normal;
        }


        img {
            max-height: 62px;
            max-width: 62px;
            height: auto;
            margin-right: 10px;
            width: 100%;
            margin-top: 0px !important;

            &.authorImg {
                border-radius: 50%;
            }
        }
    }
}


#StickySearch,
#HeaderSearch {
    width: 100%;
    display: flex;
    flex-direction: row-reverse;
    height: 100%;
    align-items: center;

    .StickySearchContainer,
    .SearchContainer {
        width: 100%;

        input {
            min-width: 0px;
            width: 0px;
            border: none;
            padding: 0;
            transition: all 0.3s;
            background: transparent;
        }

        &.showSearch {
            input {
                border: 1px solid #ccc;
                width: 100%;
                padding: 6px 40px 6px 12px;
                background: white;
            }
        }
    }

    .searchButton {
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        transition: all 0.3s;
        font-size: 20px;
        position: absolute;
        z-index: 1050;
        width: 34px;
        transform: translateZ(0);

        .fa-search,
        .fa-times {
            transition: all 0.3s;
        }

        &:hover {
            cursor: pointer;

            .fa-search,
            .fa-times {
                color: @memo-green;
            }
        }
    }

    .fa-times {
        padding-right: 6px;
    }
}

#StickySearch {
    .searchButton {

        .fa-search,
        .fa-times {
            color: @memo-grey-dark;
        }

        &:hover {

            .fa-search,
            .fa-times {
                color: @memo-blue;
            }
        }
    }
}

#HeaderSearch {
    @media (max-width: 767px) {
        padding-right: 0;
    }
}
</style>