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
