//Cole

//Lets require/import the HTTP module
var http = require('http');

var numberOfPlayers = 0;

//Lets define a port we want to listen to
const PORT=8088; 

//We need a function which handles requests and send response
function handleRequest(request, response)
{
	numberOfPlayers++;
    response.redirect("http://mhacks.colehudson.net/" + numberOfPlayers);
}

//Create a server
var server = http.createServer(handleRequest);

//Lets start our server
server.listen(PORT, function()
{
    //Callback triggered when server is successfully listening. Hurray!
    console.log("Server listening on: http://localhost:%s", PORT);
});