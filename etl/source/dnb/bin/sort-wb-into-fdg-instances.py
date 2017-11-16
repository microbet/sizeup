#!/bin/python
# Input:
# wget ftp://sizeup:$DNB_PASSWORD@ftp.dnb.com/gets/SIZEUP.WB01.$data_version.TXT.zip # output to /data/dnb/
#
# Output:
# One DnB-formatted file per FDG instance

import datetime, logging, os, sys

data_version = "OCT17"

country_codes = {
  "US": [
    "805", # United States
  ],
  "EU": [
    "357", # ITALY
    "785", # ENGLAND
    "269", # GERMANY
    "597", # POLAND
    "241", # FRANCE
    "693", # SPAIN
    "069", # BELGIUM
    "521", # NETHERLANDS
    "771", # UKRAINE
    "601", # PORTUGAL
    "041", # AUSTRIA
    "721", # SWITZERLAND
    "197", # DENMARK
    "285", # GREECE
    "797", # SCOTLAND
    "349", # IRELAND
    "801", # WALES
    "793", # NORTHERN IRELAND
    "790", # UNITED KINGDOM
  ],
  "NA": [
    "121", # Canada
    "489", # Mexico
  ],
}

sample_sics = (
  "5812", "5813", "5411", "5921", "5461", "5499", "5421", "2099", "2051",
  "5182", "5147", "5149", "5431", "2082", "5148", "3585", "3556", "2086",
  "0161", "2033", "2013", "2011", "2841", "2024", "2026", "2091", "2095",
  "2015", "2098",
)

def get_fdg_instance_file(instance):
  filename = "dnb.%s.%s.txt" % (data_version, instance)
  file = open(filename, "a")
  return file

instance_files = {
  "US":         get_fdg_instance_file("US"),
  "Austin":     get_fdg_instance_file("Austin"),
  "NA":         get_fdg_instance_file("NA"),
  "EU":         get_fdg_instance_file("EU"),
  "Manchester": get_fdg_instance_file("Manchester"),
}

logging.basicConfig(
  level=logging.INFO,
  filename="sort.log",
  filemode="a"
)

logging.info("Started at %s" % str(datetime.datetime.now()))
row_count = 0

with sys.stdin as dnb_file:
  done = False
  while not done:
    row = dnb_file.readline()
    if row:
      # logging.debug("Row: %s %s" % (row[416:419], row[834:838]))
      row_count += 1
      row_country = row[416:419]

      if row_country == country_codes["US"][0]:
        instance_files["US"].write(row)
        zip_code_start = row[419:422]
        if zip_code_start == "787" and row[834:838] in sample_sics:
          instance_files["Austin"].write(row)

      elif row_country in country_codes["NA"]:
        instance_files["NA"].write(row)

      elif row_country in country_codes["EU"]:
        instance_files["EU"].write(row)
        if (
          row_country == "785" # England
          and row[400:406] == "000515" # Manchester
          and row[834:838] in sample_sics
        ):
          instance_files["Manchester"].write(row)

    else:
      done = True

for i in instance_files: instance_files[i].close()

logging.info("Processed %s rows" % row_count)
logging.info("Finished at %s" % str(datetime.datetime.now()))
