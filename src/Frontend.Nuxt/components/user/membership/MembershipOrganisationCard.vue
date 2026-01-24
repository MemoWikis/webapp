<script lang="ts" setup>
import { Plan } from './subscription'

interface Props {
    plan: Plan
}
const props = defineProps<Props>()
const { t } = useI18n()
</script>

<template>
    <div class="org-card">
        <div class="content">
            <h3 class="title">{{ props.plan.name }}</h3>
            <div class="description">
                <p v-for="(desc, idx) in props.plan.description" :key="idx" class="desc-line">{{ desc }}</p>
            </div>
            
            <div class="features-container" v-if="props.plan.list && props.plan.list.length > 0">
                <div class="feature-label" v-if="props.plan.listLabel">{{ props.plan.listLabel }}</div>
                <div class="feature-list">
                    <div v-for="(item, index) in props.plan.list" :key="index" class="feature-item">
                        <div class="icon-container">
                            <font-awesome-icon :icon="['fa-solid', 'fa-check']" />
                        </div>
                        <span>{{ item }}</span>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="action-column">
            <div class="price-info">
                <div class="price">{{ props.plan.price }}</div>
                <div class="price-label">{{ props.plan.priceLabel }}</div>
            </div>
            <div class="button-container">
                <slot name="button"></slot>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.org-card {
    background: white;
    border-radius: 8px;
    box-shadow: @memo-grey-light 0px 0px 0px 1px;
    padding: 2rem;
    display: grid;
    grid-template-columns: 2fr 1fr;
    gap: 3rem;
    width: 100%;
    align-items: center;

    @media (max-width: 900px) {
        grid-template-columns: 1fr;
        gap: 2rem;
        padding: 1.5rem;
    }

    .content {
        .title {
            font-size: 28px;
            font-weight: 500;
            margin-bottom: 1rem;
            color: @memo-blue;
        }

        .description {
            font-size: 16px;
            color: @memo-grey-darker;
            margin-bottom: 1.5rem;
            line-height: 1.5;
            
            p {
                margin-bottom: 0.5rem;
            }
        }

        .features-container {
            .feature-label {
                font-weight: 600;
                margin-bottom: 1rem;
                font-size: 14px;
                color: @memo-grey-darker;
            }

            .feature-list {
                display: flex;
                flex-wrap: wrap;
                gap: 1.5rem;

                .feature-item {
                    display: flex;
                    align-items: center;
                    gap: 0.75rem;
                    font-size: 14px;
                    color: @memo-grey-darker;

                    .icon-container {
                        color: @memo-blue-link;
                    }
                }
            }
        }
    }

    .action-column {
        display: flex;
        flex-direction: column;
        align-items: center;
        text-align: center;
        padding-left: 2rem;
        border-left: 1px solid @memo-grey-light;

        @media (max-width: 900px) {
            padding-left: 0;
            border-left: none;
            border-top: 1px solid @memo-grey-light;
            padding-top: 2rem;
        }

        .price-info {
            margin-bottom: 1.5rem;

            .price {
                font-size: 28px;
                font-weight: 700;
                margin-bottom: 0.5rem;
                color: @memo-grey-darkest;
            }

            .price-label {
                color: @memo-grey-dark;
                font-size: 14px;
                line-height: 1.4;
            }
        }
        
        .button-container {
            width: 100%;
            max-width: 250px;
        }
    }
}
</style>
