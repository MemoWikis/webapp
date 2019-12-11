<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListController>" %>

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

        <div v-for="question in questions" @click="expandQuestion()">
            <div class="knowledgeStatus" :class="status"></div>

            <div class="questionContainer" v-show="showFullQuestion">
                <div class="questionHeader">
                    <div class="questionImg"></div>
                    <div class="questionTitle">{{question.title}}</div>
                    <div class="questionHeaderIcons">
                        <i class="fas fa-heart"></i>
                        <i class="fas fa-chevron-down"></i>
                    </div>
                </div>
                <div class="questionBody">
                    <div class="answer">{{question.answer}}</div>
                    <div class="extendedAnswer">{{question.extendedAnswer}}</div>
                    <div class="notes">
                        <div class="relatedCategories"><a v-for="category in question.categories" :href="category.url">{{category}}</a></div>
                        <div class="author"><a :href="author.url">{{author}}</a></div>
                        <div class="sources"><a v-for="source in question.sources"></a></div>
                    </div>
                    <div class="questionDetails" :data-question-id="question.Id"></div>
                    <div class="questionFooterIcons">
                        <span>{{question.commentCount}}</span>
                        <i class="far fa-comment"></i>
                        <i class="fas fa-ellipsis-h"></i>
                    </div>
                </div>
                <div class="questionFooter"></div>
            </div>
        </div>

    </question-list-component>
</div>



  