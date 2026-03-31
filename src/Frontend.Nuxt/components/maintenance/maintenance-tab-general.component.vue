<script lang="ts" setup>
import type {
    MethodData,
    RelationErrorItem
} from './maintenance.types'

// Props
const props = defineProps<{
    questionMethods: MethodData[]
    pageMethods: MethodData[]
    meiliSearchMethods: MethodData[]
    userMethods: MethodData[]
    miscMethods: MethodData[]
    toolsMethods: MethodData[]
    loggedInUserCount: number
    anonymousUserCount: number
    userIdToDelete: number
    tokenUserId: number
    tokenAmount: number
    tokenType: 'subscription' | 'paid'
    relationErrors: RelationErrorItem[]
    relationErrorsLoaded: boolean
    isAnalyzing: boolean
}>()

// Emits
const emit = defineEmits<{
    // Simple events without parameters
    (event: 'loadRelationErrors' | 'clearRelationErrorsCache' | 'deleteUser' | 'addTokensToUser' | 'removeAdminRights'): void
    // Events with string parameter
    (event: 'executeMaintenanceOperation', url: string): void
    // Events with number parameter
    (event: 'healRelations' | 'update:userIdToDelete' | 'update:tokenUserId' | 'update:tokenAmount', value: number): void
    // Token type update
    (event: 'update:tokenType', value: 'subscription' | 'paid'): void
}>()

const { t: $t } = useI18n()

// Two-way bindings
const localUserIdToDelete = computed({
    get: () => props.userIdToDelete,
    set: (value) => emit('update:userIdToDelete', value)
})

const localTokenUserId = computed({
    get: () => props.tokenUserId,
    set: (value) => emit('update:tokenUserId', value)
})

const localTokenAmount = computed({
    get: () => props.tokenAmount,
    set: (value) => emit('update:tokenAmount', value)
})

const localTokenType = computed({
    get: () => props.tokenType,
    set: (value) => emit('update:tokenType', value)
})
</script>

<template>
    <div class="tab-content">
        <LayoutPanel :title="$t('maintenance.metrics.title')">
            <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                {{ $t('maintenance.metrics.viewOverview') }}
            </NuxtLink>
        </LayoutPanel>

        <MaintenanceSection :title="$t('maintenance.questions.title')" :methods="props.questionMethods"
            :icon="['fas', 'retweet']" @method-clicked="emit('executeMaintenanceOperation', $event)" />
        <MaintenanceSection v-if="props.pageMethods.length > 0" :title="$t('maintenance.pages.title')"
            :methods="props.pageMethods" :icon="['fas', 'retweet']"
            @method-clicked="emit('executeMaintenanceOperation', $event)" />

        <LayoutPanel :title="$t('maintenance.relations.title')">
            <LayoutCard :size="LayoutCardSize.Large" :background-color="'transparent'">
                <div class="relation-errors-controls">
                    <button class="memo-button btn btn-primary" :disabled="props.isAnalyzing"
                        @click="emit('loadRelationErrors')">
                        <i v-if="props.isAnalyzing" class="fas fa-spinner fa-spin" />
                        {{ props.isAnalyzing ? 'Analyzing...' : 'Analyze and Show' }}
                    </button>
                    <button class="memo-button btn btn-warning ms-2" :disabled="props.isAnalyzing"
                        @click="emit('clearRelationErrorsCache')">
                        Clear Cache
                    </button>
                </div>
            </LayoutCard>
            <MaintenanceRelationErrorCard v-for="errorItem in props.relationErrors" :key="errorItem.parentId"
                :error-item="errorItem" @heal-relations="emit('healRelations', $event)" />
            <div v-if="props.relationErrorsLoaded && props.relationErrors.length === 0" class="no-errors-message">
                {{ $t('maintenance.relations.noErrorsFound') }}
            </div>
        </LayoutPanel>

        <MaintenanceSection :title="$t('maintenance.meiliSearch.title')" :methods="props.meiliSearchMethods"
            :description="$t('maintenance.meiliSearch.description')" :icon="['fas', 'retweet']"
            @method-clicked="emit('executeMaintenanceOperation', $event)" />

        <MaintenanceSection :title="$t('maintenance.users.title')" :methods="props.userMethods"
            :icon="['fas', 'retweet']" @method-clicked="emit('executeMaintenanceOperation', $event)">
            <LayoutCard :size="LayoutCardSize.Tiny">
                <div class="active-users-info">
                    <h4>{{ $t('maintenance.users.activeSessions') }}</h4>
                    <ul>
                        <li>{{ $t('maintenance.users.loggedIn') }}: {{ props.loggedInUserCount }} ({{
                            $t('maintenance.users.last5Minutes') }})</li>
                        <li>{{ $t('maintenance.users.anonymous') }}: {{ props.anonymousUserCount }} ({{
                            $t('maintenance.users.lastMinute') }})</li>
                    </ul>
                </div>
            </LayoutCard>
            <LayoutCard :size="LayoutCardSize.Small">
                <div class="delete-user-container">
                    <h4>{{ $t('maintenance.users.deleteUser') }}</h4>
                    <div class="delete-user-input">
                        <input v-model="localUserIdToDelete" />
                        <button class="memo-button btn btn-primary" @click="emit('deleteUser')">
                            {{ $t('maintenance.users.deleteUserButton') }}
                        </button>
                    </div>
                </div>
            </LayoutCard>
            <LayoutCard :size="LayoutCardSize.Small">
                <div class="token-management-container">
                    <h4>Add Tokens to User</h4>
                    <div class="token-management-form">
                        <div class="form-group">
                            <label>User ID:</label>
                            <input v-model.number="localTokenUserId" type="number" placeholder="User ID" />
                        </div>
                        <div class="form-group">
                            <label>Amount:</label>
                            <input v-model.number="localTokenAmount" type="number" placeholder="Token amount" />
                        </div>
                        <div class="form-group">
                            <label>Token Type:</label>
                            <select v-model="localTokenType">
                                <option value="subscription">Subscription Tokens</option>
                                <option value="paid">Paid Tokens</option>
                            </select>
                        </div>
                        <button class="memo-button btn btn-primary" @click="emit('addTokensToUser')">
                            Add Tokens
                        </button>
                    </div>
                </div>
            </LayoutCard>
        </MaintenanceSection>

        <MaintenanceSection :title="$t('maintenance.misc.title')" :methods="props.miscMethods"
            :icon="['fas', 'retweet']" @method-clicked="emit('executeMaintenanceOperation', $event)" />
        <MaintenanceSection :title="$t('maintenance.tools.title')" :methods="props.toolsMethods"
            :icon="['fas', 'hammer']" @method-clicked="emit('executeMaintenanceOperation', $event)" />

        <LayoutPanel :title="$t('maintenance.removeAdminRights.title')">
            <button class="memo-button btn btn-primary" @click="emit('removeAdminRights')">
                {{ $t('maintenance.removeAdminRights.button') }}
            </button>
        </LayoutPanel>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.delete-user-container {
    padding: 15px;

    .delete-user-input {
        display: flex;
        align-items: center;
        flex-direction: row;

        input {
            border: solid 1px @memo-grey-light;
            padding: 7px;
            margin-right: 8px;
        }
    }
}

.token-management-container {
    padding: 15px;

    .token-management-form {
        display: flex;
        flex-direction: column;
        gap: 12px;

        .form-group {
            display: flex;
            align-items: center;
            gap: 8px;

            label {
                min-width: 100px;
            }

            input,
            select {
                border: solid 1px @memo-grey-light;
                padding: 7px;
                min-width: 200px;
            }
        }
    }
}

.active-users-info {
    padding: 15px;
}

.relation-errors-controls {
    display: flex;
    align-items: center;
}

.no-errors-message {
    padding: 16px;
    color: @memo-grey-dark;
    font-style: italic;
}
</style>
