<div>
    <textarea-autosize :ref="textAreaId" class="form-control" rows="3" v-model="textContent" :min-height="30" style="padding: 2px; font-size: 18px;margin-left:-3px" />
    <p style="line-height: 1.1; margin-top: 0.2rem; color: #999999;"><small>Der Editor benutzt Markdown. Du kannst den Text **<b>bold</b>** oder *<i>italic</i>* schreiben. <br/>
        Weitere Formatierung findest du <a href="https://www.heise.de/mac-and-i/downloads/65/1/1/6/7/1/0/3/Markdown-CheatSheet-Deutsch.pdf">hier</a>.</small></p>

    <div style="display: flex; justify-content: flex-end; margin-top:0.5rem">
        <div class="btn btn-link" @click="cancelTextEdit()">Abbrechen</div>
        <div class="btn btn-primary" @click="applyNewMarkdown()">Text ändern</div>
    </div>   
</div>