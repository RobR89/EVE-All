## EVE-All
Either this is Evil or EVE for All.

This is my EVE Online program... So far...

## Use
To use it you will need the sde dump file in zip form from https://developers.eveonline.com/resource/resources
Optionally you should also have at least the Types images zip as well.
These files are used directly and do not need to be unzipped.
Place them anywhere you want, on first run an options dialog will be presented where you can specify the file locations.
This can be changed later in the program options dialog. (File->Options)

When starting, the program will take some time to read in all the data, this may take some time. (2 and a half minutes on my system, yea it's slow :( )
This unfortunately is because of the YamlDotNet.RepresentationModel.YamlStream::Load() calls that take up about 65-70% of the cpu time.  As this is an external library I have no control over it.

## SSO login
SSO login has been implemented.  Everything seams to work so far with one limitation.
The states variable has a size limit that cannot handle all permission requests at one time.
This is due to tha fact that browsers only allow a maximum of 2043 characters in the address string.
