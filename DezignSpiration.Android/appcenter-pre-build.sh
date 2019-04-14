#!/usr/bin/env bash

# Updating secrets

IdFile=$BUILD_REPOSITORY_LOCALPATH/DezignSpiration/Helpers/Constants.cs

sed -i '' "s/APP_SECRET/$APP_SECRET/g" $IdFile
sed -i '' "s/CLIENT_ID_KEY/$CLIENT_ID_KEY/g" $IdFile
sed -i '' "s/CLIENT_ID_SECRET_KEY/$CLIENT_ID_SECRET_KEY/g" $IdFile

# Print out file for reference
cat $IdFile

echo "Updated secrets!"
