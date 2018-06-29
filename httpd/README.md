## Install

vi /etc/hosts  # add hostname and ip-10-0-xxx-xxx
apt-get install apache2
cd /etc/apache2/mods-enabled
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

## Test

curl http://10.0.2.39/server-status
  578  curl -H "Host: www.sizeup.com" http://10.0.2.39/server-status
  579  curl -H "www.sizeup.com" http://10.0.2.39/dashboard/
  580  curl -H "sizeup.com" http://10.0.2.39/dashboard/
  581  curl -I -H "sizeup.com" http://10.0.2.39/dashboard/
  582  curl -I -H "lbi.sizeup.com" http://10.0.2.39/dashboard/
  583  curl -I -H "Host: www.sizeup.com" http://10.0.2.39/dashboard/
  584  curl -I -H "Host: sizeup.com" http://10.0.2.39/dashboard/
  585  curl -I -H "Host: application.sizeup.com" http://10.0.2.39/dashboard/
  586  curl -I -H "Host: lbi.sizeup.com" http://10.0.2.39/dashboard/
  587  curl -I -H "Host: www.sizeup.com" http://10.0.2.39/lbi
  590  curl -I -H "Host: news.sizeup.com" http://10.0.2.39/lbi
  591  curl -I -H "Host: news.sizeup.com" http://10.0.2.39/tags
  592  curl -I -H "Host: www.sizeup.com" http://10.0.2.39/tags
  593  curl -I -H "Host: www.sizeup.com" http://10.0.2.39/tags
curl -H "Host: www.sizeup.com" http://10.0.2.39/white-papers/small-business-banking-new-customer-demands-and-digital-expectations/
  552  curl -I -H "www.sizeup.com" http://10.0.2.39/developers/documentation
  553  curl -I -H "www.sizeup.com" http://10.0.2.39/developers/keystore/bbthacke
rs