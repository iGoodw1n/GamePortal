// Use the module syntax to export the function
let connection;
let interval;

export function connectToHub(gameId, userId) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`/gamehub?gameId=${gameId}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();
    async function start() {
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    // Start the connection.
    start();

    const updateStatus = async () => {
        await connection.invoke("Ping", userId)
    }

    interval = setInterval(updateStatus, 1000);
}

export function stopPing() {
    clearInterval(interval);
    connection.stop();
}
