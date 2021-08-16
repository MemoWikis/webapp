﻿<div class="search-category-component" v-if="isMounted">
    <form class="" v-on:submit.prevent>
        <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open' : showDropdown }">

            <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" :id="id" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true"/>
            <ul class="dropdown-menu" :aria-labelledby="id">
                <li v-if="categories.length > 0" class="searchBanner">Themen:</li>
                <li class="searchResultItem" v-for="c in categories" @click="selectItem(c)" data-toggle="tooltip" data-placement="top" :title="c.Name">
                    <img :src="c.ImageUrl"/>
                    <div>
                        <div class="searchResultLabel body-m">{{c.Name}}</div>
                        <div class="searchResultQuestionCount body-s">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                    </div>
                </li>
                <li v-if="questions.length > 0" class="searchBanner">Fragen:</li>
                <li class="searchResultItem" v-for="q in questions" @click="selectItem(q)" data-toggle="tooltip" data-placement="top" :title="q.Name">
                    <img :src="q.ImageUrl"/>
                    <div>
                        <div class="searchResultLabel body-m">{{q.Name}}</div>
                    </div>
                </li>
                <li v-if="users.length > 0" class="searchBanner">Nutzer:</li>
                <li class="searchResultItem" v-for="u in users" @click="selectItem(u)" data-toggle="tooltip" data-placement="top" :title="u.Name">
                    <img class="authorImg" :src="u.ImageUrl"/>
                    <div>
                        <div class="searchResultLabel body-m">{{u.Name}}</div>
                    </div>
                </li>
                <li v-if="noResults">
                    <div>Kein Treffer</div>
                </li>

<%--                <li class="dropdownFooter body-m">
                    <b>{{totalCount}}</b> Treffer. <br/>
                    Deins ist nicht dabei? <span class="dropdownLink" @click="createCategory = true">Erstelle hier dein Thema</span>
                </li>--%>
            </ul>
        </div>

    </form>
</div>