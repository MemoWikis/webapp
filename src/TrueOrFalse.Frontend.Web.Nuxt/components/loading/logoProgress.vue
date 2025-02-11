<script setup lang="ts">
import { useLoadingStore } from './loadingStore'

const loadingStore = useLoadingStore()

const wavePath = ref<SVGPathElement | null>(null)
const pathLength = ref(0)

const partialFillRatio = ref(0.1)

const dots = ref('')
let interval: ReturnType<typeof setInterval> | null = null

onMounted(() => {
    if (!wavePath.value) return
    pathLength.value = wavePath.value.getTotalLength()
    wavePath.value.style.strokeDasharray = `${pathLength.value}`

    if (loadingStore.isDone) {
        doInstantFinalFill()
        return
    }

    wavePath.value.style.strokeDashoffset = `${pathLength.value}`
    startPartialFill()
})

onBeforeUnmount(() => {
    cleanup()
})

const startDotsInterval = () => {
    dots.value = ''
    interval = setInterval(() => {
        dots.value = dots.value.length < 3 ? dots.value + '.' : ''
    }, 500)
}

const cleanup = () => {
    if (!wavePath.value) return
    wavePath.value.removeEventListener('transitionend', onPartialFillEnd)
    wavePath.value.removeEventListener('transitionend', onFinalTransitionEnd)
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength.value}`

    if (interval) {
        clearInterval(interval)
        interval = null
    }
}

const startPartialFill = () => {
    if (!wavePath.value) return
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${pathLength.value}`
    void wavePath.value.getBoundingClientRect()
    startDotsInterval()
    wavePath.value.style.transition = `stroke-dashoffset ${loadingStore.loadingDuration}ms cubic-bezier(0.2, 0, 0, 1)`
    wavePath.value.style.strokeDashoffset = `${pathLength.value * partialFillRatio.value}`
    wavePath.value.addEventListener('transitionend', onPartialFillEnd, { once: true })

    const unwatch = watch(
        () => loadingStore.isDone,
        (done) => {
            if (done) {
                wavePath.value?.removeEventListener('transitionend', onPartialFillEnd)
                doFinalFillEarly()
                unwatch()
            }
        }
    )
}

const onPartialFillEnd = () => {
    if (!wavePath.value) return
    const computedOffset = parseFloat(
        window.getComputedStyle(wavePath.value).getPropertyValue('stroke-dashoffset')
    )

    if (loadingStore.isDone) {
        doFinalFill(computedOffset)
    } else {
        holdAtPartialFill(computedOffset)
    }
}

const holdAtPartialFill = (offset: number) => {
    if (!wavePath.value) return
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${offset}`

    const unwatch = watch(
        () => loadingStore.isDone,
        (done) => {
            if (done) {
                doFinalFill(offset)
                unwatch()
            }
        }
    )
}

const doFinalFillEarly = () => {
    if (!wavePath.value) return
    const computedOffset = parseFloat(
        window.getComputedStyle(wavePath.value).getPropertyValue('stroke-dashoffset')
    )
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${computedOffset}`
    void wavePath.value.getBoundingClientRect()
    wavePath.value.style.transition = `stroke-dashoffset ${loadingStore.finalFillDuration}ms ease-out`
    wavePath.value.style.strokeDashoffset = '0'
    wavePath.value.addEventListener('transitionend', onFinalTransitionEnd, { once: true })
    stopDotsInterval()
}

const doFinalFill = (offset: number) => {
    if (!wavePath.value) return
    wavePath.value.style.transition = 'none'
    wavePath.value.style.strokeDashoffset = `${offset}`
    void wavePath.value.getBoundingClientRect()
    wavePath.value.style.transition = `stroke-dashoffset ${loadingStore.finalFillDuration}ms ease-out`
    wavePath.value.style.strokeDashoffset = '0'
    wavePath.value.addEventListener('transitionend', onFinalTransitionEnd, { once: true })
    stopDotsInterval()
}

const doInstantFinalFill = () => {
    if (!wavePath.value) return
    wavePath.value.style.strokeDashoffset = `${pathLength.value}`
    void wavePath.value.getBoundingClientRect()
    wavePath.value.style.transition = `stroke-dashoffset 500ms ease-out`
    wavePath.value.style.strokeDashoffset = '0'
    wavePath.value.addEventListener('transitionend', onFinalTransitionEnd, { once: true })
}

const onFinalTransitionEnd = () => {
    loadingStore.stopLoading()
}

const stopDotsInterval = () => {
    if (interval) {
        clearInterval(interval)
        interval = null
    }
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
        <div class="progress-label">Karteikarten werden generiert<span class="trailing-dots">{{ dots }}</span></div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.progress-container {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    width: 260px;

    .progress-svg {
        width: 200px;
    }

    .progress-label {
        font-size: 1.4rem;
        margin-top: 3rem;
        width: 220px;
        color: @memo-grey-darker;
        text-align: center;

        span.trailing-dots {
            position: fixed;
        }
    }
}
</style>
