<div v-if="saveMessage.length > 0">
    <div class="alert alert-success fade in" style="opacity: 1;" v-if="saveSuccess">
        <a class="close" @click.prevent="removeAlert()" href="#">×</a>
        {{saveMessage}}
    </div>
    <div class="alert alert-danger fade in" style="opacity: 1;" v-else>
        <a class="close" @click.prevent="removeAlert()" href="#">×</a>
        {{saveMessage}}
    </div>
</div>