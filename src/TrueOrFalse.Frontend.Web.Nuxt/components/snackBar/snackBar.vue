<script lang="ts" setup>
import { useSnackbarStore } from './snackBarStore'
const snackbarStore = useSnackbarStore()

async function customFunction(id: number, dismiss?: any) {
	const index = snackbarStore.customActions.findIndex(c => c.id == id)
	if (index >= 0) {
		snackbarStore.customActions[index].action()
		await nextTick()
		snackbarStore.customActions.splice(index, 1)
	}
	dismiss()
}
</script>

<template>
	<NuxtSnackbar :duration="4000">
		<template #message-content="{ text, title }">
			<strong v-if="title.url">
				<NuxtLink :to="title.url">{{ title.text }}</NuxtLink>
			</strong>
			<strong v-text="title" v-else-if="title"></strong>
			<p v-if="text.html" v-html="text.html"></p>
		</template>
		<template #message-close-icon="{ message, dismiss }">
			<div class="snackbar-content-btn">
				<div class="snackbar-btn-container">
					<div @click="dismiss" class="snackbar-btn">
						<font-awesome-icon icon="fa-solid fa-xmark" />
					</div>
				</div>
				<div v-if="message.text.buttonId" @click="customFunction(message.text.buttonId, dismiss)"
					class="snackbar-btn-container">
					<font-awesome-icon v-if="message.text.buttonIcon" :icon="message.text.buttonIcon"
						class="snackbar-btn-icon" />
					<div class="snackbar-btn">
						{{ message.text.buttonLabel }}
					</div>
				</div>
			</div>
		</template>
	</NuxtSnackbar>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

p {
	margin-bottom: 0px;
}

.snackbar-content-btn {
	display: flex;
	justify-content: space-between;
	flex-direction: row-reverse;
	margin-right: -10px;

	.snackbar-btn-container {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 10px 14px;
		cursor: pointer;
		border-radius: 24px;
		height: 40px;
		min-width: 40px;
		transition: all ease-in 100ms;

		.snackbar-btn-icon {
			margin-right: 8px;
		}

		&:hover {
			background-color: rgba(255, 255, 255, 0.15);
			transition: all ease-in 10ms;
		}

		&:active {
			background-color: rgba(255, 255, 255, 0.3);
		}

		.snackbar-btn {
			font-size: 14px;
			font-weight: 500;
			text-transform: uppercase;
			cursor: pointer;
		}
	}
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

article.vue3-snackbar-message {

	&.warning,
	&.success {
		color: @memo-blue;
	}

	&.info,
	&.error {
		a {
			color: @memo-info;
		}
	}
}
</style>