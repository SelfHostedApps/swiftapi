#!/bin/bash
set -euo pipefail

echo "==create an account==" >&2

CREATE_RESPONSE=$(curl -w "HTTPSTATUS:%{http_code}" \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"email":"testing@gmail.com","username":"tester","password":"12345","preference":"1"}' \
 http://swift-test-api:5011/user/signup)

set +e
STATUS=$(echo "$CREATE_RESPONSE" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$CREATE_RESPONSE" | sed -e "s/HTTPSTATUS:.*//g")
set -e

if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
    echo ">> create an account failed" >&2
    echo "Status: $STATUS" >&2
    echo "Body: $BODY" >&2
    exit 1
else
    echo ">> create an account successful" >&2
fi



echo "==login in as testing==" >&2

LOGIN_RESPONSE=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"email":"testing@gmail.com","password":"12345"}' \
 http://swift-test-api:5011/user/login)

STATUS=$(echo "$LOGIN_RESPONSE" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
TOKEN=$(echo "$LOGIN_RESPONSE" | sed -e "s/HTTPSTATUS:.*//g" | jq -r '.token')

if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
    echo ">> login failed ($STATUS)" >&2
    echo "Response: $(echo "$LOGIN_RESPONSE" | sed -e "s/HTTPSTATUS:.*//g")" >&2
    exit 1
else
    echo ">> login successful" >&2
    echo -n "Bearer $TOKEN"
fi
