
$(document).ready(function ()
{       
    $("#Join-Game-Button").attr("disabled", "disabled");
    $("#Join-Game-Button").attr("value", "");


    $(".Achievement-Description-Div").corner();

    //----------------------------------------------------//
    $(".Achievement-List-Div").hover(function ()
    {
        $(".Achievement-Description-Div",this).show();
    },
    function ()
    {
        $(".Achievement-Description-Div", this).hide();
    });
    //----------------------------------------------------//

    
    $('.Active-Game-TR').hover(function ()
    {
        $(this).css("background-color", "gold");

      
    },
    function ()
    {
        $(this).css("background-color", "yellow");
        $("#Selected-Table-Row").css("background-color", "gold");
    });



    $('#Bottom-Left-Box').hover(function ()
    {
    },
    function ()
    {
        $("#Selected-Table-Row").css("background-color", "yellow");
        $("#Selected-Table-Row").removeAttr('id');
        
        $("#Join-Game-Button").attr("disabled", "disabled");
        $("#Join-Game-Button").attr("value", "");
    });
    

    

    
    $('.Active-Game-TR').click(function ()
    {
        if($(this).find("td").hasClass("Red-Game-Status") == false)
        {
            $("#Join-Game-Button").attr("value", "Join Game");
            $("#Selected-Table-Row").css("background-color", "yellow");
            $("#Selected-Table-Row").removeAttr('id');
            $(this).attr("id", "Selected-Table-Row");
            $("#Join-Game-Button").removeAttr("disabled");
            $("#Join-Game-Button").css("background", "-moz-linear-gradient(top, #fceabb 0%, #fccd4d 50%, #f8b500 51%, #fbdf93 100%)");
            $('#Join-Game-Button').attr('onclick', "document.location.href='" + $(this).find(".Active-Game-Links:first").attr("href") + "';");
        }
        

    });


    
    //----------------------------------------------------//
});

