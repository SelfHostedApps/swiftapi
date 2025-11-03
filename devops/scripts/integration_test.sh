#!/bin/bash
set -e
# go to .git level of project
cd "$(git rev-parse --show-toplevel 2>dev>null || echo "$(dirname "$0")/../..")"


echo "testing users endpoint"
token=$(bash "$SCRIPT_DIR/integration_test/user.sh")
        

echo "testing tasks endpoint"
bash "$SCRIPT_DIR/integration_test/task.sh" $"token"
