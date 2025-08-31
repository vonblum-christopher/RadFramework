#!/bin/sh
cd ..
CurrentUser=$USER
sudo chown $CurrentUser RadFramework -R
cd RadFramework
echo "$CurrentUser owns the project now."
