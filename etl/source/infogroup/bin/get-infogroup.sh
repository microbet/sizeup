# TODO:
# ensure running in /data/infogroup
# parameterize data_version

data_version=201710

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

cat SIZEUP_${data_version}?.TXT | python sort-infogroup-into-fdg-instances.py

aws s3 mv --region us-east-1 infogroup.${data_version}.Austin.csv s3://sizeup-datasources/infogroup/${data_version}/

