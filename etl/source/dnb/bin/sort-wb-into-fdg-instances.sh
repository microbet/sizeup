# TODO:
# dry-run mode!
# read DNB_PASSWORD from invoker.
# Take $data_ver from argument.
# https://stackoverflow.com/questions/6133517/parse-config-files-environment-and-command-line-arguments-to-get-a-single-col
# Actually all this should just be written in a legible programming language.
# Log well in extraction script; especially checksums for verifying results; maybe performance metrics for Kashyap.
# Get S3 password or decrypt .aws/credentials, gzip em all, put on s3

data_ver=$1
if [ -z "$data_ver" ]; then
  echo "Usage: $0 DATA_VERSION"
  exit 1
fi

cmd_dir=`dirname $0`

#check for mount before running these four lines:
instance_id=`ec2metadata --instance-id`
python $cmd_dir/setup-aws-volume.py --instance-id=$instance_id
sudo mkfs -t ext4 /dev/xvdf
sudo mount /dev/xvdf /data/dnb/
sudo chmod 777 /data/dnb/

cd $cmd_dir
cmd_dir=`pwd`
cd /data/dnb

echo -n "Enter DnB password: "
read DNB_PASSWORD
for part in `seq -w 26`; do
  zipfile=SIZEUP.WB$part.$data_ver.TXT.zip
  wget "ftp://sizeup:$DNB_PASSWORD@ftp.dnb.com/gets/$zipfile" -a wget.log -nv
  unzip -p $zipfile SIZEUP.\*.WB$part.$data_ver.TXT | python $cmd_dir/sort-wb-into-fdg-instances.py $data_ver
  rm $zipfile
done

for i in dnb.$data_ver.*.txt; do
  gzip $i;
  aws s3 mv --region us-east-1 $i.gz s3://sizeup-datasources/dnb/$data_ver/$i.gz;
done

# TODO umount; detach; delete. Do this manually until permissions are set
# up to prevent this script from damaging any other EC2 resource.

# Output files were about 115 GB before compression. Add another 9 GB for the raw file from dnb.com,
# and pad it a bit, and that's the size of the volume to create.

