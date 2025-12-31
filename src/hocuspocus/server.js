import { Server } from '@hocuspocus/server'
import Redis from 'ioredis'
import dotenv from 'dotenv'
import axios from 'axios'
import { Database } from '@hocuspocus/extension-database'
import express from 'express'
import expressWebsockets from 'express-ws'
import cookie from 'cookie'

// Y.js and TipTap imports for HTML to Y.Doc conversion
import * as Y from 'yjs'
import { generateJSON } from '@tiptap/html'
import { getSchema } from '@tiptap/core'
import StarterKit from '@tiptap/starter-kit'
import Underline from '@tiptap/extension-underline'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import Link from '@tiptap/extension-link'
import Heading from '@tiptap/extension-heading'
import Image from '@tiptap/extension-image'
import { prosemirrorJSONToYDoc } from 'y-prosemirror'

if (process.env.ENVIRONMENT.trim() == 'dev') {
  dotenv.config()
}

function extractTokens(tokenString) {
  if (!tokenString || typeof tokenString !== 'string') {
    return { collaborationToken: null, shareToken: null }
  }

  if (tokenString.includes('|accessToken=')) {
    const parts = tokenString.split('|accessToken=')
    return {
      collaborationToken: parts[0],
      shareToken: parts[1],
    }
  } else {
    return {
      collaborationToken: tokenString,
      shareToken: null,
    }
  }
}

const redis = new Redis({
  host: process.env.REDIS_HOST || 'localhost',
  port: process.env.REDIS_PORT || 6379,
})

// TipTap extensions for HTML parsing
// These must match the frontend schema to ensure proper Y.Doc compatibility
// Note: We use base extensions here since custom frontend extensions use DOM APIs

// Custom Heading extension (matches frontend CustomHeading schema)
const ServerHeading = Heading.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      level: {
        default: 2,
      },
      id: {
        default: null,
        parseHTML: (element) => element.getAttribute('id'),
        renderHTML: (attributes) => {
          if (!attributes.id) return {}
          return { id: attributes.id }
        },
      },
    }
  },
})

// Custom Link extension (matches frontend CustomLink schema)
const ServerLink = Link.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      target: {
        default: '_self',
      },
    }
  },
})

// Figure extension for images with captions (matches frontend FigureExtension schema)
const ServerFigure = Image.extend({
  name: 'figure',
  
  addAttributes() {
    return {
      src: { default: null },
      alt: { default: null },
      style: { default: 'width: 100%; height: auto; cursor: pointer;' },
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
      title: { default: null },
      width: { default: null },
      height: { default: null },
    }
  },

  parseHTML() {
    return [
      {
        tag: 'figure',
        getAttrs: (element) => {
          const img = element.querySelector('img')
          if (!img) return false
          
          const figcaption = element.querySelector('figcaption')
          let caption = figcaption?.getAttribute('data-caption') || null
          let license = figcaption?.getAttribute('data-license') || null
          
          return {
            src: img.getAttribute('src'),
            alt: img.getAttribute('alt'),
            title: img.getAttribute('title'),
            caption,
            license,
            style: element.style?.cssText || 'width: 100%; height: auto; cursor: pointer;'
          }
        }
      },
      {
        tag: 'img',
        getAttrs: (element) => ({
          src: element.getAttribute('src'),
          alt: element.getAttribute('alt'),
          title: element.getAttribute('title'),
          style: element.style?.cssText || 'width: 100%; height: auto; cursor: pointer;'
        })
      }
    ]
  },

  renderHTML({ HTMLAttributes }) {
    const { caption, license, src, alt, style, ...imgAttrs } = HTMLAttributes
    return [
      'figure',
      { style, class: 'tiptap-figure' },
      ['img', { src, alt, class: 'tiptap-image', ...imgAttrs }]
    ]
  },
})

const extensions = [
  StarterKit.configure({
    heading: false, // We use custom heading
    codeBlock: true, // Use built-in codeBlock (lowlight is frontend-only)
  }),
  ServerHeading.configure({
    levels: [2, 3, 4],
  }),
  ServerLink.configure({
    openOnClick: false,
  }),
  Underline,
  TaskList,
  TaskItem.configure({
    nested: true,
  }),
  ServerFigure.configure({
    inline: true,
    allowBase64: true,
  }),
]

/**
 * Fetches page content from the backend API (MySQL via EntityCache)
 * @param {string} pageId - The page ID
 * @returns {Promise<string|null>} - The HTML content or null
 */
async function fetchContentFromBackend(pageId) {
  try {
    const response = await axios.post(
      `${process.env.BACKEND_BASE_URL}/apiVue/Hocuspocus/GetContent`,
      {
        hocuspocusKey: process.env.HOCUSPOCUS_SECRET_KEY,
        pageId: parseInt(pageId, 10),
      }
    )

    if (response.status === 200 && response.data.success) {
      return response.data.content
    }
    return null
  } catch (error) {
    console.error(`Failed to fetch content for page ${pageId}:`, error.message)
    return null
  }
}

/**
 * Converts HTML content to a Y.Doc binary state
 * @param {string} html - The HTML content
 * @returns {Uint8Array} - The Y.Doc state as binary
 */
function htmlToYDocState(html) {
  // Parse HTML to ProseMirror JSON using TipTap extensions
  const json = generateJSON(html || '<p></p>', extensions)

  // Get schema from our extensions for proper Y.Doc conversion
  const schema = getSchema(extensions)
  
  // Convert ProseMirror JSON to Y.Doc
  const ydoc = prosemirrorJSONToYDoc(
    schema,
    json,
    'default' // fragment name used by Collaboration extension
  )

  // Return the Y.Doc state as binary
  return Y.encodeStateAsUpdate(ydoc)
}

const redisDatabaseExtension = new Database({
  fetch: async ({ documentName }) => {
    // First, try to get from Redis
    const cachedData = await redis.get(documentName)
    if (cachedData) {
      console.log(`[${documentName}] Loaded from Redis cache`)
      return Buffer.from(cachedData, 'base64')
    }

    // If not in Redis, fetch from backend (MySQL)
    const pageId = documentName.substring(5) // "ydoc-123" → "123"
    console.log(`[${documentName}] Not in Redis, fetching from backend...`)

    const htmlContent = await fetchContentFromBackend(pageId)

    if (htmlContent && htmlContent.trim() !== '') {
      console.log(
        `[${documentName}] Got content from backend, converting to Y.Doc`
      )
      try {
        const state = htmlToYDocState(htmlContent)

        // Store in Redis for future requests
        await redis.set(documentName, Buffer.from(state).toString('base64'))
        console.log(`[${documentName}] Stored in Redis cache`)

        return state
      } catch (error) {
        console.error(
          `[${documentName}] Failed to convert HTML to Y.Doc:`,
          error.message
        )
        return null
      }
    }

    console.log(`[${documentName}] No content found in backend`)
    return null
  },
  store: async ({ documentName, state }) => {
    await redis.set(documentName, state.toString('base64'))
  },
})

const server = Server.configure({
  name: 'hocuspocus-dev',
  port: 3010,
  timeout: 30000,
  debounce: 5000,
  maxDebounce: 30000,
  quiet: false,
  extensions: [redisDatabaseExtension],
  async onAuthenticate({ documentName, token, connection, requestHeaders }) {
    const raw = requestHeaders.cookie || ''
    const cookies = cookie.parse(raw)
    const sessionId = cookies['.AspNetCore.Session']
    if (!sessionId) {
      throw new Error('No ASP.NET session cookie')
    }

    const tokens = extractTokens(token)
    const data = {
      token: tokens.collaborationToken,
      hocuspocusKey: process.env.HOCUSPOCUS_SECRET_KEY,
      pageId: documentName.substring(5),
      shareToken: tokens.shareToken,
    }

    await axios
      .post(
        `${process.env.BACKEND_BASE_URL}/apiVue/Hocuspocus/Authorize`,
        data,
        {
          headers: {
            Cookie: raw,
          },
          withCredentials: true,
        }
      )
      .then(function (response) {
        if (response.status === 200 && response.data.canView === true) {
          if (response.data.canEdit === false) connection.readOnly = true

          return
        } else throw new Error('Not authorized!')
      })
  },
})

const { app } = expressWebsockets(express())

app.ws('/', (websocket, request) => {
  server.handleConnection(websocket, request)
})

if (process.env.ENVIRONMENT.trim() == 'dev') {
  app.ws('/localCollab', (websocket, request) => {
    server.handleConnection(websocket, request)
  })
}

const PORT = 3010
app.listen(PORT, () => {
  console.log(`Server is running on Port:${PORT}`)
})
