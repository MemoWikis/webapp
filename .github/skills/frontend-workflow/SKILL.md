# Frontend Workflow Skill

## Purpose

Master skill for Frontend (Nuxt.js) development. Use this skill for any Frontend-related task.

## When to Use

- Creating or modifying Vue components
- Working with translations (i18n)
- Styling with LESS/CSS
- Frontend state management (Pinia stores)
- Any task in `src/Frontend.Nuxt/`

---

## Technical Stack

| Technology       | Purpose                               |
| ---------------- | ------------------------------------- |
| **Nuxt 3**       | Vue.js meta-framework (SSR/SSG)       |
| **Vue 3**        | Composition API with `<script setup>` |
| **TypeScript**   | Type safety                           |
| **Pinia**        | State management                      |
| **LESS**         | CSS preprocessor                      |
| **floating-vue** | Tooltips & popovers                   |
| **TipTap**       | Rich text editor                      |
| **i18n**         | Internationalization                  |

---

## Code Patterns

### Component Structure

```vue
<script lang="ts" setup>
// 1. Imports (auto-imported by Nuxt, avoid importing from 'vue')
const { t } = useI18n();

// 2. Props & Emits
interface Props {
  userId: number;
  showDetails?: boolean;
}
const props = withDefaults(defineProps<Props>(), {
  showDetails: false,
});

const emit = defineEmits<{
  (e: "update", value: string): void;
}>();

// 3. Composables & Stores
const userStore = useUserStore();

// 4. Reactive State
const isLoading = ref(false);

// 5. Computed
const userName = computed(() => userStore.user?.name ?? "");

// 6. Functions (use arrow functions)
const handleClick = () => {
  emit("update", "new value");
};

// 7. Lifecycle
onMounted(() => {
  loadData();
});
</script>

<template>
  <!-- Template here -->
</template>

<style lang="less" scoped>
@import (reference) "~~/assets/includes/imports.less";
// Styles here
</style>
```

### Arrow Functions (Required)

```typescript
// ✅ Correct
const fetchData = async () => {
  const result = await $api("/apiVue/User/GetUser");
  return result;
};

// ❌ Wrong
async function fetchData() {
  // ...
}
```

### Control Flow (No Single-Line Statements)

```typescript
// ✅ Correct
if (isValid) {
  doSomething();
}

// ❌ Wrong
if (isValid) doSomething();
```

---

## Tooltips (floating-vue)

**ALWAYS use `v-tooltip` directive for tooltips.** Never use native `title` attribute.

The project uses [floating-vue](https://floating-vue.starpad.dev/) via plugin at `plugins/floatingVue.ts`.

### Usage

```vue
<template>
  <!-- Simple tooltip -->
  <button v-tooltip="t('button.tooltip')">Click me</button>

  <!-- With options -->
  <button v-tooltip="{ content: t('button.tooltip'), placement: 'bottom' }">
    Click me
  </button>

  <!-- Conditional tooltip -->
  <span v-tooltip="showTooltip ? t('info.text') : null"> Info </span>
</template>
```

### Why v-tooltip?

| Feature       | `v-tooltip`       | Native `title`    |
| ------------- | ----------------- | ----------------- |
| Styling       | Customizable      | Browser default   |
| Delay         | Instant           | ~1s browser delay |
| Positioning   | Smart positioning | Fixed             |
| HTML content  | Supported         | No                |
| Accessibility | ARIA support      | Limited           |

---

## Translations (i18n)

### Location

Translation files are in `src/Frontend.Nuxt/i18n/locales/`:

- `de.json` - German (primary)
- `en.json` - English
- `fr.json` - French
- `es.json` - Spanish

### Usage

```vue
<script lang="ts" setup>
const { t } = useI18n();
</script>

<template>
  <!-- Simple -->
  <span>{{ t("settings.title") }}</span>

  <!-- With parameters -->
  <span>{{ t("user.greeting", { name: userName }) }}</span>

  <!-- Pluralization -->
  <span>{{ t("items.count", itemCount) }}</span>
</template>
```

### Key Terminology

| Term           | Description                                   |
| -------------- | --------------------------------------------- |
| `Nutzer`       | User (German, not "Benutzer")                 |
| `Seite`        | Page                                          |
| `Wiki`         | A special kind of page                        |
| `Unterseite`   | Subpage/child page                            |
| `Wunschwissen` | Wishknowledge (knowledge user wants to learn) |

### Guidelines

1. **Friendly tone**: Use "Du" form in German, casual language
2. **Consistency**: Update ALL language files when adding/changing keys
3. **Personalization**: Use parameter interpolation for user-specific content
   ```json
   "profile.wikisCreated": "Public wikis by {userName}"
   ```
4. **No abbreviations**: Spell out words completely

---

## File Naming Conventions

See **[Style Guide](../../style-guide.md)** for the complete file naming and suffix conventions.

---

## API Calls

Use the `$api` composable for API calls:

```typescript
// GET
const data = await $api<ResponseType>("/apiVue/User/GetUser", {
  method: "GET",
  query: { userId: 123 },
  credentials: "include",
});

// POST
const result = await $api<ResponseType>("/apiVue/User/Update", {
  method: "POST",
  body: { name: "New Name" },
  credentials: "include",
});
```

---

## State Management (Pinia)

### Store Pattern

```typescript
// stores/user.store.ts
export const useUserStore = defineStore("user", () => {
  // State
  const user = ref<User | null>(null);
  const isLoading = ref(false);

  // Getters (computed)
  const isLoggedIn = computed(() => user.value !== null);
  const userName = computed(() => user.value?.name ?? "");

  // Actions
  const fetchUser = async () => {
    isLoading.value = true;
    try {
      user.value = await $api<User>("/apiVue/User/GetUser");
    } finally {
      isLoading.value = false;
    }
  };

  return {
    user,
    isLoading,
    isLoggedIn,
    userName,
    fetchUser,
  };
});
```

---

## Styling (LESS)

### Import Variables

```less
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.my-component {
    color: @memo-blue;
    background: @memo-grey-lighter;
    border: 1px solid @memo-grey-light;
}
</style>
```

### Common Variables

**IMPORTANT:** Always verify variable names in `src/Frontend.Nuxt/assets/includes/colors.less` (or `imports.less`) before using them. **Foundations**:

| Variable             | Usage                             |
| -------------------- | --------------------------------- |
| `@memo-blue`         | Primary brand color               |
| `@memo-blue-link`    | Links, interactive elements       |
| `@memo-green`        | Success, positive states          |
| `@memo-yellow`       | Warnings                          |
| `@memo-grey-dark`    | Text secondary (NOT `@memo-dark`) |
| `@memo-grey-light`   | Borders                           |
| `@memo-grey-lighter` | Backgrounds                       |
| `@memo-grey-darkest` | Darkest text color                |

**Do NOT guess variables.** If you are unsure, read `src/Frontend.Nuxt/assets/includes/colors.less`. Common mistake: Using `@memo-dark` (incorrect) instead of `@memo-grey-dark` or `@memo-grey-darkest`.

---

## Accessibility

- Use semantic HTML (`<button>`, `<nav>`, `<main>`, etc.)
- Add `aria-label` for icon-only buttons
- Ensure keyboard navigation works
- Use proper heading hierarchy (`<h1>`, `<h2>`, etc.)

---

## Common Pitfalls

### 1. Hydration Mismatches

Wrap dynamic/client-only content:

```vue
<ClientOnly>
    <DynamicComponent />
    <template #fallback>
        <LoadingSkeleton />
    </template>
</ClientOnly>
```

### 2. Missing Credentials

Always include credentials for authenticated endpoints:

```typescript
// ✅ Correct
await $api("/apiVue/Protected/Endpoint", {
  credentials: "include",
});
```

### 3. Importing from 'vue'

Most Vue imports are auto-imported by Nuxt:

```typescript
// ❌ Wrong
import { ref, computed, onMounted } from "vue";

// ✅ Correct - just use them directly
const count = ref(0);
const doubled = computed(() => count.value * 2);
```

---

## Related Documentation

- [Editor System Overview](../../../docs/editor-system-overview.md)
- [Translation Glossary](../../../prompts/translation-glossary.md)
- [Translation Instructions](../../../prompts/translation-instructions.md)
