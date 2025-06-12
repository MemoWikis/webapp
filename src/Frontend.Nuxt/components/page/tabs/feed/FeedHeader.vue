<script setup lang="ts">
import { useUserStore } from '~/components/user/userStore'

const { t } = useI18n()
const userStore = useUserStore()

interface Props {
    getDescendants: boolean
    getQuestions: boolean
    getGroups: boolean
}

interface Emits {
    (e: 'update:getDescendants', value: boolean): void
    (e: 'update:getQuestions', value: boolean): void
    (e: 'update:getGroups', value: boolean): void
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const ariaId = useId()

const toggleDescendants = () => {
    emit('update:getDescendants', !props.getDescendants)
}

const toggleQuestions = () => {
    emit('update:getQuestions', !props.getQuestions)
}

const toggleGroups = () => {
    emit('update:getGroups', !props.getGroups)
}
</script>

<template>
    <div class="header">
        <VDropdown :aria-id="ariaId" :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
            <div class="feed-settings-btn">
                <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
            </div>
            <template #popper>
                <div class="checkbox-container dropdown-row" @click="toggleDescendants">
                    <label>{{ t('page.feed.includeSubpages') }}</label>
                    <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getDescendants" class="active" />
                    <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                </div>

                <template v-if="userStore.isAdmin">
                    <div class="checkbox-container dropdown-row" @click="toggleGroups">
                        <label>{{ t('page.feed.groupItems') }}</label>
                        <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getGroups" class="active" />
                        <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                    </div>

                    <div class="checkbox-container dropdown-row" @click="toggleQuestions">
                        <label>{{ t('page.feed.includeQuestions') }}</label>
                        <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getQuestions" class="active" />
                        <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                    </div>
                </template>
            </template>
        </VDropdown>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.header {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    z-index: 2;
    margin-bottom: -24px;
    margin-top: 8px;
    position: absolute;
    right: 0;
}

.feed-settings-btn {
    cursor: pointer;
    font-size: 18px;
    color: @memo-grey-dark;
    background: white;
    border-radius: 44px;
    width: 32px;
    height: 32px;
    display: flex;
    justify-content: center;
    align-items: center;
    user-select: none;
    z-index: 5;

    &:hover {
        filter: brightness(0.95);
    }

    &:active {
        filter: brightness(0.9);
    }
}

.checkbox-container {
    padding: 2px 8px;
    user-select: none;
    display: flex;
    align-items: center;
    justify-content: space-between;
    cursor: pointer;
    margin-top: 4px;
    background: white;
    z-index: 2;

    label {
        margin-bottom: 0;
        color: @memo-grey-dark;
        cursor: pointer;
        z-index: 2;
    }

    .active,
    .not-active {
        margin-left: 8px;
        font-size: 1.8em;
    }

    .active {
        color: @memo-blue-link;
    }

    .not-active {
        color: @memo-grey-light;
    }
}
</style>
