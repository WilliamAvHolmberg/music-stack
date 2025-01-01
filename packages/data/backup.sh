#!/bin/bash

# Get the script's directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)

# Determine environment and set paths accordingly
if [ "$ASPNETCORE_ENVIRONMENT" = "Production" ]; then
    BASE_DIR="/var/www/flash"
else
    BASE_DIR="$SCRIPT_DIR/.."
fi

SOURCE_DB="$BASE_DIR/data/app.db"
BACKUP_DIR="$BASE_DIR/data/backups"
BACKUP_FILE="$BACKUP_DIR/app_$TIMESTAMP.db"

# Create backup dir if it doesn't exist
mkdir -p $BACKUP_DIR

# Create backup
sqlite3 "$SOURCE_DB" ".backup '$BACKUP_FILE'"

# Keep only last 7 backups
find "$BACKUP_DIR" -name "app_*.db" -type f -mtime +7 -delete