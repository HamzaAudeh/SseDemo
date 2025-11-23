# Server-Sent Events (SSE)

This repo showcases **Server-Sent Events (SSE)**. SSE is a one-way (server to client) notification mechanism. It's natively supported by most browsers as it works on HTTP.
It is simple, yet powerful mechanism to push real-time data from server to client. It uses a single long-lived http connection to transmit data. Unlike long-polling, it receives data pushed from server directly rather than keeps on calling server for updates.

# What's inside the repo
The repo is built using .Net Core 8 with minimal endpoints, and it has a simple html page that can be run directly in browser.
The backend exposes two endpoints that send data to the client. Once the client is connected, the page shows two adjacent tables; te first shows data broadcasted from server, and the second table shows data from a specific event only.
The page has also a button that starts/stops connection to mimic a connection close.
