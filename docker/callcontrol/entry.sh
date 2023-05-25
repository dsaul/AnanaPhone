#!/bin/bash

echo "Start Call Control"

asterisk

echo "Start SSHD"
/etc/init.d/ssh start

echo "Switching to cron"

# start cron
/usr/sbin/cron -f -l 8