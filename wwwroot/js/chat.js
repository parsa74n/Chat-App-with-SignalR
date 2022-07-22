var connection = null;

$(function () {
    if (connection === null) {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .build();

        connection.on("Broadcast", function (response) {
            $("#chatRoom").append(
              
                '<div class="media w-50 ml-auto mb-3">' +
                '<img src="https://img.icons8.com/officel/16/000000/user.png" alt="user" width="50" class="rounded-circle">' +
                '<div class="media-body">' +
                '<div class="bg-primary rounded py-2 px-3 mb-2">' +
                '<p class="text-small mb-0 text-white">' + response.messageBody + '</p>' +
                '</div>' +
                '<p class="small text-muted">' + response.fromUser + '</p>' +
                '<p class="small text-muted">' + response.messageDtTm + '</p>' +
                '</div>' +
                '</div>'
            );

            $('#chatRoom').stop().animate({
                scrollTop: $('#chatRoom')[0].scrollHeight
            });
        });

        connection.on("UserConnected", function (response) {
    
            $("#chatRoom").append(
                '<div class="media w-50 mb-3">' +
                '<img src="https://img.icons8.com/dusk/64/000000/user-female-skin-type-1-2.png" alt="user" width="50" class="rounded-circle">' +
                '<div class="media-body ml-3">' +
                '<div class="bg-light rounded py-2 px-3 mb-2">' +
                '<p class="text-small mb-0 text-muted">به چت روم خوش آمدید! ' + response.userId+'</p>' +
                '</div>' +
                '<p class="small text-muted">' + response.messageDtTm + '</p>' +
                '</div>'
            );
        });

        connection.on("UserDisconnected", function (response) {
            $("#chatRoom").append(
                '<div class="media w-50 mb-3">' +
                '<img src="https://img.icons8.com/dusk/64/000000/user-female-skin-type-1-2.png" alt="user" width="50" class="rounded-circle">' +
                '<div class="media-body ml-3">' +
                '<div class="bg-light rounded py-2 px-3 mb-2">' +
                '<p class="text-small mb-0 text-muted">از چت خارج شد! ' + response.elementId + '</p>' +
                '</div>' +
                '<p class="small text-muted">' + response.messageDtTm + '</p>' +
                '</div>'
            );
            alert(response.message);
           
        });

        connection.on("HubError", function (response) {
            alert(response.error);
        });

        connection.start().then(function () {
            document.getElementById('sendButton').onclick = function () {
                var message = document.getElementById("messageInput").value;
                connection.invoke("BroadcastFromClient", message)
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
            };
        });
    }
});