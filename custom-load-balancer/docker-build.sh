#!/bin/bash
./mvnw clean package
docker build -t hurban/custom-alb .
