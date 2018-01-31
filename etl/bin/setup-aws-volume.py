#!/usr/bin/env python
# First draft; needs work.

import sys, logging
logging.basicConfig(level=logging.INFO)
if not hasattr(sys, "real_prefix"):
  logging.warn("Virtualenv was NOT detected. If this script fails, activate one. e.g.:\n. /etc/sizeup/virtualenv/primary/bin/activate")

import argparse
import boto3

parser = argparse.ArgumentParser()
parser.add_argument("--name", required=True)
parser.add_argument("--size", type=int, required=True)
parser.add_argument("--instance-id", required=True)
parser.add_argument("--zone", default="us-east-1a")
parser.add_argument("--device", required=True)
args = parser.parse_args()

ec2 = boto3.resource("ec2")
client = boto3.client("ec2")
logging.info("Creating %sGiB volume \"%s\" in %s..." % (
  args.size, args.name, args.zone))
volume = ec2.create_volume(
  AvailabilityZone=args.zone,
  Size=args.size,
  VolumeType="gp2",  # TODO change to io1
  TagSpecifications=[{
    "ResourceType": "volume",
    "Tags": [
      {"Key": "Name", "Value": args.name},
      {"Key": "production", "Value": "false"},
    ]
  }],
)
waiter = client.get_waiter("volume_available")
waiter.wait(VolumeIds=[volume.id])

logging.info("Attaching volume to %s on %s..." % (
  args.instance_id, args.device))
attachment = volume.attach_to_instance(
  Device     = args.device,
  InstanceId = args.instance_id,
)
waiter = client.get_waiter("volume_in_use")
waiter.wait(VolumeIds=[volume.id])
# TODO I don't think this waits long enough. See hack in ../Makefile

print "To clean up:\numount %s\nboto3.resource(\"ec2\").Instance(\"%s\").detach_volume(VolumeId=\"%s\")" % (args.device, args.instance_id, volume.id)
