FROM iron/go:dev
WORKDIR /app
COPY ./ /app
RUN go get github.com/gorilla/mux; cd /app; go build;

EXPOSE 8001
ENTRYPOINT ["./app"]

