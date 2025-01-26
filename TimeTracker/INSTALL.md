# Install Service on Debian 12

## Install packages and configure system services

### Install custom package from microsoft

_wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb_
<br>
_dpkg -i packages-microsoft-prod.deb_
<br>
_rm packages-microsoft-prod.deb_

### Install git

_apt install git_

### Install certbot

_apt install certbot python3-certbot-apache_

### Install SDK

_apt-get update_
<br>
_apt-get install -y dotnet-sdk-9.0_

### Update all packages

_apt-get update_
<br>
_apt-get upgrade_

### Install Apache

_apt install apache2_

### Install and enable apache modules

_a2enmod proxy proxy_http_
<br>
_systemctl restart apache2_

### Create apache config

_touch /etc/apache2/sites-available/EarthGolem.conf_

```
<VirtualHost *:80> 
    ServerName earthgolem.perforator.xyz
    ProxyPreserveHost On 
    ProxyPass / http://localhost:5000/ 
    ProxyPassReverse / http://localhost:5000/ 
 
    ErrorLog ${APACHE_LOG_DIR}/EarthGolem_error.log 
    CustomLog ${APACHE_LOG_DIR}/EarthGolem_access.log combined 
</VirtualHost>
```

### Deactivate old and activate new configuration, test and reload apache

_cd /etc/apache2/sites-available/_
<br>
_a2dissite 000-default.conf_
<br>
_rm 000-default.conf_
<br>
_a2ensite EarthGolem.conf_
<br>
_apache2ctl configtest_
<br>
_systemctl reload apache2_

## Install and configure project

### Go to working dir

_cd /var/www_

### Clone repo

_git clone https://github.com/dmtkomkov/EarthGolem.git_

### Go to project folder

_cd EarthGolem/TimeTracker/_

### Install aspnet packages

_dotnet restore_

### Install dotnet-ef tool and migrate database

_dotnet tool install --global dotnet-ef_
<br>
_dotnet ef database update_

### Create admin

_dotnet create-user --name admin --password some#password_

### Build and publish project

_dotnet build -c Release -o ./publish_

### Create service config

_touch /etc/systemd/system/timetracker.service_

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
Environment=Jwt__Key=SomeKey#ChangeIT

[Install]
WantedBy=multi-user.target
```

### Run and check timetracker service

_systemctl daemon-reload_
<br>
_systemctl enable timetracker.service_
<br>
_systemctl start timetracker.service_
<br>
_systemctl status timetracker.service_

## Install Certificate

### Request Certificate

_certbot --apache -d earthgolem.perforator.xyz_

### Automate renew certificate

_crontab -e_
```
31 1 * * * /usr/bin/certbot renew
```
