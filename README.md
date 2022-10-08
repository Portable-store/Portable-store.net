# Portable Store

Allows you to quickly download portable applications, on restricted environments such as school or work computers.

Work on Windows, maybe linux and macos later.
Can easily be ported to linux or macOS.

## Download the application

Go to [release](https://github.com/Tom60chat/Portable-store/releases), and download the console or GUI version.

## How work the console app ?

```
pstore [options] command
 
Commands :
download [App name] : Download the given application(s)
delete [App name] : Delete the given application(s)
search [App name] : Search the given application(s)
list : List downloaded application
update [App name] : Update the given applciation(s)
refresh : Refresh the application metadatas cache

run [App name] : Start an application

create [metadata file name] : Create a new application metadata
read [App name/metadata file] : Read an application metadata
```

Before downloading always refresh the 

To download an app like Rufus, put :
`pstore download Refus`

To remove it, put:
`pstore delete Refus`