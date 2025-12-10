<script lang="ts" setup>
interface RelationError {
    type: string
    childId: number
    description: string
}

interface RelationTableItem {
    relationId: number
    previousId: number | null
    nextId: number | null
    childId: number
    parentId: number
}

interface RelationErrorItem {
    parentId: number
    errors: RelationError[]
    relations: RelationTableItem[]
}

interface Props {
    errorItem: RelationErrorItem
}

const props = defineProps<Props>()

const emit = defineEmits<{
    healRelations: [pageId: number]
}>()

const handleHealClick = () => {
    emit('healRelations', props.errorItem.parentId)
}

const selectedChildId = ref<number | null>(null)
const handleClickChildId = (childId: number) => {
    if (selectedChildId.value === childId) {
        selectedChildId.value = null
        return
    }
    selectedChildId.value = childId
}

const groupedErrors = computed(() => {
    const groups: { [key: string]: RelationError[] } = {}

    props.errorItem.errors.forEach(error => {
        if (!groups[error.type]) {
            groups[error.type] = []
        }
        groups[error.type].push(error)
    })

    return Object.entries(groups).map(([key, errors]) => ({
        key,
        errors
    }))
})
</script>

<template>
    <LayoutCard :size="LayoutCardSize.Medium" class="relation-error-card">
        <div class="error-card-content">
            <h5>{{ $t('maintenance.relations.parentPageId') }}: {{ errorItem.parentId }}</h5>
            <MaintenanceRelationErrorList
                v-for="group in groupedErrors" :key="group.key"
                :group="group"
                :selected-child-id="selectedChildId"
                @click-child-id="handleClickChildId" />

            <h6>{{ $t('maintenance.relations.relationsTableTitle') }}</h6>
            <table class="relations-table">
                <thead>
                    <tr>
                        <th>{{ $t('maintenance.relations.table.relationId') }}</th>
                        <th>{{ $t('maintenance.relations.table.childId') }}</th>
                        <th>{{ $t('maintenance.relations.table.previousId') }}</th>
                        <th>{{ $t('maintenance.relations.table.nextId') }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="relation in errorItem.relations" :key="relation.relationId" class="table-item" :class="{ 'selected': selectedChildId === relation.childId }">
                        <td>{{ relation.relationId }}</td>
                        <td>{{ relation.childId }}</td>
                        <td>{{ relation.previousId || '-' }}</td>
                        <td>{{ relation.nextId || '-' }}</td>
                    </tr>
                </tbody>
            </table>

            <div class="heal-button-container">
                <button @click="handleHealClick" class="memo-button btn btn-sm btn-primary heal-card-button">
                    {{ $t('maintenance.relations.healButton') }}
                </button>
            </div>
        </div>
    </LayoutCard>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.relation-error-card {
    margin-bottom: 16px;

    .error-card-content {
        padding: 15px;

        h5 {
            margin: 0 0 15px 0;
            color: @memo-grey-darker;
            font-weight: 600;
        }

        h6 {
            margin: 15px 0 5px 0;
            color: @memo-grey-darker;
            font-weight: 600;
            font-size: 14px;
        }

        .relations-table {
            width: 100%;
            border-collapse: collapse;
            margin: 0 0 15px 0;

            th,
            td {
                padding: 8px 6px;
                text-align: left;
                border-bottom: 1px solid @memo-grey-lighter;
            }

            th {
                background-color: @memo-grey-lightest;
                font-weight: 600;
                color: @memo-grey-darker;
                border-bottom: 2px solid @memo-grey-light;
                font-size: 12px;
            }

            tbody tr {
                &:hover {
                    background-color: fade(@memo-grey-lightest, 50%);
                }
            }

            td {
                font-family: monospace;
                font-size: 11px;
            }

            .table-item {
                &.selected {
                    // background-color: fade(@memo-wish-knowledge-red, 10%);
                    background-color: @memo-grey-lighter;
                }
            }
        }

        .heal-button-container {
            display: flex;
            justify-content: flex-end;
            padding-top: 15px;

            .heal-card-button {
                padding: 6px 16px;
                font-size: 12px;
            }
        }
    }
}
</style>
