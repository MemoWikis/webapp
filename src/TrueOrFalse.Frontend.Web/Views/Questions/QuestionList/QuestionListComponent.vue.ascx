<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>

<question-list-component 
        inline-template 
        category-id="<%= Model.CategoryId %>" 
        :all-question-count="questionsCount" 
        is-admin="<%= Model.IsInstallationAdmin %>"  
        :is-question-list-to-show="isQuestionListToShow"
        :active-question-id="activeQuestionId"
        :selected-page-from-active-question="selectedPageFromActiveQuestion">
        <div class="col-xs-12 questionListComponent" id="QuestionListComponent" :data-last-index="lastQuestionInListIndex">
            <template v-for="(q, index) in questions">
                <%: Html.Partial("~/Views/Questions/QuestionList/QuestionComponent.vue.ascx")%>
            </template>
            
            <%: Html.Partial("~/Views/Questions/AddQuestion/AddQuestionComponent.vue.ascx", new AddQuestionComponentModel(Model.CategoryId)) %>

            <div id="QuestionListPagination" v-show="hasQuestions">
                <ul class="pagination col-xs-12 row justify-content-xs-center" v-if="pageArray.length <= 8">
                    <li class="page-item page-btn" :class="{ disabled : selectedPage == 1 }">
                        <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                    </li>
                    <li class="page-item" v-for="(p, key) in pageArray" @click="loadQuestions(p)" :class="{ selected : selectedPage == p }">
                        <span class="page-link">{{p}}</span>
                    </li>
                    <li class="page-item page-btn" :class="{ disabled : selectedPage == pageArray.length }">
                        <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                    </li>
                </ul>

                <ul class="pagination col-xs-12 row justify-content-xs-center" v-else>
                    <li class="page-item col-auto page-btn" :class="{ disabled : selectedPage == 1 }">
                        <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                    </li>
                    <li class="page-item col-auto" @click="loadQuestions(1)" :class="{ selected : selectedPage == 1 }">
                        <span class="page-link">1</span>
                    </li>
                    <li class="page-item col-auto" v-show="selectedPage == 5">
                        <span class="page-link">2</span>
                    </li>
                    <li class="page-item col-auto" v-show="showLeftPageSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <span class="page-link" v-on:click.this="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                            <div class="dropup" v-on:click.this="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                                <div class="dropdown-toggle" type="button" id="DropUpMenuLeft" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" v-on:click="{showLeftSelectionDropUp = !showLeftSelectionDropUp}">
                                    ...
                                </div>
                                <ul id="DropUpMenuLeftList" class="pagination dropdown-menu" aria-labelledby="DropUpMenuLeft" v-show="showLeftSelectionDropUp">
                                    <li class="page-item" v-for="p in leftSelectorArray" @click="loadQuestions(p)">
                                        <span class="page-link">{{p}}</span>
                                    </li>
                                </ul>
                            </div>
                        </span>
                    </li>
                    <li class="page-item col-auto" v-for="(p, key) in centerArray" @click="loadQuestions(p)" :class="{ selected : selectedPage == p }">
                        <span class="page-link">{{p}}</span>
                    </li>

                    <li class="page-item col-auto" v-show="showRightPageSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <span class="page-link" v-on:click.this="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                            <div class="dropup" v-on:click.this="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                                <div class="dropdown-toggle" type="button" id="DropUpMenuRight" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" v-on:click="{showRightSelectionDropUp = !showRightSelectionDropUp}">
                                    ...
                                </div>
                                <ul id="DropUpMenuRightList" class="pagination dropdown-menu" aria-labelledby="DropUpMenuLeft" v-show="showRightSelectionDropUp">
                                    <li class="page-item" v-for="p in rightSelectorArray" @click="loadQuestions(p)">
                                        <span class="page-link">{{p}}</span>
                                    </li>
                                </ul>
                            </div>
                        </span>
                    </li>
                    <li class="page-item col-auto" v-show="selectedPage == pageArray.length - 4">
                        <span class="page-link">{{pageArray.length - 1}}</span>
                    </li>
                    <li class="page-item col-auto" @click="loadQuestions(pageArray.length)" :class="{ selected : selectedPage == pageArray.length }">
                        <span class="page-link">{{pageArray.length}}</span>
                    </li>
                    <li class="page-item col-auto page-btn" :class="{ disabled : selectedPage == pageArray.length }">
                        <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                    </li>
                </ul>
            </div>
        </div>
    </question-list-component>




