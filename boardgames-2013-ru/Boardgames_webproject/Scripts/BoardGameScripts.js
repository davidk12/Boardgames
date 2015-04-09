$(document).ready(function ()
{
    
    //var lAudioNote = document.getElementById("rave");
    
    /*
    $("#rave-button").hover(function ()
    {
        $(this).show();
    }, function ()
    {
        $(this).hide();
    });
    */
    $("#rave-button").click(function ()
    {
       
        if ($(this).hasClass("rave-off") == true)
        {
            //alert("Lol");
            //lAudioNote.play();
            $(this).attr("value", "Rave Mode Off");
            $(this).removeClass("rave-off");
            $(this).addClass("rave-on");

            $("body").css("background-image", "url('/Images/flash.gif')");

        }
        else if ($(this).hasClass("rave-on") == true)
        {
           
            //lAudioNote.pause();
            //document.getElementById('rave').currentTime = 0;
            $(this).removeClass("rave-on");
            $(this).attr("value", "Rave Mode On");
            $(this).addClass("rave-off");
            $("body").css("background-image", "url('/Images/body-background.png')");
        }
    });

    

    //<audio id="rave" src= '@Url.Content("~/Content/Music/bloodrave.mp3")' preload="auto"></audio>
});



