# Update Wolters Kluwer Application Credentals

Before start the hands on lab, we need to update the application credentials. With this credentials the application will be able to start an authentication flow on the Wolters Kluwer Identity Platform

## Update the appsettings.json with the following parameters
  
    ClientId: 
    WK.ES.A3WebApi.WkeDeveloperSummit
    
    ClientSecret: 
    WofTvfyvxNX1n5c*8QZ2^!kzGcrm9W
    
    AuthenticationScopes: 
    offline_access+openid+WK.ES.NEWPOL.COR.API+WK.ES.NEWPOL.ACC.API+WK.ES.NEWPOL.MNG.API+WK.ES.Webhooks
    
    RedirectUrl: 
    https://localhost:43900


 ![img](images/1.1.-%20Update%20appsettings%20file.PNG)

## Start the application and click on "Get Access Token" button. You will need the following user and password 

    Usuario:
    developersummit2023@mailinator.com
    
    Contraseña
    WkeDevSummit.Pa$$w0rd

## Open a new tab and navigate to <a href="https://a3innuva.wolterskluwer.es/">a3innuva</a>


### NOTE: For this hands on lab you will be working the a pre-created company with the following information. 

    companyCorrelationId = $"1a73b3c0-89c5-4385-9df7-6c3fdc6f9bd7";
    activityCorrelationId = $"96bee4a4-1a17-43d6-9a47-91f000f3419b";
    channelCorrelationId = $"3428a766-8132-48f2-a535-9f9c78e04951";
