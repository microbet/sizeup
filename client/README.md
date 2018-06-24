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


## Data API

#### In the browser

Instructions are presently at https://www.sizeup.com/developers/documentation but we'll move them here.

#### Node.js

Example.

    const sizeup = require("sizeup");
    sizeup.authenticate(YOUR_KEY);
    data = sizeup.api.data;
    Promise.all([
      data.getIndustryBySeokey("shoe-and-boot-repairing"),
      data.getPlaceBySeokey("california/alameda/oakland-city"),
    ]).then(([industry, place, user]) => Promise.all([
      data.getAverageRevenue({geographicLocationId: place.City.Id, industryId: industry.Id}),
    ])).then(console.log);

#### Other languages

The Node binary allows you to use Sizeup APIs from languages other than Node.

    bin/sizeup.js findPlace '{"term":"oakland, ca"}'


## Place Names

A place is canonically specified by a hierarchy of enclosing geographical areas. In the United States, a place is uniquely identified if it has "state", "county", and "city" fields. For example, Austin TX is identified as

    {
      "state": "texas",
      "county": "travis",
      "city": "austin-city"
    }

The string form `austin-city,travis,texas` is equivalent.

Note that some cities (like Austin) are spread across multiple counties. In some reports, SizeUp shows both citywide and countywide results. For `austin-city,travis,texas` and `austin-city,hays,texas` the citywide results would be identical, but the countywide results would be different, since they refer to different counties.