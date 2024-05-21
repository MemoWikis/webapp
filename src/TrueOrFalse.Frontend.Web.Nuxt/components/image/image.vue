<script lang="ts" setup>
import { useImageLicenseStore } from './imageLicenseStore'
import { ImageFormat } from './imageFormatEnum'

interface Props {
	src: string,
	alt?: string,
	square?: boolean,
	class?: string,
	format?: ImageFormat,
	showLicense?: boolean,
	imageId?: number,
	minWidth?: number,
	minHeight?: number,
	customStyle?: string,
}

const props = defineProps<Props>()

const imageLicenseStore = useImageLicenseStore()
function openImage() {
	if (props.imageId && props.imageId > 0)
		imageLicenseStore.openImage(props.imageId)
}

const imgSrc = ref('')
function setImgSrc() {
	if (props.src.startsWith('/Images/Categories/') || props.src.startsWith('/Images/Questions/') || props.src.startsWith('/Images/Users/'))
		imgSrc.value = props.src
	else if (props.src.startsWith('/Images'))
		imgSrc.value = props.src.replace("/Images", "/img")
	else imgSrc.value = props.src
}
watch(() => props.src, () => setImgSrc())
setImgSrc()

const imgContainer = ref()
const getCustomStyle = computed(() => {
	let str = props.customStyle ?? ''
	if (props.minWidth)
		str += `min-width: ${props.minWidth}px;`
	if (props.minHeight)
		str += `min-height: ${props.minHeight}px;`
	if (props.square && imgContainer.value != null && imgContainer.value.clientWidth != null)
		str += `height: ${imgContainer.value.clientWidth}px; object-fit: cover;`
	return str
})
</script>

<template>
	<div class="img-container" :class="props.class" ref="imgContainer">
		<slot name="top"></slot>
		<img v-if="props.format == ImageFormat.Author" :src="imgSrc" class="author" :alt="props.alt"
			:style="getCustomStyle" />
		<img v-else :src="imgSrc" class="topic" :alt="props.alt" :style="getCustomStyle" />

		<div v-if="props.showLicense && props.imageId != undefined && !props.src.includes('no-category-picture')"
			class="license-btn" @click="openImage()">Lizenzinfos
		</div>
		<slot name="bottom"></slot>
	</div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.img-container {
	display: flex;
	flex-direction: column;
	align-items: center;

	&.header-author-icon {
		img {
			border-radius: 24px;

		}
	}

	img {
		height: 100%;
		width: 100%;
	}

	.license-btn {
		cursor: pointer;
		color: @memo-grey-dark;
		line-height: 18px;
		font-size: 10px;
		text-align: center;
		z-index: 2;

		&:hover {
			color: @memo-blue-lighter;
		}
	}
}

.topic {
	border-radius: 0;
}

.author {
	border-radius: 50%;
}
</style>