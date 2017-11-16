# TODO:
# ensure running in /data/dnb . and/or make wget dump to /data/dnb .
# read DNB_PASSWORD from invoker.
# parameterize data_version (hardcoded here as "OCT17").
# Log well in extraction script; especially checksums for verifying results; maybe performance metrics for Kashyap.
# Get S3 password or decrypt .aws/credentials, gzip em all, put on s3
# https://stackoverflow.com/questions/6133517/parse-config-files-environment-and-command-line-arguments-to-get-a-single-col

python setup-aws-volume.py

sudo mkfs -t ext4 /dev/xvdf
sudo mount /dev/xvdf /data/dnb/
sudo chmod 777 /data/dnb/

echo -n "Enter DnB password: "
read DNB_PASSWORD
for part in `seq -w 26`; do
  zipfile=SIZEUP.WB$part.OCT17.TXT.zip
  wget "ftp://sizeup:$DNB_PASSWORD@ftp.dnb.com/gets/$zipfile" -a wget.log -nv
  unzip -p $zipfile SIZEUP.\*.WB$part.OCT17.TXT | python sort-wb-into-fdg-instances.py
  rm $zipfile
done

for i in dnb.OCT17.*.txt; do
  gzip $i;
  aws s3 mv --region us-east-1 $i.gz s3://sizeup-datasources/dnb/OCT17/$i.gz;
done

# TODO umount; detach; delete. Do this manually until permissions are set
# up to prevent this script from damaging any other EC2 resource.

# Output files were about 115 GB before compression. Add another 9 GB for the raw file from dnb.com,
# and pad it a bit, and that's the size of the volume to create.

