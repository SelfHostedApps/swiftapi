#!/bin/bash
set -e
TOKEN=$1



echo "==Task create=="

CREATE_TASK=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X POST \
 -H "Content-Type: application/json" \
 -H "Authorization: $TOKEN" \
 -d '{"title": "test", "desc": "this is a test"}' \
 http://127.0.0.1:5011/task/create)

STATUS=$(echo "$CREATE_TASK" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$CREATE_TASK"   | sed -e "s/HTTPSTATUS:.*//g")


if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">> creating failed ($STATUS)"
        echo ">> $BODY"
        exit 1
else
        echo ">> creating a task successful"
fi




echo "==Task update=="

UPDATE_TASK=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X PATCH \
 -H "Authorization: $TOKEN" \
 http://127.0.0.1:5011/task/completed/1)


STATUS=$(echo "$UPDATE_TASK" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$UPDATE_TASK"   | sed -e "s/HTTPSTATUS:.*//g")


if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">> creating failed ($STATUS)"
        echo ">> $BODY"
        exit 1
else
        echo "creating a task successful"
fi



echo "==get all tasks=="

GET_ALL_TASK=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X GET \
 -H "Content-Type: application/json" \
 -H "Authorization: $TOKEN" \
 http://127.0.0.1:5011/task/all)

STATUS=$(echo "$GET_ALL_TASK" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$GET_ALL_TASK"   | sed -e "s/HTTPSTATUS:.*//g")


if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">> creating failed ($STATUS)"
        echo ">> $BODY"
        exit 1
else
        echo ">> get tasks successful"
fi



echo "==task delete=="

DELETE_TASK=$(curl -s -w "HTTPSTATUS:%{http_code}" \
 -X DELETE \
 -H "Authorization: $TOKEN" \
 http://127.0.0.1:5011/task/delete/1)


STATUS=$(echo "$DELETE_TASK" | grep -o "HTTPSTATUS:[0-9]*" | cut -d: -f2)
BODY=$(echo "$DELETE_TASK"   | sed -e "s/HTTPSTATUS:.*//g")


if [ "$STATUS" -lt 200 ] || [ "$STATUS" -ge 300 ]; then
        echo ">> creating failed ($STATUS)"
        echo ">> $BODY"
        exit 1
else
        echo ">> delete tasks successful"
fi

