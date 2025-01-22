# Install Service on Debian 12

## Install packages and configure system services

### Install custom package from microsoft

_$ wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb_
<br>
_$ dpkg -i packages-microsoft-prod.deb_
<br>
_$ rm packages-microsoft-prod.deb_

### Install git

_$ apt install git_

### Install certbot

_$ apt install certbot python3-certbot-apache_

### Install SDK

_$ apt-get update_
<br>
_$ apt-get install -y dotnet-sdk-9.0_

### Update all packages

_$ apt-get update_
<br>
_$ apt-get upgrade_

### Install Apache

_$ apt install apache2_

### Install and enable apache modules

_$ a2enmod proxy proxy_http_
<br>
_$ systemctl restart apache2_

### Create apache config

_$ touch /etc/apache2/sites-available/EarthGolem.conf_

```
<VirtualHost *:80> 
    ProxyPreserveHost On 
    ProxyPass / http://localhost:5000/ 
    ProxyPassReverse / http://localhost:5000/ 
 
    ErrorLog ${APACHE_LOG_DIR}/EarthGolem_error.log 
    CustomLog ${APACHE_LOG_DIR}/EarthGolem_access.log combined 
</VirtualHost>
```

### Deactivate old and activate new configuration, test and reload apache

_$ cd /etc/apache2/sites-available/_
<br>
_$ a2dissite 000-default.conf_
<br>
_$ rm 000-default.conf_
<br>
_$ a2ensite EarthGolem.conf_
<br>
_$ apache2ctl configtest_
<br>
_$ systemctl reload apache2_

## Install and configure project

### Go to working dir

_$ cd /var/www_

### Clone repo

_$ git clone https://github.com/dmtkomkov/EarthGolem.git_

### Go to project folder

_$ cd EarthGolem/TimeTracker/_

### Install aspnet packages

_$ dotnet restore_

### Install dotnet-ef tool and migrate database

_$ dotnet tool install --global dotnet-ef_
<br>
_$ dotnet ef database update_

### Create admin

_$ dotnet create-user --name admin --password P@ssw0rd_

### Build and publish project

_$ dotnet build -c Release -o ./publish_

### Create service config

_$ touch /etc/systemd/system/timetracker.service_

```
[Unit]
Description=TimeTracker ASP.NET Core Application
After=network.target

[Service]
WorkingDirectory=/var/www/EarthGolem/TimeTracker
ExecStart=/usr/bin/dotnet /var/www/EarthGolem/TimeTracker/publish/TimeTracker.dll
Restart=always
RestartSec=10
SyslogIdentifier=timetracker
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

### Run and check timetracker service

_$ systemctl daemon-reload_
<br>
_$ systemctl start timetracker.service_
<br>
_$ systemctl enable timetracker.service_
<br>
_$ systemctl status timetracker.service_

## Install Certificate

### Request Certificate

_$ certbot --apache -d earthgolem.perforator.xyz_

### Automate renew certificate

_$ crontab -e_
```
31 1 * * * /usr/bin/certbot renew
```
