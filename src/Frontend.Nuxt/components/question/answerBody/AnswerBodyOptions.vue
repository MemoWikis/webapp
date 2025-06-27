<script setup lang="ts">
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { useEditQuestionStore } from '../edit/editQuestionStore'
import { useUserStore } from '~/components/user/userStore'
import { useCommentsStore } from '~/components/comment/commentsStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'

const tabsStore = useTabsStore()
const editQuestionStore = useEditQuestionStore()
const userStore = useUserStore()
const commentsStore = useCommentsStore()
const deleteQuestionStore = useDeleteQuestionStore()

interface Props {
    canEdit: boolean,
    id: number,
    title: string,
}

const props = defineProps<Props>()

const showDropdown = ref(false)
const ariaId = useId()

const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
</script>

<template>
    <div class="Button dropdown answerbody-btn" @click="showDropdown = !showDropdown">

        <div class="answerbody-btn-inner">
            <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
        </div>

        <VDropdown :aria-id="ariaId" :distance="0" :shown="showDropdown">
            <template #popper="{ hide }">

                <div class="dropdown-row" v-if="tabsStore.activeTab === Tab.Learning && props.canEdit"
                    @click="editQuestionStore.editQuestion(props.id); hide()">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-pen" />
                    </div>
                    <div class="dropdown-label">
                        {{ t('answerbody.options.editQuestion') }}
                    </div>

                </div>

                <LazyNuxtLink :to="$urlHelper.getQuestionUrl(props.title, props.id)"
                    v-if="tabsStore.activeTab === Tab.Learning && userStore.isAdmin">
                    <div class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-file" />
                        </div>
                        <div class="dropdown-label">
                            {{ t('answerbody.options.showQuestionPage') }}
                        </div>
                    </div>
                </LazyNuxtLink>

                <LazyNuxtLink :to="`/QuestionHistory/${props.title}/${props.id}`"
                    v-if="tabsStore.activeTab === Tab.Learning && userStore.isAdmin">
                    <div class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-code-fork" />
                        </div>
                        <div class="dropdown-label">
                            {{ t('answerbody.options.showEditHistory') }}
                        </div>
                    </div>
                </LazyNuxtLink>

                <div class="dropdown-row" @click="commentsStore.openModal(props.id); hide()">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-comment" />
                    </div>
                    <div class="dropdown-label">
                        {{ t('answerbody.options.comment') }}
                    </div>
                </div>

                <div class="dropdown-row" @click="deleteQuestionStore.openModal(props.id); hide()"
                    v-if="userStore.isLoggedIn && props.canEdit">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-trash" />
                    </div>
                    <div class="dropdown-label">
                        {{ t('answerbody.options.delete') }}
                    </div>
                </div>

            </template>
        </VDropdown>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.answerbody-btn {
    font-size: 18px;
    user-select: none;

    .answerbody-btn-inner {
        cursor: pointer;
        background: white;
        height: 32px;
        width: 32px;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 15px;

        .fa-ellipsis-vertical {
            color: @memo-grey-dark;
        }

        &:hover {
            filter: brightness(0.95);
        }

        &:active {
            filter: brightness(0.85);
        }
    }
}
</style>