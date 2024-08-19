import { Server } from "@hocuspocus/server"
import { Redis as RedisExtension } from "@hocuspocus/extension-redis"
import Redis from 'ioredis'

// Create a Redis client
const redis = new Redis();

// Define custom database extension for Redis
const redisDatabaseExtension = Database({
  fetch: async ({ documentName }) => {
    // Fetch the document from Redis
    const data = await redis.get(documentName);
    if (data) {
      return Buffer.from(data, 'base64');
    }
    return null;
  },
  store: async ({ documentName, state }) => {
    // Store the document in Redis
    await redis.set(documentName, state.toString('base64'));
  },
});

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
  async onAuthenticate({ documentName, token }) {
    // Could be an API call, DB query or whatever …
    // The endpoint should return 200 OK in case the user is authenticated, and an http error
    // in case the user is not.
    console.log(documentName, token)
  },
})



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

// server.listen();