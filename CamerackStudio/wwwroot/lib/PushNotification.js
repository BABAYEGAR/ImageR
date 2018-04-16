const connection = new signalR.HubConnection(
    "/NotificationHub", { logger: signalR.LogLevel.Information });

connection.on("GetNotification", (user, message) => {
    const encodedMsg = user + " says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


connection.start().catch(err => console.error);