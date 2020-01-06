<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>


<div id="QuestionListApp">
    
    <div>
        <question-list-component inline-template category-id="<%= Model.CategoryId %>" all-question-count="<%= Model.AllQuestionCount %>">
            
            <div>
                        <div class="questionListHeader">
            <div class="questionListTitle">
                <span>{{questions.length}}</span>
                <span v-if="allQuestionCountIsBiggerThanItemCount">
                    von {{allQuestionCount}}
                </span>
                <span>
                    Frage im Thema
                </span>
            </div>

            <div>
                <i class="fas fa-ellipsis-v"></i>
            </div>
        </div>

            <question-component inline-template v-for="q in questionsOnFirstPage" :question-id="q.Id" :question-title="q.Title" :question-image="q.ImageData" :knowledge-status="q.CorrectnessProbability" :is-in-wishknowldge="q.IsInWishknowledge" :url="q.LinkToQuestion">
                
                <div @click="expandQuestion()">
                    <div class="knowledgeStatus" :class="status"></div>

                    <div class="questionContainer" v-show="showFullQuestion">
                        <div class="questionHeader">
                            <div class="questionImg"></div>
                            <div class="questionTitle">{{title}}</div>
                            <div class="questionHeaderIcons">
                                <i class="fas fa-heart"></i>
                                <i class="fas fa-chevron-down"></i>
                            </div>
                        </div>
                        <div class="questionBody">
                            <div class="answer">{{answer}}</div>
                            <div class="extendedAnswer">{{extendedAnswer}}</div>
                            <div class="notes">
                                <div class="relatedCategories"><a v-for="c in categories" :href="c.url">{{c.name}}</a></div>
                                <div class="author"><a :href="author.url">{{author.name}}</a></div>
                                <div class="sources"><a v-for="r in references" :href="r.url">{{r.name}}</a></div>
                            </div>
                            <div class="questionDetails" :data-question-id="question.Id"></div>
                            <div class="questionFooterIcons">
                                <span>{{commentCount}}</span>
                                <i class="far fa-comment"></i>
                                <i class="fas fa-ellipsis-v"></i>
                            </div>
                        </div>
                        <div class="questionFooter">
                            <div class="questionFooterLabel">Zur Themenseite</div>
                            <div class="questionFooterLabel"><a :href="q.LinkToQuestion">Lernen</a></div>
                        </div>
                    </div>
                </div>

            </question-component>
            </div>



        </question-list-component>
    </div>

</div>


<%= Scripts.Render("~/bundles/js/QuestionListApp") %>


