# Verwenden Sie das offizielle nginx:alpine-Basisimage
FROM nginx:alpine

# Kopieren Sie die default.conf-Datei in den Container
COPY nginx.conf.template /etc/nginx/templates/nginx.conf.template
COPY robots.txt /etc/nginx/html/robots.txt

EXPOSE 3000
EXPOSE 3001

# Starten Sie Nginx beim Starten des Containers
CMD ["nginx", "-g", "daemon off;"]