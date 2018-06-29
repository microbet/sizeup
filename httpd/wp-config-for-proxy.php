if ($_SERVER['HTTP_X_FORWARDED_PROTO'] == 'https') {
  $_SERVER['HTTPS'] = 'on';
  define('WP_HOME','http://www.sizeup.com');
  define('WP_SITEURL','http://www.sizeup.com');
}
else {
  define('WP_HOME','http://corporate.sizeup.com');
  define('WP_SITEURL','http://corporate.sizeup.com');
}
