
<script lang="ts" setup>
import { Plan } from './membership'

interface Props {
    plan: Plan,
    selected: boolean
}

const props = defineProps<Props>()
</script>

<template>
    <div class="col-sm-6 col-md-6 col-lg-3">
        <div class="price-inner">
            <div class="header">
                <div class="head-line">
                    {{ props.plan.name }}
                </div>
                <div class="price">
                    <span v-if="typeof props.plan.price === 'number'">
                        {{ props.plan.price }}€
                    </span>
                    <span v-else class="on-request">
                        {{ props.plan.price }}
                    </span>

                </div>
                <div class="price-label">
                    {{ props.plan.priceLabel }}
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
                <div v-if="props.plan.listLabel">{{ props.plan.listLabel }}</div>
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

.price-inner {
    border: #DDDDDD 1px solid;
    padding: 20px;

    .head-line {
        font-size: 32px;
        margin-right: 20px;
        font-weight: 400;
    }

    .contact {
        color: #0065CA;
        background: white;
        font-size: 14px;

    }

    .disabled {
        background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7)), #0065CA;
    }

    .price {
        font-size: 45px;
        font-weight: 700;
        margin-top: 50px;
        height: 64px;

        .on-request {
            font-size: 32px;
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
    }

    .description {
        margin-top: 50px;
        color: #555555;
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
                }
            }
        }

    }
}
</style>