<script lang="ts" setup>
import { ref } from 'vue'
import { useFabStore } from './fabStore';
import { Tab } from '~~/components/topic/tabs/tabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore'
import { useTopicStore } from '~~/components/topic/topicStore'
const fabStore = useFabStore()
const tabsStore = useTabsStore()
const topicStore = useTopicStore()

const width = ref(0)
const editMode = ref(false)
const footerIsVisible = ref(false)
function footerCheck() {
    if (tabsStore.activeTab == Tab.Topic)
        return;

    var contentModuleAppWidth = document.getElementById('TopicContent').clientWidth;
    var windowWidth = window.innerWidth;
    const elFooter = document.getElementById('Segmentation');

    if (elFooter) {
        var rect = elFooter.getBoundingClientRect();
        var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
        if (rect.top - viewHeight >= 0) {
            if (footerIsVisible.value && editMode.value) {
                shrink.value = false;
                expand.value = true;
            }
            footerIsVisible.value = false;
            width.value = windowWidth;
        } else {
            if (!footerIsVisible.value && editMode.value) {
                expand.value = false;
                shrink.value = true;
            }
            footerIsVisible.value = true;
            width.value = contentModuleAppWidth;
        }
    };
}
const isExtended = ref(false)
function handleScroll() {
    if (window.scrollY == 0)
        isExtended.value = true;
    else isExtended.value = false;
    fabStore.open = false;
    footerCheck();
}
onMounted(() => {
    footerCheck()
    window.addEventListener('scroll', handleScroll);
    window.addEventListener('resize', footerCheck);
})

const shrink = ref(false)
const expand = ref(false)

const isLoggedIn = useState('isLoggedIn')

const showSaveMsg = ref(false)
const saveMsg = ref('')
const newCategoryName = ref('')
function cancelEditMode() {
    // this.isOpen = false;
    // this.shrink = false;
    // this.expand = false;
    // clearTimeout(this.showFABTimer);
    // this.showFAB = true;
    // this.showMiniFAB = false;
    // this.editMode = false;
    // this.contentHasChanged = false;
}
</script>

<template>
    <div class="fab-container">
        <template v-if="tabsStore.activeTab == Tab.Topic">
            <div class="edit-mode-bar-container" v-show="fabStore.open">
                <div class="toolbar"
                    :class="{ 'pseudo-sticky': footerIsVisible, 'is-hidden': !editMode, 'shrink': shrink, 'expand': expand }"
                    :style="{ width: width + 'px' }">
                    <div class="toolbar-btn-container">
                        <div class="btn-left">
                        </div>
                        <div class="centerText" v-show="isLoggedIn">
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

                        <div class="btn-right" v-show="topicStore.contentHasChanged" v-else>
                            <div class="button" @click.prevent="topicStore.saveContent(newCategoryName)"
                                :class="{ expanded: editMode }">
                                <div class="icon">
                                    <i class="fas fa-save"></i>
                                </div>
                                <div class="btn-label">
                                    Speichern
                                </div>
                            </div>

                            <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded: editMode }">
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
        </template>
    </div>
</template>