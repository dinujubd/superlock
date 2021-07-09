# Super Locker
This repository contains a sample of lock functionality api

## Architectural Pattern
- **CQRS** with clean architecture
## Sample tests included
- Unit Test
- Integration Test
- Functional Test

## Development Environment
- Docker

## Databases
- MariaDB with Dapper
- Redis for cache

## Messaging
- RabbitMq + MassTransit

# Future Improvements
- Achieving at least 90% code coverage
- Adding more behavioral tests
- Logging and Dashboard setup Grafana + Loki
- Deployable Script for Octopus Deployment
- Cleaning up hard coded strings


# How to run?
- run `docker-compose up` in the directory of `docker`
- then run Api project

Swagger url will be https://localhost:5001/swagger

ERD:

<img width="327" alt="2021-07-10_002700" src="https://user-images.githubusercontent.com/2369887/125115538-a7c1f080-e115-11eb-8d9f-8d6a6126a9e7.png">


