//Cole
var express = require('express');
var app = express();
var numberOfPlayers = 0;

app.get('/', function (req, res) {
	numberOfPlayers++;
 	res.redirect("http://mhackslocal.colehudson.net/" + numberOfPlayers);
});

var server = app.listen(3000, function () 
{
	var host = server.address().address;
	var port = server.address().port;

	console.log('Example app listening at http://%s:%s', host, port);
});