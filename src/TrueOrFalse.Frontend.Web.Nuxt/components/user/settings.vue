<script lang="ts" setup>
import { useUserStore } from './userStore'
import { ImageStyle } from '../image/imageStyleEnum'

const userStore = useUserStore()
enum Content {
    //Profile Information
    EditProfile,
    Password,
    DeleteProfile,

    //Settings
    ShowWuwi,
    SupportLogin,

    //Notifications
    General,
    KnowledgeReport
}
const activeContent = ref<Content>()
</script>

<template>
    <div class="row">
        <div class="col-lg-3 navigation">
            <div class="overline-s no-line">Profil Informationen</div>
            <button @click="activeContent = Content.EditProfile">Profil bearbeiten</button>
            <button @click="activeContent = Content.Password">Passwort</button>
            <button @click="activeContent = Content.DeleteProfile">Profil löschen</button>

            <div class="divider"></div>
            <div class="overline-s no-line">Einstellungen</div>
            <button @click="activeContent = Content.ShowWuwi">Wunschwissen anzeigen</button>
            <button @click="activeContent = Content.SupportLogin">Support Login</button>

            <div class="divider"></div>
            <div class="overline-s no-line">Benachrichtigungen</div>
            <button @click="activeContent = Content.General">Allgemein</button>
            <button @click="activeContent = Content.KnowledgeReport">Wissensbericht</button>

            <div class="divider"></div>
        </div>
        <div class="col-lg-9 settings-content">
            <Transition>
                <div v-if="activeContent == Content.EditProfile" class="content">
                    <div class="overline-s no-line">Profilbild</div>
                    <Image :url="userStore.imgUrl" :style="ImageStyle.Author" />
                </div>
                <div v-else-if="activeContent == Content.Password" class="content"></div>
                <div v-else-if="activeContent == Content.DeleteProfile" class="content"></div>
                <div v-else-if="activeContent == Content.ShowWuwi" class="content"></div>
                <div v-else-if="activeContent == Content.SupportLogin" class="content"></div>
                <div v-else-if="activeContent == Content.General" class="content"></div>
                <div v-else-if="activeContent == Content.KnowledgeReport" class="content"></div>
            </Transition>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.divider {
    margin-top: 20px;
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
}

button {
    background: white;
    text-align: left;
    color: @memo-grey-dark;
    padding: 12px 0;
    border-radius: 24px;

    &.active {
        color: @memo-blue-link;
    }

    &:hover {
        color: @memo-blue;
        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }
}

.navigation {
    display: flex;
    flex-direction: column;
    flex-wrap: nowrap;
}

.overline-s {
    margin: 5px 0;
}

.overline-s,
button {
    padding-left: 20px;
    padding-right: 20px;
}

.navigation,
.settings-content {
    padding-top: 50px;
}
</style>