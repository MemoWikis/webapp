<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Styles.Render("~/bundles/QuestionList") %>

<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>


<div id="QuestionListApp">
    
    <div>
        <question-list-component inline-template category-id="<%= Model.CategoryId %>" all-question-count="<%= Model.AllQuestionCount %>">
            
            <div>
                <div class="questionListHeader">
                    <div class="questionListTitle">
                        <span>{{questions.length}}</span>
                        <span v-if="questions.length < allQuestionCount">
                            von {{allQuestionCount}}
                        </span>
                        <span>
                            {{questionText}} im Thema
                        </span>
                    </div>

                    <div class="questionListFilter">
                        <i class="fas fa-ellipsis-v"></i>
                    </div>
                </div>

                <question-component inline-template v-for="q in questions" :question-id="q.Id" :question-title="q.Title" :question-image="q.ImageData" :knowledge-state="q.CorrectnessProbability" :is-in-wishknowledge="q.IsInWishknowledge" :url="q.LinkToQuestion" :has-personal-answer="q.HasPersonalAnswer">
                    
                    <div class="singleQuestionRow" :class="{ open: showFullQuestion }">
                        <div class="knowledgeStatus" :style="backgroundColor"></div>
                        <div class="questionSectionFlex">
                            <div class="questionSection">

                                <div class="questionContainer">
                                    
                                    <div class="questionBodyTop">
                                        
                                        <div class="questionImg">
                                            <img :src="questionImage"></img>
                                        </div>
                                        
                                        <div class="questionContainerTopSection" >
                                            <div class="questionHeader" @click="expandQuestion()">
                                                <div class="questionTitle" ref="questionTitle" :id="questionTitleId" :class="{ trimTitle : !showFullQuestion }">{{questionTitle}}</div>
                                                <div class="questionHeaderIcons">
                                                    <div class="iconContainer">
                                                        <span :id="pinId" class="Pin" :data-question-id="questionId">
                                                        </span>
<%--                                                        <i class="far fa-heart"></i>--%>
                                                    </div>
                                                    <div class="iconContainer">
                                                        <i class="fas fa-angle-down rotateIcon" :class="{ open : showFullQuestion }"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                                            <div class="questionBody">
                                                <div class="extendedQuestion">
                                                    <component :is="questionDetails.extendedQuestion && {template:questionDetails.extendedQuestion}"></component>
                                                </div>

                                                <div class="answer">
                                                    <strong>Antwort:</strong><br/>
                                                    {{answer}}
                                                </div>
                                                <div class="extendedAnswer" v-if="extendedAnswer != null && extendedAnswer.length > 0">
                                                    <strong>Ergänzungen zur Antwort:</strong><br/>
                                                    {{extendedAnswer}}
                                                </div>
                                                <div class="notes">
                                                    <div class="relatedCategories">Thema: <a v-for="c in categories" :href="c.url">{{c.name}}, </a></div>
                                                    <div class="author">Erstellt von: <a :href="author.url">{{author}}</a></div>
                                                    <div class="sources" v-if="references">Quelle: <a v-for="r in references" :href="r.referenceText">{{r.referenceText}}</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        </div>

                                    </div>
                                    <div class="questionBodyBottom" v-show="showFullQuestion">
                                        <div class="questionDetails">

                                            <div class="questionCorrectnessProbability questionDetailRow">
                                                <div class="questionDetailIcon probability">
                                                    {{correctnessProbability}}
                                                </div>
                                                <div class="questionDetailLabel">
                                                    Wahrscheinlichkeit, dass du diese Frage richtig beantwortest. (Basis: Alle memucho-Nutzer).
                                                </div>
                                            </div>

                                            <div class="questionTotalAnswers questionDetailRow">
                                                <div class="questionDetailIcon">
                                                    <span class="sparklineTotals" :data-answerstrue="questionDetails.totalCorrectAnswers" :data-answersfalse="questionDetails.totalWrongAnswers"></span>
                                                </div>
                                                <div class="questionDetailLabel">
                                                    Insgesamt <b>{{questionDetails.totalAnswers}}</b>x beantwortet, davon <b>{{questionDetails.totalCorrectAnswers}}</b>x richtig
                                                </div>
                                            </div>

                                            <div class="questionPersonalAnswers questionDetailRow" v-if="isLoggedIn">
                                                <div class="questionDetailIcon">
                                                    <span class="sparklineTotalsUser" :data-answerstrue="questionDetails.personalCorrectAnswers" :data-answersfalse="questionDetails.personalWrongAnswers"></span>
                                                </div>
                                                <div class="questionDetailLabel">
                                                    Von dir <b>{{questionDetails.personalAnswers}}</b>x beantwortet.
                                                </div>
                                            </div>

                                            <div class="questionViews questionDetailRow">
                                                <div class="questionDetailIcon">
                                                    <i class="fa fa-eye greyed"></i>
                                                </div>
                                                <div class="questionDetailLabel">
                                                    Diese Frage wurde <b>{{questionDetails.views}}</b>x angesehen.
                                                </div>
                                            </div>

                                            <div class="questionInWishknowledgeCount questionDetailRow">
                                                <div class="questionDetailIcon">
                                                    <span :id="innerPinId" class="Pin" :data-question-id="questionId">
                                                    </span>
                                                </div>
                                                <div class="questionDetailLabel">
                                                    <b>{{questionDetails.inWishknowledgeCount}}x</b> im Wunschwissen.
                                                </div>
                                            </div>

                                        </div>

                                        <div class="questionFooterIcons">
                                            <span>{{commentCount}}</span>
                                            <i class="far fa-comment"></i>
                                            <i class="fas fa-ellipsis-v"></i>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            
                            <div class="questionFooter" v-show="showFullQuestion">
                                <div class="questionFooterLabel btn-link"><a :href="url">Lernen</a></div>
                                <div class="questionFooterLabel btn-link"><a :href="linkToFirstCategory">Zur Themenseite</a></div>
                            </div>
                        </div>

                    </div>

                </question-component>
                <ul class="pagination">
                    <li class="page-item" :class="{ disabled : selectedPage == 1 }">
                        <span class="page-link" @click="loadPreviousQuestions()">Previous</span>
                    </li>
                    <li class="page-item" @click="loadQuestions(1)" :class="{ active : selectedPage == 1 }">
                        <span class="page-link">1</span>
                    </li>
                    <li class="page-item" v-show="leftSelectorArray.length > 0">
                        <span class="page-link">...</span>
                    </li>
                    <li class="page-item" v-for="(p, key) in centerArray" @click="loadQuestions(p)" :class="{ active : selectedPage == p }">
                        <span class="page-link">{{p}}</span>
                    </li>
                    <li class="page-item" v-show="rightSelectorArray.length > 0">
                        <span class="page-link">...</span>
                    </li>
                    <li class="page-item" @click="loadQuestions(pageArray.length)" :class="{ active : selectedPage == pageArray.length }">
                        <span class="page-link">{{pageArray.length}}</span>
                    </li>
                    <li class="page-item" :class="{ disabled : selectedPage == pageArray.length }">
                        <span class="page-link" @click="loadNextQuestions()">Next</span>
                    </li>
                </ul>
            </div>

        </question-list-component>
    </div>

</div>


<%= Scripts.Render("~/bundles/js/QuestionListApp") %>



