#!/bin/bash
set -e

token=(bash ./integration_test/user.sh)

bash ./integration_test/tasks.sh $"token"
