@echo off
REM Run script for ZenFlow application

echo Starting ZenFlow application...

REM Start the application using Docker Compose
echo Starting services with Docker Compose...
docker-compose up -d

echo ZenFlow application is now running!
echo API Gateway is available at http://localhost:5000
echo User Service is available at http://localhost:5001
echo Catalog Service is available at http://localhost:5002
echo Nutrition Service is available at http://localhost:5003
echo Workout Service is available at http://localhost:5004
echo Progress Service is available at http://localhost:5005
echo Notification Service is available at http://localhost:5006
echo Reminder Service is available at http://localhost:5007