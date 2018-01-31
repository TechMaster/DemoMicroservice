#!/bin/sh
docker stop nodeapp
docker rm nodeapp
docker build -t nodeapp .
docker run --name nodeapp -d -p 8000:8000 nodeapp
sleep 1
curl http://localhost:8000/list