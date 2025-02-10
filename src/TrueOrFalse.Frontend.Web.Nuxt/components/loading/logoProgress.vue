<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref, watch } from 'vue'
import { useLoadingStore } from './loadingStore'

const loadingStore = useLoadingStore()

const wavePath = ref<SVGPathElement | null>(null)
let pathLength = 0

const PARTIAL_FILL_RATIO = 0.1

const PARTIAL_FILL_DURATION = 3000
const FINAL_FILL_DURATION = 1000

onMounted(() => {
    if (!wavePath.value) return

    pathLength = wavePath.value.getTotalLength()
    wavePath.value.style.strokeDasharray = String(pathLength)
    wavePath.value.style.strokeDashoffset = String(pathLength)

    startPartialFill()
})

onBeforeUnmount(() => {
    resetBar()
})

function startPartialFill() {
    if (!wavePath.value) return

    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength}`
    void wavePath.value.getBoundingClientRect()

    wavePath.value.style.transition = `stroke-dashoffset ${PARTIAL_FILL_DURATION}ms ease-out`
    wavePath.value.style.strokeDashoffset = `${pathLength * PARTIAL_FILL_RATIO}`

    wavePath.value.addEventListener('transitionend', onPartialFillEnd, { once: true })

    const unwatch = watch(
        () => loadingStore.isDone,
        (done) => {
            if (done) {
                wavePath.value?.removeEventListener('transitionend', onPartialFillEnd)
                doFinalFill()
                unwatch()
            }
        }
    )
}

function onPartialFillEnd() {
    if (loadingStore.isDone) {
        doFinalFill()
    } else {
        holdAtPartialFill()
    }
}

function holdAtPartialFill() {
    if (!wavePath.value) return

    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength * PARTIAL_FILL_RATIO}`

    const unwatch = watch(
        () => loadingStore.isDone,
        (done) => {
            if (done) {
                doFinalFill()
                unwatch()
            }
        }
    )
}

function doFinalFill() {
    if (!wavePath.value) return

    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength * PARTIAL_FILL_RATIO}`
    void wavePath.value.getBoundingClientRect()

    wavePath.value.style.transition = `stroke-dashoffset ${FINAL_FILL_DURATION}ms ease-out`
    wavePath.value.style.strokeDashoffset = '0'
    wavePath.value.addEventListener('transitionend', handleFinalTransitionEnd, { once: true })
}

function handleFinalTransitionEnd() {
    loadingStore.stopLoading()
}
function resetBar() {
    if (!wavePath.value) return

    wavePath.value.removeEventListener('transitionend', onPartialFillEnd)
    wavePath.value.removeEventListener('transitionend', handleFinalTransitionEnd)
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength}`
}
</script>

<template>
    <div class="progress-container">
        <svg
            width="169"
            height="70"
            viewBox="0 0 169 70"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            class="progress-svg">
            <path
                ref="wavePath"
                d="M24 41.9995
           L42.1188 14.9375
           L52.1188 56.9994
           L80.5346 15.4323
           L89.1485 55.5148
           L116.871 14.9375
           L126.871 56.5045
           L145 29.9995"
                stroke="url(#paint0_linear)"
                stroke-width="19"
                stroke-linecap="round"
                stroke-linejoin="round" />
            <circle cx="9.5" cy="60.5" r="9.5" fill="#FFA07A" />
            <circle cx="159.5" cy="9.5" r="9.5" fill="#AFD534" />

            <defs>
                <linearGradient
                    id="paint0_linear"
                    x1="24"
                    y1="46"
                    x2="145"
                    y2="26"
                    gradientUnits="userSpaceOnUse">
                    <stop offset="0" stop-color="#FFA07A" />
                    <stop offset="0.45" stop-color="#F4E13A" />
                    <stop offset="1" stop-color="#AFD534" />
                </linearGradient>
            </defs>
        </svg>
    </div>
</template>

<style scoped>
.progress-container {
    display: flex;
    align-items: center;
    justify-content: center;
}

.progress-svg {
    width: 200px;
}
</style>
