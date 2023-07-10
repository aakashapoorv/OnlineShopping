# 🚀 Online Shopping System API

An ASP.NET Core REST API for an online shopping system. The API allows for the management of products, carts, orders and sending transaction emails to users at the end of the month.

## Features

1. **Manage Products**: Allows users to get a list of products.
2. **Manage Carts**: Allows users to add a product to the shopping cart.
3. **Manage Orders**: Allows users to place an order.
4. **User Accounts**: Supports management of user details.
5. **Transaction Records**: Stores a history of all user transactions.
6. **Email Notifications**: Sends users monthly email notifications related to their transactions.

## Technologies Used

- ASP.NET Core 7.0
- MongoDB
- SendGrid
- Docker
- iTextSharp (for PDF generation)

## Installation

1. Clone the repository
2. Navigate to the project directory
3. Build and run the application
4. Optionally, build the Docker image and run the application within a Docker container

## Running the application

### Without Docker

From the project directory, run the following commands:

```bash
dotnet build
dotnet run
```

Make sure to set env variables `MONGODB_CONNECTION_STRING` and `SENDGRID_API_KEY`

### With Docker

To run the application within a Docker container, first build the Docker image:

```bash
docker build -t online-shopping-system-api .
```

Then run the Docker container:

```bash
docker run -p 80:80 -e MONGODB_CONNECTION_STRING=your_connection_string -e SENDGRID_API_KEY=your_api_key online-shopping-system-api
```

## API Endpoints

The API consists of the following endpoints:

- `GET /api/v1/carts`
- `POST /api/v1/carts`
- `POST /api/v1/carts/addproduct/{cartId}`
- `GET /api/v1/orders`
- `POST /api/v1/orders`
- `GET /api/v1/products`
- `GET /api/v1/users`

The Ccontroller, endpoints and method table:

|Controller (Tag)| Endpoint   | Method |
|-------------|------------|--------|
|Cart         | /api/v1/carts | GET    |
|Cart         | /api/v1/carts | POST   |
|Cart         | /api/v1/carts/addproduct/{cartId} | POST   |
|Order        | /api/v1/orders | GET    |
|Order        | /api/v1/orders | POST   |
|Product      | /api/v1/products | GET   |
|User         | /api/v1/users | GET    |


**Transaction Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| Id | string | The unique identifier for the transaction, represented as an ObjectId in MongoDB |
| UserId | string | The Id of the user who made the transaction |
| UserEmail | string | The email of the user who made the transaction |
| OrderId | string | The Id of the order related to the transaction |
| User | User | The User object representing the user who made the transaction |
| Order | Order | The Order object related to the transaction |
| Cart | Cart | The Cart object related to the transaction |
| Products | List of Product | A list of Product objects that were part of the transaction |
| CreatedOn | DateTime | The date and time when the transaction was created |

The "Cart" model is used to record complete transactions, including user details, the related order, and the products purchased. It also includes a timestamp (CreatedOn) indicating when the transaction occurred.

**Cart Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| Id | string | The unique identifier for the cart, represented as an ObjectId in MongoDB |
| Products | List of CartItem | A list of CartItem objects that are in the cart |
| UserId | string | The Id of the user who owns the cart |

**CartItem Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| ProductId | string | The Id of the product in the cart |
| Quantity | int | The quantity of the product in the cart |


The "Cart" model is be used to manage a user's shopping cart, which includes a list of "CartItem" objects representing individual items in the cart, each with their product Id and quantity.

**Order Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| Id | string | The unique identifier for the order, represented as an ObjectId in MongoDB |
| CartId | string | The Id of the cart which the order is based on |
| PaymentMethod | string | The method of payment used for the order |
| UserId | string | The Id of the user who made the order |

This "Order" model is used to manage and track orders made by users. Each order is associated with a particular user, a shopping cart, and a chosen payment method.

**Product Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| Id | string | The unique identifier for the product, represented as an ObjectId in MongoDB |
| Name | string | The name of the product |
| Price | decimal | The price of the product |
| Description | string | A brief description of the product |

This "Product" model is used to manage and track all the products available in the online shopping system. Each product has a unique identifier, a name, a price, and a description.

**User Model**

| Field Name | Field Type | Description |
| --- | --- | --- |
| Id | string | The unique identifier for the user, represented as an ObjectId in MongoDB |
| Username | string | The user's chosen username |
| Email | string | The user's email address |
| Password | string | The user's password (note: in a real-world application, this should be hashed and salted for security) |
| FirstName | string | The user's first name |
| LastName | string | The user's last name |

This "User" model is used to manage and track all the users registered in the online shopping system. Each user has a unique identifier, a username, an email, a password, a first name, and a last name.

## MonthlyEmailService

A service runs monthly that sends an email to all users with a summary of their transactions of the past month. The email includes a PDF attachment with details of each transaction.

---

# REST API Documentation

OnlineShopping REST API.

## Base URL

The base URL for all API endpoints is: `http://localhost`

## API Paths

### Cart

#### Get Cart

Retrieves the cart information.

- Path: `/api/v1/carts`
- Method: `GET`
- Tags: `Cart`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `Cart` array

#### Create Cart

Creates a new cart.

- Path: `/api/v1/carts`
- Method: `POST`
- Tags: `Cart`
- Request Body:
  - Content Types:
    - `application/json`
    - `text/json`
    - `application/*+json`
  - Schema: `Cart`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `Cart`

#### Add Product to Cart

Adds a product to the cart.

- Path: `/api/v1/carts/addproduct/{cartId}`
- Method: `POST`
- Tags: `Cart`
- Parameters:
  - `cartId` (path parameter) - The ID of the cart.
    - Type: `string`
    - Required: true
    - Style: `simple`
- Request Body:
  - Content Types:
    - `application/json`
    - `text/json`
    - `application/*+json`
  - Schema: `string`
- Responses:
  - 200: Success

### Order

#### Get Orders

Retrieves the list of orders.

- Path: `/api/v1/orders`
- Method: `GET`
- Tags: `Order`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `Order` array

#### Create Order

Creates a new order.

- Path: `/api/v1/orders`
- Method: `POST`
- Tags: `Order`
- Request Body:
  - Content Types:
    - `application/json`
    - `text/json`
    - `application/*+json`
  - Schema: `Order`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `Order`

### Product

#### Get Products

Retrieves the list of products.

- Path: `/api/v1/products`
- Method: `GET`
- Tags: `Product`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `Product` array

### User

#### Get Users

Retrieves the list of users.

- Path: `/api/v1/users`
- Method: `GET`
- Tags: `User`
- Responses:
  - 200: Success
    - Content Types:
      - `text/plain`
      - `application/json`
      - `text/json`
    - Schema: `User` array

## Data Models

### Cart

- Properties:
  - `id`: string (nullable)
  - `products`: CartItem array (nullable)
  - `userId`: string (nullable)

### CartItem

- Properties:
  - `productId`: string (nullable)
  - `quantity`: integer (format: int32)

### Order

- Properties:
  - `id`: string (nullable)
  - `cartId`: string (nullable)
  - `paymentMethod`: string (nullable)
  - `userId`: string (nullable)

### Product

- Properties:
  - `id`: string (nullable)
  - `name`: string (nullable)
  - `price`: number (format: double)
  - `description`: string (nullable)

### User

- Properties:
  - `id`: string (nullable)
  - `username`: string (nullable)
  - `email`: string (nullable)
  - `password`: string (nullable)
  - `firstName`: string (nullable)
  - `lastName`: string (nullable)

## Version

API Version: 1.0

---

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, please feel free to submit a pull request or create an issue on the GitHub repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.