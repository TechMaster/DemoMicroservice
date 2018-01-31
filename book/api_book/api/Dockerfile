FROM golang:alpine

COPY . /go/src/api
WORKDIR /go/src/api

RUN apk update \
&& apk add git \
&& go get ./vendor/database \
&& go get ./ \
&& go build \
&& rm -rf /var/cache/apk/*

CMD ["./api"]
EXPOSE 8001