Umbraco 8 Backoffice Themes
---------------------------

Thanks for installing the Backoffice Themes package for Umbraco8.

This package lets you change the colour scheme for Umbraco via
a dashboard in the settings section. 

** Contributions and improvements to the themes is welcomed **


Config
------

The dashboard lets you set the theme for your own user account, 
but you can use the config file to set the them for a group of
users or by the name of the server. 

As an example the below themes.config.json file will : 

1. set the admin user (-1) to use the 'dark' theme 
2. set any members of the admin group to use dark
3. use the default theme if logged on to localhost
```
{
  "users": [
    {
      "userId": -1,
      "theme": "dark"
    }
  ],
  "groups": [
    {
      "group": "admin",
      "theme":  "dark"
    }
  ],
  "servers": [
    {
      "pattern": "^localhost.*$|^.*\\.local$",
      "theme": "default"
    }
  ]
}
```
