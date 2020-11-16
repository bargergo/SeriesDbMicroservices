FROM openjdk:8-jre-alpine AS base
EXPOSE 8080
ENV APPLICATION_USER ktor
RUN adduser -D -g '' $APPLICATION_USER
RUN mkdir /app
RUN chown -R $APPLICATION_USER /app
WORKDIR /app
USER $APPLICATION_USER

FROM gradle:5.6.2-jdk8 AS build
WORKDIR /home/gradle/src
ENV GRADLE_OPTS "-XX:MaxMetaspaceSize=256m -XX:+HeapDumpOnOutOfMemoryError -Xmx512m"
COPY --chown=gradle:gradle build.gradle settings.gradle gradlew ./
COPY --chown=gradle:gradle gradle ./gradle
RUN ./gradlew build --no-daemon -x test || return 0
COPY --chown=gradle:gradle . .
RUN ./gradlew build --no-daemon -x test

FROM base AS final
COPY --from=build /home/gradle/src/build/libs .
CMD ["java", "-server", "-XX:+UnlockExperimentalVMOptions", "-XX:+UseCGroupMemoryLimitForHeap", "-XX:InitialRAMFraction=2", "-XX:MinRAMFraction=2", "-XX:MaxRAMFraction=4", "-XX:+UseG1GC", "-XX:MaxGCPauseMillis=100", "-XX:+UseStringDeduplication", "-jar", "ratings-microservice-all.jar"]