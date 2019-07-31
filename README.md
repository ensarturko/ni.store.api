## NiStore.Api

.NET Core 2.2 based micro API for key-value store.

### API Folder Structure

- Common
- Filters
- Controllers
- Services
- Data
- Models

### API Architecture

- Swagger endpoint: "URI/swagger"
- GZipping enabled
- HttpCompression enabled
- Standard JSON settings configured.
- Standard Paging objects added based on RESTful.
- Lightweight API middleware configured.
- Global exception handling & validation hangling configured.
- Logging support added on linux environment.

### TESTS Libraries

 - "Moq" for mocking.
 - FluentAssertions for easy assertion.
 - NUnit for easy unit testing.
 
### How to use?

- Install .net core 2.2.
- Clone the project.
- Run the project using cli or an IDE.(cli command: dotnet run)
- Go to /swagger or use documentation to explore the capabilities of API.
- Use a tool like Postman to make requests to API.

### What optimizations could have been done additionally?

- A caching mechanism like Redis before asking to DB.
- Prometheus and Grafana to see the metrics.
- Varnish to create some rules on HTTP requests.
- A NoSQL database.
- Authentication, Identity Server.
- Orchestrating using Kubernetes.
- More and more unit and integration tests.
- Etc.

### Short documentation

The short documentation can be viewed on this link.
https://docs.google.com/document/d/1HhQvD94T9T_u9DtI-lIWvLUSjb9dR7VBw6qgJJlykHc/edit?usp=sharing