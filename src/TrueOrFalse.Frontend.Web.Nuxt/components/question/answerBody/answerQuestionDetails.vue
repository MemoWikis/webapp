<script lang="ts" setup>
import { TopicItem } from '~~/components/search/searchHelper'
import { useUserStore } from '~~/components/user/userStore'
import * as d3 from 'd3'
import { Tab, useTabsStore } from '~~/components/topic/tabs/tabsStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useLearningSessionStore } from '~~/components/topic/learning/learningSessionStore'

const learningSessionStore = useLearningSessionStore()
const userStore = useUserStore()

const color = useCssModule('colorReferences')

interface Props {
    id: number
}
const props = defineProps<Props>()

const visibility = ref<Visibility>(Visibility.All)
const personalProbability = ref(0)
const personalProbabilityText = ref("Nicht im Wunschwissen")
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
const isInWishknowledge = ref(false)
const showTopBorder = ref(false)
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

const personalArcData = ref<{
    startAngle: number,
    endAngle: number,
    innerRadius: number,
    outerRadius: number,
    fill: string,
    class: "personalArc",
}>({
    startAngle: -0.55 * Math.PI,
    endAngle: (-0.55 + personalProbability.value / 100 * 1.1) * Math.PI,
    innerRadius: 40,
    outerRadius: 55,
    fill: personalColor.value,
    class: "personalArc",
})

const avgArcData = ref({
    startAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI - 0.01,
    endAngle: (-0.55 + avgProbability.value / 100 * 1.1) * Math.PI + 0.01,
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

const baseCounterData = ref({
    startAngle: 0,
    endAngle: 2 * Math.PI,
    innerRadius: 20,
    outerRadius: 25,
    fill: "#DDDDDD",
})

const personalWrongAnswerCountData = ref<any>({})
const personalCorrectAnswerCountData = ref<any>({})

const overallWrongAnswerCountData = ref<any>({})
const overallCorrectAnswerCountData = ref<any>({})

const tabsStore = useTabsStore()
const isLandingPage = ref(tabsStore.activeTab != Tab.Learning)
const questionIdHasChanged = ref(false)

const topics = ref<TopicItem[]>([])

function setPersonalProbability() {
    if (personalAnswerCount.value <= 0) {
        personalProbabilityText.value = "Nicht gelernt"
        personalColor.value = "#999999"
    }
    else if (personalProbability.value >= 80)
        personalProbabilityText.value = "Sicheres Wissen"
    else if (personalProbability.value < 80 && personalProbability.value >= 50)
        personalProbabilityText.value = "Zu festigen"
    else if (personalProbability.value < 50 && personalProbability.value > 0)
        personalProbabilityText.value = "Zu lernen"
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
        fill: "#707070",
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
        .each((e: any) => {
            let thisWidth = e.getComputedTextLength()
            probabilityLabelWidth = thisWidth
            e.remove()
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
        .each((e: any) => {
            let thisWidth = 0
            percentageLabelWidth.value = thisWidth
            e.remove()
        })

    return probabilityLabelWidth + percentageLabelWidth.value + 1
}

function arcTween(d: any, newStartAngle: number, newEndAngle: number, newInnerRadius: number, newOuterRadius: number) {
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
    var labelWidth = calculateLabelWidth()

    arcSvg.value.selectAll(".personalProbabilityLabel")
        .transition()
        .duration(800)
        .attr("dx", -(labelWidth / 2) - 5 + "px")
        .style("fill", () => showPersonalArc ? personalColor : "#DDDDDD")
        .tween("text", (e: any) => {
            var selection = d3.select(e)
            var start = d3.select(e).text()
            var end = personalProbability.value
            var interpolator = d3.interpolateNumber(parseInt(start), end)

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
            return "translate(" + pos.centroid(d) + ")";
        })
        .attr("dx", dxAvgLabel)
        .attr("dy", dyAvgLabel)
        .attr("text-anchor", avgLabelAnchor)
        .tween("text", (e: any) => {
            var selection = d3.select(e);
            var text = d3.select(e).text();
            var numbers = text.match(/(\d+)/);
            var end = avgProbability.value
            var interpolator = d3.interpolateNumber(parseInt(numbers![0]), end)

            return function (t: any) {
                selection.text("∅ " + Math.round(interpolator(t)) + "%")
            }
        })

    arcSvg.value.selectAll(".percentageLabel").transition()
        .duration(800)
        .attr("dx", (labelWidth / 2) - percentageLabelWidth.value - 5 + "px")
        .style("fill", () => showPersonalArc.value ? personalColor.value : "#DDDDDD")

    arcSvg.value.selectAll(".personalArc")
        .transition()
        .duration(800)
        .style("fill", personalColor)
        .style("visibility", () => {
            return showPersonalArc ? "visible" : "hidden";
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
        .text(personalProbabilityText)
        .each((e: any) => {
            probabilityTextWidth = e.getComputedTextLength()
        })
        .transition()
        .delay(200)
        .duration(200)
        .style("fill", () => personalColor.value == "#999999" ? "white" : "#555555");

    if (probabilityTextWidth != null)
        arcSvg.value.selectAll(".personalProbabilityChip")
            .transition()
            .duration(400)
            .style("fill", personalColor)
            .attr("x", - probabilityTextWidth / 2 - 11)
            .attr("width", probabilityTextWidth + 22);

    arcSvg.value.selectAll(".personalProbabilityChip,.personalProbabilityText")
        .style("visibility", () => (userStore.isLoggedIn && overallAnswerCount.value > 0) ? "visible" : "hidden");
}

const personalCounter = ref()
const overallCounter = ref()

function drawCounterArcs() {
    var arc = d3.arc()

    var personalCounterData = [
        baseCounterData.value,
        personalWrongAnswerCountData.value,
        personalCorrectAnswerCountData.value
    ]

    personalCounterSvg.value = d3.select(personalCounter.value).append("svg")
        .attr("width", 50)
        .attr("height", 50)
        .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")");

    personalCounterSvg.value.selectAll("path")
        .data(personalCounterData)
        .enter()
        .append("path")
        .style("fill", (d: any) => { return d.fill })
        .attr("class", (d: any) => { return d.class })
        .attr("d", arc);

    personalCounterSvg.value.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
        .style("visibility", () => personalAnswerCount.value > 0 ? "visible" : "hidden");

    personalCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', '14px')
        .attr('x', -7)
        .attr('y', -8)
        .html(() => {
            var fontColor = personalAnswerCount.value > 0 ? "#999999" : "#DDDDDD";
            return "<i class='fas fa-user' style='font-size:16px; color:" + fontColor + "'> </i>";
        });

    var overallCounterData = [
        baseCounterData,
        overallWrongAnswerCountData,
        overallCorrectAnswerCountData
    ];

    overallCounterSvg.value = d3.select(overallCounter.value).append("svg")
        .attr("width", 50)
        .attr("height", 50)
        .append("g").attr("transform", "translate(" + 25 + "," + 25 + ")");

    overallCounterSvg.value.selectAll("path")
        .data(overallCounterData)
        .enter()
        .append("path")
        .style("fill", (d: any) => { return d.fill })
        .attr("class", (d: any) => { return d.class })
        .attr("d", arc)

    overallCounterSvg.value.selectAll(".overallWrongAnswerCounter, .overallCorrectAnswerCounter")
        .style("visibility", () => overallAnswerCount.value > 0 ? "visible" : "hidden")

    overallCounterSvg.value.selectAll("i")
        .style("color", () => {
            return overallAnswerCount.value > 0 ? "#999999" : "#DDDDDD"
        })

    overallCounterSvg.value
        .append('svg:foreignObject')
        .attr('height', '16px')
        .attr('width', '20px')
        .attr('x', -10)
        .attr('y', -8)
        .html(() => {
            var fontColor = overallAnswerCount.value > 0 ? "#999999" : "#DDDDDD"
            if (visibility.value == Visibility.Owner)
                return "<i class='fas fa-lock' style='font-size:16px; color:" + fontColor + "'> </i>"
            else
                return "<i class='fas fa-users' style='font-size:16px; color:" + fontColor + "'> </i>"

        });
}

function updateCounters() {

    personalCounterSvg.value.selectAll(".personalWrongAnswerCounter,.personalCorrectAnswerCounter")
        .style("visibility", () => {
            return personalAnswerCount.value > 0 ? "visible" : "hidden"
        })

    personalCounterSvg.selectAll("i")
        .style("color", () => {
            return personalAnswerCount.value > 0 ? "#999999" : "#DDDDDD"
        })

    personalCounterSvg.selectAll(".personalWrongAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                personalWrongAnswerCountData.value.startAngle,
                personalWrongAnswerCountData.value.endAngle,
                20,
                25)
        })

    personalCounterSvg.selectAll(".personalCorrectAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                personalCorrectAnswerCountData.value.startAngle,
                personalCorrectAnswerCountData.value.endAngle,
                20,
                25)
        })

    personalCounterSvg.selectAll("text")
        .transition()
        .duration(800)
        .style("fill", () => personalAnswerCount.value > 0 ? "#999999" : "#DDDDDD");


    overallCounterSvg.selectAll(".overallWrongAnswerCounter, .overallCorrectAnswerCounter")
        .style("visibility", () => {
            return overallAnswerCount.value > 0 ? "visible" : "hidden";
        })

    overallCounterSvg.selectAll("i")
        .style("color", () => {
            return overallAnswerCount.value > 0 ? "#999999" : "#DDDDDD";
        })

    overallCounterSvg.selectAll(".overallWrongAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                overallWrongAnswerCountData.value.startAngle,
                overallWrongAnswerCountData.value.endAngle,
                20,
                25)
        })

    overallCounterSvg.selectAll(".overallCorrectAnswerCounter")
        .transition()
        .duration(800)
        .attrTween("d", (d: any) => {
            return arcTween(d,
                overallCorrectAnswerCountData.value.startAngle,
                overallCorrectAnswerCountData.value.endAngle,
                20,
                25)
        })

    overallCounterSvg.selectAll("text")
        .transition()
        .duration(800)
        .style("fill", () => overallAnswerCount.value > 0 ? "#999999" : "#DDDDDD")
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
        .style("fill", () => showPersonalArc.value ? personalColor.value : "#DDDDDD")
        .attr("class", "personalProbabilityLabel")
        .text(() => personalAnswerCount.value > 0 ? personalProbability.value : avgProbability.value);

    arcSvg.value.append("svg:text")
        .attr("dy", "-.35em")
        .attr("dx", (labelWidth / 2) - percentageLabelWidth.value - 5 + "px")
        .attr("style", "font-family:'Open Sans'")
        .attr("text-anchor", "left")
        .attr("font-size", "18")
        .attr("font-weight", "medium")
        .attr("class", "percentageLabel")
        .style("fill", () => showPersonalArc.value ? personalColor.value : "#DDDDDD")
        .text("%");

    arcSvg.value.append("svg:rect")
        .attr("class", "personalProbabilityChip")
        .attr("rx", 10)
        .attr("ry", 10)
        .attr("y", 20)
        .attr("height", 20)
        .style("fill", personalColor.value)
        .style("visibility", () => {
            return userStore.isLoggedIn ? "visible" : "hidden";
        })
        .attr("transform", "translate(0,0)");

    var textWidth = 0;
    arcSvg.value
        .append("svg:text")
        .attr("dy", "33.5")
        .attr("style", "font-family:Open Sans")
        .attr("text-anchor", "middle")
        .attr("font-size", "10")
        .attr("font-weight", "medium")
        .attr("class", "personalProbabilityText")
        .style("fill", () => personalColor.value == "#999999" ? "white" : "#555555")
        .attr("transform", "translate(0,0)")
        .text(personalProbabilityText)
        .each((e: any) => {
            textWidth = e.getComputedTextLength()
        })

    arcSvg.value.selectAll(".personalProbabilityChip")
        .attr("x", - textWidth / 2 - 11)
        .attr("width", textWidth + 22);

    arcSvg.value.selectAll(".personalProbabilityChip,.personalProbabilityText")
        .style("visibility", () => (userStore.isLoggedIn && overallAnswerCount.value > 0) ? "visible" : "hidden");

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
        .each((e: any) => {
            var thisWidth = e.getComputedTextLength()
            avgProbabilityLabelWidth.value = thisWidth
            e.remove()
        });

    var el = (avgProbability.value - 50) / 10
    dyAvgLabel.value = (0.20 * Math.pow(el, 2) - 5) * 2 + .25 * (Math.pow(el, 2))

    dxAvgLabel.value = 0

    if (avgProbability.value > 50) {
        if (avgProbability.value < 80)
            dxAvgLabel.value = -(80 - avgProbability.value) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = - (20 - avgProbability.value) * 6 / 100;
        avgLabelAnchor.value = "start"
    }
    else if (avgProbability.value < 50) {
        if (avgProbability.value > 20)
            dxAvgLabel.value = (avgProbability.value - 20) / 100 * avgProbabilityLabelWidth.value
        else
            dxAvgLabel.value = (avgProbability.value - 80) * 6 / 100;
        avgLabelAnchor.value = "end";
    }
    else if (avgProbability.value == 50) {
        avgLabelAnchor.value = "middle";
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
        .attr("dx", dxAvgLabel)
        .attr("dy", dyAvgLabel)
        .attr("text-anchor", avgLabelAnchor)
        .attr("style", "font-family:'Open Sans'")
        .attr("font-size", "12")
        .attr("font-weight", "regular")
        .style("fill", "#555555")
        .style("opacity", 1.0)
        .attr("class", "avgProbabilityLabel")
        .text("∅ " + avgProbability + "%")
}

const semiPie = ref()
function drawArc() {

    var width = 200
    var height = 130

    arcSvg.value = d3.select(semiPie.value).append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g").attr("transform", "translate(" + width / 2 + "," + (height - 50) + ")")

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
            return showPersonalArc ? "visible" : "hidden";
        });

    drawProbabilityLabel()
    setAvgLabel()

    arcLoaded.value = true
}

async function loadData() {

    const result = await $fetch<any>(`/apiVue/AnswerQuestionDetails/Get?id=${props.id}`)
    if (result != null) {
        personalProbability.value = result.personalProbability
        isInWishknowledge.value = result.isInWishknowledge
        avgProbability.value = result.avgProbability

        personalAnswerCount.value = result.personalAnswerCount
        personalAnsweredCorrectly.value = result.personalAnsweredCorrectly
        personalAnsweredWrongly.value = result.personalAnsweredWrongly

        visibility.value = result.visibility

        overallAnswerCount.value = result.overallAnswerCount
        overallAnsweredCorrectly.value = result.overallAnsweredCorrectly
        overallAnsweredWrongly.value = result.overallAnsweredWrongly

        personalColor.value = result.personalColor

        if (!learningSessionStore.isInTestMode)
            topics.value = result.topics

        setPersonalProbability()
        setPersonalArcData()

        setAvgArcData()

        setPersonalCounterData()
        setOverallCounterData()

        if (arcLoaded.value) {
            updateArc()
            if (questionIdHasChanged.value)
                drawCounterArcs()
            else
                updateCounters()
        } else {
            drawArc()
            drawCounterArcs()
        }
        questionIdHasChanged.value
    }
}

onMounted(() => {
    loadData()
})

watch(() => props.id, () => loadData())

function abbreviateNumber(val: number): string {
    var newVal
    if (val < 1000000) {
        return val.toLocaleString("de-DE")
    }
    else if (val >= 1000000 && val < 1000000000) {
        newVal = val / 1000000
        return parseInt(newVal.toFixed(2)).toLocaleString("de-DE") + " Mio."
    }
    return ''
}

watch(personalAnswerCount, (val) => {
    if (val > 0)
        showPersonalArc.value = true
    personalStartAngle.value = 100 - (100 / personalAnswerCount.value * personalAnsweredCorrectly.value)
    answerCount.value = abbreviateNumber(val)
})

watch(personalAnsweredCorrectly, (val) => {
    personalStartAngle.value = 100 - (100 / personalAnswerCount.value * personalAnsweredCorrectly.value)
    correctAnswers.value = abbreviateNumber(val)
})

watch(personalAnsweredWrongly, (val) => {
    personalStartAngle.value = 100 - (100 / personalAnswerCount.value * personalAnsweredCorrectly.value)
    wrongAnswers.value = abbreviateNumber(val)
})

watch(overallAnswerCount, (val) => {
    overallStartAngle.value = 100 - (100 / overallAnswerCount.value * overallAnsweredCorrectly.value);
    allAnswerCount.value = abbreviateNumber(val);
})

watch(overallAnsweredCorrectly, (val) => {
    allCorrectAnswers.value = abbreviateNumber(val);
    overallStartAngle.value = 100 - (100 / overallAnswerCount.value * overallAnsweredCorrectly.value);
})

watch(overallAnsweredWrongly, (val) => {
    allWrongAnswers.value = abbreviateNumber(val);
    overallStartAngle.value = 100 - (100 / overallAnswerCount.value * overallAnsweredCorrectly.value);
})


</script>

<template>
    <div>
        <div id="QuestionDetailsApp">
            <div class="separationBorderTop" style="min-height: 20px;"></div>

            <div id="questionDetailsContainer" class="row" style="min-height:265px">
                <div id="categoryList" class="col-sm-5" :class="{ isLandingPage: 'isLandingPage' }">
                    <div class="overline-s no-line">Themen</div>
                    <div class="categoryListChips">
                        <div style="display: flex; flex-wrap: wrap;">

                            <TopicChip v-for="(t, index) in topics" :key="index" :topic="t" :index="index"
                                :is-spoiler="learningSessionStore.isInTestMode" />

                        </div>
                    </div>
                </div>
                <div id="questionStatistics" class="col-sm-7 row">
                    <div id="probabilityContainer" class="col-sm-6" ref="probabilityContainer">
                        <div class="overline-s no-line">Antwortwahrscheinlichkeit</div>
                        <div id="semiPieSection">
                            <div id="semiPieChart" style="min-height:130px">
                                <div class="semiPieSvgContainer" ref="semiPie"
                                    :class="{ 'isInWishknowledge': isInWishknowledge }">
                                </div>
                            </div>
                            <div id="probabilityText">
                                <div v-if="userStore.isLoggedIn" style="">
                                    <strong>{{ personalProbability }}%</strong> beträgt die Wahrscheinlichkeit, dass du
                                    die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer:
                                    <strong>{{ avgProbability }}%</strong>
                                </div>
                                <div v-else style="">
                                    <strong>{{ personalProbability }}%</strong> beträgt die Wahrscheinlichkeit, dass du
                                    die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle
                                    Wahrscheinlichkeit berechnen können.
                                </div>
                            </div>
                        </div>

                    </div>
                    <div id="counterContainer" class="col-sm-6" style="font-size:12px">
                        <div class="overline-s no-line">Antworten</div>

                        <div class="counterBody">
                            <div class="counterHalf">
                                <div ref="personalCounter" style="min-width:50px"></div>
                                <div v-if="personalAnswerCount > 0" class="counterLabel">
                                    Von Dir: <br />
                                    <strong>{{ answerCount }}</strong> mal beantwortet <br />
                                    <strong>{{ correctAnswers }}</strong> richtig / <strong>{{ wrongAnswers }}</strong>
                                    falsch
                                </div>
                                <div v-else-if="userStore.isLoggedIn" class="counterLabel">
                                    Du hast diese Frage noch nie beantwortet.
                                </div>
                                <div v-else class="counterLabel">
                                    Du bist nicht angemeldet. Wir haben keine Daten. <a role="button"
                                        @click="userStore.openLoginModal()">Anmelden</a>
                                </div>
                            </div>
                            <div class="counterHalf">
                                <div ref="overallCounter" style="min-width:50px"></div>
                                <div v-if="overallAnswerCount > 0" class="counterLabel">
                                    Von allen Nutzern: <br />
                                    <strong>{{ allAnswerCount }}</strong> mal beantwortet <br />
                                    <strong>{{ allCorrectAnswers }}</strong> richtig /
                                    <strong>{{ allWrongAnswers }}</strong> falsch
                                </div>
                                <div v-else class="counterLabel">
                                    <template v-if="visibility == 1">
                                        Diese Frage ist <br />
                                        privat und nur für <br />
                                        dich sichtbar
                                    </template>
                                    <template v-else>
                                        Diese Frage wurde noch nie beantwortet.
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
            <!-- <div class="questionDetailsFooterPartialLeft">

                <div id="LicenseQuestion">
                    <% if (Model.LicenseQuestion.IsDefault()) { %>
                        <a class="TextLinkWithIcon" rel="license" href="http://creativecommons.org/licenses/by/4.0/"
                            data-toggle="popover" data-trigger="focus"
                            title="Infos zur Lizenz <%= LicenseQuestionRepo.GetDefaultLicense().NameShort %>"
                            data-placement="auto top"
                            data-content="Autor: <a href='<%= Links.UserDetail(Model.Creator) %>' <%= Model.IsInWidget ? "
                            target='_blank'" : "" %>><%= Model.Creator.Name %></a><%= Model.IsInWidget ? " (Nutzer auf
                            <a href='/' target='_blank'>memucho.de</a>)" : " " %><br />
                        <%= LicenseQuestionRepo.GetDefaultLicense().DisplayTextFull %>">
                            <div> <img src="/Images/Licenses/cc-by 88x31.png" width="60"
                                    style="margin-top: 4px; opacity: 0.6; padding-bottom: 2px;" />&nbsp;</div>
                            <div class="TextDiv"> <span class="TextSpan">
                                    <%= LicenseQuestionRepo.GetDefaultLicense().NameShort %>
                                </span></div>
                            </a><%--target blank to open outside the iframe of widget--%>

                                <% } else { %>
                                    <a class="TextLinkWithIcon" href="#" data-toggle="popover" data-trigger="focus"
                                        title="Infos zur Lizenz" data-placement="auto top"
                                        data-content="<%= Model.LicenseQuestion.DisplayTextFull %>">
                                        <div class="TextDiv"><span class="TextSpan">
                                                <%= Model.LicenseQuestion.DisplayTextShort %>
                                            </span>&nbsp;&nbsp;<i class="fa fa-info-circle">&nbsp;</i></div>
                                    </a>
                                    <% } %>
                </div>
                <div class="created"> Erstellt von: <a href="<%= Links.UserDetail(Model.Creator) %>">
                        <%= Model.Creator.Name %>
                    </a> vor <%= Model.CreationDateNiceText %>
                </div>
            </div>

            <div class="questionDetailsFooterPartialRight">
                <% if (PermissionCheck.CanView(Model.Question)){ %>
                    <div class="wishknowledgeCount"><i class="fas fa-heart"></i><span id="<%= " WishknowledgeCounter-" +
                            Model.QuestionId %>" data-relevance="<%= Model.IsInWishknowledge %>"><%=
                                    Model.Question.TotalRelevancePersonalEntries %></span></div>
                    <div class="viewCount"><i class="fas fa-eye"></i><span>
                            <%= Model.Question.TotalViews %>
                        </span></div>
                    <div class="commentCount pointer"
                        onclick="eventBus.$emit('show-comment-section-modal', <%=Model.QuestionId%>)"><a><i
                                class="fas fa-comment"></i><span id="commentCountAnswerBody">
                                <%= Model.UnsettledCommentCount %>
                            </span></a></div>
                    <%}%>
            </div> -->
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

.SetAndCategory {
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

    .category-set {
        display: flex;

        .label-category {
            .SetAndCategory();
        }

        .label-set {
            .SetAndCategory();
        }

        #Category {
            .font-size12();
        }

        .fa-chevron-right {
            margin-top: 4px;
        }
    }

    .second-row {
        padding-left: 0px;
        display: flex;

        .fa-eye {
            padding-right: 3px;
        }

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

.model-categories {
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

#QuestionDetailsApp {
    .separationBorderTop {
        min-height: 20px;
    }

    #questionDetailsContainer {
        padding-left: 15px;
        padding-right: 12px;

        @media(max-width:767px) {
            display: flex;
            flex-wrap: wrap;
            flex-flow: column-reverse;
        }

        .sectionLabel {
            text-transform: uppercase;
            font-family: 'Open Sans';
            font-size: 12px;
            font-weight: 600;
        }


        #categoryList {
            padding-bottom: 30px;

            &.isLandingPage {}

            .categoryListChips {
                @media(max-width:479px) {
                    padding-top: 10px;
                }

                overflow: hidden
            }

            .categoryListLinks {
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

                #semiPieSection {
                    @media (max-width:767px) {
                        display: flex;
                    }

                    @media (max-width:595px) {
                        padding-bottom: 10px;
                    }

                    #semiPieChart {
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

                                &.isInWishknowledge {

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

        #LicenseQuestion {
            padding-right: 10px;

            a.TextLinkWithIcon {
                flex-direction: unset !important;

                img {
                    margin: 0 8px 0 0 !important;
                }
            }
        }
    }

    .questionDetailsFooterPartialRight {
        width: 40%;
        display: flex;
        justify-content: flex-end;
        padding-right: 4px;

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
</style>