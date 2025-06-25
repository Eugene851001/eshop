dotnet sonarscanner begin /k:"eShop" /d:sonar.host.url="http://localhost:9001"  /d:sonar.token=$sonar_token

dotnet build

dotnet sonarscanner end /d:sonar.token=$sonar_token
