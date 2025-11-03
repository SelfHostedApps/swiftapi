#!/bin/bash
set -e

echo "testing users endpoint"
token=(bash ./integration_test/user.sh)
        

echo "testing tasks endpoint"
bash ./integration_test/task.sh $"token"
