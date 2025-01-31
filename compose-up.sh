#!/bin/bash

cd $(dirname "$0")
docker compose -p custom-alb-poc up -d --build --remove-orphans