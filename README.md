# Customized_Identity_For_Net_Projects
this project is an independent authentication and authorization service that can add to your project as an package 


1 : you can add this project as an class library to your project.
2: this project use some data in this format as token validation paramters 


"BearerToken": 
{

    "Issuer": "https://localhost:44320 --- or any address you want ",
    "Audience": "https://localhost:44320   --- or any address you want ",
    "Key": " -- --------- ---- key -- -- --------------",
    "EncryptKey": " -- ------- --- --------encrypted key ---------",
    "AccessTokenExpirationMinutes": 30,
    "RefreshTokenExpirationMinutes": 50
},


  Google Authentication
  
  
3 : if you want to have google authentication on your system , take a look at TokenDI class in Dependency injection folder 

4 : this class library have its own dbcontext , so you need to change the connection string to your own on ApplicationDbContextDesignTimeFactory
4.1 : you need to add the same class next to the other context class on your project .

5 : Migrations : you need to specify the context while using migration commands 

  in Package Manager Console => 
  
 
                add-migration NameOfMigration -Context NameOfContext -OutputDir where you store migration classes (its optional) 
  
  
                update-database
  
  
  in .NET CLI =>
  
  
  dotnet ef migrations add NameOfMigration --context NameOfContext --output-dir where you store migration classes (its optional) 
  
  
 dotnet ef database update --context NameOfContext
