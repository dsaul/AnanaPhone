#!/bin/bash

# Working Dir must be /app or dotnet won't find wwwroot
cd /app
echo "Start AnanaPhone Boot"
dotnet AnanaPhone.dll --stage1

echo "Start Call Control"
asterisk

echo "Start AnanaPhone"
nohup dotnet AnanaPhone.dll &

echo "Start SSHD"
/etc/init.d/ssh start

echo "Switching to cron"

# start cron
/usr/sbin/cron -f -l 8