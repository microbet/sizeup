const request = require('request');
const queryString = require('query-string');

class GoogleMap {
	constructor(){
		this.params = {
			key: 'AIzaSyBYmAqm62QJXA2XRi1KkKVtWa6-BVTZ7WE',
			size: '600x300',
			maptype: 'roadmap',
			markers: [],
		};
	}

	_getLink(){
		const strigified = queryString.stringify(this.params);
		return `https://maps.googleapis.com/maps/api/staticmap?${stringified}`;
	}
	
	addMarker({color, label, centroidLat, centroidLng}){
		this.params.markers.push(`markers=color:${color}|label:${label}|${centroidLat},${centroidLng}`);
		return this; // is this necessary?
	}

	getBase64(){
		return new Promise((resolve, reject) => {
			const mapURL = this._getLink();
			reqest.get(mapURL, function(error, response, body) {
				if (!error && response.statusCode == 200) {
					resolve(
						"data:" +
						response.headers["content-type"] +
						";base64," +
						new Buffer(body).toString("base64")
					);
				} else {
					reject({ error: "Map loading error" });
				}
			});
		});
	}
}

module.exports = GoogleMap;


