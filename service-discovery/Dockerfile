FROM openjdk:17-jdk-slim-buster
RUN mkdir /opt/app
ADD target/*.jar /opt/app/app.jar
CMD ["java", "-jar", "/opt/app/app.jar"]
