# Dockerfile
FROM node

# create destination directory
RUN mkdir -p /usr/src/nuxt-app
WORKDIR /usr/src/nuxt-app

# update and install dependency
RUN apt-get update
RUN apt-get install git

# copy the app, note .dockerignore
COPY nuxtjs-tutorial /usr/src/nuxt-app/
RUN npm install
RUN npm run build

EXPOSE 3000

ENV NUXT_HOST=0.0.0.0
ENV NUXT_PORT=3000

CMD [ "npm", "run", "dev" ]