<script lang="ts" setup>
import { TopicChangeType } from '~~/components/topic/history/topicChangeTypeEnum'
import { ImageFormat } from '~/components/image/imageFormatEnum'

export interface Change {
    topicId: number
    author: Author
    elapsedTime: string
    topicChangeType: TopicChangeType
    revisionId: number
    relationAdded?: boolean
    affectedTopicId?: number
    affectedTopicName?: string
    affectedTopicNameEncoded?: string
}

interface Author {
    id: number
    name: string
    imgUrl: string
}
interface Props {
    change: Change
    groupIndex: number
    isLast: boolean
    firstEditId?: number
}
const props = defineProps<Props>()

function getChangeTypeText(change: Change) {
    switch (change.topicChangeType) {
        case TopicChangeType.Create:
            return 'Erstellt'
        case TopicChangeType.Delete:
            return 'Gelöscht'
        case TopicChangeType.Image:
            return 'Bild Update'
        case TopicChangeType.Moved:
            return 'Verschoben'
        case TopicChangeType.Privatized:
            return 'Auf Privat gestellt'
        case TopicChangeType.Published:
            return 'Veröffentlicht'
        case TopicChangeType.Relations:
            if (change.relationAdded)
                return 'Verknüpfung hinzugefügt'
            else
                return 'Verknüpfung entfernt'
        case TopicChangeType.Renamed:
            return 'Umbenannt'
        case TopicChangeType.Restore:
            return 'Wiederhergestellt'
        case TopicChangeType.Text:
            return 'Inhalt'
        case TopicChangeType.Update:
            return 'Update'
        default: return 'Update'
    }
}

const route = useRoute()
const { $urlHelper } = useNuxtApp()
const slots = useSlots()

</script>

<template>
    <div class="row change-detail-model" :class="{ 'last-detail': isLast }">
        <div class="col-xs-3">
            <NuxtLink v-if="change.author.id > 0" :to="$urlHelper.getUserUrl(change.author.name, change.author.id)"
                class="category-change-author">
                <Image :src="change.author.imgUrl" :format="ImageFormat.Author" class="category-change-author-img" />
                {{ change.author.name }}
            </NuxtLink>
        </div>
        <div class="col-xs-3">
            {{ change.elapsedTime }}
        </div>

        <div class="col-xs-6 change-detail">
            <div class="change-detail-label">{{ getChangeTypeText(change) }}</div>
            <div v-if="change.topicChangeType == TopicChangeType.Relations">
                {{ change.affectedTopicName }}
            </div>

            <button class="memo-button btn btn-primary" v-if="change.topicChangeType == TopicChangeType.Text"
                v-on:click.stop>
                <NuxtLink v-if="firstEditId" :to="`/Historie/Thema/${route.params.id}/${change.revisionId}/${firstEditId}`">
                    Ansehen
                </NuxtLink>
                <NuxtLink v-else :to="`/Historie/Thema/${route.params.id}/${change.revisionId}/`">
                    Ansehen
                </NuxtLink>
            </button>
            <div class="extras" v-if="slots['extras']">
                <slot name="extras"></slot>
            </div>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.change-detail-model {
    border: 1px solid silver;
    margin: 0px;
    padding: 10px;
    min-height: 60px;
    display: flex;
    align-items: center;
    color: @memo-grey-dark;
    flex-wrap: wrap;
    border-bottom: none;

    &.last-detail {
        border-bottom: 1px solid silver;
    }

    .col-xs-3 {
        height: 100%;
        display: flex;
        align-items: center;

        a {
            margin-right: 4px;
        }
    }

    .allThemesHistory {
        margin-top: 10px;
    }

    .editing-history {
        margin-top: 10px;
    }

    @media screen and (max-width: 1199px) {
        .c-changes-overview {
            margin-top: 10px;
        }
    }


    @media screen and (max-width: 650px) {
        .col-xs-4 {
            width: 100%;
            margin-bottom: 10px;
        }

        .c-changes-overview,
        .editing-history {
            margin-top: 0px;
        }
    }

    @media screen and (max-width: 603px) {
        .editing-history {
            margin-top: 10px;
        }
    }

    @media screen and (max-width: 733px) {
        .display-changes {
            margin-top: 10px;
        }
    }

    @media screen and (max-width: 550px) {

        .col-xs-3,
        .col-xs-6 {
            width: 100%;
            margin-bottom: 10px;
        }

        .display-changes {
            margin-top: 0px;
        }
    }

    @media screen and (max-width: 406px) {
        .c-changes-overview {
            margin-top: 10px;
        }
    }

    @media screen and (max-width: 401px) {
        .display-changes {
            margin-top: 10px;
        }
    }

    .is-deleted {
        background-color: #ccc;
    }

    .btn {
        margin-left: auto;
        margin-right: 10px;
    }

    #Typ {
        color: #2C5FB2;
        font-weight: 900;
        padding-top: 10px;
    }

    .change-detail {
        display: flex;
        align-items: center;
        justify-content: flex-start;

        .change-detail-sub {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            padding-right: 10px;
        }

        .change-detail-label {
            background: @memo-grey-lighter;
            font-size: 12px;
            text-align: center;
            font-weight: 600;
            padding: 4px 12px;
            justify-self: flex-start;
            white-space: nowrap;
            margin-right: 8px;
        }

        .change-detail-additional-info {
            min-width: 0;
            text-overflow: ellipsis;
            overflow: hidden;
        }

        .change-detail-spacer {
            min-width: 22px;
        }

        &.relation-detail {
            flex-wrap: wrap-reverse;

            .related-category-name {
                display: flex;
                align-items: center;
                padding: 4px 0;

                .history-link {
                    margin-right: 4px;
                }
            }

            .history-link {
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;
                display: inline-block;
            }
        }
    }

    &.panel-group {
        padding: 0px;

        .panel {
            border: none;
            border-bottom: none;
            border-radius: 0;
            box-shadow: none;
            width: 100%;
            overflow: visible;

            .panel-heading {
                border: none;
                background: none;
                border-radius: 0;
                cursor: pointer;
                min-height: 60px;
                display: flex;
                align-items: center;
                color: @memo-grey-dark;
                transition: background-color ease-in-out 0.2s;
                flex-wrap: wrap;

                &:hover {
                    background-color: fade(@memo-grey-lighter, 20%);
                }

                &.collapsed {
                    .fa-chevron-up {
                        display: none;
                    }

                    .fa-chevron-down {
                        display: block;
                    }
                }

                .fa-chevron-up {
                    display: block;
                }

                .fa-chevron-down {
                    display: none;
                }

                .chevron-container {
                    width: 27px;
                    text-align: center;
                    align-items: center;
                    justify-content: center;
                    display: flex;
                }
            }

            img.history-author,
            img.history-category {
                margin-left: 5px;
                width: 20px;
                min-width: 20px;
            }

            .list-group {
                overflow: hidden;

                .list-group-item {
                    min-height: 60px;
                    border-radius: 0 !important;
                    display: flex;
                    align-items: center;

                    &:last-child {
                        border-bottom: none;
                        border-radius: 0;
                    }

                    .change-detail-spacer {
                        width: 26px;
                    }
                }
            }
        }
    }

    .history-date {
        flex-wrap: wrap;
    }

    .history-link {

        .history-author,
        .history-category {
            border-radius: 10px;
            width: 20px;
            min-width: 20px;
        }
    }
}

.extras {
    width: 40px;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>