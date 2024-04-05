<script setup lang="ts">
import { useTabsStore, Tab } from '~/components/topic/tabs/tabsStore'
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
</script>

<template>
    <div class="Button dropdown answerbody-btn" @click="showDropdown = !showDropdown">

        <div class="answerbody-btn-inner">
            <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
        </div>

        <VDropdown :distance="0" :shown="showDropdown">
            <template #popper="{ hide }">

                <div class="dropdown-row" v-if="tabsStore.activeTab == Tab.Learning && props.canEdit"
                    @click="editQuestionStore.editQuestion(props.id); hide()">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-pen" />
                    </div>
                    <div class="dropdown-label">Frage bearbeiten</div>

                </div>

                <LazyNuxtLink :to="$urlHelper.getQuestionUrl(props.title, props.id)"
                    v-if="tabsStore.activeTab == Tab.Learning && userStore.isAdmin">
                    <div class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-file" />
                        </div>
                        <div class="dropdown-label">Frageseite anzeigen</div>
                    </div>
                </LazyNuxtLink>

                <LazyNuxtLink :to="`/QuestionHistory/${props.title}/${props.id}`"
                    v-if="tabsStore.activeTab == Tab.Learning && userStore.isAdmin">
                    <div class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-code-fork" />
                        </div>
                        <div class="dropdown-label">Bearbeitungshistorie der Frage</div>
                    </div>
                </LazyNuxtLink>

                <div class="dropdown-row" @click="commentsStore.openModal(props.id); hide()">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-comment" />
                    </div>
                    <div class="dropdown-label">Frage kommentieren</div>
                </div>

                <div class="dropdown-row" @click="deleteQuestionStore.openModal(props.id); hide()"
                    v-if="userStore.isLoggedIn && props.canEdit">
                    <div class="dropdown-icon">
                        <font-awesome-icon icon="fa-solid fa-trash" />
                    </div>
                    <div class="dropdown-label">Frage löschen</div>
                </div>

            </template>
        </VDropdown>
    </div>
</template>