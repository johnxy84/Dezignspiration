#!/usr/bin/env bash

echo "Arguments for updating:"
echo " - AppSecret: $APP_SECRET"
echo " - ApiSecret: $API_SECRET_VALUE"

# Updating secrets

IdFile=$BUILD_REPOSITORY_LOCALPATH/DezignSpiration/Helpers/Constants.cs

echo "Path to Constants => ${IdFile}"

sed -i '' "s/APP_SECRET/$APP_SECRET/g" $IdFile
sed -i '' "s/API_SECRET_VALUE/$API_SECRET_VALUE/g" $IdFile

# Print out file for reference
cat $IdFile

echo "Updated secrets!"
