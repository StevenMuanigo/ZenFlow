#!/bin/bash

# Build script for ZenFlow application

echo "Building ZenFlow application..."

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Build the solution
echo "Building the solution..."
dotnet build --no-restore

# Run tests
echo "Running tests..."
dotnet test --no-build

echo "Build completed successfully!"