# APIs


## Authentication

Use your [customer API key] (represented below as `YOUR_KEY`) to authenticate to all APIs.

Each customer receives two API keys, which behave differently:

The **production key** allows unlimited API calls. We enforce access restrictions on this key: for example, to create a widget in your HTML document, the domain of your document must be on our whitelist. This prevents use of the key from unauthorized domains, and that makes it a good key to use at your production website.

The **development key** can only be used for a small number of API calls, but we don't enforce access restrictions. This lets you use the key legitimately when developing at domains that are not on our whitelist. We reserve the right to revoke this key at any time, for example if we detect unauthorized use.

Some applications provide the user with an individual user identity. This is different from your customer API key. User identities are not recognized by these APIs; always use customer API key.


## Widget API

#### SizeUp 1

Create a DOM element in your HTML document where you want the SizeUp widget to appear. Add this HTML to the element:

    <span><a href="https://www.sizeup.com/" target="_blank">SizeUp</a></span>
    <script type="text/javascript" src="https://application.sizeup.com/widget/v1/get?key=YOUR_KEY"></script>

#### SizeUp 2

In your HTML `<head>` element, include our script:

    <script type="text/javascript" src="https://application.sizeup.com/widget/v2.0/get"></script>

Authenticate yourself:

    sizeup.authenticate(YOUR_KEY);

This can throw _(TODO define auth exceptions)_.

In the HTML `<body>`, create a DOM element with an ID in your HTML document where you want the SizeUp widget to appear. During page initialization, or any time you want to install the SizeUp widget into your element, execute:

    sizeup.widget.create(ELEMENT_ID, ...);

You can specify these additional arguments for this instance of the widget:
- `key` - your API key (required if you didn't authenticate already)
- `industry` - initial industry (see [Industry Names])
- `place` - initial place (see [Place Names])
- `page` - which SizeUp tool to display initially (possible values: `"business"`, `"customers"`, `"marketing"`). If not specified, the widget may select the best tool based on what it knows about the user.
- `resizePermission` - `true` if the widget element is allowed to resize itself depending on its content and/or the window dimensions (this is the default). `false` if you want the element to retain its initial width and height, or if you want to control them externally.

Other customized attributes, such as the widget's content and visual appearance, apply to _all_ widgets created for a particular API key. Those cannot be changed at runtime.

This function can throw (TODO exceptions for permission denied, auth failure, element not found, unrecognized arguments).


# Data API

The `sizeup.data` object implements the data API, which has a number of specific functions to access specific analytic data. (`sizeup.api.data` refers to the same object but is considered obsolete.)

Each data function has the same basic signature: a dictionary of query parameters, a success callback, and an error callback. For Ecmascript 6 consumers: If you omit the callback arguments, the data function will return a Promise object. For example:

    sizeup.data.findPlace(
      { term: 'san franc', maxResults: 10 },
      function(results) { YOUR_RESULT_HANDLER(results); },
      function(error) { YOUR_ERROR_HANDLER(error); }
    );

or the equivalent:

    sizeup.data.findPlace(
      { term: 'san franc', maxResults: 10 }
    ).then(
      YOUR_RESULT_HANDLER
    ).catch(
      YOUR_ERROR_HANDLER
    );

Data functions are listed below, but first, a couple of full examples.

## Example: SizeUp from browser code

To run SizeUp entirely in a browser application, download the script directly from our servers using your product key. For example, here we look up the average revenue of Shoe Repair businesses in Chicago and its county.

#### Installation

    <script src="https://api.sizeup.com/js/?callback=onLoadSizeup&apikey=YOUR_PRODUCT_KEY_HERE"></script>
    
#### Use

    <script type="text/javascript">
      let report = {};
      
      /*** Look up industry and place objects ***/
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
      
        /*** Get citywide data ***/
        sizeup.data.getAverageRevenue(
          { geographicLocationId: report.place.City.Id, industryId: report.industry.Id },
          function(data) { alert(
            "Average revenue of " + report.industry.Name
            + " businesses in " + report.place.City.Name
            + ": $" + data.Value);
          },
          console.error);
          
        /*** Get countywide data ***/
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

    const sizeup = require("sizeup-api")(YOUR_PRODUCT_KEY_HERE);
    
    /*** Look up industry and place objects ***/
    Promise.all([
      sizeup.data.getIndustryBySeokey("coffee-shops"),
      sizeup.data.getPlaceBySeokey("california/alameda/oakland-city"),
    ]).then(([industry, place]) => {
      Promise.all([
        /*** Get citywide data ***/
        sizeup.data.getRevenuePerCapita({geographicLocationId: place.City.Id, industryId: industry.Id}),
        /*** Get countywide data ***/
        sizeup.data.getRevenuePerCapita({geographicLocationId: place.County.Id, industryId: industry.Id}),
      ]).then(([city_data, county_data]) => {
        console.log(util.format(
          "Revenue per capita of %s businesses in %s: $%s",
          industry.Name, place.City.Name, city_data.Value));
        console.log(util.format(
          "Revenue per capita of %s businesses in %s County: $%s",
          industry.Name, place.County.Name, county_data.Value));
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


## Place Names

A place is canonically specified by a hierarchy of enclosing geographical areas. In the United States, a place is uniquely identified if it has "state", "county", and "city" fields. For example, Austin TX is identified as

    {
      "state": "texas",
      "county": "travis",
      "city": "austin-city"
    }

The string form `austin-city,travis,texas` is equivalent.

Note that some cities (like Austin) are spread across multiple counties. In some reports, SizeUp shows both citywide and countywide results. For `austin-city,travis,texas` and `austin-city,hays,texas` the citywide results would be identical, but the countywide results would be different, since they refer to different counties.
