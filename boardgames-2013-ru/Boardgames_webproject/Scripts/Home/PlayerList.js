$(document).ready(function ()
{
 


    $("#Player-List-Table").tablesorter();
    $("#Player-List-Table").tablesorter({ sortList: [[0, 0], [1, 0]] });

    

    $('.Player-TR').hover(function ()
    {
        $(this).css("background-color", "gold");


    },
    function ()
    {
        $(this).css("background-color", "yellow");
        $("#Selected-Table-Row").css("background-color", "gold");
    });

    $('#Entire-Partial-View').hover(function ()
    {

    },
    function ()
    {
        $("#Highest-Rated-Box img").attr("src", "/Images/usernotfound.png");
        $("#Highest-Rated-Box p").text("");
        $("#Selected-Table-Row").css("background-color", "yellow");
        $("#Selected-Table-Row").removeAttr('id');
    });



    $('.Player-TR').click(function ()
    {
        
        if ($(this).find("img").eq(0).attr("src").indexOf("notfound") === -1)                       // checking whether the image string contains the placeholder image
        {
            $("#Highest-Rated-Box p").text($(this).find(".Hidden-Username:first").text());              
            $("#Highest-Rated-Box img").attr("src", $(this).find(".Hidden-Link:first").text());
        }
        else
        {
            $("#Highest-Rated-Box p").text($(this).find(".Hidden-Username:first").text());          //   
            $("#Highest-Rated-Box img").attr("src", $(this).find("img").eq(0).attr("src"));

        }


        $("#Selected-Table-Row").css("background-color", "yellow");
        $("#Selected-Table-Row").removeAttr('id');
        $(this).attr("id", "Selected-Table-Row");
        $("#Selected-Table-Row").css("background-color", "gold");

        $('#Join-Game-Button').attr('onclick', "document.location.href='" + $(this).find(".Active-Game-Links:first").attr("href") + "';");


    });
});

