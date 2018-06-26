# SizeUp

Explore key performance indicators specific to any industry in your chosen area. Identify possible business customers as well as the competition. The chosen area can be a county, a city, even a zip code.  _Etc etc etc. Screenshot, maybe. TODO(Mario): flesh this out._

See use examples below, or read the full docs at [Sizeup developer docs].

## Example: SizeUp from browser code

To run SizeUp entirely in a browser application, download the script directly from our servers using your product key. For example, here we look up the average revenue of Shoe Repair businesses in Chicago and its county.

#### Installation

    <script src="https://api.sizeup.com/js/?callback=onLoadSizeup&apikey=YOUR_PRODUCT_KEY_HERE"></script>

#### Use

    <script type="text/javascript">
      let report = {};

      ////// Look up industry and place objects //////
      function onLoadSizeup() {
        sizeup.data.findIndustry(
          { term: "shoe repair" },
          function(industries) { report.industry = industries[0]; onLoadIndustries(); },
          console.error);
      }
      function onLoadIndustries() {
        sizeup.data.findPlace(
          { term: "chicago" },
          function(places) { report.place = places[0]; onLoadPlaces(); },
          console.error);
      }
      function onLoadPlaces() {

        ////// Get citywide data //////
        sizeup.data.getAverageRevenue(
          { geographicLocationId: report.place.City.Id, industryId: report.industry.Id },
          function(data) { alert(
            "Average revenue of " + report.industry.Name
            + " businesses in " + report.place.City.Name
            + ": $" + data.Value);
          },
          console.error);

        ////// Get countywide data //////
        sizeup.data.getAverageRevenue(
          { geographicLocationId: report.place.County.Id, industryId: report.industry.Id },
          function(data) { alert(
            "Average revenue of " + report.industry.Name
            + " businesses in " + report.place.County.Name
            + " County: $" + data.Value);
          },
          console.error);
      }
    </script>

Outputs:

    Average revenue of Shoe & Boot Repairing businesses in Chicago: $109571
    Average revenue of Shoe & Boot Repairing businesses in Cook County: $96127

## Example: SizeUp from Node.js

The `sizeup-api` npm module allows both callback coding (as in the above example) or ES6 Promise coding, as shown below. Here we look up the revenue per capita of Coffee Shops in Oakland and its county.

#### Installation

    npm install sizeup-api

#### Use

    const sizeup = require("sizeup-api")({ key:YOUR_PRODUCT_KEY_HERE });

    ////// Look up industry and place objects //////
    Promise.all([
      sizeup.data.getIndustryBySeokey("coffee-shops"),
      sizeup.data.getPlaceBySeokey("california/alameda/oakland-city"),
    ]).then(([industry, place]) => {
      Promise.all([
        ////// Get citywide data //////
        sizeup.data.getRevenuePerCapita({geographicLocationId: place.City.Id, industryId: industry.Id}),
        ////// Get countywide data //////
        sizeup.data.getRevenuePerCapita({geographicLocationId: place.County.Id, industryId: industry.Id}),
      ]).then(([city_data, county_data]) => {
        console.log("Revenue per capita of",industry.Name,"businesses in",place.City.Name,": ",city_data.Value);
        console.log("Revenue per capita of",industry.Name,"businesses in",place.County.Name,"County : ",county_data.Value);
      })
    }).catch(console.error);

Outputs:

    Revenue per capita of Coffee Shops businesses in Oakland: $86
    Revenue per capita of Coffee Shops businesses in Alameda County: $114

## Other languages

The Node binary allows you to use Sizeup software from languages other than Node, by launching shell commands. For example:

#### Installation

    npm install -g sizeup-api

#### Use

    $ export SIZEUP_KEY=YOUR_PRODUCT_KEY_HERE
    $ bin/sizeup.js findPlace '{"term": "austin, tx"}'
    ...
      "DisplayName": "Austin, TX (Travis County - City)",
      "City": {
        "Id": 104971,
    ...
    $ bin/sizeup.js findIndustry '{"term": "burger"}'
    [
      {
        "Id": 8524,
        "Name": "Burger Restaurants",
    ...
    $ bin/sizeup.js getAverageRevenue '{"geographicLocationId": 104971, "industryId": 8524}'
    {
      "Value": 1537200,
      "Median": 1481000,
      "Name": "Austin, TX"
    }

Read more at [Sizeup developer docs].

[Sizeup developer docs]: https://www.sizeup.com/developers/documentation
