Change web config of rest api project and app config of unit test project (change *****Your local database****** with existing local database);
<add key="ProductDbConnectionString" value="data source={*****Your local database******};initial catalog=Products;persist security info=True; Integrated Security=SSPI;"/>

Application will create empty Products database when it first run;
To add database with some data, restore "../SuitSupplyCodingChallenge/Product.bak" to your local sql server

Logs will be located;
Web API -> {your repository}\SuitSupplyCodingChallenge\SuitSupply.CodingTest.ProductCatalog\API\SuitSupply.CodingTest.ProductCatalog.WebApi\Logs
Web UI -> {your repository}\SuitSupplyCodingChallenge\SuitSupply.CodingTest.ProductCatalog\UI\SuitSupply.CodingTest.ProductCatalog.WebUI\Logs