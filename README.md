## The Gilded Rose Expands

### Description

*This is a variation of the original Gilded Rose Kata created by Terry Hughes and Bobby Johnson.  

As you may know, the Gilded Rose* is a small inn in a prominent city that buys and sells
only the finest items. The shopkeeper, Allison, is looking to expand by providing
merchants in other cities with access to the shop's inventory via a public HTTP-accessible API.  In this variation, we focus on providing these endpoints:

1. Retrieve the current inventory
2. Buy an item (user must be authenticated)

### Notes

This project has been built in .NET Core (1.1.1) using Visual Studios 2017 and JetBrains Rider (EAP).

There are secrets (e.g. keys/passwords) committed into this repo in order to provide a working example of this project.  Please ensure that if you build off of this project that you change these keys/passwords to suit your needs and best practice would be to NOT commit them into version control.

#### Usage

Restore Nuget packages and then either run the application through your IDE, or execute the test projects.  The running application is configured to use EF Core's In Memory DB, so the database will be empty at the start of each run.

#### Data Format

This sample project uses JSON for transmitting data to/from the API.  JSON provides a lightweight format, that is easy to read, easy to parse, and is the most common data-interchange format used by RESTful APIs.

###### GET

A sample GET request/response from the `api/items` endpoint:

```
Request:
curl -X GET \
  http://localhost:50148/api/items \
  -H 'cache-control: no-cache' \
  -H 'postman-token: 9a26e6d9-de77-52ff-059d-9a65f102c791'

Response:
[
  {
    "name": "Item1",
    "description": "Description1",
    "price": 9.99,
    "quantity": 10,
    "id": 1
  },
  {
    "name": "Item2",
    "description": "Description2",
    "price": 99.99,
    "quantity": 100,
    "id": 2
  }
]

Response Headers:
Content-Type →application/json; charset=utf-8
Date →Mon, 10 Apr 2017 03:47:42 GMT
Server →Kestrel
Transfer-Encoding →chunked
X-Powered-By →ASP.NET
X-SourceFiles →=?UTF-8?B?QzpcVXNlcnNcRGF2aWRcRGV2ZWxvcG1lbnRcR2lsZGVkUm9zZVxHaWxkZWRSb3NlXGFwaVxpdGVtcw==?=
```

###### POST

A sample POST request/response from the `api/transactions/order` endpoint:

```
Request:
curl -X POST \
  http://localhost:50148/api/transactions/order \
  -H 'authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6ImFkbWluQGRta2FydGlrLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJyb2xlcyI6WyJhZG1pbiIsInVzZXIiXSwiaXNzIjoiaHR0cHM6Ly9kYXZpZGthcnRpay5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NThlNDhkZTViNWZhZjgxODUyYjdhOTBjIiwiYXVkIjoiOWtlSzVvWXlGT3pjWUxFQnllTThDOXJTODhBQjl6OXAiLCJleHAiOjE0OTE4MzE3MDgsImlhdCI6MTQ5MTc5NTcwOH0.2icvatP2P_XrlSpuUbsemcP7gPIrapSWkLH6p_baAws' \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -H 'postman-token: fe4efc53-9b1e-4159-0c0e-3aca554ccb0e' \
  -d '[{
	"ItemId": 1,
	"Quantity": 5
},
{
	"ItemId": 2,
	"Quantity": 35
}]'

Response:
{
  "orderDate": "2017-04-09T20:55:09.5938296-07:00",
  "total": 3549.60,
  "userId": 1,
  "user": {
    "authId": "auth0|58e48de5b5faf81852b7a90c",
    "email": "admin@dmkartik.com",
    "id": 1
  },
  "details": [
    {
      "transactionId": 1,
      "itemId": 1,
      "quantity": 5,
      "price": 9.99,
      "subTotal": 49.95,
      "item": {
        "name": "Item1",
        "description": "Description1",
        "price": 9.99,
        "quantity": 5,
        "id": 1
      },
      "id": 1
    },
    {
      "transactionId": 1,
      "itemId": 2,
      "quantity": 35,
      "price": 99.99,
      "subTotal": 3499.65,
      "item": {
        "name": "Item2",
        "description": "Description2",
        "price": 99.99,
        "quantity": 65,
        "id": 2
      },
      "id": 2
    }
  ],
  "id": 1
}

Response Headers:
Access-Control-Allow-Origin →*
Content-Type →application/json; charset=utf-8
Date →Mon, 10 Apr 2017 03:55:09 GMT
Location →http://localhost:50148/api/transactions/1
Server →Kestrel
Transfer-Encoding →chunked
X-Powered-By →ASP.NET
X-SourceFiles →=?UTF-8?B?QzpcVXNlcnNcRGF2aWRcRGV2ZWxvcG1lbnRcR2lsZGVkUm9zZVxHaWxkZWRSb3NlXGFwaVx0cmFuc2FjdGlvbnNcb3JkZXI=?=
```

#### Design Decisions

- Used integers for primary identifiers for models to make this sample project a little easier to work with.  In a real world application, I'd opt for using 36-character GUID
- [Auth0](https://auth0.com) and JSON Web Tokens (JWT) were chosen to handle authentication for this project.  JWT provides a compact and self-contained way to handle authentication in this application.  They can be used to provide access to certain routes, services and resources, can be shared across multiple applications allowing for Single Sign On.  Tokens are fashioned with an expiry, and can also be revoked if needed.  Furthermore, using Auth0 as a 3rd party service provider allows me to spin up JWT authentication quickly, while allowing my end users to authenticate with my application using whatever platform they choose, as it interfaces with many of the most popular OAuth2 connections available (e.g. Facebook, Google, Twitter, GitHub, etc.)
- Added Quantity as a property for the Item object, in order to have a simple way to keep track of the number of items on hand for a given item.  This ensures that the shopkeeper wouldn't overextend the number of sold items.  This approach of adding a field to the item itself is a bit cumbersome and could be mitigated by the use of a separate inventory table that kept track of when items are added/removed from inventory.  This could also help in tracking restocking prices for items vs purchase prices to have a better idea of the amount of profit being made.
- ASP.Net Core and Entity Framework Core were chosen as I wanted to see how viable it is to be able to develop .NET C# applications using Linux.  I ended up still doing about 75% of the development from this project on Windows, but it is nice to see the advancements being made by Microsoft in the Open Source community.  I also wanted to have an opportunity to use EF Core's In Memory DB option to help streamline my integration tests.
- Added a couple of extra endpoints to be able to add items to the DB easier and have the ability to view created transactions.
- Lastly, logging is missing from this implementation, and would need to be added in the event this project is extended.