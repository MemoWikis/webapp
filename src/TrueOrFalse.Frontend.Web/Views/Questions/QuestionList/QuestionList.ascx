<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%= Styles.Render("~/bundles/QuestionList") %>

<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>

<div id="QuestionListApp" class="row">
    <div class="col-xs-12 drop-down-question-sort">
        <div>Du lernst <b>alle</b> Fragen aus diesem Thema (4.888)</div>
        <sort-list inline-template>
                <div class="dropdown">
                    <button class="btn btn-default dropdown-toggle" type="button" id="sortQuestionList" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        {{activeSortOrder}}
                        <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu" aria-labelledby="sortQuestionList">
                        <li v-for="item in sortOrdersForQuestionsList" @click='changeSortOrder(item)'><a>{{item}}</a></li>
                    </ul>
                </div>
        </sort-list>
        <div class="Button dropdown">
            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right">
                <li><a href="<%= Links.CreateQuestion(Model.CategoryId) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                <li><a href="#" @click="toggleQuestionsList()"><i class="fa fa-angle-double-down"></i>&nbsp;Alle Fragen erweitern</a></li>
                <li><a href="#" data-allowed="logged-in"><i class="fa fa-play"></i>&nbsp;Fragen jetzt lernen </a></li>
            </ul>
        </div>
    </div>
    <question-list-component inline-template category-id="<%= Model.CategoryId %>" all-question-count="<%= Model.AllQuestionCount %>" is-admin="<%= Model.IsInstallationAdmin %>"  :is-question-list-to-show="isQuestionListToShow">
        <div class="col-xs-12">
            <div class="questionListHeader row">
                <div class="questionListTitle col-xs-11">
                    <span>{{questions.length}}</span>
                    <span v-if="questions.length < allQuestionCount">
                        von {{allQuestionCount}}
                    </span>
                    <span>
                        {{questionText}} im Thema
                    </span>
                </div>
            </div>
            <question-component inline-template v-for="q in questions" 
                                :question-id="q.Id" 
                                :question-title="q.Title" 
                                :question-image="q.ImageData" 
                                :knowledge-state="q.CorrectnessProbability" 
                                :is-in-wishknowledge="q.IsInWishknowledge" 
                                :url="q.LinkToQuestion" 
                                :has-personal-answer="q.HasPersonalAnswer" 
                                :is-admin="isAdmin"
                                :is-question-list-to-show ="isQuestionListToShow">
                
                <div class="singleQuestionRow row" :class="[{ open: showFullQuestion,  notShow: !isQuestionListToShow}, backgroundColor]">
                    <div class="questionSectionFlex col-auto">
                        <div class="questionContainer">
                            <div class="questionBodyTop row">
                                <div class="questionImg col-xs-1" @click="expandQuestion()">
                                    <img :src="questionImage"></img>
                                </div>
                                <div class="questionContainerTopSection col-xs-11" >
                                    <div class="questionHeader row">
                                        <div class="questionTitle col-xs-10" ref="questionTitle" :id="questionTitleId" :class="{ trimTitle : !showFullQuestion }" @click.self="expandQuestion()">{{questionTitle}}</div>
                                        <div class="questionHeaderIcons col-xs-2 row"  @click.self="expandQuestion()">
                                            <div class="iconContainer col-xs-6 float-right" @click="expandQuestion()">
                                                <i class="fas fa-angle-down rotateIcon" :class="{ open : showFullQuestion }"></i>
                                            </div>
                                            <div class="questionListPinContainer iconContainer col-xs-6">
                                                <span :id="pinId" class="Pin" :data-question-id="questionId">
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="extendedQuestionContainer" v-show="showFullQuestion">
                                        <div class="questionBody">
                                            <div class="RenderedMarkdown extendedQuestion">
                                                <component :is="extendedQuestion && {template:extendedQuestion}"></component>
                                                <%--<div :id="extendedQuestionId"></div>--%>
                                            </div>

                                            <div class="answer">
                                                <strong>Antwort:</strong><br/>
                                                 <component :is="answer && {template:answer}"></component>

                                            </div>
                                            <div class="extendedAnswer" v-if="extendedAnswer != null && extendedAnswer.length > 0">
                                                <strong>Ergänzungen zur Antwort:</strong><br/>
                                                <component :is="extendedAnswer && {template:extendedAnswer}"></component>
                                            </div>
                                            <div class="notes">
                                                <div class="relatedCategories">{{topicTitle}}: <a v-for="(c, i) in categories" :href="c.linkToCategory">{{c.name}}{{i != categories.length - 1 ? ', ' : ''}}</a></div>
                                                <div class="author">Erstellt von: <a :href="authorUrl">{{author}}</a></div>
                                                <div class="sources" v-if="references.length > 0 && references[0].referenceText.length > 0">Quelle: <a v-for="r in references" :href="r.referenceText">{{r.referenceText}}</a></div>
                                            </div>
                                        </div>
                                        
                                        <div class="questionDetailsSection" style="display: flex;">
                                            <div class="probabilitySection"><span class="percentageLabel" :class="backgroundColor">{{correctnessProbability}}</span> <span class="chip" :class="backgroundColor">{{correctnessProbabilityLabel}}</span></div>
                                            <div></div>
                                            <div>{{answerCount}} mal beantwortet | {{correctAnswers}} richtig / {{wrongAnswers}} falsch</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="questionBodyBottom" v-show="showFullQuestion">
                                <div :id="questionDetailsId" class="questionDetails" ></div>
                                <div class="row">
                                    <div class="questionFooterIcons col-xs-12 row pull-right">
                                        <div class="footerIcon col-xs-6 pull-right ellipsis dropup" @click="showQuestionMenu = true">
                                            <i class="fas fa-ellipsis-v" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"></i>
                                            <ul class="dropdown-menu dropdown-menu-right" v-show="showQuestionMenu">
                                                <li>
                                                    <a :href="url">Frageseite anzeigen</a>
                                                </li>
                                                <li v-if="isCreator || isAdmin == 'True' ">
                                                    <a :href="editUrl" >Frage bearbeiten</a>
                                                </li>
                                                <li id="DeleteQuestion" v-if="isCreator || isAdmin == 'True' ">
                                                    <a class="TextLinkWithIcon" data-toggle="modal" :data-questionid="questionId" href="#modalDeleteQuestion">
                                                        Frage löschen
                                                    </a>
                                                </li>
                                                <li>
                                                    <a :href="historyUrl">Versionen anzeigen</a>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="footerIcon col-xs-6 pull-right fullWidth" >
                                            <a class="commentIcon" :href="linkToComments">
                                                <span>{{commentCount}}</span>
                                                <i class="far fa-comment"></i>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </question-component>
            <div id="QuestionListPagination">
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
</div>


<%= Scripts.Render("~/bundles/js/QuestionListApp") %>



