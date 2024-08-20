import { Server } from "@hocuspocus/server"
import { Redis as RedisExtension } from "@hocuspocus/extension-redis"
import Redis from 'ioredis'
import dotenv from 'dotenv'
import axios from 'axios'

if (process.env.NODE_ENV !== 'production') {
  dotenv.config()
}

// Create a Redis client
const redis = new Redis()

// Define custom database extension for Redis
const redisDatabaseExtension = Database({
  fetch: async ({ documentName }) => {
    // Fetch the document from Redis
    const data = await redis.get(documentName)
    if (data) {
      return Buffer.from(data, 'base64')
    }
    return null;
  },
  store: async ({ documentName, state }) => {
    // Store the document in Redis
    await redis.set(documentName, state.toString('base64'))
  },
})

// Configure Hocuspocus
const server = Server.configure({
  name: "hocuspocus-dev",
  port: 3010,
  timeout: 30000,
  debounce: 5000,
  maxDebounce: 30000,
  quiet: false,
  extensions: [
    // new RedisExtension({
    //   // [required] Hostname of your Redis instance
    //   host: "localhost",

    //   // [required] Port of your Redis instance
    //   port: 6379,
    // }),
    redisDatabaseExtension
  ],
  async onConnect({connection}) {
        throw new Error("Not authorized!")

    console.log('connected')
    connection.requiresAuthentication = true;
  },
  async onAuthenticate({ documentName, token }) {
    throw new Error("Not authorized!")

    console.log('documentName---', documentName)
    console.log('token---', token)

    return
    // const result = await axios.post("http://localhost:3000/apiVue/Hocuspocus/Authorise", {
    //   token: token,
    //   hocuspocusKey: process.env.HOCUSPOCUS_SECRET_KEY,
    //   topicId: documentName.substring(5)
    // })
    // console.log('result---', result)
    // console.log(documentName, token, result)
  },
})

// start with: npx @hocuspocus/cli --port 3010
server.listen()



// Setup your express instance using the express-ws extension
// const { app } = expressWebsockets(express())

// // A basic http route
// app.get("/", (request, response) => {
//   response.send("Hello World!")
// })

// // Add a websocket route for Hocuspocus
// // You can set any contextual data like in the onConnect hook
// // and pass it to the handleConnection method.
// app.ws("/collaboration", (websocket, request) => {
//   server.handleConnection(websocket, request)
// })

// const PORT = 3010;
// app.listen(PORT, () => {
//   console.log(`Server is running on http://localhost:${PORT}`)
// })
