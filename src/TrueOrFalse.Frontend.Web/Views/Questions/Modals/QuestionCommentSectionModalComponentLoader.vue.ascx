<div>
    <default-modal-component v-if="commentIsLoaded">
        <template v-slot:header>
            <div>Diskussionen</div>
        </template>
        <template v-slot:body>
            <comment-section-component/>
        </template>
        <template v-slot:footer></template>
    </default-modal-component>
</div>