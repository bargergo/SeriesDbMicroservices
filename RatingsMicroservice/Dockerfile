FROM openjdk:11.0.9.1-jre-slim-buster AS base
EXPOSE 8080
RUN mkdir /app
WORKDIR /app

FROM openjdk:11.0.9.1-jdk-buster AS build
WORKDIR /home/gradle/src
COPY build.gradle settings.gradle gradlew gradle.properties ./
COPY gradle ./gradle

# Only download dependencies
RUN ./gradlew downloadDependencies --no-daemon

COPY . .
RUN ./gradlew clean build --no-daemon -x test

FROM base AS final
COPY --from=build /home/gradle/src/build/libs .
CMD ["java", "-server", "-XX:+UnlockExperimentalVMOptions", "-XX:+UseG1GC", "-XX:MaxGCPauseMillis=100", "-XX:+UseStringDeduplication", "-jar", "ratings-microservice-all.jar"]