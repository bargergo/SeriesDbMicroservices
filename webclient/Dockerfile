FROM node:12.16.2 as react-build
WORKDIR /app
COPY . ./
RUN npm install
RUN npm run build

FROM nginx:1.18.0-alpine
COPY --from=react-build /app/build /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]