instance_id=$(shell ec2metadata --instance-id)
virtualenv_path = /etc/sizeup/virtualenv/primary
python ?= $(virtualenv_path)/bin/python

/data/dnb.volume:
	../../bin/setup-aws-volume.py --name="/data/dnb" --size=175 --device=/dev/xvdf --instance-id=$(instance_id) --breadcrumb-file=$@
	sleep 5  # /dev/xvdf needs a few seconds before it shows up ...?
	sudo mkfs -t ext4 /dev/xvdf
	sudo mount /dev/xvdf /data/dnb/
	sudo chmod 777 /data/dnb/

dnb-data-files: /data/dnb.volume
ifndef data_version
	$(error Must specify $$data_version)
endif
	bin/sort-wb-into-fdg-instances.sh $(data_version)
	# Done. Cleaning up temporary volume...
	sh $<
