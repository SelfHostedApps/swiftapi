#!/bin/bash
set -e

echo ">> creating pod"
#create pod
podman pod create --replace \
  --name swiftapiapp \
  -p 5011:5011
  
echo "Creating Images"
echo "--------------------------------"
echo ">> creating swiftapi image"
podman build -t dotnet-app-image -f ./devops/images/dotnet-image .

echo "Creating Containers"
echo "--------------------------------"
echo ">> creating swiftapi container"
podman run --replace -d \
  --name swiftapi \
  --pod swiftapiapp \
  dotnet-app-image  

echo ">> creating postgres container"
podman run --replace -d \
  --name postgres \
  --pod swiftapiapp \
  -e POSTGRES_USER=swiftuser \
  -e POSTGRES_PASSWORD=swiftpass \
  -e POSTGRES_DB=swiftdb \
  -e PGPORT=5432 \
  -v swiftdb_data:/var/lib/postgresql/data:Z \
  -v ./devops/init.sql:/docker-entrypoint-initdb.d/init.sql:Z \
  docker.io/postgres:16
