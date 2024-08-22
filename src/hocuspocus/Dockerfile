# Use the official Node.js 20 image as the base image
FROM node:20

ENV BASE_URL=http://host.docker.internal:5069
ENV HOCUSPOCUS_SECRET_KEY=abrakadabrakey
ENV REDIS_HOST=host.docker.internal
ENV REDIS_PORT=6379
ENV EMV=docker-dev

# Set the working directory in the container
WORKDIR /usr/src/app

# Copy the package.json and package-lock.json files to the working directory
COPY package*.json ./

# Install the app dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY . .

# Expose the port the app runs on
EXPOSE 3010

# Define the command to start the app
CMD ["npm", "start"]