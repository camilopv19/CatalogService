# Task 7: APM tools - Azure Application Insights
Clone or checkout the last commit, configure an Azure account and create an Application Insights app, copy the Connection string into the project appsettings.json file, ApplicationInsights.ConnectionString object and run the API.

Hitting the endpoints will generate traces and logs.

# Task 6: Ocelot API Gateway
Clone or checkout the last commit of the [Carting solution](https://github.com/camilopv19/CartService) and run it.

In this solution, run: CatalogService and APIGateway projects.

Point the Catalog and Carting endpoints ports to the APIGateway port, then login or register as valid roles (according task 5), copy the retrieved token string and paste it in every Catalog endpoint as Bearer token authorization scheme.

NFR's implemented: 
1) New dictionary endpoint added with hardcoded result.
2) 1 minute cache for all CatalogService endpoints.

# Task 5: Identity Management System

Run the projects: Identity and CatalogService.

On CatalogService app register some users with several roles (Manager and Buyer at least), then try to consume the items or categories endpoints so test the roles. Current register logic ends with the user logged in.

Whenever a role or user change is needed, it's only matter of logging in with another user or registering a new one.

# Task 4: Message Broker Publisher with RabbitMQ

Run the project and update any Item with the PUT method. This will publish a new message to the message broker (RabbitMQ must be installed and the service running).

Clone or checkout the last commit of the [Listener project](https://github.com/camilopv19/CartService), run it and see the console messages appear.

The RabbitMQ configuration is in Fanout mode, so if any other Receiver client is listening, the message will be received too.
This could be done by implementing the 3rd part of the [RabbitMQ C# tutorial](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html), changing the exchange property from "logs" to "catalog" and running the project.


# Task2-CatalogService
.Net core 6 app with SQL Express, Xunit and EF to implement N-tier arch

Task:
Create BLL (business logic layer) and DAL (data-access layer) for Catalog Service. You must follow Clean Architecture with physical layers separation (via separate DLLs).
Constraints:
SQL database for persistence layer (for example - Microsoft SQL Server Database File).
Layers should be physically separated.

**Non-functional Requirements (NFR):**
Testability
Extensibility

**Functional Requirements:**
Key entities: Category, Item (aka Product).

Category has:
- Name – required, plain text, max length = 50.
- Image – optional, URL.
- Parent Category – optional

The following operations are allowed for Category: get/list/add/update/delete.

Item has:
- Name – required, plain text, max length = 50.
- Description – optional, can contain html.
- Image – optional, URL.
- Category – required, one item can belong to only one category.
- Price – required, money.
- Amount – required, positive int.

The following operations are allowed for Item: get/list/add/update/delete.

### Main (default) project:
CatalogService, it opens a swagger page with 2 API's/controllers: Category and Item

![image](https://github.com/camilopv19/Task2-CatalogService/assets/26941935/03c722e3-4eab-454e-927a-6f3bb6e5b1f7)
