<script lang="ts" setup>
import { SnackbarData, useSnackbarStore } from './snackBarStore'
import { useSnackbar } from 'vue3-snackbar' 

const snackbarStore = useSnackbarStore()

async function customFunction(id: number, dismiss?: any) {
	const index = snackbarStore.customActions.findIndex(c => c.id === id)
	if (index >= 0) {
		snackbarStore.customActions[index].action()
		await nextTick()
		snackbarStore.customActions.splice(index, 1)
	}
	dismiss()
}
const snackbar = useSnackbar()
snackbarStore.$onAction(({ name, after }) => {
	if (name === 'showSnackbar') {

		after((data: SnackbarData) => {
			snackbar.add({
				type: data.type,
				title: data.title ? data.title : '',
				text: data.text?.message ? data.text.message : '',
				duration: data.duration ? data.duration : 4000,
				dismissible: data.dismissible != null ? data.dismissible : true,
			})
		})
	}
})
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
					<div @click="dismiss" class="snackbar-btn" :disable="!message.dismissible">
						<font-awesome-icon icon="fa-solid fa-xmark" v-if="message.dismissible" />
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

#vue3-snackbar--container {
	margin: 0;

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

		.vue3-snackbar-message-wrapper {
			flex-grow: 1;

			.vue3-snackbar-message-close {
				display: flex;
				justify-content: flex-end;
				flex-grow: 1;
			}
		}
	}

}
</style>