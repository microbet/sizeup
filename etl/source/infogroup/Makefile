instance_id = $(shell ec2metadata --instance-id)
virtualenv_path = /etc/sizeup/virtualenv/primary
python ?= $(virtualenv_path)/bin/python

/data/infogroup.volume:
	$(python) ../../bin/setup-aws-volume.py --name="/data/infogroup" --size=22 --device=/dev/xvdg --instance-id=$(instance_id)
	sleep 5  # /dev/xvdg needs a few seconds before it shows up ...?
	sudo mkfs -t ext4 /dev/xvdg
	sudo mount /dev/xvdg /data/infogroup/
	sudo chmod 777 /data/infogroup/
	echo "sudo umount /data/infogroup" > $@
	echo "$(python) -c 'import boto3; boto3.resource(\"ec2\").Instance(\"$(instance_id)\").detach_volume(VolumeId=\"vol-TODO\")'" >> $@
	# also delete. TODO put all this in a template anyway.

infogroup-data-files: /data/infogroup.volume
ifndef data_version
	$(error Must specify $$data_version)
endif
	bin/get-infogroup.sh $(data_version)
	cat /data/infogroup/SIZEUP_${data_version}?.TXT | $(python) bin/sort-infogroup-into-fdg-instances.py $(data_version)
	aws s3 mv --region us-east-1 /data/infogroup/infogroup.$(data_version).Austin.csv s3://sizeup-datasources/infogroup/$(data_version)/
	# Please manually clean up as indicated in $<


