<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import * as d3 from 'd3'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { dom } from '@fortawesome/fontawesome-svg-core'
import { KnowledgeStatus } from '../knowledgeStatusEnum'
import { AnswerQuestionDetailsResult } from './answerQuestionDetailsResult'

const userStore = useUserStore()
const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
const { getTimeElapsedAsText } = useTimeElapsed()

interface Props {
    model: AnswerQuestionDetailsResult
}
const props = defineProps<Props>()

onMounted(() => {
    dom.watch()
    initData(props.model)
})
const personalProbabilityText = ref(t('questionLandingPage.probability.status.notInWishKnowledge'))

const arcSvg = ref<any>({})
const personalCounterSvg = ref<any>({})
const overallCounterSvg = ref<any>({})

const baseArcData = ref({
    startAngle: -0.55 * Math.PI,
    endAngle: 0.55 * Math.PI,
    innerRadius: 45,
    outerRadius: 50,
    fill: "#DDDDDD",
    class: "baseArc",
})

interface ArcData {
    startAngle: number,
    endAngle: number,
    innerRadius: number,
    outerRadius: number,
    fill: string,
    class?: string,
}

const personalArcData = ref<ArcData>({
    startAngle: -0.55 * Math.PI,
    endAngle: (-0.55 + props.model.personalProbability / 100 * 1.1) * Math.PI,
    innerRadius: 40,
    outerRadius: 55,
    fill: props.model.personalColor,
    class: "personalArc",
})

const avgArcData = ref<ArcData>({
    startAngle: (-0.55 + props.model.avgProbability / 100 * 1.1) * Math.PI - 0.01,
    endAngle: (-0.55 + props.model.avgProbability / 100 * 1.1) * Math.PI + 0.01,
    innerRadius: 37.5,
    outerRadius: 57.5,
    fill: "#707070",
    class: "avgArc"
})

const arcLoaded = ref(false)
const percentageLabelWidth = ref(0)
const avgAngle = ref(0)
const dxAvgLabel = ref(0)
const dyAvgLabel = ref(0)
const avgLabelAnchor = ref("middle")
const avgProbabilityLabelWidth = ref(0)
const showPersonalArc = ref(false)
const personalStartAngle = ref(0)
const overallStartAngle = ref(0)

const baseCounterData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#DDDDDD",
})

const personalWrongAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#FFA07A",
    class: "personalWrongAnswerCounter",
})
const personalCorrectAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#AFD534",
    class: "personalCorrectAnswerCounter",
})

const overallWrongAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#FFA07A",
    class: "overallWrongAnswerCounter",
})
const overallCorrectAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#AFD534",
    class: "overallCorrectAnswerCounter",
})

function setPersonalProbability() {
    switch (props.model.knowledgeStatus) {
        case KnowledgeStatus.Solid:
            personalProbabilityText.value = t('questionLandingPage.probability.status.solid')
            break
        case KnowledgeStatus.NeedsConsolidation:
            personalProbabilityText.value = t('questionLandingPage.probability.status.needsConsolidation')
            break
        case KnowledgeStatus.NeedsLearning:
            personalProbabilityText.value = t('questionLandingPage.probability.status.needsLearning')
            break
        default:
            personalProbabilityText.value = t('questionLandingPage.probability.status.notLearned')
    }
}

function setPersonalArcData() {
    personalArcData.value = {
        startAngle: -0.55 * Math.PI,
        endAngle: (-0.55 + props.model.personalProbability / 100 * 1.1) * Math.PI,
        innerRadius: 40,
        outerRadius: 55,
        fill: props.model.personalColor,
        class: "personalArc",
    }
}

function setAvgArcData() {
    var avgInnerRadius = 37.5
    var avgOuterRadius = 57.5

    if (props.model.personalProbability < props.model.avgProbability) {
        avgInnerRadius = 42.5
        avgOuterRadius = 52.5
    }

    avgArcData.value = {
        startAngle: (-0.55 + props.model.avgProbability / 100 * 1.1) * Math.PI - 0.01,
        endAngle: (-0.55 + props.model.avgProbability / 100 * 1.1) * Math.PI + 0.01,
        innerRadius: avgInnerRadius,
        outerRadius: avgOuterRadius,
        fill: "#707070",
        class: "avgArc"
    }

    avgAngle.value = (-0.55 + props.model.avgProbability / 100 * 1.1) * Math.PI
}

function setPersonalCounterData() {
    personalWrongAnswerCountData.value = {
        startAngle: 0,
        endAngle: (personalStartAngle.value / 100 * 1) * Math.PI * 2,
        innerRadius: 20,
        outerRadius: 25,
        fill: "#FFA07A",
        class: "personalWrongAnswerCounter",
    }

    personalCorrectAnswerCountData.value = {
        startAngle: (personalStartAngle.value / 100 * 1) * Math.PI * 2,
        endAngle: 2 * Math.PI,
        innerRadius: 20,
        outerRadius: 25,
        fill: "#AFD534",
        class: "personalCorrectAnswerCounter",
    }
}

function setOverallCounterData() {

    overallWrongAnswerCountData.value = {
        startAngle: 0,
        endAngle: (overallStartAngle.value / 100 * 1) * Math.PI * 2,
        innerRadius: 20,
        outerRadius: 25,
        fill: "#FFA07A",
        class: "overallWrongAnswerCounter",
    }

    overallCorrectAnswerCountData.value = {
        startAngle: (overallStartAngle.value / 100 * 1) * Math.PI * 2,
        endAngle: 2 * Math.PI,
        innerRadius: 20,
        outerRadius: 25,
        fill: "#AFD534",
        class: "overallCorrectAnswerCounter",
    }
}

function calculateLabelWidth() {
    let probabilityLabelWidth = 0
    var probabilityAsText = [props.model.personalProbability]

    arcSvg.value.append('g')
        .selectAll('.dummyProbability')
        .data(probabilityAsText)
        .enter()
        .append("text")
        .attr("font-family", "font-family:'Open Sans'")
        .attr("font-weight", "bold")
        .attr("font-size", "30px")
        .text(props.model.personalProbability)
        .each(function (this: any) {
            let thisWidth = this.getComputedTextLength()
            probabilityLabelWidth = thisWidth
            this.remove()
        })

    arcSvg.value.append('g')
        .selectAll('.dummyPercentage')
        .data("%")
        .enter()
        .append("text")
        .attr("font-family", "font-family:'Open Sans'")
        .attr("font-weight", "medium")
        .attr("font-size", "18px")
        .text((d: any) => d)
        .each(function (this: any) {
            let thisWidth = 0
            percentageLabelWidth.value = thisWidth
            this.remove()
        })

    return probabilityLabelWidth + percentageLabelWidth.value + 1
}

const personalCounter = ref<SVGSVGElement | null>(null)
const overallCounter = ref<SVGSVGElement | null>(null)

function angleIsNaN(a: ArcData) {
    return isNaN(a.startAngle) || isNaN(a.endAngle)
}

function drawCounterArcs() {
    if (!personalCounter.value || !overallCounter.value || angleIsNaN(personalWrongAnswerCountData.value) || angleIsNaN(personalCorrectAnswerCountData.value))
        return
    const arc = d3.arc()

    const personalCounterData = [
        baseCounterData.value,
        personalWrongAnswerCountData.value,
        personalCorrectAnswerCountData.value
    ]

    personalCounterSvg.value = d3.select(personalCounter.value)
        .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")")

    personalCounterSvg.value.selectAll("path")
        .data(personalCounterData)
        .enter()
        .append("path")
        .style("fill", (d: any) => d.fill)
        .attr("class", (d: any) => d.class)
        .attr("d", arc)

    personalCounterSvg.value.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
        .style("visibility", () => props.model.personalAnswerCount > 0 ? "visible" : "hidden")

    personalCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', '14px')
        .attr('x', -7)
        .attr('y', -8)
        .html(() => {
            var fontColor = props.model.personalAnswerCount > 0 ? "#999999" : "#DDDDDD"
            return "<i class='fa-solid fa-user' style='font-size:16px; color:" + fontColor + "'> </i>"
        })

    var overallCounterData = [
        baseCounterData.value,
        overallWrongAnswerCountData.value,
        overallCorrectAnswerCountData.value
    ]

    overallCounterSvg.value = d3.select(overallCounter.value)
        .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")")

    overallCounterSvg.value.selectAll("path")
        .data(overallCounterData)
        .enter()
        .append("path")
        .style("fill", (d: any) => d.fill)
        .attr("class", (d: any) => d.class)
        .attr("d", arc)

    overallCounterSvg.value.selectAll(".overallWrongAnswerCounter, .overallCorrectAnswerCounter")
        .style("visibility", () => props.model.overallAnswerCount > 0 ? "visible" : "hidden")

    overallCounterSvg.value.selectAll("i")
        .style("color", () => {
            return props.model.overallAnswerCount > 0 ? "#999999" : "#DDDDDD"
        })

    overallCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', props.model.visibility === Visibility.Private ? '14px' : '20px')
        .attr('x', props.model.visibility === Visibility.Private ? -7 : -10)
        .attr('y', -8)
        .html(() => {
            var fontColor = props.model.overallAnswerCount > 0 ? "#999999" : "#DDDDDD"
            if (props.model.visibility === Visibility.Private)
                return "<i class='fa-solid fa-lock' style='font-size:16px; color:" + fontColor + "'> </i>"
            else
                return "<i class='fa-solid fa-users' style='font-size:16px; color:" + fontColor + "'> </i>"

        })
}

function drawProbabilityLabel() {
    var labelWidth = calculateLabelWidth()

    arcSvg.value.append("svg:text")
        .attr("dy", ".1em")
        .attr("dx", -(labelWidth / 2) - 5 + "px")
        .attr("text-anchor", "left")
        .attr("style", "font-family:'Open Sans'")
        .attr("font-size", "30")
        .attr("font-weight", "bold")
        .style("fill", () => showPersonalArc.value ? props.model.personalColor : "#DDDDDD")
        .attr("class", "personalProbabilityLabel")
        .text(() => props.model.personalAnswerCount > 0 ? props.model.personalProbability : props.model.avgProbability)

    arcSvg.value.append("svg:text")
        .attr("dy", "-.35em")
        .attr("dx", (labelWidth / 2) - percentageLabelWidth.value - 5 + "px")
        .attr("style", "font-family:'Open Sans'")
        .attr("text-anchor", "left")
        .attr("font-size", "18")
        .attr("font-weight", "medium")
        .attr("class", "percentageLabel")
        .style("fill", () => showPersonalArc.value ? props.model.personalColor : "#DDDDDD")
        .text("%")

    arcSvg.value.append("svg:rect")
        .attr("class", "personalProbabilityChip")
        .attr("rx", 10)
        .attr("ry", 10)
        .attr("y", 20)
        .attr("height", 20)
        .style("fill", props.model.personalColor)
        .style("visibility", () => {
            return userStore.isLoggedIn ? "visible" : "hidden"
        })
        .attr("transform", "translate(0,0)")

    var textWidth = 0
    arcSvg.value
        .append("svg:text")
        .attr("dy", "33.5")
        .attr("style", "font-family:Open Sans")
        .attr("text-anchor", "middle")
        .attr("font-size", "10")
        .attr("font-weight", "medium")
        .attr("class", "personalProbabilityText")
        .style("fill", () => props.model.personalColor === "#999999" ? "white" : "#555555")
        .attr("transform", "translate(0,0)")
        .text(personalProbabilityText.value)
        .each(function (this: any) {
            textWidth = this.getComputedTextLength()
        })

    arcSvg.value.selectAll(".personalProbabilityChip")
        .attr("x", - textWidth / 2 - 11)
        .attr("width", textWidth + 22)

    arcSvg.value.selectAll(".personalProbabilityChip,.personalProbabilityText")
        .style("visibility", () => (userStore.isLoggedIn && props.model.overallAnswerCount > 0) ? "visible" : "hidden")

}

function setAvgLabelPos() {
    var probabilityData = [props.model.avgProbability]

    arcSvg.value.append('g')
        .selectAll('.dummyAvgProbability')
        .data(probabilityData)
        .enter()
        .append("text")
        .attr("font-family", "font-family:'Open Sans'")
        .attr("font-weight", "regular")
        .attr("font-size", "12")
        .text((d: any) => "∅ " + d + "%")
        .each(function (this: any) {
            var thisWidth = this.getComputedTextLength()
            avgProbabilityLabelWidth.value = thisWidth
            this.remove()
        })

    var el = (props.model.avgProbability - 50) / 10
    dyAvgLabel.value = (0.20 * Math.pow(el, 2) - 5) * 2 + .25 * (Math.pow(el, 2))

    dxAvgLabel.value = 0

    if (props.model.avgProbability > 50) {
        if (props.model.avgProbability < 80)
            dxAvgLabel.value = -(80 - props.model.avgProbability) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = - (20 - props.model.avgProbability) * 6 / 100
        avgLabelAnchor.value = "start"
    }
    else if (props.model.avgProbability < 50) {
        if (props.model.avgProbability > 20)
            dxAvgLabel.value = (props.model.avgProbability - 20) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = (props.model.avgProbability - 80) * 6 / 100
        avgLabelAnchor.value = "end"
    }
    else if (props.model.avgProbability === 50) {
        avgLabelAnchor.value = "middle"
    }
}

function setAvgLabel() {

    setAvgLabelPos()

    var pos = d3.arc()
        .innerRadius(55)
        .outerRadius(55)
        .startAngle(avgAngle.value)
        .endAngle(avgAngle.value)

    arcSvg.value.append("svg:text")
        .attr("transform", (d: any) => "translate(" + pos.centroid(d) + ")")
        .attr("dx", dxAvgLabel.value)
        .attr("dy", dyAvgLabel.value)
        .attr("text-anchor", avgLabelAnchor.value)
        .attr("style", "font-family:'Open Sans'")
        .attr("font-size", "12")
        .attr("font-weight", "regular")
        .style("fill", "#555555")
        .style("opacity", 1.0)
        .attr("class", "avgProbabilityLabel")
        .text("∅ " + props.model.avgProbability + "%")
}

const semiPie = ref()
function drawArc() {

    var width = 200
    var height = 130

    arcSvg.value = d3.select(semiPie.value).append("g").attr("transform", "translate(" + width / 2 + "," + (height - 50) + ")")

    var arc = d3.arc()

    var data = [
        baseArcData.value,
        personalArcData.value,
        avgArcData.value
    ]

    arcSvg.value.selectAll("path").data(data).enter()
        .append("path")
        .style("fill", (d: any) => d.fill)
        .attr("class", (d: any) => d.class)
        .attr("d", arc)

    arcSvg.value.selectAll(".personalArc")
        .style("visibility", () => {
            return showPersonalArc.value ? "visible" : "hidden"
        })

    drawProbabilityLabel()
    setAvgLabel()

    arcLoaded.value = true
}

function initData(e: AnswerQuestionDetailsResult) {
    setPersonalProbability()
    setPersonalArcData()

    setAvgArcData()

    setPersonalCounterData()
    setOverallCounterData()

    if (props.model.personalAnswerCount > 0)
        showPersonalArc.value = true

    personalStartAngle.value = 100 - (100 / props.model.personalAnswerCount * props.model.personalAnsweredCorrectly)
    overallStartAngle.value = 100 - (100 / props.model.overallAnswerCount * props.model.overallAnsweredCorrectly)

    drawArc()
    drawCounterArcs()
}

</script>

<template>
    <div>
        <div id="ExtendedQuestionDetails">
            <div class="separationBorderTop" style="min-height: 20px;"></div>

            <div id="questionDetailsContainer" class="row" style="min-height:265px">
                <div id="pageList" class="col-sm-5" :class="{ isLandingPage: 'isLandingPage' }">
                    <div class="overline-s no-line">{{ t('questionLandingPage.pages') }}</div>
                    <div class="pageListChips">
                        <div style="display: flex; flex-wrap: wrap;">
                            <PageChip v-for="(t, index) in model.pages" :key="t.id + index" :page="t" :index="index"
                                :is-spoiler="false" />
                        </div>
                    </div>
                </div>
                <div id="questionStatistics" class="col-sm-7">
                    <div id="probabilityContainer" class="" ref="probabilityContainer">
                        <div class="overline-s no-line">{{ t('questionLandingPage.probability.title') }}</div>
                        <div id="semiPieSection">
                            <div id="semiPieChart" style="min-height:130px">
                                <svg class="semiPieSvgContainer" ref="semiPie" width="200" height="130"
                                    :class="{ 'isInWishKnowledge': model.isInWishKnowledge }">
                                </svg>
                            </div>
                            <div id="probabilityText">
                                <div v-if="userStore.isLoggedIn" style="">
                                    <strong>{{ model.personalProbability }}%</strong> {{ t('questionLandingPage.probability.description.loggedIn') }}
                                    <strong>{{ model.avgProbability }}%</strong>
                                </div>
                                <div v-else style="">
                                    <strong>{{ model.personalProbability }}%</strong> {{ t('questionLandingPage.probability.description.notLoggedIn') }}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="counterContainer" class="" style="font-size:12px">
                        <div class="overline-s no-line">{{ t('questionLandingPage.answers.title') }}</div>

                        <div class="counterBody">
                            <div class="counterHalf">
                                <svg ref="personalCounter" style="min-width:50px" width="50" height="50"></svg>
                                <div v-if="model.personalAnswerCount > 0" class="counterLabel">
                                    {{ t('questionLandingPage.answers.personal.title') }} <br />
                                    <strong>{{ getFormattedNumber(model.personalAnswerCount) }}</strong> {{ t('questionLandingPage.answers.personal.answered') }}
                                    <br />
                                    <strong>{{ getFormattedNumber(model.personalAnsweredCorrectly) }}</strong> {{ t('questionLandingPage.answers.personal.correct') }} /
                                    <strong>{{ getFormattedNumber(model.personalAnsweredWrongly) }}</strong>
                                    {{ t('questionLandingPage.answers.personal.wrong') }}
                                </div>
                                <div v-else-if="userStore.isLoggedIn" class="counterLabel">
                                    {{ t('questionLandingPage.answers.personal.noAnswers') }}
                                </div>
                                <div v-else class="counterLabel">
                                    {{ t('questionLandingPage.answers.personal.notLoggedIn') }} <button class="btn-link"
                                        @click="userStore.openLoginModal()">{{ t('questionLandingPage.answers.personal.login') }}</button>
                                </div>
                            </div>
                            <div class="counterHalf">
                                <svg ref="overallCounter" style="min-width:50px" width="50" height="50"></svg>
                                <div v-if="model.overallAnswerCount > 0" class="counterLabel">
                                    {{ t('questionLandingPage.answers.overall.title') }} <br />
                                    <strong>{{ getFormattedNumber(model.overallAnswerCount) }}</strong> {{ t('questionLandingPage.answers.overall.answered') }}
                                    <br />
                                    <strong>{{ getFormattedNumber(model.overallAnsweredCorrectly) }}</strong> {{ t('questionLandingPage.answers.overall.correct') }} /
                                    <strong>{{ getFormattedNumber(model.overallAnsweredWrongly) }}</strong> {{ t('questionLandingPage.answers.overall.wrong') }}
                                </div>
                                <div v-else class="counterLabel">
                                    <template v-if="model.visibility === 1">
                                        {{ t('questionLandingPage.answers.overall.privateQuestion') }}
                                    </template>
                                    <template v-else>
                                        {{ t('questionLandingPage.answers.overall.noAnswers') }}
                                    </template>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="separationBorderTop" style="min-height: 10px;"></div>
        </div>
        <div id="QuestionDetailsFooter">
            <div class="questionDetailsFooterPartialLeft">
                <LicenseLink :licenseId="model.licenseId" :creator="model.creator" />
                <div class="created"> {{ t('questionLandingPage.creation.by') }}
                    <NuxtLink v-if="model.creator.id > 0" :to="$urlHelper.getUserUrl(model.creator.name, model.creator.id)">
                        &nbsp;{{ model.creator.name }}&nbsp;
                    </NuxtLink>
                    {{ getTimeElapsedAsText(model.creationDate) }}
                </div>
            </div>

            <div class="questionDetailsFooterPartialRight">
                <div class="wishknowledgeCount">
                    <font-awesome-icon icon="fa-solid fa-heart" />
                    <span class="detail-label">
                        {{ model.wishknowledgeCount }}
                    </span>
                </div>

                <div class="viewCount">
                    <font-awesome-icon icon="fa-solid fa-eye" />
                    <span class="detail-label">
                        {{ model.totalViewCount }}
                    </span>
                </div>
            </div>
        </div>
        <LicenseLinkModal />
    </div>
</template>

<style lang="less" scoped>
// Imports
@import (reference) '~~/assets/includes/imports.less';

//Variables

@font-size12: 12px;

// Mixins

.font-size12 {
    font-size: @font-size12;
}

.SetAndpage {
    margin-right: 10px;
}

.correctness-prohability {
    margin-left: 10px;
}

//Code

.question-details {
    color: @grey6x5;
    font-size: 12px;

    .show-tooltip {
        margin-right: 5px;
    }

    .page-set {
        display: flex;

        .label-page {
            .SetAndpage();
        }

        .label-set {
            .SetAndpage();
        }

        #page {
            .font-size12();
        }

        .fa-chevron-right {
            margin-top: 4px;
        }
    }

    .second-row {
        padding-left: 0px;
        display: flex;

        .media-below-lg {
            padding-left: 10px;
            padding-top: 10px;
        }

        .questionDetailsStatistic {
            padding-left: 10px;
            padding-right: 10px;

            .question-details-row {
                min-height: 24px;
                display: flex;
                line-height: 18px;
                padding: 4px 0;

                span,
                .question-details-label-double {
                    padding-top: 2px;
                    max-width: 300px;
                }

                .question-details-label-double {
                    padding-top: 4px;

                    span {
                        padding-top: 0;
                    }
                }

                .detail-icon-container {
                    min-width: 56px;
                    width: 56px;
                    display: flex;
                    justify-content: center;
                    text-align: center;
                    font-weight: bold;
                    font-size: 18px;

                    .show-tooltip {
                        margin: unset;
                    }

                    div {
                        margin: auto;

                        span {
                            top: 2px;

                            &.Pin {

                                .iAdded,
                                .iAddedNot,
                                .iAddSpinner {
                                    font-size: 18px;
                                    padding: 0;
                                    line-height: 18px;
                                    height: unset;
                                    border: 0;

                                    i {
                                        font-size: 18px;
                                        height: unset;
                                        line-height: unset;
                                    }
                                }
                            }
                        }
                    }


                    &.pie-chart {
                        font-size: 13px;
                    }

                    i {
                        padding: unset;

                        &.fa-heart {
                            color: #B13A48;
                        }
                    }
                }
            }
        }
    }

    .seen {
        padding-right: 10px;
        margin-left: 25px;
    }

    .learning-status {
        margin-right: 25px;
    }
}

.text-sponsor {
    text-align: center;
}

.model-pages {
    padding-top: 0.375em;
}

#ChipHeader,
#StatsHeader {
    padding-top: 5px;
    padding-bottom: 5px;
    font-size: 14px;
}

@media (min-width: 1200px) {
    #StatsHeader {
        display: none;
    }
}

#ExtendedQuestionDetails {
    .separationBorderTop {
        min-height: 20px;
    }

    #questionDetailsContainer {
        padding-left: 15px;
        padding-right: 12px;

        @media(max-width:767px) {
            display: flex;
            flex-wrap: wrap;
        }

        .sectionLabel {
            text-transform: uppercase;
            font-family: 'Open Sans';
            font-size: 12px;
            font-weight: 600;
        }


        #pageList {
            padding-bottom: 30px;

            .pageListChips {
                @media(max-width:479px) {
                    padding-top: 10px;
                }

                overflow: hidden
            }

            .pageListLinks {
                padding: 10px 0;
                font-size: 12px;

                a {
                    line-height: 20px;
                    padding-right: 5px;
                }
            }
        }

        #questionStatistics {

            #counterContainer {
                padding-bottom: 12px;

                .counterBody {
                    display: flex;
                    flex-wrap: wrap;

                    @media(max-width:767px) {
                        padding-bottom: 20px;
                    }

                    .counterHalf {
                        display: flex;
                        justify-content: center;
                        padding-top: 30px;

                        @media(max-width:595px) {
                            padding-top: 20px;
                            padding-left: 20px;
                        }

                        .counterLabel {
                            width: 100%;
                            padding: 0 0 0 20px;
                            text-wrap: nowrap;

                            @media (max-width:595px) {
                                max-width: 180px;
                                padding: 0 20px;
                            }

                            a:hover {
                                cursor: pointer;
                            }

                            button.btn-link {
                                padding: 0px;
                            }
                        }
                    }
                }
            }

            @media(min-width:596px) {
                display: flex;
                justify-content: space-around;
            }

            #probabilityContainer {
                min-width: 220px;
                margin-right: 20px;

                #semiPieSection {

                    @media (max-width:767px) {
                        display: flex;
                        flex-wrap: wrap;
                    }

                    @media (max-width:595px) {
                        padding-bottom: 10px;
                    }

                    #semiPieChart {
                        display: flex;
                        justify-content: center;
                        align-items: center;

                        .semiPieSvgContainer {
                            display: flex;
                            justify-content: center;

                            @media(max-width:767px) {
                                justify-content: normal;
                            }

                            @media(min-width:350px) and (max-width:767px) {

                                svg {
                                    width: 196px;

                                    g {
                                        transform: translate(86px, 80px);
                                    }
                                }
                            }

                            @media(max-width:349px) {

                                svg {
                                    width: 140px;
                                    transition: 0.2s;

                                    g {
                                        transform: translate(66px, 50px);
                                        transition: 0.2s;

                                        path.baseArc,
                                        path.personalArc,
                                        path.avgArc,
                                        text.avgProbabilityLabel {
                                            display: none;
                                        }
                                    }
                                }

                                &.isInWishKnowledge {

                                    svg {
                                        width: 100px;
                                        transition: 0.8s ease-in;

                                        g {
                                            transform: translate(50px, 50px);
                                            transition: 0.8s ease-in;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #probabilityText {
                        display: flex;
                        justify-content: center;
                        padding-bottom: 20px;
                        text-align: center;
                        font-size: 12px;
                        max-width: 300px;
                        align-items: center;

                        @media(max-width:767px) {
                            padding-top: 25px;
                            text-align: left;
                        }
                    }
                }
            }
        }
    }
}

#QuestionDetailsFooter {
    display: flex;
    justify-content: space-between;
    color: @memo-grey-dark;

    .questionDetailsFooterPartialLeft {
        width: 60%;
        display: flex;
        flex-wrap: wrap;
        padding-left: 15px;
        gap: 1rem;

        #LicenseQuestion {
            padding-right: 10px;

            .TextLinkWithIcon {
                img {
                    margin: 0 8px 0 0;
                }

                align-items: center;
                flex-wrap: nowrap;
                flex-direction: unset;

                .license-info {
                    padding-left: 4px;
                }
            }
        }
    }

    .questionDetailsFooterPartialRight {
        width: 40%;
        display: flex;
        justify-content: flex-end;
        padding-right: 4px;
        align-items: center;

        .wishknowledgeCount,
        .viewCount,
        .commentCount {
            padding: 0 8px;

            i {
                padding-right: 4px;
            }

            a {
                color: @memo-grey-dark;

                &:hover {
                    text-decoration: none;
                    color: @memo-blue;
                }
            }
        }
    }
}

.created {
    display: flex;
    align-items: center;
}

.detail-label {
    padding-left: 3px;
}

.sidesheet-open {
    #ExtendedQuestionDetails {
        #questionDetailsContainer {

            @media (max-width:1300px) {
                display: flex;
                flex-wrap: wrap;
                flex-direction: column;
            }

        }
    }
}
</style>

<style lang="less">
#LicenseQuestion {
    img {
        margin-right: 8px;
        height: 23px;
        width: 60px;
    }
}
</style>