<script lang="ts" setup>
// LayoutCardSize is auto-imported from composables

// Types
interface WhitelistedModel {
    id: number
    provider: string
    modelId: string
    displayName: string
    tokenCostMultiplier: number
}

interface AvailableModel {
    modelId: string
    displayName: string
    isWhitelisted: boolean
}

interface ProviderModels {
    providerName: string
    models: AvailableModel[]
}

// Props
const props = defineProps<{
    whitelistedModels: WhitelistedModel[]
    providerModels: ProviderModels[]
    fetchingModels: boolean
    editingCostRate: { id: number, value: number } | null
    editingDisplayName: { id: number, value: string } | null
    showDeleteConfirmModal: boolean
    modelToDelete: WhitelistedModel | null
}>()

// Emits
const emit = defineEmits<{
    (event: 'loadWhitelistedModels'): void
    (event: 'fetchAllProviderModels'): void
    (event: 'startEditCostRate', model: WhitelistedModel): void
    (event: 'saveCostRate'): void
    (event: 'cancelEditCostRate'): void
    (event: 'startEditDisplayName', model: WhitelistedModel): void
    (event: 'saveDisplayName'): void
    (event: 'cancelEditDisplayName'): void
    (event: 'confirmDeleteModel', model: WhitelistedModel): void
    (event: 'executeDelete'): void
    (event: 'cancelDelete'): void
    (event: 'toggleWhitelist', providerName: string, model: AvailableModel): void
    (event: 'update:editingCostRate', value: { id: number, value: number } | null): void
    (event: 'update:editingDisplayName', value: { id: number, value: string } | null): void
}>()

// Two-way binding for editingCostRate
const localEditingCostRate = computed({
    get: () => props.editingCostRate,
    set: (value) => emit('update:editingCostRate', value)
})

// Two-way binding for editingDisplayName
const localEditingDisplayName = computed({
    get: () => props.editingDisplayName,
    set: (value) => emit('update:editingDisplayName', value)
})
</script>

<template>
    <div class="tab-content">
        <!-- AI Models Management Panel -->
        <LayoutPanel title="AI Models Management">
            <!-- Section 1: Whitelisted Models -->
            <LayoutCard :size="LayoutCardSize.Large">
                <div class="ai-models-header">
                    <h4>Whitelisted Models</h4>
                    <button class="memo-button btn btn-secondary" @click="emit('loadWhitelistedModels')">
                        <font-awesome-icon icon="fa-solid fa-sync" /> Refresh
                    </button>
                </div>

                <template v-if="props.whitelistedModels.length > 0">
                    <table class="whitelist-table">
                        <thead>
                            <tr>
                                <th>Provider</th>
                                <th>Display Name</th>
                                <th>Model ID</th>
                                <th>Cost Rate</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="model in props.whitelistedModels" :key="model.id">
                                <td>
                                    <span class="provider-badge" :class="model.provider.toLowerCase()">{{
                                        model.provider }}</span>
                                </td>
                                <td>
                                    <template v-if="localEditingDisplayName?.id === model.id">
                                        <div class="display-name-edit">
                                            <input v-model="localEditingDisplayName.value" type="text"
                                                class="display-name-input" />
                                            <button class="btn-icon btn-save" title="Save"
                                                @click="emit('saveDisplayName')">
                                                <font-awesome-icon icon="fa-solid fa-check" />
                                            </button>
                                            <button class="btn-icon btn-cancel" title="Cancel"
                                                @click="emit('cancelEditDisplayName')">
                                                <font-awesome-icon icon="fa-solid fa-times" />
                                            </button>
                                        </div>
                                    </template>
                                    <template v-else>
                                        <span class="display-name" title="Click to edit"
                                            @click="emit('startEditDisplayName', model)">
                                            {{ model.displayName || '(no name)' }}
                                        </span>
                                    </template>
                                </td>
                                <td class="model-id-cell">{{ model.modelId }}</td>
                                <td>
                                    <template v-if="localEditingCostRate?.id === model.id">
                                        <div class="cost-rate-edit">
                                            <input v-model.number="localEditingCostRate.value" type="number" step="0.1"
                                                min="0" class="cost-rate-input" />
                                            <button class="btn-icon btn-save" title="Save"
                                                @click="emit('saveCostRate')">
                                                <font-awesome-icon icon="fa-solid fa-check" />
                                            </button>
                                            <button class="btn-icon btn-cancel" title="Cancel"
                                                @click="emit('cancelEditCostRate')">
                                                <font-awesome-icon icon="fa-solid fa-times" />
                                            </button>
                                        </div>
                                    </template>
                                    <template v-else>
                                        <span class="cost-rate" title="Click to edit"
                                            @click="emit('startEditCostRate', model)">
                                            {{ model.tokenCostMultiplier }}x
                                        </span>
                                    </template>
                                </td>
                                <td>
                                    <button class="btn-icon btn-delete" title="Remove from whitelist"
                                        @click="emit('confirmDeleteModel', model)">
                                        <font-awesome-icon icon="fa-solid fa-trash" />
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </template>
                <template v-else>
                    <p class="no-models-message">No models whitelisted yet. Use the section below to fetch and add
                        models.</p>
                </template>
            </LayoutCard>

            <!-- Section 2: Available Models from Providers -->
            <LayoutCard :size="LayoutCardSize.Large">
                <div class="ai-models-header">
                    <h4>Available Models</h4>
                    <button class="memo-button btn btn-primary" :disabled="props.fetchingModels"
                        @click="emit('fetchAllProviderModels')">
                        <font-awesome-icon v-if="props.fetchingModels" icon="fa-solid fa-spinner" spin />
                        <font-awesome-icon v-else icon="fa-solid fa-download" />
                        Load All Models
                    </button>
                </div>

                <template v-if="props.providerModels.length > 0">
                    <div v-for="provider in props.providerModels" :key="provider.providerName" class="provider-section">
                        <h5 class="provider-header">
                            <span class="provider-badge" :class="provider.providerName.toLowerCase()">{{
                                provider.providerName }}</span>
                            <span class="model-count">({{ provider.models.length }} models)</span>
                        </h5>
                        <table class="provider-models-table">
                            <thead>
                                <tr>
                                    <th>Display Name</th>
                                    <th>Model ID</th>
                                    <th>Whitelisted</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="model in provider.models" :key="model.modelId"
                                    :class="{ 'whitelisted-row': model.isWhitelisted }">
                                    <td>{{ model.displayName }}</td>
                                    <td class="model-id-cell">{{ model.modelId }}</td>
                                    <td>
                                        <label class="toggle-switch">
                                            <input type="checkbox" :checked="model.isWhitelisted"
                                                @change="emit('toggleWhitelist', provider.providerName, model)" />
                                            <span class="toggle-slider" />
                                        </label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </template>
                <template v-else>
                    <p class="no-models-message">Click "Load All Models" to fetch available models from providers.</p>
                </template>
            </LayoutCard>

            <!-- Delete Confirmation Modal -->
            <div v-if="props.showDeleteConfirmModal" class="modal-overlay" @click.self="emit('cancelDelete')">
                <div class="confirm-modal">
                    <h4>Confirm Delete</h4>
                    <p>Are you sure you want to remove <strong>{{ props.modelToDelete?.displayName }}</strong> ({{
                        props.modelToDelete?.modelId }}) from the whitelist?</p>
                    <div class="modal-actions">
                        <button class="memo-button btn btn-danger" @click="emit('executeDelete')">Delete</button>
                        <button class="memo-button btn btn-secondary" @click="emit('cancelDelete')">Cancel</button>
                    </div>
                </div>
            </div>
        </LayoutPanel>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ai-models-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 16px;

    h4 {
        margin: 0;
    }
}

.whitelist-table,
.provider-models-table {
    width: 100%;
    border-collapse: collapse;

    th,
    td {
        padding: 10px 12px;
        text-align: left;
        border-bottom: 1px solid @memo-grey-lighter;
    }

    th {
        background: @memo-grey-lighter;
        font-weight: 600;
        font-size: 13px;
    }

    tr:hover {
        background-color: rgba(0, 0, 0, 0.02);
    }
}

.provider-badge {
    display: inline-block;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 11px;
    font-weight: 600;
    text-transform: uppercase;

    &.openai {
        background: #10a37f;
        color: white;
    }

    &.anthropic {
        background: #d97706;
        color: white;
    }

    &.google {
        background: #4285f4;
        color: white;
    }
}

.model-id-cell {
    font-family: monospace;
    font-size: 12px;
    color: @memo-grey-dark;
}

.cost-rate,
.display-name {
    cursor: pointer;
    padding: 4px 8px;
    border-radius: 4px;

    &:hover {
        background: @memo-grey-lighter;
    }
}

.cost-rate-edit,
.display-name-edit {
    display: flex;
    align-items: center;
    gap: 4px;

    .cost-rate-input {
        width: 70px;
        padding: 4px 8px;
        border: 1px solid @memo-grey-light;
        border-radius: 4px;
    }

    .display-name-input {
        width: 150px;
        padding: 4px 8px;
        border: 1px solid @memo-grey-light;
        border-radius: 4px;
    }
}

.btn-icon {
    border: none;
    background: transparent;
    cursor: pointer;
    padding: 4px 8px;
    border-radius: 4px;

    &:hover {
        background: rgba(0, 0, 0, 0.1);
    }

    &.btn-save {
        color: @memo-green;
    }

    &.btn-cancel {
        color: @memo-grey-dark;
    }

    &.btn-delete {
        color: @memo-red;
    }
}

.provider-section {
    margin-top: 24px;

    .provider-header {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 12px;

        .model-count {
            font-size: 12px;
            color: @memo-grey-dark;
            font-weight: normal;
        }
    }
}

.whitelisted-row {
    background-color: #e8f5e9 !important;
}

.toggle-switch {
    position: relative;
    display: inline-block;
    width: 44px;
    height: 24px;

    input {
        opacity: 0;
        width: 0;
        height: 0;
    }

    .toggle-slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: @memo-grey-light;
        border-radius: 24px;
        transition: 0.3s;

        &:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 3px;
            bottom: 3px;
            background-color: white;
            border-radius: 50%;
            transition: 0.3s;
        }
    }

    input:checked+.toggle-slider {
        background-color: @memo-green;

        &:before {
            transform: translateX(20px);
        }
    }
}

.no-models-message {
    color: @memo-grey-dark;
    font-style: italic;
    padding: 16px 0;
}

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;

    .confirm-modal {
        background: white;
        padding: 24px;
        border-radius: 8px;
        max-width: 400px;
        width: 90%;

        h4 {
            margin-top: 0;
        }

        .modal-actions {
            display: flex;
            gap: 12px;
            justify-content: flex-end;
            margin-top: 20px;
        }
    }
}
</style>
