#!/bin/bash

docker run -p 5433:5432 --env POSTGRES_USER=postgres --env POSTGRES_PASSWORD=postgres --env POSTGRES_DB=eshop postgres
