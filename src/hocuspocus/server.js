import express from "express"
import expressWebsockets from "express-ws"
import { Server } from "@hocuspocus/server"
import { Redis } from "@hocuspocus/extension-redis"

// Configure Hocuspocus
const server = Server.configure({
  name: "hocuspocus-dev",
  port: 3010,
  timeout: 30000,
  debounce: 5000,
  maxDebounce: 30000,
  quiet: false,
  // extensions: [
  //   new Redis({
  //     // [required] Hostname of your Redis instance
  //     host: "localhost",

  //     // [required] Port of your Redis instance
  //     port: 6379,
  //   }),
  // ],
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
//   const context = {
//     user: {
//       id: 1234,
//       name: "Jane",
//     },
//   }
//   server.handleConnection(websocket, request, context)
// })

// const PORT = 3010;
// app.listen(PORT, () => {
//   console.log(`Server is running on http://localhost:${PORT}`)
// })

server.listen();