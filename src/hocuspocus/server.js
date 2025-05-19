import { Server } from "@hocuspocus/server"
import Redis from "ioredis"
import dotenv from "dotenv"
import axios from "axios"
import { Database } from "@hocuspocus/extension-database"
import express from "express"
import expressWebsockets from "express-ws"
import cookie from "cookie"

if (process.env.ENVIRONMENT.trim() == "dev") {
    dotenv.config()
}

function extractTokens(tokenString) {
    if (!tokenString || typeof tokenString !== "string") {
        return { collaborationToken: null, shareToken: null }
    }

    if (tokenString.includes("|accessToken=")) {
        const parts = tokenString.split("|accessToken=")
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
    host: process.env.REDIS_HOST || "localhost",
    port: process.env.REDIS_PORT || 6379,
})

const redisDatabaseExtension = new Database({
    fetch: async ({ documentName }) => {
        const data = await redis.get(documentName)
        if (data) {
            return Buffer.from(data, "base64")
        }
        return null
    },
    store: async ({ documentName, state }) => {
        await redis.set(documentName, state.toString("base64"))
    },
})

const server = Server.configure({
    name: "hocuspocus-dev",
    port: 3010,
    timeout: 30000,
    debounce: 5000,
    maxDebounce: 30000,
    quiet: false,
    extensions: [redisDatabaseExtension],
    async onAuthenticate({ documentName, token, connection, requestHeaders }) {
        const raw = requestHeaders.cookie || ""
        const cookies = cookie.parse(raw)
        const sessionId = cookies[".AspNetCore.Session"]
        if (!sessionId) {
            throw new Error("No ASP.NET session cookie")
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
                    if (response.data.canEdit === false)
                        connection.readOnly = true

                    return
                } else throw new Error("Not authorized!")
            })
    },
})

const { app } = expressWebsockets(express())

app.ws("/", (websocket, request) => {
    server.handleConnection(websocket, request)
})

if (process.env.ENVIRONMENT.trim() == "dev") {
    app.ws("/localCollab", (websocket, request) => {
        server.handleConnection(websocket, request)
    })
}

const PORT = 3010
app.listen(PORT, () => {
    console.log(`Server is running on Port:${PORT}`)
})
