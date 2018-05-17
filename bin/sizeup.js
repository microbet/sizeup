#!/usr/bin/env node

var exitSaying = function (s) {
    console.error(s);
    process.exit(1)
}

if (!process.env.SIZEUP_KEY)  exitSaying("ERROR: Need $SIZEUP_KEY to authenticate");
require('../src/api')(process.env.SIZEUP_KEY);  // installs sizeup.* globally; TODO reconsider

var exitWithUsage = function (s) {exitSaying((s?(s+"\n"):"") + "USAGE: sizeup <command> <json>")}

var command = process.argv[2];
if (!command)  exitWithUsage();
var commandFn = sizeup.api.data[command];
if (!commandFn)  exitSaying("ERROR: Unknown command.\n\nCommands:\n"+Object.keys(sizeup.api.data).join(', '));

var json = process.argv[3]; // TODO || stdin;
if (!json)  exitWithUsage();
try {
    var params = JSON.parse(json);
} catch (e) {
    exitWithUsage("Invalid JSON\n"+e);
}

commandFn( params,
    function(result) { console.log(JSON.stringify(result,0,2)); },
    function(exc) { exitSaying(exc); }
);
