etl-system-env:
	# Use FreeTDS with unix ODBC for Python to talk with SQL Server.
	# http://www.freetds.org/userguide/odbcconnattr.htm
	# https://snakeycode.wordpress.com/2013/12/04/installing-pyodbc-on-ubuntu-12-04-64-bit/
	#
	sudo apt-get install unixodbc unixodbc-dev python-dev
	sudo apt-get install freetds-dev freetds-bin tdsodbc
	sudo cat freetds-odbc-entry.ini >> /etc/odbcinst.ini
	sudo $(MAKE) virtualenv dir=/etc/sizeup/virtualenv/primary

# Install a virtual python environment to keep our dependencies separate
# from the Ubuntu system python. Partly based on
# https://stackoverflow.com/questions/4324558/whats-the-proper-way-to-install-pip-virtualenv-and-distribute-for-python
#
virtualenv:
ifndef dir
	$(error Must specify $$dir - directory in which to create virtualenv)
endif
	wget 'https://pypi.python.org/packages/d4/0c/9840c08189e030873387a73b90ada981885010dd9aea134d6de30cd24cb8/virtualenv-15.1.0.tar.gz#md5=44e19f4134906fe2d75124427dc9b716'
	tar xzf virtualenv-15.1.0.tar.gz
	mkdir -p `dirname ${dir}`
	python virtualenv-15.1.0/virtualenv.py ${dir}
	. ${dir}/bin/activate ; \
	  pip install pyodbc boto3 ipython==5.4 configargparse ; \
	  pip install awscli --upgrade

