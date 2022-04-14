set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE TABLE accountview(
        id varchar(200) not null PRIMARY KEY,
        policyid varchar(50) not null,
        benefitid varchar(50) null,
        sliceid varchar(50) null,
        accounttype varchar(50) not null,
        postingrulecodelastentry varchar(50) not null,
        versionlastentry integer not null,
        accounttransactionidlastentry varchar(50) not null,
        timestamplastentry timestamp not null,
        valuedatelastentry date not null,
        balance decimal not null
    );
EOSQL