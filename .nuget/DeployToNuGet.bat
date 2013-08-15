nuget.exe update -self
ECHO Y | DEL *.nupkg

#NOTE - Using an account for todd@meinershagen.net to publish to NuGet.org.  This can be changed in the future.
nuget.exe setApiKey 565e6305-d5e8-41ec-a70e-494cbb0dc6cf
nuget.exe pack ..\James.Testing\James.Testing.csproj
nuget.exe push *.nupkg