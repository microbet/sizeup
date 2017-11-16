import csv, logging, os, sqlite3, sys

#logging.basicConfig(level=logging.DEBUG)

data_version = "201710"  # TODO parameterize

sample_sics = (
  "5812", "5813", "5411", "5921", "5461", "5499", "5421", "2099", "2051",
  "5182", "5147", "5149", "5431", "2082", "5148", "3585", "3556", "2086",
  "0161", "2033", "2013", "2011", "2841", "2024", "2026", "2091", "2095",
  "2015", "2098",
)

def get_fdg_instance_file(instance):
  filename = "infogroup.%s.%s.csv" % (data_version, instance)
  file = open(filename, "a")
  return csv.writer(file, quoting=csv.QUOTE_ALL)

instance_files = {
  "Austin":     get_fdg_instance_file("Austin"),
}

with sys.stdin as infogroup_file:
  infogroup_reader = csv.reader(infogroup_file)
  for row in infogroup_reader:
    (name, zip, sic) = (row[0], row[4], row[16])
    logging.debug("%s %s %s" % (name, zip, sic))
    if zip.startswith("787") and sic[0:4] in sample_sics:
      logging.debug("... using for small sample ...")
      instance_files["Austin"].writerow(row)

#for i in instance_files: instance_files[i].close()
