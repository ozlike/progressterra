var connection, container, context, mousepress = false, oldX, oldY;

document.addEventListener('DOMContentLoaded', function () {
    connection = new signalR.HubConnectionBuilder().withUrl('/data').build();
    connection.on('broadcastMessage', bcast);
    connection.start();

   

    
});

function bcast(message) {

    let mes = 0;
}

