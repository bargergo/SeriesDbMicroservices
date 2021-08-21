# SeriesDbMicroservices
SeriesDB using Microservices architecture and Docker
## System Overview
![System Overview](https://raw.githubusercontent.com/bargergo/SeriesDbMicroservices/master/docs/system_overview.png)
## Running integration tests locally
Commands for starting databases for integration tests:
### MongoDB
```
docker run --name mongodb-seriesdb-test --env MONGO_INITDB_ROOT_USERNAME=root --env MONGO_INITDB_ROOT_PASSWORD=rootpassword --publish 27017:27017 --rm mongo:4.2.3
```
### MySQL
```
docker run --name mysql-ratingdb-test --env MYSQL_ROOT_PASSWORD=helloworld --env MYSQL_DATABASE=ratings --publish 3306:3306 --rm mysql:8.0.19
```
## Start in Kubernetes
### Start Traefik
Get Helm chart
```
helm repo add traefik https://containous.github.io/traefik-helm-chart
helm repo update
```
Install
```
helm install traefik traefik/traefik --set ports.web.nodePort=32080 --set service.type=NodePort
```
Proxy the dashboard
```
kubectl port-forward $(kubectl get pods --selector "app.kubernetes.io/name=traefik" --output=name) 9000:9000
```
Proxy the web entrypoint
```
kubectl port-forward $(kubectl get pods --selector "app.kubernetes.io/name=traefik" --output=name) 8000:8000
```
### Start databases (and message queue)
```
kubectl apply -f kubernetes/db
```
### Start applications
```
docker-compose build
kubectl apply -f kubernetes/app
```
### Remove everything
```
kubectl delete -f kubernetes/app
kubectl delete -f kubernetes/db
helm uninstall traefik
```
