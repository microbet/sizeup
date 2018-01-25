# First draft; needs work.
# Don't forget to activate a virtualenv first. Primary should work:
# . /etc/sizeup/virtualenv/primary/bin/activate

import sys, logging
if not hasattr(sys, "real_prefix"):
  logging.warn("Virtualenv was NOT detected. If this script fails, activate one. e.g.:\n. /etc/sizeup/virtualenv/primary/bin/activate")
  
import boto3
ec2 = boto3.resource("ec2")
client = boto3.client("ec2")
volume = ec2.create_volume(
  AvailabilityZone="us-east-1a",
  Size=150,
  VolumeType="gp2",  # TODO change to io1
  TagSpecifications=[{
    "ResourceType": "volume",
    "Tags": [{"Key": "Name", "Value": "/data/dnb"}]
  }],
)
waiter = client.get_waiter("volume_available")
waiter.wait(VolumeIds=[volume.id])
attachment = volume.attach_to_instance(
  Device     = "/dev/xvdf",  # TODO discover this...?
  InstanceId = my_instance_id,
)
waiter = client.get_waiter("volume_in_use")
waiter.wait(VolumeIds=[volume.id])
