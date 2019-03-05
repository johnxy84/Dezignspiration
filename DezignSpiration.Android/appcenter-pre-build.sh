#!/usr/bin/env bash

echo "Arguments for updating:"
echo " - AppSecret: $APP_SECRET"

# Updating secrets

IdFile=$BUILD_REPOSITORY_LOCALPATH/src/DezignSpiration/DezignSpiration/Helpers/Constants.cs

echo "Path to Constants => ${IdFile}"

sed -i '' "s/APP_SECRET/$APP_SECRET/g" $IdFile

# Print out file for reference
cat $IdFile

echo "Updated secrets!"
