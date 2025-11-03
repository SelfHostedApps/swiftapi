#!/bin/bash
set -e

echo "==create an account=="

CREATE_RESPONSE=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"email":"testing@gmail.com","username":"tester","password":"12345","preference":"1",}' \
 http://127.0.0.1:5011/user/signup)

STATUS=$(echo "$CREATE_RESPONSE" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$CREATE_RESPONSE" | sed -e "s/HTTPSTATUS:.*//g")

if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">> create an account failed"
        echo "$STATUS"
        echo ">> $BODY"
        exit 1
else
        echo ">> create an account Succesful"
fi




echo "==login in as testing=="

LOGIN_RESPONSE=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"email": "testing@gmail.com", "password": "12345"}' \
 http://127.0.0.1:5011/user/login)

STATUS=$(echo $LOGIN_RESPONSE | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)     
TOKEN=$(echo $LOGIN_RESPONSE | sed -e "s/HTTPSTATUS:.*//g"| jq -r '.token')

if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">>login in failed ($STATUS)"
        echo "Response: $(echo "$LOGIN_RESPONSE" | sed -e "s/HTTPSTATUS:.*//g")"
        exit 1
else
        echo "Bearer $TOKEN"
fi

