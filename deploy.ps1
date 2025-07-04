# Azure App Service Deployment Script
# This script builds the React frontend and deploys the full application to Azure App Service

param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,
    
    [Parameter(Mandatory=$true)]
    [string]$AppServiceName,
    
    [Parameter(Mandatory=$false)]
    [string]$AppServicePlan = "$AppServiceName-plan",
    
    [Parameter(Mandatory=$false)]
    [string]$Location = "East US",
    
    [Parameter(Mandatory=$false)]
    [string]$Sku = "F1"
)

Write-Host "Starting deployment to Azure App Service..." -ForegroundColor Green

# Check if Azure CLI is installed
if (!(Get-Command "az" -ErrorAction SilentlyContinue)) {
    Write-Error "Azure CLI is not installed. Please install it from https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"
    exit 1
}

# Login to Azure (if not already logged in)
Write-Host "Checking Azure login status..." -ForegroundColor Yellow
$loginStatus = az account show 2>$null
if (!$loginStatus) {
    Write-Host "Please login to Azure..." -ForegroundColor Yellow
    az login
}

# Set the working directory to the project root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Step 1: Build the React frontend
Write-Host "Building React frontend..." -ForegroundColor Blue
Set-Location "src/ecommerce-furniture-react"

# Install dependencies
Write-Host "Installing React dependencies..." -ForegroundColor Yellow
npm install

# Build the React app
Write-Host "Building React app for production..." -ForegroundColor Yellow
npm run build

# Move build files to the WebAPI wwwroot directory
Write-Host "Moving React build to WebAPI wwwroot..." -ForegroundColor Yellow
$webapiPath = "../ECommerceFurniture.WebAPI"
$wwwrootPath = "$webapiPath/wwwroot"

# Create wwwroot directory if it doesn't exist
if (!(Test-Path $wwwrootPath)) {
    New-Item -ItemType Directory -Path $wwwrootPath -Force
}

# Copy React build files
Copy-Item -Path "build/*" -Destination $wwwrootPath -Recurse -Force

Set-Location "../../"

# Step 2: Create Azure resources (if they don't exist)
Write-Host "Creating Azure resources..." -ForegroundColor Blue

# Create resource group
Write-Host "Creating resource group: $ResourceGroupName" -ForegroundColor Yellow
az group create --name $ResourceGroupName --location $Location

# Create App Service Plan
Write-Host "Creating App Service Plan: $AppServicePlan" -ForegroundColor Yellow
az appservice plan create --name $AppServicePlan --resource-group $ResourceGroupName --sku $Sku --is-linux

# Create App Service
Write-Host "Creating App Service: $AppServiceName" -ForegroundColor Yellow
az webapp create --resource-group $ResourceGroupName --plan $AppServicePlan --name $AppServiceName --runtime "DOTNET|9.0"

# Step 3: Configure App Service settings
Write-Host "Configuring App Service settings..." -ForegroundColor Blue

# Set environment variables
az webapp config appsettings set --resource-group $ResourceGroupName --name $AppServiceName --settings @'
[
  {
    "name": "ASPNETCORE_ENVIRONMENT",
    "value": "Production"
  },
  {
    "name": "AppSettings__FrontendUrl",
    "value": "https://${AppServiceName}.azurewebsites.net"
  }
]
'@

# Step 4: Deploy the application
Write-Host "Deploying application to Azure..." -ForegroundColor Blue

# Build and publish the .NET application
Write-Host "Building .NET application..." -ForegroundColor Yellow
dotnet publish src/ECommerceFurniture.WebAPI/ECommerceFurniture.WebAPI.csproj -c Release -o ./publish

# Create deployment package
Write-Host "Creating deployment package..." -ForegroundColor Yellow
Compress-Archive -Path "./publish/*" -DestinationPath "./publish.zip" -Force

# Deploy the zip file
Write-Host "Deploying to Azure App Service..." -ForegroundColor Yellow
az webapp deployment source config-zip --resource-group $ResourceGroupName --name $AppServiceName --src "./publish.zip"

# Clean up
Remove-Item "./publish" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "./publish.zip" -Force -ErrorAction SilentlyContinue

Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "Your application is now available at: https://$AppServiceName.azurewebsites.net" -ForegroundColor Green
Write-Host "API documentation (Swagger) is available at: https://$AppServiceName.azurewebsites.net/swagger" -ForegroundColor Green 