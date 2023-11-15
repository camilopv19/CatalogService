# CatalogService: Message Broker Publisher with RabbitMQ

For Task 4: Checkout the last commit

Run the project and update any Item with the PUT method. This will publish a new message.

The RabbitMQ configuration is in Fanout mode, so if any other Receiver client is listening, the message will be received too.

This could be done by implementing the 3rd part of the [RabbitMQ C# tutorial](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html) and changing the exchange property from "logs" to "catalog".