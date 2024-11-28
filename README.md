# CurrencyExchangeDemo
 Microservice Asp.Net Core & Reactjs for Currency Exchange Rates Demo

The project is about a client server application that should lively demonstrate the exchange rates of currencies that fetched from an external online source.
A short video has been provided for explanation about the project in the link: https://www.loom.com/share/414105b2f14542b4a012a57b233bc14f?sid=27444f8a-ccc7-4299-a047-4430ba08b213

The project separated to 2 parts: one for the back-end and the other is for the front-end.

The back-end project has been implemented complying microservice architecture using C# Asp.Net Core.
The front-end application has been implemented by Reactjs as a JavaScript framework for modern front-end technologies.

Front-End
For the front-end I created a Reactjs App that consumes the Rates Api implemented in the back-end microservice. This calling action has been declared in api.js using Axios, It calls the api from ApiGateway and get All the rates. 
The main process is handled by the ApiPage.js which is the main component to show the results of the currency rates and this component is added to the app.js of React.
The business process of this component is fetching the Rates and Showing them in the table. They are also kept in a UseState object for comparison reasons from the new changed rates. The rates fetched every 5 seconds and then compared with the previous read rates to define change percentage and type of change as is demonstrating with colored arrows. You can see the process in the screen.


Backend:
As mentioned, I implemented the project as a microservice, so the back-end project consists of 2 main parts: 
1-	The Api Gateway Project that handles all the requests coming from the clients.
2-	The main microservice that handle the processes related to fetching the rates from an external source and respond to the clients 

Api Gateway
The api gateway project has been implemented by Ocelot.
To implement an api gateway with Ocelot, we need a json file to keep the routes related information.
Every route has a downstream and upstream url address and handles defined types of http-methods.
As you can see in the file apigateway.json we have defined a GET route from "/gateway/currency-exchange/rates" to "/api/v1/currency-exchange/rates" with the localhost and port 5052.


Microservice
For the microservice part we have multiple projects that aligns with the Clean Architecture design pattern:

Api project
The Api project is the presentation layer so it receives the requests and responds to them, as you can see there is a RatesController class for Rates that is responsible to return the currency exchange rates by GET http-method with the route address of "api/v1/currency-exchange/[Controller]" that is already set as downstream in api gateway configuration file. This controller inherits from the BaseController abstract class that has the MediatR object to handle the requests as a Mediator design pattern and also there are some implementations for the format of the ActionResults and also for logging, in it. 
There are also we have a ServicesSetup class file that is responsible to set the ServicesContainer for Dependency Inversion Control.
I implemented a Middleware to handle the exceptions in all scope of this microservice named ExceptionHandlingMiddleware.

After the controller gets the request of demonstrating rates it initiates an AllRatesQuery object from the Application layer.

Application Project
The application layer in Clean Architecture is responsible for Orchestrating the requests and flow of the processes. It should have the Abstraction Layer of using components for the business processes. As you can see here, we have the interfaces for CurrencyExchange and Caching and Validation rules that are the dependencies for the main part of this project : AllRatesQuery.cs file that handles the process of getting the rates and return back to the presentation. After the request comes, it get the rates and we have a small interfering with the result that changes the rates for fantasy demonstration in the client side. After the rate values is returned, they are returned back by the specified ViewModel collection.
In this layer we have also the descriptions of Mapping Objects, for example we have the Mapping Configuration of the CurrencyExchange class of the Domain layer to RateViewModel that is used for the presentation layer. In this project I used Mapster to configure the Mappings.
There is also a ServicesSetup class for setting the dependencies in the Services Container related to Application Layer including: Mapping Configurations, Validations and MediatR

Infrastructure Layer
As mentioned previously, the abstraction layer in the Application project should be implemented and this responsibility is performed in the infrastructure layer for (SoC) Seperation of Concern.
We have 2 Interfaces that implemented here:
The 1st one is for Caching to improve the performance of returning rates by decreasing number of calls to external api, it uses InMemoryCache to perform this responsibility. 
The other one is CurrencyExchange that fetches the rates exactly from the external source using a HttpClient object and tracing the Json result. The configuration of this action including the External Api Address id injected from appsetting.json to this class.
There is also a ServicesSetup class for setting the dependencies in the Services Container related to the Infrastructure Layer.

We have another project for testing this project as a sample UnitTest, I have implemented a Unit Test for the CurrencyExchange class by XUnit library.

Domain Layer
In the Domain project I have defined the Entities which may are used in all the other projects and layers.

Resources
The resources project is another project to integrate the messages and could be used for the Cultures and Multi Languages capabilities.

After running the back-end project as you can see there are 2 different environment is running, one is for the microservice Api project by the address http://localhost:5052 and the other one is the api gateway project that is visible in this Console by the address http://localhost:5157/gateway


