
$(document).ready(function ()
{
    $("#Join-Game-Button").attr("disabled", "disabled");            // disabling the join game button by default
    $("#Join-Game-Button").attr("value", "");                       // settings its value to 


    $(".Achievement-Description-Div").corner();

    //----------------------------------------------------//
    $(".Achievement-List-Div").hover(function ()
    {
        $(".Achievement-Description-Div", this).show();
    },
    function ()
    {
        $(".Achievement-Description-Div", this).hide();
    });
    //----------------------------------------------------//

    /*
    $(function ()
    {
        var username = usrName;
        // Reference the auto-generated proxy for the hub.  
        var chat = $.connection.chatHub;
        // Create a function that the hub can call back to display messages.
        chat.client.addNewMessageToPage = function (name, message)
        {
            // Add the message to the page. 
            $('#Lobby-Chat-Window').append('<p class = "Chat-Message"><strong class = "Chat-Message-Username">' + htmlEncode(name)
                + '</strong>: ' + htmlEncode(message) + '</p>');
        };
        // Get the user name and store it to prepend to messages.
        // Set initial focus to message input box.  
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function ()
        {
            $('#sendmessage').click(function ()
            {
                if ($('#message').val() == "")
                {

                }
                else
                {
                    chat.server.send(username, $('#message').val());
                    // Clear text box and reset focus for next comment. 
                    $('#message').val('').focus();
                }
                // Call the Send method on the hub. 

            });


        });
    });

    */



    ////////////////////////////
    /*
    ///var hub = $.connection.ticTacToeHub;
    var chat_hub = $.connection.chatHub;


    chat_hub.client.addNewMessageToPage = function (username, message)
    {
        console.log(message);
        $($('#Lobby-Chat-Window')).append('<p class = "Chat-Message"><strong class = "Chat-Message-Username">' + username
     + '</strong>: ' + message + '</p>');

    };
    //

    $.connection.chat_hub.start().done(function ()
    {
        alert("lol");   
        //hub.server.join(group);
        chat_hub.server.join(group);

        //Send chat
        $('#sendmessage').click(function ()
        {
            
            // Call the Send method on the hub.
            // $('#Lobby-Chat-Window').val()

                   
            console.log('Calling Sending message', group, username);

            $($('#Chat-Box-Field')).append('<p class = "Chat-Message"><strong class = "Chat-Message-Username">' + username
            + '</strong>: ' + $('#message').val() + '</p>');

            $.connection.chatHub.server.send(group, username, $('#Lobby-Chat-Window').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();

            //$("#Lobby-Chat-Div").attr({ scrollTop: $("#Lobby-Chat-Div").attr("scrollHeight") });
        });

    /////////////////////////////

    */



    // This optional function html-encodes messages for display in the page.
    function htmlEncode(value)
    {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }





    function populate_table(list_model)
    {
        var length = list_model.user_list.length;
       
        $('#Player-List-Table').append('<tr>'  
                                            + '<th id="User-Avatar-th"></th>'
                                            + '<th id="Username-th">Name</th>'
                                            + '<th id="User-Rating-th">Rating</th>'
                                            + '<th id="Profile-Link-th"></th>'
                                    + '</tr>');

        
        for (var count = 0; count < length; count++)
        {

            $('#Player-List-Table tr:last').after('<tr>'
                + '<td class ="Username-Avatar-td"><img class = "Username-Avatar-In-List"  src="/Images/Users/' + list_model.user_list[count].username + '.png"/></td>'
                + '<td>' + list_model.user_list[count].username + '</td>'
                + '<td>' + list_model.user_list[count].rating + '</td>'
                + '<td></td>'
                + '</tr>');
        }
    }

























});
