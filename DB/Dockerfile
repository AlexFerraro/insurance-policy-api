FROM postgres:latest

COPY script.sql /docker-entrypoint-initdb.d/

ENV POSTGRES_USER=postgres \
    POSTGRES_PASSWORD=postgres \
    POSTGRES_DB=policy_db

EXPOSE 5432

CMD ["postgres"]