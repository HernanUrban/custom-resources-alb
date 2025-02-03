#!/bin/bash
guard () {
    local what=$1
    shift
    if "$@"; then
        echo "$what succeeded" >&2
    else
        rc=$?
        echo "$what failed" >&2
        exit $rc
    fi
}

BASEDIR="$( cd "$( dirname "$0" )" && pwd )"

echo "-= UniqueUuidApi =-"
cd UniqueUuidApi/src/UniqueUuidApi
guard "UniqueUuidApi build and docker image" ./docker-build.sh
cd ${BASEDIR}
echo "-= Service Discovery =- "
cd service-discovery/
guard "service-discovery build and docker image" ./docker-build.sh $1
cd ${BASEDIR}
echo "-= Custom Load Balancer =-"
 cd custom-load-balancer
guard "scustom-load-balancer build and docker image" ./docker-build.sh $1
cd ${BASEDIR}
echo "All projects have been built and images have been created successfully!"