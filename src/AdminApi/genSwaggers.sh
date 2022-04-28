#!/bin/bash

mkdir /app/swagger

find ../ -iname '*.nswag' -print0 | while read -d $'\0' path
do
	cp $path ./ > /dev/null
	NSWAG_FILE=${path##*/}
	MODULE_NAME=${NSWAG_FILE%.*}
	JSON_FILE=${MODULE_NAME}.json
	
	dotnet swagger tofile --output /app/swagger/$JSON_FILE /app/UltimatePlaylist.AdminApi.dll $MODULE_NAME
done