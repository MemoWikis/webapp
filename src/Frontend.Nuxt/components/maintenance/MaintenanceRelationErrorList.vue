<script lang="ts" setup>
interface RelationError {
    type: string
    childId: number
    description: string
}

interface ErrorGroup {
    key: string
    errors: RelationError[]
}

interface Props {
    group: ErrorGroup
    selectedChildId: number | null
}

const props = defineProps<Props>()

const emit = defineEmits<{
    clickChildId: [childId: number]
}>()

const handleClickChildId = (childId: number) => {
    emit('clickChildId', childId)
}

const showList = ref<boolean>(true)

watch(() => props.group, () => {
    showList.value = true
}, { immediate: true, deep: true })

</script>

<template>
    <div class="error-group">
        <div class="error-group-header" @click="showList = !showList">{{ group.key }}</div>
        <Transition name="fade">
            <ul class="error-list" v-show="showList">
                <li v-for="error in group.errors" :key="`${error.childId}-${error.type}`"
                    :class="[{ 'selected': selectedChildId === error.childId, 'no-selection': error.type.toLowerCase() === 'brokenorder' }]" @click="handleClickChildId(error.childId)"
                    class="error-item">
                    <strong>{{ error.type }}:</strong> {{ error.description }}
                    <font-awesome-icon :icon="['fas', 'caret-left']" v-if="selectedChildId === error.childId" />
                </li>
            </ul>
        </Transition>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.error-group {
    margin-bottom: 20px;

    &:last-child {
        margin-bottom: 15px;
    }

    .error-group-header {
        font-weight: 600;
        color: @memo-grey-darker;
        margin-bottom: 8px;
        font-size: 14px;
        cursor: pointer;
        user-select: none;

        &:hover {
            color: @memo-blue;
        }
    }
}

.error-list {
    margin: 0 0 0 0;
    padding-left: 20px;

    .error-item {
        cursor: pointer;
        transition: background-color 0.3s;
        padding: 0 8px;
        border-radius: 4px;

        &:hover {
            background-color: fade(@memo-grey-lighter, 50%);
        }

        &.selected {
            background-color: @memo-grey-lightest;
        }

        &.no-selection {
            background-color: unset;
            cursor: unset;

            &:hover {
                background-color: unset;
            }
        }
    }

    li {
        margin-bottom: 8px;
        padding: 4px 0;

        strong {
            font-weight: 600;
        }
    }
}
</style>
