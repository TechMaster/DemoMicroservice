FROM golang:alpine as builder

COPY . /go/src/api
WORKDIR /go/src/api

RUN apk update \
&& apk add git \
&& go get ./vendor/database \
&& go get ./ \
&& go build \
&& rm -rf /var/cache/apk/*


FROM alpine:latest
RUN apk --no-cache add ca-certificates
WORKDIR /app
COPY --from=builder /go/src/api/ /app
CMD ["./api"]
EXPOSE 8001
