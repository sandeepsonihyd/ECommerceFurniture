# Azure App Service Deployment Guide

This guide will help you deploy your ECommerce Furniture application (React frontend + .NET 9.0 Web API) to Azure App Service.

## Prerequisites

1. **Azure Account**: You need an active Azure subscription
2. **Azure CLI**: Install from [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
3. **Node.js**: Version 18 or higher
4. **.NET 9.0 SDK**: Install from [here](https://dotnet.microsoft.com/download/dotnet/9.0)
5. **PowerShell**: For running the deployment script

## Deployment Options

### Option 1: Automated Deployment (Recommended)

#### Using PowerShell Script

1. Open PowerShell as Administrator
2. Navigate to your project root directory
3. Run the deployment script:

```powershell
.\deploy.ps1 -ResourceGroupName "rg-ecommerce-furniture" -AppServiceName "your-unique-app-name"
```

**Parameters:**
- `ResourceGroupName`: Name for your Azure resource group
- `AppServiceName`: Must be globally unique (e.g., "ecommerce-furniture-yourname")
- `AppServicePlan`: (Optional) App Service Plan name
- `Location`: (Optional) Azure region (default: "East US")
- `Sku`: (Optional) Pricing tier (default: "F1" - Free tier)

#### Example:
```powershell
.\deploy.ps1 -ResourceGroupName "rg-ecommerce-prod" -AppServiceName "ecommerce-furniture-demo2024" -Sku "B1"
```

### Option 2: Manual Deployment

#### Step 1: Prepare Azure Resources

1. **Login to Azure CLI:**
```bash
az login
```

2. **Create Resource Group:**
```bash
az group create --name rg-ecommerce-furniture --location "East US"
```

3. **Create App Service Plan:**
```bash
az appservice plan create --name ecommerce-plan --resource-group rg-ecommerce-furniture --sku F1 --is-linux
```

4. **Create App Service:**
```bash
az webapp create --resource-group rg-ecommerce-furniture --plan ecommerce-plan --name your-unique-app-name --runtime "DOTNET|9.0"
```

#### Step 2: Build React Frontend

1. **Navigate to React project:**
```bash
cd src/ecommerce-furniture-react
```

2. **Install dependencies:**
```bash
npm install
```

3. **Build for production:**
```bash
npm run build
```

4. **Copy build files to WebAPI:**
```bash
# Windows
mkdir ..\ECommerceFurniture.WebAPI\wwwroot
xcopy build\* ..\ECommerceFurniture.WebAPI\wwwroot\ /E /Y

# Linux/Mac
mkdir -p ../ECommerceFurniture.WebAPI/wwwroot
cp -r build/* ../ECommerceFurniture.WebAPI/wwwroot/
```

#### Step 3: Deploy .NET Application

1. **Navigate back to project root:**
```bash
cd ../../
```

2. **Publish .NET application:**
```bash
dotnet publish src/ECommerceFurniture.WebAPI/ECommerceFurniture.WebAPI.csproj -c Release -o ./publish
```

3. **Create deployment package:**
```bash
# Windows PowerShell
Compress-Archive -Path "./publish/*" -DestinationPath "./publish.zip" -Force

# Linux/Mac
cd publish && zip -r ../publish.zip . && cd ..
```

4. **Deploy to Azure:**
```bash
az webapp deployment source config-zip --resource-group rg-ecommerce-furniture --name your-unique-app-name --src "./publish.zip"
```

#### Step 4: Configure Environment Variables

```bash
az webapp config appsettings set --resource-group rg-ecommerce-furniture --name your-unique-app-name --settings "ASPNETCORE_ENVIRONMENT=Production" "AppSettings__FrontendUrl=https://your-unique-app-name.azurewebsites.net"
```

### Option 3: GitHub Actions (CI/CD)

1. **Setup GitHub Secrets:**
   - Go to your GitHub repository → Settings → Secrets and variables → Actions
   - Add `AZUREAPPSERVICE_PUBLISHPROFILE` secret with your App Service's publish profile

2. **Get Publish Profile:**
```bash
az webapp deployment list-publishing-profiles --resource-group rg-ecommerce-furniture --name your-unique-app-name --xml
```

3. **Update Workflow:**
   - Edit `.github/workflows/azure-deploy.yml`
   - Change `AZURE_WEBAPP_NAME` to your app service name
   - Commit and push to main branch

## Configuration Updates

### 1. Update App Service Name in Configuration

Before deployment, update the following files with your actual Azure App Service name:

**appsettings.json:**
```json
{
  "AppSettings": {
    "FrontendUrl": "https://your-actual-app-name.azurewebsites.net"
  }
}
```

### 2. Database Configuration

Your application is already configured to use Azure SQL Database. Ensure your connection string is correct in `appsettings.json`.

### 3. CORS Configuration

The application is configured to allow requests from your Azure App Service domain. The CORS policy will automatically use the configured frontend URL.

## Post-Deployment Verification

1. **Check Application Status:**
```bash
az webapp show --resource-group rg-ecommerce-furniture --name your-unique-app-name --query "state"
```

2. **View Application Logs:**
```bash
az webapp log tail --resource-group rg-ecommerce-furniture --name your-unique-app-name
```

3. **Test Your Application:**
   - Frontend: `https://your-app-name.azurewebsites.net`
   - API Documentation: `https://your-app-name.azurewebsites.net/swagger`

## Troubleshooting

### Common Issues:

1. **App Name Already Exists:**
   - App Service names must be globally unique
   - Try adding numbers or your initials to the name

2. **Build Failures:**
   - Ensure Node.js 18+ is installed
   - Ensure .NET 9.0 SDK is installed
   - Clear npm cache: `npm cache clean --force`

3. **Database Connection Issues:**
   - Verify your Azure SQL Database is accessible
   - Check firewall settings to allow Azure services

4. **CORS Issues:**
   - Verify the FrontendUrl in appsettings matches your actual domain
   - Check browser developer tools for CORS errors

### Logs and Monitoring:

```bash
# Stream logs
az webapp log tail --resource-group rg-ecommerce-furniture --name your-app-name

# Download logs
az webapp log download --resource-group rg-ecommerce-furniture --name your-app-name

# Check app insights (if configured)
az monitor app-insights component show --app your-app-name --resource-group rg-ecommerce-furniture
```

## Scaling and Production Considerations

### 1. Upgrade from Free Tier
```bash
az appservice plan update --name ecommerce-plan --resource-group rg-ecommerce-furniture --sku B1
```

### 2. Configure Custom Domain
```bash
az webapp config hostname add --resource-group rg-ecommerce-furniture --webapp-name your-app-name --hostname yourdomain.com
```

### 3. Enable Application Insights
```bash
az monitor app-insights component create --app your-app-name --location "East US" --resource-group rg-ecommerce-furniture
```

### 4. Configure Backup
```bash
az webapp config backup create --resource-group rg-ecommerce-furniture --webapp-name your-app-name --backup-name daily-backup --container-url "your-storage-url"
```

## Environment Management

For different environments (Development, Staging, Production), create separate:
- Resource Groups
- App Services
- App Service Plans
- Environment-specific configuration files

## Security Best Practices

1. **Use Azure Key Vault** for sensitive configuration
2. **Enable HTTPS only** in App Service settings
3. **Configure authentication** if needed
4. **Set up monitoring and alerts**
5. **Regular security updates**

## Cost Optimization

1. **Use Free Tier (F1)** for development/testing
2. **Scale up to Basic (B1)** for production
3. **Consider App Service Environment** for enterprise scenarios
4. **Monitor usage** with Azure Cost Management

---

**Need Help?** 
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [.NET 9.0 on Azure](https://docs.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore)
- [Azure CLI Reference](https://docs.microsoft.com/en-us/cli/azure/webapp) 