@CALL dotnet restore
@CALL msbuild %~dp0..\RessurectIT.Pdf.Print.sln /t:Pack /p:"Configuration=%~1Installer";"Platform=Any CPU"