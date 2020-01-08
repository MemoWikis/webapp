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
                        <span v-if="questions.length < allQuestionCount">
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

            <question-component inline-template v-for="q in questions" :question-id="q.Id" :question-title="q.Title" :question-image="q.ImageData" :knowledge-state="q.CorrectnessProbability" :is-in-wishknowldge="q.IsInWishknowledge" :url="q.LinkToQuestion">
                
                <div class="singleQuestionRow" @click="expandQuestion()">
                    <div class="knowledgeStatus" :class="state"></div>
                    <div class="questionContainer">
                        <div class="questionHeader">
                            <div class="questionImg">
                                <img src="questionImage.Url"></img>
                            </div>
                            <div class="questionTitle">{{questionTitle}}</div>
                            <div class="questionHeaderIcons">
                                <i class="fas fa-heart"></i>
                                <i class="fas fa-chevron-down"></i>
                            </div>
                        </div>
                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                            <div class="questionBody">
                                <div class="extendedQuestion"></div>

                                <div class="answer">
                                    <strong>Antwort:</strong><br/>
                                    {{answer}}
                                </div>
                                <div class="extendedAnswer" v-if="extendedAnswer != null && extendedAnswer.length > 0">
                                    <strong>Ergänzungen zur Antwort:</strong><br/>
                                    {{extendedAnswer}}
                                </div>
                                <div class="notes">
                                    <div class="relatedCategories"><a v-for="c in categories" :href="c.url">{{c.name}}, </a></div>
                                    <div class="author"><a :href="author.url">{{author.name}}</a></div>
                                    <div class="sources"><a v-for="r in references" :href="r.url">{{r.name}}</a></div>
                                </div>
                                <div class="questionDetails">
                                    <div class="questionCorrectnessProbability">
                                        <span>{{correctnessProbability}} Wahrscheinlichkeit, dass du diese Frage richtig beantwortest</span>
                                    </div>
                                    <div class="questionTotalAnswers">{{questionDetails.totalAnswers}}</div>
                                    <div class="questionPersonalAnswers" v-if="isLoggedIn">{{questionDetails.personalAnswers}}</div>
                                    <div class="questionViews">{{questionDetails.views}} <span></span></div>
                                </div>
                                <div class="questionFooterIcons">
                                    <span>{{commentCount}}</span>
                                    <i class="far fa-comment"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </div>
                            </div>
                            <div class="questionFooter">
                                <div class="questionFooterLabel">Zur Themenseite</div>
                                <div class="questionFooterLabel"><a :href="url">Lernen</a></div>
                            </div>
                        </div>

                    </div>
                </div>

            </question-component>
            <span v-for="(p, key) in pageArray" @Click="loadQuestions(p)"> [{{p}}] </span>

            </div>



        </question-list-component>
    </div>

</div>


<%= Scripts.Render("~/bundles/js/QuestionListApp") %>


