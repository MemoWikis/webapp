<script lang="ts" setup>
const props = defineProps(['tabKnowledgeModel', 'userModel'])

</script>

<template>


    <div v-if="!props.userModel.User.ShowWishKnowledge && !props.userModel.IsCurrentUser"
        class="bs-callout bs-callout-info" style="margin-top: 15px;">
        <h4>Nicht öffentlich</h4>
        <p>
            {{props.userModel.Name}} hat ihr/sein Wunschwissen nicht veröffentlicht.
        </p>

        <p>
            <a href="#" class="btn btn-default btn-sm featureNotImplemented">Bitte zeige mir dein
                Wunschwissen</a>
        </p>
    </div>

    <div v-else class="row">
        <div class="col-lg-12 xxs-stack">
            <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #afd534;">
                Fragen ({{props.tabKnowledgeModel.WishQuestionsCount}}:</div>
            <template v-if="props.tabKnowledgeModel.WishQuestionsCount > 0">
                <div v-for="q in props.tabKnowledgeModel.First100WishQuestions">
                    <font-awesome-icon icon="fa-solid fa-lock" v-if="q.IsPrivate" />
                    <NuxtLink :to="`/question/${q.Name}/${q.Id}`">{{q.Text}}</NuxtLink>
                </div>
            </template>
            <div v-else style="padding-top: 10px; padding-bottom: 10px;">--
                {{props.userModel.IsCurrentUser ? 'Du hast keine Fragen zu deinem Wunschwissen hinzugefügt' :
                props.userModel.Name +
                ' hat keine Fragen zum Wunschwissen hinzgeufügt.'}}
                --
            </div>
        </div>

        <div class="col-lg-12 xxs-stack">
            <template v-if="props.userModel.User.ShowWishKnowledge || props.userModel.IsCurrentUser">
                <h4 style="margin-top: 20px;">Themen mit Wunschwissen</h4>
                <template v-for="t in props.tabKnowledgeModel.">
                    <NuxtLink :to="`${t.Name}/${t.Id}`">
                        {{t.Name}} mit {{t.QuestionCount}}x Fragen {{t.QuestionCount != 1 ? '' : 'n'}}
                    </NuxtLink>
                    <br />
                </template>
            </template>
        </div>

    </div>
</template>
