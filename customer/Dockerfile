FROM mhart/alpine-node:latest

WORKDIR /src/app

COPY ./package.json /src/app/

RUN yarn

EXPOSE 8000

COPY ./ /src/app/

ENTRYPOINT ["node", "index.js"]