#!/bin/bash

echo "Start AnanaPhone Boot"
dotnet /app/AnanaPhone.dll --stage1

echo "Start Call Control"
asterisk

echo "Start AnanaPhone"
nohup dotnet /app/AnanaPhone.dll &

echo "Start SSHD"
/etc/init.d/ssh start

echo "Switching to cron"

# start cron
/usr/sbin/cron -f -l 8