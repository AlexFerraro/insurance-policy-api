FROM postgres:15

COPY script.sql /docker-entrypoint-initdb.d/

ENV POSTGRES_USER=postgres \
    POSTGRES_PASSWORD=postgres \
    POSTGRES_DB=apolicy_db

EXPOSE 5432

CMD ["postgres"]