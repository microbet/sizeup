#!/usr/bin/env python

import sys, logging
logging.basicConfig(level=logging.INFO)
if not hasattr(sys, "real_prefix"):
  logging.warn("Virtualenv was NOT detected. If this script fails, activate one. e.g.:\n. /etc/sizeup/virtualenv/primary/bin/activate")

import argparse
import boto3

parser = argparse.ArgumentParser()
parser.add_argument("--volume-id", required=True)
args = parser.parse_args()

ec2 = boto3.resource("ec2")
client = boto3.client("ec2")

boto3.resource("ec2").Volume(args.volume_id).detach_from_instance();
waiter = client.get_waiter("volume_available")
waiter.wait(VolumeIds=[args.volume_id])
boto3.resource("ec2").Volume(args.volume_id).delete();
waiter = client.get_waiter("volume_deleted")
waiter.wait(VolumeIds=[args.volume_id])


