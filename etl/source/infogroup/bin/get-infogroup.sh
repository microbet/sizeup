#!/bin/bash

# TODO:
# ensure running in /data/infogroup

data_version=$1
if [ -z "$data_version" ]; then
  echo "Usage: $0 DATA_VERSION"
  exit 1
fi

cd /data/infogroup

cat <<EOF | sftp SIZUP1RT@sftp.igxfer.com
get SIZEUP_${data_version}1.ZIP
get SIZEUP_${data_version}2.ZIP
get SIZEUP_${data_version}3.ZIP
get SIZEUP_${data_version}4.ZIP
get SIZEUP_${data_version}5.ZIP
get cbsa.ZIP
get codedall.ZIP
get countyp.ZIP
get fran3new.ZIP
get industry.ZIP
get naicsconv.ZIP
EOF

for i in *.ZIP; do unzip $i; aws s3 mv --region us-east-1 $i s3://sizeup-datasources/infogroup/${data_version}/$i; done

