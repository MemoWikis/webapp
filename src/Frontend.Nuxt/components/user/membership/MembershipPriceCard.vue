<script lang="ts" setup>
import { Plan } from './subscription'

interface Props {
    plan: Plan,
    selected: boolean
}

const props = defineProps<Props>()
</script>

<template>
    <div class="card">
        <div class="price-inner">
            <div class="header">
                <div class="head-line">
                    {{ props.plan.name }}
                </div>
                <div class="price-container">
                    <div class="price" v-if="typeof props.plan.price === 'number'">
                        {{ props.plan.price }}€
                    </div>
                    <div class="price text" v-else>
                        {{ props.plan.price }}
                    </div>
                    <div class="price-label" v-html="props.plan.priceLabel">
                    </div>
                </div>
            </div>
            <div class="button-container">
                <slot name="button"></slot>
            </div>
            <div class="description">
                <p v-for="item in props.plan.description">
                    {{ item }}
                </p>
            </div>
            <div class="list-container">
                <div v-if="props.plan.listLabel">
                    <b>{{ props.plan.listLabel }}</b>
                </div>
                <div v-for="item in props.plan.list" class="list-item">
                    <div class="icon-container">
                        <font-awesome-icon :icon="['fa-solid', 'fa-check']" />
                    </div>
                    <div v-html="item">
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.card {
    margin-top: 10px;
    width: calc(50% - 1rem);
    margin-bottom: 10px;

    @media (max-width: 580px) {
        width: 100%;
    }
}

.sidesheet-open {
    .card {

        @media (max-width: 1000px) {
            width: calc(100% - 1rem);
        }

    }
}

.recommended {
    .price-inner {
        box-shadow: @memo-blue-link 0px 0px 0px 2px;
        // border: solid 1px @memo-blue-link;
    }
}

.selected {
    .price-inner {
        box-shadow: @memo-green 0px 0px 0px 2px;

        // border: solid 1px @memo-green;
    }
}

.price-inner {
    box-shadow: @memo-grey-light 0px 0px 0px 1px;
    height: 100%;
    padding: 20px;

    @media screen and (max-width: @screen-sm) {
        height: unset;
        padding-bottom: 40px;
    }

    .head-line {
        font-size: 32px;
        font-weight: 400;
        height: 42px;
    }

    .contact {
        color: #0065CA;
        background: white;
        font-size: 14px;
    }

    .price-container {
        margin-top: 50px;
        height: 90px;

        .price {
            font-size: 45px;
            font-weight: 700;
            height: 42px;
            line-height: 42px;
            margin-bottom: 10px;

            &.text {
                font-size: 18px;
                line-height: 24px;
                height: 24px;
            }
        }
    }

    .price-organisation {
        margin-top: 50px;
        font-weight: 700;
        font-size: 18px;

    }

    .price-label {
        color: @memo-grey-dark;
        min-height: 60px;
    }

    .button-container {
        margin-top: 20px;
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .description {
        margin: 40px 0;
        color: @memo-grey-darker;
        height: 150px;

        @media screen and (min-width: 386px) and (max-width: @screen-sm) {
            height: 90px;
        }
    }

    .list-container {
        font-weight: 400;
        font-size: 14px;
        color: @memo-blue;

        .list-item {
            display: flex;

            .icon-container {
                min-width: 24px;
                width: 24px;
                padding-right: 4px;

                .fa-check {
                    font-size: 18px;
                    font-weight: 900;
                    color: @memo-blue-link;
                }
            }
        }
    }
}
</style>