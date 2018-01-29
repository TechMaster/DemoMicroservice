FROM golang

ARG app_env
ENV APP_ENV $app_env

COPY . /go/src/api
WORKDIR /go/src/api

RUN go get ./vendor/database

RUN go get ./
RUN go build

CMD if [ ${APP_ENV} = production ]; \
	then \
	api; \
	else \
	go get github.com/pilu/fresh && \
	fresh; \
	fi
	
EXPOSE 8001
