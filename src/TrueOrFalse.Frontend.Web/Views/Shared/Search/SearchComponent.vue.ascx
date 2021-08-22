<div class="search-category-component" v-if="isMounted" :id="id + 'Container'">
    <form v-on:submit.prevent>
        <div class="form-group dropdown searchAutocomplete" :class="{ 'open' : showDropdown }">
            <div class="searchInputContainer">
                <input ref="searchInput" class="form-control dropdown-toggle":class="{ 'hasSearchIcon' : showSearchIcon }" type="text" v-model="searchTerm" :id="id" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true" placeholder="Suche"/>
                <div v-if="showSearchIcon" class="searchIconContainer">
                    <i class="fas fa-search"></i>
                </div>
            </div>
            <ul class="dropdown-menu" :aria-labelledby="id + 'Dropdown'">
                <li v-if="categories.length > 0" class="searchBanner">
                    <div>Themen </div>
                    <div>{{categoryCount}} Treffer</div>
                </li>
                <li class="searchResultItem" v-for="c in categories" @click="selectItem(c)" data-toggle="tooltip" data-placement="top" :title="c.Name">
                    <img :src="c.ImageUrl"/>
                    <div class="searchResultLabelContainer">
                        <div class="searchResultLabel body-m">{{c.Name}}</div>
                        <div class="searchResultSubLabel body-s">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                    </div>
                </li>
                <li v-if="questions.length > 0" class="searchBanner">
                    <div>Fragen </div>
                    <div>{{questionCount}} Treffer</div>
                </li>
                <li class="searchResultItem" v-for="q in questions" @click="selectItem(q)" data-toggle="tooltip" data-placement="top" :title="q.Name">
                    <img :src="q.ImageUrl"/>
                    <div class="searchResultLabelContainer">
                        <div class="searchResultLabel body-m">{{q.Name}}</div>
                        <div class="searchResultSubLabel body-s"></div>
                    </div>
                </li>
                <li v-if="users.length > 0" class="searchBanner">
                    <div>Nutzer </div>
                    <div class="link" @click="openUsers()">zeige {{userCount}} Treffer</div>
                </li>
                <li class="searchResultItem" v-for="u in users" @click="selectItem(u)" data-toggle="tooltip" data-placement="top" :title="u.Name">
                    <img class="authorImg" :src="u.ImageUrl"/>
                    <div class="searchResultLabelContainer">
                        <div class="searchResultLabel body-m">{{u.Name}}</div>
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