<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<script type="text/x-template" id="pin-wuwi-template">
    <%: Html.Partial("~/Views/Shared/PinComponentVue/PinComponent.vue.ascx") %>
</script>
<%= Styles.Render("~/bundles/QuestionList") %>
<%= Styles.Render("~/bundles/switch") %>
<%= Scripts.Render("~/bundles/js/QuestionListComponents") %>
<div id="QuestionListApp" class="row">
    <div class="col-xs-12 drop-down-question-sort">
        <div class="header">Du lernst <b>{{selectedQuestionCount}}</b> Fragen aus diesem Thema ({{allQuestionsCountFromCategory}})</div>
        <div id="ButtonAndDropdown">
        <session-config-component inline-template @update="updateQuestionsCount" :questions-count="questionsCount" :all-questions-count-from-category="allQuestionsCountFromCategory">
        <div class="rootElement">
            <img id="SessionConfigReminderLeft" src="/Images/Various/SessionConfigReminderLeft.svg" >
            <img id="SessionConfigReminderRight" src="/Images/Various/SessionConfigReminder.svg" >
            <div id="CustomSessionConfigBtn" @click="openModal()" data-toggle="tooltip" data-html="true" title="<p><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>"><i class="fa fa-cog" aria-hidden="true"></i></div>
            <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="notLoggedInModal" v-show="!isLoggedIn">
                            <span class="notLoggedInHeader">Die Lernoptionen sind nur für eingeloggte Nutzer verfügbar.</span>
                            <span class="notLoggedInButton"><button class="btn btn-primary"> <%: Html.ActionLink("Kostenlos registrieren", Links.RegisterAction, Links.RegisterController) %></button></span>
                            <span class="login">Ich bin schon Nutzer!&nbsp;<a href="#" data-btn-login="true">Anmelden!</a></span>
                        </div>
                        <div class="modal-header" id="SessionConfigHeader">
                            <h5>LERNOPTIONEN</h5>
                            <h4 class="modal-title">Personalisiere dein Lernen </h4>
                        </div>
                        <div class="modal-body">
                            <transition name="fade">
                                <div class="restricted-options" v-show="!isLoggedIn && isHoveringOptions" @mouseover="isHoveringOptions = true" transition="fade">
                                </div>
                            </transition>
                            <div ref="radioSection" class="must-logged-in" :class="{'disabled-radios' : !isLoggedIn}" @mouseover="isHoveringOptions = true" @mouseleave="isHoveringOptions = false">
                                    <transition name="fade">
                                        <div v-show="!isLoggedIn && isHoveringOptions" class="blur" :style="{height: radioHeight + 'px'}"></div>
                                    </transition>
                                    <div class="modal-section-label">Prüfungsmodus&nbsp;<i class="fa fa-info-circle" aria-hidden="true"></i></div>
                                    <div class="test-mode">
                                        <div class="center">
                                            <input type="checkbox" id="cbx" style="display:none" v-model="isTestMode" />
                                            <label for="cbx" class="toggle">
                                                <span></span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="test-mode-info">
                                        Du willst es Wissen? Im Prüfungsmodus kannst Du Dein Wissen realistisch testen: zufällige Fragen ohne Antworthilfe und Wiederholungen. Viel Erfolg!
                                    </div>
                                    <div class="modal-divider"></div>
                                    <div id="CheckboxesLearnOptions" class="row">
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="AllQuestions" type="checkbox" v-model="allQuestions" :disabled="!isLoggedIn" value="False" v-if="allQuestions || !displayMinus" v-on:click="allQuestions = !allQuestions"/>
                                            <i id="AllQuestionMinus" class="fas fa-minus-square" v-if="displayMinus" v-on:click="allQuestions = !allQuestions"></i>
                                            Alle Fragen
                                        </label> <br />
                                        <label class="checkbox-label">
                                            <input id="QuestionInWishknowledge" type="checkbox" v-model="inWishknowledge" :disabled="!isLoggedIn" value="False"/>
                                            In meinem Wunschwissen
                                        </label>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="UserIsAuthor" type="checkbox" v-model="createdByCurrentUser" :disabled="!isLoggedIn" value="False"/>
                                            Von mir erstellt
                                        </label> <br />
                                        <label class="checkbox-label">
                                            <input id="IsNotQuestionInWishKnowledge" type="checkbox" v-model="isNotQuestionInWishKnowledge" :disabled="!isLoggedIn" value="False"/>
                                            Nicht in meinem Wunschwissen
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="sliders row">
                                <div class="col-sm-6">
                                    <label class="sliderLabel">Deine Antwortwahrscheinlichkeit</label>
                                    <div class="sliderContainer">
                                        <div class="leftLabel">gering</div>
                                        <div class="vueSlider">                            
                                            <vue-slider direction="ltr" :lazy="true" v-model="probabilityRange" :tooltip-formatter="percentages"></vue-slider>
                                        </div>
                                        <div class="rightLabel">hoch</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label class="sliderLabel"> Maximale Anzahl an Fragen</label>
                                    <div v-if="maxSelectableQuestionCount > 0" class="sliderContainer">
                                        <div class="leftLabel">0</div>
                                        <div class="vueSlider">                            
                                            <vue-slider v-model="selectedQuestionCount" :max="maxSelectableQuestionCount" ></vue-slider>
                                        </div>
                                        <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                                    </div>
                                    <div v-else class="alert alert-warning noQuestions" role="alert">Leider sind keine Fragen mit diesen Einstellungen verfügbar. Bitte ändere die Antwortwahrscheinlichkeit oder wähle "Alle Fragen" aus.</div>
                                    <div class="alert alert-warning" v-if="(selectedQuestionCount == 0) && maxSelectableQuestionCount > 0">Du musst mindestens 1 Frage auswählen.</div>
                                    
                                </div>
                            </div>
                            <div class="row modal-more-options">
                                <div class="more-options class= col-sm-12" @click="displayNone = !displayNone">
                                    <span>Erweiterte Optionen</span>
                                    <span class="angle">
                                        <i v-if="displayNone" class="fas fa-angle-down"></i>
                                        <i v-if="!displayNone" class="fas fa-angle-up"></i>
                                    </span>
                                </div>
                                <div id="QuestionSortSessionConfig" v-bind:class="{displayNone: displayNone, opacity: isTestMode }" class=" col-sm-12">
                                    <div class="randomQuestions">
                                        <input type="checkbox" id="randomQuestions" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="randomQuestions" />
                                        <label for="randomQuestions" class="toggle" :class="{inactive: !isLoggedIn || isTestMode}">
                                            <span :class="{inactiveSpan: !isLoggedIn || isTestMode}"></span>
                                        </label>
                                        <span>&nbsp;Zufällige Fragen<i> Erhöhe die Schwierigkeit mit zufällig vorgelegten Fragen.</i></span> 
                                    </div>
                                    
                                    <div class="answerHelp">
                                        <input type="checkbox" id="answerHelp" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="answerHelp" />
                                        <label for="answerHelp" class="toggle" :class="{inactive: !isLoggedIn}">
                                            <span :class="{inactiveSpan: !isLoggedIn}"></span>
                                        </label>
                                        <span>&nbsp;Antworthilfe<i> Die Antworthilfe zeigt dir auf Wunsch die richtige Antwort</i></span>
                                    </div>
                                    <div class="repititions">
                                        <input type="checkbox" id="repititions" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="repititions" />
                                        <label for="repititions" class="toggle" :class="{inactive: !isLoggedIn}">
                                            <span :class="{inactiveSpan: !isLoggedIn}"></span>
                                        </label>
                                        <span>&nbsp;Wiederholungen<i> Falsch gelöste Fragen werden dir zur Beantwortung erneut vorgelegt.</i></span>
                                    </div>
                                </div>
                            </div>
                            <div class="themes-info">
                                <p> Du lernst <b>{{selectedQuestionCount}}</b> Fragen aus dem Thema {{categoryName}} ({{allQuestionsCountFromCategory}})</p>
                            </div>
              <%--              <div class="row">
                                <div id="SafeLearnOptions">
                                    <div class="col-sm-12 safe-settings">
                                        <label>
                                            <input type="checkbox" id="safeOptions" v-model="safeLearningSessionOptions" :disabled="!isLoggedIn"/>
                                            Diese Einstellungen für zukünftiges Lernen speichern.(Dieses Feature ist vorbereitet und funktioniert noch nicht)
                                        </label>
                                    </div>
                                    <div class="info-options col-sm-12">
                                        Ein Neustart deiner Lernsitzung setzt deinen Lernfortschritt zurück. Die Antwortwahrscheinlichkeit der bisher beantworteten Fragen bleibt erhalten.
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                        <div class="modal-footer">
                            <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                            <div type="button" class="btn btn-primary" :class="{ 'disabled' : maxQuestionCountIsZero }" @click="loadCustomSession()"><i class="fas fa-play"></i> Anwenden</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </session-config-component>

        <div id="QuestionListHeaderDropDown" class="Button dropdown">
            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                <li><a href="<%= Links.CreateQuestion(Model.CategoryId) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i><span>Frage hinzufügen</span></a></li>
                <li @click="toggleQuestionsList()" style="cursor: pointer"><a><i class="fa fa-angle-double-down"></i><span>Alle Fragen erweitern</span></a></li>
                <li style="cursor: pointer"><a data-allowed="logged-in" @click="startNewLearningSession()"><i class="fa fa-play"></i><span>Fragen jetzt lernen</span></a></li>
            </ul>
        </div>
    </div>
</div>
    <question-list-component 
        inline-template 
        category-id="<%= Model.CategoryId %>" 
        :all-question-count="questionsCount" 
        is-admin="<%= Model.IsInstallationAdmin %>"  
        :is-question-list-to-show="isQuestionListToShow"
        :active-question ="activeQuestion"
        :selected-page-from-active-question="selectedPageFromActiveQuestion">
        <div class="col-xs-12 questionListComponent">
            <question-component inline-template
                                v-on:pin-unpin ="changePin()"
                                v-for="(q, index) in questions"
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
                                :active-question ="activeQuestion"
                                :selected-page ="selectedPage"
                                :selected-page-from-active-question="selectedPageFromActiveQuestion"
                                :length-of-questions-array="questions[0].LearningSessionStepCount"
                                :question-link-to-comment ="q.LinkToComment"
                                :link-to-edit-question ="q.LinkToEditQuestion"
                                :link-to-question-versions ="q.LinkToQuestionVersions"
                                :link-to-question ="q.LinkToQuestion">
                
                <div class="singleQuestionRow" :class="[{ open: showFullQuestion}, backgroundColor]">
                    <div class="questionSectionFlex">
                        <div class="questionContainer">
                            <div class="questionBodyTop">
                                <div class="questionImg col-xs-1" @click="expandQuestion()">
                                    <img :src="questionImage"></img>
                                </div>
                                <div class="questionContainerTopSection col-xs-11" >
                                    <div class="questionHeader row">
                                        <div class="questionTitle col-xs-9" ref="questionTitle" :id="questionTitleId" @click.self="expandQuestion()">{{questionTitle}}</div>
                                        <div class="questionHeaderIcons col-xs-3"  @click.self="expandQuestion()">
                                            <div class="iconContainer float-right" @click="expandQuestion()">
                                                <i class="fas fa-angle-down rotateIcon" :class="{ open : showFullQuestion }"></i>
                                            </div>
                                            <div>
                                                <pin-wuwi-component :is-in-wishknowledge="isInWishknowledge" :question-id="questionId" />
                                            </div>
                                            <div class="go-to-question iconContainer">
                                                <span class="fas fa-play" :class="{ 'activeQ': questionIndex === activeQuestion && selectedPageFromActiveQuestion === selectedPage }" :data-question-id="questionId" @click="loadSpecificQuestion()">
                                                </span>
                                            </div>
                                            <%----%>

                                        </div>
                                    </div>
                                    <div class="extendedQuestionContainer" v-show="showFullQuestion">
                                        <div class="questionBody">
                                            <div class="RenderedMarkdown extendedQuestion">
                                                <component :is="extendedQuestion && {template:extendedQuestion}"></component>
                                            </div>
                                            <div class="answer">
                                                Richtige Antwort: <component :is="answer && {template:answer}"></component>
                                            </div>
                                            <div class="extendedAnswer" v-if="extendedAnswer.length > 11">
                                                <strong>Ergänzungen zur Antwort:</strong><br/>
                                                <component :is="extendedAnswer && {template:extendedAnswer}"></component>
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
                                <div class="questionFooterIcons">
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
                                            <li v-if="isAdmin == 'True' || isCreator"><a :href="linkToEditQuestion" data-allowed="logged-in"><i class="fa fa-code-fork"></i><span>Frage bearbeiten</span></a></li>
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






