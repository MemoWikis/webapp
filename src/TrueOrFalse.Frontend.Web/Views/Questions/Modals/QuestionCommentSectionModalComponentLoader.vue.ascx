<div>
    <default-modal-component showCloseButton="false" v-if="commentIsLoaded">
        <template v-slot:header>
            <div>Diskussionen</div>
        </template>
        <template v-slot:body>
            <comment-section-component :questionId="questionId"/>
        </template>
        <template v-slot:footer></template>
    </default-modal-component>
</div>