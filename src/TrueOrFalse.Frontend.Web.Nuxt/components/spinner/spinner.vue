<script lang="ts" setup>
import { useSpinnerStore } from './spinnerStore'
const spinnerStore = useSpinnerStore()
</script>

<template>
    <div v-if="spinnerStore.active">
        <Teleport to="body">
            <div class="spinner">
                <div class="double-bounce1"></div>
                <div class="double-bounce2"></div>
            </div>
        </Teleport>
    </div>
</template>

<style scoped lang="less">
.spinner {
    width: 75px;
    height: 75px;
    position: fixed;
    margin-top: 300px;
    left: calc(50% - 37.5px);
    text-align: center;
    z-index: 99999;
}

.double-bounce1,
.double-bounce2 {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    background-color: #003264;
    opacity: 0.6;
    position: absolute;
    -webkit-animation: sk-bounce 2.0s infinite ease-in-out;
    animation: sk-bounce 2.0s infinite ease-in-out;
}

.double-bounce2 {
    -webkit-animation-delay: -1.0s;
    animation-delay: -1.0s;
}

@-webkit-keyframes sk-bounce {

    0%,
    100% {
        -webkit-transform: scale(0.0)
    }

    50% {
        -webkit-transform: scale(1.0)
    }
}

@keyframes sk-bounce {

    0%,
    100% {
        transform: scale(0.0);
        -webkit-transform: scale(0.0);
    }

    50% {
        transform: scale(1.0);
        -webkit-transform: scale(1.0);
    }
}
</style>