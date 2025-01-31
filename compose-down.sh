#!/bin/bash

cd $(dirname "$0")
docker compose -p custom-alb-poc down -v --remove-orphans