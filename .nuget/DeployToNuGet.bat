nuget.exe update -self
ECHO Y | DEL *.nupkg

REM Using an account for todd@meinershagen.net to publish to NuGet.org.  This can be changed in the future.
set /p NuGetApiKey= Please enter the project's NuGet API Key:
nuget.exe setApiKey %NuGetApiKey%

nuget.exe pack ..\James.Testing\James.Testing.csproj
# nuget.exe pack ..\James.Abstractions.System\James.Abstractions.System.csproj
nuget.exe pack ..\James.Testing.Rest\James.Testing.Rest.csproj
nuget.exe pack ..\James.Testing.Wcf\James.Testing.Wcf.csproj

nuget.exe push *.nupkg