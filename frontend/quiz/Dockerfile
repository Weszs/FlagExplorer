# Use Nginx (a high-performance web server) to serve your static files
FROM nginx:alpine

# Copy your HTML and JSON files into the Nginx server folder
# The '.' means 'everything in my current folder'
COPY . /usr/share/nginx/html

# Expose port 80 so we can see the website
EXPOSE 80