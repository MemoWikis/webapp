<question-component inline-template
                    v-on:pin-unpin ="changePin()"
                    :question-id="q.Id" 
                    :question-title="q.Title" 
                    :question-image="q.ImageData" 
                    :knowledge-state="q.CorrectnessProbability" 
                    :is-in-wishknowledge="q.IsInWishknowledge" 
                    :url="q.LinkToQuestion" 
                    :has-personal-answer="q.HasPersonalAnswer" 
                    :is-admin="isAdmin"
                    :is-question-list-to-show ="isQuestionListToShow"
                    :question-index="index"
                    :active-question-id="activeQuestionId"
                    :selected-page ="selectedPage"
                    :selected-page-from-active-question="selectedPageFromActiveQuestion"
                    :length-of-questions-array="questions[0].LearningSessionStepCount"
                    :question-link-to-comment ="q.LinkToComment"
                    :link-to-edit-question ="q.LinkToEditQuestion"
                    :link-to-question-versions ="q.LinkToQuestionVersions"
                    :link-to-question ="q.LinkToQuestion"
                    :key="q.Id"
                    :session-index="q.SessionIndex"
                    :is-last-item="index == (questions.length-1)"
                    :visibility="q.Visibility">
    
    <div class="singleQuestionRow" :class="[{ open: showFullQuestion}, backgroundColor]">
        <div class="questionSectionFlex">
            <div class="questionContainer">
                <div class="questionBodyTop">
                    <div class="questionImg col-xs-1" @click="expandQuestion()">
                        <img :src="questionImage"></img>
                    </div>
                    <div class="questionContainerTopSection col-xs-11" >
                        <div class="questionHeader row">
                            <div class="questionTitle col-xs-9" ref="questionTitle" :id="questionTitleId" @click="expandQuestion()">
                                <component :is="questionTitleHtml && {template:questionTitleHtml}" @hook:mounted="highlightCode(questionTitleId)" ></component>
                                <div v-if="visibility == 1" class="privateQuestionIcon">
                                    <p>
                                        <i class="fas fa-lock"></i>
                                    </p>
                                </div>
                            </div>
                            <div class="questionHeaderIcons col-xs-3"  @click.self="expandQuestion()">
                                <div class="iconContainer float-right" @click="expandQuestion()">
                                    <i class="fas fa-angle-down rotateIcon" :class="{ open : showFullQuestion }"></i>
                                </div>
                                <div>
                                    <pin-wuwi-component :is-in-wishknowledge="isInWishknowledge" :question-id="questionId" />
                                </div>
                                <div class="go-to-question iconContainer">
                                    <span class="fas fa-play" :class="{ 'activeQ': activeQuestionId == questionId }" :data-question-id="questionId" @click="loadSpecificQuestion()">
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                            <div class="questionBody">
                                <div class="RenderedMarkdown extendedQuestion" :id="extendedQuestionId">
                                    <component :is="extendedQuestion && {template:extendedQuestion}" @hook:mounted="highlightCode(extendedQuestionId)"></component>
                                </div>
                                <div class="answer" :id="answerId">
                                    Richtige Antwort: <component :is="answer && {template:answer}" @hook:mounted="highlightCode(answerId)"></component>
                                </div>
                                <div class="extendedAnswer" v-if="extendedAnswer.length > 11" :id="extendedAnswerId">
                                    <strong>Ergänzungen zur Antwort:</strong><br/>
                                    <component :is="extendedAnswer && {template:extendedAnswer}" @hook:mounted="highlightCode(extendedAnswerId)"></component>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="questionBodyBottom" v-show="showFullQuestion">
                    <div class="questionStats questionStatsInQuestionList">
                        <div class="probabilitySection">
                            <span class="percentageLabel" :class="backgroundColor">{{correctnessProbability}}&nbsp;</span> 
                            <span class="chip" :class="backgroundColor">{{correctnessProbabilityLabel}}</span>
                        </div>
                        <div class="answerCountFooter">{{answerCount}}&nbsp;mal&nbsp;beantwortet&nbsp;|&nbsp;{{correctAnswers}}&nbsp;richtig&nbsp;/&nbsp;{{wrongAnswers}}&nbsp;falsch</div>
                    </div>
                    <div id="QuestionFooterIcons" class="questionFooterIcons">
                        <div>
                            <a class="commentIcon" :href="questionLinkToComment">
                                <i class="fa fa-comment"><span style="font-weight: 400;">&nbsp;{{commentCount}}</span></i>
                            </a>
                        </div>
                        <div class="Button dropdown">
                            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <i class="fa fa-ellipsis-v"></i>
                            </a>
                            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                                <li v-if="isAdmin == 'True' || isCreator"><a :href="linkToEditQuestion" data-allowed="logged-in"><i class="fa fa-pen"></i><span>Frage bearbeiten</span></a></li>
                                <li style="cursor: pointer"><a :href="linkToQuestion"><i class="fas fa-file"></i><span>Frageseite anzeigen</span></a></li>
                                <li><a :href="linkToQuestionVersions" data-allowed="logged-in"><i class="fa fa-code-fork"></i><span>Bearbeitungshistorie der Frage</span></a></li>
                                <li style="cursor: pointer"><a :href="questionLinkToComment"><i class="fas fa-comment"></i><span>Frage kommentieren</span></a></li>
                                <li v-if="isAdmin == 'True'">
                                    <a data-toggle="modal" :data-questionid="questionId" href="#modalDeleteQuestion">
                                        <i class="fas fa-trash"></i><span>Frage löschen</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</question-component>