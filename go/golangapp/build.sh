#!/bin/sh
docker stop goapp
docker rm goapp
docker build -t goapp .
docker run --name goapp -d -p 8001:8001 goapp
curl http://localhost:8001/book
