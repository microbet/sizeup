# Prototype; do not automate.
# TODO:
# ensure running in /data/buergel
# parameterize data_version

data_ver=$1
if [ -z "$data_ver" ]; then
  echo "Usage: $0 DATA_VERSION"
  echo "Buergel does not supply a version stamp. Use YYYY.MM"
  exit 1
fi

cat <<EOF | sftp ftsiz01p@sftp.buergel-online.de
get outbox/Buergel_Company_Profile_Part1.csv
get outbox/Buergel_Company_Profile_Part2.csv
get outbox/Buergel_Company_Profile_Part3.csv
get outbox/Buergel_Company_Profile_Part4.csv
EOF

# TODO cat *.csv | python sort-buergel-into-fdg-instances.py

for i in *.csv; do gzip $i; aws s3 mv --region us-east-1 $i.gz s3://sizeup-datasources/buergel/${data_version}/$i.gz; done

