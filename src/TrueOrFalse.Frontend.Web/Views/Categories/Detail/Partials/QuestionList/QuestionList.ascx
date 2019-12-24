<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>

<div id="QuestionListApp">
    <question-list-component category-id="<%= Model.CategoryId %>" all-question-count="<%= Model.AllQuestionCount %>">

        <div class="questionListHeader">
            <div class="questionListTitle">
                <span>{{questions.length}}</span>
                <span v-if="allQuestionCount > itemCountPerPage">
                    von {{allQuestionCount}}
                </span>
                <span>
                    Frage{{plural}} im Thema
                </span>
            </div>

            <div>
                <i class="fas fa-ellipsis-h"></i>
            </div>
        </div>

        <div v-for="q in questions">
            <div @click="expandQuestion(q.Id)">
                <div class="knowledgeStatus" :class="status"></div>

                <div class="questionContainer" v-show="showFullQuestion">
                    <div class="questionHeader">
                        <div class="questionImg"></div>
                        <div class="questionTitle">{{q.title}}</div>
                        <div class="questionHeaderIcons">
                            <i class="fas fa-heart"></i>
                            <i class="fas fa-chevron-down"></i>
                        </div>
                    </div>
                    <div class="questionBody">
                        <div class="answer">{{expandedQuestions[q.Id].answer}}</div>
                        <div class="extendedAnswer">{{expandedQuestions[q.Id].extendedAnswer}}</div>
                        <div class="notes">
                            <div class="relatedCategories"><a v-for="c in extendedQuestion[q.Id].categories" :href="c.url">{{c.name}}</a></div>
                            <div class="author"><a :href="extendedQuestion[question.Id].author.url">{{expandedQuestions[q.Id].author.name}}</a></div>
                            <div class="sources"><a v-for="r in extendedQuestion[q.Id].references" :href="r.url">{{r.name}}</a></div>
                        </div>
                        <div class="questionDetails" :data-question-id="question.Id"></div>
                        <div class="questionFooterIcons">
                            <span>{{expandedQuestions[q.Id].commentCount}}</span>
                            <i class="far fa-comment"></i>
                            <i class="fas fa-ellipsis-h"></i>
                        </div>
                    </div>
                    <div class="questionFooter">
                        <div class="questionFooterLabel">Zur Themenseite</div>
                        <div class="questionFooterLabel"><a :href="q.LinkToQuestion">Lernen</a></div>
                    </div>
                </div>
            </div>
        </div>

    </question-list-component>
</div>



  