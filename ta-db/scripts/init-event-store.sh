set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE TABLE events(
        index serial not null PRIMARY KEY,
        aggregateid varchar(200) not null,
        version integer not null,
        timestamp timestamp not null,
        body varchar(2000) not null,
        metadata varchar(2000),
        dispatched boolean not null default false
    );

    CREATE INDEX index_events_aggregateid ON events(aggregateid);
    CREATE INDEX index_events_aggregateid_version ON events(aggregateid, version);
    CREATE INDEX index_events_dispatched ON events(dispatched);
EOSQL