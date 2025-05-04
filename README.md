# TestResultsAPI

## Approach

- Built the API using .NET 8 with ASP.NET Core, exposing endpoints for importing test result XML docs and retrieving aggregated statistics per test ID.
- Implemented a custom input formatter (`MarkrXmlInputFormatter.cs`) to support the `text/xml+markr` content type and parse XML payloads into C# objects.
- Used Entity Framework Core with PostgreSQL for data persistence and schema management.
- Used the MathNet.Numerics library for calculating statistics.

## Assumptions

- Student names were not stored in the database. It was assumed that if access to student names is required, they can be retrieved via the student number using an external service.

## Points of Interest

- Entity Framework Core (EF Core) is used for Object Relational Mapping with PostgreSQL.
  - The database schema is managed using EF Core migrations, which are automatically applied at runtime to keep the database schema in sync with the models.
  - The DbContext handles access to test result records and enables efficient LINQ-based querying and filtering.
- All data is stored in a Dockerized Postgres container, allowing persistence across restarts. If this POC were to be shipped to prod, the PostgreSQL container could be replaced with a managed database service like Amazon RDS.

## Running the app

### Prerequisites
- Docker + Docker Compose

### Build + Run

1. Clone the repo
2. Start the app and database with `docker-compose up --build`. This will:
   - Build the .NET app and host it at `http://localhost:4567`
   - Spin up a PostgreSQL database 
   - Apply EF Core migrations automatically

### Send test requests

To import:
```
curl -X POST http://localhost:4567/import \
  -H 'Content-Type: text/xml+markr' \
  -d @- <<XML
<mcq-test-results>
    <mcq-test-result scanned-on="2017-12-04T12:12:10+11:00">
        <first-name>Jane</first-name>
        <last-name>Austen</last-name>
        <student-number>521585128</student-number>
        <test-id>1234</test-id>
        <summary-marks available="20" obtained="13" />
    </mcq-test-result>
</mcq-test-results>
XML
```
To query aggregate stats:

```
curl http://localhost:4567/results/1234/aggregate
```

## Run the tests

1. Run `dotnet test`
   - Note: for the Component Tests to pass, you'll need to have the app running (see [above](#build--run)).