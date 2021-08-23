<div class="inline-text-editor" @click="contentIsChanged = true">
    <template v-if="editor">
        <editor-menu-bar-component :key="menuBarComponentKey" :editor="editor" :heading="true"/>
    </template>
    <template v-if="editor">
        <editor-content :editor="editor"/>
    </template>
</div>
