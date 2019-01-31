<content-module inline-template >
    <li class="module" markdown="<%: Model.Markdown %>" v-if="!isDeleted">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">