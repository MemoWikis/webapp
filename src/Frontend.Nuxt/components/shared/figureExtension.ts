//TipTap Figure Extension - based on: https://github.com/bae-sh/tiptap-extension-resize-image
//Provides figure/image support with resizing, captions, and license information

import Image from '@tiptap/extension-image'
import { dom } from '@fortawesome/fontawesome-svg-core'
import { color } from '~/constants/colors'
import { useFigureExtensionStore } from './figureExtensionStore'

const resizeHandle = `position: absolute; width: 9px; height: 9px; border: 2px solid ${color.memoGreyDarker}; border-radius: 50%;`

// Helper function to get figcaption content
const getFigcaptionContent = (caption: string | null, license: string | null) => {
    // Helper to check if HTML content has actual content (not just empty tags)
    const hasActualContent = (html: string | null): boolean => {
        if (!html) 
            return false

        const trimmed = html.trim()
        return trimmed !== '' && trimmed !== '<p></p>' && trimmed !== '<br>'
    }
    
    // Helper to strip HTML tags for text-only rendering
    const stripHtml = (html: string | null): string => {
        if (!html) 
            return ''

        // Simple regex-based HTML tag removal that works in both client and server environments
        return html.replace(/<[^>]*>/g, '').replace(/&nbsp;/g, ' ').replace(/&amp;/g, '&').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').trim()
    }
    
    // Helper to strip p tags but keep other formatting
    const stripPTags = (html: string | null): string => {
        if (!html) 
            return ''
        
        return html.replace(/<\/?p[^>]*>/g, '').trim()
    }
    
    const cleanCaption = stripPTags(caption || '')
    const cleanLicense = stripPTags(license || '')
    
    const hasCaption = hasActualContent(cleanCaption)
    const hasLicense = hasActualContent(cleanLicense)
    
    // Create HTML version (for interactive use) - preserve HTML formatting but no p tags
    let html = ''
    if (!hasCaption && !hasLicense) html = `(No license)`
    else if (!hasCaption && hasLicense) html = cleanLicense
    else if (hasCaption && !hasLicense) html = `${cleanCaption}. (No license)`
    else html = `${cleanCaption}. ${cleanLicense}`
    
    // Create text version (for SSR/prerendering)
    let text = ''
    const captionText = stripHtml(cleanCaption)
    const licenseText = stripHtml(cleanLicense)
    
    if (!captionText && !licenseText) text = `(No license)`
    else if (!captionText && licenseText) text = `${licenseText}`
    else if (captionText && !licenseText) text = `${captionText}. (No license)`
    else text = `${captionText}. ${licenseText}`
    
    return { html, text, hasLicense }
}

// Helper function to add figcaption click handler
const addFigcaptionClickHandler = (
    $figcaption: HTMLElement, 
    caption: string | null, 
    license: string | null, 
    src: string | null, 
    alt: string | null, 
    showModalFn?: () => void
): void => {
    $figcaption.style.cursor = 'pointer'
    $figcaption.addEventListener('click', (e: MouseEvent) => {
        // Don't trigger if clicking on a link or if the target is inside a link
        const target = e.target as HTMLElement
        if (target.tagName === 'A' || target.closest('a')) {
            return // Let the link handle its own click
        }
        
        e.preventDefault()
        e.stopPropagation()
        
        // Use the provided modal function (which has access to the update logic)
        if (showModalFn) {
            showModalFn()
        } else {
            // Fallback: just open edit modal without update logic
            const store = useFigureExtensionStore()
            store.openEditModal(caption || '', license || '', src || '', alt || '', () => {})
        }
    })
}

const FigureExtension = Image.extend({
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
                    return figcaption?.getAttribute('data-caption') || null
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
                    
                    let caption = null
                    let license = null
                    
                    if (figcaption) {
                        // Always get caption and license from data attributes
                        caption = figcaption.getAttribute('data-caption') || null
                        license = figcaption.getAttribute('data-license') || null
                        
                        // Clean up empty values
                        if (!caption || caption === '') caption = null
                        if (!license || license === '') license = null
                    }
                    
                    return {
                        src: img.getAttribute('src'),
                        alt: img.getAttribute('alt'),
                        title: img.getAttribute('title'),
                        caption: caption,
                        license: license,
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
        
        // Ensure img element always has tiptap-image class, remove any existing class from imgAttrs
        const { class: imgClass, ...cleanImgAttrs } = imgAttrs
        const imgElement = ['img', { src, alt, class: 'tiptap-image', ...cleanImgAttrs }]
        
        const captionData = getFigcaptionContent(caption, license)
        if (captionData.text) {
            const figcaptionClasses = ['tiptap-figcaption']
            if (!captionData.hasLicense) {
                figcaptionClasses.push('no-license')
            }
            
            const figcaptionAttrs: Record<string, string> = {
                class: figcaptionClasses.join(' ')
            }
            
            if (license) {
                figcaptionAttrs['data-license'] = license
            }
            
            if (caption) {
                figcaptionAttrs['data-caption'] = caption
            }
            
            // Use text version for SSR/prerendering to avoid HTML display issues
            return [
                'figure',
                { style, class: 'tiptap-figure' },
                imgElement,
                ['figcaption', figcaptionAttrs, captionData.text]
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
                const store = useFigureExtensionStore()
                store.openEditModal(
                    caption, 
                    license, 
                    src, 
                    alt,
                    (data: { caption: string | null, license: string | null }) => {
                        const { caption: newCaption, license: newLicense } = data
                        
                        // Create the complete figcaption HTML
                        const captionData = getFigcaptionContent(newCaption, newLicense)
                        
                        // Update node attributes - store caption and license separately
                        if (typeof getPos === 'function') {
                            const newAttrs = {
                                ...node.attrs,
                                caption: newCaption,    // Store only the caption content
                                license: newLicense     // Store only the license content
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
                            
                            // Always add data attributes for future parsing
                            if (newCaption) {
                                $figcaption.setAttribute('data-caption', newCaption)
                            }
                            if (newLicense) {
                                $figcaption.setAttribute('data-license', newLicense)
                            }
                            
                            addFigcaptionClickHandler($figcaption, newCaption, newLicense, src, alt, showCaptionModal)
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
                
                // Add no-license class if there's no license
                if (!license) {
                    $captionController.classList.add('no-license')
                }
                
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

            // Set up container styling
            $container.setAttribute('style', style)
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
                
                // Always add data attributes for future parsing
                if (caption) {
                    $figcaption.setAttribute('data-caption', caption)
                }
                if (license) {
                    $figcaption.setAttribute('data-license', license)
                }
                
                addFigcaptionClickHandler($figcaption, caption, license, src, alt, showCaptionModal)
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
            let startX: number
            let startWidth: number

            $img.addEventListener('click', (e) => {
                e.preventDefault()
                e.stopPropagation()

                // Remove remaining dots and position controller (but keep floating button)
                const isMobile = document.documentElement.clientWidth < 768
                if (isMobile) {
                    const focusedElement = document.querySelector('.ProseMirror-focused') as HTMLElement
                    focusedElement?.blur()
                }

                // Count base elements: img + optional figcaption
                const hasFigcaption = $container.querySelector('figcaption') as HTMLElement | null
                const baseElementCount = hasFigcaption ? 2 : 1
                if ($container.childElementCount > baseElementCount) {
                    // Remove all controls except the base elements
                    const elementsToKeep: (Element | HTMLElement)[] = [$img]
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

                // Add resize handles to the figure - add active class for styling
                $container.classList.add('active')

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

                        const onMouseMove = (e: MouseEvent) => {
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

                            const onTouchMove = (e: TouchEvent) => {
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
                const $target = e.target as Element
                if (!$target) return
                
                const isClickInside = $container.contains($target) || 
                    ($target instanceof Element && $target.classList.contains('menubar_button'))

                if (!isClickInside) {
                    // Remove active class to hide border and controls
                    $container.classList.remove('active')

                    // Count base elements: img + optional figcaption
                    const hasFigcaption = $container.querySelector('figcaption') as HTMLElement | null
                    const baseElementCount = hasFigcaption ? 2 : 1
                    if ($container.childElementCount > baseElementCount) {
                        // Remove all controls except the base elements
                        const elementsToKeep: (Element | HTMLElement)[] = [$img]
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

// Client-side hydration function to upgrade figcaptions after SSR
const hydrateFigcaptions = () => {
    // Only hydrate figcaptions inside #AnswerBody
    const answerBody = document.getElementById('AnswerBody')
    if (!answerBody) return
    
    const figcaptions = answerBody.querySelectorAll('figcaption[data-caption], figcaption[data-license]')
    
    figcaptions.forEach((figcaption) => {
        const caption = figcaption.getAttribute('data-caption')
        const license = figcaption.getAttribute('data-license')
        
        // Re-render with proper HTML content
        const captionData = getFigcaptionContent(caption, license)
        if (captionData.html) {
            figcaption.innerHTML = captionData.html
            
            // Add click handlers for interactive behavior
            const figure = figcaption.closest('figure')
            const img = figure?.querySelector('img')
            const src = img?.getAttribute('src') || null
            const alt = img?.getAttribute('alt') || null
            
            addFigcaptionClickHandler(
                figcaption as HTMLElement, 
                caption, 
                license, 
                src, 
                alt
            )
        }
    })
}

export { FigureExtension, FigureExtension as default, hydrateFigcaptions }
