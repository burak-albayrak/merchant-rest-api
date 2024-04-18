# Merchant API

The Merchant API is a RESTful web service designed to manage information for merchants engaged in commerce. Built with ASP.NET Core and MongoDB, this API offers a robust platform for merchant data management.


## Features

### Merchant Operations

The API supports CRUD (Create, Read, Update, Delete) operations for managing merchant information. This encompasses creating a new merchant, updating existing merchant details, retrieving merchant information by ID, and deleting a merchant from the database.

### Filtering

The API allows merchants to be filtered based on various attributes such as city and star rating. Filtering facilitates targeted searches and provides flexibility in fetching specific merchant data.

### Sorting

Sorting capabilities are implemented to organize merchants based on a chosen field. Merchants can be sorted in ascending or descending order based on attributes like name, review star rating, or review count.

### Pagination

Pagination is employed when listing merchants to manage large datasets effectively. This feature limits the number of records displayed per page, enhancing performance and user experience.

### Error Handling

Custom error handling is integrated into the API to provide meaningful error messages and appropriate HTTP status codes for various scenarios. This ensures a robust and user-friendly error management system.

### Logging

Logging plays a crucial role in tracking application activities, debugging, and monitoring. The API utilizes ASP.NET Core's built-in logging mechanism to maintain logs of various operations and errors.

## Usage

### Creating a Merchant

#### Endpoint

`POST /api/merchants`

#### Request Body

```json
{
    "name": "ABC Store",
    "address": {
        "city": "Istanbul",
        "cityCode": 34
    },
    "reviewStar": 4.5,
    "reviewCount": 100
}
```

#### Response

Upon successful creation, the API returns the newly created merchant object with a generated ID.

### Listing Merchants

#### Endpoint

`GET /api/merchants`

#### Parameters

- `page`: Page number
- `pageSize`: Number of records per page
- `city`: City name
- `reviewStarRange`: Star rating range (e.g., `3,5`)
- `sortBy`: Field to sort by
- `sortOrder`: Sorting order (`asc` or `desc`)

#### Response

The API returns a paginated list of merchants based on the provided filters and sorting criteria.

### Updating a Merchant

#### Endpoint

`PUT /api/merchants/{id}`

#### Request Body

```json
{
    "name": "New ABC Store Name",
    "address": {
        "city": "Istanbul",
        "cityCode": 34
    },
    "reviewStar": 4.7,
    "reviewCount": 110
}
```

#### Response

Upon successful update, the API returns the updated merchant object.

### Deleting a Merchant

#### Endpoint

`DELETE /api/merchants/{id}`

#### Response

Upon successful deletion, the API returns a success message indicating the deletion was successful.

## Error Handling

### Custom Errors

Custom error classes are provided to handle specific HTTP status codes and scenarios:

- `400 Bad Request`: Indicates an invalid request format or missing parameters.
- `404 Not Found`: Indicates that the requested resource (e.g., merchant) was not found.

### Error Handling Middleware

The API includes an ErrorHandlingMiddleware to catch and handle exceptions and custom errors. This middleware ensures consistent error responses and logs detailed error information for debugging.

## Logging

### Logging Levels

Different logging levels are utilized to categorize log messages based on their severity:

- `Information`: General information messages
- `Warning`: Warning messages that do not halt the application but may require attention
- `Error`: Error messages indicating issues that need immediate attention

### Log File Structure

Log files are stored in the `Logs` directory and contain the following information:

- Timestamp
- Log level
- Message details, including method names, parameter values, and exception details

## Configuration

### AppSettings

Application settings are defined in `appsettings.json` and `appsettings.Development.json`. These files contain configurations for:

- Logging levels
- Allowed hosts
- CORS policies

### MongoDB Settings

MongoDB connection settings are configured under `MongoDBSettings` in `appsettings.json`. This includes:

- Connection string
- Database name
- Collection name for merchants
