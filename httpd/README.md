## Install

```
vi /etc/hosts  # add hostname and ip-10-0-xxx-xxx
apt-get install apache2
cd /etc/apache2/mods-enabled
ln -s ../mods-available/headers.load
ln -s ../mods-available/proxy.conf
ln -s ../mods-available/proxy.load
ln -s ../mods-available/proxy_http.load
ln -s ../mods-available/rewrite.load
ln -s ../mods-available/status.conf
ln -s ../mods-available/status.load
mkdir /var/www/html/bbthackers
vi /var/www/html/bbthackers/index.html
vi /var/www/htpasswd
cd /etc/apache2
vi apache2.conf
```

Ensure the Wordpress installation's wp-config.php contains the code in wp-config-for-proxy.php .

## Test

```
curl http://$host/server-status
curl -H "Host: www.sizeup.com" http://$host/server-status
curl -H "Host: www.sizeup.com" http://$host/dashboard/
curl -I -H "Host: sizeup.com" http://$host/dashboard/
curl -I -H "Host: application.sizeup.com" http://$host/dashboard/
curl -I -H "Host: lbi.sizeup.com" http://$host/dashboard/
curl -I -H "Host: www.sizeup.com" http://$host/lbi
curl -I -H "Host: news.sizeup.com" http://$host/
curl -I -H "Host: news.sizeup.com" http://$host/lbi
curl -I -H "Host: news.sizeup.com" http://$host/company/about/
curl -I -H "Host: www.sizeup.com" http://$host/company/about/
curl -I -H "Host: www.sizeup.com" http://$host/white-papers/small-business-banking-new-customer-demands-and-digital-expectations/
curl -I -H "www.sizeup.com" http://$host/developers/documentation
curl -I -H "www.sizeup.com" http://$host/developers/keystore/bbthackers
```
