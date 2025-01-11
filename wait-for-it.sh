#!/usr/bin/env bash
# wait-for-it.sh: Wait for a service to be ready

set -e

host=$1
shift
port=$1
shift
cmd="$@"

>&2 echo "Waiting for $host:$port..."

while ! nc -z $host $port; do
  sleep 1
done

>&2 echo "$host:$port is up - executing command"
exec $cmd
