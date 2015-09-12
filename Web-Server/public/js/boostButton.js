//Cole
var fps = 1;

function BoostButton(element) 
{

    var self = this;
    this.m_element = element;

    // message handlers..
    this.onStartNewGame = function() 
    {

    };

    $(element).on("touchend", function () {
        self.handleClick();
    });

    this.handleClick = function () 
    {
        var msg = {
            'type': "boost"
        };
        conn.sendMessage(msg, 0);
    };

    this.redraw = function() 
    {
        var color = 0;
        var text = "";
        color = "black";
        text = "BOOST"

        $(self.m_element).css("background", color);
        $(self.m_element).children(".text").html(text);
    }
}