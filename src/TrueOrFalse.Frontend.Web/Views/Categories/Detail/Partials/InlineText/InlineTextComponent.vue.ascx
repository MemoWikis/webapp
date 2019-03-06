<div>
    <textarea-autosize class="form-control" rows="3" v-model="textContent" :min-height="30" style="padding: 2px; font-size: 18px;margin-left:-3px">
    </textarea-autosize>
    <div style="display: flex; justify-content: flex-end; margin-top:0.5rem">
        <a class="CancelEdit" @click="cancelTextEdit()" style="margin-right:0.5rem">abbrechen</a>
        <div class="btn btn-primary" @click="applyNewMarkdown()">Text ändern</div>       
    </div>   
</div>