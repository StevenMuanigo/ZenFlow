@echo off
REM Build script for ZenFlow application

echo Building ZenFlow application...

REM Restore dependencies
echo Restoring dependencies...
dotnet restore

REM Build the solution
echo Building the solution...
dotnet build --no-restore

REM Run tests
echo Running tests...
dotnet test --no-build

echo Build completed successfully!