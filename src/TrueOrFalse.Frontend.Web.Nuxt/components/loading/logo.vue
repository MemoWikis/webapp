<script lang="ts" setup>

const wavePath = ref<SVGPathElement>()

onMounted(() => {

    if (wavePath.value) {
        const length = wavePath.value.getTotalLength()

        wavePath.value.style.strokeDasharray = String(length)
        wavePath.value.style.strokeDashoffset = String(length)

        wavePath.value.classList.add('animate-wave')
    }

})
</script>

<template>
    <svg
        ref="svgRef"
        width="169"
        height="70"
        viewBox="0 0 169 70"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
        class="wave-logo">
        <path
            ref="wavePath"
            d="M24 40.0005
                L42.129 13.4955
                L52.129 55.0625
                L79.8515 14.4852
                L88.4654 54.5677
                L116.8812 13.0006
                L126.8812 55.0625
                L145 28.0005"
            stroke="url(#paint0_linear_579_11675)"
            stroke-width="19"
            stroke-linecap="round"
            stroke-linejoin="round" />

        <circle cx="9.5" cy="60.5" r="9.5" fill="#FFA07A" />
        <circle cx="159.5" cy="9.5" r="9.5" fill="#AFD534" />

        <defs>
            <linearGradient
                id="paint0_linear_579_11675"
                x1="24"
                y1="40.0005"
                x2="145"
                y2="28.0005"
                gradientUnits="userSpaceOnUse">
                <stop offset="0" stop-color="#FFA07A" />
                <stop offset="0.454885" stop-color="#F4E13A" />
                <stop offset="1" stop-color="#AFD534" />
            </linearGradient>
        </defs>
    </svg>



</template>

<style lang="less" scoped>
.wave-logo {
    width: 200px;
}

@keyframes waveDrawLeftToRight {
    0% {
        stroke-dashoffset: var(--waveLength);
    }

    100% {
        stroke-dashoffset: 0;
    }
}

.animate-wave {
    --waveLength: 294;

    animation: waveDrawLeftToRight 2s linear infinite;
}
</style>
