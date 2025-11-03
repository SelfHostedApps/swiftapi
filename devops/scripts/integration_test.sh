#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "testing users endpoint"
token=$(bash "$SCRIPT_DIR/integration_test/user.sh")
        

echo "testing tasks endpoint"
bash "$SCRIPT_DIR/integration_test/task.sh" $"token"
