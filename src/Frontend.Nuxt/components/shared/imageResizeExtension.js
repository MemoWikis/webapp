//based on: https://github.com/bae-sh/tiptap-extension-resize-image

import Image from '@tiptap/extension-image'
import { dom } from '@fortawesome/fontawesome-svg-core'
import { color } from '~/constants/colors'
import { useTiptapImageLicenseStore } from './tiptapImageLicenseStore'

const borderStyle = `1px dashed ${color.memoGreen}`
const resizeHandle = `position: absolute; width: 9px; height: 9px; border: 2px solid ${color.memoGreyDarker}; border-radius: 50%;`
const fontColor = `${color.memoGreyLighter}`

// Helper function to get figcaption content
const getFigcaptionContent = (caption, license) => {
    // Helper to check if HTML content has actual content (not just empty tags)
    const hasActualContent = (html) => {
        if (!html) return false
        const trimmed = html.trim()
        return trimmed !== '' && trimmed !== '<p></p>' && trimmed !== '<br>'
    }
    
    const cleanCaption = caption || ''
    const cleanLicense = license || ''
    
    const hasCaption = hasActualContent(cleanCaption)
    const hasLicense = hasActualContent(cleanLicense)
    
    let html = ''
    
    if (!hasCaption && !hasLicense) html = `(No license)`
    else if (!hasCaption && hasLicense) html = `${cleanLicense}`
    else if (hasCaption && !hasLicense) html = `${cleanCaption} (No license)`
    else html = `${cleanCaption} ${cleanLicense}`
    
    return { html, hasLicense }
}

// Helper function to add figcaption click handler
const addFigcaptionClickHandler = ($figcaption, caption, license, src, alt) => {
    $figcaption.style.cursor = 'pointer'
    $figcaption.addEventListener('click', (e) => {
        // Don't trigger if clicking on a link or if the target is inside a link
        if (e.target.tagName === 'A' || e.target.closest('a')) {
            return // Let the link handle its own click
        }
        
        e.preventDefault()
        e.stopPropagation()
        const store = useTiptapImageLicenseStore()
        store.openModal(caption, license, src, alt)
    })
}

const ImageResize = Image.extend({
    name: 'figure',

    addAttributes() {
        return {
            src: {
                default: null,
            },
            alt: {
                default: null,
            },
            style: {
                default: 'width: 100%; height: auto; cursor: pointer;',
                parseHTML: (element) => {
                    // For figure elements, get style from the figure itself
                    if (element.tagName === 'FIGURE') {
                        const width = element.getAttribute('width')
                        return width
                            ? `width: ${width}px; height: auto; cursor: pointer;`
                            : element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                    // For legacy img elements, get style from the img
                    const width = element.getAttribute('width')
                    return width
                        ? `width: ${width}px; height: auto; cursor: pointer;`
                        : element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                },
            },
            caption: {
                default: null,
                parseHTML: (element) => {
                    const figcaption = element.querySelector('figcaption')
                    return figcaption?.innerHTML || null
                },
            },
            license: {
                default: null,
                parseHTML: (element) => {
                    const figcaption = element.querySelector('figcaption')
                    return figcaption?.getAttribute('data-license') || null
                },
            },
            title: {
                default: null,
            },
            loading: {
                default: null,
            },
            srcset: {
                default: null,
            },
            sizes: {
                default: null,
            },
            crossorigin: {
                default: null,
            },
            usemap: {
                default: null,
            },
            ismap: {
                default: null,
            },
            width: {
                default: null,
            },
            height: {
                default: null,
            },
            referrerpolicy: {
                default: null,
            },
            longdesc: {
                default: null,
            },
            decoding: {
                default: null,
            },
            class: {
                default: null,
            },
            id: {
                default: null,
            },
            name: {
                default: null,
            },
            draggable: {
                default: true,
            },
            tabindex: {
                default: null,
            },
            'aria-label': {
                default: null,
            },
            'aria-labelledby': {
                default: null,
            },
            'aria-describedby': {
                default: null,
            },
        }
    },

    parseHTML() {
        return [
            {
                tag: 'figure',
                getAttrs: (element) => {
                    const img = element.querySelector('img')
                    const figcaption = element.querySelector('figcaption')
                    
                    if (!img) return false
                    
                    return {
                        src: img.getAttribute('src'),
                        alt: img.getAttribute('alt'),
                        title: img.getAttribute('title'),
                        caption: figcaption?.innerHTML || null,
                        license: figcaption?.getAttribute('data-license') || null,
                        style: element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                }
            },
            {
                tag: 'img',
                getAttrs: (element) => {
                    return {
                        src: element.getAttribute('src'),
                        alt: element.getAttribute('alt'),
                        title: element.getAttribute('title'),
                        caption: null,
                        style: element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                }
            }
        ]
    },

    renderHTML({ HTMLAttributes }) {
        const { caption, license, src, alt, style, ...imgAttrs } = HTMLAttributes
        
        const imgElement = ['img', { src, alt, class: 'tiptap-image', ...imgAttrs }]
        
        const captionData = getFigcaptionContent(caption, license)
        if (captionData.html) {
            const figcaptionClasses = ['tiptap-figcaption']
            if (!captionData.hasLicense) {
                figcaptionClasses.push('no-license')
            }
            
            const figcaptionAttrs = {
                class: figcaptionClasses.join(' ')
            }
            
            if (license) {
                figcaptionAttrs['data-license'] = license
            }
            
            return [
                'figure',
                { style, class: 'tiptap-figure' },
                imgElement,
                ['figcaption', figcaptionAttrs, captionData.html]
            ]
        } else {
            return [
                'figure',
                { style, class: 'tiptap-figure' },
                imgElement
            ]
        }
    },

    addNodeView() {
        return ({ node, editor, getPos }) => {
            const {
                view,
                options: { editable },
            } = editor
            const { src, alt, caption, license, style } = node.attrs
            
            // Create wrapper and container elements
            const $wrapper = document.createElement('div')
            const $container = document.createElement('figure')
            const $img = document.createElement('img')
            const iconStyle = `
                background: white;
                border: hidden;
                font-size: 18px;
                width: 36px;
                height: 36px;
                margin: 0px;
                color: ${color.memoGreyDarker};
                text-align: center;
                padding: 0px 21px;
                display: flex;
                justify-content: center;
                align-items: center;
                transition: filter 0.1s;        
            `

            const dispatchNodeView = () => {
                if (typeof getPos === 'function') {
                    const newAttrs = {
                        ...node.attrs,
                        style: `${$container.style.cssText}`,
                    }
                    view.dispatch(view.state.tr.setNodeMarkup(getPos(), null, newAttrs))
                }
            }

            const showCaptionModal = () => {
                const store = useTiptapImageLicenseStore()
                store.openEditModal(
                    caption, 
                    license, 
                    src, 
                    alt,
                    (data) => {
                        const { caption: newCaption, license: newLicense } = data
                        
                        // Create the complete figcaption HTML
                        const captionData = getFigcaptionContent(newCaption, newLicense)
                        
                        // Update node attributes - store the complete HTML in caption
                        if (typeof getPos === 'function') {
                            const newAttrs = {
                                ...node.attrs,
                                caption: captionData.html,
                                license: newLicense
                            }
                            view.dispatch(view.state.tr.setNodeMarkup(getPos(), null, newAttrs))
                        }

                        // Update figcaption in DOM
                        const existingFigcaption = $container.querySelector('figcaption')
                        if (existingFigcaption) {
                            $container.removeChild(existingFigcaption)
                        }

                        if (captionData.html) {
                            const $figcaption = document.createElement('figcaption')
                            $figcaption.className = 'tiptap-figcaption'
                            if (!captionData.hasLicense) {
                                $figcaption.classList.add('no-license')
                            }
                            $figcaption.innerHTML = captionData.html
                            if (newLicense) {
                                $figcaption.setAttribute('data-license', newLicense)
                            }
                            addFigcaptionClickHandler($figcaption, captionData.html, newLicense, src, alt)
                            $container.appendChild($figcaption)
                        }
                    }
                )
            }

            const paintPositionController = () => {
                const $positionController = document.createElement('div')

                const $leftController = document.createElement('div')
                const $centerController = document.createElement('div')
                const $rightController = document.createElement('div')

                $positionController.setAttribute('class', 'position-controller')

                // Set up left alignment button
                $leftController.classList.add('menubar_button')
                const leftIcon = document.createElement('i')
                leftIcon.classList.add('fa-solid', 'fa-align-left')
                $leftController.appendChild(leftIcon)
                $leftController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 auto 0 0;`)
                    dispatchNodeView()
                })

                // Set up center alignment button  
                $centerController.classList.add('menubar_button')
                const centerIcon = document.createElement('i')
                centerIcon.classList.add('fa-solid', 'fa-align-center')
                $centerController.appendChild(centerIcon)
                $centerController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 auto;`)
                    dispatchNodeView()
                })

                // Set up right alignment button
                $rightController.classList.add('menubar_button')
                const rightIcon = document.createElement('i')
                rightIcon.classList.add('fa-solid', 'fa-align-right')
                $rightController.appendChild(rightIcon)
                $rightController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 0 0 auto;`)
                    dispatchNodeView()
                })

                $positionController.appendChild($leftController)
                $positionController.appendChild($centerController)
                $positionController.appendChild($rightController)

                $container.appendChild($positionController)

                // Add separate caption/license editing button
                const $captionController = document.createElement('div')
                $captionController.classList.add('menubar_button', 'caption-controller')
                const captionIcon = document.createElement('i')
                captionIcon.classList.add('fa-solid', 'fa-file-contract')
                $captionController.appendChild(captionIcon)
                $captionController.title = 'Edit caption and license'
                $captionController.addEventListener('click', (e) => {
                    e.preventDefault()
                    e.stopPropagation()
                    showCaptionModal()
                })

                $container.appendChild($captionController)

                // Convert Font Awesome icons to SVG
                if (dom && dom.i2svg) {
                    dom.i2svg({ node: $container })
                }
            }

            // Set up wrapper and container
            $wrapper.setAttribute('style', `display: flex;`)
            $wrapper.appendChild($container)

            // Ensure no inline border styling initially (let CSS handle transparent border)
            const cleanStyle = style.replace(/border:\s*[^;]*;?/g, '')
            $container.setAttribute('style', cleanStyle)
            $container.className = 'tiptap-figure'
            
            // Set up image
            $img.setAttribute('src', src || '')
            $img.className = 'tiptap-image'
            if (alt) $img.setAttribute('alt', alt)
            $container.appendChild($img)

            // Add figcaption if caption or license exists
            const captionData = getFigcaptionContent(caption, license)
            if (captionData.html) {
                const $figcaption = document.createElement('figcaption')
                $figcaption.className = 'tiptap-figcaption'
                if (!captionData.hasLicense) {
                    $figcaption.classList.add('no-license')
                }
                $figcaption.innerHTML = captionData.html
                if (license) {
                    $figcaption.setAttribute('data-license', license)
                }
                addFigcaptionClickHandler($figcaption, caption, license, src, alt)
                $container.appendChild($figcaption)
            }

            // If not editable, return simple view
            if (!editable) return { dom: $container }

            // Add resize functionality for editable mode
            const isMobile = document.documentElement.clientWidth < 768
            const dotPosition = isMobile ? '-8px' : '-4px'
            const dotsPosition = [
                `top: ${dotPosition}; left: ${dotPosition}; cursor: nwse-resize;`,
                `top: ${dotPosition}; right: ${dotPosition}; cursor: nesw-resize;`,
                `bottom: ${dotPosition}; left: ${dotPosition}; cursor: nesw-resize;`,
                `bottom: ${dotPosition}; right: ${dotPosition}; cursor: nwse-resize;`,
            ]

            let isResizing = false
            let startX, startWidth

            $img.addEventListener('click', (e) => {
                e.preventDefault()
                e.stopPropagation()

                // Remove remaining dots and position controller (but keep floating button)
                const isMobile = document.documentElement.clientWidth < 768
                isMobile && (document.querySelector('.ProseMirror-focused')?.blur())

                // Count base elements: img + optional figcaption
                const hasFigcaption = $container.querySelector('figcaption')
                const baseElementCount = hasFigcaption ? 2 : 1
                if ($container.childElementCount > baseElementCount) {
                    // Remove all controls except the base elements
                    const elementsToKeep = [$img]
                    if (hasFigcaption) {
                        elementsToKeep.push(hasFigcaption)
                    }
                    
                    const children = Array.from($container.children)
                    children.forEach(child => {
                        if (!elementsToKeep.includes(child)) {
                            $container.removeChild(child)
                        }
                    })
                }

                paintPositionController()

                // Add resize handles to the figure - show green border when clicked for all images
                $container.setAttribute('style', `position: relative; border: ${borderStyle}; ${style} cursor: pointer;`)

                Array.from({ length: 4 }, (_, index) => {
                    const $dot = document.createElement('div')
                    $dot.setAttribute(
                        'style',
                        `${resizeHandle} ${dotsPosition[index]}`
                    )

                    $dot.addEventListener('mousedown', (e) => {
                        e.preventDefault()
                        isResizing = true
                        startX = e.clientX
                        startWidth = $container.offsetWidth

                        const onMouseMove = (e) => {
                            if (!isResizing) return
                            const deltaX = index % 2 === 0 ? -(e.clientX - startX) : e.clientX - startX
                            const newWidth = startWidth + deltaX

                            $container.style.width = newWidth + 'px'
                        }

                        const onMouseUp = () => {
                            if (isResizing) {
                                isResizing = false
                            }
                            dispatchNodeView()

                            document.removeEventListener('mousemove', onMouseMove)
                            document.removeEventListener('mouseup', onMouseUp)
                        }

                        document.addEventListener('mousemove', onMouseMove)
                        document.addEventListener('mouseup', onMouseUp)
                    })

                    // Touch support for mobile
                    $dot.addEventListener(
                        'touchstart',
                        (e) => {
                            e.cancelable && e.preventDefault()
                            isResizing = true
                            startX = e.touches[0].clientX
                            startWidth = $container.offsetWidth

                            const onTouchMove = (e) => {
                                if (!isResizing) return
                                const deltaX = index % 2 === 0 
                                    ? -(e.touches[0].clientX - startX) 
                                    : e.touches[0].clientX - startX
                                const newWidth = startWidth + deltaX

                                $container.style.width = newWidth + 'px'
                            }

                            const onTouchEnd = () => {
                                if (isResizing) {
                                    isResizing = false
                                }
                                dispatchNodeView()

                                document.removeEventListener('touchmove', onTouchMove)
                                document.removeEventListener('touchend', onTouchEnd)
                            }

                            document.addEventListener('touchmove', onTouchMove)
                            document.addEventListener('touchend', onTouchEnd)
                        },
                        { passive: false }
                    )
                    
                    $container.appendChild($dot)
                })
            })

            // Click outside to remove controls
            document.addEventListener('click', (e) => {
                const $target = e.target
                const isClickInside = $container.contains($target) || $target.style.cssText === iconStyle

                if (!isClickInside) {
                    // Remove inline border styling completely to fall back to CSS defaults
                    const containerStyle = $container.getAttribute('style')
                    let newStyle = containerStyle?.replace(`border: ${borderStyle};`, '') || ''
                    // Also remove any leftover border styling
                    newStyle = newStyle.replace(/border:\s*[^;]*;?/g, '')
                    $container.setAttribute('style', newStyle)

                    // Count base elements: img + optional figcaption
                    const hasFigcaption = $container.querySelector('figcaption')
                    const baseElementCount = hasFigcaption ? 2 : 1
                    if ($container.childElementCount > baseElementCount) {
                        // Remove all controls except the base elements
                        const elementsToKeep = [$img]
                        if (hasFigcaption) {
                            elementsToKeep.push(hasFigcaption)
                        }
                        
                        const children = Array.from($container.children)
                        children.forEach(child => {
                            if (!elementsToKeep.includes(child)) {
                                $container.removeChild(child)
                            }
                        })
                    }
                }
            })

            return {
                dom: $wrapper,
            }
        }
    },
})

export { ImageResize, ImageResize as default }
