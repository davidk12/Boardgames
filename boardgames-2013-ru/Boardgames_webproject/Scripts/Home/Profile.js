
$(document).ready(function ()
{

    $(".Achievement-Description-Div").corner();
    $(".Achievement-List-Div").hover(function ()
    {
        $(".Achievement-Description-Div", this).show();
    }, function ()
    {
        $(".Achievement-Description-Div", this).hide();
    });

});

