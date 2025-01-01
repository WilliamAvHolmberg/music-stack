# Infrastructure Setup

This directory contains all infrastructure configuration for the Flash Workshop application.

## Directory Structure
```
infra/
├── nginx/              # Nginx configuration
├── systemd/            # Systemd service files
├── scripts/            # Deployment and maintenance scripts
└── docs/              # Additional documentation
```

## Server Details
- Provider: Digital Ocean
- Region: Frankfurt
- OS: Ubuntu 22.04

## Initial Setup Steps
1. Create Droplet (Done)
2. Configure SSH (Done)
3. Install Dependencies
4. Configure Services
5. Deploy Application

## Deployment Process
Detailed deployment process is documented in `scripts/deploy.sh` 