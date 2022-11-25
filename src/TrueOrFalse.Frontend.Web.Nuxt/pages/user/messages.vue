<script lang="ts" setup>

const { data: model } = await useFetch<any>(`/apiVue/UserMessages/Get/`, {
    baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local',
    credentials: 'include',
    mode: 'no-cors',
})

async function loadAll() {
    console.log(model)
}

</script>

<template>
    <div class="container">

        <div class="row">
            <div class="col-md-9">

                <h1 style="margin-top: 0; margin-bottom: 20px;">
                    <span class="ColoredUnderline Message" style="padding-right: 3px;">Nachrichten</span>
                </h1>
                <div id="messagesWrapper">
                    <UserMessagesRows v-if="model.Messages != null" v-for="msg in model.Messages" :msg="msg" />
                    <div class="alert alert-info" v-if="model.Messages.filter(m => !m.IsRead).length == 0">
                        Du hast aktuell keine ungelesenen Nachrichten.
                    </div>

                    <p v-if="model.ReadMessagesCount > 0">
                        Du hast {{ model.ReadMessagesCount }} gelesene Nachricht{{ model.ReadMessagesCount == 0 ||
                                model.ReadMessagesCount > 1 ? 'en' : ''
                        }}.
                    <div @click="loadAll()">Alle anzeigen</div>.
                    </p>

                </div>

            </div>
            <div class="col-md-3"></div>
        </div>
    </div>
</template>

<style lang="less" scoped>
.container {
    min-height: 400px;
}
</style>