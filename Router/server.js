//cole
var express = require('express');
var app = express();
var numberOfPlayers = 0;

//redirect to the local server
app.get('/', function(req, res) {
  res.redirect("http://mhackslocal.colehudson.net/" + numberOfPlayers);
});