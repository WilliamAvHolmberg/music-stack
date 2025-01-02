#!/bin/bash
set -e

# Source and validate environment variables
source .env

# Required variables
PROJECT_NAME=${PROJECT_NAME:?"PROJECT_NAME is required"}
MACHINE_IP=${MACHINE_IP:?"MACHINE_IP is required"}
DEPLOY_PATH=${DEPLOY_PATH:?"DEPLOY_PATH is required"}

echo "Starting local setup for $PROJECT_NAME..."
echo "Machine IP: $MACHINE_IP"
echo "Deploy path: $DEPLOY_PATH"

# Execute remote setup with required variables
ssh root@$MACHINE_IP PROJECT_NAME="$PROJECT_NAME" DEPLOY_PATH="$DEPLOY_PATH" 'bash -s' < packages/infra/scripts/setup.sh

echo "Local setup completed!" 