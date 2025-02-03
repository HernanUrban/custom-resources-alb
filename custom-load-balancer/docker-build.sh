#!/bin/bash
echo "build..."
./mvnw clean package

echo "create docker image"

if [[ -z "$1" ]]; then
  echo "without OTEL"
    docker build -t hhurban/custom-alb .
else
    echo "with OTEL"
    docker build -t hurban/custom-alb -f Dockerfile_otel .
fi