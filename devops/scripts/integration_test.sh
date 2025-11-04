#!/bin/bash
set -e
# go to .git level of project
cd "$(git rev-parse --show-toplevel 2>/dev/null || echo "$(dirname "$0")/../..")"


echo "testing users endpoint"
token=$(bash "/integration_test/user.sh")


echo "testing tasks endpoint"
bash "/integration_test/task.sh" "$token"
