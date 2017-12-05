instance_id=$(shell ec2metadata --instance-id)

/data/dnb.TEST:
	bin/setup-aws-volume.py --name="/data/dnb.TEST" --size=156 --instance-id=$(instance_id)
	sleep 5  # /dev/xvdf needs a few seconds before it shows up ...?
	sudo mkfs -t ext4 /dev/xvdf
	sudo mount /dev/xvdf /data/dnb/
	sudo chmod 777 /data/dnb/

# TODO what's a good way to stash the info we need to tear down the mount?
