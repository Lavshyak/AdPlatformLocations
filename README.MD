По SOLID минимально сделал, чтобы тестировать можно было и выглядело попроще.\
Работает на FrozenDictionary<string, FrozenSet\<string\>>. Можно было бы еще на дереве, но это уже надо понимать, сколько площадок и регионов будет, бенчмарки делать.

Либо по простому dotnet build dotnet run и сами порты смотрите.\
Либо docker compose up, для хоста будет \
http://localhost:6464, \

GET http://localhost:6464/AdPlatforms/Get?location=/ru, \
POST с formfile http://localhost:6464/AdPlatforms/ReInit, \

http://localhost:6464/swagger, \
http://localhost:6464/swagger/v1/swagger.json, \
http://localhost:6464/openapi/v1.json

Есть еще xunit тесты (AdPlatformLocations.Lib.Tests), так же в докер композе для них сервис есть (либо руками dotnet test).