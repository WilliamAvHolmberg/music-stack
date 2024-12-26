#!/bin/bash
set -e

# Store the root directory
ROOT_DIR="$(pwd)"

# Source and validate environment variables
source .env

# Required variables
PROJECT_NAME=${PROJECT_NAME:?"PROJECT_NAME is required"}
MACHINE_IP=${MACHINE_IP:?"MACHINE_IP is required"}
DEPLOY_PATH=${DEPLOY_PATH:?"DEPLOY_PATH is required"}
OPENAI_API_KEY=${OPENAI_API_KEY:?"OPENAI_API_KEY is required"}
CLAUDE_API_KEY=${CLAUDE_API_KEY:?"CLAUDE_API_KEY is required"}
ELKS_API_USERNAME=${ELKS_API_USERNAME:?"ELKS_API_USERNAME is required"}
ELKS_API_PASSWORD=${ELKS_API_PASSWORD:?"ELKS_API_PASSWORD is required"}

echo "Starting local deployment for $PROJECT_NAME..."
echo "Machine IP: $MACHINE_IP"
echo "Deploy path: $DEPLOY_PATH"

# Build frontend
echo "Building frontend..."
cd "$ROOT_DIR/web"
npm run build
tar czf web-dist.tar.gz -C dist .
scp web-dist.tar.gz root@$MACHINE_IP:/tmp/
rm web-dist.tar.gz

# Build backend
echo "Building backend..."
cd "$ROOT_DIR/Api"
dotnet publish -c Release
cd bin/Release/net8.0/publish
tar czf api-dist.tar.gz *
scp api-dist.tar.gz root@$MACHINE_IP:/tmp/
rm api-dist.tar.gz

# Copy deployment files
echo "Copying deployment files..."
cd "$ROOT_DIR"
scp infra/scripts/deploy.sh root@$MACHINE_IP:/tmp/deploy.sh
scp infra/nginx/app.conf root@$MACHINE_IP:/tmp/app.conf

# Execute remote deployment with required variables
ssh root@$MACHINE_IP \
    PROJECT_NAME="$PROJECT_NAME" \
    MACHINE_IP="$MACHINE_IP" \
    DEPLOY_PATH="$DEPLOY_PATH" \
    ASPNETCORE_ENVIRONMENT="Production" \
    OPENAI_API_KEY="$OPENAI_API_KEY" \
    CLAUDE_API_KEY="$CLAUDE_API_KEY" \
    ELKS_API_USERNAME="$ELKS_API_USERNAME" \
    ELKS_API_PASSWORD="$ELKS_API_PASSWORD" \
    'bash /tmp/deploy.sh'

# Health check
echo "Performing health check..."
MAX_RETRIES=5
RETRY_COUNT=0
HEALTH_URL="http://$MACHINE_IP/api/health"

while [ $RETRY_COUNT -lt $MAX_RETRIES ]; do
    if curl -s -f "$HEALTH_URL" > /dev/null; then
        echo "Health check passed! API is responding."
        echo "Local deployment completed!"
        exit 0
    fi
    
    RETRY_COUNT=$((RETRY_COUNT + 1))
    if [ $RETRY_COUNT -lt $MAX_RETRIES ]; then
        echo "Health check failed. Retrying in 5 seconds... (Attempt $RETRY_COUNT/$MAX_RETRIES)"
        sleep 5
    fi
done

echo "ERROR: Health check failed after $MAX_RETRIES attempts!"
exit 1 