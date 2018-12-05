#!/usr/bin/env bash

echo "Arguments for updating:"
echo " - AppSecret: $APP_SECRET"

# Updating ids

IdFile=$BUILD_REPOSITORY_LOCALPATH/src/MyApp/Ids.cs

sed -i '' "s/APP_SECRET/$APP_SECRET/g" $IdFile

# Print out file for reference
cat $IdFile

echo "Updated id!"
