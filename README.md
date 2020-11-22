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
