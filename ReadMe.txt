Change web config of rest api project and app config of unit test project (change *****Your local database****** with existing local database);
<add key="ProductDbConnectionString" value="data source={*****Your local database******};initial catalog=Products;persist security info=True; Integrated Security=SSPI;"/>

Application will create empty Products database when it first run;
To add database with some data, restore "../SuitSupplyCodingChallenge/Product.bak" to your local sql server

Logs will be located;
Web API -> {your repository}\SuitSupplyCodingChallenge\SuitSupply.CodingTest.ProductCatalog\API\SuitSupply.CodingTest.ProductCatalog.WebApi\Logs
Web UI -> {your repository}\SuitSupplyCodingChallenge\SuitSupply.CodingTest.ProductCatalog\UI\SuitSupply.CodingTest.ProductCatalog.WebUI\Logs

For API Documentation; 
https://app.swaggerhub.com/apis/SuitSupplyProductApi/ProductWebApi/1.0.0

Make sure Web API runs on 2220 port and Web UI is on 2221.

Git repository;
https://github.com/EmrKsm/SuitSupplyCodingChallenge.git

Before start debugging set startup project as multiple from solution properties. You can apply this setting with looking StartupProjectConfig.png file.
