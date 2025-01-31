#!/bin/bash
dotnet clean
dotnet build
docker build -t hurban/unique-uuid-api .
