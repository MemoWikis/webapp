<script lang="ts" setup>
</script>

<template>
    <div id="Spinner" class="spinner">
        <div class="double-bounce1"></div>
        <div class="double-bounce2"></div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#Spinner {
    width: 75px;
    height: 75px;
    left: calc(50% - 37.5px);
}

.double-bounce1,
.double-bounce2 {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    opacity: 0.6;
    position: absolute;
    -webkit-animation: sk-bounce 2.0s infinite ease-in-out;
    animation: sk-bounce 2.0s infinite ease-in-out;
}

.double-bounce1 {
    background-color: @memo-blue;
}

.double-bounce2 {
    background-color: @memo-blue-link;
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