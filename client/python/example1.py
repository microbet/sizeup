import json
import logging
import os
import subprocess

# Specify our API key and the path to sizeup executable.
os.environ["SIZEUP_KEY"] = "6388E63C-3D44-472B-A424-712395B1AD51"
sizeup = "node_modules/sizeupjs/bin/sizeup.js"

# The user's business revenue.
my_revenue = 12345000

# Get data object for Boulder:
command = [sizeup, "findPlace", json.dumps({"term":"boulder"})]
boulder = json.loads(subprocess.check_output(command))[0]
logging.info("Found %s, %s", boulder["City"]["Name"], boulder["State"]["Name"])

# Get data object for Breweries industry:
command = [sizeup, "getIndustryBySeokey", json.dumps({"seokey":"brewers-manufacturers"})]
breweries = json.loads(subprocess.check_output(command))
logging.info("Found %s industry", breweries["Name"])

# Compare revenue to industry standards:
for place in (boulder["City"], boulder["State"], boulder["Nation"]):
  command = [sizeup, "getAverageRevenuePercentile", json.dumps({
    "value": my_revenue,
    "geographicLocationId": place["Id"],
    "industryId": breweries["Id"]})]
  result = json.loads(subprocess.check_output(command))
  print("You have greater revenue than %s percent of %s in %s."
    % (result["Percentile"], breweries["Name"], result["Name"]))
