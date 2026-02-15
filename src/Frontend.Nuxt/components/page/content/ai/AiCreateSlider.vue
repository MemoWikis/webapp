<script lang="ts" setup>
interface Props {
    modelValue: number
    min: number
    max: number
    labels: Record<number, string>
    leftLabel: string
    rightLabel: string
    disabled?: boolean
}

const props = defineProps<Props>()
const emit = defineEmits<{ 'update:modelValue': [value: number] }>()

const { isMobile } = useDevice()
const dropdownAriaId = useId()

const currentLabel = computed(() => props.labels[props.modelValue] ?? '')

function sliderBackground(): string {
    const percentage = ((props.modelValue - props.min) / (props.max - props.min)) * 100
    return `linear-gradient(to right, #101010 0%, #101010 ${percentage}%, #EFEFEF ${percentage}%, #EFEFEF 100%)`
}

const sliderStyle = computed(() => ({
    background: sliderBackground()
}))

function onSliderInput(event: Event) {
    const target = event.target as HTMLInputElement
    emit('update:modelValue', Number(target.value))
}
</script>

<template>
    <div class="ai-create-slider">
        <!-- Desktop: Slider -->
        <div v-if="!isMobile" class="slider-container">
            <input type="range" class="slider-input" :value="props.modelValue" :min="props.min" :max="props.max"
                :style="sliderStyle" :disabled="props.disabled" :aria-valuetext="currentLabel" @input="onSliderInput" />
            <div class="slider-labels">
                <span class="slider-label-left">{{ props.leftLabel }}</span>
                <span class="slider-label-current">{{ currentLabel }}</span>
                <span class="slider-label-right">{{ props.rightLabel }}</span>
            </div>
        </div>

        <!-- Mobile: Dropdown -->
        <VDropdown v-else :aria-id="dropdownAriaId" :distance="0" class="slider-dropdown">
            <div class="slider-select">
                <span>{{ currentLabel }}</span>
                <font-awesome-icon :icon="['fas', 'chevron-down']" />
            </div>

            <template #popper="{ hide }">
                <div class="slider-dropdown-menu detail-dropdown-popper">
                    <div v-for="(label, level) in props.labels" :key="level" class="dropdown-row"
                        :class="{ active: props.modelValue === Number(level) }"
                        @click="emit('update:modelValue', Number(level)); hide()">
                        {{ label }}
                    </div>
                </div>
            </template>
        </VDropdown>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ai-create-slider {
    .slider-container {
        padding: 0 8px;
    }

    .slider-input {
        width: 100%;
        height: 6px;
        -webkit-appearance: none;
        appearance: none;
        background: @memo-grey-lighter;
        border-radius: 3px;
        outline: none;
        cursor: pointer;
        user-select: none;

        &::-webkit-slider-thumb {
            -webkit-appearance: none;
            appearance: none;
            width: 18px;
            height: 18px;
            background: white;
            border: 2px solid @memo-grey-darker;
            border-radius: 50%;
            cursor: pointer;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.15);
        }

        &::-moz-range-thumb {
            width: 18px;
            height: 18px;
            background: white;
            border: 2px solid @memo-grey-darker;
            border-radius: 50%;
            cursor: pointer;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.15);
        }

        &::-moz-range-progress {
            background: @memo-grey-darkest;
            border-radius: 3px;
            height: 6px;
        }
    }

    .slider-labels {
        display: flex;
        justify-content: space-between;
        margin-top: 8px;
        font-size: 12px;
        color: @memo-grey-dark;

        * {
            width: 33.3333%;
        }

        .slider-label-current {
            text-align: center;
            font-weight: 600;
            color: @memo-blue;
        }

        .slider-label-right {
            text-align: right;
        }
    }

    .slider-select {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        padding: 12px;
        border: 1px solid @memo-grey-lighter;
        border-radius: 0px;
        background: white;
        font-size: 14px;
        font-weight: 500;
        color: inherit;
        cursor: pointer;

        &:hover {
            filter: brightness(0.95);
        }
    }
}
</style>
