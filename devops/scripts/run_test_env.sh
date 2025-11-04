#!/bin/bash

echo "Create test pod"
echo "-------------------------------"
podman pod create --name swift-test-pod -p 5012:5011

echo "Create image"
echo "-------------------------------"
podman build -t dotnet-app-image -f ../images/dotnet-image .

echo "Create test containers"
echo "-------------------------------"
echo ">> create swift-test-app"
podman run --replace -d \
 --name swift-test-api \
 --pod swift-test-pod \
 -e ConnectionStrings__Default="Host=postgres-test;Port=5432;Database=swiftdb;Username=swiftuser;Password=swiftpass" \
 dotnet-app-image

echo ">> create postgres test db"
podman run --replace -d \
 --name postgres-test \
 --pod swift-test-pod \
 -e POSTGRES_USER=swiftuser \
 -e POSTGRES_PASSWORD=swiftpass \
 -e POSTGRES_DB=swiftdb \
 -e PGPORT=5432 \
 -v ../init.sql:/docker-entrypoint-initdb.d/init.sql:ro \
 docker.io/postgres:16
