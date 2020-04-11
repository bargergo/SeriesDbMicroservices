FROM gradle:5.6.2-jdk8 AS build
COPY --chown=gradle:gradle ./RatingsMicroservice /home/gradle/src
WORKDIR /home/gradle/src
ENV GRADLE_OPTS "-XX:MaxMetaspaceSize=256m -XX:+HeapDumpOnOutOfMemoryError -Xmx512m"
RUN gradle build --no-daemon

FROM openjdk:8-jre-alpine

EXPOSE 8080

ENV APPLICATION_USER ktor
RUN adduser -D -g '' $APPLICATION_USER

RUN mkdir /app
RUN chown -R $APPLICATION_USER /app

USER $APPLICATION_USER

COPY --from=build /home/gradle/src/build/libs/*.jar /app/
WORKDIR /app

CMD ["java", "-server", "-XX:+UnlockExperimentalVMOptions", "-XX:+UseCGroupMemoryLimitForHeap", "-XX:InitialRAMFraction=2", "-XX:MinRAMFraction=2", "-XX:MaxRAMFraction=2", "-XX:+UseG1GC", "-XX:MaxGCPauseMillis=100", "-XX:+UseStringDeduplication", "-jar", "ratings-microservice-all.jar"]