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
import { generateHTML, generateJSON } from '@tiptap/html'
import StarterKit from '@tiptap/starter-kit'
import Underline from '@tiptap/extension-underline'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { yDocToProsemirrorJSON, prosemirrorJSONToYDoc } from 'y-prosemirror'

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

// TipTap extensions for HTML parsing (must match frontend configuration)
const extensions = [
  StarterKit.configure({
    heading: {
      levels: [2, 3, 4],
    },
    codeBlock: false,
  }),
  Underline,
  TaskList,
  TaskItem.configure({
    nested: true,
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
  // Parse HTML to ProseMirror JSON using TipTap
  const json = generateJSON(html || '<p></p>', extensions)

  // Convert ProseMirror JSON to Y.Doc
  const ydoc = prosemirrorJSONToYDoc(
    StarterKit.configure().schema || undefined,
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
