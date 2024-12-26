#!/bin/bash
set -e

# Validate required environment variables
: "${PROJECT_NAME:?'PROJECT_NAME is required'}"
: "${DEPLOY_PATH:?'DEPLOY_PATH is required'}"

echo "Starting remote setup for $PROJECT_NAME..."
echo "Deploy path: $DEPLOY_PATH"

# Update system
echo "Updating system packages..."
sudo apt update
sudo apt upgrade -y

# Install required packages
echo "Installing required packages..."
sudo apt install -y nginx sqlite3 certbot python3-certbot-nginx

# Install .NET SDK
echo "Installing .NET SDK..."
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Create application directory structure
echo "Creating directory structure..."
sudo mkdir -p $DEPLOY_PATH/{web,api,data/backups,api/secrets}
sudo chown -R www-data:www-data $DEPLOY_PATH

# Create backup script
echo "Creating backup script..."
cat > $DEPLOY_PATH/data/backup.sh << EOL
#!/bin/bash
TIMESTAMP=\$(date +%Y%m%d_%H%M%S)
SOURCE_DB="$DEPLOY_PATH/data/app.db"
BACKUP_DIR="$DEPLOY_PATH/data/backups"
BACKUP_FILE="\$BACKUP_DIR/app_\$TIMESTAMP.db"

mkdir -p \$BACKUP_DIR
sqlite3 "\$SOURCE_DB" ".backup '\$BACKUP_FILE'"
find "\$BACKUP_DIR" -name "app_*.db" -type f -mtime +7 -delete
EOL

# Make backup script executable
sudo chmod +x $DEPLOY_PATH/data/backup.sh

# Setup backup cron job
(crontab -l 2>/dev/null; echo "0 */6 * * * $DEPLOY_PATH/data/backup.sh") | crontab -

# Create systemd service
echo "Setting up systemd service..."
cat > /etc/systemd/system/$PROJECT_NAME-api.service << EOL
[Unit]
Description=$PROJECT_NAME API Service
After=network.target

[Service]
WorkingDirectory=$DEPLOY_PATH/api
ExecStart=/usr/bin/dotnet Api.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production
User=www-data

[Install]
WantedBy=multi-user.target
EOL

# Reload systemd and enable service
sudo systemctl daemon-reload
sudo systemctl enable $PROJECT_NAME-api

# Set correct permissions
echo "Setting permissions..."
sudo chown -R www-data:www-data $DEPLOY_PATH
sudo chmod 755 $DEPLOY_PATH
sudo chmod 755 $DEPLOY_PATH/data
sudo chmod 755 $DEPLOY_PATH/data/backups
sudo chmod 700 $DEPLOY_PATH/api/secrets

echo "Remote setup completed!" 