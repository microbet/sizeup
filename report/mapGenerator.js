const request = require('request');

/*
* paramObj 
* url = google map url, size, maptype (roadmap prob), markerstr (generates markers from google api), key
*/

function getStaticMap(optionsObj) {
    return new Promise((resolve, reject) => {
      console.log('hop');
	  const mapURL = optionsObj.url + '?size=' + optionsObj.size + '&maptype=' + optionsObj.maptype + '&markers=' + optionsObj.markerStr + '&key=' + optionsObj.key;
      console.log('debug', mapURL);
	  request.get(mapURL, function(error, response, body) {
        console.log('this');
		if (!error && response.statusCode == 200) {
          console.log('ish');
		  resolve(
            "data:" +
              response.headers["content-type"] +
              ";base64," +
              new Buffer(body) // .toString("base64")
          );
        } else {
		  console.log('okey');
          reject({ error: "Map loading error" });
        }
		console.log('now', error);
      });
    });
}
//	let base64 = request.get(paramObj.url
// var download = function(uri, filename, callback){
//    request.head(uri, function(err, res, body){
//      request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
//    });
//  };

module.exports = {
	getStaticMap: getStaticMap,
}
	