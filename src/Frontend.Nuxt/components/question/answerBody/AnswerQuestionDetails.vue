<script lang="ts" setup>
import { PageItem } from '~~/components/search/searchHelper'
import { useUserStore } from '~~/components/user/userStore'
import * as d3 from 'd3'
import { Tab, useTabsStore } from '~/components/page/tabs/tabsStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { dom } from '@fortawesome/fontawesome-svg-core'
import { KnowledgeStatus } from '../knowledgeStatusEnum'
import { useCommentsStore } from '~~/components/comment/commentsStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'
import { AnswerQuestionDetailsResult } from './answerQuestionDetailsResult'
import { useActivityPointsStore } from '~/components/activityPoints/activityPointsStore'
import { color } from '~/constants/colors'

const learningSessionStore = useLearningSessionStore()
const userStore = useUserStore()
const commentsStore = useCommentsStore()
const deleteQuestionStore = useDeleteQuestionStore()
const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
const { getTimeElapsedAsText } = useTimeElapsed()
interface Props {
    id: number,
    landingPage?: boolean
    model?: any
}

const props = defineProps<Props>()
await commentsStore.loadFirst(props.id)

const visibility = ref<Visibility>(Visibility.Public)
const personalProbability = ref(0)
const personalProbabilityText = ref(t('questionLandingPage.probability.status.notInWishKnowledge'))
const personalColor = ref(color.memoGreyLight)
const avgProbability = ref(0)
const personalAnswerCount = ref(0)
const personalAnsweredCorrectly = ref(0)
const personalAnsweredWrongly = ref(0)
const answerCount = ref('0')
const correctAnswers = ref('0')
const wrongAnswers = ref('0')
const overallAnswerCount = ref(0)
const overallAnsweredCorrectly = ref(0)
const overallAnsweredWrongly = ref(0)
const allAnswerCount = ref('0')
const allCorrectAnswers = ref('0')
const allWrongAnswers = ref('0')
const isInWishKnowledge = ref(false)
const arcSvg = ref<any>({})
const personalCounterSvg = ref<any>({})
const overallCounterSvg = ref<any>({})
const knowledgeStatus = ref<KnowledgeStatus>(KnowledgeStatus.NotLearned)

const baseArcData = ref({
    startAngle: -0.55 * Math.PI,
    endAngle: 0.55 * Math.PI,
    innerRadius: 45,
    outerRadius: 50,
    fill: color.memoGreyLight,
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
    endAngle: (-0.55 + personalProbability.value / 100 * 1.1) * Math.PI,
    innerRadius: 40,
    outerRadius: 55,
    fill: personalColor.value,
    class: "personalArc",
})

const avgArcData = ref<ArcData>({
    startAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI - 0.01,
    endAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI + 0.01,
    innerRadius: 37.5,
    outerRadius: 57.5,
    fill: color.memoGreyDark,
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
    fill: color.memoGreyLight,
})

const personalWrongAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: color.memoSalmon,
    class: "personalWrongAnswerCounter",
})
const personalCorrectAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: color.memoGreen,
    class: "personalCorrectAnswerCounter",
})

const overallWrongAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: color.memoSalmon,
    class: "overallWrongAnswerCounter",
})
const overallCorrectAnswerCountData = ref<ArcData>({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: color.memoGreen,
    class: "overallCorrectAnswerCounter",
})

const tabsStore = useTabsStore()
const questionIdHasChanged = ref(false)

const pages = ref<PageItem[]>([])

function setPersonalProbability() {
    switch (knowledgeStatus.value) {
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
        endAngle: (-0.55 + personalProbability.value / 100 * 1.1) * Math.PI,
        innerRadius: 40,
        outerRadius: 55,
        fill: personalColor.value,
        class: "personalArc",
    }
}

function setAvgArcData() {
    var avgInnerRadius = 37.5
    var avgOuterRadius = 57.5

    if (personalProbability.value < avgProbability.value) {
        avgInnerRadius = 42.5
        avgOuterRadius = 52.5
    }

    avgArcData.value = {
        startAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI - 0.01,
        endAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI + 0.01,
        innerRadius: avgInnerRadius,
        outerRadius: avgOuterRadius,
        fill: color.memoGreyDark,
        class: "avgArc"
    }

    avgAngle.value = (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI
}

function setPersonalCounterData() {
    personalWrongAnswerCountData.value = {
        startAngle: 0,
        endAngle: (personalStartAngle.value / 100 * 1) * Math.PI * 2,
        innerRadius: 20,
        outerRadius: 25,
        fill: color.memoSalmon,
        class: "personalWrongAnswerCounter",
    }

    personalCorrectAnswerCountData.value = {
        startAngle: (personalStartAngle.value / 100 * 1) * Math.PI * 2,
        endAngle: 2 * Math.PI,
        innerRadius: 20,
        outerRadius: 25,
        fill: color.memoGreen,
        class: "personalCorrectAnswerCounter",
    }
}

function setOverallCounterData() {

    overallWrongAnswerCountData.value = {
        startAngle: 0,
        endAngle: (overallStartAngle.value / 100 * 1) * Math.PI * 2,
        innerRadius: 20,
        outerRadius: 25,
        fill: color.memoSalmon,
        class: "overallWrongAnswerCounter",
    }

    overallCorrectAnswerCountData.value = {
        startAngle: (overallStartAngle.value / 100 * 1) * Math.PI * 2,
        endAngle: 2 * Math.PI,
        innerRadius: 20,
        outerRadius: 25,
        fill: color.memoGreen,
        class: "overallCorrectAnswerCounter",
    }
}

function calculateLabelWidth() {
    let probabilityLabelWidth = 0
    var probabilityAsText = [personalProbability.value]

    arcSvg.value.append('g')
        .selectAll('.dummyProbability')
        .data(probabilityAsText)
        .enter()
        .append("text")
        .attr("font-family", "font-family:'Open Sans'")
        .attr("font-weight", "bold")
        .attr("font-size", "30px")
        .text(personalProbability.value)
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

function arcTween(d: any, newStartAngle: number = 0, newEndAngle: number = 0, newInnerRadius: number = 0, newOuterRadius: number = 0) {
    if (d == null || isNaN(newStartAngle) || isNaN(newEndAngle) || isNaN(newInnerRadius) || isNaN(newOuterRadius))
        return
    var arc = d3.arc()
    var interpolateStart = d3.interpolate(d.startAngle, newStartAngle)
    var interpolateRadiusStart = d3.interpolate(d.innerRadius, newInnerRadius)
    var interpolateEnd = d3.interpolate(d.endAngle, newEndAngle)
    var interpolateRadiusEnd = d3.interpolate(d.outerRadius, newOuterRadius)
    return (t: any) => {
        d.innerRadius = interpolateRadiusStart(t)
        d.outerRadius = interpolateRadiusEnd(t)
        d.startAngle = interpolateStart(t)
        d.endAngle = interpolateEnd(t)
        return arc(d)
    }
}

function updateArc() {

    const labelWidth = calculateLabelWidth()
    arcSvg.value.selectAll(".personalProbabilityLabel")
        .transition()
        .duration(800)
        .attr("dx", -(labelWidth / 2) - 5 + "px")
        .style("fill", () => showPersonalArc.value ? personalColor.value : color.memoGreyLight)
        .tween("text", function (this: any) {
            const selection = d3.select(this)
            const start = d3.select(this).text()
            const end = personalProbability.value
            const interpolator = d3.interpolateNumber(parseInt(start), end)

            return (t: any) => {
                selection.text(Math.round(interpolator(t)))
            }
        })

    var pos = d3.arc()
        .innerRadius(55)
        .outerRadius(55)
        .startAngle(avgAngle.value)
        .endAngle(avgAngle.value)
    arcSvg.value.select(".avgProbabilityLabel")
        .transition()
        .duration(400)
        .style("opacity", 0.0)

    arcSvg.value.select(".avgProbabilityLabel")
        .transition()
        .delay(400)
        .duration(400)
        .style("opacity", 1.0)
        .attr("transform", (d: any) => {
            return "translate(" + pos.centroid(d) + ")"
        })
        .attr("dx", dxAvgLabel.value)
        .attr("dy", dyAvgLabel.value)
        .attr("text-anchor", avgLabelAnchor.value)
        .tween("text", function (this: any) {
            var selection = d3.select(this)
            var text = d3.select(this).text()
            var numbers = text.match(/(\d+)/)
            var end = avgProbability.value
            var interpolator = d3.interpolateNumber(parseInt(numbers![0]), end)

            return function (t: any) {
                selection.text("∅ " + Math.round(interpolator(t)) + "%")
            }
        })

    arcSvg.value.selectAll(".percentageLabel").transition()
        .duration(800)
        .attr("dx", (labelWidth / 2) - percentageLabelWidth.value - 5 + "px")
        .style("fill", () => showPersonalArc.value ? personalColor.value : color.memoGreyLight)

    arcSvg.value.selectAll(".personalArc")
        .transition()
        .duration(800)
        .style("fill", personalColor.value)
        .style("visibility", () => {
            return showPersonalArc.value ? "visible" : "hidden"
        })
        .attrTween("d", (d: any) => {
            return arcTween(d,
                personalArcData.value.startAngle,
                personalArcData.value.endAngle,
                personalArcData.value.innerRadius,
                personalArcData.value.outerRadius)
        })

    arcSvg.value.selectAll(".avgArc")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                avgArcData.value.startAngle,
                avgArcData.value.endAngle,
                avgArcData.value.innerRadius,
                avgArcData.value.outerRadius)
        })

    var probabilityTextWidth
    arcSvg.value.selectAll(".personalProbabilityText")
        .text(personalProbabilityText.value)
        .each(function (this: any) {
            probabilityTextWidth = this.getComputedTextLength()
        })
        .transition()
        .delay(200)
        .duration(200)
        .style("fill", () => personalColor.value === color.memoGrey ? "white" : color.memoGreyDarker)

    if (probabilityTextWidth != null)
        arcSvg.value.selectAll(".personalProbabilityChip")
            .transition()
            .duration(400)
            .style("fill", personalColor.value)
            .attr("x", - probabilityTextWidth / 2 - 11)
            .attr("width", probabilityTextWidth + 22)

    arcSvg.value.selectAll(".personalProbabilityChip,.personalProbabilityText")
        .style("visibility", () => (userStore.isLoggedIn && overallAnswerCount.value > 0) ? "visible" : "hidden")

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
        .style("visibility", () => personalAnswerCount.value > 0 ? "visible" : "hidden")

    personalCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', '14px')
        .attr('x', -7)
        .attr('y', -8)
        .html(() => {
            var fontColor = personalAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight
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
        .style("visibility", () => overallAnswerCount.value > 0 ? "visible" : "hidden")

    overallCounterSvg.value.selectAll("i")
        .style("color", () => {
            return overallAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight
        })

    overallCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', visibility.value === Visibility.Private ? '14px' : '20px')
        .attr('x', visibility.value === Visibility.Private ? -7 : -10)
        .attr('y', -8)
        .html(() => {
            var fontColor = overallAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight
            if (visibility.value === Visibility.Private)
                return "<i class='fa-solid fa-lock' style='font-size:16px; color:" + fontColor + "'> </i>"
            else
                return "<i class='fa-solid fa-users' style='font-size:16px; color:" + fontColor + "'> </i>"

        })
}

function updateCounters() {
    if (!personalCounter.value || !overallCounter.value || angleIsNaN(personalWrongAnswerCountData.value) || angleIsNaN(personalCorrectAnswerCountData.value))
        return

    personalCounterSvg.value.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
        .style("visibility", () => {
            return personalAnswerCount.value > 0 ? "visible" : "hidden"
        })

    personalCounterSvg.value.selectAll("i")
        .style("color", () => {
            return personalAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight
        })

    personalCounterSvg.value.selectAll(".personalWrongAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                personalWrongAnswerCountData.value.startAngle,
                personalWrongAnswerCountData.value.endAngle,
                20,
                25)
        })

    personalCounterSvg.value.selectAll(".personalCorrectAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                personalCorrectAnswerCountData.value.startAngle,
                personalCorrectAnswerCountData.value.endAngle,
                20,
                25)
        })

    personalCounterSvg.value.selectAll("text")
        .transition()
        .duration(800)
        .style("fill", () => personalAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight)


    overallCounterSvg.value.selectAll(".overallWrongAnswerCounter, .overallCorrectAnswerCounter")
        .style("visibility", () => {
            return overallAnswerCount.value > 0 ? "visible" : "hidden"
        })

    overallCounterSvg.value.selectAll("i")
        .style("color", () => {
            return overallAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight
        })

    overallCounterSvg.value.selectAll(".overallWrongAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                overallWrongAnswerCountData.value.startAngle,
                overallWrongAnswerCountData.value.endAngle,
                20,
                25)
        })

    overallCounterSvg.value.selectAll(".overallCorrectAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                overallCorrectAnswerCountData.value.startAngle,
                overallCorrectAnswerCountData.value.endAngle,
                20,
                25)
        })

    overallCounterSvg.value.selectAll("text")
        .transition()
        .duration(800)
        .style("fill", () => overallAnswerCount.value > 0 ? color.memoGrey : color.memoGreyLight)
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
        .style("fill", () => showPersonalArc.value ? personalColor.value : color.memoGreyLight)
        .attr("class", "personalProbabilityLabel")
        .text(() => personalAnswerCount.value > 0 ? personalProbability.value : avgProbability.value)

    arcSvg.value.append("svg:text")
        .attr("dy", "-.35em")
        .attr("dx", (labelWidth / 2) - percentageLabelWidth.value - 5 + "px")
        .attr("style", "font-family:'Open Sans'")
        .attr("text-anchor", "left")
        .attr("font-size", "18")
        .attr("font-weight", "medium")
        .attr("class", "percentageLabel")
        .style("fill", () => showPersonalArc.value ? personalColor.value : color.memoGreyLight)
        .text("%")

    arcSvg.value.append("svg:rect")
        .attr("class", "personalProbabilityChip")
        .attr("rx", 10)
        .attr("ry", 10)
        .attr("y", 20)
        .attr("height", 20)
        .style("fill", personalColor.value)
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
        .style("fill", () => personalColor.value === color.memoGrey ? "white" : color.memoGreyDarker)
        .attr("transform", "translate(0,0)")
        .text(personalProbabilityText.value)
        .each(function (this: any) {
            textWidth = this.getComputedTextLength()
        })

    arcSvg.value.selectAll(".personalProbabilityChip")
        .attr("x", - textWidth / 2 - 11)
        .attr("width", textWidth + 22)

    arcSvg.value.selectAll(".personalProbabilityChip,.personalProbabilityText")
        .style("visibility", () => (userStore.isLoggedIn && overallAnswerCount.value > 0) ? "visible" : "hidden")

}

function setAvgLabelPos() {
    var probabilityData = [avgProbability.value]

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

    var el = (avgProbability.value - 50) / 10
    dyAvgLabel.value = (0.20 * Math.pow(el, 2) - 5) * 2 + .25 * (Math.pow(el, 2))

    dxAvgLabel.value = 0

    if (avgProbability.value > 50) {
        if (avgProbability.value < 80)
            dxAvgLabel.value = -(80 - avgProbability.value) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = - (20 - avgProbability.value) * 6 / 100
        avgLabelAnchor.value = "start"
    }
    else if (avgProbability.value < 50) {
        if (avgProbability.value > 20)
            dxAvgLabel.value = (avgProbability.value - 20) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = (avgProbability.value - 80) * 6 / 100
        avgLabelAnchor.value = "end"
    }
    else if (avgProbability.value === 50) {
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
        .style("fill", color.memoGreyDarker)
        .style("opacity", 1.0)
        .attr("class", "avgProbabilityLabel")
        .text("∅ " + avgProbability.value + "%")
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

watch(() => props.id, (o, n) => {
    if (o != n)
        questionIdHasChanged.value = true
    else questionIdHasChanged.value = false
})

const backgroundColor = ref('')
const correctnessProbabilityLabel = ref(t('knowledgeStatus.notLearned'))

const setKnowledgebarData = () => {

    switch (knowledgeStatus.value) {
        case KnowledgeStatus.Solid:
            backgroundColor.value = "solid"
            correctnessProbabilityLabel.value = t('knowledgeStatus.solid')
            break
        case KnowledgeStatus.NeedsConsolidation:
            backgroundColor.value = "needsConsolidation"
            correctnessProbabilityLabel.value = t('knowledgeStatus.needsConsolidation')
            break
        case KnowledgeStatus.NeedsLearning:
            backgroundColor.value = "needsLearning"
            correctnessProbabilityLabel.value = t('knowledgeStatus.needsLearning')
            break
        default:
            backgroundColor.value = "notLearned"
            correctnessProbabilityLabel.value = t('knowledgeStatus.notLearned')
            break
    }
}
watch(knowledgeStatus, () => {
    setKnowledgebarData()
})

async function initData(model: AnswerQuestionDetailsResult) {
    // if (model.questionId !== props.id)
    //     return

    personalProbability.value = model.personalProbability
    isInWishKnowledge.value = model.isInWishKnowledge
    avgProbability.value = model.avgProbability

    personalAnswerCount.value = model.personalAnswerCount
    personalAnsweredCorrectly.value = model.personalAnsweredCorrectly
    personalAnsweredWrongly.value = model.personalAnsweredWrongly

    visibility.value = model.visibility

    overallAnswerCount.value = model.overallAnswerCount
    overallAnsweredCorrectly.value = model.overallAnsweredCorrectly
    overallAnsweredWrongly.value = model.overallAnsweredWrongly

    personalColor.value = model.personalColor

    creator.value = model.creator
    creationDate.value = model.creationDate
    totalViewCount.value = model.totalViewCount
    wishknowledgeCount.value = model.wishknowledgeCount
    licenseId.value = model.licenseId
    knowledgeStatus.value = model.knowledgeStatus

    if (!learningSessionStore.isInTestMode)
        pages.value = model.pages

    setPersonalProbability()
    setPersonalArcData()

    setAvgArcData()

    setPersonalCounterData()
    setOverallCounterData()
    await nextTick()
    if (arcLoaded.value) {
        updateArc()
        if (questionIdHasChanged.value)
            drawCounterArcs()
        else
            updateCounters()
    } else if (showExtendedDetails.value) {
        drawArc()
        drawCounterArcs()
    }

    questionIdHasChanged.value = false
}

const { $logger } = useNuxtApp()
const loadDataResult = ref<AnswerQuestionDetailsResult>()
async function loadData() {
    await nextTick()

    if (props.id === deleteQuestionStore.deletedQuestionId)
        return
    const result = await $api<AnswerQuestionDetailsResult>(`/apiVue/AnswerQuestionDetails/Get/${props.id}`, {
        credentials: 'include',
        mode: 'cors',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
    if (result) {
        loadDataResult.value = result
        initData(result)
    }
}

onMounted(async () => {
    dom.watch()
    await nextTick()
    props.landingPage ? initData(props.model) : loadData()
})

watch(() => props.id, () => loadData())

// Computed properties for better reactivity
const computedPersonalStartAngle = computed(() => {
    if (personalAnswerCount.value === 0) {
        return 0
    }
    return 100 - (100 / personalAnswerCount.value * personalAnsweredCorrectly.value)
})

const computedOverallStartAngle = computed(() => {
    if (overallAnswerCount.value === 0) {
        return 0
    }
    return 100 - (100 / overallAnswerCount.value * overallAnsweredCorrectly.value)
})

const computedAnswerCount = computed(() => getFormattedNumber(personalAnswerCount.value))
const computedCorrectAnswers = computed(() => getFormattedNumber(personalAnsweredCorrectly.value))
const computedWrongAnswers = computed(() => getFormattedNumber(personalAnsweredWrongly.value))
const computedAllAnswerCount = computed(() => getFormattedNumber(overallAnswerCount.value))
const computedAllCorrectAnswers = computed(() => getFormattedNumber(overallAnsweredCorrectly.value))
const computedAllWrongAnswers = computed(() => getFormattedNumber(overallAnsweredWrongly.value))

// Watch for side effects that can't be computed
watch(personalAnswerCount, (val) => {
    if (val > 0) {
        showPersonalArc.value = true
    }
})

// Sync computed values to reactive refs for backward compatibility
watch(computedPersonalStartAngle, (val) => {
    personalStartAngle.value = val
})

watch(computedOverallStartAngle, (val) => {
    overallStartAngle.value = val
})

watch(computedAnswerCount, (val) => {
    answerCount.value = val
})

watch(computedCorrectAnswers, (val) => {
    correctAnswers.value = val
})

watch(computedWrongAnswers, (val) => {
    wrongAnswers.value = val
})

watch(computedAllAnswerCount, (val) => {
    allAnswerCount.value = val
})

watch(computedAllCorrectAnswers, (val) => {
    allCorrectAnswers.value = val
})

watch(computedAllWrongAnswers, (val) => {
    allWrongAnswers.value = val
})

const creator = ref({
    name: '',
    id: 0
})
const creationDate = ref<Date | string>()
const wishknowledgeCount = ref(0)
const totalViewCount = ref(0)
const licenseId = ref(1) // Default to CC BY 4.0

function openCommentModal() {
    commentsStore.openModal(props.id)
}

onMounted(() => {
    learningSessionStore.$onAction(({ after, name }) => {
        if (name === 'markCurrentStep') {
            after(() => {
                loadData()
            })
        }
    })
    watch(() => tabsStore.activeTab, (val) => {
        if (val === Tab.Learning)
            loadData()
    })
})

watch(() => userStore.isLoggedIn, () => {
    loadData()
})


const showExtendedDetails = ref(false)
watch(showExtendedDetails, () => {
    if (loadDataResult.value)
        initData(loadDataResult.value)
})

const activityPointsStore = useActivityPointsStore()


</script>

<template>
    <div>
        <div id="MiniQuestionDetailsContainer">
            <div id="MiniQuestionDetails">
                <div class="questionStats" v-show="!showExtendedDetails">
                    <div class="probabilitySection">
                        <span class="chip" :class="backgroundColor">{{ correctnessProbabilityLabel }}</span>
                    </div>
                    <div class="answerDetails">
                        <div>
                            <strong>{{ personalProbability }}%</strong> {{ t('answerbody.details.answerProbability') }}
                        </div>
                        <div class="counter">
                            <div>
                                <strong>{{ answerCount }}</strong> {{ t('answerbody.details.answeredXTimes') }}
                            </div>
                            <div class="spacer"></div>
                            <div>
                                <strong>{{ correctAnswers }} </strong> {{ t('answerbody.details.correct') }} /
                                <strong>{{ wrongAnswers }}</strong> {{ t('answerbody.details.wrong') }}
                            </div>
                        </div>
                    </div>
                    <div @click="showExtendedDetails = true" class="expendDetailsToggle">
                        {{ t('answerbody.details.showDetails') }}
                    </div>
                </div>
                <div @click="showExtendedDetails = false" class="expendDetailsToggle" v-if="showExtendedDetails">
                    {{ t('answerbody.details.hideDetails') }}
                </div>
            </div>

            <div id="ActivityPointsDisplay">
                <div>
                    <small>{{ t('answerbody.details.yourScore') }}</small>
                </div>
                <div class="activitypoints-display-detail">
                    <span id="ActivityPoints">
                        {{ activityPointsStore.points }}
                    </span>
                    <font-awesome-icon
                        icon="fa-solid fa-circle-info"
                        class="activity-points-icon"
                        v-tooltip="t('answerbody.details.tooltipLearningPoints')" />
                </div>
            </div>
        </div>
        <div class="separationBorderTop"></div>

        <div id="ExtendedQuestionDetails" v-show="showExtendedDetails">

            <div id="questionDetailsContainer" class="row" style="min-height:265px">
                <div id="pageList" class="col-sm-5" :class="{ isLandingPage: 'isLandingPage' }">
                    <div class="overline-s no-line">{{ t('answerbody.details.pages') }}</div>
                    <div class="pageListChips">
                        <div style="display: flex; flex-wrap: wrap;">
                            <PageChip
                                v-for="(t, index) in pages"
                                :key="t.id + index"
                                :page="t"
                                :index="index"
                                :is-spoiler="learningSessionStore.isInTestMode && t.isSpoiler && !$props.landingPage" />
                        </div>
                    </div>
                </div>
                <div id="questionStatistics" class="col-sm-7">
                    <div id="probabilityContainer" class="" ref="probabilityContainer">
                        <div class="overline-s no-line">{{ t('answerbody.details.answerProbability') }}</div>
                        <div id="semiPieSection">
                            <div id="semiPieChart" style="min-height:130px">
                                <svg
                                    class="semiPieSvgContainer"
                                    ref="semiPie"
                                    width="200"
                                    height="130"
                                    :class="{ 'isInWishKnowledge': isInWishKnowledge }"></svg>
                            </div>
                            <div id="probabilityText">
                                <div v-if="userStore.isLoggedIn" style="">
                                    <strong>{{ personalProbability }}%</strong>
                                    {{ t('answerbody.details.loggedInProbabilityText') }}
                                    <strong>{{ avgProbability }}%</strong>
                                </div>
                                <div v-else style="">
                                    <strong>{{ personalProbability }}%</strong>
                                    {{ t('answerbody.details.notLoggedInProbabilityText') }}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="counterContainer" class="" style="font-size:12px">
                        <div class="overline-s no-line">{{ t('answerbody.details.answers') }}</div>
                        <div class="counterBody">
                            <div class="counterHalf">
                                <svg ref="personalCounter" style="min-width:50px" width="50" height="50"></svg>
                                <div v-if="personalAnswerCount > 0" class="counterLabel">
                                    <template v-if="userStore.isLoggedIn">
                                        {{ t('answerbody.details.fromYou') }}
                                    </template>
                                    <template v-else>
                                        {{ t('answerbody.details.fromUnregistered') }}
                                    </template>
                                    <br />
                                    <strong>{{ answerCount }}</strong> {{ t('answerbody.details.answeredXTimes') }} <br />
                                    <strong>{{ correctAnswers }}</strong> {{ t('answerbody.details.correct') }} /
                                    <strong>{{ wrongAnswers }}</strong> {{ t('answerbody.details.wrong') }}
                                </div>
                                <div v-else-if="userStore.isLoggedIn" class="counterLabel">
                                    {{ t('answerbody.details.neverAnswered') }}
                                </div>
                                <div v-else class="counterLabel">
                                    {{ t('answerbody.details.youAreNotLoggedInNoData') }}
                                    <button class="btn-link" @click="userStore.openLoginModal()">
                                        {{ t('answerbody.details.login') }}
                                    </button>
                                </div>
                            </div>
                            <div class="counterHalf">
                                <svg ref="overallCounter" style="min-width:50px" width="50" height="50"></svg>
                                <div v-if="overallAnswerCount > 0" class="counterLabel">
                                    {{ t('answerbody.details.fromAllUsers') }} <br />
                                    <strong>{{ allAnswerCount }}</strong> {{ t('answerbody.details.answeredXTimes') }} <br />
                                    <strong>{{ allCorrectAnswers }}</strong> {{ t('answerbody.details.correct') }} /
                                    <strong>{{ allWrongAnswers }}</strong> {{ t('answerbody.details.wrong') }}
                                </div>
                                <div v-else class="counterLabel">
                                    <template v-if="visibility === 1">
                                        {{ t('answerbody.details.privateQuestionForYou') }}
                                    </template>
                                    <template v-else>
                                        {{ t('answerbody.details.neverAnsweredAll') }}
                                    </template>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="separationBorderTop"></div>
        </div>
        <div id="QuestionDetailsFooter">
            <div class="questionDetailsFooterPartialLeft">
                <LicenseLink :licenseId="licenseId" :creator="creator" />
                <div class="created">
                    {{ t('answerbody.details.createdBy') }}
                    <NuxtLink v-if="creator.id > 0" :to="$urlHelper.getUserUrl(creator.name, creator.id)">
                        &nbsp;{{ creator.name }}&nbsp;
                    </NuxtLink>
                    {{ getTimeElapsedAsText(creationDate) }}
                </div>
            </div>
            <div class="questionDetailsFooterPartialRight">
                <div class="wishknowledgeCount">
                    <font-awesome-icon icon="fa-solid fa-heart" />
                    <span class="detail-label">
                        {{ wishknowledgeCount }}
                    </span>
                </div>

                <div class="viewCount">
                    <font-awesome-icon icon="fa-solid fa-eye" />
                    <span class="detail-label">
                        {{ totalViewCount }}
                    </span>
                </div>
                <div class="commentCount pointer" @click="openCommentModal()">
                    <font-awesome-icon icon="fa-solid fa-comment" />
                    <span id="commentCountAnswerBody" class="detail-label">
                        {{ commentsStore.unsettledComments.length }}
                    </span>
                </div>
            </div>
        </div>
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

.separationBorderTop {
    min-height: 10px;
    margin-bottom: 10px;
}

#ExtendedQuestionDetails {

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
                            max-width: 160px;
                            width: 100%;
                            padding: 0 0 0 20px;

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
    margin-top: -10px;

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

        .created {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
        }
    }

    .questionDetailsFooterPartialRight {
        width: 40%;
        display: flex;
        justify-content: flex-end;
        padding-right: 4px;
        height: 100%;
        flex-wrap: wrap;

        .wishknowledgeCount,
        .viewCount,
        .commentCount {
            display: flex;
            align-items: center;
            justify-content: center;
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

.detail-label {
    padding-left: 3px;
}

.expendDetailsToggle {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: flex-end;
    color: @memo-blue-link;
    cursor: pointer;
    user-select: none;
    font-size: 11px;
    margin: 12px 4px 0;

    &:hover {
        text-decoration: underline;
    }
}

.spacer {
    margin-left: 8px;
    margin-right: 4px;
    height: 8px;
    width: 1px;
    background-color: @memo-grey-light;
}

#MiniQuestionDetailsContainer {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    padding-bottom: 8px;

    #MiniQuestionDetails {
        display: flex;
        justify-content: flex-start;
        align-items: center;
        padding-right: 12px;
        width: 100%;

        .separationBorderTop {
            margin-top: 8px;
        }

        .questionStats {
            display: flex;
            font-size: 14px;
            padding-left: 12px;
            padding-right: 12px;
            width: 100%;

            .answerDetails {
                margin-top: 12px;
                align-items: flex-start;
                display: flex;
                color: @memo-grey-dark;
                font-size: 11px;
                flex-direction: column;
                margin-right: 12px;

                @media(max-width: @screen-xxs-max) {
                    padding-right: 0px;
                }

                .counter {
                    display: flex;
                    justify-content: flex-start;
                    align-items: center;
                    flex-direction: row;
                }

                strong {
                    margin: 0 4px;
                }
            }

            .probabilitySection {
                margin-top: 12px;
                padding-right: 10px;
                display: flex;
                justify-content: center;
                align-items: center;

                span {
                    &.percentageLabel {
                        font-weight: bold;
                        color: @memo-grey-light;

                        &.solid {
                            color: @memo-green;
                        }

                        &.needsConsolidation {
                            color: @memo-yellow;
                        }

                        &.needsLearning {
                            color: @memo-salmon;
                        }

                        &.notLearned {
                            color: @memo-grey-light;
                        }
                    }

                    &.chip {
                        padding: 2px 12px;
                        border-radius: 20px;
                        background: @memo-grey-light;
                        color: @memo-grey-darker;
                        white-space: nowrap;

                        &.solid {
                            background: @memo-green;
                        }

                        &.needsConsolidation {
                            background: @memo-yellow;
                        }

                        &.needsLearning {
                            background: @memo-salmon;
                        }

                        &.notLearned {
                            background: @memo-grey-light;
                            color: @memo-grey-darker;
                        }
                    }
                }

                &.open {
                    height: unset;
                    margin-top: 20px;
                    margin-bottom: 20px;
                    transition: all .2s ease-out;
                    box-shadow: 0px 1px 6px 0px #C4C4C4;
                }
            }

            @media(max-width: @screen-sm-min) {
                flex-wrap: wrap;
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