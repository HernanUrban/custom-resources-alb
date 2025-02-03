#!/bin/bash
echo "build..."
./mvnw clean package

echo "create docker image"

if [[ -z "$1" ]]; then
    echo "without OTEL"
    docker build -t hurban/discovery-service .
else
    echo "with OTEL"
    docker build -t hurban/discovery-service -f Dockerfile_otel .
fi

