# Code Style Guide

**Last Updated:** January 5, 2026  
**Stack:** Nuxt 3, TypeScript, Pinia, Vue 3

## 1. Goals

- **Predictable file discovery** by suffix
- **Consistent naming** for imports, refactors, search
- **Minimal ambiguity** across domain / UI / infrastructure

## 2. Naming Rules

### File and Folder Names

**kebab-case** for all folders and files

```
user-profile/
order-status.enum.ts
user-card.component.vue
```

### Exported Symbols

**PascalCase** for components, classes, types, enums

```typescript
UserCard
UserStatus
UserProfile
```

**camelCase** for functions, composables, variables

```typescript
useUserProfile
formatCurrency
```

### Store Identifiers

Store function name: `useXxxStore` (PascalCase in `Xxx`)

```typescript
useUserProfileStore
```

## 3. File Suffixes (Single Source of Truth)

### Vue UI

| Type | Suffix | Example |
|------|--------|---------|
| Component (SFC) | `*.component.vue` | `user-card.component.vue` |
| Page (Nuxt) | `*.page.vue` (optional) | `account.page.vue` |
| Layout (Nuxt) | `*.layout.vue` | `default.layout.vue` |

### State / Logic

| Type | Suffix | Example |
|------|--------|---------|
| Pinia store | `*.store.ts` | `user-profile.store.ts` |
| Composable | `*.composable.ts` | `use-user-profile.composable.ts` |
| Service (API client) | `*.service.ts` | `user.service.ts` |
| Repository (data access) | `*.repository.ts` | `user.repository.ts` |

### Types / Contracts

| Type | Suffix | Example |
|------|--------|---------|
| Types | `*.types.ts` | `user.types.ts` |
| DTOs (API payload) | `*.dto.ts` | `user.dto.ts` |
| Schema (validation) | `*.schema.ts` | `user.schema.ts` |

### Constants / Domain Values

| Type | Suffix | Example |
|------|--------|---------|
| Enum | `*.enum.ts` | `order-status.enum.ts` |
| Constants | `*.constants.ts` | `routes.constants.ts` |
| Config | `*.config.ts` | `http.config.ts` |

### Utilities

| Type | Suffix | Example |
|------|--------|---------|
| Pure helpers | `*.utils.ts` | `date.utils.ts` |
| Mappers/adapters | `*.mapper.ts` | `user.mapper.ts` |

### Testing

| Type | Suffix | Example |
|------|--------|---------|
| Unit test | `*.spec.ts` | `user-card.spec.ts` |
| Integration/E2E | `*.e2e.ts` | `checkout.e2e.ts` |

## 4. Folder Conventions

```
src/Frontend.Nuxt/
├── components/**       → *.component.vue
├── pages/**            → *.page.vue (optional)
├── layouts/**          → *.layout.vue
├── composables/**      → *.composable.ts
├── stores/**           → *.store.ts (or pageStore.ts pattern)
├── services/**         → *.service.ts
├── types/**            → *.types.ts / *.dto.ts
├── constants/**        → *.constants.ts / *.enum.ts
└── utils/**            → *.utils.ts / *.mapper.ts
```

**Note:** Current project uses some legacy patterns (e.g., `pageStore.ts`, `userStore.ts`). New code should follow the suffix pattern above.

## 5. Import Rules

### Prefer Absolute Imports

Use `~/` alias in Nuxt:

```typescript
import { useUserProfileStore } from '~/stores/user-profile.store'
```

### Prefer Type-Only Imports

```typescript
import type { UserStatus } from '~/constants/user-status.enum'
```

## 6. Vue Component Rules (SFC)

- Use `<script setup lang="ts">`
- Props via `defineProps`, emits via `defineEmits`
- Component name matches file (PascalCase):
  - `user-card.component.vue` → `UserCard`

**Example:**

```vue
<script setup lang="ts">
import type { User } from '~/types/user.types'

interface Props {
  user: User
}

const props = defineProps<Props>()

const emit = defineEmits<{
  select: [user: User]
}>()
</script>

<template>
  <div class="user-card" @click="emit('select', user)">
    {{ user.name }}
  </div>
</template>
```

## 7. Pinia Store Rules

- **File:** `kebab-case.store.ts`
- **Export:** `useXxxStore`
- **Store id:** stable string, preferably camelCase domain key

**Example:**

```typescript
// stores/user-profile.store.ts
import { defineStore } from 'pinia'

export const useUserProfileStore = defineStore('userProfile', () => {
  const profile = ref<UserProfile | null>(null)
  
  async function loadProfile(userId: number) {
    // ...
  }
  
  return {
    profile,
    loadProfile
  }
})
```

## 8. Quick Examples

### Enum

```typescript
// constants/order-status.enum.ts
export enum OrderStatus {
  Pending = 'pending',
  Confirmed = 'confirmed',
  Shipped = 'shipped',
  Delivered = 'delivered'
}
```

### Component

```vue
<!-- components/orders/order-card.component.vue -->
<script setup lang="ts">
import type { Order } from '~/types/order.types'
import { OrderStatus } from '~/constants/order-status.enum'

interface Props {
  order: Order
}

const props = defineProps<Props>()
</script>

<template>
  <div class="order-card">
    <h3>Order #{{ order.id }}</h3>
    <span :class="order.status">{{ order.status }}</span>
  </div>
</template>
```

### Store

```typescript
// stores/orders.store.ts
import { defineStore } from 'pinia'
import type { Order } from '~/types/order.types'

export const useOrdersStore = defineStore('orders', () => {
  const orders = ref<Order[]>([])
  
  async function fetchOrders() {
    const data = await $api<Order[]>('/api/orders')
    orders.value = data
  }
  
  return {
    orders,
    fetchOrders
  }
})
```

## 9. Migration Path

Current project has some legacy patterns (e.g., `pageStore.ts` instead of `page.store.ts`). **New files** must follow the suffix convention. **Existing files** are only refactored when touched for other reasons.

## 10. Exceptions

Nuxt convention files (`index.ts`, `app.vue`, `error.vue`, `nuxt.config.ts`, `[dynamic].vue`) are exempt from suffix rules.
