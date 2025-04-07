import { Server } from "@hocuspocus/server"
import Redis from 'ioredis'
import dotenv from 'dotenv'
import axios from 'axios'
import { Database } from "@hocuspocus/extension-database"
import express from "express"
import expressWebsockets from "express-ws"

if (process.env.ENVIRONMENT.trim() == 'development') {
  dotenv.config()
}

function extractDocumentInfo(documentName) {
  if (!documentName || typeof documentName !== 'string') {
    return { documentName, pageId: null, shareToken: null }
  }
  
  if (!documentName.startsWith('ydoc-')) {
    return { documentName, pageId: null, shareToken: null }
  }
  
  const withoutPrefix = documentName.substring(5)
  
  if (withoutPrefix.includes(':')) {
    const [pageIdStr, shareToken] = withoutPrefix.split(':')
    const pageId = parseInt(pageIdStr, 10)
    
    return {
      documentName,
      pageId: isNaN(pageId) ? null : pageId,
      shareToken
    }
  } else {
    const pageId = parseInt(withoutPrefix, 10)
    
    return {
      documentName,
      pageId: isNaN(pageId) ? null : pageId,
      shareToken: null
    }
  }
}

const redis = new Redis({
  host: process.env.REDIS_HOST || 'localhost',
  port: process.env.REDIS_PORT || 6379,
})

const redisDatabaseExtension = new Database({
  fetch: async ({ documentName }) => {
    const documentInfo = extractDocumentInfo(documentName)

    const data = await redis.get(documentInfo.documentName)
    if (data) {
      return Buffer.from(data, 'base64')
    }
    return null
  },
  store: async ({ documentName, state }) => {
    const documentInfo = extractDocumentInfo(documentName)

    await redis.set(documentInfo.documentName, state.toString('base64'))
  },
})

const server = Server.configure({
  name: "hocuspocus-dev",
  port: 3010,
  timeout: 30000,
  debounce: 5000,
  maxDebounce: 30000,
  quiet: false,
  extensions: [
    redisDatabaseExtension,
  ],
  async onAuthenticate({ documentName, token, connection }) {
    const documentInfo = extractDocumentInfo(documentName)
    const data = {
      token: token,
      hocuspocusKey: process.env.HOCUSPOCUS_SECRET_KEY,
      pageId: documentInfo.pageId,
      shareToken: documentInfo.shareToken
    }
    await axios.post(`${process.env.BACKEND_BASE_URL}/apiVue/Hocuspocus/Authorise`, data).then(function (response) {
      if (response.status === 200 && response.data.canView === true) {

        if (response.data.canEdit === false)
          connection.readOnly = true

        return
      }
      else 
        throw new Error("Not authorized!")
    })
  },
})

const { app } = expressWebsockets(express())

app.ws("/", (websocket, request) => {
  server.handleConnection(websocket, request)
})

const PORT = 3010;
app.listen(PORT, () => {
  console.log(`Server is running on Port:${PORT}`)
})
