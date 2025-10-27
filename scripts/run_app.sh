#!/bin/bash

PODMAN=/usr/bin/podman

echo ">> creating pod"
#create pod
$PODMAN pod create --replace \
  --name swiftapiapp \
  -p 5011:5011
  
echo "Creating Images"
echo "--------------------------------"
echo ">> creating swiftapi image"
$PODMAN build -t dotnet-app-image -f ./images/dotnet-image .

echo "Creating Containers"
echo "--------------------------------"
echo ">> creating swiftapi container"
$PODMAN run --replace -d \
  --name swiftapi \
  --pod swiftapiapp \
  dotnet-app-image  

echo ">> creating postgres container"
$PODMAN run --replace -d \
  --name postgres \
  --pod swiftapiapp \
  -e POSTGRES_USER=swiftuser \
  -e POSTGRES_PASSWORD=swiftpass \
  -e POSTGRES_DB=swiftdb \
  -e PGPORT=5432 \
  docker.io/postgres:16
