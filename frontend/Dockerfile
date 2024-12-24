# Use official Node.js image to build the React app
FROM node:18 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the package.json and package-lock.json (if available)
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the frontend code into the container
COPY . ./

# Build the React app
RUN npm run build

# Use nginx to serve the built app
FROM nginx:alpine

# Copy the build folder from the previous build stage into nginx's default static file location
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start nginx to serve the React app
CMD ["nginx", "-g", "daemon off;"]
