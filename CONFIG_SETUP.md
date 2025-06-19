# Configuration Setup Guide

## 🔐 Security Notice

This repository does NOT contain sensitive credentials. You must set up your own configuration files locally.

## 📋 Setup Instructions

### 1. Copy Template Files

Copy the template configuration files and rename them:

```bash
# API Configuration
copy SiroccoDemo.APIs\Web.config.template SiroccoDemo.APIs\Web.config

# Moq Configuration
copy SiroccoDemo.Moq\App.config.template SiroccoDemo.Moq\App.config
```

### 2. Update Configuration Values

#### SiroccoDemo.APIs/Web.config

Replace the following placeholders in your copied `Web.config`:

```xml
<add name="CrmConnectionString" connectionString="AuthType=OAuth;Username=[YOUR_USERNAME];Password=[YOUR_PASSWORD];Url=[YOUR_CRM_URL];AppId=[YOUR_APP_ID];RedirectUri=[YOUR_REDIRECT_URI];TokenCacheStorePath=c:\MyTokenCache;LoginPrompt=Never" />
```

**Replace with your actual values:**

- `[YOUR_USERNAME]` - Your Dynamics 365 username
- `[YOUR_PASSWORD]` - Your Dynamics 365 password
- `[YOUR_CRM_URL]` - Your Dynamics 365 URL (e.g., https://yourorg.crm4.dynamics.com)
- `[YOUR_APP_ID]` - Your Azure AD Application ID
- `[YOUR_REDIRECT_URI]` - Your redirect URI

#### SiroccoDemo.Moq/App.config

Replace the following placeholders:

```xml
<add name="Moq" connectionString="AuthType=ClientSecret; url=[YOUR_CRM_URL];ClientId=[YOUR_CLIENT_ID];ClientSecret=[YOUR_CLIENT_SECRET]" />
```

**Replace with your actual values:**

- `[YOUR_CRM_URL]` - Your Dynamics 365 URL
- `[YOUR_CLIENT_ID]` - Your Azure AD Application/Client ID
- `[YOUR_CLIENT_SECRET]` - Your Azure AD Client Secret

### 3. Verify .gitignore

The `.gitignore` file prevents accidental commits of sensitive config files. Make sure these patterns exist:

```
# Sensitive config files - use .template versions instead
SiroccoDemo.APIs/Web.config
SiroccoDemo.Moq/App.config
SiroccoDemo.Infrastructure/app.config
SiroccoDemo.Plugins/app.config
```

## ⚠️ Important Security Notes

1. **NEVER commit real credentials** to version control
2. **Always use template files** for the repository
3. **Keep your local config files private**
4. **Rotate credentials** if accidentally exposed
5. **Use environment variables** in production environments

## 🔧 Alternative: Environment Variables (Recommended for Production)

For production deployments, consider using environment variables instead of config files:

```csharp
// Example: Reading from environment variables
var connectionString = Environment.GetEnvironmentVariable("CRM_CONNECTION_STRING")
    ?? ConfigurationManager.ConnectionStrings["CrmConnectionString"].ConnectionString;
```

## 📞 Support

If you have questions about configuration setup, contact the development team.
