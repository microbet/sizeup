# First draft; needs work.

import boto3
ec2 = boto3.resource("ec2")
volume = ec2.create_volume(
  AvailabilityZone="us-east-1a",
  Size=150,
  VolumeType="gp2",  # TODO change to io1
)
waiter = client.get_waiter("volume_available")
waiter.wait(VolumeIds=[volume.id])
attachment = volume.attach_to_instance(
  Device     = "/dev/xvdf",  # TODO discover this...?
  InstanceId = my_instance_id,
)
waiter = client.get_waiter("volume_in_use")
waiter.wait(VolumeIds=[volume.id])
