<script lang="ts" setup>
import { ref } from 'vue'
import { Tab } from '~~/components/topic/tabs/tabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore'

const tabsStore = useTabsStore()

</script>

<template>
    <div class="fab-container" v-show="contentIsReady">
        <template v-if="tabsStore.activeTab == Tab.Topic">
            <div class="edit-mode-bar-container" v-show="showBar">
                <div class="toolbar"
                    :class="{ 'pseudo-sticky': footerIsVisible, 'is-hidden': !editMode, 'shrink': shrink, 'expand': expand }"
                    :style="{ width: width + 'px' }">
                    <div class="toolbar-btn-container">
                        <div class="btn-left">
                        </div>
                        <div class="centerText" v-show="showLoginReminder">
                            <div>
                                Um zu speichern, musst du&nbsp;<a href="#" data-btn-login="true"
                                    onclick="eventBus.$emit('show-login-modal')"
                                    style="padding-top: 4px">angemeldet</a>&nbsp;sein.
                            </div>
                        </div>

                        <div v-if="showSaveMsg" class="saveMsg">
                            <div>
                                {{ saveMsg }}
                            </div>
                        </div>

                        <div class="btn-right" v-show="contentHasChanged" v-else>
                            <div class="button" @click.prevent="saveContent(changedCategoryName)"
                                :class="{ expanded: editMode }" v-ripple="{ center, class: 'r-green' }">
                                <div class="icon">
                                    <i class="fas fa-save"></i>
                                </div>
                                <div class="btn-label">
                                    Speichern
                                </div>
                            </div>

                            <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded: editMode }"
                                v-ripple="{ center, class: 'r-green' }">
                                <div class="icon">
                                    <i class="fas fa-times"></i>
                                </div>
                                <div class="btn-label">
                                    Verwerfen
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <template>
    </div>
</template>