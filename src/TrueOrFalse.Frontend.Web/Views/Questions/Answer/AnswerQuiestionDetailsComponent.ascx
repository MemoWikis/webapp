<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<question-details-component>
    
    <div v-if="showTopBorder"></div>
    <div id="questionDetailsContainer">
        <div id="questionStatistics">
            <div id="probabilityContainer">
                <div id="semiPieChart"></div>
                <div id="probabilityText">
                    <div v-if="isLoggedIn"></div>
                    <div v-else></div>
                </div>
                <div id="probabilityState" :class="state"></div>
            </div>
            <div id="counterContainer"></div>
        </div>
        
        <div id="categoryList" v-if="showCategoryList"></div>
    </div>
</question-details-component>