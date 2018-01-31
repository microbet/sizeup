instance_id=$(shell ec2metadata --instance-id)

/data/dnb.volume:
	../../bin/setup-aws-volume.py --name="/data/dnb" --size=175 --device=/dev/xvdf --instance-id=$(instance_id)
	sleep 5  # /dev/xvdf needs a few seconds before it shows up ...?
	sudo mkfs -t ext4 /dev/xvdf
	sudo mount /dev/xvdf /data/dnb/
	sudo chmod 777 /data/dnb/
	echo "umount /data/dnb" > $@

# TODO what's a good way to stash the info we need to tear down the mount?

dnb-data-files: /data/dnb.volume
ifndef data_version
	$(error Must specify $$data_version)
endif
	bin/sort-wb-into-fdg-instances.sh $(data_version)
	# Please manually clean up as indicated in $<

